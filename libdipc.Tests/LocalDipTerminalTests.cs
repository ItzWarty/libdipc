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

      private Mock<ILocalTerminalConfiguration> configuration = new Mock<ILocalTerminalConfiguration>();
      private Guid otherTerminalGuid = new Guid("372EE479-3B88-447E-9526-90F7F3578276");
      private Mock<IDipNode> otherTerminal = new Mock<IDipNode>();
      private DateTime emptyMessageTimeSent;
      private DateTime emptyMessageTimeReceived;
      
      private IPeeringResult succeededPeeringResult;
      private IPeeringResult failedPeeringResult;
      private IMessage<EmptyContentV1> emptyMessageFromTestObj;
      private IEnvelopeV1<EmptyContentV1> emptyMessageFromTestObjEnvelope;

      [TestInitialize]
      public void Setup()
      {
         testObj = new LocalTerminal(configuration.Object);

         succeededPeeringResult = new PeeringResult(PeeringState.Connected, testObj);
         failedPeeringResult = new PeeringResult(PeeringState.Disconnected, testObj);
         otherTerminal.Setup((t) => t.PeerAsync(testObj)).Returns(Task.FromResult(succeededPeeringResult));
         otherTerminal.Setup((t) => t.Guid).Returns(otherTerminalGuid);
         
         emptyMessageTimeSent = DateTime.UtcNow;
         emptyMessageTimeReceived = emptyMessageTimeSent + TimeSpan.FromSeconds(5);
         emptyMessageFromTestObj = new Message<EmptyContentV1>(new EmptyContentV1());
         emptyMessageFromTestObjEnvelope = new EnvelopeV1<EmptyContentV1>(testObj.Guid, otherTerminalGuid, new[] { otherTerminalGuid }, emptyMessageTimeSent, emptyMessageTimeReceived, emptyMessageFromTestObj);
      }

      [TestMethod]
      public void LocalTerminalIdentifierReflectsConfiguration()
      {
         configuration.ResetCalls();

         var identifiers = new Queue<string>(new []{"", "blah"});
         configuration.Setup((c) => c.NodeIdentifier).Returns(identifiers.Dequeue);

         Assert.AreEqual("", new LocalTerminal(configuration.Object).Identifier);
         Assert.AreEqual("blah", new LocalTerminal(configuration.Object).Identifier);

         configuration.Verify((c) => c.NodeIdentifier, Times.Exactly(2));
      }

      [TestMethod]
      public async Task LocalTerminalPeeringAddsPeerToNode()
      {
         var result = await testObj.PeerAsync(otherTerminal.Object);

         Assert.IsTrue(testObj.Peers.Contains(otherTerminal.Object));
         Assert.IsTrue(result.PeeringState == succeededPeeringResult.PeeringState);
      }

      [TestMethod]
      public async Task LocalDipTerminalDelegatesSendMessage()
      {
         otherTerminal.ResetCalls();
         testObj.PeerAsync(otherTerminal.Object).Wait();
         testObj.Send(otherTerminal.Object, emptyMessageFromTestObj);
         otherTerminal.Object.Receive(EnvelopeFactory.NewDirectEnvelopeFromContent(testObj, otherTerminal.Object, emptyMessageFromTestObj));
         otherTerminal.Verify((t) => t.Receive(It.Is<IEnvelopeV1<EmptyContentV1>>(e => e.Message == emptyMessageFromTestObj)));
      }

      private class EmptyContentV1 : ISerializable
      {
         public void GetObjectData(SerializationInfo info, StreamingContext context)
         {
         }
      }
   }
}
