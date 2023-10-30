using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class UserDB : BaseDB
    {
        protected override BaseEntity NewEntity()
        {
            return new User();
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            User user = entity as User;
            user.ID = int.Parse(reader["id"].ToString());
            user.FirstName = reader["firstname"].ToString(); 
            user.LastName = reader["lastname"].ToString(); 
            user.UserName = reader["userName"].ToString();
            user.PassWord = reader["Password"].ToString();
            user.Gender = reader["gender"].Equals(true); // true -> male
            user.BIRTHDATE = DateTime.Parse(reader["birthDate"].ToString());
            user.EMAIL = reader["Email"].ToString();

            return user;
        }

        //שאילתה המחזירה את כלל המשתמשים
        public UserList SelectAll()
        {
            command.CommandText = "SELECT * FROM tblUser";
            UserList users = new UserList(ExecuteCommand());
            return users;
        }

        //שאילתה המחזירה משתמש לפי מאפיין (id)
        public User SelectByID(int id)
        {
            command.CommandText = "SELECT * FROM tblUser WHERE id=" + id.ToString() + ";";
            UserList users = new UserList(ExecuteCommand());
            if(users.Count() > 0)
            {
                return users[0];
            }
            return null;
        }

        public bool Login(User user)
        {
            command.CommandText = "SELECT * FROM tblUser Where userName ='"+ user.UserName + "';";
            UserList users = new UserList(ExecuteCommand());
            if(users.Count() > 0 && users[0].PassWord.Equals(user.PassWord))
            {
                return true;
            }
            return false;
        }
    }
}
