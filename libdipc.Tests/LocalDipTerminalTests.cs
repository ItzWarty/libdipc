using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dargon.Ipc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Sequences;
using System.Linq;

namespace libdipc.Tests
{
   [TestClass]
   public class LocalDipTerminalTests
   {
      private LocalTerminal testObj;

      private Mock<ITerminalConfiguration> configuration = new Mock<ITerminalConfiguration>();
      private Mock<IDipNode> otherTerminal = new Mock<IDipNode>();
      
      private IPeeringResult succeededPeeringResult;
      private IPeeringResult failedPeeringResult;
      private IDipMessage<EmptyContentV1> emptyMessageFromTestObj;

      [TestInitialize]
      public void Setup()
      {
         testObj = new LocalTerminal(configuration.Object);

         succeededPeeringResult = new PeeringResult(PeeringState.Connected, testObj);
         failedPeeringResult = new PeeringResult(PeeringState.Disconnected, testObj);
         otherTerminal.Setup((t) => t.Peer(testObj)).Returns(Task.FromResult(succeededPeeringResult));

         emptyMessageFromTestObj = new DipMessage<EmptyContentV1>(testObj, otherTerminal.Object, new EmptyContentV1());
      }

      [TestMethod]
      public void LocalTerminalIdentifierReflectsConfiguration()
      {
         var identifiers = new Queue<string>(new []{"", "blah"});
         configuration.Setup((c) => c.NodeIdentifier).Returns(identifiers.Dequeue);

         Assert.AreEqual("", testObj.Identifier);
         Assert.AreEqual("blah", testObj.Identifier);

         configuration.Verify((c) => c.NodeIdentifier, Times.Exactly(2));
      }

      [TestMethod]
      public async Task LocalTerminalPeeringAddsPeerToNode()
      {
         var result = await testObj.Peer(otherTerminal.Object);

         Assert.IsTrue(testObj.Peers.Contains(otherTerminal.Object));
         Assert.IsTrue(result.PeeringState == succeededPeeringResult.PeeringState);
      }

      [TestMethod]
      public async Task LocalDipTerminalDelegatesSendMessage()
      {
         await testObj.Peer(otherTerminal.Object);

         testObj.SendMessage(otherTerminal.Object, emptyMessageFromTestObj);

         otherTerminal.Verify((t) => t.ReceiveMessage(testObj, emptyMessageFromTestObj));
      }

      private class EmptyContentV1 : ISerializable
      {
         public void GetObjectData(SerializationInfo info, StreamingContext context)
         {
         }
      }
   }
}
