
using OnlineShoppingApp.Business.Types;
using OnlineShoppingApp.Data.Entities;
using OnlineShoppingApp.Data.Enums;
using OnlineShoppingApp.Data.Repositories;
using OnlineShoppingApp.Data.UnitOfWork;
using OnlineShoppingApp.Business.DataProtection;
using System;
using System.Threading.Tasks;
using System.Linq;
using OnlineShoppingApp.Business.Operations.User.Dtos;
using OnlineShoppingApp.Business.Operations.User;

namespace OnlineShoppingApp.Business.Services
{
    public class UserManager : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IDataProtection _protector;

        public UserManager(IRepository<UserEntity> userRepository, IUnitOfWork unitOfWork, IDataProtection protector)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _protector = protector;
        }

        public async Task<ServiceMessage> RegisterUser(RegisterUserDto user)
        {
            var hasEmail = _userRepository.GetAll(x => x.Email.ToLower() == user.Email.ToLower());
            if (hasEmail.Any())
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Email adresi zaten mevcut."
                };
            }

            var userEntity = new UserEntity
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = _protector.Protect(user.Password),
                BirthDate = user.BirthDate,
                PhoneNumber = user.PhoneNumber,
                Role = Role.Customer
            };

            _userRepository.Add(userEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Kullanıcı kaydı sırasında bir hata oluştu.");
            }

            return new ServiceMessage
            {
                IsSucceed = true
            };
        }

        public ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user)
        {
            var userEntity = _userRepository.Get(x => x.Email.ToLower() == user.Email.ToLower());
            if (userEntity == null)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Kullanıcı adı veya şifre hatalı."
                };
            }

            var unprotectedPassword = _protector.Unprotect(userEntity.Password);
            if (unprotectedPassword == user.Password)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = true,
                    Data = new UserInfoDto
                    {
                        Id = userEntity.Id,
                        Email = userEntity.Email,
                        FirstName = userEntity.FirstName,
                        LastName = userEntity.LastName,
                       BirthDate = userEntity.BirthDate,
                       Role = userEntity.Role,
                       PhoneNumber = userEntity.PhoneNumber,
                       
                       
                    }
                };
            }

            return new ServiceMessage<UserInfoDto>
            {
                IsSucceed = false,
                Message = "Kullanıcı adı veya şifre hatalı."
            };
        }
    }
}
