using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Ipc
{
   public interface ITerminalConfiguration
   {
      string NodeIdentifier { get; set; }
   }

   public class TerminalConfiguration : ITerminalConfiguration
   {
      public string NodeIdentifier { get; set; }
   }
}
