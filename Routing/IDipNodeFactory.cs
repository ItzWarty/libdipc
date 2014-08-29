using System;
using Dargon.Ipc.Networking;

namespace Dargon.Ipc.Routing
{
   public interface IDipNodeFactory
   {
      IDipNode CreateLocalRouter(ILocalRouterConfiguration configuration);
      IDipNode CreateLocalhostNetwork(string hostname, Guid hostguid);
      IDipNode CreateGlobalNetwork(string namezpace);
   }

   public class DefaultDipNodeFactory : IDipNodeFactory
   {
      public IDipNode CreateLocalRouter(ILocalRouterConfiguration configuration)
      {
         return new LocalRouter(configuration);
      }

      public IDipNode CreateLocalhostNetwork(string hostname, Guid hostguid)
      {
         return new LocalhostNetwork(hostname, hostguid);
      }

      public IDipNode CreateGlobalNetwork(string namezpace)
      {
         return new GlobalNetwork(namezpace, Guid.Empty);
      }
   }
}