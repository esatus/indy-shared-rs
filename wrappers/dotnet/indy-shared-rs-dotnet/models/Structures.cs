using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace indy_shared_rs_dotnet.Models
{
    public class Structures
    {
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct FfiStrList
        {
            public uint count;
            public FfiStr* data;
            public static FfiStrList Create(string[] args)
            {
                FfiStrList list = new();
                list.count = (uint)args.Length;
                FfiStr[] ffiStrings = new FfiStr[list.count];
                for (int i = 0; i < args.Length; i++)
                {
                    ffiStrings[i] = FfiStr.Create(args[i]);
                }
                fixed(FfiStr* ffiStr_p = &ffiStrings[0])
                {
                    list.data = ffiStr_p;
                }
                return list;
            }

            public static FfiStrList Create(List<string> args)
            {
                return Create(args.ToArray());
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FfiStr
        {
            public IntPtr data;
            public static FfiStr Create(string arg)
            {
                FfiStr FfiString = new();
                FfiString.data = new IntPtr();
                FfiString.data = Marshal.StringToCoTaskMemUTF8(arg);
                return FfiString;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct ByteBuffer
        {
            public uint len;
            public byte* value;
        }
    }
}
