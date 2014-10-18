using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMockito;

namespace Dargon.Ipc.Components
{
   [TestClass]
   public class RoutingBehaviorComponentImplTests : NMockitoInstance
   {
      private RoutingBehaviorComponentImpl testObj;

      [Mock] private readonly INetworkLayer networkLayer = null;
      [Mock] private readonly ILocalNodeInternal owner = null;

      [TestInitialize]
      public void Setup() 
      { 
         InitializeMocks();

         testObj = new RoutingBehaviorComponentImpl(networkLayer);
      }

      [TestMethod]
      public void AttachHasNoVisibleSideEffects()
      {
         testObj.Attach(owner);
         VerifyNoMoreInteractions();
      }

      [TestMethod]
      public void RouteDelegatesToNetworkLayerTest()
      {
         AttachHasNoVisibleSideEffects();

         var node = CreateMock<INode>();
         var envelope = CreateMock<IEnvelope>();
         testObj.Route(node, envelope);
         Verify(networkLayer, Once()).Transport(owner, envelope);
         VerifyNoMoreInteractions();
      }
   }
}
