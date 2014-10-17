using Dargon.Ipc.Components;
using Dargon.PortableObjects;

namespace Dargon.Ipc
{
   public interface ILocalNodeInternal : ILocalNode
   {
      void Receive(INode sender, IEnvelope envelope);
      T GetComponent<T>() where T : IComponent;
   }
}