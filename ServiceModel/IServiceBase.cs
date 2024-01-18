using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModel
{
    [ServiceContract]
    public interface IServiceBase
    {
        //Get
        [OperationContract] UserList GetAllUsers();
        [OperationContract] GameList GetAllGames();
        [OperationContract] GroupList GetAllGroups();
        [OperationContract] GuessList GetAllGuesses();
        [OperationContract] PlayerList GetAllPlayers();
        //insert
        [OperationContract] int InsertUser(User user);
        [OperationContract] int InsertGame(Game game);
        [OperationContract] int InsertGroup(Group group);
        [OperationContract] int InsertGuess(Guess guess);
        [OperationContract] int InsertPlayer(Player player);

        //update

        [OperationContract] int UpdateUser(User user);
        [OperationContract] int UpdateGame(Game game);
        [OperationContract] int UpdateGroup(Group group);
        [OperationContract] int UpdateGuess(Guess guess);
        [OperationContract] int UpdatePlayer(Player player);


        //delete
        [OperationContract] int DeleteUser(User user);
        [OperationContract] int DeleteGame(Game game);
        [OperationContract] int DeleteGroup(Group group);
        [OperationContract] int DeleteGuess(Guess guess);
        [OperationContract] int DeletePlayer(Player player);

        //others
        [OperationContract] User Login(User user);
        [OperationContract] GameList GetGameResults();
        [OperationContract] void InsertNewGames();
    }
}
