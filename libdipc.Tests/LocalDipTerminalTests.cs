using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dargon.Ipc;
using Dargon.Ipc.Messaging;
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
      private static Guid otherTerminalGuid = new Guid("372EE479-3B88-447E-9526-90F7F3578276");
      private IDipIdentifier otherTerminalIdentifier = new DipIdentifier(otherTerminalGuid);
      private Mock<IDipNode> otherTerminal = new Mock<IDipNode>();
      private DateTime emptyMessageTimeSent;
      private DateTime emptyMessageTimeReceived;
      
      private IPeeringResult succeededPeeringResult;
      private IPeeringResult failedPeeringResult;
      private IMessage<object> emptyMessageFromTestObj;
      private IEnvelopeV1<object> emptyMessageFromTestObjEnvelope;

      [TestInitialize]
      public void Setup()
      {
         testObj = new LocalTerminal(configuration.Object);

         succeededPeeringResult = new PeeringResult(PeeringState.Connected, testObj);
         failedPeeringResult = new PeeringResult(PeeringState.Disconnected, testObj);
         otherTerminal.Setup((t) => t.PeerChildAsync(testObj)).Returns(Task.FromResult(succeededPeeringResult));
         otherTerminal.Setup((t) => t.Guid).Returns(otherTerminalGuid);
         
         emptyMessageTimeSent = DateTime.UtcNow;
         emptyMessageTimeReceived = emptyMessageTimeSent + TimeSpan.FromSeconds(5);
         emptyMessageFromTestObj = new Message<object>(new object());
         emptyMessageFromTestObjEnvelope = new EnvelopeV1<object>(testObj.Identifier, otherTerminalIdentifier, new[] { otherTerminalGuid }, emptyMessageTimeSent, emptyMessageTimeReceived, emptyMessageFromTestObj);
      }

      [TestMethod]
      public void LocalTerminalRefusesParentingPeers()
      {
         var result = testObj.PeerChildAsync(otherTerminal.Object).Result;
         Assert.AreEqual(PeeringState.Disconnected, result.PeeringState);
         Assert.IsNotNull(result.Exception);
      }
   }
}
