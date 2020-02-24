using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Interface;

namespace SUP.Right.Rules
{
    /// <summary>
    /// 权限管理
    /// </summary>
    public class Info : IRight
    {

        protected Services services = new Services();

        public bool Add(IRightEntity entity)
        {
            return false;
        }

        public bool Remove(IRightEntity entity)
        {
            return false;
        }

        public bool Update(IRightEntity entity)
        {
            return false;
        }

        public virtual bool Validate(string ocode, string logid, string rightname, string funcname, params object[] objs)
        {
            return false;
        }

        public CType GetInfo<CType>(string ocode, string logid, params object[] objs)
        {
            CType type = default(CType);
            //type = (CType)DataSource;
            return type;
        }
    }
}
