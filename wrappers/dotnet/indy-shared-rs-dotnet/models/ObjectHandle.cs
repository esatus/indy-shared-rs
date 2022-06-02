﻿using indy_shared_rs_dotnet.indy_credx;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.Models
{
    public class ObjectHandle
    {
        private readonly uint _handle;
        private readonly IndyObject _indyObject;

        public ObjectHandle(uint handle)
        {
            _handle = handle;
        }

        public Task<string> TypeName()
        {
            return ObjectGetTypeName(_handle);
        }

        public Task<string> ObjectGetTypeName(uint handle)
        {
            string result = "";
            NativeMethods.credx_object_get_type_name(handle, ref result);
            return Task.FromResult(result);
        }

        public void ObjectFree()
        {
            NativeMethods.credx_object_free(_handle);
        }
    }
}

