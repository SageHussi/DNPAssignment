using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        User user = new User { Username = request.Username, Password = request.Password };
        User created = await userRepo.AddAsync(user);
        UserDto dto = new UserDto { Id = created.UserId, Username = created.Username };
        return Created($"/users/{dto.Id}", dto);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingleUser(int id)
    {
        User user = await userRepo.GetSingleAsync(id);
        UserDto dto = new UserDto { Id = user.UserId, Username = user.Username };
        return Ok(dto);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        await userRepo.DeleteAsync(id);
        return NoContent();
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers([FromQuery] string? usernameContains)
    {
        IQueryable<User> users = userRepo.GetManyAsync();

        if (!string.IsNullOrWhiteSpace(usernameContains))
        {
            users = users.Where(u => u.Username.Contains(usernameContains));
        }

        List<UserDto> dtos = users
            .Select(u => new UserDto { Id = u.UserId, Username = u.Username })
            .ToList();

        return Ok(dtos);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateUser(int id, [FromBody] UpdateUserDto request)
    {
        User user = new User { UserId = id, Username = request.Username, Password = request.Password };
        await userRepo.UpdateAsync(user);
        return NoContent();
    }
}