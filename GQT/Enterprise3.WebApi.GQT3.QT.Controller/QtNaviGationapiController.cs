using Enterprise3.Common.Base.Criterion;
using Enterprise3.Common.Model.Results;
using Enterprise3.WebApi.ApiControllerBase;
using GQT3.QT.Model.Domain;
using GQT3.QT.Service.Interface;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Enterprise3.WebApi.GQT3.QT.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter]
    public class QtNaviGationapiController : ApiBase
    {
        IQtNaviGationService QtNaviGationService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public QtNaviGationapiController()
        {
            QtNaviGationService = base.GetObject<IQtNaviGationService>("GQT3.QT.Service.QtNaviGation");
        }
        /// <summary>
        /// 按钮自定排序获取数据
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public string GetNaviGation(long Operator)
        {
            var dic = new Dictionary<string, object>();
            new CreateCriteria(dic).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
            new CreateCriteria(dic).Add(ORMRestrictions<Int64>.Eq("Operator", Operator));
            var Query = QtNaviGationService.Find(dic);
            return DataConverterHelper.SerializeObject(Query);
        }


        /// <summary>
        /// 按钮自定排序修改按钮的排序位置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetNaviCode([FromUri]long Operator)
        {
            if (Operator != 0)
            {
                var dic = new Dictionary<string, object>();
                new CreateCriteria(dic).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                new CreateCriteria(dic).Add(ORMRestrictions<Int64>.Eq("Operator", Operator));
                var Query = QtNaviGationService.Find(dic);
                if (Query.Data.Count > 0)
                {
                    var area1 = Query.Data.Where(m => m.Invisible == 1).OrderBy(m=>m.Sortvalue).ToList();
                    var area2 = Query.Data.Where(m => m.Invisible == 2).OrderBy(m => m.Sortvalue).ToList();
                    var data = new
                    {
                        area1,
                        area2
                    };
                    return DataConverterHelper.SerializeObject(data);
                }
                else
                {
                    List<QtNaviGationModel> qtlist = new List<QtNaviGationModel>();
                    QtNaviGationModel model = new QtNaviGationModel();
                    SavedResult<Int64> savedResult = new SavedResult<long>();
                    var list = new List<QtNaviGationModel>
                    {
                        new QtNaviGationModel{Sortvalue=1,Buttoncode="projectmanage",Operator=Operator,PersistentState=PersistentState.Added,Invisible=2,Name="预算项目",Menu="projectmanage",Srcs="yunsuan"},
                        new QtNaviGationModel{Sortvalue=2,Buttoncode="fund",Operator=Operator,PersistentState=PersistentState.Added,Invisible=2,Name="资金拨付",Menu="fund",Srcs="zjbf"},
                        new QtNaviGationModel{Sortvalue=3,Buttoncode="approvalcenter",Operator=Operator,PersistentState=PersistentState.Added,Invisible=2,Name="审批中心",Menu="approvalcenter",Srcs="spzx"},
                        new QtNaviGationModel{Sortvalue=4,Buttoncode="paycenter",Operator=Operator,PersistentState=PersistentState.Added,Invisible=2,Name="支付中心",Menu="paycenter",Srcs="zfzx"},
                        new QtNaviGationModel{Sortvalue=5,Buttoncode="bankaccount",Operator=Operator,PersistentState=PersistentState.Added,Invisible=2,Name="银行档案",Menu="bankaccount",Srcs="yhdn"  },
                        new QtNaviGationModel{Sortvalue=6,Buttoncode="setting",Operator=Operator,PersistentState=PersistentState.Added,Invisible=2,Name="系统设置",Menu="sysSetting",Srcs="xtsz"},
                        new QtNaviGationModel{Sortvalue=7,Buttoncode="projectspent",Operator=Operator,PersistentState=PersistentState.Added,Invisible=2,Name="项目用款",Menu="projectspent",Srcs="xmyk"},
                    };
                    #region 通过反射的方式获取object的属性数据
                    // foreach (var item in list)
                    //{
                    //Type type = item.GetType();
                    //var pro = type.GetProperties();

                    //for (int i = 0, j = pro.Length; i < j; i++)
                    //{
                    //    if (pro[i].CanRead)
                    //    {
                    //        try
                    //        {
                    //            var o = pro[i].GetValue(item, null);
                    //            if (o != null)
                    //            {
                    //                if (o.GetType() == typeof(int))
                    //                {
                    //                    model.Sortvalue = (int)o;
                    //                }
                    //                else if (o.GetType() == typeof(string))
                    //                {
                    //                    model.Buttoncode = o.ToString();

                    //                }
                    //                else if (o.GetType() == typeof(long))
                    //                {
                    //                    model.Operator = (long)o;
                    //                }
                    //                model.Invisible = 2;
                    //                model.PersistentState = PersistentState.Added;
                    //            }
                    //            else
                    //            {
                    //                return DCHelper.ErrorMessage("数据赋值失败");
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            return DCHelper.ErrorMessage(ex.ToString());
                    //        }
                    //    }
                    //}
                    //qtlist.Add(model);
                    // }
                    #endregion
                    try
                    {
                        savedResult = QtNaviGationService.Save<Int64>(list, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        return DCHelper.ErrorMessage(ex.ToString());
                    }
                    var QueryS = QtNaviGationService.Find(dic);
                    if (QueryS.Data.Count > 0)
                    {
                        var area1 = Query.Data.Where(m => m.Invisible == 2).OrderBy(m=>m.Sortvalue);
                        var area2 = Query.Data.Where(m => m.Invisible == 1).OrderBy(m=>m.Sortvalue);
                        var data = new
                        {
                            area1,
                            area2
                        };
                        return DataConverterHelper.SerializeObject(data);
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("数据列表获取失败");
                    }
                }
            }
            else
            {
                return DCHelper.ErrorMessage("参数传递失败");

            }

        }

        /// <summary> 
        /// 按钮自定排序根据数组修改按钮的排序
        /// </summary>
        /// <param name="qtNaviGationdtl"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public string PostNaviSort([FromBody]QtNaviGationdtlModel qtNaviGationdtl)
        {
            List<QtNaviGationModel> list = new List<QtNaviGationModel>();
            SavedResult<Int64> savedResult = new SavedResult<Int64>();
            var area1 = qtNaviGationdtl.area1;
            var area2 = qtNaviGationdtl.area2;
            var index = 0;
            if (area1 != null)
            {
                for (int i = 0; i < area1.Count(); i++)
                {
                    QtNaviGationModel model = new QtNaviGationModel
                    {
                        Buttoncode = area1[i].Buttoncode,
                        Invisible = area1[i].Invisible,
                        PhId = area1[i].PhId,
                        Name = area1[i].Name,
                        Menu = area1[i].Menu,
                        Srcs = area1[i].Srcs,
                        Sortvalue = i + 1,
                        Operator = area1[i].Operator,
                        Creator = area1[i].Creator,
                        Createtime =area1[i].Createtime,
                        Editor= area1[i].Editor,
                        CurOrgId =area1[i].CurOrgId,
                        NgInsertDt=area1[i].NgInsertDt,
                        NgUpdateDt=area1[i].NgUpdateDt,
                        NgRecordVer = area1[i].NgRecordVer,
                        Modifier= area1[i].Modifier,
                        Modifiertime= area1[i].Modifiertime,
                        PersistentState = PersistentState.Modified
                    };
                    list.Add(model);
                    index = area1.Count();
                }
            }
            if (area2 != null)
            {
                for (int j = 0; j < area2.Count(); j++)
                {
                    QtNaviGationModel model = new QtNaviGationModel
                    {
                        Buttoncode = area2[j].Buttoncode,
                        Sortvalue = index + j + 1,
                        Invisible = area2[j].Invisible,
                        PhId = area2[j].PhId,
                        Name = area2[j].Name,
                        Menu = area2[j].Menu,
                        Srcs = area2[j].Srcs,
                        Operator = area2[j].Operator,
                        Creator = area2[j].Creator,
                        Createtime = area2[j].Createtime,
                        Editor = area2[j].Editor,
                        CurOrgId = area2[j].CurOrgId,
                        NgInsertDt = area2[j].NgInsertDt,
                        NgUpdateDt = area2[j].NgUpdateDt,
                        NgRecordVer = area2[j].NgRecordVer,
                        Modifier = area2[j].Modifier,
                        Modifiertime = area2[j].Modifiertime,
                        PersistentState = PersistentState.Modified
                    };
                    list.Add(model);
                }
            }
            foreach (var item in list)
            {
                var dic = new Dictionary<string, object>();
                new CreateCriteria(dic).Add(ORMRestrictions<Int64>.Eq("PhId", item.PhId));
                var Query = QtNaviGationService.Find(dic);
                if (Query.Data.Count < 0)
                {
                    savedResult.Status = ResponseStatus.Error;
                    savedResult.Msg = "找不到匹配数据";
                }
            }
            try
            {
                var Result = QtNaviGationService.Save<Int64>(list,"");
            }
            catch (Exception ex)
            {

                savedResult.Status = ResponseStatus.Error;
                savedResult.Msg = ex.Message.ToString();
            }
            return DataConverterHelper.SerializeObject(savedResult); ;

        }
    }
}
