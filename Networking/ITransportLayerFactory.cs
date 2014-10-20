using Dargon.Ipc.Networking;
using Dargon.Transport;

namespace Dargon.Ipc
{
   public interface ITransportLayerFactory
   {
      ITransportLayer Create();
   }

   class TransportLayerFactory : ITransportLayerFactory
   {
      private readonly IDtpNodeFactory dtpNodeFactory;

      public TransportLayerFactory() : this(new DefaultDtpNodeFactory()) { }

      public TransportLayerFactory(IDtpNodeFactory dtpNodeFactory) { this.dtpNodeFactory = dtpNodeFactory; }

      public ITransportLayer Create() { return new TransportLayer(dtpNodeFactory); }
   }

   public class TransportLayer : ITransportLayer
   {
      public TransportLayer(IDtpNodeFactory dtpNodeFactory) { var node = dtpNodeFactory.CreateNode(NodeRole.ServerOrClient, 1337); }

      public void Transport(INode node, IEnvelope envelope) { throw new System.NotImplementedException(); }
   }
}
