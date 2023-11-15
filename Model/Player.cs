using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Player:BaseEntity
    {
        protected string firstname;
        protected string lastname;
        protected int number;
        protected Group playerGroup;
        protected bool isCaptain; 

        public string FirstName { get { return this.firstname; } set { this.firstname = value; } }
        public string LastName { get { return this.lastname; } set { this.lastname = value; } }
        public int Number { get { return this.number; } set { this.number = value; } }
        public bool IsCaptain { get { return this.isCaptain; } set { this.isCaptain = value; } }
        public Group PlayerGroup { get { return this.playerGroup; } set { this.playerGroup = value; } }

    }
    public class PlayerList : List<Player>
    {
        //בנאי ברירת מחדל - אוסף ריק
        public PlayerList() { }
        //המרה אוסף גנרי לרשימת משתמשים
        public PlayerList(IEnumerable<Player> list)
            : base(list) { }
        //המרה מטה מטיפוס בסיס לרשימת משתמשים
        public PlayerList(IEnumerable<BaseEntity> list)
            : base(list.Cast<Player>().ToList()) { }
    }

}
