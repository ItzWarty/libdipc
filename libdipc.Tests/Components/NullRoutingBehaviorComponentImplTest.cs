using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMockito;

namespace Dargon.Ipc.Components
{
   [TestClass]
   public class NullRoutingBehaviorComponentImplTest : NMockitoInstance
   {
      private NullRoutingBehaviorComponentImpl testObj;

      [Mock] private readonly ILocalNodeInternal node = null;
      [Mock] private readonly IEnvelope envelope = null;

      [TestInitialize]
      public void Setup()
      {
         InitializeMocks();

         testObj = new NullRoutingBehaviorComponentImpl();
      }

      [TestMethod]
      public void AttachHasNoSideEffectsTest()
      {
         testObj.Attach(node);
         VerifyNoMoreInteractions();
      }

      [TestMethod]
      public void RouteHasNoSideEffectsTest() {
         testObj.Route(node, envelope);
         VerifyNoMoreInteractions();
      }
   }
}
