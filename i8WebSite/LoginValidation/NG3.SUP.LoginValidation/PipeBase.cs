using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.SUP.LoginValidation.Interface;

namespace NG3.SUP.LoginValidation
{
    public interface IPipeBase<TFilter, TParam>
    {
        void AddFilter(TFilter filter);

        void RemoveFilter(TFilter filter);

        void RemoveFilterAt(int index);

        void InsertFilter(int index, TFilter filter);

        FilterResult Execute(TParam param);
    }

    public abstract class PipeBase<TFilter, TParam> : IPipeBase<TFilter, TParam>
    {
        IList<TFilter> _filterList = new List<TFilter>();

        #region IPipeBase<TFilter> Members

        public void AddFilter(TFilter filter)
        {
            _filterList.Add(filter);
        }

        public void RemoveFilter(TFilter filter)
        {
            _filterList.Remove(filter);
        }

        public void RemoveFilterAt(int index)
        {
            _filterList.RemoveAt(index);
        }

        public void InsertFilter(int index, TFilter filter)
        {
            _filterList.Insert(index, filter);
        }

        public abstract FilterResult Execute(TParam param);

        #endregion
    }
}
