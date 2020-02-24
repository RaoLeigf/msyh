using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Common.DataEntity.Individual
{
    public class NGText : ExtControlInfoBase
    {
        private int _maxLength = 100;
               

        public NGText()
        {
 
        }
              

        public int maxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }

    }

    public class NGTextArea : ExtControlInfoBase
    {
        private int _maxLength = 100;
        
        public NGTextArea()
        {
 
        }

        public int maxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }
    }

    public class NGDate : ExtControlInfoBase
    {

        public NGDate()
        { 
        }
              
    }

    public class NGDateTime : ExtControlInfoBase
    {
        public NGDateTime()
        { 
        }
              

    }

    
    public class NGNumber : ExtControlInfoBase
    {
        private decimal _maxValue = Int32.MaxValue; //最大值
        //private int _maxLength = 10; //最大长度
        private int _decimalPrecision = 0; //小数点位数
        private string _decimalSeparator = "."; //小数点符号
        private bool _showPercent = false;
        private double _step = 1;

        public NGNumber()
        {

        }

        public NGNumber(string fieldtype,int length, int declen,bool showPercent)
        {
            _showPercent = showPercent;
            if (showPercent)
            {
                step = 0.01;
            }
            switch (fieldtype)
            {
                case "03":
                    //temp = "integer";
                    _maxValue = Int32.MaxValue;
                    //_maxLength = 10;
                    break;
                case "04":
                    //temp = "smallint";
                    _maxValue = Int16.MaxValue;
                    //_maxLength = 5;
                    break;
                case "05":
                    //temp = "tinyint";
                    _maxValue = Byte.MaxValue;
                    //_maxLength = 5;
                    break;
                case "06":
                    //temp = "numeric";
                    _decimalPrecision = declen;
                    //_maxLength = length;
                    break;
                case "10":
                    //temp = "bigint";
                    _maxValue = Int32.MaxValue;//前端只能支持32位整形
                    //_maxLength = 10;
                    break;
                default:
                    break;
            }

            //根据长度和精度控制最大值，否则oracle报错
            int integerLen = length - declen;//整数位
            string s = ".";
            string maxString = "0";
            if (declen < length)
            {
                maxString = s.PadLeft(integerLen, '9').PadRight(declen, '9');
            }
            else if (declen == length)
            {//等于
                maxString = s.PadLeft(1, '0').PadRight(declen, '9');
            }

            _maxValue = Convert.ToDecimal(maxString);
        }


        //public int maxLength
        //{
        //    get { return _maxLength; }
        //    set { _maxLength = value; }
        //}

        public decimal maxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }

        public int decimalPrecision
        {
            get { return _decimalPrecision; }
            set { _decimalPrecision = value; }
        }
      

        public string decimalSeparator
        {
            get { return _decimalSeparator; }
            set { _decimalSeparator = value; }
        }

        public bool showPercent
        {
            get
            {
                return _showPercent;
            }

            set
            {
                _showPercent = value;
            }
        }

        public double step
        {
            get
            {
                return _step;
            }

            set
            {
                _step = value;
            }
        }
    }

    public class NGInt : ExtControlInfoBase
    {
        private int _maxValue = Int32.MaxValue; //最大值       
        private int _decimalPrecision = 0; //小数点位数        


        public NGInt()
        {

        }

        public NGInt(string fieldtype)
        {

            switch (fieldtype)
            {
                case "03":
                    //temp = "integer";
                    _maxValue = Int32.MaxValue;                    
                    break;
                case "04":
                    //temp = "smallint";
                    _maxValue = Int16.MaxValue;                   
                    break;
                case "05":
                    //temp = "tinyint";
                    _maxValue = Byte.MaxValue;                    
                    break;              
                case "10":
                    //temp = "bigint";
                    _maxValue = Int32.MaxValue;//前端只能支持32位整形                    
                    break;
                default:
                    break;
            }
        }


       
        public int maxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }

        public int decimalPrecision
        {
            get { return _decimalPrecision; }
            set { _decimalPrecision = value; }
        }

    }


    public class NGComboBox : NGRichHelp
    {
             
        private string _queryMode;

        public NGComboBox()
        {
        }

        public IList<KeyValueEntity> data
        {
            get;
            set;
        }
        
        public string queryMode
        {
            get { return _queryMode; }
            set { _queryMode = value; }
        }
        
    }

    public class NGCommonHelp : NGHelp
    {

    }
    

    public class NGHelp : ExtControlInfoBase
    {

        private string _valueField;
        private string _displayField;
        private string _helpid;
        private bool _matchFieldWidth = true;

        public NGHelp()
        {
        }

        public string valueField
        {
            get { return _valueField; }
            set { _valueField = value; }
        }

        public string displayField
        {
            get { return _displayField; }
            set { _displayField = value; }
        }

        public string helpid
        {
            get { return _helpid; }
            set { _helpid = value; }
        }

        public bool matchFieldWidth
        {
            get { return _matchFieldWidth; }
            set { _matchFieldWidth = value; }
        }

        public string usercodeField
        {
            get;
            set;
        }

        //客户端sql条件
        public string clientSqlFilter
        {
            get;
            set;
        }
        

    }

    public class NGRadio : ExtControlInfoBase
    {
        private string _inputValue;

        public string inputValue
        {
            get { return _inputValue; }
            set { _inputValue = value; }
        }

        public NGRadio()
        { 
        }

    }

    public class NGCheckbox : ExtControlInfoBase
    {
        private string _inputValue = "1";

        public string inputValue
        {
            get { return _inputValue; }
            set { _inputValue = value; }
        }

        public NGCheckbox()
        { }

      
    }

    public class NGRichHelp : NGHelp
    {

        private bool ormMode = true;
        private string _listFields;

        private string _listHeadTexts;

        private bool _showAutoHeader;

        public NGRichHelp()
        {
        }
          
        public bool ORMMode
        {
            get { return ormMode; }
            set { ormMode = value; }
        }
        public string listFields
        {
            get { return _listFields; }
            set { _listFields = value; }
        }

        public string listHeadTexts
        {
            get { return _listHeadTexts; }
            set { _listHeadTexts = value; }
        }

        public bool showAutoHeader
        {
            get { return _showAutoHeader; }
            set { _showAutoHeader = value; }
        }
    }

    public class NGCustomFormHelp : NGRichHelp
    {

    }

    public class NGButton : ExtControlInfoBase
    {        
        private string text;

        public NGButton()
        {
 
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
                
    }
    
}
