using System;

namespace Dargon.Ipc.OldNetworking
{
   public interface INetworkBootstrapConfiguration
   {
      ILocalRouterConfiguration RouterConfiguration { get; }
      string HostName { get; }
      Guid HostGuid { get; }
      bool IsHostRepresentative { get; }
      string Namespace { get; }
   }
}