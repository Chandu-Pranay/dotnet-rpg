using dotnet_rpg.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authrepo;
        public AuthController(IAuthRepository authrepo)
        {
            _authrepo = authrepo;    
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto request){
            ServiceResponse<int> response=await _authrepo.Register(
                new User{Username=request.username},request.password
            );
        if(!response.Success)
            return BadRequest(response);
        return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto request){
            ServiceResponse<string> response=await _authrepo.Login(
                request.username,request.password
            );
        if(!response.Success)
            return BadRequest(response);
        return Ok(response);
        }
    }
}