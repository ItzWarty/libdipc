using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMockito;

namespace Dargon.Ipc.Components
{
   [TestClass]
   public class IdentityComponentImplTests : NMockitoInstance
   {
      private IdentityComponentImpl testObj;

      private readonly Guid guid = Guid.NewGuid();

      [TestInitialize]
      public void Setup()
      {
         InitializeMocks();

         testObj = new IdentityComponentImpl(guid);
      }

      [TestMethod]
      public void GuidReflectsConstructorParameterTest()
      {
         AssertEquals(guid, testObj.Guid);
         VerifyNoMoreInteractions();
      }

      [TestMethod]
      public void AttachHasNoSideEffectsTest()
      {
         var node = CreateMock<ILocalNodeInternal>();
         testObj.Attach(node);
         VerifyNoMoreInteractions();
      }
   }
}
