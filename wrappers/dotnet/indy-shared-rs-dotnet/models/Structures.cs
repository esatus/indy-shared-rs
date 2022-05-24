using System;
using System.Runtime.InteropServices;

namespace indy_shared_rs_dotnet.models
{
	public class Structures
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct FfiStrList
        {
			public uint count;
			public string data;
        }

		[StructLayout(LayoutKind.Sequential)]
		unsafe public struct ByteBuffer
		{
			public uint len;
            public byte* value;
        }
	}
}
