#region Summary
/**************************************************************************************
    * 类 名 称：        OrganizationModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        OrganizationModel.cs
    * 创建时间：        2018/9/13 
    * 作    者：        夏华军    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Enterprise3.Common.Model;
using Enterprise3.Common.Model.NHORM;
using Enterprise3.Common.Model.Enums;

namespace GQT3.QT.Model.Domain
{
    /// <summary>
    /// Organization实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class OrganizeModel : EntityBase<OrganizeModel>
    {
		/// <summary>
		/// 主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 PhId
		{
			get;
			set;
		}

		/// <summary>
		/// 部门归属组织
		/// </summary>
		[DataMember]
		public virtual System.Int64 ParentOrgId
		{
			get;
			set;
		}

		/// <summary>
		/// 组织编号
		/// </summary>
		[DataMember]
		public virtual System.String OCode
		{
			get;
			set;
		}

		/// <summary>
		/// 组织名称
		/// </summary>
		[DataMember]
		public virtual System.String OName
		{
			get;
			set;
		}

		/// <summary>
		/// 外文全称
		/// </summary>
		[DataMember]
		public virtual System.String ForeignFn
		{
			get;
			set;
		}

		/// <summary>
		/// 外文缩写
		/// </summary>
		[DataMember]
		public virtual System.String ForeignAb
		{
			get;
			set;
		}

		/// <summary>
		/// 拼音首字母
		/// </summary>
		[DataMember]
		public virtual System.String Bopomofo
		{
			get;
			set;
		}

		/// <summary>
		/// 简码
		/// </summary>
		[DataMember]
		public virtual System.String CodeValue
		{
			get;
			set;
		}

		/// <summary>
		/// 组织排序号
		/// </summary>
		[DataMember]
		public virtual System.String OrgIndex
		{
			get;
			set;
		}

		/// <summary>
		/// 是否独立法人：y,n
		/// </summary>
		[DataMember]
		public virtual System.String IfCorp
		{
			get;
			set;
		}

		/// <summary>
		/// 是否登录系统
		/// </summary>
		[DataMember]
		public virtual System.String IfLogin
		{
			get;
			set;
		}

		/// <summary>
		/// 是否活动
		/// </summary>
		[DataMember]
		public virtual System.String IsActive
		{
			get;
			set;
		}

		/// <summary>
		/// 有效开始日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? EbDt
		{
			get;
			set;
		}

		/// <summary>
		/// 有效结束日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? EeDt
		{
			get;
			set;
		}

		/// <summary>
		/// 封存时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? CloseOrgDt
		{
			get;
			set;
		}

		/// <summary>
		/// 税号
		/// </summary>
		[DataMember]
		public virtual System.String OTax
		{
			get;
			set;
		}

		/// <summary>
		/// 地址
		/// </summary>
		[DataMember]
		public virtual System.String OAddr
		{
			get;
			set;
		}

		/// <summary>
		/// 邮政编码
		/// </summary>
		[DataMember]
		public virtual System.String OZip
		{
			get;
			set;
		}

		/// <summary>
		/// 电话
		/// </summary>
		[DataMember]
		public virtual System.String OTel
		{
			get;
			set;
		}

		/// <summary>
		/// 传真
		/// </summary>
		[DataMember]
		public virtual System.String OFax
		{
			get;
			set;
		}

		/// <summary>
		/// e_mail
		/// </summary>
		[DataMember]
		public virtual System.String OEmail
		{
			get;
			set;
		}

		/// <summary>
		/// 备注
		/// </summary>
		[DataMember]
		public virtual System.String OMemo
		{
			get;
			set;
		}

		/// <summary>
		/// 负责人
		/// </summary>
		[DataMember]
		public virtual System.String OOwner
		{
			get;
			set;
		}

		/// <summary>
		/// 财务主管
		/// </summary>
		[DataMember]
		public virtual System.String OCfo
		{
			get;
			set;
		}

		/// <summary>
		/// 业务主管
		/// </summary>
		[DataMember]
		public virtual System.String OCbo
		{
			get;
			set;
		}

		/// <summary>
		/// 销售主管
		/// </summary>
		[DataMember]
		public virtual System.String OCio
		{
			get;
			set;
		}

		/// <summary>
		/// 组织类型
		/// </summary>
		[DataMember]
		public virtual System.String OrgType
		{
			get;
			set;
		}

		/// <summary>
		/// 代表岗位
		/// </summary>
		[DataMember]
		public virtual System.String StationCode
		{
			get;
			set;
		}

		/// <summary>
		/// 组织代表人
		/// </summary>
		[DataMember]
		public virtual System.String EmpCode
		{
			get;
			set;
		}

		/// <summary>
		/// 同步原始单位号gzb用
		/// </summary>
		[DataMember]
		public virtual System.String EnterpriseNo
		{
			get;
			set;
		}

		/// <summary>
		/// 行业类型
		/// </summary>
		[DataMember]
		public virtual System.Int64 TradeTypeId
		{
			get;
			set;
		}

		/// <summary>
		/// 对应项目
		/// </summary>
		[DataMember]
		public virtual System.Int64 RelProject
		{
			get;
			set;
		}

		/// <summary>
		/// GFI使用标志
		/// </summary>
		[DataMember]
		public virtual System.String GfiImport
		{
			get;
			set;
		}

		/// <summary>
		/// 薪资月结标志
		/// </summary>
		[DataMember]
		public virtual System.String PrStatus
		{
			get;
			set;
		}

		/// <summary>
		/// 是否发薪
		/// </summary>
		[DataMember]
		public virtual System.String IsSalary
		{
			get;
			set;
		}

		/// <summary>
		/// 启用标志
		/// </summary>
		[DataMember]
		public virtual System.String EnableFlg
		{
			get;
			set;
		}

		/// <summary>
		/// 网络自由呼是否显示
		/// </summary>
		[DataMember]
		public virtual System.String IsNfcShow
		{
			get;
			set;
		}

		/// <summary>
		/// 是否纳税组织
		/// </summary>
		[DataMember]
		public virtual System.String IsRatePay
		{
			get;
			set;
		}

		/// <summary>
		/// 内部申报组织
		/// </summary>
		[DataMember]
		public virtual System.String IsTaxOrg
		{
			get;
			set;
		}

		/// <summary>
		/// 是否建立仓库关联
		/// </summary>
		[DataMember]
		public virtual System.String IsStat
		{
			get;
			set;
		}

		/// <summary>
		/// 是否末级部门
		/// </summary>
		[DataMember]
		public virtual System.String IsEnd
		{
			get;
			set;
		}

		/// <summary>
		/// 同步微信
		/// </summary>
		[DataMember]
		public virtual System.String IsWeChat
		{
			get;
			set;
		}

		/// <summary>
		/// 微信组织编码
		/// </summary>
		[DataMember]
		public virtual System.String WeChatId
		{
			get;
			set;
		}

	}

}