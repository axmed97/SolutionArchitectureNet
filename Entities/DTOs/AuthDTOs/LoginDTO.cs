namespace Entities.DTOs.AuthDTOs
{
    public class LoginDTO
    {
        public required string EmailOrUsername { get; set; }
        public required string Password { get; set; }
    }
}
