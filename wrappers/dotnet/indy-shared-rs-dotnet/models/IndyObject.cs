using indy_shared_rs_dotnet.indy_credx;
using System;
using System.IO;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.models.Structures;
using System.Text;

namespace indy_shared_rs_dotnet.models
{
	public class IndyObject
	{
		private uint _handle;
		public string objectAsJson { get; }

		public IndyObject(uint handle)
		{
			_handle = handle;
			objectAsJson = toJson().GetAwaiter().GetResult();
		}

		public unsafe Task<string> toJson()
		{
			ByteBuffer byteBuffer = ObjectGetJson(_handle);
			return Task.FromResult(DecodeToString(byteBuffer));
		}

		private unsafe ByteBuffer ObjectGetJson(uint handle)
		{
			ByteBuffer result = new()
			{
				len = 0,
				value = null
			};

			NativeMethods.credx_object_get_json(handle, ref result);
			return result;
		}

		private unsafe string DecodeToString(ByteBuffer byteBuffer)
		{
			Char[] charArray = new Char[byteBuffer.len];
			UTF8Encoding utf8Decoder = new UTF8Encoding(true, true);

			fixed (Char* char_ptr = &charArray[0])
			{
				utf8Decoder.GetChars(byteBuffer.value, (int)byteBuffer.len, char_ptr, (int)byteBuffer.len);
			}
			return new string(charArray);
		}
	}
}
