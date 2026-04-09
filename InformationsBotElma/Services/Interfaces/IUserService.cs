using InformationsBotElma.Domain.Models;
using DomainUserRole = InformationsBotElma.Domain.Enums.UserRole;

namespace InformationsBotElma.Services.Interfaces;

public interface IUserService
{
    UserModel Create(string userName, DomainUserRole role);
    IReadOnlyCollection<UserModel> GetAll();
    UserModel? GetById(long userId);
    UserModel? GetByUserName(string userName);
    bool Update(long userId, string newUserName, DomainUserRole role);
    bool Delete(long userId);
}
