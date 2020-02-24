using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.Container
{
    public class FieldSetForm : ExtContainer
    {
        private List<FieldSet> fieldSets = new List<FieldSet>();

        public List<FieldSet> FieldSets
        {
            get { return fieldSets; }
            set { fieldSets = value; }
        }
    }
}
