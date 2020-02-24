#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectThresholdService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        ProjectThresholdService.cs
    * 创建时间：        2018/10/17 
    * 作    者：        李长敏琛    
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
using SUP.Common.Base;

namespace GQT3.QT.Service
{
	/// <summary>
	/// ProjectThreshold服务组装处理类
	/// </summary>
    public partial class ProjectThresholdService : EntServiceBase<ProjectThresholdModel>, IProjectThresholdService
    {
		#region 类变量及属性
		/// <summary>
        /// ProjectThreshold业务外观处理对象
        /// </summary>
		IProjectThresholdFacade ProjectThresholdFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IProjectThresholdFacade;
            }
        }
        #endregion

        #region 实现 IProjectThresholdService 业务添加的成员

        public  bool SaveOrUpdate(string data, IList<OrganizeModel> organizes,IList<ProjectThresholdModel> projectThresholds,DataStoreParam dataStoreParam) {
            if (data.EndsWith("|"))
            {
                data = data.Substring(0, data.Length - 1);
            }
            /** 数据格式
                * '|':元素分割符
                * ':':属性分割符
                * ...|PhId:Orgcode:FThreshold|...
                **/
            String[] Items = data.Split('|');
            foreach (String item in Items) {
                //取到item，即VProjectThresholdModel的序列化对象
                String[] attrs = item.Split(':');
                ProjectThresholdModel p1;
                if (attrs[0].Equals("0") && !attrs[2].Equals("-1"))
                {
                    var os = from lt1 in organizes
                             where lt1.OCode == attrs[1]
                             select lt1;
                    p1 = new ProjectThresholdModel();
                    OrganizeModel o;
                    if (os.Count() == 1)
                    {
                        o = os.ToList()[0];
                    }
                    else
                    {
                        throw new Exception("组织代码不唯一");
                    }
                    p1.Orgid = o.PhId;
                    p1.Orgcode = attrs[1];
                    p1.FThreshold = attrs[2];
                    p1.ProjTypeId = attrs[3];
                    p1.ProjTypeName = attrs[4];
                    p1.PersistentState = PersistentState.Added;
                    SavedResult<Int64> t= this.ProjectThresholdFacade.Save<Int64>(p1);
                }
                if (!attrs[0].Equals("0") && !attrs[2].Equals("-1")) {
                    //不可以用上一个context读取的对象用在新的context 中做保存，内置session对象不一致
                    p1 = (from lt1 in projectThresholds
                          where lt1.PhId == Int64.Parse(attrs[0])
                          select lt1).ToList()[0];
                    p1.FThreshold = attrs[2];
                    p1.ProjTypeId = attrs[3];
                    p1.ProjTypeName = attrs[4];
                    p1.PersistentState = PersistentState.Modified;
                    
                    SavedResult<Int64> t=this.Save<Int64>(p1,"");
                    //this.ProjectThresholdFacade.Save<Int64>(p1);
                }
            }
            return true;
        }

        public IList<OrganizeModel> GetSBOrganizes(IList<OrganizeModel> organizes, IList<CorrespondenceSettings2Model> correspondenceSettings2s) {
            IList<string> phids = new List<string>();
            foreach (CorrespondenceSettings2Model csm in correspondenceSettings2s) {
                if (csm.Dylx.Equals("SB")) {
                    phids.Add(csm.DefStr2);
                }
            }
            return (from ot1 in organizes
                    where phids.Contains(ot1.PhId.ToString())
                    select ot1).ToList();
        }
        #endregion
    }
}

