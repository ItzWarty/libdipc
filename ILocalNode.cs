using System.Threading.Tasks;
using Dargon.PortableObjects;

namespace Dargon.Ipc
{
   public interface ILocalNode : INode
   {
      Task<IPeeringResult> Connect(INode node);
      void Send<TPayload>(INode node, TPayload payload) where TPayload : IPortableObject;
   }
}