using Enterprise3.Common.Model.Results;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.GQT3.QT.Model;
using Enterprise3.WebApi.GQT3.QT.Model.Response;
using GData3.Common.Model;
using GQT3.QT.Model.Domain;
using GQT3.QT.Service.Interface;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace Enterprise3.WebApi.GQT3.QT.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter]
    public class CoverUpForOrgApiController : ApiBase
    {
        IQtCoverUpForOrgService QtCoverUpForOrgService { get; set; }

        IQtCoverUpDataService QtCoverUpDataService { get; set; }

        IQtTableCustomizeService QtTableCustomizeService { get; set; }


        /// <summary>
        /// 构造函数
        /// </summary>
        public CoverUpForOrgApiController()
        {
            QtCoverUpForOrgService = base.GetObject<IQtCoverUpForOrgService>("GQT3.QT.Service.QtCoverUpForOrg");
            QtCoverUpDataService = base.GetObject<IQtCoverUpDataService>("GQT3.QT.Service.QtCoverUpData");
        }

        /// <summary>
        /// 获取所有内置模板数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetAllCoverDataList([FromUri]BaseListModel param)
        {
            try
            {
                //内置的模板数据
                var allCoverUps = this.QtCoverUpDataService.GetQtCoverUpDataList();
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = allCoverUps,
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 获取组织对应打印模板数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetAllCoverUpList([FromUri]BaseListModel param)
        {
            if (string.IsNullOrEmpty(param.uCode))
            {
                return DCHelper.ErrorMessage("用户编码不能为空！");
            }
            if (string.IsNullOrEmpty(param.processCode))
            {
                return DCHelper.ErrorMessage("过程选择不能为空！");
            }
            try
            {

                if(param.uCode != "Admin")
                {
                    return DCHelper.ErrorMessage("非Admin用户无权限访问此功能！");
                }
                //内置的模板数据
                var allCoverUps = this.QtCoverUpDataService.GetQtCoverUpDataList();
                IList<AllCoverUpModel> allCoverUpModels = new List<AllCoverUpModel>();
                if (param.processCode.Equals("001"))
                {
                    AllCoverUpModel allCoverUpModel = new AllCoverUpModel();
                    allCoverUpModel.ProcessCode = "001";
                    allCoverUpModel.ProcessName = "预立项";
                    allCoverUpModel.QtCoverUpForOrgs = this.QtCoverUpForOrgService.GetQtCoverUpForOrgs(allCoverUpModel.ProcessCode, allCoverUpModel.ProcessName);
                    allCoverUpModels.Add(allCoverUpModel);
                }
                else if (param.processCode.Equals("002"))
                {
                    AllCoverUpModel allCoverUpModel = new AllCoverUpModel();
                    allCoverUpModel.ProcessCode = "002";
                    allCoverUpModel.ProcessName = "项目立项";
                    allCoverUpModel.QtCoverUpForOrgs = this.QtCoverUpForOrgService.GetQtCoverUpForOrgs(allCoverUpModel.ProcessCode, allCoverUpModel.ProcessName);
                    allCoverUpModels.Add(allCoverUpModel);
                }
                else if (param.processCode.Equals("003"))
                {
                    AllCoverUpModel allCoverUpModel = new AllCoverUpModel();
                    allCoverUpModel.ProcessCode = "003";
                    allCoverUpModel.ProcessName = "年中新增";
                    allCoverUpModel.QtCoverUpForOrgs = this.QtCoverUpForOrgService.GetQtCoverUpForOrgs(allCoverUpModel.ProcessCode, allCoverUpModel.ProcessName);
                    allCoverUpModels.Add(allCoverUpModel);
                }
                else if (param.processCode.Equals("004"))
                {
                    AllCoverUpModel allCoverUpModel = new AllCoverUpModel();
                    allCoverUpModel.ProcessCode = "004";
                    allCoverUpModel.ProcessName = "年中调整";
                    allCoverUpModel.QtCoverUpForOrgs = this.QtCoverUpForOrgService.GetQtCoverUpForOrgs(allCoverUpModel.ProcessCode, allCoverUpModel.ProcessName);
                    allCoverUpModels.Add(allCoverUpModel);
                }
                //else if (param.processCode.Equals("003"))
                //{
                //    AllCoverUpModel allCoverUpModel = new AllCoverUpModel();
                //    allCoverUpModel.ProcessCode = "003";
                //    allCoverUpModel.ProcessName = "项目立项申报";
                //    allCoverUpModel.QtCoverUpForOrgs = this.QtCoverUpForOrgService.GetQtCoverUpForOrgs(allCoverUpModel.ProcessCode, allCoverUpModel.ProcessName);
                //    allCoverUpModels.Add(allCoverUpModel);
                //}
                //else if (param.processCode.Equals("004"))
                //{
                //    AllCoverUpModel allCoverUpModel = new AllCoverUpModel();
                //    allCoverUpModel.ProcessCode = "004";
                //    allCoverUpModel.ProcessName = "项目立项汇总";
                //    allCoverUpModel.QtCoverUpForOrgs = this.QtCoverUpForOrgService.GetQtCoverUpForOrgs(allCoverUpModel.ProcessCode, allCoverUpModel.ProcessName);
                //    allCoverUpModels.Add(allCoverUpModel);
                //}
                else
                {
                    //(获取四个过程项)
                    AllCoverUpModel allCoverUpModel1 = new AllCoverUpModel();
                    allCoverUpModel1.ProcessCode = "001";
                    allCoverUpModel1.ProcessName = "预立项";
                    allCoverUpModel1.QtCoverUpForOrgs = this.QtCoverUpForOrgService.GetQtCoverUpForOrgs(allCoverUpModel1.ProcessCode, allCoverUpModel1.ProcessName);
                    allCoverUpModels.Add(allCoverUpModel1);
                    AllCoverUpModel allCoverUpModel2 = new AllCoverUpModel();
                    allCoverUpModel2.ProcessCode = "002";
                    allCoverUpModel2.ProcessName = "项目立项";
                    allCoverUpModel2.QtCoverUpForOrgs = this.QtCoverUpForOrgService.GetQtCoverUpForOrgs(allCoverUpModel2.ProcessCode, allCoverUpModel2.ProcessName);
                    allCoverUpModels.Add(allCoverUpModel2);
                    AllCoverUpModel allCoverUpModel3 = new AllCoverUpModel();
                    allCoverUpModel3.ProcessCode = "003";
                    allCoverUpModel3.ProcessName = "年中新增";
                    allCoverUpModel3.QtCoverUpForOrgs = this.QtCoverUpForOrgService.GetQtCoverUpForOrgs(allCoverUpModel3.ProcessCode, allCoverUpModel3.ProcessName);
                    allCoverUpModels.Add(allCoverUpModel3);
                    AllCoverUpModel allCoverUpModel4 = new AllCoverUpModel();
                    allCoverUpModel4.ProcessCode = "004";
                    allCoverUpModel4.ProcessName = "年中调整";
                    allCoverUpModel4.QtCoverUpForOrgs = this.QtCoverUpForOrgService.GetQtCoverUpForOrgs(allCoverUpModel4.ProcessCode, allCoverUpModel4.ProcessName);
                    allCoverUpModels.Add(allCoverUpModel4);
                }
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = allCoverUpModels,
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 修改所有组织的套打数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostUpdateCoverUpList([FromBody]BaseInfoModel<List<AllCoverUpModel>> param)
        {
            if (string.IsNullOrEmpty(param.uCode))
            {
                return DCHelper.ErrorMessage("用户编码不能为空！");
            }
            if(param.infoData == null && param.infoData.Count <=0)
            {
                return DCHelper.ErrorMessage("参数传递不能为空！");
            }
            try
            {
                if (param.uCode != "Admin")
                {
                    return DCHelper.ErrorMessage("非Admin用户无权限访问此功能！");
                }
                var result = this.QtCoverUpForOrgService.UpdateCoverUpList(param.infoData);
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 根据过程组织获取对应的打印格式
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetCoverUpByOrg([FromUri]BaseListModel param)
        {
            if (string.IsNullOrEmpty(param.orgid) || string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织主键及组织编码不能为空！");
            }
            if (string.IsNullOrEmpty(param.processCode))
            {
                return DCHelper.ErrorMessage("过程选择不能为空！");
            }
            try
            {
                var result = this.QtCoverUpForOrgService.GetCoverUpByOrg(long.Parse(param.orgid), param.processCode);
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = result
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        #region//关于表格列表自定义

        /// <summary>
        /// 修改列表自定义集合
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostUpdateTableList([FromBody]BaseInfoModel<List<QtTableCustomizeModel>> param)
        {
            if (string.IsNullOrEmpty(param.uid) || string.IsNullOrEmpty(param.uCode))
            {
                return DCHelper.ErrorMessage("用户信息不能为空！");
            }
            if (string.IsNullOrEmpty(param.TableCode) || string.IsNullOrEmpty(param.TableName))
            {
                return DCHelper.ErrorMessage("列表信息不能为空！");
            }
            if(param.infoData == null || param.infoData.Count < 1)
            {
                return DCHelper.ErrorMessage("传递的数据不能为空！");
            }
            try
            {
                List<QtTableCustomizeModel> qtTableCustomizes = new List<QtTableCustomizeModel>();
                qtTableCustomizes = param.infoData;
                SavedResult<long> savedResult = new SavedResult<long>();
                if(qtTableCustomizes != null && qtTableCustomizes.Count > 0)
                {
                    foreach (var table in qtTableCustomizes)
                    {
                        table.UserId = long.Parse(param.uid);
                        table.UserCode = param.uCode;
                        table.TableCode = param.TableCode;
                        table.TableName = param.TableName;
                        if(table.PhId == 0)
                        {
                            table.PersistentState = PersistentState.Added;
                        }
                        else
                        {
                            if(table.PersistentState != PersistentState.Deleted)
                            {
                                table.PersistentState = PersistentState.Modified;
                            }
                        }
                    }
                    savedResult = this.QtTableCustomizeService.UpdateQtTableCustomizes(qtTableCustomizes);
                }
                return DataConverterHelper.SerializeObject(savedResult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 根据用户与表编码获取该表格所有列是否显示的数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetTableList([FromUri]BaseListModel param)
        {
            if (string.IsNullOrEmpty(param.uid))
            {
                return DCHelper.ErrorMessage("用户账号不能为空！");
            }
            if (string.IsNullOrEmpty(param.TableCode))
            {
                return DCHelper.ErrorMessage("列表编码不能为空！");
            }
            try
            {
                var result = this.QtTableCustomizeService.GetQtTableCustomizes(long.Parse(param.uid), param.TableCode);
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = result
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        #endregion
    }
}