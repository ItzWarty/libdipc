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
      Terminal       = 0x00000001U,
      Router         = 0x00000002U,
      Network        = 0x00000004U,

      LocalTerminal  = Local | Terminal,
      LocalRouter    = Local | Router,
      LocalNetwork   = Local | Network,
      RemoteTerminal = Remote | Terminal,
      RemoteRouter   = Remote | Router,
      RemoteNetwork  = Remote | Network
   }
}
