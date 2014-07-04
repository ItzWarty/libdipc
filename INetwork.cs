using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Ipc
{
   public interface INetwork
   {
      Guid Guid { get; }
      void AddEdge(IDipNode peer1, IDipNode peer2);
      void Route(IDipNode source, IDipNode destination);
   }
}
