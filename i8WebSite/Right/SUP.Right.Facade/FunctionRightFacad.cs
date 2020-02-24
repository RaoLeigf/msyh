using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Interface;
using SUP.Right.Rules;

namespace SUP.Right.Facade
{
    
    public class ButtonRightFacad : FunctionRightFacad
    {
        [NG3.DBControl(NG3.DbControlOption.BeginTransaction)]
        public bool Validate(string ocode, string logid, string rightname, string funcname, params object[] objs)
        {
            return manager.ButtonRight.Validate(ocode, logid, rightname, funcname, objs);
        }
    }

    public class EntryRightFacad : FunctionRightFacad
    {
        [NG3.DBControl(NG3.DbControlOption.BeginTransaction)]
        public bool Validate(string ocode, string logid, string rightname, string funcname, params object[] objs)
        {
            return manager.EntryRight.Validate(ocode, logid, rightname, funcname, objs);
        }
    }

    public class ButtonRightByMenuFacad : FunctionRightFacad
    {
        [NG3.DBControl(NG3.DbControlOption.BeginTransaction)]
        public bool Validate(string ocode, string logid, string rightname, string funcname, params object[] objs)
        {
            return manager.ButtonRightByMenu.Validate(ocode, logid, rightname, funcname, objs);
        }
    }

    public class FunctionRightFacad
    {
        protected Manager manager = new Manager();
        [NG3.DBControl]
        public bool Add(IRightEntity entity)
        {
            return manager.FieldRight.Update(entity);
        }
        [NG3.DBControl]
        public bool Remove(IRightEntity entity)
        {
            return manager.FieldRight.Update(entity);
        }

        [NG3.DBControl]
        public bool Update(IRightEntity entity)
        {

            return manager.FieldRight.Update(entity);
        }
    }
}
