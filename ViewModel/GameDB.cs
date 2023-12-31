﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class GameDB : BaseDB
    {
        protected override BaseEntity NewEntity()
        {
            return new Game() as BaseEntity;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Game game = entity as Game;
            game.ID = int.Parse(reader["GameID"].ToString());
            game.HOMESCORE = int.Parse(reader["HTeamScore"].ToString());
            game.AWAYSCORE = int.Parse(reader["ATeamScore"].ToString());

            int HomeTeamId = int.Parse(reader["HomeTeam"].ToString());
            int AwayTeamId = int.Parse(reader["AwayTeam"].ToString());

            GroupDB groupDB = new GroupDB();
            game.HOMETEAM = groupDB.SelectByID(HomeTeamId);
            game.AWAYTEAM = groupDB.SelectByID(AwayTeamId);

            return game;
        }
        protected override void LoadParameters(BaseEntity entity)
        {
            Game game = entity as Game;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@GameID", game.ID);
            command.Parameters.AddWithValue("@HomeTeam", game.HOMETEAM.ID);
            command.Parameters.AddWithValue("@AwayTeam", game.AWAYTEAM.ID);
            command.Parameters.AddWithValue("@HTeamScore", game.HOMESCORE);
            command.Parameters.AddWithValue("@ATeamScore", game.AWAYSCORE);
            command.Parameters.AddWithValue("@GameDate", game.Date);
        }

        //שאילתה המחזירה את כלל המשחקים
        public GameList SelectAll()
        {
            command.CommandText = "SELECT * FROM tblGames";
            GameList games = new GameList(ExecuteCommand());
            return games;
        }

        //שאילתה המחזירה משחק לפי מאפיין (id)
        public Game SelectByID(int id)
        {
            command.CommandText = "SELECT * FROM tblGames WHERE id=" + id.ToString() + ";";
            GameList games = new GameList(ExecuteCommand());
            if (games.Count() > 0)
            {
                return games[0];
            }
            return null;
        }

        public int Insert(Game game)
        {
            command.CommandText = "INSERT INTO TblGames " +
                "(HomeTeam, AwayTeam, HteamScore, ATeamScore, GameDate)" +
                "VALUES (@HomeTeam, @AwayTeam, @HteamScore, " +
                "@ATeamScore, @GameDate)";
            LoadParameters(game);
            return ExecuteCRUD();
        }
        public int Update(Game game)
        {
            command.CommandText = "UPDATE TblGames SET HomeTeam = @HomeTeam, AwayTeam = @AwayTeam, " +
                "HteamScore = @HteamScore, ATeamScore = @ATeamScore, GameDate = @GameDate WHERE id = @id";
            LoadParameters(game);
            return ExecuteCRUD();
        }
        public int Delete(Game game)
        {
            command.CommandText = "DELETE FROM TblUser WHERE ID = @id";
            LoadParameters(game);
            return ExecuteCRUD();
        }

    }
}
