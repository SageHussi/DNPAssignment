using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI;   // use the same namespace as your other controllers

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public AuthController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost("login")]
    public ActionResult<UserDto> Login([FromBody] LoginRequest request)
    {
        User? user = userRepo.GetManyAsync()
            .FirstOrDefault(u => u.Username == request.Username);

        if (user == null || user.Password != request.Password)
        {
            return Unauthorized("Wrong username or password");
        }

        UserDto dto = new() { Id = user.UserId, Username = user.Username };
        return Ok(dto);
    }
}