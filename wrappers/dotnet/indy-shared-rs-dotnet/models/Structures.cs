using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace indy_shared_rs_dotnet.Models
{
    public class Structures
    {

        /**
        [StructLayout(LayoutKind.Sequential)]
        public struct FfiStrList
        {
            public uint count;
            public FfiStr[] data;
            public static FfiStrList Create(string[] args)
            {
                FfiStrList list = new();
                list.count = (uint)args.Length;
                list.data = new FfiStr[list.count];
                for (int i = 0; i < args.Length; i++)
                {
                    list.data[i] = FfiStr.Create(args[i]);
                }
                return list;
            }
            public static FfiStrList Create(List<string> args)
            {
                return Create(args.ToArray());
            }
        }**/

        
        [StructLayout(LayoutKind.Sequential)]
        public struct FfiStrList
        {
            public uint count;
            public IntPtr[] data;
            public static FfiStrList Create(string[] args)
            {
                FfiStrList list = new();
                list.count = (uint)args.Length;
                list.data = new IntPtr[list.count];
                for (int i = 0; i < args.Length; i++)
                {
                    list.data[i] = Marshal.StringToCoTaskMemUTF8(args[i]);
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

        /**
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct FfiStr
        {
            public char* data;
        }
        /** aus Rust
            #[repr(transparent)]
            pub struct FfiStr<'a> {
            cstr: *const c_char,
            _boo: PhantomData<&'a ()>,
}
         **/

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct ByteBuffer
        {
            public uint len;
            public byte* value;
        }
    }
}
