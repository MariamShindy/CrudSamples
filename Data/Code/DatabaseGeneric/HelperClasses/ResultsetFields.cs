﻿//////////////////////////////////////////////////////////////
// <auto-generated>This code was generated by LLBLGen Pro 5.10.</auto-generated>
//////////////////////////////////////////////////////////////
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates
// Templates vendor: Solutions Design.
//////////////////////////////////////////////////////////////
using System;
using System.Runtime.Serialization;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace CompanyCrudTwo.HelperClasses
{
	/// <summary>Helper class which will eases the creation of custom made resultsets. Usable in typed lists and dynamic lists created using the dynamic query engine.</summary>
	[Serializable]
	public partial class ResultsetFields : EntityFields2
	{
		/// <summary>CTor</summary>
		public ResultsetFields(int amountFields) : base(amountFields, ModelInfoProviderSingleton.GetInstance(), null) {	}
		
		/// <summary>Deserialization constructor</summary>
		/// <param name="info">Info.</param>
		/// <param name="context">Context.</param>
		protected ResultsetFields(SerializationInfo info, StreamingContext context) : base(info.GetInt32("_amountFields"), ModelInfoProviderSingleton.GetInstance(), null)
		{
			PerformDeserializationInitialization(info);
		}

	}
}
