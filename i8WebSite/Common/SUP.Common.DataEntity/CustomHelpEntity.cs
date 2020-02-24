using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Common.DataEntity
{
    public class CustomHelpEntity
    {
        public string HelpId
        {
            get; set; 
        }

        public string Title
        {
            get;set;
        }

        public string HeadText
        {
            get; set;
        }

        public string TableName
        {
            get; set;
        }

        public string CodeField
        {
            get; set;
        }

        public string NameField 
        {
            get; set;
        }

        public string AllField 
        { 
            get; set;
        }

        public string FromSql
        {
            get; set;
        }

        public string Sql
        {
            get; set;
        }
    }
}
