using System;

namespace Dargon.Ipc.Routing
{
   [Flags]
   public enum DipRole : uint
   {
      Local          = 0x00000001U,
      Remote         = 0x00000002U,
      Host           = 0x00000004U,
      Global         = 0x00000008U,
      
      Terminal       = 0x00000100U,
      Router         = 0x00000200U,
      Network        = 0x00000400U,

      Service        = 0x00010000U,

      LocalTerminal  = Local | Terminal,
      LocalRouter    = Local | Router,
      HostTerminal   = Host | Terminal,
      HostRouter     = Host | Router,
      HostNetwork    = Host | Network,
      RemoteTerminal = Remote | Terminal,
      RemoteRouter   = Remote | Router,
      RemoteNetwork  = Remote | Network,
      GlobalNetwork  = Global | Network
   }
}
