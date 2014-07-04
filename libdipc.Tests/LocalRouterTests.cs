using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Ipc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace libdipc.Tests
{
   [TestClass]
   class LocalRouterTests
   {
      private LocalRouter testObj;

      private Mock<LocalTerminal> terminal1 = new Mock<LocalTerminal>();
      private Mock<LocalTerminal> terminal2 = new Mock<LocalTerminal>();
      
      [TestInitialize]
      public void Setup()
      {
         testObj = new LocalRouter(new LocalRouterConfiguration() { NodeIdentifier = "router1" });
      }

      [TestMethod]
      public void LocalRouterPeersWithTerminals()
      {
         var result1 = testObj.PeerAsync(terminal1.Object).Result;
         var result2 = testObj.PeerAsync(terminal2.Object).Result;

         Assert.AreEqual(PeeringState.Connected, result1.PeeringState);
         Assert.AreEqual(PeeringState.Connected, result2.PeeringState);

         Assert.IsTrue(testObj.Peers.Contains(terminal1.Object));
         Assert.IsTrue(testObj.Peers.Contains(terminal2.Object));
      }
   }
}
