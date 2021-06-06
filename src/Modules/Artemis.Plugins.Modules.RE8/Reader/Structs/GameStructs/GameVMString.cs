using System.Runtime.InteropServices;

namespace SRTPluginProviderRE8.Structs.GameStructs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]

    public unsafe struct GameVMString
    {
        //[FieldOffset(0x10)] public int Length;
        //[FieldOffset(0x14)] public string StringByteArray;
        //
        //public static GameVMString AsStruct(byte[] data)
        //{
        //    fixed (byte* pb = &data[0])
        //    {
        //        return *(GameVMString*)pb;
        //    }
        //}
    }
}