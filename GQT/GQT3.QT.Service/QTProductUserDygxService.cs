#region Summary
/**************************************************************************************
    * 类 名 称：        QTProductUserDygxService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        QTProductUserDygxService.cs
    * 创建时间：        2018/12/12 
    * 作    者：        刘杭    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using SUP.Common.DataEntity;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;

using GQT3.QT.Service.Interface;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;
using Enterprise3.Common.Base.Criterion;
using SUP.Common.Base;
using GQT3.QT.Model.Enums;

namespace GQT3.QT.Service
{
	/// <summary>
	/// QTProductUserDygx服务组装处理类
	/// </summary>
    public partial class QTProductUserDygxService : EntServiceBase<QTProductUserDygxModel>, IQTProductUserDygxService
    {
		#region 类变量及属性
		/// <summary>
        /// QTProductUserDygx业务外观处理对象
        /// </summary>
		IQTProductUserDygxFacade QTProductUserDygxFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQTProductUserDygxFacade;
            }
        }

        private IQTProductFacade QTProductFacade { get; set; }
        private IQTProductUserFacade QTProductUserFacade { get; set; }

        private IUserFacade UserFacade { get; set; }
        #endregion

        #region 实现 IQTProductUserDygxService 业务添加的成员

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="User"></param>
        /// <param name="adddata"></param>
        /// <param name="deletedata"></param>
        /// <returns></returns>
        public CommonResult Save2(User2Model User,List<QTProductUserDygxModel> adddata,List<string> deletedata)
        {
            CommonResult result = new CommonResult();
            try
            {
                if (adddata != null && adddata.Count > 0)
                {
                    for (var i = 0; i < adddata.Count; i++)
                    {
                        QTProductUserDygxModel qTProductUserDygxModel = adddata[i];
                        //Dictionary<string, object> dic1 = new Dictionary<string, object>();
                        //new CreateCriteria(dic1).Add(ORMRestrictions<string>.Eq("ProductBZ", qTProductUserDygxModel.ProductBZ));
                        //QTProductModel qTProductModel = QTProductFacade.Find(dic1).Data[0];
                        //qTProductUserDygxModel.ProductPhid = qTProductModel.PhId;
                        Dictionary<string, object> dic2 = new Dictionary<string, object>();
                        new CreateCriteria(dic2).Add(ORMRestrictions<string>.Eq("ProductBZ", qTProductUserDygxModel.ProductBZ))
                            .Add(ORMRestrictions<string>.Eq("ProductUserCode", qTProductUserDygxModel.ProductUserCode));
                        QTProductUserModel qTProductUserModel = QTProductUserFacade.Find(dic2).Data[0];
                        if (qTProductUserDygxModel.PhId == 0)
                        {
                            qTProductUserDygxModel.ProductPhid = qTProductUserModel.ProductPhid;
                            qTProductUserDygxModel.ProductUserPhid = qTProductUserModel.PhId;
                            qTProductUserDygxModel.ProductUserName = qTProductUserModel.ProductUserName;
                            qTProductUserDygxModel.ProductUserPwd = qTProductUserModel.ProductUserPwd;
                            qTProductUserDygxModel.Fg3userPhid = User.PhId;
                            qTProductUserDygxModel.Fg3userno = User.UserNo;
                            qTProductUserDygxModel.Fg3username = User.UserName;
                            qTProductUserDygxModel.PersistentState = PersistentState.Added;
                            base.Save<Int64>(qTProductUserDygxModel, "");
                        }
                        else
                        {
                            QTProductUserDygxModel qTProductUserDygxModel2 = base.Find(qTProductUserDygxModel.PhId).Data;
                            qTProductUserDygxModel2.ProductBZ = qTProductUserDygxModel.ProductBZ;
                            qTProductUserDygxModel2.ProductUserCode = qTProductUserDygxModel.ProductUserCode;
                            qTProductUserDygxModel2.ProductPhid = qTProductUserModel.ProductPhid;
                            qTProductUserDygxModel2.ProductUserPhid = qTProductUserModel.PhId;
                            qTProductUserDygxModel2.ProductUserName = qTProductUserModel.ProductUserName;
                            qTProductUserDygxModel2.ProductUserPwd = qTProductUserModel.ProductUserPwd;
                            qTProductUserDygxModel2.Fg3userPhid = User.PhId;
                            qTProductUserDygxModel2.Fg3userno = User.UserNo;
                            qTProductUserDygxModel2.Fg3username = User.UserName;
                            qTProductUserDygxModel2.PersistentState = PersistentState.Modified;
                            base.Save<Int64>(qTProductUserDygxModel2, "");
                        }
                    }
                }
                if (deletedata != null && deletedata.Count > 0)
                {
                    for (var j = 0; j < deletedata.Count; j++)
                    {
                        base.Delete(deletedata[j]);
                    }
                }
            }
            catch (Exception e)
            {
                result.Status = ResponseStatus.Error;
                result.Msg = "保存失败!";
            }
            return result;
        }

        /// <summary>
        /// 判断是否设置过该产品的对应账号
        /// </summary>
        /// <param name="ProductBZ"></param>
        /// <returns></returns>
        public CommonResult JudgeByProduct(string ProductBZ)
        {
            CommonResult result = new CommonResult();
            Dictionary<string, object> dic1 = new Dictionary<string, object>();
            new CreateCriteria(dic1).Add(ORMRestrictions<string>.Eq("ProductBZ", ProductBZ));
            var QTProductUserDygxModels = base.Find(dic1).Data;
            if (QTProductUserDygxModels.Count > 0)
            {
                result.Status = ResponseStatus.Error;
                result.Msg = "已设置过该产品的对应账号";
            }
            else
            {
                result.Status = ResponseStatus.Success;
                result.Msg = "未设置过该产品的对应账号";
            }

            return result;
        }

        /// <summary>
        /// 删除后设置对应关系
        /// </summary>
        /// <param name="ProductBZ"></param>
        /// <returns></returns>
        public CommonResult SaveByProductIfDelete(string ProductBZ)
        {
            
            CommonResult result = new CommonResult();
            try
            {
                Dictionary<string, object> dic1 = new Dictionary<string, object>();
                new CreateCriteria(dic1).Add(ORMRestrictions<string>.Eq("ProductBZ", ProductBZ));
                base.Delete(dic1);

                Dictionary<string, object> dic2 = new Dictionary<string, object>();
                new CreateCriteria(dic2).Add(ORMRestrictions<EnumUserStatus>.Eq("Status", EnumUserStatus.Activate));
                //var Users = UserFacade.FacadeHelper.LoadWithPageInfinity("GQT.QT.GetAllUsers", dic2);
                var Users = UserFacade.Find(dic2).Data;
                for (var i = 0; i < Users.Count; i++)
                {
                    User2Model user = Users[i];
                    Dictionary<string, object> dic3 = new Dictionary<string, object>();
                    new CreateCriteria(dic3).Add(ORMRestrictions<string>.Eq("ProductBZ", ProductBZ))
                            .Add(ORMRestrictions<string>.Eq("ProductUserCode", user.UserNo));
                    var IfUser = QTProductUserFacade.Find(dic3).Data;
                    if (IfUser.Count > 0)
                    {
                        QTProductUserModel qTProductUserModel = IfUser[0];
                        QTProductUserDygxModel qTProductUserDygxModel = new QTProductUserDygxModel
                        {
                            ProductBZ = ProductBZ,
                            ProductUserCode = user.UserNo,
                            ProductPhid = qTProductUserModel.ProductPhid,
                            ProductUserPhid = qTProductUserModel.PhId,
                            ProductUserName = qTProductUserModel.ProductUserName,
                            ProductUserPwd = qTProductUserModel.ProductUserPwd,
                            Fg3userPhid = user.PhId,
                            Fg3userno = user.UserNo,
                            Fg3username = user.UserName,
                            PersistentState = PersistentState.Added
                        };
                        base.Save<Int64>(qTProductUserDygxModel, "");

                    }
                }
            }
            catch (Exception e)
            {
                result.Status = ResponseStatus.Error;
                result.Msg = "同步失败!";
            }
            return result;
        }

        /// <summary>
        /// 不删除设置对应关系
        /// </summary>
        /// <param name="ProductBZ"></param>
        /// <returns></returns>
        public CommonResult SaveByProductNoDelete(string ProductBZ)
        {
            CommonResult result = new CommonResult();
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                new CreateCriteria(dic).Add(ORMRestrictions<EnumUserStatus>.Eq("Status", EnumUserStatus.Activate));
                //var Users = UserFacade.FacadeHelper.LoadWithPageInfinity("GQT.QT.GetAllUsers", dic2);
                var Users = UserFacade.Find(dic).Data;
                for (var i = 0; i < Users.Count; i++)
                {
                    User2Model user = Users[i];
                    Dictionary<string, object> dic2 = new Dictionary<string, object>();
                    new CreateCriteria(dic2).Add(ORMRestrictions<string>.Eq("ProductBZ", ProductBZ))
                        .Add(ORMRestrictions<string>.Eq("Fg3userno", user.UserNo));
                    var IfDYGX = base.Find(dic2).Data;
                    Dictionary<string, object> dic3 = new Dictionary<string, object>();
                    new CreateCriteria(dic3).Add(ORMRestrictions<string>.Eq("ProductBZ", ProductBZ))
                            .Add(ORMRestrictions<string>.Eq("ProductUserCode", user.UserNo));
                    var IfUser = QTProductUserFacade.Find(dic3).Data;
                    if (IfDYGX .Count==0&& IfUser.Count > 0)
                    {
                        QTProductUserModel qTProductUserModel = IfUser[0];
                        QTProductUserDygxModel qTProductUserDygxModel = new QTProductUserDygxModel
                        {
                            ProductBZ = ProductBZ,
                            ProductUserCode = user.UserNo,
                            ProductPhid = qTProductUserModel.ProductPhid,
                            ProductUserPhid = qTProductUserModel.PhId,
                            ProductUserName = qTProductUserModel.ProductUserName,
                            ProductUserPwd = qTProductUserModel.ProductUserPwd,
                            Fg3userPhid = user.PhId,
                            Fg3userno = user.UserNo,
                            Fg3username = user.UserName,
                            PersistentState = PersistentState.Added
                        };
                        base.Save<Int64>(qTProductUserDygxModel, "");

                    }
                }
            }
            catch (Exception e)
            {
                result.Status = ResponseStatus.Error;
                result.Msg = "同步失败!";
            }
            return result;
        }

        /// <summary>
        /// 没有该产品对应关系设置对应关系
        /// </summary>
        /// <param name="ProductBZ"></param>
        /// <returns></returns>
        public CommonResult SaveByProduct(string ProductBZ)
        {
            CommonResult result = new CommonResult();
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                new CreateCriteria(dic).Add(ORMRestrictions<EnumUserStatus>.Eq("Status", EnumUserStatus.Activate));
                //var Users = UserFacade.FacadeHelper.LoadWithPageInfinity("GQT.QT.GetAllUsers", dic2);
                var Users = UserFacade.Find(dic).Data;
                for (var i = 0; i < Users.Count; i++)
                {
                    User2Model user = Users[i];
                    
                    Dictionary<string, object> dic3 = new Dictionary<string, object>();
                    new CreateCriteria(dic3).Add(ORMRestrictions<string>.Eq("ProductBZ", ProductBZ))
                            .Add(ORMRestrictions<string>.Eq("ProductUserCode", user.UserNo));
                    var IfUser = QTProductUserFacade.Find(dic3).Data;
                    if (IfUser.Count > 0)
                    {
                        QTProductUserModel qTProductUserModel = IfUser[0];
                        QTProductUserDygxModel qTProductUserDygxModel = new QTProductUserDygxModel
                        {
                            ProductBZ = ProductBZ,
                            ProductUserCode = user.UserNo,
                            ProductPhid = qTProductUserModel.ProductPhid,
                            ProductUserPhid = qTProductUserModel.PhId,
                            ProductUserName = qTProductUserModel.ProductUserName,
                            ProductUserPwd = qTProductUserModel.ProductUserPwd,
                            Fg3userPhid = user.PhId,
                            Fg3userno = user.UserNo,
                            Fg3username = user.UserName,
                            PersistentState = PersistentState.Added
                        };
                        base.Save<Int64>(qTProductUserDygxModel, "");

                    }
                }
            }
            catch (Exception e)
            {
                result.Status = ResponseStatus.Error;
                result.Msg = "同步失败!";
            }
            return result;
        }

        #endregion
    }
}

