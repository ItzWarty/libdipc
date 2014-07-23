using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Transport;

namespace Dargon.Ipc.Networking
{
   public enum DTP_DIP : byte
   {
      USER_RESERVED_BEGIN     = DTP.USER_RESERVED_BEGIN,
      PASS_ENVELOPE           = USER_RESERVED_BEGIN
   }
}
