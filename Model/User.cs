using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class User: BaseEntity
    {
        protected string firstname;
        protected string lastname;
        protected string userName;
        protected string password;
        protected DateTime birthDate;
        protected bool gender;
        protected string Email;
        protected bool isAdmin;
        protected Group favoriteGroup;

        protected GuessList guesses;

        public string FirstName { get { return this.firstname; } set { this.firstname = value; } }
        public string LastName { get { return this.lastname; } set { this.lastname = value; } }
        public string UserName { get { return this.userName; } set { this.userName = value; } }
        public string PassWord { get { return this.password; } set { this.password = value; } }
        public DateTime BIRTHDATE { get { return this.birthDate; } set { this.birthDate = value; } }
        public bool Gender { get { return this.gender; } set { this.gender = value; } }
        public string EMAIL { get { return this.Email; } set { this.Email = value; } }
        public bool ISADMIN { get { return this.isAdmin; } set { this.isAdmin = value; } }
        public Group FAVORITEGROUP { get { return this.favoriteGroup; } set { this.favoriteGroup = value; } }
        public GuessList Guesses { get { return this.guesses; } set { this.guesses = value; } }
    }
    public class UserList : List<User>
    {
        //בנאי ברירת מחדל - אוסף ריק
        public UserList() { }
        //המרה אוסף גנרי לרשימת משתמשים
        public UserList(IEnumerable<User> list)
            : base(list) { }
        //המרה מטה מטיפוס בסיס לרשימת משתמשים
        public UserList(IEnumerable<BaseEntity> list)
            : base(list.Cast<User>().ToList()) { }
    }
}
