namespace Dargon.Ipc.Components
{
   public class NullRoutingBehaviorComponentImpl : RoutingBehaviorComponent
   {
      public void Attach(ILocalNodeInternal node) { }

      public void Route(INode sender, IEnvelope envelope) { }
   }
}