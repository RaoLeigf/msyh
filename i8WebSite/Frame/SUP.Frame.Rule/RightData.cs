using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using i6.Biz.DMC;
using Enterprise.Common.DataAccess;
using Enterprise3.Rights.AnalyticEngine;


namespace SUP.Frame.Rule
{
    public class RightData
    {

       private FuncRightControl cpcmInterface = null;          

       private  DataTable allPageRightsDt = null;//所有web、winform权限
       private  DataTable userPageRightsDt = null;//用户权限
       private  DataTable allPbRightsDt = null;//所有pb权限
         
       private  DataTable psoftCustomReportDt = null;//psoft自定义报表

        #region property

       public string LoginConnectionStr
       {
           get;
           set;
       }
        /// <summary>
        /// 所有web、winform权限
        /// </summary>
        public  DataTable AllPageRightsDt
        {
            get
            {
                if (allPageRightsDt == null)
                {
                    allPageRightsDt = cpcmInterface.GetAllPageRights();
                    //SetAllPageRightsPrimaryKey(allPageRightsDt);
                }
                return allPageRightsDt;
            }
        }

        /// <summary>
        /// 用户权限
        /// </summary>
        public  DataTable UserPageRightsDt
        {
            get
            {
               
                if (userPageRightsDt == null)
                {
                    cpcmInterface = new FuncRightControl();
                    userPageRightsDt = cpcmInterface.GetUserInOrgRights(NG3.AppInfoBase.OrgID, NG3.AppInfoBase.UserID);
                }
                return userPageRightsDt;
            }
        }

        public DataTable UserPageRightsDtExt
        {
            get
            {
                if (userPageRightsDt == null)
                {                  
                    for (int i = 0; i < 3; i++)
                    {
                        cpcmInterface = new FuncRightControl();
                        userPageRightsDt = cpcmInterface.GetUserInOrgRights(NG3.AppInfoBase.OrgID, NG3.AppInfoBase.UserID);
                        if (userPageRightsDt == null || userPageRightsDt.Rows.Count == 0)
                        {
                            System.Threading.Thread.Sleep(500);
                        }
                        else
                        {
                            break;
                        }
                    }
                }                
                
                return userPageRightsDt;
            }
        }

        ///// <summary>
        ///// 所有pb权限
        ///// </summary>
        //public  DataTable AllPbRightsDt
        //{
        //    get
        //    {
        //        if (allPbRightsDt == null)
        //        {
        //            cpcmInterface = new FuncRightControl();
        //            allPbRightsDt = cpcmInterface.GetAllPbRights();
        //            SetAllPBRightsPrimaryKey(allPbRightsDt, Enterprise.Common.DataAccess.AppSessionConfig.GetCurrentProduct() + Enterprise.Common.DataAccess.AppSessionConfig.GetCurrentSeries());
        //        }
        //        return allPbRightsDt;
        //    }

        //}

        ///// <summary>
        ///// 所有pb权限
        ///// </summary>
        //public DataTable AllPbRightsDtExt
        //{
        //    get
        //    {
        //        if (allPbRightsDt == null)
        //        {
        //            cpcmInterface = new FuncRightControl();
        //            allPbRightsDt = cpcmInterface.GetAllPbRights();
        //            SetAllPBRightsPrimaryKey(allPbRightsDt, Enterprise.Common.DataAccess.AppSessionConfig.GetCurrentProduct() + Enterprise.Common.DataAccess.AppSessionConfig.GetCurrentSeries());
        //        }
        //        return allPbRightsDt;
        //    }

        //}


        public  DataTable PsoftCustomReportDt
        {
            get 
            {
                return psoftCustomReportDt; 
            }           
        }


        #endregion

        public RightData()
        {
           
        }

        private  void SetAllPageRightsPrimaryKey(DataTable dtAllPageRights)
        {
            DataColumn[] dataColumn = new DataColumn[4];
            dataColumn[0] = dtAllPageRights.Columns["pagename"];
            dataColumn[1] = dtAllPageRights.Columns["rightid"];
            dataColumn[2] = dtAllPageRights.Columns["moduleno"];
            dataColumn[3] = dtAllPageRights.Columns["lineid"];

            dtAllPageRights.PrimaryKey = dataColumn;
        }

        private  void SetAllPBRightsPrimaryKey(DataTable dtAllPbRights, string product)
        {
            DataRow[] drs = null;
            DataColumn[] dataColumn = new DataColumn[3];
            switch (product.ToUpper())
            {
                case "A3":
                    drs = dtAllPbRights.Select("rightname2 is null");
                    for (int i = drs.Length - 1; i >= 0; i--)
                    {
                        drs[i].Delete();
                    }
                    dtAllPbRights.AcceptChanges();
                    dataColumn[0] = dtAllPbRights.Columns["rightname2"];
                    dataColumn[1] = dtAllPbRights.Columns["moduleno"];
                    dataColumn[2] = dtAllPbRights.Columns["rightid"];
                    dtAllPbRights.PrimaryKey = dataColumn;
                    break;
                case "GE":
                    drs = dtAllPbRights.Select("rightname5 is null");
                    for (int i = drs.Length - 1; i >= 0; i--)
                    {
                        drs[i].Delete();
                    }
                    dtAllPbRights.AcceptChanges();
                    dataColumn[0] = dtAllPbRights.Columns["rightname5"];
                    dataColumn[1] = dtAllPbRights.Columns["moduleno"];
                    dataColumn[2] = dtAllPbRights.Columns["rightid"];
                    dtAllPbRights.PrimaryKey = dataColumn;
                    break;
                default:
                    drs = dtAllPbRights.Select("rightname1 is null");
                    for (int i = drs.Length - 1; i >= 0; i--)
                    {
                        drs[i].Delete();
                    }
                    dtAllPbRights.AcceptChanges();
                    dataColumn[0] = dtAllPbRights.Columns["rightname1"];
                    dataColumn[1] = dtAllPbRights.Columns["moduleno"];
                    dataColumn[2] = dtAllPbRights.Columns["rightid"];
                    dtAllPbRights.PrimaryKey = dataColumn;
                    break;
            }
        }
    }
}
