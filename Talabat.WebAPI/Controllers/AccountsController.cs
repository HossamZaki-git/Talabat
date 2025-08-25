
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Talabat.Core.Domain_Models.Identity;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Repository.Identity;
using Talabat.WebAPI.DTOs;
using Talabat.WebAPI.Errors;
using Talabat.WebAPI.Extensions;
using Address = Talabat.Core.Domain_Models.Identity.Address;

namespace Talabat.WebAPI.Controllers
{
    public class AccountsController : BaseAPIController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ITokenProvider tokenProvider;
        private readonly IMapper mapper;

        public AccountsController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            ITokenProvider tokenProvider,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenProvider = tokenProvider;
            this.mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> LogIn(LoginDTO loginDTO)
        {
            var user = await userManager.FindByEmailAsync(loginDTO.Email);

            if (user is null)
                return BadRequest(new ErrorReturn(400));

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded)
                return Unauthorized(new ErrorReturn(401));

            string token = await tokenProvider.GenerateToken(user, userManager);

            return Ok(new UserDTO
            {
                Email = user.Email,
                DisplayName = user.DispalyName,
                Token = token
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            bool emailInUse = ( await IsInUse(registerDTO.Email) ).Value;
            if (emailInUse)
                return BadRequest(new ErrorReturn(400, "This email is already in use"));

            var user = new ApplicationUser
            {
                Email = registerDTO.Email,
                UserName = registerDTO.Email.Split("@")[0],
                PhoneNumber = registerDTO.Phone,
                DispalyName = registerDTO.DisplayName
            };

            var result = await userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
                return BadRequest(new ErrorReturn(400));

            string token = await tokenProvider.GenerateToken(user, userManager);

            return Ok(new UserDTO
            {
                DisplayName = user.DispalyName,
                Email = user.Email,
                Token = token
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await userManager.FindByEmailAsync( User.FindFirstValue(ClaimTypes.Email) );
            return Ok(new UserDTO
            {
                DisplayName = user.DispalyName,
                Email = user.Email,
                Token = await tokenProvider.GenerateToken(user,userManager)
            });
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDTO>> GetAddress()
            => mapper.Map<Address, AddressDTO>(userManager.FindUser_AddressInclusion(User).Address);

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDTO>> UpdateAddress(AddressDTO addressDto)
        {
            var newAddress = mapper.Map<AddressDTO, Address>(addressDto);
            var user = userManager.FindUser_AddressInclusion(User);
            if (user.Address is not null)
            {
                newAddress.ID = user.Address.ID;
                user.Address = newAddress;
            }
            else
                user.Address = newAddress;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
                return Ok(addressDto);

            return BadRequest(new ErrorReturn(400));
        }

        [HttpGet("IsInUseEmail")]
        public async Task<ActionResult<bool>> IsInUse([FromQuery] string email)
            => await userManager.FindByEmailAsync(email) is not null;
    }
}
