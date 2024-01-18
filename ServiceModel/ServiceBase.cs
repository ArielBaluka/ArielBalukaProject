using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace ServiceModel
{
    public class ServiceBase : IServiceBase
    {
        public GameList GetAllGames()
        {
            GameDB db = new GameDB();
            GameList list = db.SelectAll();
            return list;
        }

        public GroupList GetAllGroups()
        {
            GroupDB db = new GroupDB();
            GroupList list = db.SelectAll();
            return list;
        }

        public GuessList GetAllGuesses()
        {
            GuessDB db = new GuessDB();
            GuessList list = db.SelectAll();
            return list;
        }

        public PlayerList GetAllPlayers()
        {
            PlayerDB db = new PlayerDB();
            PlayerList list = db.SelectAll();
            return list;
        }

        public UserList GetAllUsers()
        {
            UserDB db = new UserDB();
            UserList list = db.SelectAll();
            return list;
        }
        //
        public int InsertGame(Game game)
        {
            GameDB db = new GameDB();
            int res = db.Insert(game);
            return res;
        }

        public int InsertGroup(Group group)
        {
            GroupDB db = new GroupDB();
            int res = db.Insert(group);
            return res;
        }

        public int InsertGuess(Guess guess)
        {
            GuessDB db = new GuessDB();
            int res = db.Insert(guess);
            return res;
        }

        public int InsertPlayer(Player player)
        {
            PlayerDB db = new PlayerDB();
            int res = db.Insert(player);
            return res;
        }

        public int InsertUser(User user)
        {
            UserDB db = new UserDB();
            int res = db.Insert(user);
            return res;
        }
        

        public int UpdateGame(Game game)
        {
            GameDB db = new GameDB();
            int res = db.Update(game);
            return res;
        }

        public int UpdateGroup(Group group)
        {
            GroupDB db = new GroupDB();
            int res = db.Update(group);
            return res;
        }

        public int UpdateGuess(Guess guess)
        {
            GuessDB db = new GuessDB();
            int res = db.Update(guess);
            return res;
        }

        public int UpdatePlayer(Player player)
        {
            PlayerDB db = new PlayerDB();
            int res = db.Update(player);
            return res;
        }

        public int UpdateUser(User user)
        {
            UserDB db = new UserDB();
            int res = db.Update(user);
            return res;
        }


        public int DeleteGame(Game game)
        {
            GameDB db = new GameDB();
            int res = db.Delete(game);
            return res;
        }

        public int DeleteGroup(Group group)
        {
            GroupDB db = new GroupDB();
            int res = db.Delete(group);
            return res;
        }

        public int DeleteGuess(Guess guess)
        {
            GuessDB db = new GuessDB();
            int res = db.Delete(guess);
            return res;
        }

        public int DeletePlayer(Player player)
        {
            PlayerDB db = new PlayerDB();
            int res = db.Delete(player);
            return res;
        }

        public int DeleteUser(User user)
        {
            UserDB db = new UserDB();
            int res = db.Delete(user);
            return res;
        }
        public User Login(User user)
        {
            UserDB db = new UserDB();
            User LoggedUser = db.Login(user);
            if(LoggedUser != null && LoggedUser.UserName.Equals(user.UserName) 
                && LoggedUser.PassWord.Equals(user.PassWord))
                return LoggedUser;
            return null;
        }

        public GameList GetGameResults()
        {
            GameList list = PremierLeagueData.ParseRSSEPL();
            return list;
        }


        public void InsertNewGames()
        {
            GameList list = GetGameResults();
            GameDB db = new GameDB();
            foreach(Game game in list)
            {
                if(!db.isExist(game))
                {
                    db.Insert(game);
                }
            }
        }

    }
}
