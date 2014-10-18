namespace Dargon.Ipc.Components
{
   public interface ReceivingBehaviorComponent : Component
   {
      void Receive(INode sender, IEnvelope envelope);
   }
}
