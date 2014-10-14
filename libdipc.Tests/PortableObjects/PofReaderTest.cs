using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dargon.PortableObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMockito;

namespace libdipc.Tests.PortableObjects
{
   [TestClass]
   public class PofReaderTest : NMockitoInstance
   {
      private PofReader testObj;
      [Mock] private IPofContext context;
      [Mock] private ISlotSource slotSource;

      [TestInitialize]
      public void Setup()
      {
         InitializeMocks();
         testObj = new PofReader(context, slotSource);
      }

      [TestMethod]
      public void TestHappyPath()
      {
         Assert.IsNotNull(context);
         Assert.IsNotNull(slotSource);

         var arr = new byte[4];
         When(slotSource.GetSlot(0)).ThenReturn(null);
         When(slotSource.GetSlot(1)).ThenReturn(arr);
         Assert.IsNull(slotSource.GetSlot(0));
         Assert.AreEqual(arr, slotSource.GetSlot(1));

         Verify(slotSource).GetSlot(0);
      }

      private void Herp<TMockInterface, TResult>(Expression<Func<TMockInterface, TResult>> expr)
      {
         Console.WriteLine(expr.Body.GetType());
         Console.WriteLine(((MethodCallExpression)expr.Body).Object);
         Console.WriteLine(((MethodCallExpression)expr.Body).Object.GetType());
         Console.WriteLine(((ParameterExpression)((MethodCallExpression)expr.Body).Object).Name);
         Console.WriteLine(((MethodCallExpression)expr.Body).Method);
         Console.WriteLine(((MethodCallExpression)expr.Body).Arguments);
      }
   }
}
