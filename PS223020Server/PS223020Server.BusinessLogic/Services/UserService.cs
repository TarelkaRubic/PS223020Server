using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PS223020Server.BusinessLogic.Core.Interfaces;
using PS223020Server.BusinessLogic.Core.Models;
using PS223020Server.DataAccess.Core.Interfaces.DbContext;
using PS223020Server.DataAccess.Core.Models;
using Share.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS223020Server.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IRubicContext _context;

        public UserService(IMapper mapper, IRubicContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<UserInformationBlo> AuthWithEmail(string email, string password)
        {
            UserRto user = await _context.Users.FirstOrDefaultAsync(p => p.Email == email && p.Password == password);

            if (user == null) throw new NotFoundException($"Пользователь с почтой {email} не найден");

            return await ConvertToUserInformationAsync(user);
        }

        public async Task<UserInformationBlo> AuthWithLogin(string login, string password)
        {
            UserRto user = await _context.Users.FirstOrDefaultAsync(p => p.Login == login && p.Password == password);

            if (user == null) throw new NotFoundException($"Пользователь с логином {login} не найден");

            return await ConvertToUserInformationAsync(user);
        }

        public async Task<UserInformationBlo> AuthWithPhone(string numberPrefix, string number, string password)
        {
            UserRto user = await _context.Users.FirstOrDefaultAsync(p => p.PhoneNumberPrefix == numberPrefix && p.PhoneNumber == number && p.Password == password);

            if (user == null) throw new NotFoundException($"Пользователь с телефоном {numberPrefix}{number} не найден");

            return await ConvertToUserInformationAsync(user);
        }

        public async Task<bool> DoesExist(string numberPrefix, string number)
        {
            bool result = await _context.Users.AnyAsync(y => y.PhoneNumber == number && y.PhoneNumberPrefix == numberPrefix);
            return result;
        }

        public async Task<UserInformationBlo> Get(int userId)
        {
            UserRto user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) throw new NotFoundException("Пользователь не найден");

            return await ConvertToUserInformationAsync(user);
        }

        public async Task<UserInformationBlo> RegisterWithPhone(string numberPrefix, string number, string password)
        {
            bool result = await _context.Users.AnyAsync(y => y.PhoneNumber == number && y.PhoneNumberPrefix == numberPrefix);
            if (result == true) throw new BadRequestException("Такой пользователь уже есть");

            UserRto user = new UserRto()
            {
                Password = password,
                PhoneNumber = number,
                PhoneNumberPrefix = numberPrefix
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            UserInformationBlo userInfoBlo = await ConvertToUserInformationAsync(user);
            return userInfoBlo;
        }

        public async Task<UserInformationBlo> Update(UserUpdateBlo userUpdateBlo)
        {
            UserRto user = await _context.Users.FirstOrDefaultAsync(y => y.PhoneNumber == userUpdateBlo.CurrentPhoneNumber && y.PhoneNumberPrefix == userUpdateBlo.CurrentNumberPrefix && y.Password == userUpdateBlo.CurrentNumberPassword);

            if (user == null) throw new NotFoundException("Токого пользователя нет");

            user.Password = userUpdateBlo.Password;
            user.Email = userUpdateBlo.Email;
            user.Login = userUpdateBlo.Login;
            user.IsBoy = userUpdateBlo.IsBoy;
            user.PhoneNumberPrefix = userUpdateBlo.PhoneNumberPrefix;
            user.PhoneNumber = userUpdateBlo.PhoneNumber;
            user.FirstName = userUpdateBlo.FirstName;
            user.LastName = userUpdateBlo.LastName;
            user.Patronymic = userUpdateBlo.Patronymic;
            user.Birthday = userUpdateBlo.Birthday;
            user.AvatarUrl = userUpdateBlo.AvatarUrl;

            UserInformationBlo userInfoBlo = await ConvertToUserInformationAsync(user);
            return userInfoBlo;
        }

        private async Task<UserInformationBlo> ConvertToUserInformationAsync(UserRto userRto)
        {
            if (userRto == null) throw new ArgumentNullException(nameof(userRto));

            UserInformationBlo userInformationBlo = _mapper.Map<UserInformationBlo>(userRto);

            return userInformationBlo;
        }
    }
}
