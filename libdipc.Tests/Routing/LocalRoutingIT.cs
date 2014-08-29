using Dargon.Ipc.Messaging;
using Dargon.Ipc.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace libdipc.Tests.Routing
{
   [TestClass]
   public class LocalRoutingIT
   {
      private LocalRouter rootRouter;
      private LocalRouter innerRouter1;
      private LocalRouter innerRouter1_1;
      private LocalRouter innerRouter1_2;
      private LocalRouter innerRouter2;
      private LocalTerminal terminal1;
      private LocalTerminal terminal2;
      private LocalTerminal terminal3;
      private LocalTerminal terminal4;
      private LocalTerminal terminal5;
      private LocalTerminal terminal6;
      private LocalTerminal terminal7;

      [TestInitialize]
      public void Setup()
      {
         rootRouter = new LocalRouter(new LocalRouterConfiguration() { NodeIdentifier = "root_router" });
         innerRouter1 = new LocalRouter(new LocalRouterConfiguration() { NodeIdentifier = "inner_router_1" });
         innerRouter1_1 = new LocalRouter(new LocalRouterConfiguration() { NodeIdentifier = "inner_router_1_1" });
         innerRouter1_2 = new LocalRouter(new LocalRouterConfiguration() { NodeIdentifier = "inner_router_1_2" });
         innerRouter2 = new LocalRouter(new LocalRouterConfiguration() { NodeIdentifier = "inner_router_2" });
         terminal1 = new LocalTerminal(new LocalTerminalConfiguration() { NodeIdentifier = "terminal1" });
         terminal2 = new LocalTerminal(new LocalTerminalConfiguration() { NodeIdentifier = "terminal2" });
         terminal3 = new LocalTerminal(new LocalTerminalConfiguration() { NodeIdentifier = "terminal3" });
         terminal4 = new LocalTerminal(new LocalTerminalConfiguration() { NodeIdentifier = "terminal4" });
         terminal5 = new LocalTerminal(new LocalTerminalConfiguration() { NodeIdentifier = "terminal5" });
         terminal6 = new LocalTerminal(new LocalTerminalConfiguration() { NodeIdentifier = "terminal6" });
         terminal7 = new LocalTerminal(new LocalTerminalConfiguration() { NodeIdentifier = "terminal7" });
      }

      [TestMethod]
      public void Run()
      {
         SuccessfullyPeerRouterWithChild(rootRouter, terminal1);
         SuccessfullyPeerRouterWithChild(rootRouter, innerRouter1);
         SuccessfullyPeerRouterWithChild(innerRouter1, terminal2);
         SuccessfullyPeerRouterWithChild(innerRouter1, innerRouter1_1);
         SuccessfullyPeerRouterWithChild(innerRouter1_1, terminal3);
         SuccessfullyPeerRouterWithChild(innerRouter1_1, terminal4);
         SuccessfullyPeerRouterWithChild(innerRouter1, innerRouter1_2);
         SuccessfullyPeerRouterWithChild(innerRouter1_2, terminal5);
         SuccessfullyPeerRouterWithChild(innerRouter1_2, terminal6);
         SuccessfullyPeerRouterWithChild(rootRouter, innerRouter2);
         SuccessfullyPeerRouterWithChild(innerRouter2, terminal7);

         var nodes = new IDipNode[] { rootRouter, innerRouter1, innerRouter1_1, innerRouter1_2, innerRouter2, terminal1, terminal2, terminal3, terminal4, terminal5, terminal6, terminal7 };
         for (var i = 0; i < nodes.Length; i++)
         {
            for (var j = i + 1; j < nodes.Length; j++)
            {
               SendPingAndPong(nodes[i], nodes[j]);
            }
         }
      }

      private void SuccessfullyPeerRouterWithChild(LocalRouter router, IDipNode node)
      {
         var result = node.PeerParentAsync(router).Result;
         Assert.AreEqual(PeeringState.Connected, result.PeeringState);
      }

      private void SendPingAndPong(IDipNode a, IDipNode b)
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
