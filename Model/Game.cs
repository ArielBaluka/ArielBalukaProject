using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class Game: BaseEntity
    {
        protected DateTime date;
        protected int HomeScore;
        protected int AwayScore;
        protected Group HomeTeam;
        protected Group AwayTeam;

        [DataMember]
        public DateTime Date { get { return this.date; } set { this.date = value; } }
        [DataMember]
        public int HOMESCORE { get { return this.HomeScore; } set { this.HomeScore = value; } }
        [DataMember]
        public int AWAYSCORE { get { return this.AwayScore; } set { this.AwayScore = value; } }
        [DataMember]
        public Group HOMETEAM { get { return this.HomeTeam; } set { this.HomeTeam = value; } }
        [DataMember]
        public Group AWAYTEAM { get { return this.AwayTeam; } set { this.AwayTeam = value; }}
    }
    [CollectionDataContract]
    public class GameList : List<Game>
    {
        //בנאי ברירת מחדל - אוסף ריק
        public GameList() { }
        //המרה אוסף גנרי לרשימת משתמשים
        public GameList(IEnumerable<Game> list)
            : base(list) { }
        //המרה מטה מטיפוס בסיס לרשימת משתמשים
        public GameList(IEnumerable<BaseEntity> list)
            : base(list.Cast<Game>().ToList()) { }
    }
}
