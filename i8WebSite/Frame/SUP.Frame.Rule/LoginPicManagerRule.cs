using NG3.Bill.Base;
using SUP.Common.Rule;
using SUP.Frame.DataAccess;
using System;
using System.Data;
using System.IO;

namespace SUP.Frame.Rule
{
    public class LoginPicManagerRule
    {

        private LoginPicManagerDac dac = null;

        public LoginPicManagerRule()
        {
            dac = new LoginPicManagerDac();
        }

        public void AddNode(string id, string name, string src, string attachid)
        {
            long phid = CommonUtil.GetPhId("fg3_loginpicture");
            dac.AddNode(phid, id, name, src, attachid);
        }

        public void DelNode(string phid)
        {
            try
            {
                string rootpath = AppDomain.CurrentDomain.BaseDirectory;
                DataTable dt = dac.GetLoginPicture(phid);

                //图片节点需要删除图片文件以及附件表存储数据
                if (dt.Rows[0]["type"].ToString() == "1")
                {
                    if (!string.IsNullOrEmpty(dt.Rows[0]["src"].ToString()))
                    {
                        string src = dt.Rows[0]["src"].ToString();
                        string path = Path.Combine(rootpath, src);
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }

                    if(dt.Rows[0]["attachid"] != DBNull.Value)
                    {
                        try
                        {
                            NG3UploadFileService.NG3Del("", (long)dt.Rows[0]["attachid"]);
                        }
                        catch { }
                    }                   
                }

                dac.DelNode(phid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
