using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Expression
{
    /// <summary>
    /// 操作数
    /// </summary>
    public class Operand
    {
        private object _value;

        public object Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        public Operand(object value)
        {
            Value = value;
        }

        //判断是否
        public bool IsString()
        {
            if (Value.GetType().ToString().Equals("System.String")) return true;

            return false;
        }

        //转换成数据型
        public static bool ToNumber(object value,out double val)
        {
            val = -1;
            if (value == null) return false;
            bool rtn = false;            
            rtn = double.TryParse(value.ToString(),out val);
            return rtn;
        }
        //转换成BOOL类型
        public static bool ToBool(object value, out bool val)
        {
            val = false;
            if (value == null) return false;

            bool rtn = false;
            rtn = bool.TryParse(value.ToString(), out val);

            return rtn;
        }

        //转换成datetime类型
        public static bool ToDateTime(object value, out DateTime val)
        {
            val = DateTime.Now;
            if (value == null) return false;


            bool rtn = false;
            string sdt = value.ToString();
            string sFormat = string.Empty;
            if (sdt.IndexOf(".") > 0)
                sFormat = "yyyy.MM.dd";
            else if (sdt.IndexOf("/") > 0)
                sFormat = "yyyy/MM/dd";
            else if (sdt.IndexOf("-") > 0)
                sFormat = "yyyy-MM-dd";

            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = sFormat;

            rtn = DateTime.TryParse(sdt, dtFormat, DateTimeStyles.None, out val);

            return rtn;
        }

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="operands"></param>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        /// <returns></returns>
        public static bool GetDoubles(Operand[] operands, out double op1, out double op2)
        {
            if(operands ==null) throw new AddinException("参数变量为空！");
            if (operands.Length < 2) throw new AddinException("参数个数出错！");
            op1 = 0; op2 = 0;
            bool rtn = Operand.ToNumber(operands[0].Value,out op1);
            if (!rtn) throw new AddinException(operands[0].Value+"参数解析出错！");
            rtn = Operand.ToNumber(operands[1].Value,out op2);
            if (!rtn) throw new AddinException(operands[1].Value+"参数解析出错！");

            return true;
        }

        public static bool GetBools(Operand[] operands, out bool op1, out bool op2)
        {
            if (operands == null) throw new AddinException("参数变量为空！");
            if (operands.Length < 2) throw new AddinException("参数个数出错！");
            op1 = false; op2 = false;

            bool rtn = Operand.ToBool(operands[0].Value,out op1);
            if (!rtn) throw new AddinException(operands[0].Value + "参数解析出错！");
            rtn = Operand.ToBool(operands[1].Value,out op2);
            if (!rtn) throw new AddinException(operands[1].Value + "参数解析出错！");

            return true;
        }

        public static bool GetBools(Operand[] operands, out bool op1)
        {
            if (operands == null) throw new AddinException("参数变量为空！");
            if (operands.Length < 1) throw new AddinException("参数个数出错！");
            op1 = false; 

            bool rtn = Operand.ToBool(operands[0].Value, out op1);
            if (!rtn) throw new AddinException(operands[0].Value + "参数解析出错！");

            return true;
        }



        public static bool GetDateTimes(Operand[] operands, out DateTime op1)
        {
            if (operands == null) throw new AddinException("参数变量为空！");
            if (operands.Length < 1) throw new AddinException("参数个数出错！");

            bool rtn = Operand.ToDateTime(operands[0].Value, out op1);

            if (!rtn) throw new AddinException(operands[0].Value + "参数解析出错！");

            return true;

        }
    }
}
