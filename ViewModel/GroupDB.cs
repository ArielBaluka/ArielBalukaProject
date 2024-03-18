using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class GroupDB: BaseDB
    {
        protected override BaseEntity NewEntity()
        {
            return new Group() as BaseEntity;
        }
        protected override void LoadParameters(BaseEntity entity)
        {
            Group group = entity as Group;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@GroupName", group.GroupName);
            command.Parameters.AddWithValue("@Points", group.Points);
            command.Parameters.AddWithValue("@GroupShortcut", group.GroupShortcut);
            command.Parameters.AddWithValue("@id", group.ID);
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Group group = entity as Group;
            group.ID = int.Parse(reader["id"].ToString());
            group.GroupName = reader["GroupName"].ToString();
            group.Points = int.Parse(reader["Points"].ToString());
            group.GroupShortcut = reader["GroupShortcut"].ToString();
            return group;
        }

        //שאילתה המחזירה את כלל הקבוצות
        public GroupList SelectAll()
        {
            command.CommandText = "SELECT * FROM tblGroup";
            GroupList groups = new GroupList(ExecuteCommand());
            return groups;
        }

        //שאילתה המחזירה את כלל הקבוצות
        public GroupList SelectAllByPoints()
        {
            command.CommandText = "SELECT * FROM tblGroup ORDER BY Points DESC";
            GroupList groups = new GroupList(ExecuteCommand());
            return groups;
        }

        //שאילתה המחזירה קבוצה לפי מאפיין (id)
        public Group SelectByID(int id)
        {
            command.CommandText = "SELECT * FROM tblGroup WHERE id=" + id.ToString() + ";";
            GroupList groups = new GroupList(ExecuteCommand());
            if (groups.Count() > 0)
            {
                return groups[0];
            }
            return null;
        }

        public int Insert(Group group)
        {
            command.CommandText = "INSERT INTO TblGroup " +
                "(GroupName, Points, GroupShortcut)" +
                "VALUES (@GroupName, @Points, @GroupShortcut)";
            LoadParameters(group);
            return ExecuteCRUD();
        }
        public int Update(Group game)
        {
            command.CommandText = "UPDATE TblGroup SET GroupName = @GroupName, Points = @Points, " +
                "GroupShortcut = @GroupShortcut WHERE id = @id";
            LoadParameters(game);
            return ExecuteCRUD();
        }
        public int Delete(Group game)
        {
            command.CommandText = "DELETE FROM TblGroup WHERE ID = @id";
            LoadParameters(game);
            return ExecuteCRUD();
        }
    }
}
