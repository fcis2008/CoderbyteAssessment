using BusinessLogicLayer.DTO;

namespace BusinessLogicLayer.Service.Contract
{
    public interface IUserService
    {
        Task<dynamic> GetUserByID(string id);
        Task IsEmailAlreadyExistAsync(string email);
        Task<CreateUserResponseDTO> AddNewUserAsync(NewUserDTO userDTO);
    }
}
