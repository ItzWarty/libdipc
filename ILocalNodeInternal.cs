using Dargon.Ipc.Components;
using Dargon.PortableObjects;

namespace Dargon.Ipc
{
   public interface ILocalNodeInternal : ILocalNode
   {
      void HandleRemoteNodeCreated(INode node);
      void HandleRemoteNodeDestroyed(INode node);
      void Receive(INode sender, IEnvelope envelope);
      T GetComponent<T>() where T : Component;
   }
}