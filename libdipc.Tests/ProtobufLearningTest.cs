using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoBuf;

namespace libdipc.Tests
{
   [TestClass]
   public class ProtobufLearningTest
   {
      [TestInitialize]
      public void Setup()
      {  
      }

      [TestMethod]
      public void RunProtobufLearningTestByteArraySerialization()
      {
         var ms = new MemoryStream();
         var data = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
         Serializer.Serialize<byte[]>(ms, data);
         ms.Position = 0;
         var nextData = Serializer.Deserialize<byte[]>(ms); // doesn't work with vague object
         Console.WriteLine(nextData.GetType());
         Console.WriteLine(nextData);
      }

      [TestMethod]
      public void RunProtobufLearningTestGenericsSerialization()
      {
         var ms = new MemoryStream();
         var data = new TestGenericClass<string>();
         Serializer.Serialize<ITestGenericClass<string>>(ms, data);
         ms.Position = 0;
         var nextData = Serializer.Deserialize<ITestGenericClass<string>>(ms); // doesn't work with vague object
         Console.WriteLine(nextData.GetType());
         Console.WriteLine(nextData);
      }

      private interface ITestGenericClass<T> where T : class
      {
         [ProtoMember(1)]
         byte[] Values { get; set; }

         [ProtoMember(2)]
         T Herp { get; set; }
      }

      [ProtoContract]
      private class TestGenericClass<T> : ITestGenericClass<T> where T : class
      {
         [ProtoMember(1)]
         public byte[] Values { get; set; }

         [ProtoMember(2)]
         public T Herp { get; set; }
      }
   }
}
