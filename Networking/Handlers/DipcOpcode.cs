using Dargon.Transport;

namespace Dargon.Ipc.Networking.Handlers
{
   public enum DipcOpcode : byte
   {
      USER_RESERVED_BEGIN  = DTP.USER_RESERVED_BEGIN,
      AdvertiseServices    = USER_RESERVED_BEGIN,
      EnumerateServices    = USER_RESERVED_BEGIN + 1
   }
}