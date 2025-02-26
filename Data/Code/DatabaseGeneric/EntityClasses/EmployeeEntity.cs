﻿//////////////////////////////////////////////////////////////
// <auto-generated>This code was generated by LLBLGen Pro 5.10.</auto-generated>
//////////////////////////////////////////////////////////////
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates
// Templates vendor: Solutions Design.
//////////////////////////////////////////////////////////////
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using CompanyCrudTwo.HelperClasses;
using CompanyCrudTwo.FactoryClasses;
using CompanyCrudTwo.RelationClasses;

using SD.LLBLGen.Pro.ORMSupportClasses;

namespace CompanyCrudTwo.EntityClasses
{
	// __LLBLGENPRO_USER_CODE_REGION_START AdditionalNamespaces
	// __LLBLGENPRO_USER_CODE_REGION_END

	/// <summary>Entity class which represents the entity 'Employee'.<br/><br/></summary>
	[Serializable]
	public partial class EmployeeEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END
	
	{
		private DepartmentEntity _department;
		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END

		private static EmployeeEntityStaticMetaData _staticMetaData = new EmployeeEntityStaticMetaData();
		private static EmployeeRelations _relationsFactory = new EmployeeRelations();

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name Department</summary>
			public static readonly string Department = "Department";
		}

		/// <summary>Static meta-data storage for navigator related information</summary>
		protected class EmployeeEntityStaticMetaData : EntityStaticMetaDataBase
		{
			public EmployeeEntityStaticMetaData()
			{
				SetEntityCoreInfo("EmployeeEntity", InheritanceHierarchyType.None, false, (int)CompanyCrudTwo.EntityType.EmployeeEntity, typeof(EmployeeEntity), typeof(EmployeeEntityFactory), false);
				AddNavigatorMetaData<EmployeeEntity, DepartmentEntity>("Department", "Employees", (a, b) => a._department = b, a => a._department, (a, b) => a.Department = b, CompanyCrudTwo.RelationClasses.StaticEmployeeRelations.DepartmentEntityUsingDepartmentIdStatic, ()=>new EmployeeRelations().DepartmentEntityUsingDepartmentId, null, new int[] { (int)EmployeeFieldIndex.DepartmentId }, null, true, (int)CompanyCrudTwo.EntityType.DepartmentEntity);
			}
		}

		/// <summary>Static ctor</summary>
		static EmployeeEntity()
		{
		}

		/// <summary> CTor</summary>
		public EmployeeEntity()
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public EmployeeEntity(IEntityFields2 fields)
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this EmployeeEntity</param>
		public EmployeeEntity(IValidator validator)
		{
			InitClassEmpty(validator, null);
		}

		/// <summary> CTor</summary>
		/// <param name="employeeId">PK value for Employee which data should be fetched into this Employee object</param>
		public EmployeeEntity(System.Int32 employeeId) : this(employeeId, null)
		{
		}

		/// <summary> CTor</summary>
		/// <param name="employeeId">PK value for Employee which data should be fetched into this Employee object</param>
		/// <param name="validator">The custom validator object for this EmployeeEntity</param>
		public EmployeeEntity(System.Int32 employeeId, IValidator validator)
		{
			InitClassEmpty(validator, null);
			this.EmployeeId = employeeId;
		}

		/// <summary>Private CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected EmployeeEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			// __LLBLGENPRO_USER_CODE_REGION_START DeserializationConstructor
			// __LLBLGENPRO_USER_CODE_REGION_END
		}

		/// <summary>Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'Department' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoDepartment() { return CreateRelationInfoForNavigator("Department"); }
		
		/// <inheritdoc/>
		protected override EntityStaticMetaDataBase GetEntityStaticMetaData() {	return _staticMetaData; }

		/// <summary>Initializes the class members</summary>
		private void InitClassMembers()
		{
			PerformDependencyInjection();
			// __LLBLGENPRO_USER_CODE_REGION_START InitClassMembers
			// __LLBLGENPRO_USER_CODE_REGION_END

			OnInitClassMembersComplete();
		}

		/// <summary>Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this EmployeeEntity</param>
		/// <param name="fields">Fields of this entity</param>
		private void InitClassEmpty(IValidator validator, IEntityFields2 fields)
		{
			OnInitializing();
			this.Fields = fields ?? CreateFields();
			this.Validator = validator;
			InitClassMembers();
			// __LLBLGENPRO_USER_CODE_REGION_START InitClassEmpty
			// __LLBLGENPRO_USER_CODE_REGION_END


			OnInitialized();
		}

		/// <summary>The relations object holding all relations of this entity with other entity classes.</summary>
		public static EmployeeRelations Relations { get { return _relationsFactory; } }

		/// <summary>Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Department' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathDepartment { get { return _staticMetaData.GetPrefetchPathElement("Department", CommonEntityBase.CreateEntityCollection<DepartmentEntity>()); } }

		/// <summary>The DepartmentId property of the Entity Employee<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Employee"."DepartmentId".<br/>Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0.<br/>Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> DepartmentId
		{
			get { return (Nullable<System.Int32>)GetValue((int)EmployeeFieldIndex.DepartmentId, false); }
			set	{ SetValue((int)EmployeeFieldIndex.DepartmentId, value); }
		}

		/// <summary>The EmployeeId property of the Entity Employee<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Employee"."EmployeeId".<br/>Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0.<br/>Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int32 EmployeeId
		{
			get { return (System.Int32)GetValue((int)EmployeeFieldIndex.EmployeeId, true); }
			set { SetValue((int)EmployeeFieldIndex.EmployeeId, value); }		}

		/// <summary>The Name property of the Entity Employee<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Employee"."Name".<br/>Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100.<br/>Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Name
		{
			get { return (System.String)GetValue((int)EmployeeFieldIndex.Name, true); }
			set	{ SetValue((int)EmployeeFieldIndex.Name, value); }
		}

		/// <summary>The Salary property of the Entity Employee<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Employee"."Salary".<br/>Table field type characteristics (type, precision, scale, length): Decimal, 18, 0, 0.<br/>Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Decimal> Salary
		{
			get { return (Nullable<System.Decimal>)GetValue((int)EmployeeFieldIndex.Salary, false); }
			set	{ SetValue((int)EmployeeFieldIndex.Salary, value); }
		}

		/// <summary>Gets / sets related entity of type 'DepartmentEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(false)]
		public virtual DepartmentEntity Department
		{
			get { return _department; }
			set { SetSingleRelatedEntityNavigator(value, "Department"); }
		}
		// __LLBLGENPRO_USER_CODE_REGION_START CustomEntityCode
		// __LLBLGENPRO_USER_CODE_REGION_END


	}
}

namespace CompanyCrudTwo
{
	public enum EmployeeFieldIndex
	{
		///<summary>DepartmentId. </summary>
		DepartmentId,
		///<summary>EmployeeId. </summary>
		EmployeeId,
		///<summary>Name. </summary>
		Name,
		///<summary>Salary. </summary>
		Salary,
		/// <summary></summary>
		AmountOfFields
	}
}

namespace CompanyCrudTwo.RelationClasses
{
	/// <summary>Implements the relations factory for the entity: Employee. </summary>
	public partial class EmployeeRelations: RelationFactory
	{

		/// <summary>Returns a new IEntityRelation object, between EmployeeEntity and DepartmentEntity over the m:1 relation they have, using the relation between the fields: Employee.DepartmentId - Department.DepartmentId</summary>
		public virtual IEntityRelation DepartmentEntityUsingDepartmentId
		{
			get	{ return ModelInfoProviderSingleton.GetInstance().CreateRelation(RelationType.ManyToOne, "Department", false, new[] { DepartmentFields.DepartmentId, EmployeeFields.DepartmentId }); }
		}

	}
	
	/// <summary>Static class which is used for providing relationship instances which are re-used internally for syncing</summary>
	internal static class StaticEmployeeRelations
	{
		internal static readonly IEntityRelation DepartmentEntityUsingDepartmentIdStatic = new EmployeeRelations().DepartmentEntityUsingDepartmentId;

		/// <summary>CTor</summary>
		static StaticEmployeeRelations() { }
	}
}
