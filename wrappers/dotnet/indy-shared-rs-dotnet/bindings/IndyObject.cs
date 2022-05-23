using indy_shared_rs_dotnet.indy_credx;
using System;
using System.IO;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.bindings.Structures;

namespace indy_shared_rs_dotnet.bindings
{
	public class IndyObject
	{
		uint _handle;

		public IndyObject(uint handle)
		{
			_handle = handle;
		}

		public Task<ByteBuffer> toJson()
        {
			//TODO return as string ... 
			return ObjectGetJson(_handle);
        }

		public Task<ByteBuffer> ObjectGetJson(uint handle)
        {
			ByteBuffer result;
			result.len = 0;
			result.value = (IntPtr)0;
			
            NativeMethods.credx_object_get_json(handle, ref result);
			return Task.FromResult(result);

		}
	}
}
