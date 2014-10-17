using System.Threading.Tasks;

namespace Dargon.Ipc.Components
{
   public interface IPeeringBehaviorComponent : IComponent
   {
      Task<IPeeringResult> PeerParentAsync(INode parent);
      Task<IPeeringResult> PeerChildAsync(INode child);
   }
}
