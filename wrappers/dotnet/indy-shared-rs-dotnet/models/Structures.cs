﻿using indy_shared_rs_dotnet.indy_credx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace indy_shared_rs_dotnet.Models
{
    public class Structures
    {
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct FfiStrList
        {
            public IntPtr count;
            public FfiStr* data;
            public static FfiStrList Create(string[] args)
            {
                FfiStrList list = new();
                list.count = (IntPtr)args.Length;
                if (args.First() != null) 
                {
                    FfiStr[] ffiStrings = new FfiStr[(uint)args.Length];
                    for (int i = 0; i < args.Length; i++)
                    {
                        ffiStrings[i] = FfiStr.Create(args[i]);
                    }
                    fixed (FfiStr* ffiStr_p = &ffiStrings[0])
                    {
                        list.data = ffiStr_p;
                    }
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
                if (arg != null) {
                    FfiString.data = Marshal.StringToCoTaskMemUTF8(arg); 
                }
                return FfiString;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct ByteBuffer
        {
            public long len;
            public byte* value;
            
            public static ByteBuffer Create(string json)
            {
                UTF8Encoding decoder = new UTF8Encoding(true, true);
                byte[] bytes = new byte[json.Length];
                decoder.GetBytes(json, 0, json.Length, bytes, 0);
                ByteBuffer buffer = new();
                buffer.len = json.Length;
                fixed (byte* bytebuffer_p = &bytes[0])
                {
                    buffer.value = bytebuffer_p;
                }
                return buffer;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FfiCredRevInfo
        {
            public IntPtr regDefObjectHandle;
            public IntPtr regDefPvtObjectHandle;
            public IntPtr registryObjectHandle;
            public IntPtr regIdx; //long
            public FfiLongList regUsed;
            public FfiStr tailsPath;

            public static FfiCredRevInfo Create(CredentialRevocationConfig entry)
            {
                FfiCredRevInfo result = new();
                if (entry != null)
                {
                    result.regDefObjectHandle = (IntPtr)entry.revRegDefObjectHandle;
                    result.regDefPvtObjectHandle = (IntPtr)entry.revRegDefPvtObjectHandle;
                    result.registryObjectHandle = (IntPtr)entry.revRegObjectHandle;
                    result.regIdx = (IntPtr)entry.regIdx;
                    result.regUsed = FfiLongList.Create(entry.regUsed);
                    result.tailsPath = FfiStr.Create(entry.tailsPath);
                }
                return result;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct FfiLongList
        {
            public IntPtr count;
            public long* data;
            public static FfiLongList Create(long[] args)
            {
                FfiLongList list = new();
                list.count = (IntPtr)args.Length;
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
            public IntPtr CredentialObjectHandle;
            public long Timestamp;
            public IntPtr RevStateObjectHandle;

            public static FfiCredentialEntry Create(CredentialEntry entry)
            {
                FfiCredentialEntry result = new();
                if (entry != null) 
                {
                result.CredentialObjectHandle = (IntPtr)entry.CredentialObjectHandle;
                result.Timestamp = entry.Timestamp;
                result.RevStateObjectHandle = (IntPtr)entry.RevStateObjectHandle;
                }
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

            public static FfiCredentialProve Create(CredentialProof prove)
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
        public struct FfiRevocationEntry
        {
            public long DefEntryIdx;
            public uint Entry;
            public long Timestamp;

            public static FfiRevocationEntry Create(RevocationRegistryEntry entry)
            {
                FfiRevocationEntry result = new();
                result.DefEntryIdx = entry.DefEntryIdx;
                result.Entry = entry.Entry;
                result.Timestamp = entry.Timestamp;
                
                return result;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct FfiCredentialEntryList
        {
            public IntPtr count;
            public FfiCredentialEntry* data;
            public static FfiCredentialEntryList Create(CredentialEntry[] args)
            {
                FfiCredentialEntryList list = new();
                list.count = (IntPtr)args.Length;
                FfiCredentialEntry[] ffiCredentialEntries = new FfiCredentialEntry[args.Length];
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
            public IntPtr count;
            public FfiCredentialProve* data;
            public static FfiCredentialProveList Create(CredentialProof[] args)
            {
                FfiCredentialProveList list = new();
                list.count = (IntPtr)args.Length;
                FfiCredentialProve[] ffiCredentialProves = new FfiCredentialProve[args.Length];
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

            public static FfiCredentialProveList Create(List<CredentialProof> args)
            {
                return Create(args.ToArray());
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct FfiUIntList
        {
            public IntPtr count;
            public uint* data;
            public static FfiUIntList Create(uint[] args)
            {
                FfiUIntList list = new();
                list.count = (IntPtr)args.Length;
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

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct FfiRevocationEntryList
        {
            public IntPtr count;
            public FfiRevocationEntry* data;
            public static FfiRevocationEntryList Create(RevocationRegistryEntry[] args)
            {
                FfiRevocationEntryList list = new();
                list.count = (IntPtr)args.Length;
                FfiRevocationEntry[] ffiRevocationEntries = new FfiRevocationEntry[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    ffiRevocationEntries[i] = FfiRevocationEntry.Create(args[i]);
                }
                fixed (FfiRevocationEntry* ffiEntryP = &ffiRevocationEntries[0])
                {
                    list.data = ffiEntryP;
                }
                return list;
            }

            public static FfiRevocationEntryList Create(List<RevocationRegistryEntry> args)
            {
                return Create(args.ToArray());
            }
        }
    }
}
