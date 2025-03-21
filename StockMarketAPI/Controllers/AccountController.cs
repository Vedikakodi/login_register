using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BCrypt.Net;
using DataStore.Abstraction.Models;
using FeatureObjects.Abstraction.DTOs;
using FeatureObjects.Infrastructure.Services;
using DataStore.Abstraction.Repositories;
using System.Threading.Tasks;
using FeatureObjects.Abstraction.DTO;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly JwtTokenService _jwtTokenService;

    public AccountController(IUserRepository userRepository, JwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto userDto)
    {
        var existingUser = await _userRepository.GetUserByEmail(userDto.Email);
        if (existingUser != null)
            return BadRequest(new { message = "Email already exists!" });

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        var newUser = new User
        {
            Name = userDto.Name,
            Email = userDto.Email,
            PasswordHash = hashedPassword
        };

        await _userRepository.AddUser(newUser);
        return Ok(new { message = "Registration successful!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userRepository.GetUserByEmail(loginDto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid credentials" });

        // Generate JWT Token
        var token = _jwtTokenService.GenerateToken(user);
        return Ok(new { Token = token });
    }

    [HttpGet("profile")]
    [Authorize] // Protect this route with JWT
    public IActionResult GetProfile()
    {
        var userId = User.FindFirst("id")?.Value;
        return Ok(new { message = "Protected Profile Data", userId });
    }
}
