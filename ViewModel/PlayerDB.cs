﻿using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class PlayerDB:BaseDB
    {
        protected override BaseEntity NewEntity()
        {
            return new Player() as BaseEntity;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Player player = entity as Player;
            player.ID = int.Parse(reader["PlayerID"].ToString());
            player.IsCaptain = bool.Parse(reader["IsCaptain"].ToString());
            player.FirstName = reader["PName"].ToString();
            player.LastName = reader["PLastName"].ToString();
            player.Number = int.Parse(reader["PNumber"].ToString());

            int groupId = int.Parse(reader["PGroup"].ToString());
            GroupDB groupDB = new GroupDB();
            player.PlayerGroup = groupDB.SelectByID(groupId);

            return player;
        }

        protected override void LoadParameters(BaseEntity entity)
        {
            Player player = entity as Player;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@PName", player.FirstName);
            command.Parameters.AddWithValue("@PLastName", player.LastName);
            command.Parameters.AddWithValue("@PNumber", player.Number);
            command.Parameters.AddWithValue("@PGroup", player.PlayerGroup.ID);
            command.Parameters.AddWithValue("@IsCaptain", player.IsCaptain);
            command.Parameters.AddWithValue("@PlayerID", player.ID);
        }


        //שאילתה המחזירה את כלל הניחושים
        public PlayerList SelectAll()
        {
            command.CommandText = "SELECT * FROM tblPlayers";
            PlayerList players = new PlayerList(ExecuteCommand());
            return players;
        }
        //שאילתה המחזירה משתמש לפי מאפיין (id)
        public Player SelectByID(int id)
        {
            command.CommandText = "SELECT * FROM tblPlayers WHERE id=" + id.ToString() + ";";
            PlayerList players = new PlayerList(ExecuteCommand());
            if (players.Count() > 0)
            {
                return players[0];
            }
            return null;
        }

        public int Insert(Player player)
        {
            command.CommandText = "INSERT INTO tblPlayers " +
                "(PName, PlastName, PNumber, PGroup, IsCaptain)" +
                " VALUES (@PName, @PlastName, @PNumber, @PGroup, @IsCaptain)";
            LoadParameters(player);
            return ExecuteCRUD();
        }
        public int Update(Player player)
        {
            command.CommandText = "UPDATE TblPlayers SET UserID = @UserID, GameID = @GameID, " +
                "PName = @PName, PlastName = @PlastName, PNumber = @PNumber, PGroup = @PGroup, " +
                "IsCaptain = @isCaptain WHERE id = @id";
            LoadParameters(player);
            return ExecuteCRUD();
        }
        public int Delete(Player player)
        {
            command.CommandText = "DELETE FROM TblPlayers WHERE ID = @id";
            LoadParameters(player);
            return ExecuteCRUD();
        }

        public bool IsExist(Player player)
        {
            command.CommandText = $"SELECT * FROM tblPlayers WHERE PNumber = {player.Number} AND " +
                $"PGroup = {player.PlayerGroup.ID}";
            PlayerList players = new PlayerList(ExecuteCommand());
            return players.Count() > 0;
        }

        public PlayerList SelectByGroup(Group g)
        {
            command.CommandText = $"SELECT * FROM tblPlayers WHERE PGroup = {g.ID}";
            PlayerList players = new PlayerList(ExecuteCommand());
            return players;
        }

        public PlayerList SelectByUser(User u)
        {
            command.CommandText = $"SELECT * FROM (tblPlayers INNER JOIN tblGroup ON tblPlayers.PGroup = tblGroup.id) WHERE PGroup = {u.FAVORITEGROUP.ID}";
            PlayerList players = new PlayerList(ExecuteCommand());
            return players;
        }
    }
}
