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
            guess.ISCORRECT = bool.Parse(reader["IsCorrect"].ToString());

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
            //command.Parameters.AddWithValue("@TeamGuessed",guess.TEAMGUESSED==null?1: guess.TEAMGUESSED.ID);
            if(guess.ISDRAW)
                command.Parameters.AddWithValue("@TeamGuessed", 1);
            else
                command.Parameters.AddWithValue("@TeamGuessed", guess.TEAMGUESSED.ID);
            command.Parameters.AddWithValue("@IsDraw", guess.ISDRAW);
            command.Parameters.AddWithValue("@IsCorrect", guess.ISCORRECT);
        }

        //שאילתה המחזירה את כלל הניחושים
        public GuessList SelectAll()
        {
            command.CommandText = "SELECT * FROM tblGuess";
            GuessList guesses = new GuessList(ExecuteCommand());
            return guesses;
        }

        public GuessList SelectByGame(Game game)
        {
            command.CommandText = $"SELECT * FROM tblGuess WHERE GameID = {game.ID}";
            GuessList guesses = new GuessList(ExecuteCommand());
            return guesses;
        }

        public int Insert(Guess guess)
        {
            command.CommandText = "INSERT INTO tblGuess " +
                "(UserID, GameID, TeamGuessed, isDraw, IsCorrect)" +
                "VALUES (@UserID, @GameID, @TeamGuessed, @isDraw, @IsCorrect)";
            LoadParameters(guess);
            return ExecuteCRUD();
        }
        public int Update(Guess guess)
        {
            command.CommandText = "UPDATE tblGuess SET UserID = @UserID, GameID = @GameID, " +
                "TeamGuessed = @TeamGuessed, isDraw = @isDraw, IsCorrect = @IsCorrect WHERE GameID = @GameID AND UserID = @UserID";
            LoadParameters(guess);
            return ExecuteCRUD();
        }
        public int Delete(Guess guess)
        {
            command.CommandText = "DELETE FROM tblGuess WHERE UserID = @UserID AND GameID = @GameID";
            LoadParameters(guess);
            return ExecuteCRUD();
        }

        public int DeleteByGame(Game game)
        {
            command.CommandText = $"DELETE FROM tblGuess WHERE GameID = {game.ID}";
            return ExecuteCRUD();
        }

        public int DeleteByUser(User user)
        {
            command.CommandText = $"DELETE FROM tblGuess WHERE UserID = {user.ID}";
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
