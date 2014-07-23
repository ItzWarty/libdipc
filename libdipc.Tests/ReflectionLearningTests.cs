using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace libdipc.Tests
{
   [TestClass]
   public class ReflectionLearningTests
   {
      [TestInitialize]
      public void Setup()
      {
      }

      [TestMethod]
      public void GenericTypeDefinitionIsAThing()
      {
         var type = typeof(GenericClass<,>);
         Assert.IsTrue(type.IsGenericType);
         Assert.IsTrue(type.IsGenericTypeDefinition);
         Assert.IsFalse(type.IsGenericParameter);
         //foreach (var argument in type.GetGenericArguments())
         //   Console.WriteLine("> " + argument);
      }

      [TestMethod]
      public void GenericTypeDefinitionIsntAlwaysTrue()
      {
         var type = typeof(GenericClass<int, float>);
         Assert.IsTrue(type.IsGenericType);
         Assert.IsFalse(type.IsGenericTypeDefinition);
         Assert.IsFalse(type.IsGenericParameter);
         //foreach (var argument in type.GetGenericArguments())
         //   Console.WriteLine("> " + argument);
      }

      [TestMethod]
      public void Run()
      {
         var a = new GenericClass<List<int>, float>();
      }
   }

   public class GenericClass<TParam1, TParam2>
   {
      public TParam1 Field1 { get; set; }
      public TParam2 Field2 { get; set; }
      public Tuple<TParam1, TParam2> CombinationField1 { get; set; }
   }
}
