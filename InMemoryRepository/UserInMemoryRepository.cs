namespace InMemoryRepository;
using Entities;
using RepositoryContracts;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> users = new();

    public Task<User> AddAsync(User user)
    {
        user.UserId = users.Any()
            ? users.Max(p => p.UserId) + 1
            : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = users.SingleOrDefault(u =>u.UserId  == user.UserId);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.UserId}' not found");
        }

        users.Remove(existingUser);
        users.Add(user);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        User? userToRemove = users.SingleOrDefault(u =>u.UserId  == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }

        users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? User = users.SingleOrDefault(u => u.UserId == id);
        if (User is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }
        return Task.FromResult(User);
    }

    public IQueryable<User> GetManyAsync()
    {
        return users.AsQueryable();
    }
}