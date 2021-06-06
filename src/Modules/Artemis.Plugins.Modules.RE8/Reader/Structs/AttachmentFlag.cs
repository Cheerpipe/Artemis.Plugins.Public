using System;

namespace SRTPluginProviderRE8.Structs
{
    [Flags]
    public enum AttachmentsFlag : int
    {
        None = 0x00,
        First = 0x01, // Weapons that never occupy two-slots will not have this flag for attachments.
        Second = 0x02,
        Third = 0x04
    }
}