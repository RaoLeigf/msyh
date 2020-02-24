using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web.Services.Protocols;
using System.Web.Services;
using System.Web;
using System.Xml.Serialization;
using Newtonsoft.Json;
using NG3.Cache.Client;
using NG3.Cache.Interface;
using NG3.MemCached.Client;
using NG3.Log.Log4Net;

namespace SUP.NSServer
{
    /// <summary>
    /// 
    /// </summary>
    public class NGCOM
    {
        private static object syncLock = new object();
        private static object moduleBuyLock = new object();

        #region 单件构造
        static private NGCOM _instance;
        static public NGCOM Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncLock)
                    {
                        _instance = new NGCOM();
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region 定义
        private NGNSServerService NSServerProxy = new NGNSServerService();// NSServer的代理
        private NG3.LzwZip lzwZipService = new NG3.LzwZip();// 解压缩工具

        private const string NG_LEGAL_SN = "NG_LEGAL_SN";//产品序列号缓存
        private const string NG_LEGAL_USERNAME = "NG_LEGAL_USERNAME";//合法用户缓存键
        private const string NG_LEGAL_HREmployees = "NG_LEGAL_HREmployees";//员工数
        private const string NG_LEGAL_WmForm = "NG_LEGAL_WmForm";//表单模板数
        private const string NG_LEGAL_RW = "NG_LEGAL_RW";//报表仓库报表记录数
        private const string NG_LEGAL_IC = "NG_LEGAL_IC";//指标中心自定义指标记录数
        private const string NG_LEGAL_OAUSERS = "NG_LEGAL_OAUSERS";//OA用户数
        private const string NG_LEGAL_ALLUSERS = "NG_LEGAL_ALLUSERS";//全模块用户数
        private const string NG_LEGAL_MUSERS = "NG_LEGAL_MUSERS";//移动应用用户数（并发）
        private const string NG_LEGAL_MODULEBUYLIST = "NG_LEGAL_MODULEBUYLIST";//模块是否购买
        private const string NG_LEGAL_MODULEINSTALLLIST = "NG_LEGAL_MODULEINSTALLLIST";//安装模块
        private const string NG_LEGAL_MAINMODULEINSTALLEDLIST = "NG_LEGAL_MAINMODULEINSTALLEDLIST";//主模块        
        private const string NG_LEGAL_MODULERIGHTSCOUNT = "NG_LEGAL_MODULERIGHTSCOUNT";//组织模块授权缓存键
        private const int defaultOAUsers = 3;//OA默认用户数
        private const int defaultAllUsers = 2;//全模块默认用户数
        private const int defaultMUsers = 2;//移动应用用户数（并发）

        private string m_nsserverAddr = "";//默认为空
        private string m_product = "";//默认为空
        private string m_productCode = "";//默认为空
        private string m_DatFile = "";//默认为空

        private string sInstalledMainModluesRegeditItem = "";//主模块安装注册表串
        private string sInstalledModulesRegeditItem = "";//模块安装注册表串
        #endregion

        #region 日志定义
        private ILogger _logger = null;
        private ILogger Logger
        {
            get
            {
                if (_logger == null)
                {                    
                    _logger = Log4NetLoggerFactory.Instance.CreateLogger(typeof(NGCOM), LogType.logoperation);
                }
                return _logger;
            }
        }
        #endregion

        #region 模块定义
        private Hashtable _htAllModules;
        public Hashtable HtAllModules
        {
            get
            {
                if (_htAllModules == null)
                {
                    _htAllModules = new Hashtable();

                    if (this.Product.ToUpper() == "A3")
                    {
                        #region 增加A3模块
                        _htAllModules.Add("WM.IWP", "WM.IWP");//协同办公-->共享平台
                        _htAllModules.Add("WM.OM", "WM.OM");//协同办公-->办公事务管理
                        _htAllModules.Add("WM.CM", "WM.CM");//协同办公-->证书管理
                        _htAllModules.Add("WM.PRM", "WM.PRM");//协同办公-->公共关系管理
                        _htAllModules.Add("WM.KM", "WM.KM");//协同办公-->文档知识管理
                        _htAllModules.Add("WM.DOS", "WM.DOS");//协同办公-->档案管理
                        _htAllModules.Add("WM.FC", "WM.FC");//协同办公-->自定义表单管理
                        _htAllModules.Add("GFI.GC", "GFI.GC");//财务管理-->账务中心
                        _htAllModules.Add("GFI.FR", "GFI.FR");//财务管理-->报表中心
                        _htAllModules.Add("GFI.GM", "GFI.GM");//财务管理-->集团管理
                        _htAllModules.Add("GFI.EBP", "GFI.EBP");//财务管理-->网银平台
                        _htAllModules.Add("EAM.EAM", "EAM.EAM");//资产管理-->资产平台
                        _htAllModules.Add("EAM.FA", "EAM.FA");//资产管理-->资产核算
                        _htAllModules.Add("EAM.AP", "EAM.AP");//资产管理-->资产采购
                        _htAllModules.Add("CRM.BOM", "CRM.BOM");//客户关系管理-->商机管理
                        _htAllModules.Add("CRM.CIM", "CRM.CIM");//客户关系管理-->客户信息管理 
                        _htAllModules.Add("CRM.CSM", "CRM.CSM");//客户关系管理-->客户服务管理
                        _htAllModules.Add("CRM.SSM", "CRM.SSM");//客户关系管理-->基础数据
                        _htAllModules.Add("PMS.PI", "PMS.PI");//项目管理-->项目信息
                        _htAllModules.Add("PMS.PP", "PMS.PP");//项目管理-->项目进度计划
                        _htAllModules.Add("PMS.PD", "PMS.PD");//项目管理-->项目文档
                        _htAllModules.Add("PMS.PMSBD", "PMS.PMSBD");//项目管理-->基础数据
                        _htAllModules.Add("PA.PASA", "PA.PASA");//合同管理-->销售合同
                        _htAllModules.Add("PA.PAPO", "PA.PAPO");//合同管理-->采购合同
                        _htAllModules.Add("PA.PABD", "PA.PABD");//合同管理-->合同基础设置
                        _htAllModules.Add("LG.LGPO", "LG.LGPO");//供应链管理-->采购管理
                        _htAllModules.Add("LG.LGSA", "LG.LGSA");//供应链管理-->销售管理
                        _htAllModules.Add("LG.LGIN", "LG.LGIN");//供应链管理-->库存管理
                        _htAllModules.Add("LG.POS", "LG.POS");//供应链管理-->专卖零售协同管理
                        _htAllModules.Add("PM.MPS", "PM.MPS");//生产计划-->主生产计划
                        _htAllModules.Add("PM.MRP", "PM.MRP");//生产计划-->物料需求计划
                        _htAllModules.Add("PM.AOM", "PM.AOM");//生产计划-->敏捷询单管理
                        _htAllModules.Add("PM.MDM", "PM.MDM");//生产计划-->制造数据管理
                        _htAllModules.Add("PM.PPC", "PM.PPC");//生产计划-->参数化产品配置
                        _htAllModules.Add("MES.SFC", "MES.SFC");//制造执行-->车间作业控制
                        _htAllModules.Add("MES.CAQ", "MES.CAQ");//制造执行-->产品质量管理
                        _htAllModules.Add("MES.RMM", "MES.RMM");//制造执行-->重复式生产管理
                        _htAllModules.Add("CO.CA", "CO.CA");//成本管理-->成本核算
                        _htAllModules.Add("CO.PCA", "CO.PCA");//成本管理-->标准制造成本
                        _htAllModules.Add("CO.DCM", "CO.DCM");//成本管理-->灵动制造成本
                        _htAllModules.Add("PMC.PH", "PMC.PH");//项管控中心-->项目人员
                        _htAllModules.Add("PMC.PS", "PMC.PS");//项目管控中心-->项目监理
                        _htAllModules.Add("HR.EMA", "HR.EMA");//人力资源-->员工台账
                        _htAllModules.Add("HR.EMI", "HR.EMI");//人力资源-->人事档案
                        _htAllModules.Add("HR.EGM", "HR.EGM");//人力资源-->员工事务
                        _htAllModules.Add("HR.TIM", "HR.TIM");//人力资源-->考勤管理
                        _htAllModules.Add("HR.PAM", "HR.PAM");//人力资源-->绩效管理
                        _htAllModules.Add("HR.PBM", "HR.PBM");//人力资源-->薪资福利
                        _htAllModules.Add("HR.RIM", "HR.RIM");//人力资源-->招聘管理
                        _htAllModules.Add("HR.TDM", "HR.TDM");//人力资源-->培训管理
                        _htAllModules.Add("HR.ECM", "HR.ECM");//人力资源-->员工关系
                        _htAllModules.Add("HR.OGM", "HR.OGM");//人力资源-->组织管理
                        _htAllModules.Add("SUP.BM", "SUP.BM");//柔性统一平台-->业务建模
                        _htAllModules.Add("SUP.BPM", "SUP.BPM");//柔性统一平台-->流程管理
                        _htAllModules.Add("SUP.ALM", "SUP.ALM");//柔性统一平台-->预警管理
                        _htAllModules.Add("SUP.RW", "SUP.RW");//柔性统一平台-->报表仓库
                        _htAllModules.Add("SUP.IC", "SUP.IC");//柔性统一平台-->数据窗口
                        _htAllModules.Add("SUP.UC", "SUP.UC");//柔性统一平台-->统一通讯平台
                        _htAllModules.Add("SUP.UIP", "SUP.UIP");//柔性统一平台-->万向接口平台
                        _htAllModules.Add("I6BASE.DMC", "I6BASE.DMC");//基础数据-->企业级基础数据
                        _htAllModules.Add("I6BASE.DC", "I6BASE.DC");//基础数据-->企业级权限中心
                        #endregion
                    }
                    else if (this.Product.ToUpper() == "GE")
                    {
                        #region 增加GE模块
                        _htAllModules.Add("WM.IWP", "WM.IWP");//协同办公-->共享平台
                        _htAllModules.Add("WM.KM", "WM.KM");//协同办公-->文档管理
                        _htAllModules.Add("WM.FC", "WM.FC");//协同办公-->办公表单管理
                        _htAllModules.Add("GFI.GC", "GFI.GC");//财务管理-->账务中心
                        _htAllModules.Add("GFI.FR", "GFI.FR");//财务管理-->报表中心
                        _htAllModules.Add("EAM.FA", "EAM.FA");//资产管理-->资产核算
                        _htAllModules.Add("CRM.BOM", "CRM.BOM");//客户关系管理-->商机管理
                        _htAllModules.Add("CRM.CIM", "CRM.CIM");//客户关系管理-->客户信息管理 
                        _htAllModules.Add("CRM.SSM", "CRM.SSM");//客户关系管理-->基础数据
                        _htAllModules.Add("PA.PASA", "PA.PASA");//合同管理-->销售合同
                        _htAllModules.Add("PA.PAPO", "PA.PAPO");//合同管理-->采购合同
                        _htAllModules.Add("PA.PABD", "PA.PABD");//合同管理-->合同基础设置
                        _htAllModules.Add("LG.LGPO", "LG.LGPO");//供应链管理-->采购管理
                        _htAllModules.Add("LG.LGSA", "LG.LGSA");//供应链管理-->销售管理
                        _htAllModules.Add("LG.LGIN", "LG.LGIN");//供应链管理-->库存管理
                        _htAllModules.Add("PM.MRP", "PM.MRP");//生产计划-->生产计划
                        _htAllModules.Add("PM.SFC", "PM.SFC");//制造执行-->工单管理
                        _htAllModules.Add("PM.MDM", "PM.MDM");//生产计划-->制造数据管理
                        _htAllModules.Add("CO.DCM", "CO.DCM");//成本管理-->灵动制造成本
                        _htAllModules.Add("HR.EGM", "HR.EGM");//人力资源-->劳动合同
                        _htAllModules.Add("HR.PBM", "HR.PBM");//人力资源-->薪资福利
                        _htAllModules.Add("SUP.BM", "SUP.BM");//柔性统一平台-->业务建模
                        _htAllModules.Add("SUP.BPM", "SUP.BPM");//柔性统一平台-->流程管理
                        _htAllModules.Add("SUP.ALM", "SUP.ALM");//柔性统一平台-->预警管理
                        _htAllModules.Add("SUP.UC", "SUP.UC");//柔性统一平台-->统一通讯平台
                        _htAllModules.Add("SUP.UIP", "SUP.UIP");//柔性统一平台-->万向接口平台
                        _htAllModules.Add("I6BASE.DMC", "I6BASE.DMC");//基础数据-->企业级基础数据
                        _htAllModules.Add("I6BASE.DC", "I6BASE.DC");//基础数据-->企业级权限中心
                        #endregion
                    }
                    else if (this.Product.ToUpper() == "I6" || this.Product.ToUpper() == "I6P")
                    {
                        #region 柔性统一平台、基础数据
                        _htAllModules.Add("SUP.BM", "SUP.BM");//柔性统一平台-->业务建模
                        _htAllModules.Add("SUP.BPM", "SUP.BPM");//柔性统一平台-->流程管理
                        _htAllModules.Add("SUP.ALM", "SUP.ALM");//柔性统一平台-->预警管理
                        _htAllModules.Add("SUP.UC", "SUP.UC");//柔性统一平台-->统一通讯平台
                        _htAllModules.Add("SUP.UIP", "SUP.UIP");//柔性统一平台-->万向接口平台
                        _htAllModules.Add("I6BASE.DMC", "I6BASE.DMC");//基础数据-->企业级基础数据
                        _htAllModules.Add("I6BASE.DC", "I6BASE.DC");//基础数据-->企业级权限中心
                        #endregion

                        #region 协同办公
                        _htAllModules.Add("WM.IWP", "WM.IWP");      //协同办公-->自助平台
                        _htAllModules.Add("WM.OM", "WM.OM");        //协同办公-->办公事务管理
                        _htAllModules.Add("WM.CM", "WM.CM");        //协同办公-->证书管理
                        _htAllModules.Add("WM.PRM", "WM.PRM");      //协同办公-->公共关系管理
                        _htAllModules.Add("WM.KM", "WM.KM");        //协同办公-->文档知识管理
                        _htAllModules.Add("WM.DOS", "WM.DOS");      //协同办公-->档案管理
                        _htAllModules.Add("WM.FC", "WM.FC");        //协同办公-->自定义表单管理
                        _htAllModules.Add("WM.NEWS", "WM.NEWS");    //协同办公-->新闻公告
                        #endregion

                        #region 人力资源
                        _htAllModules.Add("HR.EMA", "HR.EMA");      //人力资源-->员工台账
                        _htAllModules.Add("HR.EMI", "HR.EMI");      //人力资源-->人事档案
                        _htAllModules.Add("HR.EGM", "HR.EGM");      //人力资源-->员工事务
                        _htAllModules.Add("HR.YG", "HR.YG");        //人力资源-->用工管理[i6P]
                        _htAllModules.Add("HR.RG", "HR.RG");        //人力资源-->劳务管理[i6P]
                        _htAllModules.Add("HR.TIM", "HR.TIM");      //人力资源-->考勤管理
                        _htAllModules.Add("HR.PAM", "HR.PAM");      //人力资源-->绩效管理
                        _htAllModules.Add("HR.PBM", "HR.PBM");      //人力资源-->薪资福利
                        _htAllModules.Add("HR.RIM", "HR.RIM");      //人力资源-->招聘管理
                        _htAllModules.Add("HR.TDM", "HR.TDM");      //人力资源-->培训管理
                        _htAllModules.Add("HR.ECM", "HR.ECM");      //人力资源-->员工关系
                        _htAllModules.Add("HR.HRP", "HR.HRP");      //人力资源-->资源规划
                        _htAllModules.Add("HR.CMM", "HR.CMM");      //人力资源-->能力素质
                        _htAllModules.Add("HR.CDM", "HR.CDM");      //人力资源-->职业发展
                        _htAllModules.Add("HR.MCA", "HR.MCA");      //人力资源-->成本分析
                        _htAllModules.Add("HR.OGM", "HR.OGM");      //人力资源-->组织管理
                        #endregion

                        #region 集团财务
                        _htAllModules.Add("GFI.GC", "GFI.GC");      //集团财务-->账务中心
                        _htAllModules.Add("GFI.FR", "GFI.FR");      //集团财务-->报表中心
                        _htAllModules.Add("GFI.BU", "GFI.BU");      //集团财务-->全面预算
                        _htAllModules.Add("GFI.GM", "GFI.GM");      //集团财务-->集团管理
                        _htAllModules.Add("GFI.EBP", "GFI.EBP");    //集团财务-->网银平台
                        _htAllModules.Add("GFI.BD", "GFI.BD");      //集团财务-->基础数据
                        #endregion

                        #region 资产管理
                        _htAllModules.Add("EAM.EAM", "EAM.EAM");    //资产管理-->资产平台
                        _htAllModules.Add("EAM.FA", "EAM.FA");      //资产管理-->资产核算
                        _htAllModules.Add("EAM.AP", "EAM.AP");      //资产管理-->资产采购
                        _htAllModules.Add("EAM.EJM", "EAM.EJM");    //资产管理-->资产租赁[i6P]
                        _htAllModules.Add("EAM.EMM", "EAM.EMM");    //资产管理-->设备管理[i6P]
                        _htAllModules.Add("EAM.PEMM", "EAM.PEMM");  //资产管理-->工程设备管理
                        #endregion

                        #region 资金管控中心
                        _htAllModules.Add("FPM.EZJ", "FPM.EZJ");    //资金管控中心-->资金计划[i6P]
                        _htAllModules.Add("FPM.ZJ", "FPM.ZJ");      //资金管控中心-->项目资金计划[i6P]
                        _htAllModules.Add("FPM.QT", "FPM.QT");      //资金管控中心-->结算中心
                        _htAllModules.Add("FPM.BCC", "FPM.BCC");    //资金管控中心-->高级费用资金计划
                        _htAllModules.Add("FPM.SCC", "FPM.SCC");    //资金管控中心-->标准费用资金计划
                        #endregion

                        #region 企业管控中心
                        _htAllModules.Add("EPM.MC", "EPM.MC");      //企业管控中心-->管理驾驶舱
                        _htAllModules.Add("EPM.MA", "EPM.MA");      //企业管控中心-->经营分析
                        _htAllModules.Add("EPM.IC", "EPM.IC");      //企业管控中心-->指标中心
                        _htAllModules.Add("EPM.RW", "EPM.RW");      //企业管控中心-->报表仓库
                        _htAllModules.Add("EPM.RMC", "EPM.RMC");    //企业管控中心-->网络报表
                        _htAllModules.Add("EPM.EPMMQ", "EPM.EPMMQ");//企业管控中心-->经理查询
                        _htAllModules.Add("EPM.EPMDMC", "EPM.EPMDMC");//企业管控中心-->基础数据设置
                        #endregion

                        if (this.Product.ToUpper() == "I6")
                        {
                            #region 其它i6模块
                            _htAllModules.Add("CRM.BOM", "CRM.BOM");//客户关系管理-->商机管理
                            _htAllModules.Add("CRM.MPM", "CRM.MPM");//客户关系管理-->市场营销管理
                            _htAllModules.Add("CRM.CIM", "CRM.CIM");//客户关系管理-->客户信息管理 
                            _htAllModules.Add("CRM.DPM", "CRM.DPM");//客户关系管理-->渠道管理
                            _htAllModules.Add("CRM.CSM", "CRM.CSM");//客户关系管理-->客户服务管理
                            _htAllModules.Add("CRM.CLM", "CRM.CLM");//客户关系管理-->客户忠诚度管理
                            _htAllModules.Add("CRM.CTM", "CRM.CTM");//客户关系管理-->客户培训管理
                            _htAllModules.Add("CRM.MM", "CRM.MM");//客户关系管理-->会员管理
                            _htAllModules.Add("CRM.SSM", "CRM.SSM");//客户关系管理-->基础数据
                            _htAllModules.Add("CM.CMSA", "CM.CMSA");//合同管理-->销售合同
                            _htAllModules.Add("CM.CMPU", "CM.CMPU");//合同管理-->采购合同
                            _htAllModules.Add("CM.CMFI", "CM.CMFI");//合同管理-->财务合同
                            _htAllModules.Add("CM.CMCBD", "CM.CMCBD");//合同管理-->基础设置
                            _htAllModules.Add("SCM.PU", "SCM.PU");//供应链管理-->采购管理
                            _htAllModules.Add("SCM.GPU", "SCM.GPU");//供应链管理-->集团采购
                            _htAllModules.Add("SCM.IN", "SCM.IN");//供应链管理-->库存管理
                            _htAllModules.Add("SCM.SDA", "SCM.SDA");//供应链管理-->分销管理
                            _htAllModules.Add("SCM.SO", "SCM.SO");//供应链管理-->直销管理
                            _htAllModules.Add("SCM.POS", "SCM.POS");//供应链管理-->专卖零售协同管理
                            _htAllModules.Add("SCM.MQ", "SCM.MQ");//供应链管理-->经理查询
                            _htAllModules.Add("SCM.TD", "SCM.TD");//供应链管理-->外贸单证管理
                            _htAllModules.Add("SCM.SCMPSU", "SCM.SCMPSU");//供应链管理-->供应商门户(i6)
                            _htAllModules.Add("SCM.GDE", "SCM.GDE");//	供应链管理-->集团配送
                            _htAllModules.Add("SCM.SCMBD", "SCM.SCMBD");//供应链管理-->基础数据
                            _htAllModules.Add("EM.PDD", "EM.PDD");//技术管理-->产品设计数据
                            _htAllModules.Add("EM.PMR", "EM.PMR");//技术管理-->客户产品设计
                            _htAllModules.Add("EM.CPP", "EM.CPP");//技术管理-->客户产品定价
                            _htAllModules.Add("EM.CP", "EM.CP");//技术管理-->客户打样
                            _htAllModules.Add("EM.EDM", "EM.EDM");//技术管理-->基础数据
                            _htAllModules.Add("PM.MPS", "PM.MPS");//生产计划-->主生产计划
                            _htAllModules.Add("PM.MRP", "PM.MRP");//生产计划-->物料需求计划
                            _htAllModules.Add("PM.AOM", "PM.AOM");//生产计划-->敏捷询单管理
                            _htAllModules.Add("PM.PSM", "PM.PSM");//生产计划-->生产样品管理
                            _htAllModules.Add("PM.GMP", "PM.GMP");//生产计划-->药品质量管理
                            _htAllModules.Add("PM.MDM", "PM.MDM");//生产计划-->制造数据管理
                            _htAllModules.Add("PM.PPC", "PM.PPC");//生产计划-->参数化产品配置
                            _htAllModules.Add("MES.SFC", "MES.SFC");//制造执行-->车间作业控制
                            _htAllModules.Add("MES.CAQ", "MES.CAQ");//制造执行-->产品质量管理
                            _htAllModules.Add("MES.RMM", "MES.RMM");//制造执行-->重复式生产管理
                            _htAllModules.Add("MES.ENM", "MES.ENM");//制造执行-->能源管理
                            _htAllModules.Add("CO.CA", "CO.CA");//成本管理-->成本核算
                            _htAllModules.Add("CO.ICC", "CO.ICC");//成本管理-->灵动成本核算
                            _htAllModules.Add("CO.PCA", "CO.PCA");//成本管理-->标准制造成本
                            _htAllModules.Add("CO.DCM", "CO.DCM");//成本管理-->灵动制造成本

                            _htAllModules.Add("PMS.PI", "PMS.PI");//项目管理-->项目信息
                            _htAllModules.Add("PMS.PP", "MS.PP");//项目管理-->项目进度计划
                            _htAllModules.Add("PMS.PD", "PMS.PD");//项目管理-->项目文档
                            _htAllModules.Add("PMS.PMSBD", "PMS.PMSBD");//项目管理-->基础数据
                            _htAllModules.Add("PMC.PH", "PMC.PH");//项目管控中心-->项目人员
                            _htAllModules.Add("PMC.PS", "PMC.PS");//项目管控中心-->项目监理
                            #endregion
                        }

                        if (this.Product.ToUpper() == "I6P")
                        {
                            #region 其它i6P模块
                            _htAllModules.Add("PMS.JD", "PMS.JD");      //项目管理-->进度管理
                            _htAllModules.Add("PMS.CZ", "PMS.CZ");      //项目管理-->产值管理
                            _htAllModules.Add("PMS.ZY", "PMS.ZY");      //项目管理-->资源需求计划
                            _htAllModules.Add("PMS.ZL", "PMS.ZL");      //项目管理-->质量管理
                            _htAllModules.Add("PMS.AQ", "PMS.AQ");      //项目管理-->安全管理
                            _htAllModules.Add("PMS.FM", "PMS.FM");      //项目管理-->竣工管理
                            _htAllModules.Add("PMS.RM", "PMS.RM");      //项目管理-->风险管理
                            _htAllModules.Add("PMS.SW", "PMS.SW");      //项目管理-->项目事务
                            _htAllModules.Add("PMS.TK", "PMS.TK");      //项目管理-->投资控制
                            _htAllModules.Add("PCM.ZB", "PCM.ZB");      //合同管理-->招标管理
                            _htAllModules.Add("PCM.CBHT", "PCM.CBHT");  //合同管理-->承包合同
                            _htAllModules.Add("PCM.LWCB", "PCM.LWCB");  //合同管理-->劳务承包合同
                            _htAllModules.Add("PCM.QTSK", "PCM.QTSK");  //合同管理-->其他收入合同
                            _htAllModules.Add("PCM.FBHT", "PCM.FBHT");  //合同管理-->分包合同
                            _htAllModules.Add("PCM.LWFB", "PCM.LWFB");  //合同管理-->劳务分包合同
                            _htAllModules.Add("PCM.CGHT", "PCM.CGHT");  //合同管理-->采购合同
                            _htAllModules.Add("PCM.QTFK", "PCM.QTFK");  //合同管理-->其他支出合同
                            _htAllModules.Add("PCM.HTCX", "PCM.HTCX");  //合同管理-->合同查询
                            _htAllModules.Add("PCM.SRM", "PCM.SRM");    //合同管理-->供方评价
                            _htAllModules.Add("PMM.CG", "PMM.CG");      //物资管理-->采购管理
                            _htAllModules.Add("PMM.KC", "PMM.KC");      //物资管理-->库存管理
                            _htAllModules.Add("PMM.JGM", "PMM.JGM");    //物资管理-->甲供材料管理
                            _htAllModules.Add("PMM.CGA", "PMM.CGA");    //物资管理-->国际运输管理
                            _htAllModules.Add("PCO.ERM", "PCO.ERM");    //成本管理-->企业定额管理
                            _htAllModules.Add("PCO.AYS", "PCO.AYS");    //成本管理-->灵动成本
                            _htAllModules.Add("PCO.JXC", "PCO.JXC");    //成本管理-->精细成本

                            _htAllModules.Add("PMC.PC", "PMC.PC");      //项目管控中心-->项目管控

                            _htAllModules.Add("CRM.BOM", "CRM.BOM");    //客户关系管理-->商机管理
                            _htAllModules.Add("CRM.ZTB", "CRM.ZTB");    //客户关系管理-->投标管理
                            _htAllModules.Add("CRM.EBM", "CRM.EBM");    //客户关系管理-->电子标书
                            _htAllModules.Add("CRM.CIM", "CRM.CIM");    //客户关系管理-->客户信息管理 
                            _htAllModules.Add("CRM.SSM", "CRM.SSM");    //客户关系管理-->基础数据
                            #endregion
                        }
                    }

                }
                return _htAllModules;
            }
        }

        private Hashtable _htNoControlModules;
        public Hashtable HtNoControlModules
        {
            get
            {
                if (_htNoControlModules == null)
                {
                    _htNoControlModules = new Hashtable();
                    _htNoControlModules.Add("GFI.BD", "08");//集团财务-->基础数据
                    _htNoControlModules.Add("CRM.SSM", "151");//客户关系管理-->基础数据
                    _htNoControlModules.Add("CM.CMCBD", "6030");//合同管理-->基础设置[i6合同]
                    _htNoControlModules.Add("PA.PABD", "3230");//合同管理-->合同基础设置[A3、GE合同]
                    _htNoControlModules.Add("SCM.GDE", "118");//供应链管理-->集团配送
                    _htNoControlModules.Add("SCM.SCMBD", "101");//供应链管理-->基础数据
                    _htNoControlModules.Add("EM.EDM", "7240");//技术管理-->基础数据
                    _htNoControlModules.Add("PM.MDM", "22");//生产计划-->制造数据管理
                    _htNoControlModules.Add("WM.IWP", "181");//协同办公-->自助平台
                    _htNoControlModules.Add("HR.OGM", "211");//人力资源-->组织管理
                    _htNoControlModules.Add("PMS.PMSBD", "83");//项目管理-->基础数据
                    _htNoControlModules.Add("EPM.EPMDMC", "130");//企业管控中心-->基础数据设置
                    _htNoControlModules.Add("SUP.BM", "249");//柔性统一平台-->业务建模
                    _htNoControlModules.Add("SUP.BPM", "248");//柔性统一平台-->流程管理
                    _htNoControlModules.Add("SUP.ALM", "247");//柔性统一平台-->预警管理
                    _htNoControlModules.Add("SUP.UC", "252");//柔性统一平台-->统一通讯平台
                    _htNoControlModules.Add("SUP.UIP", "245");//柔性统一平台-->万向接口平台
                    _htNoControlModules.Add("I6BASE.DMC", "241");//基础数据-->企业级基础数据
                    _htNoControlModules.Add("I6BASE.DC", "11");//基础数据-->企业级权限中心
                }
                return _htNoControlModules;
            }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 
        /// </summary>
        public NGCOM()
        {
            //设置NSServer连接代理的url
            this.setNSServerURL(NSServerProxy);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NGNSServer"></param>
        /// <returns></returns>
        private void setNSServerURL(NGNSServerService NGNSServer)
        {
            string url;

            if (System.Web.HttpContext.Current.Request.Url.IsDefaultPort)
            {
                url = this.NSServerAddr + "/NSServer/default.aspx";
            }
            else
            {
                string strNSServerIP = this.NSServerAddr;
                if (strNSServerIP.IndexOf(":") > 0)
                {
                    //解决手工连接远程NSServer服务器
                    strNSServerIP = strNSServerIP.Substring(0, strNSServerIP.IndexOf(":"));
                }

                url = strNSServerIP + ":" + System.Web.HttpContext.Current.Request.Url.Port + "/NSServer/default.aspx";
            }
            url = @"http://" + url;

            NGNSServer.Url = url;
        }
        #endregion

        #region 属性定义
        /// <summary>
        /// NSServer地址
        /// </summary>
        private string NSServerAddr
        {
            get
            {
                if (string.IsNullOrWhiteSpace(m_nsserverAddr))
                {
                    m_nsserverAddr = System.Configuration.ConfigurationManager.AppSettings["NSServer"];
                }
                return m_nsserverAddr;
            }
        }
        /// <summary>
        /// 对应产品：i6|i6P|A3|GE
        /// </summary>
        public string Product
        {
            get
            {
                if (m_product == null)
                {
                    ProductInfo info = new ProductInfo();
                    m_product = info.ProductCode + info.Series.ToUpper();
                }
                return m_product;
            }
        }
        /// <summary>
        /// 对应的产品代码 .../i6 5/W3 6/....
        /// </summary>
        public string ProductCode
        {
            get
            {
                switch (this.Product.ToUpper())
                {
                    case "PSOFT":
                        m_productCode = "3";
                        break;
                    case "A3":
                        m_productCode = "4";
                        break;
                    case "W3":
                        m_productCode = "6";
                        break;
                    case "EPO":
                        m_productCode = "10";
                        break;
                    case "UIC":
                        m_productCode = "11";
                        break;
                    case "M3":
                        m_productCode = "16";
                        break;
                    case "I6P":
                        m_productCode = "20";
                        break;
                    case "GE":
                        m_productCode = "21";
                        break;
                    case "I6S":
                        m_productCode = "22";
                        break;
                    case "GEP":
                        m_productCode = "25";
                        break;
                    default:	//默认i6
                        m_productCode = "5";
                        break;
                }

                return m_productCode;
            }
        }
        /// <summary>
        /// 相关产品对应的机密文件
        /// </summary>
        public string ProductDatFile
        {
            get
            {
                switch (this.Product.ToUpper())
                {
                    case "PSOFT":
                        m_DatFile = "PSoft.Dat";
                        break;
                    case "A3":
                        m_DatFile = "A3.Dat";
                        break;
                    case "W3":
                        m_DatFile = "W3.Dat";
                        break;
                    case "EPO":
                        m_DatFile = "epo.Dat";
                        break;
                    case "UIC":
                        m_DatFile = "UIC.Dat";
                        break;
                    case "M3":
                        m_DatFile = "M3.Dat";
                        break;
                    case "I6P":
                        m_DatFile = "i6P.Dat";
                        break;
                    case "GE":
                        m_DatFile = "GE.Dat";
                        break;
                    case "I6S":
                        m_DatFile = "i6S.Dat";
                        break;
                    case "GEP":
                        m_DatFile = "GEP.Dat";
                        break;
                    default:	//默认i6
                        m_DatFile = "i6.Dat";
                        break;
                }
                return m_DatFile;
            }
        }
        /// <summary>
        /// //获得序列号
        /// </summary>
        public string SN
        {
            get
            {
                //使用缓存,提升速度
                string sn = string.Empty;

                IMemCachedClient client = MemCachedClientFactory.GetMemCachedClient();
                sn = client.GetSmallObject<string>(NG_LEGAL_SN);
                if (string.IsNullOrEmpty(sn))
                {
                    sn = this.GetSN();
                    client.SetSmallObject(NG_LEGAL_SN, sn);
                }

                return sn;
            }
        }
        /// <summary>
        /// 获得合法使用单位名
        /// </summary>
        public string UserName
        {
            get
            {
                //使用缓存,提升速度
                string legalusername = string.Empty;

                IMemCachedClient client = MemCachedClientFactory.GetMemCachedClient();
                legalusername = client.GetSmallObject<string>(NG_LEGAL_USERNAME);
                if (legalusername == null)
                {
                    legalusername = this.GetUserName();
                    client.SetSmallObject(NG_LEGAL_USERNAME, legalusername);
                }

                return legalusername;
            }
        }
        /// <summary>
        /// 得到人力资源用户数
        /// </summary>
        /// <returns></returns>
        public int HREmployees
        {
            get
            {
                int intHREmployees = 0;
                //System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                //stopWatch.Start();

                //Logger.Error(string.Format("Start HREmployees {0}",DateTime.Now));
                IMemCachedClient client = MemCachedClientFactory.GetMemCachedClient();
                var obj = client.GetSmallObject(NG_LEGAL_HREmployees);
                if (obj == null)
                {
                    //System.Diagnostics.Stopwatch stopWatch1 = new System.Diagnostics.Stopwatch();
                    //stopWatch1.Start();
                    //Logger.Error("Start HREmployees   MemCachedClient");
                    intHREmployees = this.GetHREmployees();
                    client.SetSmallObject(NG_LEGAL_HREmployees, intHREmployees, 6);

                    //stopWatch1.Stop();
                    //Logger.Error(string.Format("End HREmployees   MemCachedClient Time :{0}", stopWatch1.ElapsedMilliseconds));
                }
                else
                {
                    int.TryParse(obj.ToString(), out intHREmployees);
                }

                //stopWatch.Stop();
                //Logger.Error(string.Format("End HREmployees Time :{0}", stopWatch.ElapsedMilliseconds));
                return intHREmployees;
            }
        }
        /// <summary>
        /// 表单模板数
        /// </summary>
        public int WmForm
        {
            get
            {
                int iwmform = 0;

                IMemCachedClient client = MemCachedClientFactory.GetMemCachedClient();
                var obj = client.GetSmallObject(NG_LEGAL_WmForm);
                if (obj == null)
                {

                    iwmform = this.GetWmFormCount();
                    if (iwmform == 5)//默认表单数
                        client.SetSmallObject(NG_LEGAL_WmForm, iwmform, 6);
                    else
                        client.SetSmallObject(NG_LEGAL_WmForm, iwmform, 1440);
                }
                else
                {
                    int.TryParse(obj.ToString(), out iwmform);
                }

                return iwmform;
            }
        }
        /// <summary>
        /// 报表仓库报表记录数
        /// </summary>
        public int rw
        {
            get
            {
                int irw = 0;

                IMemCachedClient client = MemCachedClientFactory.GetMemCachedClient();
                var obj = client.GetSmallObject(NG_LEGAL_RW);
                if (obj == null)
                {

                    irw = this.GetRwCount();
                    if (irw == 5)//默认报表仓库报表记录数
                        client.SetSmallObject(NG_LEGAL_RW, irw, 6);
                    else
                        client.SetSmallObject(NG_LEGAL_RW, irw, 1440);
                }
                else
                {
                    int.TryParse(obj.ToString(), out irw);
                }

                return irw;
            }
        }
        /// <summary>
        /// 指标中心自定义指标记录数
        /// </summary>
        public int ic
        {
            get
            {
                {
                    int iic = 0;

                    IMemCachedClient client = MemCachedClientFactory.GetMemCachedClient();
                    var obj = client.GetSmallObject(NG_LEGAL_IC);
                    if (obj == null)
                    {

                        iic = this.GetICCount();
                        if (iic == 5)//默认指标中心自定义指标记录数
                            client.SetSmallObject(NG_LEGAL_IC, iic, 6);
                        else
                            client.SetSmallObject(NG_LEGAL_IC, iic, 1440);
                    }
                    else
                    {
                        int.TryParse(obj.ToString(), out iic);
                    }

                    return iic;
                }
            }
        }
        /// <summary>
        /// OA用户数
        /// </summary>
        public int OAUsers
        {
            get
            {
                int oaUsers = 0;
                //System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                //stopWatch.Start();

                //Logger.Error(string.Format("Start OAUsers {0}", DateTime.Now));
                IMemCachedClient client = MemCachedClientFactory.GetMemCachedClient();
                var obj = client.GetSmallObject(NG_LEGAL_OAUSERS);
                if (obj == null)
                {
                    //System.Diagnostics.Stopwatch stopWatch1 = new System.Diagnostics.Stopwatch();
                    //stopWatch1.Start();
                    //Logger.Error("Start OAUsers   MemCachedClient");
                    oaUsers = this.GetOAUsers();
                    if (oaUsers == defaultOAUsers)
                        client.SetSmallObject(NG_LEGAL_OAUSERS, oaUsers, 6);
                    else
                        client.SetSmallObject(NG_LEGAL_OAUSERS, oaUsers, 1440);

                    //stopWatch1.Stop();
                    //Logger.Error(string.Format("End OAUsers   MemCachedClient Time :{0}", stopWatch1.ElapsedMilliseconds));
                }
                else
                {
                    int.TryParse(obj.ToString(), out oaUsers);
                }

                //stopWatch.Stop();
                //Logger.Error(string.Format("End OAUsers Time :{0}", stopWatch.ElapsedMilliseconds));
                return oaUsers;
            }
        }
        /// <summary>
        /// 全模块用户数
        /// </summary>
        public int AllUsers
        {
            get
            {
                int allUsers = 0;
                //System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                //stopWatch.Start();

                //Logger.Error(string.Format("Start AllUsers {0}", DateTime.Now));
                IMemCachedClient client = MemCachedClientFactory.GetMemCachedClient();
                var obj = client.GetSmallObject(NG_LEGAL_ALLUSERS);
                if (obj == null)
                {
                    //System.Diagnostics.Stopwatch stopWatch1 = new System.Diagnostics.Stopwatch();
                    //stopWatch1.Start();
                    //Logger.Error("Start AllUsers   MemCachedClient");
                    allUsers = this.GetAllUsers();
                    if (allUsers == defaultAllUsers)
                        client.SetSmallObject(NG_LEGAL_ALLUSERS, allUsers, 6);
                    else
                        client.SetSmallObject(NG_LEGAL_ALLUSERS, allUsers, 1440);

                    //stopWatch1.Stop();
                    //Logger.Error(string.Format("End AllUsers   MemCachedClient Time :{0}", stopWatch1.ElapsedMilliseconds));
                }
                else
                {
                    int.TryParse(obj.ToString(), out allUsers);
                }

                //stopWatch.Stop();
                //Logger.Error(string.Format("End AllUsers Time :{0}", stopWatch.ElapsedMilliseconds));
                return allUsers;
            }
        }
        /// <summary>
        /// 移动应用用户数（并发）
        /// </summary>
        public int MUsers
        {
            get
            {
                int musers = 0;
                IMemCachedClient client = MemCachedClientFactory.GetMemCachedClient();
                var obj = client.GetSmallObject(NG_LEGAL_MUSERS);
                if (obj == null)
                {
                    musers = this.GetMUsers();
                    if (musers == defaultMUsers)
                        client.SetSmallObject(NG_LEGAL_MUSERS, musers, 10);
                    else
                        client.SetSmallObject(NG_LEGAL_MUSERS, musers, 1440);
                }
                else
                {
                    int.TryParse(obj.ToString(), out musers);
                }
                return musers;
            }
        }
        #endregion

        #region 获取正版属性方法
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetSN()
        {
            string sRetu = "";

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    sRetu = NSServerProxy.License_GetSN(0, true, int.Parse(this.ProductCode));
                    break;
                }
                catch (Exception ex)
                {
                    //Logger.Error(ex.Message);
                    sRetu = "";
                }
                Thread.Sleep(200);
            }

            // 执行数据解密
            sRetu = lzwZipService.Level2DecodeFromBase64(sRetu);
            if (sRetu == null) sRetu = "";
            try
            {
                int.Parse(sRetu);
            }
            catch
            {
                sRetu = "";
            }
            return sRetu;
        }
        /// <summary>
        /// 获取正版属性方法
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string GetProperty(string item)
        {
            string sRetu = "";
            string sNSServer = "";

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    sNSServer = NSServerProxy.License_GetProperty(0, true, int.Parse(this.ProductCode), item);

                    if (!string.IsNullOrEmpty(sNSServer))
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    //Logger.Error(item + ",sNSServer:" + ex.Message);
                    sNSServer = "";
                }
                Thread.Sleep(200);
            }

            if (sNSServer == null)
            {
                //Logger.Error(item + ",执行数据解密前为NULL");
            }
            else if (sNSServer == string.Empty)
            {
                //Logger.Error(item + ",执行数据解密前为空");
            }
            // 执行数据解密
            sRetu = lzwZipService.Level2DecodeFromBase64(sNSServer);
            if (sRetu == null)
            {
                //Logger.Error(item + ",执行数据解密后为NUlL");
                sRetu = "";
            }
            else if (sRetu == string.Empty)
            {
                //Logger.Error(item + ",执行数据解密后为空");
            }

            string stmp = sRetu;
            try
            {
                int.Parse(sRetu);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("***********************************");
                System.Diagnostics.Debug.WriteLine(stmp);
                System.Diagnostics.Debug.WriteLine(item);
                System.Diagnostics.Debug.WriteLine(sRetu);
                System.Diagnostics.Debug.WriteLine("***********************************");
                //sRetu="32768";
            }

            return sRetu;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            return this.GetProperty("Username");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetHREmployees()
        {
            string sRetu;
            sRetu = this.GetProperty("HREMP");
            if (sRetu == "") sRetu = "100"; 
            return int.Parse(sRetu);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int GetWmFormCount()
        {
            string sRetu;
            sRetu = this.GetProperty("WmForm");
            if (string.IsNullOrEmpty(sRetu)) sRetu = "2";
            return int.Parse(sRetu);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int GetRwCount()
        {
            string sRetu;
            sRetu = this.GetProperty("rw");
            if (string.IsNullOrEmpty(sRetu)) sRetu = "5";
            return int.Parse(sRetu);
        }
        /// <summary>
        /// 自定义指标数
        /// </summary>
        /// <returns></returns>
        private int GetICCount()
        {
            string sRetu;
            sRetu = this.GetProperty("ic");
            if (string.IsNullOrEmpty(sRetu)) sRetu = "1";
            return int.Parse(sRetu);
        }
        /// <summary>
        /// OA用户数-2012-7-19
        /// </summary>
        /// <returns></returns>
        private int GetOAUsers()
        {
            string sRetu;
            sRetu = this.GetProperty("OAUsers");
            if (string.IsNullOrEmpty(sRetu)) sRetu = "3";
            return int.Parse(sRetu);
        }
        /// <summary>
        /// 全模块用户数-2012-7-19
        /// </summary>
        /// <returns></returns>
        private int GetAllUsers()
        {
            string sRetu;
            sRetu = this.GetProperty("AllUsers");
            if (string.IsNullOrEmpty(sRetu)) sRetu = "2";
            return int.Parse(sRetu);
        }
        /// <summary>
        /// 移动应用用户数（并发）
        /// </summary>
        /// <returns></returns>
        private int GetMUsers()
        {
            string sRetu;
            sRetu = this.GetProperty("musers");
            if (string.IsNullOrEmpty(sRetu)) sRetu = "2";
            return int.Parse(sRetu);
        }
        #endregion

        #region 正版演示版Hashtable处理
        /// <summary>
        /// 判断是否购买,此模块是否为演示版，还需要组织模块授权判断
        /// </summary>
        public Hashtable ModuleBuy
        {
            get
            {
                Hashtable ht = null;

                IMemCachedClient client = MemCachedClientFactory.GetMemCachedClient();
                ht = client.Get<Hashtable>(NG_LEGAL_MODULEBUYLIST);
                if (ht == null)
                {
                    lock (moduleBuyLock)
                    {
                        ht = client.Get<Hashtable>(NG_LEGAL_MODULEBUYLIST);
                        if (ht != null)
                        {
                            return ht;
                        }
                        ht = this.GetModuleBuyList();

                        client.Set(NG_LEGAL_MODULEBUYLIST, ht, 720);

                    }
                }

                return ht;
            }
        }
        private Hashtable GetModuleBuyList()
        {
            Hashtable ht = new Hashtable();

            string sModule = "";
            foreach (DictionaryEntry de in this.HtAllModules)
            {
                if (string.IsNullOrEmpty(sModule))
                {
                    sModule += de.Key;
                }
                else
                {
                    sModule += "\t" + de.Key;
                }
            }

            string sLicenseIsModulesDemo = this.GetAllModulesDemo(sModule);

            string[] arrModule = sModule.Split('\t');
            string[] arrLicenseValue = sLicenseIsModulesDemo.Split('\t');

            for (int i = 0; i < arrModule.Length; i++)
            {
                bool bDemo = arrLicenseValue[i] == "0" ? true : false;//0表示已购买，1表示未购买
                if (this.HtNoControlModules.ContainsKey(arrModule[i].ToString()))
                {
                    bDemo = false;
                }
                ht.Add(arrModule[i], bDemo);
            }

            return ht;
        }
        /// <summary>
        /// 判断某模块是否安装
        /// </summary>
        public Hashtable ModuleInstalled
        {
            get
            {
                Hashtable ht = new Hashtable();

                IMemCachedClient client = MemCachedClientFactory.GetMemCachedClient();
                ht = client.Get<Hashtable>(NG_LEGAL_MODULEINSTALLLIST);
                if (ht == null)
                {
                    ht = this.GetModuleInstall();
                    client.Set(NG_LEGAL_MODULEINSTALLLIST, ht);
                }

                return ht;
            }
        }
        private Hashtable GetModuleInstall()
        {
            bool bInstalled = true;
            Hashtable ht = new Hashtable();

            #region ENV=ENV.PBDK,ENV.ODBC,ENV.AnyWhere,ENV.NGReport,ENV.Crystal,ENV.ODBC.NET,ENV.OTHER

            //ENV.PBDK
            bInstalled = this.IsModuleInstalled("ENV.PBDK");
            ht.Add("ENV.PBDK", bInstalled);
            //ENV.ODBC
            bInstalled = this.IsModuleInstalled("ENV.ODBC");
            ht.Add("ENV.ODBC", bInstalled);
            //ENV.AnyWhere
            bInstalled = this.IsModuleInstalled("ENV.AnyWhere");
            ht.Add("ENV.AnyWhere", bInstalled);
            //ENV.NGReport
            bInstalled = this.IsModuleInstalled("ENV.NGReport");
            ht.Add("ENV.NGReport", bInstalled);
            //ENV.Crystal
            bInstalled = this.IsModuleInstalled("ENV.Crystal");
            ht.Add("ENV.Crystal", bInstalled);
            //ENV.ODBC.NET
            bInstalled = this.IsModuleInstalled("ENV.ODBC.NET");
            ht.Add("ENV.ODBC.NET", bInstalled);
            //ENV.OTHER
            bInstalled = this.IsModuleInstalled("ENV.OTHER");
            ht.Add("ENV.OTHER", bInstalled);

            #endregion

            #region i6DMC=i6DMC
            bInstalled = this.IsModuleInstalled("i6DMC");
            ht.Add("i6DMC", bInstalled);
            bInstalled = this.IsModuleInstalled("i6.UIP");
            ht.Add("i6.UIP", bInstalled);
            #endregion

            #region UIC=UIC.DMC,UIC.PSD,UIC.PCU,UIC.PSU,UIC.PEM,UIC.PEM,UIC.PFI,UIC.PPT,UIC.RMC,UIC.IWP,UIC.PPSU,UIC.PCO,UIC.WSM
            bInstalled = this.IsModuleInstalled("UIC.DMC");
            ht.Add("UIC.DMC", bInstalled);
            bInstalled = this.IsModuleInstalled("UIC.UIP");
            ht.Add("UIC.UIP", bInstalled);
            bInstalled = this.IsModuleInstalled("UIC.PSD");
            ht.Add("UIC.PSD", bInstalled);
            bInstalled = this.IsModuleInstalled("UIC.PCU");
            ht.Add("UIC.PCU", bInstalled);
            bInstalled = this.IsModuleInstalled("UIC.PSU");
            ht.Add("UIC.PSU", bInstalled);
            bInstalled = this.IsModuleInstalled("UIC.PEM");
            ht.Add("UIC.PEM", bInstalled);
            bInstalled = this.IsModuleInstalled("UIC.PFI");
            ht.Add("UIC.PFI", bInstalled);
            bInstalled = this.IsModuleInstalled("UIC.PPT");
            ht.Add("UIC.PPT", bInstalled);
            bInstalled = this.IsModuleInstalled("UIC.RMC");
            ht.Add("UIC.RMC", bInstalled);
            bInstalled = this.IsModuleInstalled("UIC.IWP");
            ht.Add("UIC.IWP", bInstalled);
            bInstalled = this.IsModuleInstalled("UIC.PPSU");
            ht.Add("UIC.PPSU", bInstalled);
            bInstalled = this.IsModuleInstalled("UIC.PCO");
            ht.Add("UIC.PCO", bInstalled);
            bInstalled = this.IsModuleInstalled("UIC.PSS");
            ht.Add("UIC.PSS", bInstalled);
            bInstalled = this.IsModuleInstalled("UIC.WSM");
            ht.Add("UIC.WSM", bInstalled);
            #endregion

            #region i6CRM=CRM.MPM,CRM.DSP,CRM.DPM,CRM.TSM,CRM.CSM,CRM.CLM,CRM.CPI,CRM.SIM,CRM.CTM,CRM.MQS,CRM.SSM,CRM.QQM,CRM.BOM
            //CRM.CIM
            bInstalled = this.IsModuleInstalled("CRM.CIM");
            ht.Add("CRM.CIM", bInstalled);
            //CRM.MPM
            bInstalled = this.IsModuleInstalled("CRM.MPM");
            ht.Add("CRM.MPM", bInstalled);
            //CRM.DSP
            bInstalled = this.IsModuleInstalled("CRM.DSP");
            ht.Add("CRM.DSP", bInstalled);
            //CRM.DPM
            bInstalled = this.IsModuleInstalled("CRM.DPM");
            ht.Add("CRM.DPM", bInstalled);
            //CRM.TSM
            bInstalled = this.IsModuleInstalled("CRM.TSM");
            ht.Add("CRM.TSM", bInstalled);
            //CRM.CSM
            bInstalled = this.IsModuleInstalled("CRM.CSM");
            ht.Add("CRM.CSM", bInstalled);
            //CRM.CLM
            bInstalled = this.IsModuleInstalled("CRM.CLM");
            ht.Add("CRM.CLM", bInstalled);
            //CRM.CPI
            bInstalled = this.IsModuleInstalled("CRM.CPI");
            ht.Add("CRM.CPI", bInstalled);
            //CRM.SIM
            bInstalled = this.IsModuleInstalled("CRM.SIM");
            ht.Add("CRM.SIM", bInstalled);
            //CRM.CTM
            bInstalled = this.IsModuleInstalled("CRM.CTM");
            ht.Add("CRM.CTM", bInstalled);
            //CRM.MQS
            bInstalled = this.IsModuleInstalled("CRM.MQS");
            ht.Add("CRM.MQS", bInstalled);
            //CRM.SSM
            bInstalled = this.IsModuleInstalled("CRM.SSM");
            ht.Add("CRM.SSM", bInstalled);
            //i6.CRM.CMB
            bInstalled = this.IsModuleInstalled("i6.CRM.CMB"); ;
            ht.Add("i6.CRM.CMB", bInstalled);
            //CRM.QQM
            bInstalled = this.IsModuleInstalled("CRM.QQM");
            ht.Add("CRM.QQM", bInstalled);
            //CRM.BOM
            bInstalled = this.IsModuleInstalled("CRM.BOM");
            ht.Add("CRM.BOM", bInstalled);
            //CRM.MM
            bInstalled = this.IsModuleInstalled("CRM.MM");
            ht.Add("CRM.MM", bInstalled);
            #endregion

            #region i6EC=i6.EC.BD,i6.EC.PU,i6.EC.IN,i6.EC.SDA,i6.EC.SDB,i6.EC.SO,i6.EC.POS,i6.EC.MQ,i6.EC.TD,i6.EC.EMB,i6.EC.GPU,i6.EC.GDE,i6.EC.SAM
            //i6.EC.BD
            bInstalled = this.IsModuleInstalled("i6.EC.BD");
            ht.Add("i6.EC.BD", bInstalled);
            //i6.EC.PU
            bInstalled = this.IsModuleInstalled("i6.EC.PU");
            ht.Add("i6.EC.PU", bInstalled);
            //i6.EC.IN
            bInstalled = this.IsModuleInstalled("i6.EC.IN");
            ht.Add("i6.EC.IN", bInstalled);
            //i6.EC.SDA
            bInstalled = this.IsModuleInstalled("i6.EC.SDA");
            ht.Add("i6.EC.SDA", bInstalled);
            //i6.EC.SDB
            bInstalled = this.IsModuleInstalled("i6.EC.SDB");
            ht.Add("i6.EC.SDB", bInstalled);
            //i6.EC.SO
            bInstalled = this.IsModuleInstalled("i6.EC.SO");
            ht.Add("i6.EC.SO", bInstalled);
            //i6.EC.POS
            bInstalled = this.IsModuleInstalled("i6.EC.POS");
            ht.Add("i6.EC.POS", bInstalled);
            //i6.EC.MQ
            bInstalled = this.IsModuleInstalled("i6.EC.MQ");
            ht.Add("i6.EC.MQ", bInstalled);
            //i6.EC.TD
            bInstalled = this.IsModuleInstalled("i6.EC.TD");
            ht.Add("i6.EC.TD", bInstalled);
            //i6.EC.EMB
            bInstalled = this.IsModuleInstalled("i6.EC.EMB");
            ht.Add("i6.EC.EMB", bInstalled);
            //i6.EC.GPU
            bInstalled = this.IsModuleInstalled("i6.EC.GPU");
            ht.Add("i6.EC.GPU", bInstalled);
            //i6.EC.GDE
            bInstalled = this.IsModuleInstalled("i6.EC.GDE");
            ht.Add("i6.EC.GDE", bInstalled);
            //i6.EC.SAM
            bInstalled = this.IsModuleInstalled("i6.EC.SAM");
            ht.Add("i6.EC.SAM", bInstalled);
            #endregion

            #region i6Server=i6.Server.AClt,i6.Server.BSrv,i6.Server.CSrv
            //i6.Server.AClt
            bInstalled = this.IsModuleInstalled("i6.Server.AClt");
            ht.Add("i6.Server.AClt", bInstalled);
            //i6.Server.BSrv
            bInstalled = this.IsModuleInstalled("i6.Server.BSrv");
            ht.Add("i6.Server.BSrv", bInstalled);
            //i6.Server.CSrv
            bInstalled = this.IsModuleInstalled("i6.Server.CSrv");
            ht.Add("i6.Server.CSrv", bInstalled);
            #endregion

            #region i6HR=HR.OGM,HR.EGM,HR.ECM,HR.RIM,HR.TDM,HR.PAM,HR.EMI,HR.PBM,HR.CMM,HR.MCA,RG
            //HR.OGM
            bInstalled = this.IsModuleInstalled("HR.OGM");
            ht.Add("HR.OGM", bInstalled);
            //HR.EMI
            bInstalled = this.IsModuleInstalled("HR.EMI");
            ht.Add("HR.EMI", bInstalled);
            //HR.EGM
            bInstalled = this.IsModuleInstalled("HR.EGM");
            ht.Add("HR.EGM", bInstalled);
            //HR.ECM
            bInstalled = this.IsModuleInstalled("HR.ECM");
            ht.Add("HR.ECM", bInstalled);
            //HR.RIM
            bInstalled = this.IsModuleInstalled("HR.RIM");
            ht.Add("HR.RIM", bInstalled);
            //HR.TDM
            bInstalled = this.IsModuleInstalled("HR.TDM");
            ht.Add("HR.TDM", bInstalled);
            //HR.PAM
            bInstalled = this.IsModuleInstalled("HR.PAM");
            ht.Add("HR.PAM", bInstalled);

            //HR.TIM
            bInstalled = this.IsModuleInstalled("HR.TIM");
            ht.Add("HR.TIM", bInstalled);

            //HR.PBM
            bInstalled = this.IsModuleInstalled("HR.PBM");
            ht.Add("HR.PBM", bInstalled);
            //RG
            if (Product.ToUpper() == "I6P")
            {
                bInstalled = this.IsModuleInstalled("HR.RG");
                ht.Add("HR.RG", bInstalled);
            }
            //HR.CMM
            bInstalled = this.IsModuleInstalled("HR.CMM");
            ht.Add("HR.CMM", bInstalled);
            //i6.HR.HMB
            bInstalled = this.IsModuleInstalled("i6.HR.HMB");
            ht.Add("i6.HR.HMB", bInstalled);
            //HR.HRP
            bInstalled = this.IsModuleInstalled("HR.HRP");
            ht.Add("HR.HRP", bInstalled);
            //HR.CDM
            bInstalled = this.IsModuleInstalled("HR.CDM");
            ht.Add("HR.CDM", bInstalled);
            //HR.MCA
            bInstalled = this.IsModuleInstalled("HR.MCA");
            ht.Add("HR.MCA", bInstalled);

            #endregion

            #region WM=WM.IWP,WM.OM,WM.KM,WM.PJM,WM.RM,i6.WM.WMB,WM.WCM,WM.DOS,WM.WFM,WM.FC
            //WM.IWP
            bInstalled = this.IsModuleInstalled("WM.IWP");
            ht.Add("WM.IWP", bInstalled);
            //WM.OM
            bInstalled = this.IsModuleInstalled("WM.OM");
            ht.Add("WM.OM", bInstalled);
            //WM.KM
            bInstalled = this.IsModuleInstalled("WM.KM");
            ht.Add("WM.KM", bInstalled);
            //WM.PJM
            bInstalled = this.IsModuleInstalled("WM.PJM");
            ht.Add("WM.PJM", bInstalled);
            //WM.RM
            bInstalled = this.IsModuleInstalled("WM.RM");
            ht.Add("WM.RM", bInstalled);
            //i6.WM.WMB
            bInstalled = this.IsModuleInstalled("i6.WM.WMB");
            ht.Add("i6.WM.WMB", bInstalled);
            //WM.WCM
            bInstalled = this.IsModuleInstalled("WM.WCM");
            ht.Add("WM.WCM", bInstalled);
            //WM.DOS
            bInstalled = this.IsModuleInstalled("WM.DOS");
            ht.Add("WM.DOS", bInstalled);
            //WM.WFM
            bInstalled = this.IsModuleInstalled("WM.WFM");
            ht.Add("WM.WFM", bInstalled);
            //WM.FC
            bInstalled = this.IsModuleInstalled("WM.FC");
            ht.Add("WM.FC", bInstalled);
            #endregion

            #region INTFI=Intfi.BD,Intfi.GC,Intfi.QT,Intfi.FR,Intfi.BU,Intfi.FA,Intfi.LC,Intfi.DA,Intfi.GM,Intfi.CA,Intfi.EM,Intfi.WEB,Intfi.CM
            //INTFI.BD
            bInstalled = this.IsModuleInstalled("INTFI.BD");
            ht.Add("INTFI.BD", bInstalled);
            //INTFI.GC
            bInstalled = this.IsModuleInstalled("INTFI.GC");
            ht.Add("INTFI.GC", bInstalled);
            //INTFI.QT
            bInstalled = this.IsModuleInstalled("INTFI.QT");
            ht.Add("INTFI.QT", bInstalled);
            //INTFI.FR
            bInstalled = this.IsModuleInstalled("INTFI.FR");
            ht.Add("INTFI.FR", bInstalled);
            //INTFI.BU
            bInstalled = this.IsModuleInstalled("INTFI.BU");
            ht.Add("INTFI.BU", bInstalled);
            //INTFI.FA
            bInstalled = this.IsModuleInstalled("INTFI.FA");
            ht.Add("INTFI.FA", bInstalled);
            //INTFI.LC
            bInstalled = this.IsModuleInstalled("INTFI.LC");
            ht.Add("INTFI.LC", bInstalled);
            //INTFI.DA
            bInstalled = this.IsModuleInstalled("INTFI.DA");
            ht.Add("INTFI.DA", bInstalled);
            //INTFI.GM
            bInstalled = this.IsModuleInstalled("INTFI.GM");
            ht.Add("INTFI.GM", bInstalled);
            //INTFI.CA
            bInstalled = this.IsModuleInstalled("INTFI.CA");
            ht.Add("INTFI.CA", bInstalled);
            //INTFI.EM
            bInstalled = this.IsModuleInstalled("INTFI.EM");
            ht.Add("INTFI.EM", bInstalled);
            //INTFI.CM
            bInstalled = this.IsModuleInstalled("INTFI.CM");
            ht.Add("INTFI.CM", bInstalled);
            //i6.INTFI.IMB
            bInstalled = this.IsModuleInstalled("i6.INTFI.IMB");
            ht.Add("i6.INTFI.IMB", bInstalled);
            #endregion

            #region PM=PM.MDM,PM.MPS,PM.MRP,PM.SFC,PM.PCA,PM.CAQ,PM.RMM,PM.PMQ,PM.EMM,PM.CPM,PM.GMP
            //PM.MDM
            bInstalled = this.IsModuleInstalled("PM.MDM");
            ht.Add("PM.MDM", bInstalled);
            //PM.MPS
            bInstalled = this.IsModuleInstalled("PM.MPS");
            ht.Add("PM.MPS", bInstalled);
            //PM.MRP
            bInstalled = this.IsModuleInstalled("PM.MRP");
            ht.Add("PM.MRP", bInstalled);
            //PM.SFC
            bInstalled = this.IsModuleInstalled("PM.SFC");
            ht.Add("PM.SFC", bInstalled);
            //PM.PCA
            bInstalled = this.IsModuleInstalled("PM.PCA");
            ht.Add("PM.PCA", bInstalled);
            //PM.CAQ
            bInstalled = this.IsModuleInstalled("PM.CAQ");
            ht.Add("PM.CAQ", bInstalled);
            //PM.RMM
            bInstalled = this.IsModuleInstalled("PM.RMM");
            ht.Add("PM.RMM", bInstalled);
            //PM.PMQ
            bInstalled = this.IsModuleInstalled("PM.PMQ");
            ht.Add("PM.PMQ", bInstalled);

            //PM.EMM
            bInstalled = this.IsModuleInstalled("PM.EMM");
            ht.Add("PM.EMM", bInstalled);
            //PM.CPM
            bInstalled = this.IsModuleInstalled("PM.CPM");
            ht.Add("PM.CPM", bInstalled);
            //PM.GMP
            bInstalled = this.IsModuleInstalled("PM.GMP");
            ht.Add("PM.GMP", bInstalled);
            //i6.PM.PMB
            bInstalled = this.IsModuleInstalled("i6.PM.PMB");
            ht.Add("i6.PM.PMB", bInstalled);

            bInstalled = this.IsModuleInstalled("PM.ENM");
            ht.Add("PM.ENM", bInstalled);

            bInstalled = this.IsModuleInstalled("PM.AOM");
            ht.Add("PM.AOM", bInstalled);
            bInstalled = this.IsModuleInstalled("PM.PSM");
            ht.Add("PM.PSM", bInstalled);
            #endregion

            #region EPM = EPM.DMC,EPM.IC,EPM.MC,EPM.MA,EPM.MQ,EPM.RMC,EPM.BU
            bInstalled = this.IsModuleInstalled("EPM.DMC");
            ht.Add("EPM.DMC", bInstalled);
            bInstalled = this.IsModuleInstalled("EPM.IC");
            ht.Add("EPM.IC", bInstalled);
            bInstalled = this.IsModuleInstalled("EPM.MC");
            ht.Add("EPM.MC", bInstalled);
            bInstalled = this.IsModuleInstalled("EPM.MA");
            ht.Add("EPM.MA", bInstalled);
            bInstalled = this.IsModuleInstalled("EPM.MQ");
            ht.Add("EPM.MQ", bInstalled);
            bInstalled = this.IsModuleInstalled("EPM.RMC");
            ht.Add("EPM.RMC", bInstalled);
            bInstalled = this.IsModuleInstalled("EPM.BU");
            ht.Add("EPM.BU", bInstalled);
            if (Product == "I6" || Product == "I6P")
            {
                bInstalled = this.IsModuleInstalled("EPM.RW");
                ht.Add("EPM.RW", bInstalled);
            }
            #endregion

            #region M3=M3.EMB,M3.LMB,M3.CMB,M3.HMB,M3.WMB,M3.IMB,M3.PMB，M3.DCB
            bInstalled = this.IsModuleInstalled("M3.EMB");
            ht.Add("M3.EMB", bInstalled);
            bInstalled = this.IsModuleInstalled("M3.LMB");
            ht.Add("M3.LMB", bInstalled);
            bInstalled = this.IsModuleInstalled("M3.CMB");
            ht.Add("M3.CMB", bInstalled);
            bInstalled = this.IsModuleInstalled("M3.HMB");
            ht.Add("M3.HMB", bInstalled);
            bInstalled = this.IsModuleInstalled("M3.WMB");
            ht.Add("M3.WMB", bInstalled);
            bInstalled = this.IsModuleInstalled("M3.IMB");
            ht.Add("M3.IMB", bInstalled);
            bInstalled = this.IsModuleInstalled("M3.PMB");
            ht.Add("M3.PMB", bInstalled);
            bInstalled = this.IsModuleInstalled("M3.DCB");
            ht.Add("M3.DCB", bInstalled);
            bInstalled = this.IsModuleInstalled("M3.PSMB");
            ht.Add("M3.PSMB", bInstalled);
            #endregion

            #region CO=CO.CA,CO.PCA,CO.DCM,CO.INA
            bInstalled = this.IsModuleInstalled("CO.CA");
            ht.Add("CO.CA", bInstalled);
            bInstalled = this.IsModuleInstalled("CO.PCA");
            ht.Add("CO.PCA", bInstalled);
            bInstalled = this.IsModuleInstalled("CO.DCM");
            ht.Add("CO.DCM", bInstalled);
            bInstalled = this.IsModuleInstalled("CO.INA");
            ht.Add("CO.INA", bInstalled);
            #endregion

            #region EAM = "EAM.EAM", "EAM.FA", "EAM.LC", "EAM.AC", "EAM.EMM"
            bInstalled = this.IsModuleInstalled("EAM.EAM");
            ht.Add("EAM.EAM", bInstalled);
            bInstalled = this.IsModuleInstalled("EAM.FA");
            ht.Add("EAM.FA", bInstalled);
            bInstalled = this.IsModuleInstalled("EAM.LC");
            ht.Add("EAM.LC", bInstalled);
            bInstalled = this.IsModuleInstalled("EAM.AC");
            ht.Add("EAM.AC", bInstalled);
            bInstalled = this.IsModuleInstalled("EAM.EMM");
            ht.Add("EAM.EMM", bInstalled);
            bInstalled = this.IsModuleInstalled("EAM.AM");
            ht.Add("EAM.AM", bInstalled);
            #endregion

            #region W3=W3.DMC,CRM.SPM,CRM.CSC,其它的和CRM,HR,WM重复
            //W3.DMC
            bInstalled = this.IsMainModuleInstalled("W3.DMC");
            ht.Add("W3.DMC", bInstalled);

            bInstalled = this.IsModuleInstalled("W3.UIP");
            ht.Add("W3.UIP", bInstalled);

            //CRM.SPM
            bInstalled = this.IsModuleInstalled("CRM.SPM");
            ht.Add("CRM.SPM", bInstalled);
            //CRM.CSC
            bInstalled = this.IsModuleInstalled("CRM.CSC");
            ht.Add("CRM.CSC", bInstalled);
            #endregion

            #region CM=CM.CBD
            //CM.CBD
            bInstalled = this.IsModuleInstalled("CM.CBD");
            ht.Add("CM.CBD", bInstalled);
            #endregion

            #region EM=EM.RDM,EM.ACDM,EM.DCDM
            //EM.RDM
            bInstalled = this.IsModuleInstalled("i6.EM.RDM");
            ht.Add("i6.EM.RDM", bInstalled);
            //EM.ACDM
            bInstalled = this.IsModuleInstalled("i6.EM.ACDM");
            ht.Add("i6.EM.ACDM", bInstalled);
            //EM.DCDM
            bInstalled = this.IsModuleInstalled("i6.EM.DCDM");
            ht.Add("i6.EM.DCDM", bInstalled);
            #endregion

            #region
            bInstalled = this.IsModuleInstalled("PP");
            ht.Add("PP", bInstalled);
            bInstalled = this.IsModuleInstalled("PMS");
            ht.Add("PMS", bInstalled);
            #endregion

            #region
            bInstalled = this.IsModuleInstalled("PS.ESD");
            ht.Add("PS.ESD", bInstalled);
            bInstalled = this.IsModuleInstalled("PS.ESV");
            ht.Add("PS.ESV", bInstalled);
            bInstalled = this.IsModuleInstalled("PS.EPS");
            ht.Add("PS.EPS", bInstalled);
            #endregion

            #region
            bInstalled = this.IsModuleInstalled("SUP.BM");
            ht.Add("SUP.BM", bInstalled);
            bInstalled = this.IsModuleInstalled("SUP.BPM");
            ht.Add("SUP.BPM", bInstalled);
            bInstalled = this.IsModuleInstalled("SUP.UIP");
            ht.Add("SUP.UIP", bInstalled);
            bInstalled = this.IsModuleInstalled("SUP.ISERVER");
            ht.Add("SUP.ISERVER", bInstalled);
            if (Product != "I6" && Product != "I6P")
            {
                bInstalled = this.IsModuleInstalled("SUP.RW");
                ht.Add("SUP.RW", bInstalled);

                bInstalled = this.IsModuleInstalled("SUP.IC");
                ht.Add("SUP.IC", bInstalled);
            }
            #endregion

            return ht;
        }
        /// <summary>
        /// 判断某主模块是否安装
        /// </summary>
        public Hashtable MainModuleInstalled
        {
            get
            {
                Hashtable ht = new Hashtable();

                IMemCachedClient client = MemCachedClientFactory.GetMemCachedClient();
                ht = client.Get<Hashtable>(NG_LEGAL_MAINMODULEINSTALLEDLIST);
                if (ht == null)
                {
                    ht = this.GetMainModuleInstall();
                    client.Set(NG_LEGAL_MAINMODULEINSTALLEDLIST, ht);
                }

                return ht;
            }
        }
        private Hashtable GetMainModuleInstall()
        {
            bool bInstalled = true;
            Hashtable ht = new Hashtable();
            //COMServer
            bInstalled = this.IsMainModuleInstalled("COMServer");
            ht.Add("COMServer", bInstalled);

            //ENV
            bInstalled = this.IsMainModuleInstalled("ENV");
            ht.Add("ENV", bInstalled);

            //NGServer
            bInstalled = this.IsMainModuleInstalled("NGServer");
            if (!bInstalled)
            {
                bInstalled = this.IsModuleInstalled("NGServer");
            }
            ht.Add("NGServer", bInstalled);

            //DBCNT
            bInstalled = this.IsMainModuleInstalled("DBCNT");
            if (!bInstalled)
            {
                bInstalled = this.IsModuleInstalled("DBCNT");
            }
            ht.Add("DBCNT", bInstalled);

            //DRC
            bInstalled = this.IsMainModuleInstalled("DRC");
            if (!bInstalled)
            {
                bInstalled = this.IsModuleInstalled("DRC");
            }
            ht.Add("DRC", bInstalled);

            //INTFI
            bInstalled = this.IsMainModuleInstalled("INTFI");
            ht.Add("INTFI", bInstalled);

            //PM
            bInstalled = this.IsMainModuleInstalled("PM");
            ht.Add("PM", bInstalled);

            //i6DMC
            bInstalled = this.IsMainModuleInstalled("i6DMC");
            if (!bInstalled)
            {
                bInstalled = this.IsModuleInstalled("i6DMC");
            }
            ht.Add("i6DMC", bInstalled);

            //i6Server
            bInstalled = this.IsMainModuleInstalled("i6Server");
            ht.Add("i6Server", bInstalled);
            //i6EC
            bInstalled = this.IsMainModuleInstalled("i6EC");
            ht.Add("i6EC", bInstalled);

            //UIC 特殊处理
            bInstalled = this.IsMainModuleInstalled("UIC");
            ht.Add("UIC", bInstalled);
            ht.Add("UIC.UIC", bInstalled);

            bInstalled = this.IsMainModuleInstalled("UIC.DMC");
            ht.Add("UIC.DMC", bInstalled);


            //i6CRM
            bInstalled = this.IsMainModuleInstalled("i6CRM");
            ht.Add("i6CRM", bInstalled);

            //i6HR
            bInstalled = this.IsMainModuleInstalled("i6HR");
            ht.Add("i6HR", bInstalled);

            //i6WM
            bInstalled = this.IsMainModuleInstalled("i6WM");
            ht.Add("i6WM", bInstalled);

            //W3 ----------------------------------------
            bInstalled = this.IsMainModuleInstalled("W3");
            ht.Add("W3", bInstalled);
            //W3.CRM
            bInstalled = this.IsMainModuleInstalled("W3.CRM");
            ht.Add("W3.CRM", bInstalled);
            //W3.HR
            bInstalled = this.IsMainModuleInstalled("W3.HR");
            ht.Add("W3.HR", bInstalled);

            //W3.WM
            bInstalled = this.IsMainModuleInstalled("W3.WM");
            ht.Add("W3.WM", bInstalled);

            //W3.DMC
            bInstalled = this.IsMainModuleInstalled("W3.DMC");
            ht.Add("W3.DMC", bInstalled);

            //A3 ----------------------------------------
            bInstalled = this.IsMainModuleInstalled("A3");
            ht.Add("A3", bInstalled);
            //A3.CRM
            bInstalled = this.IsMainModuleInstalled("A3.CRM");
            ht.Add("A3.CRM", bInstalled);
            //W3.HR
            bInstalled = this.IsMainModuleInstalled("A3.HR");
            ht.Add("A3.HR", bInstalled);

            //A3.WM
            bInstalled = this.IsMainModuleInstalled("A3.WM");
            ht.Add("A3.WM", bInstalled);

            //i6EPM
            bInstalled = this.IsMainModuleInstalled("i6EPM");
            ht.Add("i6EPM", bInstalled);
            //M3----------------------------------------
            bInstalled = this.IsMainModuleInstalled("M3");
            ht.Add("M3", bInstalled);

            //CO----------------------------------------
            bInstalled = this.IsMainModuleInstalled("CO");
            ht.Add("CO", bInstalled);

            //CM----------------------------------------
            bInstalled = this.IsMainModuleInstalled("i6CM");
            ht.Add("i6CM", bInstalled);

            //EM----------------------------------------
            bInstalled = this.IsMainModuleInstalled("i6EM");
            ht.Add("i6EM", bInstalled);

            //EAM----------------------------------------
            bInstalled = this.IsMainModuleInstalled("i6EAM");
            ht.Add("i6EAM", bInstalled);


            //PMS----------------------------------------
            bInstalled = this.IsMainModuleInstalled("PMS");
            ht.Add("PMS", bInstalled);

            //PCM----------------------------------------
            bInstalled = this.IsMainModuleInstalled("PCM");
            ht.Add("PCM", bInstalled);

            //PMM----------------------------------------
            bInstalled = this.IsMainModuleInstalled("PMM");
            ht.Add("PMM", bInstalled);

            //PCO----------------------------------------
            bInstalled = this.IsMainModuleInstalled("PCO");
            ht.Add("PCO", bInstalled);

            //PS----------------------------------------
            bInstalled = this.IsMainModuleInstalled("PS");
            ht.Add("PS", bInstalled);

            //SUP----------------------------------------
            bInstalled = this.IsMainModuleInstalled("SUP");
            ht.Add("SUP", bInstalled);

            return ht;
        }
        #endregion

        #region 获取模块信息方法
        /// <summary>
        /// 判断是否32位或者是64位
        /// </summary>
        /// <returns></returns>
        private Boolean DetectSystemIs64()
        {
            ConnectionOptions oConn = new ConnectionOptions();
            ManagementScope oMS = new ManagementScope("\\\\localhost", oConn);
            ObjectQuery oQuery = new ObjectQuery("select AddressWidth from Win32_Processor");
            ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMS, oQuery);
            ManagementObjectCollection oReturnCollection = oSearcher.Get();
            string addressWidth = string.Empty;
            foreach (ManagementObject oReturn in oReturnCollection)
            {
                addressWidth = oReturn["AddressWidth"].ToString();
            }
            if (addressWidth == "64")
                return true;
            return false;
        }

        /// <summary>
        /// 判断模块是否安装
        /// </summary>
        /// <param name="AModuleID"></param>
        /// <returns></returns>
        private bool IsModuleInstalled(string AModuleID)
        {
            if (string.IsNullOrEmpty(sInstalledMainModluesRegeditItem))
            {
                Boolean is64 = DetectSystemIs64();
                if (m_product == null) m_product = "";

                if (m_product == "W3")
                {
                    sInstalledMainModluesRegeditItem = @"HKEY_LOCAL_MACHINE\SOFTWARE\NG\w3MidServer";
                }
                else if (m_product == "UIC")
                {
                    sInstalledMainModluesRegeditItem = @"HKEY_LOCAL_MACHINE\SOFTWARE\NG\uicMidServer";
                }
                else if (m_product == "M3")
                {
                    sInstalledMainModluesRegeditItem = @"HKEY_LOCAL_MACHINE\SOFTWARE\NG\m3MidServer";
                }
                else if (m_product.ToUpper() == "I6P")
                {
                    sInstalledMainModluesRegeditItem = @"HKEY_LOCAL_MACHINE\SOFTWARE\NG\i6PMidServer";
                }
                else if (m_product.ToUpper() == "I6S")
                {
                    sInstalledMainModluesRegeditItem = @"HKEY_LOCAL_MACHINE\SOFTWARE\NG\i6SMidServer";
                }
                else if (m_product.ToUpper() == "GE")
                {
                    sInstalledMainModluesRegeditItem = @"HKEY_LOCAL_MACHINE\SOFTWARE\NG\GEMidServer";
                }
                else if (m_product.ToUpper() == "A3")
                {
                    sInstalledMainModluesRegeditItem = @"HKEY_LOCAL_MACHINE\SOFTWARE\NG\A3MidServer";
                }
                else if (m_product.ToUpper() == "GEP")
                {
                    sInstalledMainModluesRegeditItem = @"HKEY_LOCAL_MACHINE\SOFTWARE\NG\GEPMidServer";
                }
                else
                {
                    sInstalledMainModluesRegeditItem = @"HKEY_LOCAL_MACHINE\SOFTWARE\NG\i6MidServer";
                }
                if (is64)
                    sInstalledMainModluesRegeditItem = sInstalledMainModluesRegeditItem.Replace(@"HKEY_LOCAL_MACHINE\SOFTWARE", @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node");
            }

            string sReturn = NG3.Win32.RegistryService.GetString(sInstalledMainModluesRegeditItem, "InstalledModules");
            if (sReturn == null) return false;
            if (sReturn.ToUpper().IndexOf(AModuleID.ToUpper()) >= 0) return true;
            return false;
        }
        /// <summary>
        /// 判断主模块是否安装
        /// </summary>
        /// <param name="AModuleID"></param>
        /// <returns></returns>
        private bool IsMainModuleInstalled(string AModuleID)
        {
            if (string.IsNullOrEmpty(sInstalledModulesRegeditItem))
            {
                Boolean is64 = DetectSystemIs64();
                if (m_product == null) m_product = "";

                string fmtStr = is64 ? @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\NG\{0}MidServer" : @"HKEY_LOCAL_MACHINE\SOFTWARE\NG\{0}MidServer";

                switch (m_product)
                {
                    case "W3":
                    case "UIC":
                    case "M3":
                    case "I6P":
                    case "I6S":
                    case "GE":
                    case "A3":
                    case "GEP":
                        sInstalledModulesRegeditItem = string.Format(fmtStr, m_product);
                        break;
                    default:
                        sInstalledModulesRegeditItem = string.Format(fmtStr, "i6");
                        break;
                }
            }

            string sReturn = NG3.Win32.RegistryService.GetString(sInstalledModulesRegeditItem, "InstalledMainModlues");
            if (sReturn == null) return false;
            if (sReturn.ToUpper().IndexOf(AModuleID.ToUpper()) >= 0) return true;
            return false;
        }
        /// <summary>
        /// 模块合法验证信息[单个模块],来判断是否已购买：0表示已购买，1表示未购买
        /// </summary>
        /// <param name="AModuleID"></param>
        /// <returns></returns>
        private bool IsModuleDemo(string AModuleID)
        {
            string sRetu = "";

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    sRetu = NSServerProxy.License_IsModuleDemo(0, true, int.Parse(this.ProductCode), AModuleID);
                    break;
                }
                catch (Exception ex)
                {
                    //Logger.Error(ex.Message);
                    sRetu = "";
                }
                Thread.Sleep(200);
            }
            //return false;

            // 执行数据解密处理
            sRetu = lzwZipService.Level2DecodeFromBase64(sRetu);

            if (sRetu == null || sRetu == "") sRetu = "1";

            if (sRetu == "0") return false; else return true;
        }
        /// <summary>
        /// 模块合法验证信息[所有模块],来判断是否已购买：0表示已购买，1表示未购买
        /// </summary>
        /// <param name="AModules"></param>
        /// <returns></returns>
        private string GetAllModulesDemo(string AModules)
        {
            string sRetu = "";

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    sRetu = NSServerProxy.License_IsModulesDemo(0, true, int.Parse(this.ProductCode), AModules);
                    break;
                }
                catch (Exception ex)
                {
                    //LogHelper.GetCommonLogger().Info(ex.Message);
                    sRetu = "";
                }
                Thread.Sleep(200);
            }

            // 执行数据解密处理
            sRetu = lzwZipService.Level2DecodeFromBase64(sRetu);

            return sRetu.ToString();
        }
        #endregion

        #region 获取组织模块授权
        /// <summary>
        /// 
        /// </summary>
        public Hashtable ModuleRightsCount
        {
            get
            {
                Hashtable ht = new Hashtable();

                IMemCachedClient client = MemCachedClientFactory.GetMemCachedClient();
                ht = client.Get<Hashtable>(NG_LEGAL_MODULERIGHTSCOUNT);
                if (ht == null)
                {
                    ht = this.GetModuleRightsCount();
                    client.Set(NG_LEGAL_MODULERIGHTSCOUNT, ht, 720);
                }

                return ht;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Hashtable GetModuleRightsCount()
        {
            Hashtable ht = new Hashtable();
            string sRetu;

            try
            {
                string strNSServerIP = this.NSServerAddr;
                if (strNSServerIP.IndexOf(":") > 0)
                {
                    strNSServerIP = strNSServerIP.Substring(0, strNSServerIP.IndexOf(":"));
                }
                string url = "http://" + strNSServerIP + ":8035/NSServer/Rest/ModulesInfo/GetModulesOrgNum";

                HttpClient httpClient = new HttpClient();
                var task = httpClient.GetAsync(new Uri(url));
                task.Result.EnsureSuccessStatusCode();//方法EnsureSuccessStatusCode:Web服务器返回HTTP错误状态代码，则引发异常
                sRetu = task.Result.Content.ReadAsStringAsync().Result;//方法ReadAsStringAsync:将HTTP内容作为异步操作写入到字符串中
            }
            catch
            {
                sRetu = "";
            }

            // 执行数据解密处理
            sRetu = lzwZipService.Level2DecodeFromBase64(sRetu);
            List<ModuleRightObject> list = JsonConvert.DeserializeObject(sRetu, typeof(List<ModuleRightObject>)) as List<ModuleRightObject>;

            if (list != null)
            {
                foreach (var item in list)
                {
                    //产品信息小写，易于前端判断
                    if (item.Sn == null) continue;//演示版

                    if (item.Sn.Equals(this.SN, StringComparison.OrdinalIgnoreCase))
                    {
                        ht.Add(item.moduleId.ToLower(), item.controlNum);
                    }
                }
            }

            return ht;
        }
        #endregion

        #region 其它方法
        public void OutputLog(string msg)
        {
            try
            {
                Debug.WriteLine(DateTime.Now.ToString() + msg + "\r\n");
            }
            catch { }
        }
        #endregion
    }

    [Serializable]
    public class ModuleRightObject
    {
        public string moduleId { get; set; }
        public string controlNum { get; set; }
        public string Sn { get; set; }
    }

}
