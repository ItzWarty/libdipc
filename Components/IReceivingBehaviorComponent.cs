namespace Dargon.Ipc.Components
{
   public interface IReceivingBehaviorComponent : IComponent
   {
      void Receive(INode sender, IEnvelope envelope);
   }
}
