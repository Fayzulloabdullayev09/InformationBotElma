using System.Text;
using DomainTaskStatus = InformationsBotElma.Domain.Enums.TaskStatus;
using DomainUserRole = InformationsBotElma.Domain.Enums.UserRole;
using InformationsBotElma.Configuration;
using InformationsBotElma.Domain.Models;

namespace InformationsBotElma.Handlers;

public class BotHandler
{
    private readonly UserHandler _userHandler;
    private readonly TaskHandler _taskHandler;
    private readonly Dictionary<long, BotConversationState> _chatStates = new();

    public BotHandler(UserHandler userHandler, TaskHandler taskHandler)
    {
        _userHandler = userHandler;
        _taskHandler = taskHandler;
    }

    public void SeedDemoData()
    {
        foreach (var adminUserName in AppConfiguration.Seed.AdminUserNames)
        {
            _userHandler.Register(adminUserName, DomainUserRole.Admin);
        }

        foreach (var regularUserName in AppConfiguration.Seed.RegularUserNames)
        {
            _userHandler.Register(regularUserName, DomainUserRole.User);
        }

        var admin = _userHandler.GetByUserName(AppConfiguration.Seed.AdminUserNames.First())!;
        var operatorUser = _userHandler.GetByUserName(AppConfiguration.Seed.RegularUserNames.First())!;

        var firstTask = _taskHandler.CreateTask(
            "Bot logikasini yig'ish",
            "Handler va service qatlamlarini bog'lash",
            DateTime.UtcNow.AddDays(2),
            admin.UserName);

        _taskHandler.CreateTask(
            "Tasklarni ko'rsatish",
            "Console ichida foydalanuvchi va vazifalar ro'yxatini chiqarish",
            DateTime.UtcNow.AddDays(4),
            operatorUser.UserName);

        _taskHandler.UpdateTask(
            firstTask.TaskId,
            "Bot logikasini yakunlash",
            "CRUD amallarini ishlaydigan ko'rinishga keltirish",
            DateTime.UtcNow.AddDays(3),
            DomainTaskStatus.InProgress,
            admin.UserName);
    }

    public string RenderDashboard()
    {
        var admin = _userHandler.GetByUserName(AppConfiguration.Seed.AdminUserNames.First());
        return admin is null
            ? "Admin foydalanuvchi topilmadi."
            : RenderDashboard(admin.UserId);
    }

    public string RenderDashboard(long currentUserId)
    {
        var currentUser = _userHandler.GetAllUsers().FirstOrDefault(user => user.UserId == currentUserId);
        if (currentUser is null)
        {
            return "Foydalanuvchi topilmadi.";
        }

        if (currentUser.Role != DomainUserRole.Admin)
        {
            return $"'{currentUser.UserName}' foydalanuvchisida dashboardni ko'rish uchun ruxsat yo'q.";
        }

        var builder = new StringBuilder();
        builder.AppendLine($"Dashboard owner: {currentUser.UserName} ({currentUser.Role})");
        builder.AppendLine();
        builder.AppendLine("Foydalanuvchilar:");

        foreach (var user in _userHandler.GetAllUsers())
        {
            builder.AppendLine($"{user.UserId}. {user.UserName} | role: {user.Role} | {user.RegistrationDateTime:u}");
        }

        builder.AppendLine();
        builder.AppendLine("Vazifalar:");

        foreach (var task in _taskHandler.GetAllTasks())
        {
            builder.AppendLine($"{task.TaskId}. {task.TaskName} | {task.TaskStatus} | deadline: {task.DeadLine:u}");
        }

        return builder.ToString();
    }

    public string HandleMessage(long chatId, string senderUserName, string messageText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(senderUserName);
        ArgumentException.ThrowIfNullOrWhiteSpace(messageText);

        var normalizedUserName = senderUserName.Trim();
        var normalizedMessage = messageText.Trim();
        var currentUser = EnsureUserExists(normalizedUserName);
        var state = GetState(chatId);

        if (normalizedMessage.Equals("/start", StringComparison.OrdinalIgnoreCase))
        {
            _chatStates[chatId] = BotConversationState.Idle;
            return BuildStartMessage(currentUser.UserName, currentUser.Role);
        }

        if (state == BotConversationState.WaitingForNewUserName)
        {
            _chatStates[chatId] = BotConversationState.Idle;
            return HandleNewUserNameInput(currentUser, normalizedMessage);
        }

        return normalizedMessage.ToLowerInvariant() switch
        {
            "/adduser" => BeginAddUserFlow(currentUser, chatId),
            "/dashboard" => RenderDashboard(currentUser.UserId),
            "/help" => BuildHelpMessage(),
            _ => "Buyruq tushunilmadi. /start yoki /help yuboring."
        };
    }

    private UserModel EnsureUserExists(string senderUserName)
    {
        var existingUser = _userHandler.GetByUserName(senderUserName);
        if (existingUser is not null)
        {
            return existingUser;
        }

        return _userHandler.Register(senderUserName, DomainUserRole.User);
    }

    private BotConversationState GetState(long chatId)
    {
        return _chatStates.TryGetValue(chatId, out var state)
            ? state
            : BotConversationState.Idle;
    }

    private string BeginAddUserFlow(UserModel currentUser, long chatId)
    {
        if (currentUser.Role != DomainUserRole.Admin)
        {
            return "Sizda foydalanuvchi qo'shish uchun ruxsat yo'q.";
        }

        _chatStates[chatId] = BotConversationState.WaitingForNewUserName;
        return "Yangi foydalanuvchi username yuboring. Misol: @new_user";
    }

    private string HandleNewUserNameInput(UserModel currentUser, string newUserName)
    {
        if (currentUser.Role != DomainUserRole.Admin)
        {
            return "Sizda foydalanuvchi qo'shish uchun ruxsat yo'q.";
        }

        if (_userHandler.GetByUserName(newUserName) is not null)
        {
            return $"'{newUserName}' allaqachon mavjud.";
        }

        var createdUser = _userHandler.Register(newUserName, DomainUserRole.User);
        return $"Yangi foydalanuvchi qo'shildi: {createdUser.UserName}. Role: {createdUser.Role}";
    }

    private static string BuildStartMessage(string userName, DomainUserRole role)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"Xush kelibsiz, {userName}.");
        builder.AppendLine($"Sizning role: {role}");
        builder.AppendLine();
        builder.AppendLine("Mavjud buyruqlar:");
        builder.AppendLine("/start - bot menyusini ko'rsatadi");
        builder.AppendLine("/help - buyruqlar ro'yxatini ko'rsatadi");
        builder.AppendLine("/dashboard - dashboardni ko'rsatadi");
        builder.AppendLine("/adduser - yangi foydalanuvchi qo'shish jarayonini boshlaydi");
        return builder.ToString();
    }

    private static string BuildHelpMessage()
    {
        return """
Buyruqlar:
/start - bot menyusini ochadi
/help - yordam xabarini ko'rsatadi
/dashboard - admin uchun dashboard
/adduser - admin yangi user qo'shishi uchun
""";
    }
}
