namespace Dargon.Ipc.Components
{
   public interface RoutingBehaviorComponent : Component
   {
      void Route(INode sender, IEnvelope envelope);
   }
}
