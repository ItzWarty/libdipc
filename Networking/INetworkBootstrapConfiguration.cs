using System;
using Dargon.Ipc.Routing;

namespace Dargon.Ipc.Networking
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