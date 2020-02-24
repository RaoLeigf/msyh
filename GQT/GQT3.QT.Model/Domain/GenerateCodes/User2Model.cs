#region Summary
/**************************************************************************************
    * 类 名 称：        UserModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        UserModel.cs
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

using GQT3.QT.Model.Enums;

namespace GQT3.QT.Model.Domain
{
    /// <summary>
    /// User实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class User2Model : EntityBase<User2Model>
    {
		/// <summary>
		/// phid
		/// </summary>
		[DataMember]
		public virtual System.Int64 PhId
		{
			get;
			set;
		}

		/// <summary>
		/// 用户no
		/// </summary>
		[DataMember]
		public virtual System.String UserNo
		{
			get;
			set;
		}

		/// <summary>
		/// 姓
		/// </summary>
		[DataMember]
		public virtual System.String FirstName
		{
			get;
			set;
		}

		/// <summary>
		/// 名
		/// </summary>
		[DataMember]
		public virtual System.String LastName
		{
			get;
			set;
		}

		/// <summary>
		/// 用户名称
		/// </summary>
		[DataMember]
		public virtual System.String UserName
		{
			get;
			set;
		}

		/// <summary>
		/// 登录标志
		/// </summary>
		[DataMember]
		public virtual System.Int16 LgSign
		{
			get;
			set;
		}

		/// <summary>
		/// 用户状态
		/// </summary>
		[DataMember]
		public virtual EnumUserStatus Status
		{
			get;
			set;
		}

		/// <summary>
		/// 用户分类
		/// </summary>
		[DataMember]
		public virtual System.Int64 UserClass
		{
			get;
			set;
		}

		/// <summary>
		/// 所属部门
		/// </summary>
		[DataMember]
		public virtual System.Int64 DeptId
		{
			get;
			set;
		}

		/// <summary>
		/// 描述
		/// </summary>
		[DataMember]
		public virtual System.String Descript
		{
			get;
			set;
		}

		/// <summary>
		/// 有效开始日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? StrDate
		{
			get;
			set;
		}

		/// <summary>
		/// 有效结束日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? EndDate
		{
			get;
			set;
		}

		/// <summary>
		/// 设置密码标识
		/// </summary>
		[DataMember]
		public virtual System.String PwdIdentity
		{
			get;
			set;
		}

		/// <summary>
		/// 设置密码创建时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? PwdDateTime
		{
			get;
			set;
		}

		/// <summary>
		/// question
		/// </summary>
		[DataMember]
		public virtual System.String Question
		{
			get;
			set;
		}

		/// <summary>
		/// answer
		/// </summary>
		[DataMember]
		public virtual System.String Answer
		{
			get;
			set;
		}

		/// <summary>
		/// email
		/// </summary>
		[DataMember]
		public virtual System.String Email
		{
			get;
			set;
		}

		/// <summary>
		/// 手机号码
		/// </summary>
		[DataMember]
		public virtual System.String MobileNo
		{
			get;
			set;
		}

		/// <summary>
		/// 最近修改口令的日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? ChgPwdDate
		{
			get;
			set;
		}

		/// <summary>
		/// 错误登陆次数
		/// </summary>
		[DataMember]
		public virtual System.String Errtimes
		{
			get;
			set;
		}

		/// <summary>
		/// 桌面角色
		/// </summary>
		[DataMember]
		public virtual System.String DesktopActor
		{
			get;
			set;
		}

		/// <summary>
		/// 启用ip控制
		/// </summary>
		[DataMember]
		public virtual System.Int16 IpCtrl
		{
			get;
			set;
		}

		/// <summary>
		/// 统一身份认证用户串
		/// </summary>
		[DataMember]
		public virtual System.String UiaKey
		{
			get;
			set;
		}

		/// <summary>
		/// 为1则为集成账户用户不能直接登录系统
		/// </summary>
		[DataMember]
		public virtual System.Int16 GuestFlg
		{
			get;
			set;
		}

		/// <summary>
		/// 跨组织标志
		/// </summary>
		[DataMember]
		public virtual System.Int16 MultiCorpFlg
		{
			get;
			set;
		}

		/// <summary>
		/// 单点登录组织
		/// </summary>
		[DataMember]
		public virtual System.Int64 SingleSignOnOrg
		{
			get;
			set;
		}

		/// <summary>
		/// 最后登陆组织号
		/// </summary>
		[DataMember]
		public virtual System.Int64 LastLoginOrg
		{
			get;
			set;
		}

		/// <summary>
		/// 对应员工
		/// </summary>
		[DataMember]
		public virtual System.Int64 HrId
		{
			get;
			set;
		}

		/// <summary>
		/// 角色区分组织管理员
		/// </summary>
		[DataMember]
		public virtual System.Int16 DiffOrgRoleRight
		{
			get;
			set;
		}

		/// <summary>
		/// 联盟体人才
		/// </summary>
		[DataMember]
		public virtual System.Int16 IsUbe
		{
			get;
			set;
		}

		/// <summary>
		/// 当前项目
		/// </summary>
		[DataMember]
		public virtual System.Int64 CurrentPc
		{
			get;
			set;
		}

		/// <summary>
		/// 微信号
		/// </summary>
		[DataMember]
		public virtual System.String WeChat
		{
			get;
			set;
		}

	}

}