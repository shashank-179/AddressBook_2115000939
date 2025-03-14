using Business_Layer.Interface;
using Microsoft.AspNetCore.Mvc;
using Model_Layer.DTO;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAddressBookBL _addressBookBL;

    public AuthController(IAddressBookBL _addressBookBL)
    {
        this._addressBookBL = _addressBookBL;
    }

    [HttpPost("register")]
    public IActionResult Register(UserDTO userDto)
    {
        var result = _addressBookBL.Register(userDto);
        if (result == "User already exists")
            return BadRequest(new { message = result });

        return Ok(new { message = result });
    }

    [HttpPost("login")]
    public IActionResult Login(UserDTO userDto)
    {
        var token = _addressBookBL.Login(userDto);
        if (token == null)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(new { token });
    }
}
