using System;
using System.Runtime.InteropServices;

namespace indy_shared_rs_dotnet.bindings
{
	public class Structures
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct FfiStrList
        {
			public uint count;
			public string data;
        }
	}
}
