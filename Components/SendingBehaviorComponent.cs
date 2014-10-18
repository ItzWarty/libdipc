namespace Dargon.Ipc.Components
{
   public interface SendingBehaviorComponent : Component
   {
      void Send<TPayload>(INode node, TPayload payload);
      //      void Send<T>(INode recipient, IMessage<T> message);
      //      void Send<T>(IEnvelopeV1<T> envelope);
      //      void SendV1<T>(IEnvelopeV1<T> envelope);
   }
}
