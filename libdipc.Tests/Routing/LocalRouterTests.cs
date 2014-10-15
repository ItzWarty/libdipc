using System;
using System.Linq;
using System.Threading.Tasks;
using Dargon.Ipc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMockito;

namespace libdipc.Tests.Routing
{
   [TestClass]
   public class LocalRouterTests : NMockitoInstance
   {
      private LocalRouter testObj;

      private readonly Guid accessibleLocalNodeGuid = Guid.NewGuid();
      [Mock] private readonly INode accessibleLocalNode = null;

      private readonly Guid unaccessibleLocalNodeGuid = Guid.NewGuid();
      [Mock] private readonly INode unaccessibleLocalNode = null;
      private readonly Exception unaccessibleLocalNodePeeringException = new Exception();

      private readonly Guid accessibleRemoteRelayGuid = Guid.NewGuid();
      [Mock] private readonly INode accessibleRemoteRelay = null;

      private readonly Guid unaccessibleRemoteTerminalGuid = Guid.NewGuid();
      [Mock] private readonly INode unaccessibleRemoteTerminal = null;
      
      [TestInitialize]
      public void Setup()
      {
         InitializeMocks();

         testObj = new LocalRouter(new LocalRouterConfiguration() { NodeIdentifier = "router1" });
         When(accessibleLocalNode.Guid).ThenReturn(accessibleLocalNodeGuid);
         When(accessibleLocalNode.Role).ThenReturn(DipRole.LocalRouter);
         When(accessibleLocalNode.PeerParentAsync(testObj)).ThenReturn(Task.FromResult<IPeeringResult>(new PeeringResult(PeeringState.Connected, testObj)));

         When(unaccessibleLocalNode.Guid).ThenReturn(unaccessibleLocalNodeGuid);
         When(unaccessibleLocalNode.Role).ThenReturn(DipRole.LocalRouter);
         When(unaccessibleLocalNode.PeerParentAsync(testObj)).ThenReturn(Task.FromResult<IPeeringResult>(new PeeringResult(PeeringState.Disconnected, testObj, unaccessibleLocalNodePeeringException)));

         When(accessibleRemoteRelay.Guid).ThenReturn(accessibleRemoteRelayGuid);
         When(accessibleRemoteRelay.Role).ThenReturn(DipRole.RemoteRouter);
         When(accessibleRemoteRelay.PeerParentAsync(testObj)).ThenReturn(Task.FromResult<IPeeringResult>(new PeeringResult(PeeringState.Connected, testObj)));

         When(unaccessibleRemoteTerminal.Guid).ThenReturn(unaccessibleRemoteTerminalGuid);
         When(unaccessibleRemoteTerminal.Role).ThenReturn(DipRole.RemoteTerminal);
      }

      [TestMethod]
      public void LocalRouterDelegatesSuccessfulPeerWithLocalNodesWithoutErrors()
      {
         var result = testObj.PeerChildAsync(accessibleLocalNode).Result;

         Verify(accessibleLocalNode).PeerParentAsync(testObj);
         Assert.AreEqual(PeeringState.Connected, result.PeeringState);
         Assert.AreEqual(accessibleLocalNode, result.Peer);
         Assert.IsNull(result.Exception);
      }

      [TestMethod]
      public void LocalRouterBubblesFailedPeering()
      {
         var result = testObj.PeerChildAsync(unaccessibleLocalNode).Result;

         Verify(unaccessibleLocalNode).PeerParentAsync(testObj);
         Assert.AreEqual(PeeringState.Disconnected, result.PeeringState);
         Assert.AreEqual(unaccessibleLocalNode, result.Peer);
         Assert.AreEqual(unaccessibleLocalNodePeeringException, result.Exception);
      }

      [TestMethod]
      public void LocalRouterDelegatesSuccessfulPeerWithRemoteRouterWithoutErrors()
      {
         var result = testObj.PeerChildAsync(accessibleRemoteRelay).Result;

         Verify(accessibleRemoteRelay).PeerParentAsync(testObj);
         Assert.AreEqual(PeeringState.Connected, result.PeeringState);
         Assert.AreEqual(accessibleRemoteRelay, result.Peer);
         Assert.IsNull(result.Exception);
      }

      [TestMethod]
      public void LocalRouterUnsuccessfulPeersWithRemoteTerminalWithoutDelegation()
      {
         var result = testObj.PeerChildAsync(unaccessibleRemoteTerminal).Result;

         Verify(unaccessibleRemoteTerminal, Never()).PeerParentAsync(testObj);
         Assert.AreEqual(PeeringState.Disconnected, result.PeeringState);
         Assert.AreEqual(unaccessibleRemoteTerminal, result.Peer);
         Assert.IsNotNull(result.Exception);
      }

      [TestMethod]
      public void LocalRouterReflectsSuccessfullyPeeredNodesInPeersProperty()
      {
         testObj.PeerChildAsync(accessibleLocalNode).Wait();
         Assert.IsTrue(testObj.Peers.Contains(accessibleLocalNode));

         testObj.PeerChildAsync(accessibleRemoteRelay).Wait();
         Assert.IsTrue(testObj.Peers.Contains(accessibleRemoteRelay));

         Assert.AreEqual(2, testObj.Peers.Count);
      }

      [TestMethod]
      public void LocalRouterDoesNotReflectUnsuccessfullyPeeredNodesInPeersProperty()
      {
         testObj.PeerChildAsync(unaccessibleRemoteTerminal).Wait();
         Assert.IsFalse(testObj.Peers.Contains(unaccessibleRemoteTerminal));

         testObj.PeerChildAsync(unaccessibleLocalNode).Wait();
         Assert.IsFalse(testObj.Peers.Contains(unaccessibleLocalNode));

         Assert.AreEqual(0, testObj.Peers.Count);
      }
   }
}
