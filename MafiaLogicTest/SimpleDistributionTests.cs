using System.Linq;
using Mafia;
using NUnit.Framework;

namespace MafiaLogicTest
{
    [TestFixture]
    public class SimpleDistributionTests
    {
        private static SimpleRoleDist _dist;
        
        [SetUp]
        public void Setup()
        {
            _dist = new SimpleRoleDist();
        }
        
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        public void TestMafiaCount(int playersCount)
        {
            var result = _dist.DistributeRoles(playersCount);
            Assert.AreEqual(playersCount / 7 + 1, result.Count(r => r.GetType() == typeof(MafiaRole)));
        }
    }
}