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
            return new User() as BaseEntity;
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
            user.ISADMIN = reader["IsAdmin"].Equals(true);

            int groupId = int.Parse(reader["FavoriteGroupID"].ToString());

            GroupDB groupDB = new GroupDB();
            user.FAVORITEGROUP = groupDB.SelectByID(groupId);

            return user;
        }

        protected override void LoadParameters(BaseEntity entity)
        {
            User user = entity as User;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@firstname", user.FirstName);
            command.Parameters.AddWithValue("@lastname", user.LastName);
            command.Parameters.AddWithValue("@userName", user.UserName);
            command.Parameters.AddWithValue("@Password", user.PassWord);
            command.Parameters.AddWithValue("@gender", user.Gender);
            command.Parameters.AddWithValue("@birthDate", user.BIRTHDATE.ToShortDateString());
            command.Parameters.AddWithValue("@Email", user.EMAIL);
            command.Parameters.AddWithValue("@IsAdmin", user.ISADMIN);
            command.Parameters.AddWithValue("@FavoriteGroupID", user.FAVORITEGROUP.ID);
            command.Parameters.AddWithValue("@id", user.ID);
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



        public int Insert(User user)
        {
            command.CommandText = "INSERT INTO TblUser " +
                "(firstname, lastname, userName, [Password], gender, birthDate, " +
                "Email, IsAdmin, FavoriteGroupID) VALUES (@firstname, @lastname, @userName, " +
                "@Password, @gender, @birthDate, @Email, @isAdmin, @FavoriteGroupID)";
            LoadParameters(user);
            return ExecuteCRUD();
        }
        public int Update(User user)
        {
            command.CommandText = "UPDATE TblUser SET username = @username firstname = @firstname, lastname = @lastname, " +
                "userName = @userName, Password = @Password, gender = @gender, birthDate = @birthDate, Email = @Email, " +
                "isAdmin = @isAdmin, FavoriteGroup = @FavoriteGroup WHERE id = @id";
            LoadParameters(user);
            return ExecuteCRUD();
        }
        public int Delete(User user)
        {
            command.CommandText = "DELETE FROM TblUser WHERE ID =@id";
            LoadParameters(user);
            return ExecuteCRUD();
        }

        public User Login(User user)
        {
            command.CommandText = $"SELECT * FROM TblUser WHERE (UserName = '{user.UserName}') " +
                $"AND ([Password] = '{user.PassWord}')";
            UserList list = new UserList(base.ExecuteCommand());
            if (list.Count == 1)
                return list[0];
            return null;
        }

    }
}
