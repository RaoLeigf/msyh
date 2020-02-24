#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service
    * 类 名 称：			QTSysSetService
    * 文 件 名：			QTSysSetService.cs
    * 创建时间：			2019/6/3 
    * 作    者：			刘杭    
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
using GSP3.SP.Facade.Interface;
using GSP3.SP.Model.Domain;

namespace GQT3.QT.Service
{
    /// <summary>
    /// QTSysSet服务组装处理类
    /// </summary>
    public partial class QTSysSetService : EntServiceBase<QTSysSetModel>, IQTSysSetService
    {
        #region 类变量及属性
        /// <summary>
        /// QTSysSet业务外观处理对象
        /// </summary>
        IQTSysSetFacade QTSysSetFacade
        {
            get
            {
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQTSysSetFacade;
            }
        }

        private IOrganizationFacade OrganizationFacade { get; set; }
        private IUserFacade UserFacade { get; set; }

        private IGAppvalProcFacade GAppvalProcFacade { get; set; }
        #endregion

        #region 实现 IQTSysSetService 业务添加的成员

        /// <summary>
        /// 保存审批类型
        /// </summary>
        /// <param name="sysSetModel"></param>
        /// <returns></returns>
        public SavedResult<Int64> PostAddProcType(QTSysSetModel sysSetModel)
        {
            if (sysSetModel == null)
                return null;

            string code = "";

            //DicType为"splx"表示是审批类型的数据
            IList<QTSysSetModel> setModels = QTSysSetFacade.Find(t => t.DicType == "splx").Data;
            if (setModels == null || setModels.Count == 0)
            {
                code = "1";
            }
            else
            {
                string maxCode = setModels.OrderByDescending(t => t.TypeCode).ToList()[0].TypeCode;
                code = (Convert.ToInt32(maxCode) + 1) + "";
            }

            sysSetModel.TypeCode = code;
            sysSetModel.TypeCode = int.Parse(sysSetModel.Value).ToString();//与001，002等相对应，与添加顺序无关
            sysSetModel.DicType = "splx";
            //先进行判断，审批流数据不能重复
            var oldset = this.QTSysSetFacade.Find(t => t.DicType == "splx" && t.Value == sysSetModel.Value).Data;
            if(oldset != null && oldset.Count > 0)
            {
                throw new Exception("审批流类型配置不能重复！");
            }
            SavedResult<Int64> savedResult = QTSysSetFacade.Save<Int64>(sysSetModel);
            return savedResult;
        }

        /// <summary>
        /// 获取审批类型
        /// </summary>
        /// <returns></returns>
        public List<QTSysSetModel> GetProcTypes()
        {

            return QTSysSetFacade.GetProcTypes();
        }

        /// <summary>
        /// 修改审批类型
        /// </summary>
        /// <param name="sysSetModel"></param>
        /// <returns></returns>
        public SavedResult<Int64> PostUpdateProcType(QTSysSetModel sysSetModel)
        {
            //先进行判断，审批流数据不能重复
            var oldsets = this.QTSysSetFacade.Find(t => t.DicType == "splx" && t.Value == sysSetModel.Value && t.PhId != sysSetModel.PhId).Data;
            if (oldsets != null && oldsets.Count > 0)
            {
                throw new Exception("审批流类型配置不能重复！");
            }
            else
            {
                var oldset = this.QTSysSetFacade.Find(t => t.PhId == sysSetModel.PhId).Data;
                if(oldset != null && oldset.Count > 0)
                {
                    if (!oldset[0].Value.Equals(sysSetModel.Value))
                    {
                        var procs = this.GAppvalProcFacade.Find(t => t.SPLXPhid == oldset[0].PhId).Data;
                        if(procs != null && procs.Count > 0)
                        {
                            throw new Exception("此类型下已存在审批流，不能修改审批流类型！");
                        }
                    }
                }
            }
            return QTSysSetFacade.PostUpdateProcType(sysSetModel);
        }

        /// <summary>
        /// 返回用户是否启用了Usb加密狗支付，如果启用了则返回对应信息
        /// </summary>
        /// <param name="user_phid">用户phid</param>
        /// <param name="usbKey">启用后，返回UsbKey</param>
        /// <param name="start_dt">启用后，返回启用时间</param>
        /// <param name="endDt">启用后，返回失效时间</param>
        /// <returns>返回值 true 表示启用了， false 表示未启用</returns>
        public bool GetPayUsbKeyIsActive(long user_phid, out string usbKey, out DateTime start_dt, out DateTime endDt)
        {
            bool isActive = false;
            usbKey = "";
            start_dt = DateTime.MinValue;
            endDt = DateTime.MinValue;

            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("DicType", "PayUsbKey"))
                .Add(ORMRestrictions<string>.Eq("TypeCode", user_phid.ToString()));

            var result = this.EntFacade.FacadeHelper.Find(dicWhere);
            if (result.IsError == false && result.Data != null && result.Data.Count() > 0)
            {
                var model = result.Data[0];
                usbKey = model.Value;
                if (!string.IsNullOrEmpty(model.DEFSTR2))
                {
                    start_dt = DateTime.ParseExact(model.DEFSTR2, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                }

                if (!string.IsNullOrEmpty(model.DEFSTR3))
                {
                    endDt = DateTime.ParseExact(model.DEFSTR3, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                }

                if (model.Isactive == 1)
                {
                    isActive = true;
                }
                else
                {
                    isActive = false;
                }
            }

            return isActive;
        }

        /// <summary>
        /// 根据id获取组织
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public OrganizeModel GetOrg(long orgid)
        {
            if (orgid != 0)
            {
                OrganizeModel organize = OrganizationFacade.Find(orgid).Data;
                return organize;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取所有的组织信息
        /// </summary>
        /// <returns></returns>
        public List<OrganizeModel> GetAllOrgs()
        {
            List<OrganizeModel> organizes = new List<OrganizeModel>();
            organizes = this.OrganizationFacade.Find(t => t.PhId != (long)0).Data.ToList();
            return organizes;
        }

        /// <summary>
        /// 根据id获取用户
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public User2Model GetUser(long userid)
        {
            if (userid != 0)
            {
                User2Model user = UserFacade.Find(userid).Data;
                return user;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据组织以及类型获取不同的基础数据集合
        /// </summary>
        /// <param name="orgCode">组织编码</param>
        /// <param name="dicType">基础数据类别</param>
        /// <returns></returns>
        public IList<QTSysSetModel> GetSetListByOrgType(string orgCode, string dicType)
        {
            IList<QTSysSetModel> qTSysSets = new List<QTSysSetModel>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<string>.Eq("Orgcode", orgCode))
                .Add(ORMRestrictions<string>.Eq("DicType", dicType))
                .Add(ORMRestrictions<byte>.Eq("Isactive", (byte)0));
            qTSysSets = this.QTSysSetFacade.Find(dic).Data;
            return qTSysSets;
        }
        #endregion
    }
}

