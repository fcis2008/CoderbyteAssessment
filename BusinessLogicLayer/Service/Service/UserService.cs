using BusinessLogicLayer.AutoMapperConfig;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Service.Contract;
using DataAccessLayer.Model;
using DataAccessLayer.Repository.Contract;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLogicLayer.Service.Service
{
    public class UserService : ServiceBase, IUserService
    {
        public UserService(IRepositoryWrapper repoWrapper) : base(repoWrapper)
        {

        }

        public async Task<CreateUserResponseDTO> AddNewUserAsync(NewUserDTO newUserDTO)
        {
            await IsEmailAlreadyExistAsync(newUserDTO.Email);
            var user = OMapper.Mapper.Map<User>(newUserDTO);
            user.Id = Convert.ToBase64String(await GenerateSaltedHashAsyc(Guid.NewGuid().ToByteArray(), Encoding.ASCII.GetBytes("450d0b0db2bcf4adde5032eca1a7c416e560cf44")));
            _repoWrapper.User.Create(user);
            await _repoWrapper.Save();

            CreateUserResponseDTO createUserResponseDTO = new()
            {
                Id = user.Id
            };
            return createUserResponseDTO;
        }

        private async Task<byte[]> GenerateSaltedHashAsyc(byte[] plainText, byte[] salt)
        {
            using var sha1Algorithm = SHA1.Create();

            byte[] plainTextWithSaltBytes = new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
                plainTextWithSaltBytes[i] = plainText[i];

            for (int i = 0; i < salt.Length; i++)
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];

            return await sha1Algorithm.ComputeHashAsync(new MemoryStream(plainTextWithSaltBytes));
        }

        public async Task<dynamic> GetUserByID(string id)
        {
            var user = await _repoWrapper.User.FindByIdAsync(id);

            if (user == null)
                throw new InternalServiceException(System.Net.HttpStatusCode.NotFound, "User wasn't found");
            else
            {
                var userDTO = OMapper.Mapper.Map<UserDTO>(user);

                dynamic result = new ExpandoObject();

                result.FirstName = userDTO.FirstName;
                result.LastName = userDTO.LastName;

                //Omit the user "email" property if “marketingConsent” is false.
                if (userDTO.MarketingConsent)
                    result.Email = userDTO.Email;

                result.Id = userDTO.ID;

                return result;
            }
        }

        public async Task IsEmailAlreadyExistAsync(string email)
        {
            var result = await _repoWrapper.User.FindByCondition(u => u.Email == email).AnyAsync();
            if (result)
                throw new InternalServiceException(System.Net.HttpStatusCode.Conflict, "The email address you provided is already used by another user. Please change the email");
        }
    }
}
