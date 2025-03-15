using Business_Layer.Interface;
using Business_Layer.Service;
using Microsoft.AspNetCore.Mvc;
using Model_Layer.DTO;
using Repository_Layer.Service;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAddressBookBL _addressBookBL;
    private readonly RedisCacheService redisCache;
    private readonly RabbitMQService rabbitMQService;

    public AuthController(IAddressBookBL _addressBookBL, RedisCacheService redisCache, RabbitMQService rabbitMQService)
    {
        this._addressBookBL = _addressBookBL;
        this.redisCache = redisCache;
        this.rabbitMQService= rabbitMQService;
    }
    /// <summary>
    /// This api is used to register a new user in the database
    /// </summary>
    /// <param name="userDto"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public IActionResult Register(UserDTO userDto)
    {
        var result = _addressBookBL.Register(userDto);
        if (result == "User already exists")
            return BadRequest(new { message = result });
        rabbitMQService.PublishMessage("user.registered", result);

        return Ok(new { message = result });
    }
    /// <summary>
    /// This api is used to login the registered user 
    /// </summary>
    /// <param name="userDto"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public IActionResult Login(UserDTO userDto)
    {
        var token = _addressBookBL.Login(userDto);
        if (token == null)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(new { token });
    }
    /// <summary>
    /// This api is used to send forgot password token to email
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("forgot-password")]
    public IActionResult ForgotPassword([FromBody] ForgotPasswordDTO request)
    {
        var result = _addressBookBL.ForgotPassword(request.Email);
        return result == "User not found" ? NotFound(new { message = result }) : Ok(new { message = result });
    }
    /// <summary>
    /// This api is used to reset the forgotten password
    /// </summary>
    /// <param name="resetPasswordDto"></param>
    /// <returns></returns>

    [HttpPost("reset-password")]
    public IActionResult ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
    {
        var result = _addressBookBL.ResetPassword(resetPasswordDto.Token, resetPasswordDto.NewPassword);
        return result == "Invalid or expired token" ? BadRequest(new { message = result }) : Ok(new { message = result });
    }
}
