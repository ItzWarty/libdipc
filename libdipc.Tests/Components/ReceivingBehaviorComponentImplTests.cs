using System;
using ItzWarty;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMockito;

namespace Dargon.Ipc.Components
{
   [TestClass]
   public class ReceivingBehaviorComponentImplTests : NMockitoInstance
   {
      private ReceivingBehaviorComponentImpl testObj;

      [Mock] private readonly ILocalNodeInternal node = null;
      [Mock] private readonly IEnvelope envelope = null;
      [Mock] private readonly RoutingBehaviorComponent routingBehavior = null;

      private readonly Guid guid = Guid.NewGuid();
      private readonly Guid otherGuid = Guid.NewGuid();

      [TestInitialize]
      public void Setup()
      {
         InitializeMocks();

         testObj = new ReceivingBehaviorComponentImpl();
         When(node.Guid).ThenReturn(guid);
         When(node.GetComponent<RoutingBehaviorComponent>()).ThenReturn(routingBehavior);
      }

      [TestMethod]
      public void AttachHasNoSideEffectsTest()
      {
         testObj.Attach(node);
         VerifyNoMoreInteractions();
      }

      [TestMethod]
      [ExpectedException(typeof(InvalidOperationException))]
      public void ReceiveThrowsIfUnattachedTest()
      {
         testObj.Receive(node, envelope);
         VerifyNoMoreInteractions();
      }

      [TestMethod]
      public void ReceiveDelegatesToRoutingBehaviorIfUntargetedTest()
      {
         AttachHasNoSideEffectsTest();

         When(envelope.Recipient).ThenReturn(otherGuid);
         testObj.Receive(node, envelope);
         Verify(node, Once()).Guid.Wrap();
         Verify(envelope, Once()).Recipient.Wrap();
         Verify(node, Once()).GetComponent<RoutingBehaviorComponent>();
         Verify(routingBehavior).Route(node, envelope);
         VerifyNoMoreInteractions();
      }
   }
}
