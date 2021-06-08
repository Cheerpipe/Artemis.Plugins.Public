using System.Runtime.InteropServices;

namespace SRTPluginProviderRE8.Structs.GameStructs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]

    public unsafe struct GameStates
    {
        [FieldOffset(0x40)] public uint GameState;
        [FieldOffset(0x48)] public byte PauseState;

        public static GameStates AsStruct(byte[] data)
        {
            fixed (byte* pb = &data[0])
            {
                return *(GameStates*)pb;
            }
        }
    }
}