using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PS223020Server.BusinessLogic.Core.Interfaces;
using PS223020Server.BusinessLogic.Core.Models;
using PS223020Server.Core.Models;
using PS223020Server.DataAccess.Core.Interfaces.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PS223020Server.Controllers
{
    [Route("api/[controller]")]
    [Controller]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("registration")]
        public async Task<ActionResult<UserInformationDto>> Register(UserIdentityDto userIdentityDto)
        {
            UserInformationBlo userInformationBlo = await _userService.RegisterWithPhone(userIdentityDto.NumberPrefix, userIdentityDto.Number, userIdentityDto.Password);

            return await ConvertToUserInformationAsync(userInformationBlo);
        }
        [HttpPost("AuthWithPhone")]
        public async Task<ActionResult> AuthWithPhone(UserIdentityDto userIdentityDto)
        {
            try
            {
                await _userService.AuthWithPhone(userIdentityDto.NumberPrefix, userIdentityDto.Number, userIdentityDto.Password);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
            
        }
        [HttpPost("AuthWithEmail")]
        public async Task<ActionResult> AuthWithEmail(UserIdentityDto userIdentityDto)
        {
            try
            {
              await  _userService.AuthWithEmail(userIdentityDto.Email, userIdentityDto.Password);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
        [HttpPost("AuthWithLogin")]
        public async Task<ActionResult> AuthWithLogin(UserIdentityDto userIdentityDto)
        {
            try
            {
                await _userService.AuthWithLogin(userIdentityDto.Login, userIdentityDto.Password);
                  return Ok();
            }
            catch
            {
                 return NotFound();
            }
        }
        [HttpGet("Get/{userId}")]
        public async Task<ActionResult> Get(int userId)
        {
            try
            {
                await _userService.Get(userId);
                  return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
        [HttpPatch("Update")]
        public async Task<ActionResult> Update(UserUpdateDto userUpdateDto)
        {
            try
            {
                UserUpdateBlo userUpdateBlo =_mapper.Map<UserUpdateBlo>(userUpdateDto);
                await _userService.Update(userUpdateBlo);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
        [HttpPatch("DoesExist")]
        public async Task<ActionResult> DoesExist(UserIdentityDto userIdentityDto)
        {
            try
            {
               
                await _userService.DoesExist(userIdentityDto.NumberPrefix, userIdentityDto.Number);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }


        private async Task<UserInformationDto> ConvertToUserInformationAsync(UserInformationBlo userInformationBlo)
        {
            if (userInformationBlo == null)
            {
                throw new ArgumentNullException(nameof(userInformationBlo));
            }

            UserInformationDto userInformationDto = _mapper.Map<UserInformationDto>(userInformationBlo);

            return userInformationDto;
        }
    }
}
