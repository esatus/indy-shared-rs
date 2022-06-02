use std::os::raw::c_char;

use ffi_support::{rust_string_to_c, FfiStr};
use indy_data_types::anoncreds::cred_def::CredentialDefinition;
use indy_data_types::anoncreds::cred_def::CredentialDefinitionPrivate;
use indy_data_types::anoncreds::cred_def::CredentialKeyCorrectnessProof;
use indy_utils::Qualifiable;

use super::error::{catch_error, ErrorCode};
use super::object::{IndyObjectId, ObjectHandle};
use super::util::FfiStrList;
use crate::services::{
    issuer::create_schema,
    types::{DidValue, Schema, SchemaId},
};
use log::{debug, error, log_enabled, info, Level};

use crate::services::{prover::create_master_secret, types::MasterSecret};

pub struct StoredCredDef {
    pub public: CredentialDefinition,
    pub private: CredentialDefinitionPrivate,
    pub key_proof: CredentialKeyCorrectnessProof,
}
pub const ISSUER_DID: &'static str = "NcYxiDXkpYi6ov5FcYDi1e";
pub static GVT_SCHEMA_ATTRIBUTES: &[&'static str; 4] = &["name", "age", "sex", "height"];
pub struct IssuerWallet {
    pub did: DidValue,
    pub cred_defs: Vec<StoredCredDef>,
}

impl Default for IssuerWallet {
    fn default() -> Self {
        Self {
            did: DidValue::from(ISSUER_DID.to_string()),
            cred_defs: vec![],
        }
    }
}

#[no_mangle]
pub extern "C" fn credx_create_schema(
    origin_did: FfiStr,
    schema_name: FfiStr,
    schema_version: FfiStr,
    attr_names: FfiStrList,
    seq_no: i64,
    result_p: *mut ObjectHandle,
) -> ErrorCode {
    catch_error(|| {
        check_useful_c_ptr!(result_p);
		//env_logger::init();
        //debug!("this is a debug {}", "message");
        //error!("this is printed by default");

        let origin_did = {
            let did = origin_did
                .as_opt_str()
                .ok_or_else(|| err_msg!("Missing origin DID"))?;
            DidValue::from_str(did)?
        };
        //let mut issuer_wallet = IssuerWallet::default();
        let schema_name = schema_name
            .as_opt_str()
            .ok_or_else(|| err_msg!("Missing schema name"))?;
        let schema_version = schema_version
            .as_opt_str()
            .ok_or_else(|| err_msg!("Missing schema version"))?;
        let schema = create_schema(
            &origin_did,
            //&issuer_wallet.did,
            schema_name,
            schema_version,
            attr_names.to_string_vec()?.into(),
            //GVT_SCHEMA_ATTRIBUTES[..].into(),
            if seq_no > 0 {
                Some(seq_no as u32)
            } else {
                None
            },
        )?;
        let handle = ObjectHandle::create(schema)?;
        unsafe { *result_p = handle };
        Ok(())
    })
}

#[no_mangle]
pub extern "C" fn credx_schema_get_attribute(
    handle: ObjectHandle,
    name: FfiStr,
    result_p: *mut *const c_char,
) -> ErrorCode {
    catch_error(|| {
        check_useful_c_ptr!(result_p);
        let schema = handle.load()?;
        let schema = schema.cast_ref::<Schema>()?;
        let val = match name.as_opt_str().unwrap_or_default() {
            "id" => schema.get_id().to_string(),
            s => return Err(err_msg!("Unsupported attribute: {}", s)),
        };
        unsafe { *result_p = rust_string_to_c(val) };
        Ok(())
    })
}

impl_indy_object!(Schema, "Schema");
impl_indy_object_from_json!(Schema, credx_schema_from_json);

impl IndyObjectId for Schema {
    type Id = SchemaId;

    fn get_id(&self) -> Self::Id {
        match self {
            Schema::SchemaV1(s) => s.id.clone(),
        }
    }
}
