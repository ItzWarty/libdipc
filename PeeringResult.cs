using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Ipc
{
   public interface IPeeringResult
   {
      PeeringState PeeringState { get; }
      IDipNode Peer { get; }
      Exception Exception { get; }
   }

   public class PeeringResult : IPeeringResult
   {
      public PeeringState PeeringState { get; private set; }
      public IDipNode Peer { get; private set; }
      public Exception Exception { get; private set; }

      public PeeringResult(PeeringState peeringState, IDipNode peer = null, Exception exception = null)
      {
         PeeringState = peeringState;
         Peer = peer;
         Exception = exception;
      }
   }
}
