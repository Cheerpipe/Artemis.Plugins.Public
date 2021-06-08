using System.Runtime.InteropServices;

namespace SRTPluginProviderRE8.Structs.GameStructs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]

    public unsafe struct GameItemCustomParams
    {
        [FieldOffset(0x10)] public float Power;
        [FieldOffset(0x18)] public float Rate;
        [FieldOffset(0x20)] public float Reload;
        [FieldOffset(0x28)] public int StackSize;

        public static GameItemCustomParams AsStruct(byte[] data)
        {
            fixed (byte* pb = &data[0])
            {
                return *(GameItemCustomParams*)pb;
            }
        }
    }
}