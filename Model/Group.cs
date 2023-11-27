using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class Group: BaseEntity
    {
        protected string groupName;
        protected int points;
        protected string groupShortcut;

        [DataMember]
        public string GroupName { get { return this.groupName; } set { this.groupName = value; } }
        [DataMember]
        public int Points { get { return this.points; } set { this.points = value; } }
        [DataMember]
        public string GroupShortcut { get { return this.groupShortcut; } set { this.groupShortcut = value; } }
    }
    [CollectionDataContract]
    public class GroupList : List<Group>
    {
        //בנאי ברירת מחדל - אוסף ריק
        public GroupList() { }
        //המרה אוסף גנרי לרשימת משתמשים
        public GroupList(IEnumerable<Group> list)
            : base(list) { }
        //המרה מטה מטיפוס בסיס לרשימת משתמשים
        public GroupList(IEnumerable<BaseEntity> list)
            : base(list.Cast<Group>().ToList()) { }
    }

}
