namespace LibraryManagementSystem_API.Models
{
    public class User
    {
        public int userID
        { get; set; }
        public string userName
        { get; set; }
        public string userPassword
        { get; set; }
        public string passwordHash
        { get; set; }
    }
}
