using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMockito;

namespace Dargon.Ipc
{
   [TestClass]
   public class NodeStubTests : NMockitoInstance
   {
      private NodeStub testObj;

      private readonly Guid guid = Guid.NewGuid();

      [TestInitialize]
      public void Setup()
      {
         testObj = new NodeStub(guid);
      }

      [TestMethod]
      public void GuidReflectsConstructorParameterTest()
      {
         AssertEquals(guid, testObj.Guid);
         VerifyNoMoreInteractions();
      }
   }
}
