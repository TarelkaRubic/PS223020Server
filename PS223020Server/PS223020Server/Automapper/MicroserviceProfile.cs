using AutoMapper;
using PS223020Server.BusinessLogic.Core.Models;
using PS223020Server.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PS223020Server.Automapper
{
    public class MicroserviceProfile : Profile
    {
        public MicroserviceProfile()
        {
            CreateMap<UserInformationBlo, UserInformationDto>();
            CreateMap<UserUpdateBlo, UserUpdateDto>();

        }
    }
}
