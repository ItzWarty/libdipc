using System;
using System.Linq;
using System.Threading.Tasks;
using Dargon.Ipc.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace libdipc.Tests.Routing
{
   [TestClass]
   public class LocalRouterTests
   {
      private LocalRouter testObj;

      private readonly Guid accessibleLocalNodeGuid = Guid.NewGuid();
      private readonly Mock<IDipNode> accessibleLocalNode = new Mock<IDipNode>();

      private readonly Guid unaccessibleLocalNodeGuid = Guid.NewGuid();
      private readonly Mock<IDipNode> unaccessibleLocalNode = new Mock<IDipNode>();
      private readonly Exception unaccessibleLocalNodePeeringException = new Exception();

      private readonly Guid accessibleRemoteRelayGuid = Guid.NewGuid();
      private readonly Mock<IDipNode> accessibleRemoteRelay = new Mock<IDipNode>();

      private readonly Guid unaccessibleRemoteTerminalGuid = Guid.NewGuid();
      private readonly Mock<IDipNode> unaccessibleRemoteTerminal = new Mock<IDipNode>();
      
      [TestInitialize]
      public void Setup()
      {
         testObj = new LocalRouter(new LocalRouterConfiguration() { NodeIdentifier = "router1" });
         accessibleLocalNode.Setup((n) => n.Guid).Returns(accessibleLocalNodeGuid);
         accessibleLocalNode.Setup((n) => n.Role).Returns(DipRole.LocalRouter);
         accessibleLocalNode.Setup((n) => n.PeerParentAsync(testObj)).Returns(Task.FromResult<IPeeringResult>(new PeeringResult(PeeringState.Connected, testObj)));

         unaccessibleLocalNode.Setup((n) => n.Guid).Returns(unaccessibleLocalNodeGuid);
         unaccessibleLocalNode.Setup((n) => n.Role).Returns(DipRole.LocalRouter);
         unaccessibleLocalNode.Setup((n) => n.PeerParentAsync(testObj)).Returns(Task.FromResult<IPeeringResult>(new PeeringResult(PeeringState.Disconnected, testObj, unaccessibleLocalNodePeeringException)));

         accessibleRemoteRelay.Setup((n) => n.Guid).Returns(accessibleRemoteRelayGuid);
         accessibleRemoteRelay.Setup((n) => n.Role).Returns(DipRole.RemoteRouter);
         accessibleRemoteRelay.Setup((n) => n.PeerParentAsync(testObj)).Returns(Task.FromResult<IPeeringResult>(new PeeringResult(PeeringState.Connected, testObj)));

         unaccessibleRemoteTerminal.Setup((n) => n.Guid).Returns(unaccessibleRemoteTerminalGuid);
         unaccessibleRemoteTerminal.Setup((n) => n.Role).Returns(DipRole.RemoteTerminal);
      }

      [TestMethod]
      public void LocalRouterDelegatesSuccessfulPeerWithLocalNodesWithoutErrors()
      {
         var result = testObj.PeerChildAsync(accessibleLocalNode.Object).Result;

         accessibleLocalNode.Verify((t) => t.PeerParentAsync(testObj));
         Assert.AreEqual(PeeringState.Connected, result.PeeringState);
         Assert.AreEqual(accessibleLocalNode.Object, result.Peer);
         Assert.IsNull(result.Exception);
      }

      [TestMethod]
      public void LocalRouterBubblesFailedPeering()
      {
         var result = testObj.PeerChildAsync(unaccessibleLocalNode.Object).Result;

         unaccessibleLocalNode.Verify((t) => t.PeerParentAsync(testObj));
         Assert.AreEqual(PeeringState.Disconnected, result.PeeringState);
         Assert.AreEqual(unaccessibleLocalNode.Object, result.Peer);
         Assert.AreEqual(unaccessibleLocalNodePeeringException, result.Exception);
      }

      [TestMethod]
      public void LocalRouterDelegatesSuccessfulPeerWithRemoteRouterWithoutErrors()
      {
         var result = testObj.PeerChildAsync(accessibleRemoteRelay.Object).Result;

         accessibleRemoteRelay.Verify((t) => t.PeerParentAsync(testObj));
         Assert.AreEqual(PeeringState.Connected, result.PeeringState);
         Assert.AreEqual(accessibleRemoteRelay.Object, result.Peer);
         Assert.IsNull(result.Exception);
      }

      [TestMethod]
      public void LocalRouterUnsuccessfulPeersWithRemoteTerminalWithoutDelegation()
      {
         var result = testObj.PeerChildAsync(unaccessibleRemoteTerminal.Object).Result;

         unaccessibleRemoteTerminal.Verify((t) => t.PeerParentAsync(testObj), Times.Never());
         Assert.AreEqual(PeeringState.Disconnected, result.PeeringState);
         Assert.AreEqual(unaccessibleRemoteTerminal.Object, result.Peer);
         Assert.IsNotNull(result.Exception);
      }

      [TestMethod]
      public void LocalRouterReflectsSuccessfullyPeeredNodesInPeersProperty()
      {
         testObj.PeerChildAsync(accessibleLocalNode.Object).Wait();
         Assert.IsTrue(testObj.Peers.Contains(accessibleLocalNode.Object));

         testObj.PeerChildAsync(accessibleRemoteRelay.Object).Wait();
         Assert.IsTrue(testObj.Peers.Contains(accessibleRemoteRelay.Object));

         Assert.AreEqual(2, testObj.Peers.Count);
      }

      [TestMethod]
      public void LocalRouterDoesNotReflectUnsuccessfullyPeeredNodesInPeersProperty()
      {
         testObj.PeerChildAsync(unaccessibleRemoteTerminal.Object).Wait();
         Assert.IsFalse(testObj.Peers.Contains(unaccessibleRemoteTerminal.Object));

         testObj.PeerChildAsync(unaccessibleLocalNode.Object).Wait();
         Assert.IsFalse(testObj.Peers.Contains(unaccessibleLocalNode.Object));

         Assert.AreEqual(0, testObj.Peers.Count);
      }
   }
}
