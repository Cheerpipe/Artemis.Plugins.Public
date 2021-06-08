using System.Runtime.InteropServices;

namespace SRTPluginProviderRE8.Structs.GameStructs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]

    public unsafe struct GameInventoryItem
    {
        [FieldOffset(0x31)] public byte IsTemporary;
        [FieldOffset(0x34)] public int SortOrder;
        [FieldOffset(0x38)] public byte IsUsing;
        [FieldOffset(0x3C)] public uint ItemId;
        [FieldOffset(0x40)] public uint ItemCategoryHash;
        [FieldOffset(0x44)] public int SlotNo;
        [FieldOffset(0x48)] public uint QuickSlotHash;
        [FieldOffset(0x4C)] public int StackSize;
        [FieldOffset(0x50)] public byte IsHorizontal;
        [FieldOffset(0x54)] public uint IncludeItemID;
        [FieldOffset(0x58)] public int IncludeStackSize;
        [FieldOffset(0x5C)] public uint IncludeItemIDSub;
        [FieldOffset(0x60)] public int IncludeStackSizeSub;
        [FieldOffset(0x64)] public byte IsHidden;

        public static GameInventoryItem AsStruct(byte[] data)
        {
            fixed (byte* pb = &data[0])
            {
                return *(GameInventoryItem*)pb;
            }
        }
    }
}