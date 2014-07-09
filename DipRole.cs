using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Ipc
{
   [Flags]
   public enum DipRole : uint
   {
      Local          = 0x00010000U,
      Remote         = 0x00020000U,
      Host           = 0x00040000U,
      Global         = 0x00080000U,
      
      Terminal       = 0x00000001U,
      Router         = 0x00000002U,
      Network        = 0x00000004U,

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
