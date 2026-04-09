using InformationsBotElma.Domain.Models;
using InformationsBotElma.Repositories.Interfaces;

namespace InformationsBotElma.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly Dictionary<long, UserModel> _users = new();

    public UserModel Add(UserModel user)
    {
        _users[user.UserId] = Copy(user);
        return Copy(user);
    }

    public IReadOnlyCollection<UserModel> GetAll()
    {
        return _users.Values.OrderBy(user => user.UserId).Select(Copy).ToArray();
    }

    public UserModel? GetById(long userId)
    {
        return _users.TryGetValue(userId, out var user) ? Copy(user) : null;
    }

    public UserModel? GetByUserName(string userName)
    {
        var user = _users.Values.FirstOrDefault(x =>
            string.Equals(x.UserName, userName, StringComparison.OrdinalIgnoreCase));

        return user is null ? null : Copy(user);
    }

    public bool Update(UserModel user)
    {
        if (!_users.ContainsKey(user.UserId))
        {
            return false;
        }

        _users[user.UserId] = Copy(user);
        return true;
    }

    public bool Delete(long userId)
    {
        return _users.Remove(userId);
    }

    private static UserModel Copy(UserModel user)
    {
        return new UserModel
        {
            UserId = user.UserId,
            UserName = user.UserName,
            Role = user.Role,
            RegistrationDateTime = user.RegistrationDateTime
        };
    }
}
