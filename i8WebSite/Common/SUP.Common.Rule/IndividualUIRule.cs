using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SUP.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Common.Rule
{
    public class IndividualUIRule
    {

        private IndividualUIDac dac;

        public IndividualUIRule()
        {
            dac = new IndividualUIDac();
        }

        public int Save(DataTable dt, List<string> dellist)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState == DataRowState.Deleted) continue;

                if (dr.RowState == DataRowState.Added)
                {
                    //dr["code"] = Guid.NewGuid().ToString();
                    //dr["phid"] = CommonUtil.GetMaxId("fg_individualinfo");//CommonUtil.GetPhId("fg_individualinfo");
                    dr["phid"] = CommonUtil.GetPhId("fg_individualinfo");
                }
            }


            return dac.Save(dt, dellist);
        }
                
        public string GetIndividualColumnForList(string bustype, List<string> tables)
        {
            JArray modelArr = new JArray();//model
            JArray colArr = new JArray();//列

            DataTable dt = dac.GetIndividualColumnForList(bustype, tables);

            foreach (DataRow dr in dt.Rows)
            {
                string uixtype = dr["uixtype"].ToString();
                string colname = dr["c_name"].ToString();
                string header = dr["c_fullname"].ToString();
                var modelObj = new JObject
                {
                    {"name", colname},
                    {"type", ""},
                    {"mapping", colname}
                };

                modelArr.Add(modelObj);

                if (uixtype == "ngRichHelp")
                {

                    var modelNameObj = new JObject
                    {
                       {"name", colname+"_name"},
                       {"type", ""},
                       {"mapping", colname+"_name"}
                    };
                    modelArr.Add(modelNameObj);

                    var colObj = new JObject
                   {
                      {"header", header},
                      {"hidden", true},
                      {"dataIndex", colname}
                   };

                    //名称列
                    var nameColObj = new JObject
                   {
                      {"header", header},
                      {"dataIndex", colname + "_name"}
                   };
                    colArr.Add(colObj);
                    colArr.Add(nameColObj);
                }
                else if (uixtype == "ngComboBox")
                {
                    var colObj = new JObject
                   {
                      {"header", header},
                      {"dataIndex", colname},
                      {"renderer", GetRenderer(dr["combodata"].ToString())},
                   };
                   colArr.Add(colObj);
                }
                else
                {
                    var colObj = new JObject
                   {
                      {"header", header},                     
                      {"dataIndex", colname}
                   };
                   colArr.Add(colObj);
                }              

            }

            var jo = new JObject
            {
               {"models", modelArr},
               {"columns", colArr}
            };

            string s = Newtonsoft.Json.JsonConvert.SerializeObject(jo);
            return s;
        }

        public string GetIndividualUI(string bustype)
        {
            string str = dac.GetIndividualUIByOrgID(bustype);//按登录组织获取界面方案

            if (string.IsNullOrWhiteSpace(str))
            {
                str = dac.GetIndividualUI(bustype);
            }

            return str;
        }
        public string GetRenderer(string comboData)
        {
            StringBuilder sb = new StringBuilder();
            string[] arr = comboData.Split(';');
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (string item in arr)
            {
                string[] inarr = item.Split('|');
                dic.Add(inarr[0], inarr[1]);
            }

            sb.Append("function (val) { " );
            sb.Append(" var temp; " );
            sb.Append(" switch (val) { " );

            foreach (KeyValuePair<string,string> item in dic)
            {
                sb.Append(string.Format(" case '{0}': temp = '{1}'; break;", item.Key, item.Value) );
            }

            sb.Append("  default: temp = ''; }" );

            sb.Append("  return temp; }" );

            return sb.ToString();
            
        }

        /// <summary>
        /// 更新检测
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public string CheckUIInfo(ref List<Int64> idList, string bustype, string ids)
        {
            DataTable dt = dac.GetCheckInfo(bustype,ids);
            return Deal(dt,false, ref idList);
        }

        //更新
        public string UpdateUIInfo(string ids)
        {
            List<Int64> idList = null;
            DataTable dt = dac.GetUpdateInfo(ids);
            return Deal(dt,true, ref idList);
        }

        //处理
        public string Deal(DataTable dt, bool updateFlg, ref List<Int64> idList)
        {           

            idList = new List<Int64>();//有更新的id列表
            List<string> msgList = new List<string>();
            Dictionary<Int64, string> updateInfo = new Dictionary<Int64, string>();
            Dictionary<Int64, string> updateMsgDic = new Dictionary<long, string>();//升级日志

            Merge merger = new Merge();
            Checker checker = new Checker();
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    merger.Msg.Clear();
                    string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"NG3Resource\IndividualInfo\", dr["url"].ToString().Replace('/', '\\'));

                    //1、先检测用户模板
                    StringBuilder userTemplateMsg = new StringBuilder();
                    int userSuccessCount = 0;//成功
                    int userErrCount = 0;
                    string schemaName = dr["name"].ToString();
                    string bus = dr["bustype"].ToString() + "," + dr["busname"].ToString();
                    string info = dr["individualinfo"].ToString();
                    CheckUser(checker, info, userTemplateMsg, ref userSuccessCount, ref userErrCount, bus, schemaName);
                    if (userSuccessCount == 0)
                    {
                        userTemplateMsg.Insert(0,string.Format("--------自定义界面【{0}{1}-{2}】检测不通过--------" + "\r\n", dr["bustype"].ToString(),
                                                         dr["busname"].ToString(), dr["name"].ToString()));
                        msgList.Add(userTemplateMsg.ToString());
                                      
                    }
                    //2、检测系统js模板
                    StringBuilder sysTemplateMsg = new StringBuilder();
                    int sysSuccessCount = 0;//成功
                    int sysErrCount = 0;
                    int notExistCount = 0;
                    CheckSys(checker, merger, sysTemplateMsg, ref sysSuccessCount, ref sysErrCount, ref notExistCount, bus, file);
                    if (sysSuccessCount == 0)
                    {
                        sysTemplateMsg.Insert(0, string.Format("--------自定义界面js模板【{0}{1}-{2}】检测不通过--------" + "\r\n", dr["bustype"].ToString(),
                                                      dr["busname"].ToString(), dr["name"].ToString()));
                        msgList.Add(sysTemplateMsg.ToString());
                    }

                    if (userSuccessCount == 0 || sysSuccessCount == 0)
                    {
                        continue;//用户模板、用户模板检测不通过     
                    }

                    //3、检测更新
                    JObject newJo = null;
                    try
                    {
                        string newUI = merger.GetNewFromPlugin(file);
                        newJo = JsonConvert.DeserializeObject<JObject>(newUI);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("加载模板文件【{0}】出错:{1}", file, ex.Message));
                    }

                    string individualUI = dr["individualinfo"].ToString();//数据库中的自定义UI信息
                    JObject individualJo = JsonConvert.DeserializeObject<JObject>(individualUI);
                    //处理form
                    JObject newFormJa = newJo["form"] as JObject;
                    JObject indvidualFormJo = individualJo["form"] as JObject;
                    //处理fieldSetForm
                    JObject newFieldsetFormJa = newJo["fieldSetForm"] as JObject;
                    JObject individualFieldsetFormJo = individualJo["fieldSetForm"] as JObject;

                    //查找新版本UI插件中的字段
                    DataTable newList = merger.InitialNewFieldList(newFormJa, newFieldsetFormJa);
                    //查找用户自定义信息中的字段信息        
                    DataTable indList = merger.InitialIndividualFieldList(indvidualFormJo, individualFieldsetFormJo);

                    //检测、处理移动的字段（字段从p1移动到p2,但是一张表的字段分布在两个panel的时候，升级时全移动到一个panel中，有问题，处理不了，人工处理）
                    //merger.DeleteField(indvidualFormJo, individualFieldsetFormJo, newList, indList);

                    //处理form
                    merger.MergeForm(newFormJa, indvidualFormJo, indList);
                    //处理fieldsetForm
                    merger.MergeFieldsetForm(newFieldsetFormJa, individualFieldsetFormJo, indList);
                    //处理grid
                    JObject newGridJa = newJo["grid"] as JObject;
                    JObject indvidualGridJo = individualJo["grid"] as JObject;
                    merger.MergeGrid(newGridJa, indvidualGridJo);
                    //处理tabPanel
                    JObject newTabJa = newJo["tabPanel"] as JObject;
                    JObject indvidualTabJo = individualJo["tabPanel"] as JObject;
                    merger.MergeTabPanel(newTabJa, indvidualTabJo);
                
                    if (merger.Msg.Length > 0)//有合并信息，并且用户模板检测不通过
                    {
                        idList.Add(Convert.ToInt64(dr["phid"]));
                        if (updateFlg)
                        {
                            updateMsgDic.Add(Convert.ToInt64(dr["phid"]), merger.Msg.ToString());//升级信息
                        }
                        merger.Msg.Insert(0, string.Format("--------自定义界面【{0}{1}-{2}】有更新--------" + "\r\n", dr["bustype"].ToString(),
                                                         dr["busname"].ToString(), dr["name"].ToString()));
                        msgList.Add(merger.Msg.ToString());
                    }                   


                    if (updateFlg)//记录更新信息
                    {
                        string str = JsonConvert.SerializeObject(individualJo);
                        updateInfo.Add(Convert.ToInt64(dr["phid"]), str);
                    }
                }//try
                catch (Exception ex)
                {
                    merger.Msg.AppendLine(string.Format("--------自定义界面【{0}{1}-{2}】检测不通过--------", dr["bustype"].ToString(),
                                                     dr["busname"].ToString(), dr["name"].ToString()));
                    merger.Msg.AppendLine("检测出错:" + ex.Message);                 
                    msgList.Add(merger.Msg.ToString());
                }

            }

            if (updateFlg)
            {
                BackAndUpdate(updateInfo, idList,updateMsgDic);//备份且更新
            }        

            StringBuilder sb = new StringBuilder();
            foreach (string str in msgList)
            {
                sb.AppendLine();
                sb.AppendLine(str);
                sb.AppendLine();
            }
            return sb.ToString();     
        }
              
        //备份并更新
        private int BackAndUpdate(Dictionary<Int64, string> updateInfo, List<Int64> idList, Dictionary<Int64, string> updateMsgDic)
        {
            DataTable updateDt = dac.GetUpdate(idList);
            DataTable backDt = dac.GetSchema();//获取空表

            
            foreach (DataRow dr in updateDt.Rows)
            {
                
                Int64 phid = Convert.ToInt64(dr["phid"]);
                //先备份
                DataRow backRow = backDt.NewRow();
                backRow.BeginEdit();
                backRow["phid"] = CommonUtil.GetPhId("fg_individualinfo");
                backRow["bustype"] = dr["bustype"];
                backRow["name"] = string.Format("{0}-(副本{1})",dr["name"],DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                backRow["remark"] = string.Format("{0}{1}-(副本),升级内容:{2}", dr["name"],phid,updateMsgDic[phid]);                
                backRow["defaultflg"] = "0";
                backRow["isbackup"] = "1";//是备份记录
                backRow["scriptcode_pub"] = dr["scriptcode_pub"];
                backRow["scriptcode_draft"] = dr["scriptcode_draft"];
                backRow["userdef_scripturl"] = dr["userdef_scripturl"];
                backRow["individualinfo"] = dr["individualinfo"]; 
                backRow.EndEdit();

                backDt.Rows.Add(backRow);

                //更新
                dr["individualinfo"] = updateInfo[phid];
            }

            return dac.Update(backDt, updateDt);

        }

        /// <summary>
        /// 检测系统模板
        /// </summary>
        /// <returns></returns>
        public string CheckSysTemplate()
        {
            StringBuilder msg = new StringBuilder();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"NG3Resource\IndividualInfo\");

            DataTable dt = dac.GetSysTemplateInfo();

            Merge merger = new Merge();
            Checker checker = new Checker();
            int successCount = 0;//成功
            int errCount = 0;
            int notExistCount = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string busType = dr["bustype"].ToString();
                string bus = dr["bustype"].ToString() + "," + dr["busname"].ToString();
                string file = Path.Combine(path, dr["url"].ToString().Replace('/', '\\'));
                CheckSys(checker,merger,msg, ref successCount, ref errCount, ref notExistCount, bus, file);
            }

            msg.AppendLine();
           msg.AppendLine(string.Format("总共检测{0}条数据,成功{1}条,失败{2}条", dt.Rows.Count,
                 successCount, errCount));

            return msg.ToString();   

        }

        private void CheckSys(Checker checker, Merge merger,StringBuilder msg, ref int successCount, ref int errCount, ref int notExistCount, string bus, string file)
        {
            try
            {
                if (File.Exists(file))
                {
                    string jsContent = merger.GetNewFromPlugin(file);
                    JObject newJo = JsonConvert.DeserializeObject<JObject>(jsContent);//检测格式

                    StringBuilder checkResult = new StringBuilder();
                    bool flg = true;
                    if (newJo != null)
                    {
                        JObject newFormJa = newJo["form"] as JObject;
                        JObject newFieldsetFormJa = newJo["fieldSetForm"] as JObject;
                        JObject newGridJa = newJo["grid"] as JObject;
                        JObject newTabJa = newJo["tabPanel"] as JObject;
                        flg = checker.Check(newFormJa, newFieldsetFormJa, newGridJa, newTabJa, checkResult);//检测内容
                    }

                    if (flg)
                    {
                        msg.AppendLine(string.Format("业务点【{0}】的自定义文件【{1}】解析成功！", bus, file));
                        successCount++;
                    }
                    else
                    {
                        msg.AppendLine(string.Format("业务点【{0}】的自定义文件【{1}】注册不规范：{2}", bus, file, checkResult));
                        errCount++;
                    }
                }
                else
                {
                    msg.AppendLine(string.Format("业务点【{0}】的自定义文件【{1}】不存在！", bus, file));
                    notExistCount++;
                }
            }
            catch (Exception ex)
            {
                msg.AppendLine(string.Format("业务点【{0}】的自定义文件【{1}】内容解析异常：{2}", bus, file, ex.Message));
                errCount++;
            }
        }

        public string CheckAllUserTemplate()
        {
            DataTable dt = dac.GetAllUserTemplateInfo();
            return CheckUserTemplate(dt);
        }

        /// <summary>
        /// 检测用户自定义模板
        /// </summary>
        /// <returns></returns>
        private string CheckUserTemplate(DataTable dt)
        {
            StringBuilder msg = new StringBuilder();
            //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\NG3Resource\IndividualInfo\");

            //DataTable dt = dac.GetAllUserTemplateInfo();

            Merge merger = new Merge();
            Checker checker = new Checker();
            int successCount = 0;//成功
            int errCount = 0;
            int notExistCount = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string schemaName = dr["name"].ToString();
                string bus = dr["bustype"].ToString() + "," + dr["busname"].ToString();
                string info = dr["individualinfo"].ToString();
                CheckUser(checker, info, msg, ref successCount, ref errCount, bus, schemaName);
            }

            msg.AppendLine();
            msg.AppendLine(string.Format("总共检测{0}条数据,成功{1}条,失败{2}条", dt.Rows.Count,
                  successCount, errCount, notExistCount));

            return msg.ToString();
        }

        private void CheckUser(Checker checker,string info, StringBuilder msg, ref int successCount, ref int errCount, string bus, string schemaName)
        {
            try
            {             
                JObject newJo = JsonConvert.DeserializeObject<JObject>(info);//检测格式

                StringBuilder checkResult = new StringBuilder();
                bool flg = true;
                if (newJo != null)
                {
                    JObject newFormJa = newJo["form"] as JObject;
                    JObject newFieldsetFormJa = newJo["fieldSetForm"] as JObject;
                    JObject newGridJa = newJo["grid"] as JObject;
                    JObject newTabJa = newJo["tabPanel"] as JObject;
                    flg = checker.Check(newFormJa, newFieldsetFormJa, newGridJa, newTabJa, checkResult);//检测内容
                }

                if (flg)
                {
                    msg.AppendLine(string.Format("自定义界面【{0}-{1}】解析成功！", bus, schemaName));
                    successCount++;
                }
                else
                {
                    msg.AppendLine(string.Format("自定义界面【{0}-{1}】注册不规范：{2}", bus, schemaName, checkResult));
                    errCount++;
                }

            }
            catch (Exception ex)
            {
                msg.AppendLine(string.Format("自定义界面【{0}-{1}】内容解析异常：{2}", bus, schemaName, ex.Message));
                errCount++;
            }
        }


        public int Copy(Int64 phid)
        {
            DataTable dt = dac.GetInfoById(phid);
            DataTable backDt = dac.GetSchema();//获取空表

            int iret = 0;
            if(dt.Rows.Count > 0) {

                DataRow dr = dt.Rows[0];               
               
                DataRow backRow = backDt.NewRow();
                backRow.BeginEdit();
                backRow["phid"] = CommonUtil.GetPhId("fg_individualinfo");
                backRow["bustype"] = dr["bustype"];
                backRow["name"] = string.Format("{0}-(复制)", dr["name"]);
                backRow["remark"] = string.Format("{0}-({1}复制{2})", dr["name"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), phid);
                backRow["defaultflg"] = "0";
                backRow["isbackup"] = "0";//是备份记录
                backRow["scriptcode_pub"] = dr["scriptcode_pub"];
                backRow["scriptcode_draft"] = dr["scriptcode_draft"];
                backRow["userdef_scripturl"] = dr["userdef_scripturl"];
                backRow["individualinfo"] = dr["individualinfo"];
                backRow.EndEdit();

                backDt.Rows.Add(backRow);

               iret = dac.SaveCopy(backDt);      
            }

            return iret;
        }

        public int SaveOrg(DataTable dtAddOrg, List<string> listDelOrg, string phid)
        {
            foreach (DataRow dr in dtAddOrg.Rows)
            {
                    //dr["code"] = Guid.NewGuid().ToString();
                    dr["phid"] = CommonUtil.GetPhId("fg_individualinfo_allot");                
            }
            return dac.SaveOrg(dtAddOrg, listDelOrg, phid);
        }

        public int SyncScript()
        {
            //foreach (DataRow dr in dtAddOrg.Rows)
            //{
            //    //dr["code"] = Guid.NewGuid().ToString();
            //    dr["phid"] = CommonUtil.GetPhId("fg_individualinfo_allot");
            //}
            return dac.SyncScript();
        }


    }


}
