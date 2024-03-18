using Model;
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

            game.Date = DateTime.Parse(reader["GameDate"].ToString());

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
            command.Parameters.AddWithValue("@HomeTeam", game.HOMETEAM.ID);
            command.Parameters.AddWithValue("@AwayTeam", game.AWAYTEAM.ID);
            command.Parameters.AddWithValue("@HTeamScore", game.HOMESCORE);
            command.Parameters.AddWithValue("@ATeamScore", game.AWAYSCORE);
            command.Parameters.AddWithValue("@GameDate", game.Date.ToShortDateString());
            command.Parameters.AddWithValue("@GameID", game.ID);
        }

        //שאילתה המחזירה את כלל המשחקים
        public GameList SelectAll()
        {
            command.CommandText = "SELECT * FROM tblGames ORDER BY GameID";
            GameList games = new GameList(ExecuteCommand());
            return games;
        }

        //שאילתה המחזירה משחק לפי מאפיין (id)
        public Game SelectByID(int id)
        {
            command.CommandText = $"SELECT * FROM tblGames WHERE GameID = {id}";
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
                "HteamScore = @HteamScore, ATeamScore = @ATeamScore, GameDate = @GameDate WHERE GameID = @id";
            LoadParameters(game);
            return ExecuteCRUD();
        }
        public int Delete(Game game)
        {
            command.CommandText = $"DELETE FROM tblGames WHERE GameID ={game.ID}";
            LoadParameters(game);
            return ExecuteCRUD();
        }

        public bool isExist(Game game)
        {
            command.CommandText = $"SELECT * FROM tblGames WHERE GameDate = #{game.Date.ToShortDateString()}# " +
                $"AND HomeTeam = {game.HOMETEAM.ID} AND AwayTeam = {game.AWAYTEAM.ID}";

            GameList games = new GameList(ExecuteCommand());
            return games.Count() > 0;
        }

        public bool IsUpdated(Game game)
        {
            command.CommandText = $"SELECT * FROM tblGames WHERE GameDate = #{game.Date.ToShortDateString()}# " +
                $"AND HomeTeam = {game.HOMETEAM.ID} AND AwayTeam = {game.AWAYTEAM.ID}";

            GameList games = new GameList(ExecuteCommand());
            return games[0].AWAYSCORE != -1; //i insert games to sql with -1 scores
        }

    }
}
