namespace Dargon.Ipc
{
   public interface INetworkLayer
   {
      void Transport(IEnvelope envelope);
   }
}
