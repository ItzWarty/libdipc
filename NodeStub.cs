using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Ipc
{
   public class NodeStub : INode
   {
      private readonly Guid guid;

      public NodeStub(Guid guid) {
         this.guid = guid;
      }

      public Guid Guid { get { return guid; } }
   }
}
