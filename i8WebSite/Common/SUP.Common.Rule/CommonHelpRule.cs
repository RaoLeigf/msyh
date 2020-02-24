using System.Collections.Generic;
using System.Linq;
using System.Text;

using SUP.Common.DataEntity;
using SUP.Common.DataAccess;
using SUP.CustomForm.DataAccess;

namespace SUP.Common.Rule
{
    public class CommonHelpRule
    {

        private CommonHelpDac commDac = new CommonHelpDac();
        private RichHelpDac richDac = new RichHelpDac();
        private HelpDac helpDac = new HelpDac();


        public CommonHelpRule()
        {

        }


        public string GetHelpObject(string helpid)
        {
            CommonHelpDac dac = new CommonHelpDac();
            CommonHelpEntity item = dac.GetCommonHelpItem(helpid);

            var sb = new StringBuilder("var helpObj={");          
            sb.Append("title:'" + item.Title + "',");
            sb.Append("icon:'" + string.Empty + "',");
            sb.Append("codeField:'" + item.CodeField + "',");
            sb.Append("nameField:'" + item.NameField + "',");
            sb.Append("selectMode:" + 1 + ",");
            sb.Append("t0:'");
            sb.Append(string.Empty);
            sb.Append("',t1:'");
            sb.Append(dac.GetQueryTemplate(helpid).Replace("\r\n", string.Empty).Replace("'", "''"));
            sb.Append("',t2:'");
            sb.Append(dac.GetListTemplate(helpid).Replace("\r\n", string.Empty).Replace("'", "''"));
            sb.Append("'\r\n};");            

            return sb.ToString();
        }

        //批量代码转名称
        public HelpValueNameEntity[] GetAllNames(IList<HelpValueNameEntity> list)
        {

            //foreach (HelpValueNameEntity item in list)
            //{
            //    item.Name = commDac.GetName(item.HelpID, item.Code, item.HelpType, string.Empty, item.OutJsonQuery, item.HelpType);
            //}

            foreach (HelpValueNameEntity item in list)
            {
                //if (item.HelpType == "ngRichHelp" || item.HelpType == "ngMultiRichHelp")//从数据库获取帮助信息
                //{
                //    item.Name = richDac.GetName(item.HelpID, item.Code, item.SelectMode, string.Empty, item.OutJsonQuery, item.HelpType);
                //}
                //else//ngCommonHelp,ngComboBox,ngTreeComboBox暂时从xml配置文件获得帮助信息
                //{
                //    item.Name = commDac.GetName(item.HelpID, item.Code, item.SelectMode, string.Empty, item.OutJsonQuery, item.HelpType);

                //}

                if (item.HelpType == "ngCommonHelp")//ngCommonHelp暂时从xml配置文件获得帮助信息
                {
                    item.Name = commDac.GetName(item.HelpID, item.Code, item.SelectMode, string.Empty, item.OutJsonQuery, item.HelpType);
                }
                else if (item.HelpType == "ngCustomFormHelp" || item.HelpType == "ngCustFormMutilHelp")
                {
                    item.Name = helpDac.GetName(item.HelpID, item.Code, item.SelectMode);
                }
                else//从数据库获取帮助信息
                {
                    item.Name = richDac.GetName(item.HelpID, item.Code, item.SelectMode, string.Empty, item.OutJsonQuery, item.HelpType);
                }
            }
          
            return list.ToArray<HelpValueNameEntity>();
        }
    }
}
