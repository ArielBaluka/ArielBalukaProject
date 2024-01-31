using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ViewModel
{
    public class GuessDB:BaseDB
    {
        protected override BaseEntity NewEntity()
        {
            return new Guess() as BaseEntity;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Guess guess = entity as Guess;
            guess.ISDRAW = bool.Parse(reader["IsDraw"].ToString());

            int groupId = int.Parse(reader["TeamGuessed"].ToString());
            GroupDB groupDB = new GroupDB();
            guess.TEAMGUESSED = groupDB.SelectByID(groupId);
            
            int userId = int.Parse(reader["UserID"].ToString());
            UserDB userDB = new UserDB();
            guess.USER = userDB.SelectByID(userId);

            int gameID = int.Parse(reader["GameID"].ToString());
            GameDB gameDB = new GameDB();
            guess.GAME = gameDB.SelectByID(gameID);

            return guess;
        }

        protected override void LoadParameters(BaseEntity entity)
        {
            Guess guess = entity as Guess;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@UserID", guess.USER.ID);
            command.Parameters.AddWithValue("@GameID", guess.GAME.ID);
            command.Parameters.AddWithValue("@TeamGuessed",guess.TEAMGUESSED==null?1: guess.TEAMGUESSED.ID);
            command.Parameters.AddWithValue("@IsDraw", guess.ISDRAW);
        }

        //שאילתה המחזירה את כלל הניחושים
        public GuessList SelectAll()
        {
            command.CommandText = "SELECT * FROM tblGuess";
            GuessList guesses = new GuessList(ExecuteCommand());
            return guesses;
        }

        public int Insert(Guess guess)
        {
            command.CommandText = "INSERT INTO TblGuess " +
                "(UserID, GameID, TeamGuessed, isDraw)" +
                "VALUES (@UserID, @GameID, @TeamGuessed, @isDraw)";
            LoadParameters(guess);
            return ExecuteCRUD();
        }
        public int Update(Guess guess)
        {
            command.CommandText = "UPDATE TblGuess SET UserID = @UserID, GameID = @GameID, " +
                "TeamGuessed = @TeamGuessed, isDraw = @isDraw WHERE id = @id";
            LoadParameters(guess);
            return ExecuteCRUD();
        }
        public int Delete(Guess guess)
        {
            command.CommandText = "DELETE FROM TblGuess WHERE UserID = @UserID AND GameID = @GameID";
            LoadParameters(guess);
            return ExecuteCRUD();
        }

        public GuessList GetUserGuesses(User user)
        {
            command.CommandText = $"SELECT * FROM tblGuess WHERE UserID = {user.ID}";
            GuessList guesses = new GuessList(ExecuteCommand());
            return guesses;
        }

    }
}
