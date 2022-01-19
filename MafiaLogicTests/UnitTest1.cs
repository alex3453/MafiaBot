using NUnit.Framework;

namespace MafiaLogicTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}
/*
 [TestFixture]
    public class MafiaGameTests
    {
        private static void Test(IEnumerable<string> players, List<List<Dictionary<string, string>>> turns,
            IEnumerable<string> expectedWinners)
        {
            var mafia = new MafiaGame(new SimpleRoleDist());
            foreach (var player in players)
                mafia.RegisterPlayer(player);
            mafia.StartGame();
        }
        
        [TestCase(new [] {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"}, 
            new List<List<Dictionary<string, string>>>
            {
                new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        {"1", "2"},
                        {"1", "3"}
                    }
                }
            }, 
            new [] {"1"})]
        [TestCase(null, null, null)]
        public static void RunTests(IEnumerable<string> players, 
            List<List<Dictionary<string, string>>> turns,
            IEnumerable<string> expectedWinners)
        {
            Test(players, turns, expectedWinners);
        }
    }
*/