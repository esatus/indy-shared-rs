using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

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

            public static ByteBuffer Create(string json)
            {
                UTF8Encoding decoder = new UTF8Encoding(true, true);
                byte[] bytes = new byte[json.Length];
                decoder.GetBytes(json, 0, json.Length, bytes, 0);
                ByteBuffer buffer = new();
                buffer.len = (uint)json.Length;
                fixed (byte* bytebuffer_p = &bytes[0]) 
                {
                    buffer.value = bytebuffer_p;
                }
                return buffer;
            }
        }

        /**
        [StructLayout(LayoutKind.Sequential)]
        public struct FfiCredRevInfo
        {
            public uint regDefHandle;
            public uint regDefPvtHandle;
            public uint registry;
            public long regIdx;
            public FfiLongList regUsed;
            public FfiStr tailsPath;

        }
        **/
    }
}
