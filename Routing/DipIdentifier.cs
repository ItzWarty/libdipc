using System;
using ProtoBuf;

namespace Dargon.Ipc.Routing
{
   [ProtoContract]
   public interface IDipIdentifier
   {
      [ProtoMember(1)]
      Guid Guid { get; }

      // 0 for root, +1 for each downward hop
      [ProtoMember(2)]
      int Depth { get; }

      // Equivalent to writing this[int i] results to array
      [ProtoMember(3)]
      Guid[] Breadcrumbs { get; }

      // 0 - root, this[Depth] for self guid
      Guid this[int i] { get; }
   }

   public class DipIdentifier : IDipIdentifier
   {
      private IDipIdentifier m_parent;
      private readonly Guid m_guid;

      public DipIdentifier(Guid guid)
      {
         m_guid = guid;
      }

      public IDipIdentifier Parent
      {
         get { return m_parent; }
         set { m_parent = value; }
      }

      public Guid Guid { get { return m_guid; } }

      public int Depth { get { return m_parent == null ? 0 : m_parent.Depth + 1; } }
      
      public Guid[] Breadcrumbs 
      {
         get
         {
            var depth = this.Depth;
            var results = new Guid[depth + 1];
            var current = (IDipIdentifier)this;
            DipIdentifier currentAsDipIdentifier;
            while ((currentAsDipIdentifier = current as DipIdentifier) != null)
            {
               results[depth] = current.Guid;
               
               depth--;
               current = currentAsDipIdentifier.Parent;
            }
            while (depth >= 0)
            {
               results[depth] = current[depth];
               depth--;
            }
            return results;
         }
      }

      public Guid this[int i]
      {
         get
         {
            if (i == this.Depth)
               return Guid;
            else
               return Parent[i - 1];
         }
      }
   }
}
