using System;
using System.Threading.Tasks;
using Dargon.Ipc.Components;
using Dargon.PortableObjects;
using ItzWarty;
using NMockito;
using Xunit;

namespace Dargon.Ipc
{
   public class LocalNodeImplTest : NMockitoInstance
   {
      private LocalNodeImpl<> testObj;

      [Mock] private readonly IdentityComponent identityComponent = null;
      [Mock] private readonly PeeringBehaviorComponent peeringBehavior = null;
      [Mock] private readonly ReceivingBehaviorComponent receivingBehavior = null;
      [Mock] private readonly SendingBehaviorComponent sendingBehavior = null;
      [Mock] private readonly RoutingBehaviorComponent routingBehavior = null;
      [Mock] private readonly DiscoveryBehaviorComponent discoveryBehavior = null;

      public LocalNodeImplTest()
      {
         testObj = new LocalNodeImpl<>(identityComponent, peeringBehavior, receivingBehavior, sendingBehavior, routingBehavior, discoveryBehavior);
         VerifyNoMoreInteractions();
      }

      [Fact]
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
      
      [Fact]
      public void GuidDelegatesToIdentityComponentTest()
      {
         var guid = Guid.NewGuid();
         When(identityComponent.Guid).ThenReturn(guid);
         var result = testObj.Guid;
         AssertEquals(guid, result);
         Verify(identityComponent).Guid.Wrap();
         VerifyNoMoreInteractions();
      }
      
      [Fact]
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
      
      [Fact]
      public void SendDelegatesToSendingBehaviorComponentTest()
      {
         var otherNode = CreateUntrackedMock<INode>();
         var payload = CreateUntrackedMock<IPortableObject>();
         testObj.Send(otherNode, payload);
         Verify(sendingBehavior).Send(otherNode, payload);
         VerifyNoMoreInteractions();
      }
      
      [Fact]
      public void ReceiveDelegatesReceivingBehaviorComponentTest()
      {
         var otherNode = CreateUntrackedMock<INode>();
         var payload = CreateUntrackedMock<IEnvelope>();
         testObj.Receive(otherNode, payload);
         Verify(receivingBehavior).Receive(otherNode, payload);
         VerifyNoMoreInteractions();
      }

      [Fact]
      public void HandleRemoteNodeCreatedDelegatesToDiscoveryBehaviorComponentTest()
      {
         var otherNode = CreateUntrackedMock<INode>();
         testObj.HandleRemoteNodeCreated(otherNode);
         Verify(discoveryBehavior).HandleServiceList(otherNode);
         VerifyNoMoreInteractions();
      }
      
      [Fact]
      public void HandleRemoteNodeDestroyedDelegatesToDiscoveryBehaviorComponentTest()
      {
         var otherNode = CreateUntrackedMock<INode>();
         testObj.HandleRemoteNodeDestroyed(otherNode);
         Verify(discoveryBehavior).HandleServiceLost(otherNode);
         VerifyNoMoreInteractions();
      }
      
      [Fact]
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
