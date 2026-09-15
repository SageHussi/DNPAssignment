namespace CLI.UI.ManageUsers;
using Entities;
using RepositoryContracts;

public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task createUserAsync()
    {
        Console.WriteLine("Enter username:");
        string username = Console.ReadLine();
        
        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();
        
        User newUser = new User{Username = username, Password = password };
        User createdUser = await userRepository.AddAsync(newUser);
        
        Console.WriteLine("User added successfully: " + createdUser.UserId);
    }
}