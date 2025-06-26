using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem_API.Models
{
    public class User
    {
        public int userID
        { get; set; }
        public string userName
        { get; set; }
        public string passwordHash
        { get; set; }
        public string userRole
        { get; set; } = "User";//makes default role user
    }
}
