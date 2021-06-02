using System;
using System.ComponentModel.DataAnnotations;

namespace EliteBuild.Account
{
    public class LoginModel
    {
        public String Name { get; set; }
        [Required]
        public String Phone { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }
    }

    public class UserModel
    {
        public UserModel(Models.DataBase.User User)
        {
            this.ID = User.ID.Value;
            this.Login = User.Login.Value;
            this.Role = User.Role.Value;
        }

        public Int32 ID { get; set; }
        public String Login { get; set; }
        public Int32 Role { get; set; }
    }
}