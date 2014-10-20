using System;
using System.IO;
using Dargon.Transport;
using ItzWarty;

namespace Dargon.Ipc.Networking.Handlers
{
   public class AdvertiseServicesRith : RemotelyInitializedTransactionHandler
   {
      private readonly DiscoveryEndpointServer discoveryEndpoint;

      public AdvertiseServicesRith(uint transactionId, DiscoveryEndpointServer discoveryEndpoint) : base(transactionId) { this.discoveryEndpoint = discoveryEndpoint; }

      public override void ProcessInitialMessage(IDSPExSession session, TransactionInitialMessage message)
      {
         using (var ms = new MemoryStream(message.DataBuffer, message.DataOffset, message.DataLength))
         using (var reader = new BinaryReader(ms))
         {
            var nodeGuid = reader.ReadGuid();
            var serviceCount = reader.ReadUInt32();
            var serviceGuids = Util.Generate((int)serviceCount, i => reader.ReadGuid());

            discoveryEndpoint.HandleServiceAdvertisement(nodeGuid, serviceGuids);

            session.DeregisterRITransactionHandler(this);
         }
      }

      public override void ProcessMessage(IDSPExSession session, TransactionMessage message) { throw new NotImplementedException(); }
   }
}