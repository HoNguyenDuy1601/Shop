namespace Shop.Areas.Admin.Models
{
    public class UserLoginModel
    {
        public string username;
        public string password;
        public int? Id;

        public UserLoginModel(string username, string password, int? idAccount)
        {
            this.username = username;
            this.password = password;
            this.Id = idAccount; 
        }
    }
}
