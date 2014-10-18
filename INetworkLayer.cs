namespace Dargon.Ipc
{
   public interface INetworkLayer
   {
      void Transport(INode node, IEnvelope envelope);
   }
}
