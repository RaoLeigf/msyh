using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.SUP.LoginValidation.Interface
{
    public interface IFilterBase<TReturn, TParam>
    {
        TReturn Filter(TParam param);
    }

    public abstract class FilterBase<TReturn, TParam> : IFilterBase<TReturn, TParam>
    {
        public abstract TReturn Filter(TParam param);
    }
}
