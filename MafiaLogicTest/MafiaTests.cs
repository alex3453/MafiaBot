using System.Collections.Generic;
using System.Linq;
using Mafia;
using NUnit.Framework;

namespace MafiaLogicTest
{
    [TestFixture]
    public class MafiaTests
    {
        private static MafiaGame _mafiaGame;
        
        [SetUp]
        public void Setup()
        {
            _mafiaGame = new MafiaGame(new DistributionForTests());
        }

        private static void RegisterPlayers(int playersCount)
        {
            for (var i = 0; i < playersCount; i++)
                _mafiaGame.RegisterPlayer(i.ToString());
        }

        private static void SelfVoting(int playersCount)
        {
            for (var i = 0; i < playersCount; i++)
            {
                var playerName = i.ToString();
                _mafiaGame.Vote(playerName, playerName);
            }
        }
        
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void LessThenFourPlayersCantStartTest(int playersCount)
        {
            RegisterPlayers(playersCount);
            Assert.AreEqual(Status.WaitingPlayers, _mafiaGame.Status);
        }
        
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(100)]
        public void MoreThanFourPlayersStartTest(int playersCount)
        {
            RegisterPlayers(playersCount);
            Assert.AreEqual(Status.ReadyToStart, _mafiaGame.Status);
        }

        [TestCase]
        public void GameStartedTest()
        {
            RegisterPlayers(4);
            _mafiaGame.StartGame();
            Assert.AreEqual(Status.Voting, _mafiaGame.Status);
        }

        [TestCase]
        public void MafiaStartKillingTest()
        {
            var playersCount = 4;
            RegisterPlayers(playersCount);
            _mafiaGame.StartGame();
            SelfVoting(playersCount);
            Assert.AreEqual(Status.MafiaKilling, _mafiaGame.Status);
        }
        
        [TestCase]
        public void DayKillTest()
        {
            RegisterPlayers(4);
            _mafiaGame.StartGame();
            _mafiaGame.Vote("0", "1");
            _mafiaGame.Vote("1", "1");
            _mafiaGame.Vote("2", "1");
            _mafiaGame.Vote("3", "1");
            Assert.AreEqual("1", _mafiaGame.DeadPlayers.Single());
            Assert.IsTrue(_mafiaGame.IsSomeBodyDied);
        }
        
        [TestCase]
        public void MafiaWinTest()
        {
            RegisterPlayers(4);
            _mafiaGame.StartGame();
            _mafiaGame.Vote("0", "1");
            _mafiaGame.Vote("1", "1");
            _mafiaGame.Vote("2", "1");
            _mafiaGame.Vote("3", "1");
            _mafiaGame.Act(_mafiaGame.AllPlayers.First(player => player.Role is MafiaRole), 2);
            Assert.AreEqual(Status.MafiaWins, _mafiaGame.Status);
        }
        
        [TestCase]
        public void PeacefulWinTest()
        {
            RegisterPlayers(4);
            _mafiaGame.StartGame();
            _mafiaGame.Vote("0", "0");
            _mafiaGame.Vote("1", "0");
            _mafiaGame.Vote("2", "0");
            _mafiaGame.Vote("3", "0");
            Assert.AreEqual(Status.PeacefulWins, _mafiaGame.Status);
        }
        
        [TestCase]
        public void MafiaSelfKillTest()
        {
            RegisterPlayers(4);
            _mafiaGame.StartGame();
            _mafiaGame.Vote("0", "1");
            _mafiaGame.Vote("1", "1");
            _mafiaGame.Vote("2", "1");
            _mafiaGame.Vote("3", "1");
            _mafiaGame.Act(_mafiaGame.AllPlayers.First(player => player.Role is MafiaRole), 1);
            Assert.AreEqual(Status.PeacefulWins, _mafiaGame.Status);
        }
        
        [TestCase]
        public void DayAfterNightTest()
        {
            const int playersCount = 4;
            RegisterPlayers(playersCount);
            _mafiaGame.StartGame();
            SelfVoting(playersCount);
            _mafiaGame.Act(_mafiaGame.AllPlayers.First(player => player.Role is MafiaRole), 2);
            Assert.AreEqual(Status.Voting, _mafiaGame.Status);
        }
        
        [TestCase]
        public void MafiaChooseDifferentPlayersKillTest()
        {
            const int playersCount = 7;
            RegisterPlayers(playersCount);
            _mafiaGame.StartGame();
            SelfVoting(playersCount);
            var mafiaPlayers = _mafiaGame.AllPlayers.Where(player => player.Role is MafiaRole).ToList();
            _mafiaGame.Act(mafiaPlayers[0], 3);
            _mafiaGame.Act(mafiaPlayers[1], 4);
            Assert.AreEqual(1, _mafiaGame.DeadPlayers.Count);
            Assert.IsTrue(_mafiaGame.DeadPlayers.Single() == "2" || _mafiaGame.DeadPlayers.Single() == "3");
        }
        
        [TestCase]
        public void MafiaChooseSamePlayerKillTest()
        {
            const int playersCount = 7;
            RegisterPlayers(playersCount);
            _mafiaGame.StartGame();
            SelfVoting(playersCount);
            var mafiaPlayers = _mafiaGame.AllPlayers.Where(player => player.Role is MafiaRole).ToList();
            _mafiaGame.Act(mafiaPlayers[0], 3);
            _mafiaGame.Act(mafiaPlayers[1], 3);
            Assert.IsTrue(_mafiaGame.DeadPlayers.Single() == "2");
        }
        
        [TestCase]
        public void SameVotedCountTest()
        {
            const int playersCount = 4;
            RegisterPlayers(playersCount);
            _mafiaGame.StartGame();
            _mafiaGame.Vote("0", "0");
            _mafiaGame.Vote("1", "0");
            _mafiaGame.Vote("2", "1");
            _mafiaGame.Vote("3", "1");
            Assert.AreEqual(0, _mafiaGame.DeadPlayers.Count);
        }
        
        [TestCase]
        public void VoteNotInGamePlayerTest()
        {
            const int playersCount = 4;
            RegisterPlayers(playersCount);
            _mafiaGame.StartGame();
            Assert.AreEqual(OperationStatus.NotInGame, _mafiaGame.Vote("6", "0"));
        }
        
        [TestCase]
        public void AlreadyVotePlayerTest()
        {
            const int playersCount = 4;
            RegisterPlayers(playersCount);
            _mafiaGame.StartGame();
            _mafiaGame.Vote("1", "0");
            Assert.AreEqual(OperationStatus.Already, _mafiaGame.Vote("1", "1"));
        }
        
        [TestCase]
        public void VoteDeadPlayerTest()
        {
            const int playersCount = 5;
            RegisterPlayers(playersCount);
            _mafiaGame.StartGame();
            _mafiaGame.Vote("0", "1");
            _mafiaGame.Vote("1", "1");
            _mafiaGame.Vote("2", "1");
            _mafiaGame.Vote("3", "1");
            _mafiaGame.Vote("5", "1");
            _mafiaGame.Act(_mafiaGame.AllPlayers.First(player => player.Role is MafiaRole), 2);
            Assert.AreEqual(OperationStatus.NotInGame, _mafiaGame.Vote("1", "0"));
        }
        
        [TestCase]
        public void NullCantVoteTest()
        {
            const int playersCount = 5;
            RegisterPlayers(playersCount);
            _mafiaGame.StartGame();
            Assert.AreEqual(OperationStatus.NotInGame, _mafiaGame.Vote(null, "0"));
        }
        
        [TestCase]
        public void CantVoteNullTest()
        {
            const int playersCount = 5;
            RegisterPlayers(playersCount);
            _mafiaGame.StartGame();
            Assert.AreEqual(OperationStatus.Cant, _mafiaGame.Vote("1", null));
        }
        
        [TestCase]
        public void NotMafiaCantKillTest()
        {
            const int playersCount = 4;
            RegisterPlayers(playersCount);
            _mafiaGame.StartGame();
            SelfVoting(playersCount);
            Assert.AreEqual(OperationStatus.WrongAct, 
                _mafiaGame.Act(_mafiaGame.AllPlayers.First(player => player.Role is not MafiaRole), 2));
        }
        
        [TestCase]
        public void MafiaCantChooseTwiceTest()
        {
            const int playersCount = 7;
            RegisterPlayers(playersCount);
            _mafiaGame.StartGame();
            SelfVoting(playersCount);
            var mafiaPlayers = _mafiaGame.AllPlayers.Where(player => player.Role is MafiaRole).ToList();
            _mafiaGame.Act(mafiaPlayers[0], 3);
            Assert.AreEqual(OperationStatus.Already, _mafiaGame.Act(mafiaPlayers[0], 3));
        }
        
        [TestCase]
        public void IncorrectNumberKillTest()
        {
            const int playersCount = 7;
            RegisterPlayers(playersCount);
            _mafiaGame.StartGame();
            SelfVoting(playersCount);
            var mafiaPlayers = _mafiaGame.AllPlayers.Where(player => player.Role is MafiaRole).ToList();
            Assert.AreEqual(OperationStatus.Incorrect, _mafiaGame.Act(mafiaPlayers[0], 100));
            Assert.AreEqual(OperationStatus.Incorrect, _mafiaGame.Act(mafiaPlayers[0], 0));
            Assert.AreEqual(OperationStatus.Incorrect, _mafiaGame.Act(mafiaPlayers[0], -10));
        }
    }
    
    public class DistributionForTests : IRoleDistribution
    {
        public List<Role> DistributeRoles(int playersCount)
        {
            var result = new List<Role>();
            var mafiaCount = playersCount / 7 + 1;
            for (var i = 0; i < mafiaCount; i++)
                result.Add(new MafiaRole());
            for (var i = 0; i < playersCount - mafiaCount; i++)
                result.Add(new PeacefulRole());
            return result;
        }
    }
}
