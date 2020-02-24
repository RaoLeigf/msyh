using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Interface;
using SUP.Right.Rules;
using NG3.Data.Service;

namespace SUP.Right.Facade
{
    public class InfoRightFacad
    {
        public bool Validate(string ocode, string logid, string rightname, string funcname, params object[] objs)
        {
            try
            {
                DbHelper.Open();
                return (new SUP.Right.Rules.Manager()).InfoRight.Validate(ocode, logid, rightname, funcname, objs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close();
            }
        }
    }
}
