using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Guess:BaseEntity
    {
        protected User user;
        protected Game Game;
        protected Group TeamGuessed;
        protected bool isDraw;
        protected bool isCorrect;

        public User USER { get { return this.user; } set { this.user = value; } }
        public Game GAME { get { return this.Game; } set { this.Game = value; } }
        public Group TEAMGUESSED { get { return this.TeamGuessed; } set { this.TeamGuessed = value; } }
        public bool ISDRAW { get { return this.isDraw; } set { this.isDraw = value; } }       
        public bool ISCORRECT { get { return this.isCorrect; } set { this.isCorrect = value; } }       
    }

    public class GuessList : List<Guess>
    {
        //בנאי ברירת מחדל - אוסף ריק
        public GuessList() { }
        //המרה אוסף גנרי לרשימת משתמשים
        public GuessList(IEnumerable<Guess> list)
            : base(list) { }
        //המרה מטה מטיפוס בסיס לרשימת משתמשים
        public GuessList(IEnumerable<BaseEntity> list)
            : base(list.Cast<Guess>().ToList()) { }
    }
}
