using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Interface;
using SUP.Right.Rules;
using NG3.Aop.Transaction;
using SUP.Right.Facade;

namespace SUP.Right.Controller
{
    public class FieldRightController
    {
         private FieldRightFacad rightFacad;

         public FieldRightController()
        {
            rightFacad = AopObjectProxy.GetObject<FieldRightFacad>(new FieldRightFacad());
        }

        public bool Validate(string ocode, string logid, string rightname, string funcname, params object[] objs)
        {

            return rightFacad.Validate(ocode, logid, rightname, funcname, objs);
        }
    }
}
