using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class Guess:BaseEntity
    {
        protected User user;
        protected Game Game;
        protected Group TeamGuessed;
        protected bool isDraw;
        protected bool isCorrect;

        [DataMember]
        public User USER { get { return this.user; } set { this.user = value; } }
        [DataMember]
        public Game GAME { get { return this.Game; } set { this.Game = value; } }
        [DataMember]
        public Group TEAMGUESSED { get { return this.TeamGuessed; } set { this.TeamGuessed = value; } }
        [DataMember]
        public bool ISDRAW { get { return this.isDraw; } set { this.isDraw = value; } }
        [DataMember]
        public bool ISCORRECT { get { return this.isCorrect; } set { this.isCorrect = value; } }       
    }

    [CollectionDataContract]
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
