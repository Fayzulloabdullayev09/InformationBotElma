using InformationsBotElma.Domain.Models;
using InformationsBotElma.Services.Interfaces;
using DomainUserRole = InformationsBotElma.Domain.Enums.UserRole;

namespace InformationsBotElma.Handlers;

public class UserHandler
{
    private readonly IUserService _userService;

    public UserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public UserModel Register(string userName, DomainUserRole role)
    {
        return _userService.Create(userName, role);
    }

    public IReadOnlyCollection<UserModel> GetAllUsers()
    {
        return _userService.GetAll();
    }

    public UserModel? GetByUserName(string userName)
    {
        return _userService.GetByUserName(userName);
    }

    public bool Rename(long userId, string newUserName, DomainUserRole role)
    {
        return _userService.Update(userId, newUserName, role);
    }

    public bool Remove(long userId)
    {
        return _userService.Delete(userId);
    }
}
