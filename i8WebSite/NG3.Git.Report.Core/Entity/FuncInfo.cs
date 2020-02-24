using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core.Entity
{
    /// <summary>
    /// 函数信息
    /// </summary>
    public class FuncInfo
    {
        private string identifier;
        //函数所属模块     
        private string module;
        
        //函数名         
        private string name = "";

        //函数用法       
        private string usage = "";

        //函数详细信息         
        private string detail = "";
        //示例
        private string example = "";

        private IList<FuncParameter> paras;

        private EnumFuncDataType resultType;

        //函数计算结果
        private FuncCalcResult calcResult;
        //函数分解结果
        private FuncResolveResult resolveResult;
        //函数跟踪结果
        private FuncTrackResult trackResult;


        /// <summary>
        /// 整个函数运行时标识符
        /// </summary>
        public string Identifier
        {
            set { identifier = value; }
            get {
                //函数运算时的标识符，函数名加上函数参数
                StringBuilder sb = new StringBuilder();
                sb.Append(Name);
                sb.Append("-");
                paras.OrderBy(p => p.Index )
                    .ToList()
                    .ForEach(p => {
                        sb.Append(p.Value);
                        sb.Append("-");

                    });

                identifier = sb.ToString();
                return identifier;
            }
        }

        public string Module
        {
            set { module = value; }
            get { return module; }
        }

        public string Name
        {
            set { name = value; }
            get { return name; }
        }

        public string Usage
        {
            set { usage = value; }
            get { return usage; }
        }

        public string Detail
        {
            set { detail = value; }
            get { return detail; }
        }

        public string Example
        {
            set { example = value; }
            get { return example; }
        }

        public IList<FuncParameter> Paras
        {
            set { paras = value; }
            get { return paras; }
        }

        public FuncCalcResult CalcResult
        {
            get
            {
                return calcResult;
            }

            set
            {
                calcResult = value;
            }
        }

        public FuncResolveResult ResolveResult
        {
            get
            {
                return resolveResult;
            }

            set
            {
                resolveResult = value;
            }
        }

        public FuncTrackResult TrackResult
        {
            get
            {
                return trackResult;
            }

            set
            {
                trackResult = value;
            }
        }

        public EnumFuncDataType ResultType
        {
            get
            {
                return resultType;
            }

            set
            {
                resultType = value;
            }
        }
    }
}
