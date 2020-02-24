using Enterprise3.Common.Base.Criterion;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.GQT3.QT.Model;
using GQT3.QT.Model.Domain;
using GQT3.QT.Service.Interface;
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
    public class CorrespondenceSettings2ApiController : ApiBase
    {
        ICorrespondenceSettings2Service CorrespondenceSettings2Service { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CorrespondenceSettings2ApiController()
        {
            CorrespondenceSettings2Service = base.GetObject<ICorrespondenceSettings2Service>("GQT3.QT.Service.CorrespondenceSettings2");
        }

        /// <summary>
        /// 根据操作员取申报单位
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetSBUnit([FromUri]BaseListModel parameters)
        {
            //uid:'488181024000001'
            IList<OrganizeModel> organizes = CorrespondenceSettings2Service.GetSBUnit(long.Parse(parameters.uid));
            return DCHelper.ModelListToJson<OrganizeModel>(organizes, organizes.Count);
        }

        /// <summary>
        /// 保存用户设置的当前年度
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string PostSaveUserYear([FromBody]BaseListModel parameters)
        {
            if (string.IsNullOrEmpty(parameters.uid))
            {
                return DCHelper.ErrorMessage("用户主键不能为空！");
            }
            if (string.IsNullOrEmpty(parameters.Year))
            {
                return DCHelper.ErrorMessage("年份信息不能为空！");
            }
            try
            {
                var dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "YEAR"))
                    .Add(ORMRestrictions<string>.Eq("Dwdm", parameters.uid));
                var Model2 = CorrespondenceSettings2Service.Find(dicWhere).Data;
                if (Model2 != null && Model2.Count > 0)
                {
                    Model2[0].Dydm = parameters.Year;
                    Model2[0].PersistentState = PersistentState.Modified;
                    var result = CorrespondenceSettings2Service.Save<Int64>(Model2[0], "");
                    return DataConverterHelper.SerializeObject(result);
                }
                else
                {
                    CorrespondenceSettings2Model model = new CorrespondenceSettings2Model();
                    model.Dydm = parameters.Year;
                    model.Dylx = "YEAR";
                    model.Dwdm = parameters.uid;
                    model.PersistentState = PersistentState.Added;
                    var result = CorrespondenceSettings2Service.Save<Int64>(model, "");
                    return DataConverterHelper.SerializeObject(result);
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }

        }

        #region//vue绩效评分相关

        /// <summary>
        /// 获取相应组织的绩效评分集合
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetJXJLset([FromUri]BaseListModel param)
        {
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织编码不能为空！");
            }
            try
            {
                IList<CorrespondenceSettings2Model> correspondenceSettings2s = new List<CorrespondenceSettings2Model>();
                correspondenceSettings2s = this.CorrespondenceSettings2Service.Find(t => t.Dylx == "JXJL" && t.Dwdm == param.orgCode).Data;
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = correspondenceSettings2s
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        #endregion
    }
}
