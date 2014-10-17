﻿using Dargon.Transport;

namespace Dargon.Ipc
{
   public interface INetworkLayerFactory
   {
      INetworkLayer Create();
   }

   class NetworkLayerFactory : INetworkLayerFactory
   {
      private readonly IDtpNodeFactory dtpNodeFactory;

      public NetworkLayerFactory() : this(new DefaultDtpNodeFactory()) { }

      public NetworkLayerFactory(IDtpNodeFactory dtpNodeFactory) { this.dtpNodeFactory = dtpNodeFactory; }

      public INetworkLayer Create() { return new NetworkLayer(dtpNodeFactory); }
   }

   public class NetworkLayer : INetworkLayer
   {
      public NetworkLayer(IDtpNodeFactory dtpNodeFactory) { var node = dtpNodeFactory.CreateOrConnectNode("dargon_ipc"); }

      public void Transport(IEnvelope envelope) { throw new System.NotImplementedException(); }
   }
}
