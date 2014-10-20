using Dargon.Ipc.Networking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMockito;

namespace Dargon.Ipc.Components
{
   [TestClass]
   public class RoutingBehaviorComponentImplTests : NMockitoInstance
   {
      private RoutingBehaviorComponentImpl testObj;

      [Mock] private readonly ITransportLayer transportLayer = null;
      [Mock] private readonly ILocalNodeInternal owner = null;

      [TestInitialize]
      public void Setup() 
      { 
         InitializeMocks();

         testObj = new RoutingBehaviorComponentImpl(transportLayer);
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
         Verify(transportLayer, Once()).Transport(owner, envelope);
         VerifyNoMoreInteractions();
      }
   }
}
