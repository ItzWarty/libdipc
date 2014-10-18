using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMockito;

namespace Dargon.Ipc.Routing
{
   [TestClass]
   public class LocalDipTerminalTests : NMockitoInstance
   {
      private LocalTerminal testObj;

      [Mock] private readonly ILocalTerminalConfiguration configuration = null;
      [Mock] private readonly INode otherTerminal = null;

      private static readonly Guid otherTerminalGuid = new Guid("372EE479-3B88-447E-9526-90F7F3578276");
      private readonly IDipIdentifier otherTerminalIdentifier = new DipIdentifier(otherTerminalGuid);
      private DateTime emptyMessageTimeSent;
      private DateTime emptyMessageTimeReceived;
      
      private IPeeringResult succeededPeeringResult;
      private IPeeringResult failedPeeringResult;
      private IMessage<object> emptyMessageFromTestObj;
      private IEnvelopeV1<object> emptyMessageFromTestObjEnvelope;

      [TestInitialize]
      public void Setup()
      {
         InitializeMocks();
         testObj = new LocalTerminal(configuration);

         succeededPeeringResult = new PeeringResult(PeeringState.Connected, testObj);
         failedPeeringResult = new PeeringResult(PeeringState.Disconnected, testObj);
         When(otherTerminal.PeerChildAsync(testObj)).ThenReturn(Task.FromResult(succeededPeeringResult));
         When(otherTerminal.Guid).ThenReturn(otherTerminalGuid);
         
         emptyMessageTimeSent = DateTime.UtcNow;
         emptyMessageTimeReceived = emptyMessageTimeSent + TimeSpan.FromSeconds(5);
         emptyMessageFromTestObj = new Message<object>(new object());
         emptyMessageFromTestObjEnvelope = new EnvelopeV1<object>(testObj.Identifier, otherTerminalIdentifier, new[] { otherTerminalGuid }, emptyMessageTimeSent, emptyMessageTimeReceived, emptyMessageFromTestObj);
      }

      [TestMethod]
      public void LocalTerminalRefusesParentingPeers()
      {
         var result = testObj.PeerChildAsync(otherTerminal).Result;
         Assert.AreEqual(PeeringState.Disconnected, result.PeeringState);
         Assert.IsNotNull(result.Exception);
      }
   }
}
