using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Ipc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace libdipc.Tests
{
   [TestClass]
   public class LocalRoutingIT
   {
      private LocalRouter router;
      private LocalTerminal terminal1;
      private LocalTerminal terminal2;

      [TestInitialize]
      public void Setup()
      {
         var routerConfig = new LocalRouterConfiguration() { NodeIdentifier = "router" };
         router = new LocalRouter(routerConfig);
         terminal1 = new LocalTerminal(new LocalTerminalConfiguration() { NodeIdentifier = "terminal1" });
         terminal2 = new LocalTerminal(new LocalTerminalConfiguration() { NodeIdentifier = "terminal2" });
      }

      [TestMethod]
      public void Run()
      {
         SuccessfullyPeerRouterAndTerminal(router, terminal1);
         SuccessfullyPeerRouterAndTerminal(router, terminal2);
         SendPingAndPong(terminal1, terminal2);
      }

      private void SuccessfullyPeerRouterAndTerminal(LocalRouter router, LocalTerminal terminal)
      {
         var result = terminal.PeerAsync(router).Result;
         Assert.AreEqual(PeeringState.Connected, result.PeeringState);
      }

      private void SendPingAndPong(LocalTerminal a, LocalTerminal b)
      {
         object ping = 1;
         object pong = 2;
         a.Send(b, new Message<object>(ping));
         Assert.AreEqual(ping, b.DequeueMessage().Content);
         b.Send(a, new Message<object>(pong));
         Assert.AreEqual(pong, a.DequeueMessage().Content);
      }
   }
}
