﻿using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.IndyCredx
{
    public class SchemaApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="originDid"></param>
        /// <param name="schemaName"></param>
        /// <param name="schemaVersion"></param>
        /// <param name="attrNames"></param>
        /// <param name="seqNo"></param>
        /// <returns></returns>
        public static async Task<Schema> CreateSchemaAsync(string originDid, string schemaName, string schemaVersion, List<string> attrNames, uint seqNo)
        {
            uint schemaObjectHandle = 0;
            int errorCode = NativeMethods.credx_create_schema(FfiStr.Create(originDid), FfiStr.Create(schemaName), FfiStr.Create(schemaVersion), FfiStrList.Create(attrNames), seqNo, ref schemaObjectHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            Schema schemaObject = await CreateSchemaObjectAsync(schemaObjectHandle);
            return await Task.FromResult(schemaObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemaJson"></param>
        /// <returns></returns>
        public static async Task<Schema> CreateSchemaFromJsonAsync(string schemaJson)
        {
            uint schemaObjectHandle = 0;
            int errorCode = NativeMethods.credx_schema_from_json(ByteBuffer.Create(schemaJson), ref schemaObjectHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            Schema schemaObject = await CreateSchemaObjectAsync(schemaObjectHandle);
            return await Task.FromResult(schemaObject);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static async Task<string> GetSchemaAttributeAsync(Schema schema, string attributeName)
        {
            string result = "";
            //note: only "id" as attributeName supported so far.
            int errorCode = NativeMethods.credx_schema_get_attribute(schema.Handle, FfiStr.Create(attributeName), ref result);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            return await Task.FromResult(result);
        }

        private static async Task<Schema> CreateSchemaObjectAsync(uint objectHandle)
        {
            string schemaJson = await ObjectApi.ToJsonAsync(objectHandle);
            Schema schemaObject = JsonConvert.DeserializeObject<Schema>(schemaJson, Settings.JsonSettings);
            schemaObject.Handle = objectHandle;
            return await Task.FromResult(schemaObject);
        }
    }
}
