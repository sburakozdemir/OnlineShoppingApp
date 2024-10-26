using OnlineShoppingApp.Business.Operations.User.Dtos;
using OnlineShoppingApp.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Business.Operations.User
{
    public interface IUserService
    {
        Task<ServiceMessage> RegisterUser(RegisterUserDto user);

        ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user);
    }
}
