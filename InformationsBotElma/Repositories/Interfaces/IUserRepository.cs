using InformationsBotElma.Domain.Models;

namespace InformationsBotElma.Repositories.Interfaces;

public interface IUserRepository
{
    UserModel Add(UserModel user);
    IReadOnlyCollection<UserModel> GetAll();
    UserModel? GetById(long userId);
    UserModel? GetByUserName(string userName);
    bool Update(UserModel user);
    bool Delete(long userId);
}
