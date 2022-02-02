using System.Threading.Tasks;
using API.Extensions;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;


        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, ITokenService tokenService, 
            IMapper mapper, IConfiguration config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.DisplayName)) return BadRequest("This name is taken");

            if (await CheckEmailExistsAsync(registerDto.Email)) return BadRequest("This email is taken");

            var user = _mapper.Map<ApplicationUser>(registerDto);
            user.UserName = registerDto.DisplayName;

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest("Bad request!");

           /*  var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            
            var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

            string url = 
                    $"{_config["ApiAppUrl"]}/api/account/confirmemail?email={user.Email}&token={validEmailToken}";

            await _emailService.SendEmailAsync(user.Email, 
                "Confirm your email", $"<h1>Welcome to EDoctor</h1>" +
                $"<p>Please confirm your email by <a href='{url}'>Clicking here</a></p>"); */

            var roleResult = await _userManager.AddToRoleAsync(user, "Visitor");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateToken(user),
                Email = user.Email,
                UserId = user.Id
            };     
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {          
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return BadRequest("Bad request!");

          /*   if (!await _userManager.IsEmailConfirmedAsync(user)) 
                return Unauthorized(new ServerResponse(401, "Email is not confirmed")); */

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized();

            if (user.LockoutEnd != null) return Unauthorized();

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateToken(user),
                Email = user.Email,
                UserId = user.Id
            };
        }

        [Authorize]
        [HttpGet("getaddress")]
        public async Task<ActionResult<ShippingAddressDto1>> GetCustomerAddress()
        {
            var user = await _userManager.FindUserWithAddressByClaims(HttpContext.User);
      
            return _mapper.Map<ShippingAddressDto1>(user.Address);
        }

        [Authorize]
        [HttpPut("updateaddress")]
        public async Task<ActionResult<ShippingAddressDto1>> UpdateCustomerAddress(ShippingAddressDto1 address)
        {
            var user = await _userManager.FindUserWithAddressByClaims(HttpContext.User);

            user.Address = _mapper.Map<Address>(address);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return Ok(_mapper.Map<ShippingAddressDto1>(user.Address));

            return BadRequest("Problem updating the user");
        } 

       
        private async Task<bool> UserExists(string displayname)
        {
            return await _userManager.Users.AnyAsync(x => x.DisplayName == displayname.ToLower());
        }     

        private async Task<bool> CheckEmailExistsAsync(string email)
        {             
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}













