﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.573
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

// 
// 此源代码由 wsdl, Version=1.1.4322.573 自动生成。

// 
using System.Diagnostics;
using System.Xml.Serialization;
using System;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Services;

namespace SUP.NSServer
{
	/// <remarks/>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Web.Services.WebServiceBindingAttribute(Name="NGNSServerServiceSoap", Namespace="http://tempuri.org/")]
	public class NGNSServerService : System.Web.Services.Protocols.SoapHttpClientProtocol 
	{
    
		/// <remarks/>
		public NGNSServerService() 
		{
			this.Url = "http://localhost/i6Site/i6Service/NGNSServerService.asmx";
		}
    
		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/License_GetProperty", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public string License_GetProperty(int in0, bool in1, int in2, string in3) 
		{
			object[] results = this.Invoke("License_GetProperty", new object[] {
																				   in0,
																				   in1,
																				   in2,
																				   in3});
			return ((string)(results[0]));
		}
    
		/// <remarks/>
		public System.IAsyncResult BeginLicense_GetProperty(int in0, bool in1, int in2, string in3, System.AsyncCallback callback, object asyncState) 
		{
			return this.BeginInvoke("License_GetProperty", new object[] {
																			in0,
																			in1,
																			in2,
																			in3}, callback, asyncState);
		}
    
		/// <remarks/>
		public string EndLicense_GetProperty(System.IAsyncResult asyncResult) 
		{
			object[] results = this.EndInvoke(asyncResult);
			return ((string)(results[0]));
		}
    
		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/License_GetSN", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public string License_GetSN(int in0, bool in1, int in2) 
		{
			object[] results = this.Invoke("License_GetSN", new object[] {
																			 in0,
																			 in1,
																			 in2});
			return ((string)(results[0]));
		}
    
		/// <remarks/>
		public System.IAsyncResult BeginLicense_GetSN(int in0, bool in1, int in2, System.AsyncCallback callback, object asyncState) 
		{
			return this.BeginInvoke("License_GetSN", new object[] {
																	  in0,
																	  in1,
																	  in2}, callback, asyncState);
		}
    
		/// <remarks/>
		public string EndLicense_GetSN(System.IAsyncResult asyncResult) 
		{
			object[] results = this.EndInvoke(asyncResult);
			return ((string)(results[0]));
		}
    
		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/License_IsModuleDemo", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public string License_IsModuleDemo(int in0, bool in1, int in2, string in3) 
		{
			object[] results = this.Invoke("License_IsModuleDemo", new object[] {
																					in0,
																					in1,
																					in2,
																					in3});
			return ((string)(results[0]));
		}
    
		/// <remarks/>
		public System.IAsyncResult BeginLicense_IsModuleDemo(int in0, bool in1, int in2, string in3, System.AsyncCallback callback, object asyncState) 
		{
			return this.BeginInvoke("License_IsModuleDemo", new object[] {
																			 in0,
																			 in1,
																			 in2,
																			 in3}, callback, asyncState);
		}
    
		/// <remarks/>
		public string EndLicense_IsModuleDemo(System.IAsyncResult asyncResult) 
		{
			object[] results = this.EndInvoke(asyncResult);
			return ((string)(results[0]));
		}
    
		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/License_GetAbout", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public string License_GetAbout(int in0, bool in1, int in2) 
		{
			object[] results = this.Invoke("License_GetAbout", new object[] {
																				in0,
																				in1,
																				in2});
			return ((string)(results[0]));
		}
    
		/// <remarks/>
		public System.IAsyncResult BeginLicense_GetAbout(int in0, bool in1, int in2, System.AsyncCallback callback, object asyncState) 
		{
			return this.BeginInvoke("License_GetAbout", new object[] {
																		 in0,
																		 in1,
																		 in2}, callback, asyncState);
		}
    
		/// <remarks/>
		public string EndLicense_GetAbout(System.IAsyncResult asyncResult) 
		{
			object[] results = this.EndInvoke(asyncResult);
			return ((string)(results[0]));
		}
    
		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/License_GetPropertyName", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public string License_GetPropertyName(int in0, bool in1, int in2, string in3) 
		{
			object[] results = this.Invoke("License_GetPropertyName", new object[] {
																					   in0,
																					   in1,
																					   in2,
																					   in3});
			return ((string)(results[0]));
		}
    
		/// <remarks/>
		public System.IAsyncResult BeginLicense_GetPropertyName(int in0, bool in1, int in2, string in3, System.AsyncCallback callback, object asyncState) 
		{
			return this.BeginInvoke("License_GetPropertyName", new object[] {
																				in0,
																				in1,
																				in2,
																				in3}, callback, asyncState);
		}
    
		/// <remarks/>
		public string EndLicense_GetPropertyName(System.IAsyncResult asyncResult) 
		{
			object[] results = this.EndInvoke(asyncResult);
			return ((string)(results[0]));
		}
    
		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/License_GetPropertySelectValue", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public string License_GetPropertySelectValue(int in0, bool in1, int in2, string in3, string in4) 
		{
			object[] results = this.Invoke("License_GetPropertySelectValue", new object[] {
																							  in0,
																							  in1,
																							  in2,
																							  in3,
																							  in4});
			return ((string)(results[0]));
		}
    
		/// <remarks/>
		public System.IAsyncResult BeginLicense_GetPropertySelectValue(int in0, bool in1, int in2, string in3, string in4, System.AsyncCallback callback, object asyncState) 
		{
			return this.BeginInvoke("License_GetPropertySelectValue", new object[] {
																					   in0,
																					   in1,
																					   in2,
																					   in3,
																					   in4}, callback, asyncState);
		}
    
		/// <remarks/>
		public string EndLicense_GetPropertySelectValue(System.IAsyncResult asyncResult) 
		{
			object[] results = this.EndInvoke(asyncResult);
			return ((string)(results[0]));
		}

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/License_GetProperties", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string License_GetProperties(int in0, bool in1, int in2, string in3)
        {
            object[] results = this.Invoke("License_GetProperties", new object[] {
																				   in0,
																				   in1,
																				   in2,
																				   in3});
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/License_IsModuleDemo", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string License_IsModulesDemo(int in0, bool in1, int in2, string in3)
        {
            object[] results = this.Invoke("License_IsModulesDemo", new object[] {
																					in0,
																					in1,
																					in2,
																					in3});
            return ((string)(results[0]));
        }

	}
}
