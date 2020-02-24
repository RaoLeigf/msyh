using Enterprise3.Common.Base.Criterion;
using Enterprise3.Common.Model.Results;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.GQT3.QT.Model;
using GData3.Common.Model;
using GQT3.QT.Model.Domain;
using GQT3.QT.Service.Interface;
using GYS3.YS.Model.Domain;
using GYS3.YS.Service.Interface;
using Newtonsoft.Json;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Enterprise3.WebApi.GQT3.QT.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter]
    public class CorrespondenceSettingsApiController : ApiBase
    {
        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }

        IYsAccountMstService YsAccountMstService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CorrespondenceSettingsApiController()
        {
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
            YsAccountMstService = base.GetObject<IYsAccountMstService>("GYS3.YS.Service.YsAccountMst");
        }

        /// <summary>
        /// 根据操作员和申报单位取预算部门
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetDept([FromUri]BaseInfoModel<CorrespondenceSettingsModel> parameters)
        {
            /*infoData: {
                    Dwdm: '9999',
                    Dydm: '101'
                }*/
            //SELECT dydm FROM Z_QTDYGX WHERE Z_QTDYGX.dwdm='9999'  and Z_QTDYGX.dylx = '97' AND Z_QTDYGX.dydm like '101%' 
            IList<OrganizeModel> organizes = CorrespondenceSettingsService.GetDept(parameters.infoData.Dwdm, parameters.infoData.Dydm);
            return DCHelper.ModelListToJson<OrganizeModel>(organizes, organizes.Count);
        }

        /// <summary>
        /// 组织树(不带部门的)(有权限的)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetOrgTree([FromUri] long UserId)
        {
            List<OrganizeModel> organize = CorrespondenceSettingsService.GetOrgTree(UserId);
            return DCHelper.ModelListToJson<OrganizeModel>(organize, organize.Count);
            //return JsonConvert.SerializeObject(organize);
        }

        /// <summary>
        /// 得到登录信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetLogin()
        {
            return CorrespondenceSettingsService.GetLogin();
        }

        /// <summary>
        /// 根据单位CODE取部门
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetDeptByUnit([FromUri]Parameter parameters)
        {
            if (string.IsNullOrEmpty(parameters.uid))
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }
            if (string.IsNullOrEmpty(parameters.orgid))
            {
                return DCHelper.ErrorMessage("组织编码为空！");
            }
            IList<OrganizeModel> organizes = CorrespondenceSettingsService.GetDeptByUnit(long.Parse(parameters.orgid), long.Parse(parameters.uid));
            return DCHelper.ModelListToJson<OrganizeModel>(organizes, organizes.Count);
        }

        /// <summary>
        /// 完整组织树（没有权限，包括部门）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetALLOrgTree()
        {
            List<OrganizeModel> organize = CorrespondenceSettingsService.GetALLOrgTree();
            return DCHelper.ModelListToJson<OrganizeModel>(organize, organize.Count);
        }

        /// <summary>
        /// 根据组织或者部门获取操作员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string getUserByOrg([FromUri]string OrgCode, [FromUri]BaseListModel parameters, [FromUri]string queryStr)
        {
            var result = CorrespondenceSettingsService.getUserByOrg(OrgCode, queryStr);
            return DCHelper.ModelListToJson<CorrespondenceSettingsModel>(result.Data, (Int32)result.Data.Count);
        }

        /// <summary>
        /// 得到子级(包括部门)(不是树 是list)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetAllChildList([FromUri]long OrgId)
        {
            var result = CorrespondenceSettingsService.GetAllChildList(OrgId);
            return DCHelper.ModelListToJson<OrganizeModel>(result, result.Count);
        }

        /// <summary>
        /// 得到子级(包括部门)(树)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetAllChildTree([FromUri]long OrgId)
        {
            var result = CorrespondenceSettingsService.GetAllChildTree(OrgId);
            return DCHelper.ModelListToJson<OrganizeModel>(result, result.Count);
        }

        /// <summary>
        /// 得到完整登录信息（组织用户）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetLogininfo([FromUri]Parameter parameters)
        {
            if (parameters == null || string.IsNullOrEmpty(parameters.uid))
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }
            if (string.IsNullOrEmpty(parameters.orgid))
            {
                return DCHelper.ErrorMessage("组织编码为空！");
            }
            return CorrespondenceSettingsService.GetLogininfo(long.Parse(parameters.orgid), long.Parse(parameters.uid));
        }

        /// <summary>
        /// 查找操作员默认组织跟部门
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetFDeclarationUnit([FromUri]Parameter parameters)
        {
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "08"))
                .Add(ORMRestrictions<Int32>.Eq("DefInt1", 1)).Add(ORMRestrictions<string>.Eq("Dwdm", parameters.usercode));
            var result = new object();
            var orgCode = "";
            var dept = "";
            var orgName = "";
            var deptName = "";
            var orgCodeList = CorrespondenceSettingsService.Find(dicWhere);
            if (orgCodeList.Data.Count > 0)
            {
                orgCode = orgCodeList.Data[0].Dydm;
                dept = orgCodeList.Data[0].DefStr3;
                if (!string.IsNullOrEmpty(orgCode))
                {
                    orgName = CorrespondenceSettingsService.GetOrg(orgCode).OName;
                }
                else
                {
                    result = new
                    {
                        Status = "error",
                        Msg= "该操作员未设置默认组织！"
                    };
                    return JsonConvert.SerializeObject(result);
                }
                if (!string.IsNullOrEmpty(dept))
                {
                    deptName = CorrespondenceSettingsService.GetOrg(dept).OName;
                }
                else
                {
                    result = new
                    {
                        Status = "error",
                        Msg = "该操作员未设置默认部门！"
                    };
                    return JsonConvert.SerializeObject(result);
                }
            }
            result = new
            {
                Status="success",
                orgCode = orgCode,
                deptCode = dept,
                orgName = orgName,
                deptName = deptName
            };
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 得到包含自己及下级的组织树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetOrg_tree([FromUri]Parameter parameters)
        {
            if (string.IsNullOrEmpty(parameters.orgid))
            {
                return DCHelper.ErrorMessage("组织主键为空！");
            }
            OrganizeModel org = CorrespondenceSettingsService.GetOrg_tree(long.Parse(parameters.orgid));
            IList<YsAccountMstModel> ysAccountMsts = new List<YsAccountMstModel>();
            ysAccountMsts = this.YsAccountMstService.Find(t => t.PhId > (long)0).Data;
            org = GetOrgVerify(org, ysAccountMsts);
            return DataConverterHelper.SerializeObject(org);
        }
        /// <summary>
        /// 给组织树加上上报信息
        /// </summary>
        /// <param name="organize">组织对象</param>
        /// <param name="ysAccountMsts">预决算主表集合</param>
        /// <returns></returns>
        public OrganizeModel GetOrgVerify(OrganizeModel organize, IList<YsAccountMstModel> ysAccountMsts)
        {
            if(organize != null)
            {
                YsAccountMstModel ysAccount = new YsAccountMstModel();
                ysAccount = ysAccountMsts.ToList().Find(t => t.Orgid == organize.PhId);
                if(ysAccount != null)
                {
                    organize.VerifyEnd = ysAccount.VerifyEnd;
                    organize.VerifyEndTime = ysAccount.VerifyEndTime;
                    organize.VerifyMiddle = ysAccount.VerifyMiddle;
                    organize.VerifyMiddleTime = ysAccount.VerifyMiddleTime;
                    organize.VerifyStart = organize.VerifyStart;
                    organize.VerifyStartTime = organize.VerifyStartTime;
                }
                if(organize.children != null && organize.children.Count > 0)
                {
                    foreach (var child in organize.children)
                    {
                        GetOrgVerify(child, ysAccountMsts);
                    }
                }
            }
            return organize;
        }

        #region//绩效相关的组织部门接口
        /// <summary>
        /// 根据用户编码回去相应的预算部门
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetYSBM([FromUri]BaseListModel param)
        {
            if (string.IsNullOrEmpty(param.uCode))
            {
                return DCHelper.ErrorMessage("用户编码不能为空！");
            }
            try
            {
                var result = CorrespondenceSettingsService.FindYSBM(param.uCode);

                return DataConverterHelper.SerializeObject(result);
                //return DataConverterHelper.EntityListToJson<OrganizeModel>(result, (Int32)result.Count);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        #endregion
    }
}
