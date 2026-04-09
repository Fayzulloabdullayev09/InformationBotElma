using InformationsBotElma.Domain.Models;
using InformationsBotElma.Repositories.Interfaces;
using InformationsBotElma.Services.Interfaces;
using DomainUserRole = InformationsBotElma.Domain.Enums.UserRole;

namespace InformationsBotElma.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private long _nextUserId = 1;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public UserModel Create(string userName, DomainUserRole role)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);

        var user = new UserModel
        {
            UserId = _nextUserId++,
            UserName = userName.Trim(),
            Role = role,
            RegistrationDateTime = DateTime.UtcNow
        };

        return _userRepository.Add(user);
    }

    public IReadOnlyCollection<UserModel> GetAll()
    {
        return _userRepository.GetAll();
    }

    public UserModel? GetById(long userId)
    {
        return _userRepository.GetById(userId);
    }

    public UserModel? GetByUserName(string userName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);
        return _userRepository.GetByUserName(userName.Trim());
    }

    public bool Update(long userId, string newUserName, DomainUserRole role)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newUserName);

        var user = _userRepository.GetById(userId);
        if (user is null)
        {
            return false;
        }

        user.UserName = newUserName.Trim();
        user.Role = role;
        return _userRepository.Update(user);
    }

    public bool Delete(long userId)
    {
        return _userRepository.Delete(userId);
    }
}
