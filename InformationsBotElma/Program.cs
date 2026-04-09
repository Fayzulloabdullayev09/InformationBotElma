using InformationsBotElma.Configuration;
using InformationsBotElma.Handlers;
using InformationsBotElma.Repositories;
using InformationsBotElma.Services;

var userRepository = new InMemoryUserRepository();
var taskRepository = new InMemoryTaskRepository();

var userService = new UserService(userRepository);
var taskService = new TaskService(taskRepository);

var userHandler = new UserHandler(userService);
var taskHandler = new TaskHandler(taskService);
var botHandler = new BotHandler(userHandler, taskHandler);

botHandler.SeedDemoData();

Console.WriteLine("InformationsBotElma ishga tushdi.");
Console.WriteLine();
Console.WriteLine($"Bot token config: {AppConfiguration.Bot.Token}");
Console.WriteLine($"Database connection config: {AppConfiguration.Database.ConnectionString}");
Console.WriteLine();

var adminUserName = AppConfiguration.Seed.AdminUserNames.First();
var regularUserName = AppConfiguration.Seed.RegularUserNames.First();

Console.WriteLine("Admin /start javobi:");
Console.WriteLine(botHandler.HandleMessage(1001, adminUserName, "/start"));
Console.WriteLine();

Console.WriteLine("Admin /adduser javobi:");
Console.WriteLine(botHandler.HandleMessage(1001, adminUserName, "/adduser"));
Console.WriteLine();

Console.WriteLine("Admin yangi username yubordi:");
Console.WriteLine(botHandler.HandleMessage(1001, adminUserName, "@new_member"));
Console.WriteLine();

Console.WriteLine("Admin /dashboard javobi:");
Console.WriteLine(botHandler.HandleMessage(1001, adminUserName, "/dashboard"));
Console.WriteLine();

Console.WriteLine("Oddiy user /start javobi:");
Console.WriteLine(botHandler.HandleMessage(2001, regularUserName, "/start"));
Console.WriteLine();

Console.WriteLine("Oddiy user /dashboard javobi:");
Console.WriteLine(botHandler.HandleMessage(2001, regularUserName, "/dashboard"));
Console.WriteLine();

Console.WriteLine("Chiqish uchun Enter bosing...");
Console.ReadLine();
