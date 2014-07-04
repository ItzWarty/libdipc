using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Ipc
{
   public class LocalRouterConfiguration : ILocalRouterConfiguration
   {
      public string NodeIdentifier { get; set; }
      public Guid Guid { get; set; }
   }
}
