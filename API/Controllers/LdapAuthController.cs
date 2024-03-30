using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BookingApp.DTO;
using BookingApp.DTO.auth;
using BookingApp.Helpers;
using BookingApp.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingApp.Controllers
{
    public class LdapAuthController : ApiControllerBase
    {
        private readonly ILdapService _ldapService;

        public LdapAuthController(ILdapService ldapService)
        {
          _ldapService = ldapService;
        }
       

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] UserForLoginDto model)
        {
            return StatusCodeResult(await _ldapService.LoginAsync(model));
        }

    }

}
