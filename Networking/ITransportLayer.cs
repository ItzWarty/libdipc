namespace Dargon.Ipc.Networking
{
   public interface ITransportLayer
   {
      void Transport(INode node, IEnvelope envelope);
   }
}
