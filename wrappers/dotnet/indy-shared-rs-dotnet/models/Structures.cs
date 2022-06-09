using indy_shared_rs_dotnet.indy_credx;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace indy_shared_rs_dotnet.Models
{
    public class Structures
    {
        
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct FfiStrList2
        {
            public uint count;
            public IntPtr* data;
            public static FfiStrList2 Create(string[] args)
            {
                FfiStrList2 list = new();
                list.count = (uint)args.Length;
                IntPtr[] ffiStrings = new IntPtr[list.count];
                for (int i = 0; i < args.Length; i++)
                {
                    ffiStrings[i] = Marshal.StringToCoTaskMemUTF8(args[i]);
                }

                fixed (IntPtr* ffiStr_p = &ffiStrings[0])
                {
                    list.data = ffiStr_p;
                }
                return list;
            }

            public static FfiStrList2 Create(List<string> args)
            {
                return Create(args.ToArray());
            }
        }

        /**
        //Note: This version doesnt work in credx_create_credential -> Access Memory issue
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct FfiStrList2
        {
            public uint count;
            public IntPtr data;
            public static FfiStrList2 Create(string[] args)
            {
                FfiStrList2 list = new();
                list.count = (uint)args.Length;
                list.data = new IntPtr();
                list.data = Marshal.AllocHGlobal(args.Length * sizeof(IntPtr));
                IntPtr[] ffiStrings = new IntPtr[list.count];
                for (int i = 0; i < args.Length; i++)
                {
                    ffiStrings[i] = Marshal.StringToCoTaskMemUTF8(args[i]);
                }
                Marshal.Copy(ffiStrings, 0, list.data, args.Length);

                //Debug for 3 entries in args
                UTF8Encoding decoder = new UTF8Encoding(true, true);
                IntPtr debugPtr0 = Marshal.ReadIntPtr(list.data, 0* sizeof(IntPtr));
                IntPtr debugPtr1 = Marshal.ReadIntPtr(list.data, 1* sizeof(IntPtr));
                IntPtr debugPtr2 = Marshal.ReadIntPtr(list.data, 2* sizeof(IntPtr));
                byte[] debugBytes0 = new byte[args[0].Length];
                byte[] debugBytes1 = new byte[args[1].Length];
                byte[] debugBytes2 = new byte[args[2].Length];
                
                for (int i = 0; i < args[0].Length; i++)
                {
                    debugBytes0[i] = Marshal.ReadByte(debugPtr0, i);
                }
                string debugStr0 = decoder.GetString(debugBytes0, 0, args[0].Length);

                for (int i = 0; i < args[1].Length; i++)
                {
                    debugBytes1[i] = Marshal.ReadByte(debugPtr1, i);
                }
                string debugStr1 = decoder.GetString(debugBytes1, 0, args[1].Length);

                for (int i = 0; i < args[2].Length; i++)
                {
                    debugBytes2[i] = Marshal.ReadByte(debugPtr2, i);
                }
                string debugStr2 = decoder.GetString(debugBytes2, 0, args[2].Length);
                string[] debugArray = { debugStr0, debugStr1, debugStr2 };
                return list;
            }

            public static FfiStrList2 Create(List<string> args)
            {
                return Create(args.ToArray());
            }
        }**/

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
                fixed (FfiStr* ffiStr_p = &ffiStrings[0])
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

            // Todo 
            //public static void Free(ByteBuffer buffer)
            //{
            //    NativeMethods.credx_buffer_free(buffer);
            //}
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FfiCredRevInfo
        {
            public uint regDefObjectHandle;
            public uint regDefPvtObjectHandle;
            public uint registryObjectHandle;
            public long regIdx;
            public FfiLongList regUsed;
            public FfiStr tailsPath;

            public static FfiCredRevInfo Create(CredentialRevocationInfo entry)
            {
                if(entry != null)
                {
                    FfiCredRevInfo result = new();
                    result.regDefObjectHandle = entry.regDefObjectHandle;
                    result.regDefPvtObjectHandle = entry.regDefPvtObjectHandle;
                    result.registryObjectHandle = entry.registryObjectHandle;
                    result.regIdx = entry.regIdx;
                    result.regUsed = FfiLongList.Create(entry.regUsed);
                    result.tailsPath = FfiStr.Create(entry.tailsPath);
                    return result;
                }
                else
                {
                    return new();
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct FfiLongList
        {
            public uint count;
            public long* data;
            public static FfiLongList Create(long[] args)
            {
                FfiLongList list = new();
                list.count = (uint)args.Length;
                fixed (long* uintP = &args[0])
                {
                    list.data = uintP;
                }
                return list;
            }

            public static FfiLongList Create(List<long> args)
            {
                return Create(args.ToArray());
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FfiCredentialEntry
        {
            public uint CredentialObjectHandle;
            public long Timestamp;
            public uint RevStateObjectHandle;

            public static FfiCredentialEntry Create(CredentialEntry entry)
            {
                FfiCredentialEntry result = new();
                result.CredentialObjectHandle = entry.CredentialObjectHandle;
                result.Timestamp = entry.Timestamp;
                result.RevStateObjectHandle = entry.RevStateObjectHandle;
                return result;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FfiCredentialProve
        {
            public long EntryIndex;
            public FfiStr Referent;
            public byte IsPredicate;
            public byte Reveal;

            public static FfiCredentialProve Create(CredentialProve prove)
            {
                FfiCredentialProve result = new();
                result.EntryIndex = prove.EntryIndex;
                result.Referent = FfiStr.Create(prove.Referent);
                result.IsPredicate = prove.IsPredicate;
                result.Reveal = prove.Reveal;
                return result;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct FfiCredentialEntryList
        {
            public uint count;
            public FfiCredentialEntry* data;
            public static FfiCredentialEntryList Create(CredentialEntry[] args)
            {
                FfiCredentialEntryList list = new();
                list.count = (uint)args.Length;
                FfiCredentialEntry[] ffiCredentialEntries = new FfiCredentialEntry[list.count];
                for (int i = 0; i < args.Length; i++)
                {
                    ffiCredentialEntries[i] = FfiCredentialEntry.Create(args[i]);
                }
                fixed (FfiCredentialEntry* ffiEntryP = &ffiCredentialEntries[0])
                {
                    list.data = ffiEntryP;
                }
                return list;
            }

            public static FfiCredentialEntryList Create(List<CredentialEntry> args)
            {
                return Create(args.ToArray());
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct FfiCredentialProveList
        {
            public uint count;
            public FfiCredentialProve* data;
            public static FfiCredentialProveList Create(CredentialProve[] args)
            {
                FfiCredentialProveList list = new();
                list.count = (uint)args.Length;
                FfiCredentialProve[] ffiCredentialProves = new FfiCredentialProve[list.count];
                for (int i = 0; i < args.Length; i++)
                {
                    ffiCredentialProves[i] = FfiCredentialProve.Create(args[i]);
                }
                fixed (FfiCredentialProve* ffiProveP = &ffiCredentialProves[0])
                {
                    list.data = ffiProveP;
                }
                return list;
            }

            public static FfiCredentialProveList Create(List<CredentialProve> args)
            {
                return Create(args.ToArray());
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct FfiUIntList
        {
            public uint count;
            public uint* data;
            public static FfiUIntList Create(uint[] args)
            {
                FfiUIntList list = new();
                list.count = (uint)args.Length;
                fixed (uint* uintP = &args[0])
                {
                    list.data = uintP;
                }
                return list;
            }

            public static FfiUIntList Create(List<uint> args)
            {
                return Create(args.ToArray());
            }
        }
    }
}
