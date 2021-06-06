using System.Runtime.InteropServices;

namespace SRTPluginProviderRE8.Structs.GameStructs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]

    public unsafe struct GameRank
    {
        [FieldOffset(0x70)] public int Score;
        [FieldOffset(0x74)] public int Rank;

        public static GameRank AsStruct(byte[] data)
        {
            fixed (byte* pb = &data[0])
            {
                return *(GameRank*)pb;
            }
        }
    }
}