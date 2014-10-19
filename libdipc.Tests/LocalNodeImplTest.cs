using System;
using System.Threading.Tasks;
using Dargon.Ipc.Components;
using Dargon.PortableObjects;
using ItzWarty;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMockito;

namespace Dargon.Ipc
{
   [TestClass]
   public class LocalNodeImplTest : NMockitoInstance
   {
      private LocalNodeImpl testObj;

      [Mock] private readonly IdentityComponent identityComponent = null;
      [Mock] private readonly PeeringBehaviorComponent peeringBehavior = null;
      [Mock] private readonly ReceivingBehaviorComponent receivingBehavior = null;
      [Mock] private readonly SendingBehaviorComponent sendingBehavior = null;
      [Mock] private readonly RoutingBehaviorComponent routingBehavior = null;
      [Mock] private readonly DiscoveryBehaviorComponent discoveryBehavior = null;

      [TestInitialize]
      public void Setup()
      {
         InitializeMocks();

         testObj = new LocalNodeImpl(identityComponent, peeringBehavior, receivingBehavior, sendingBehavior, routingBehavior, discoveryBehavior);
         VerifyNoMoreInteractions();
      }

      [TestMethod]
      public void InitializeAttachesComponentsTest()
      {
         testObj.Initialize();

         Verify(identityComponent).Attach(testObj);
         Verify(peeringBehavior).Attach(testObj);
         Verify(receivingBehavior).Attach(testObj);
         Verify(sendingBehavior).Attach(testObj);
         Verify(routingBehavior).Attach(testObj);
         Verify(discoveryBehavior).Attach(testObj);
         VerifyNoMoreInteractions();
      }

      [TestMethod]
      public void GuidDelegatesToIdentityComponentTest()
      {
         var guid = Guid.NewGuid();
         When(identityComponent.Guid).ThenReturn(guid);
         var result = testObj.Guid;
         AssertEquals(guid, result);
         Verify(identityComponent).Guid.Wrap();
         VerifyNoMoreInteractions();
      }

      [TestMethod]
      public void SetParentDelegatesToPeeringBehaviorComponentTest()
      {
         var otherNode = CreateUntrackedMock<INode>();
         var peeringResultTask = new Task<IPeeringResult>(() => null);
         When(peeringBehavior.PeerParentAsync(otherNode)).ThenReturn(peeringResultTask);
         var result = testObj.SetParent(otherNode);
         AssertEquals(peeringResultTask, result);
         Verify(peeringBehavior).PeerParentAsync(otherNode);
         VerifyNoMoreInteractions();
      }

      [TestMethod]
      public void SendDelegatesToSendingBehaviorComponentTest()
      {
         var otherNode = CreateUntrackedMock<INode>();
         var payload = CreateUntrackedMock<IPortableObject>();
         testObj.Send(otherNode, payload);
         Verify(sendingBehavior).Send(otherNode, payload);
         VerifyNoMoreInteractions();
      }

      [TestMethod]
      public void ReceiveDelegatesReceivingBehaviorComponentTest()
      {
         var otherNode = CreateUntrackedMock<INode>();
         var payload = CreateUntrackedMock<IEnvelope>();
         testObj.Receive(otherNode, payload);
         Verify(receivingBehavior).Receive(otherNode, payload);
         VerifyNoMoreInteractions();
      }

      [TestMethod]
      public void HandleRemoteNodeCreatedDelegatesToDiscoveryBehaviorComponentTest()
      {
         var otherNode = CreateUntrackedMock<INode>();
         testObj.HandleRemoteNodeCreated(otherNode);
         Verify(discoveryBehavior).HandleRemoteNodeCreated(otherNode);
         VerifyNoMoreInteractions();
      }

      [TestMethod]
      public void HandleRemoteNodeDestroyedDelegatesToDiscoveryBehaviorComponentTest()
      {
         var otherNode = CreateUntrackedMock<INode>();
         testObj.HandleRemoteNodeDestroyed(otherNode);
         Verify(discoveryBehavior).HandleRemoteNodeDestroyed(otherNode);
         VerifyNoMoreInteractions();
      }

      [TestMethod]
      public void GetComponentTest()
      {
         AssertEquals(identityComponent, testObj.GetComponent<IdentityComponent>());
         AssertEquals(peeringBehavior, testObj.GetComponent<PeeringBehaviorComponent>());
         AssertEquals(receivingBehavior, testObj.GetComponent<ReceivingBehaviorComponent>());
         AssertEquals(sendingBehavior, testObj.GetComponent<SendingBehaviorComponent>());
         AssertEquals(routingBehavior, testObj.GetComponent<RoutingBehaviorComponent>());
         VerifyNoMoreInteractions();
      }
   }
}
