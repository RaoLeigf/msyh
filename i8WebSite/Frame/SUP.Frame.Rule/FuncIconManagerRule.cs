using System;
using System.Data;
using System.IO;
using System.Web;
using System.Collections.Generic;
using SUP.Frame.DataAccess;
using SUP.Frame.DataEntity;
using SUP.Common.Rule;
using NG3.Bill.Base;

namespace SUP.Frame.Rule
{
    public class FuncIconManagerRule
    {
        private FuncIconManagerDac dac = null;

        public FuncIconManagerRule()
        {
            dac = new FuncIconManagerDac();
        }

        public DataTable GetFuncIconGrid(string code, string suite, ref int totalRecord)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.TableName = "FuncIconGrid";

                dt.Columns.Add(new DataColumn("BusPhid", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("FuncIconId", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("FuncIconName", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("FuncIconSrc", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("FuncIconDefaultSrc", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("FuncName", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("FuncPath", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("DefaultIcon", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("CustomIcon", Type.GetType("System.String")));

                string funcPath = suite + new CustomFloatMenuRule().GetFullMenuNameByCode(code).Replace("-", "\\");
                DataRow dr;
                string icontype = "";
                string srcPre = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host
                    + ":" + HttpContext.Current.Request.Url.Port + "/" + HttpContext.Current.Request.Url.Segments[1];
                string iconname;
                string funciconname;

                DataTable menuDt = dac.GetChildMenuDtByCode(code);

                totalRecord = menuDt.Rows.Count;
                funcPath = funcPath.Substring(0, funcPath.LastIndexOf("\\"));

                for (int i = 0; i < totalRecord; i++)
                {
                    dr = dt.NewRow();
                    dr["BusPhid"] = menuDt.Rows[i]["busphid"];
                    dr["FuncIconId"] = menuDt.Rows[i]["funciconid"];
                    funciconname = menuDt.Rows[i]["funciconname"].ToString();
                    dr["FuncIconName"] = menuDt.Rows[i]["funcicondisplayname"];
                    dr["FuncPath"] = funcPath + "\\" + menuDt.Rows[i]["name"];
                    dr["FuncName"] = dr["FuncPath"].ToString().Substring(dr["FuncPath"].ToString().LastIndexOf("\\") + 1);
                    if (dr["FuncIconName"].ToString() != "")
                    {
                        dr["CustomIcon"] = "√";
                        if (funciconname != "")
                        {
                            dr["FuncIconDefaultSrc"] = srcPre + "NG3Resource/FuncIcons/" + funciconname;
                        }
                        else
                        {
                            dr["FuncIconDefaultSrc"] = srcPre + "NG3Resource/FuncIcons/通用.png";
                        }
                    }
                    else
                    {
                        if (funciconname != "")
                        {
                            dr["DefaultIcon"] = "√";
                        }
                        else
                        {
                            dr["FuncIconDefaultSrc"] = srcPre + "NG3Resource/FuncIcons/通用.png";
                        }
                    }
                    if (dr["FuncIconId"].ToString() != "")
                    {
                        try
                        {
                            iconname = dac.GetFuncIconNameType(dr["FuncIconId"].ToString(), ref icontype);
                            if (icontype == "0")
                            {
                                dr["FuncIconSrc"] = srcPre + "NG3Resource/FuncIcons/" + iconname;
                            }
                            else if (icontype == "1")
                            {
                                dr["FuncIconSrc"] = srcPre + "NG3Resource/CustomIcons/" + iconname;
                            }
                        }
                        catch
                        {
                            if (funciconname != "")
                            {
                                dr["FuncIconSrc"] = srcPre + "NG3Resource/FuncIcons/" + funciconname;
                            }
                            else
                            {
                                dr["FuncIconSrc"] = srcPre + "NG3Resource/FuncIcons/通用.png";
                            }
                        }
                    }
                    else
                    {
                        if (funciconname != "")
                        {
                            dr["FuncIconSrc"] = srcPre + "NG3Resource/FuncIcons/" + funciconname;
                        }
                        else
                        {
                            dr["FuncIconSrc"] = srcPre + "NG3Resource/FuncIcons/通用.png";
                        }
                    }
                    dt.Rows.Add(dr);
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetFuncIcons(string tag, ref int totalRecord)
        {
            return GetFuncIcons(true, true, tag, ref totalRecord);
        }

        public DataTable GetFuncIcons(bool buildinIconShow, bool customIconShow, string tag, ref int totalRecord)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.TableName = "FuncIcons";

                dt.Columns.Add(new DataColumn("id", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("name", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("src", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("tag", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("icontype", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("attachid", Type.GetType("System.String")));

                DataRow row;
                string path;
                string srcPre = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host
                    + ":" + HttpContext.Current.Request.Url.Port + "/" + HttpContext.Current.Request.Url.Segments[1];
                DataTable iconDt;

                if (buildinIconShow)
                {
                    path = AppDomain.CurrentDomain.BaseDirectory + @"NG3Resource\FuncIcons";
                    if (Directory.Exists(path))
                    {
                        iconDt = dac.GetIconDt("0");
                        totalRecord += iconDt.Rows.Count;
                        for (int i = 0; i < iconDt.Rows.Count; i++)
                        {
                            row = dt.NewRow();
                            row["id"] = iconDt.Rows[i]["phid"]; ;
                            row["name"] = iconDt.Rows[i]["name"];
                            row["src"] = srcPre + "/NG3Resource/FuncIcons/" + row["name"];
                            row["tag"] = iconDt.Rows[i]["tag"];
                            row["icontype"] = "0";
                            dt.Rows.Add(row);
                        }
                    }
                }

                if (customIconShow)
                {
                    path = AppDomain.CurrentDomain.BaseDirectory + @"NG3Resource\CustomIcons";
                    if(!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    if (Directory.Exists(path))
                    {
                        iconDt = dac.GetIconDt("1");
                        totalRecord += iconDt.Rows.Count;
                        for (int i = 0; i < iconDt.Rows.Count; i++)
                        {
                            row = dt.NewRow();
                            row["id"] = iconDt.Rows[i]["phid"];
                            row["name"] = iconDt.Rows[i]["name"];
                            row["src"] = srcPre + "/NG3Resource/CustomIcons/" + row["name"];
                            if (File.Exists(path + "\\" + row["name"]))
                            {
                                row["tag"] = iconDt.Rows[i]["tag"];
                                row["icontype"] = "1";
                                row["attachid"] = iconDt.Rows[i]["attachid"];
                                dt.Rows.Add(row);
                            }
                            else
                            {
                                if (iconDt.Rows[i]["attachid"] != DBNull.Value)
                                {
                                    try
                                    {
                                        byte[] buffer = NG3UploadFileService.NG3GetEx("", (long)iconDt.Rows[i]["attachid"]);
                                        File.WriteAllBytes(path + "\\" + row["name"], buffer);

                                        row["tag"] = iconDt.Rows[i]["tag"];
                                        row["icontype"] = "1";
                                        row["attachid"] = iconDt.Rows[i]["attachid"];
                                        dt.Rows.Add(row);
                                    }
                                    catch
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }

                if (tag != "")
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = "tag like '%" + tag + ";%'";
                    dt = dv.ToTable();
                    totalRecord = dt.Rows.Count;
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool AddFuncIcon(string name, string tag, string attachid)
        {
            long phid = CommonUtil.GetPhId("fg3_funcicon"); 
            return dac.AddFuncIcon(phid, name, tag, attachid);
        }

        public bool AddFuncIconTag(string name)
        {
            long phid = CommonUtil.GetPhId("fg3_funcicontag");
            return dac.AddFuncIconTag(phid, name);
        }

        public bool DelFuncIcon(string id)
        {
            try
            {
                bool deletable = dac.CheckDelFuncIcon(id);
                if (deletable)
                {
                    DataTable dt = dac.GetFuncIconDtByPhid(id);
                    string tag = dt.Rows[0]["tag"].ToString();
                    string name = dt.Rows[0]["name"].ToString();

                    //删除图标文件
                    string path = AppDomain.CurrentDomain.BaseDirectory + @"NG3Resource\CustomIcons";
                    if (File.Exists(path + "\\" + name))
                    {
                        File.Delete(path + "\\" + name);
                    }

                    //删除附件表存储数据
                    if (dt.Rows[0]["attachid"] != DBNull.Value)
                    {
                        try
                        {
                            NG3UploadFileService.NG3Del("", (long)dt.Rows[0]["attachid"]);
                        }
                        catch { }
                    }

                    return dac.DelFuncIcon(id, tag);
                }
                else return deletable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool SaveFuncIconSet(List<FuncIconEntity> funcIconList)
        {
            bool flag = false;
            foreach (FuncIconEntity funcIcon in funcIconList)
            {
                if (funcIcon.iconid == "" && funcIcon.name == "")
                {
                    continue;
                }

                string result = dac.GetMenuFuncIconCount(funcIcon.busphid);
                if (result != "0")
                {
                    dac.UpdateMenuFuncIcon(funcIcon.busphid, funcIcon.iconid, funcIcon.name);
                }
                else
                {
                    long phid = CommonUtil.GetPhId("fg3_menufuncicon");
                    dac.InsertMenuFuncIcon(phid, funcIcon.busphid, funcIcon.iconid, funcIcon.name);
                }

                flag = true;
            }

            if (flag)
            {
                new MainTreeDac().SetTimeStamp(NG3.AppInfoBase.DbServerName.ToUpper() + "-" + NG3.AppInfoBase.UCode + "-" + "FuncIconManager", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            }

            return flag;
        }

    }
}
