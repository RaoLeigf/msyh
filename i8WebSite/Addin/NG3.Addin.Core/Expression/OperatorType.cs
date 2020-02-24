using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Expression
{
    /// <summary>
    /// 数字表示优化级
    /// </summary>
    public enum OperatorType
    {
        LEFT =-1, //(
        RIGHT = -1, //)
        FUNC = -2, //表示的是函数
        MUL = -3, //*
        DIV = -3, // /
        ADD= -4,// +
        SUB= -4, //-

        LT = -5, // <
        LE = -5, // <=
        GT = -5, //>
        GE =-5, // >=
        ET = -6, // ==
        UT = -6, // <>
        AND= -7, // &&
        OR = -8, // ||
        COMMA = -9,
        ERR = -100

    }
}
