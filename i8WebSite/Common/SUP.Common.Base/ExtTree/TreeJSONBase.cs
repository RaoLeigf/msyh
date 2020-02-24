using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Common.Base
{
    //ext树的数据格式
    [Serializable]
    public class TreeJSONBase
    {
        private bool _leaf  = false;

        private bool _leafSeted = false;//叶子节点是否标记过

        public bool LeafSeted
        {
            get { return _leafSeted; }            
        }

        public virtual string id { get; set; }

        public virtual Int64 PhId { get; set; }//phid都是int64

        public virtual string text { get; set; }

        public virtual string cls { get; set; }

        public virtual bool expanded { get; set; }

        public virtual IList<TreeJSONBase> children { get; set; }

        public  bool leaf { 
            get {return _leaf;}
            set { _leaf = value; _leafSeted = true; } 
        }

        //public virtual bool @checked { get; set; }

        public virtual string hrefTarget { get; set; }

        public virtual int myLevel { get; set; }

        public virtual bool allowDrag { get; set; }

        public virtual string exparams { get; set; }

        public virtual string customsort { get; set; }

        public virtual string iconCls { get; set; }

        public virtual bool disabled { get; set; }

    } 
}
