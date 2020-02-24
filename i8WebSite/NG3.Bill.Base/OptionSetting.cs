using SUP.Frame.DataEntity;
using SUP.Frame.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Bill.Base
{
    public class OptionSetting
    {
        private OptionSettingRule rule=null;

        public OptionSetting()
        {
            rule = new OptionSettingRule();      
        }

        /// <summary>
        /// 根据分组获取选项列表
        /// </summary>
        /// <param name="option_group"></param>
        /// <param name="option_code"></param>
        /// <returns></returns>
        public List<OptionSettingEntity> GetOptionDetail(string option_group)
        {
            List<OptionSettingEntity> optionList = rule.GetOptionDetail(option_group);
            return optionList;
        }

        /// <summary>
        /// 根据分组、选项代码和单个组织key获取选项列表
        /// </summary>
        /// <param name="option_group"></param>
        /// <param name="option_code"></param>
        /// <returns></returns>
        public OptionSettingEntity GetOptionValue(string option_group, string option_code,string key)
        {
            OptionSettingEntity optionModel = rule.GetOptionValue(option_group,option_code,key);
            return optionModel;
        }

        /// <summary>
        /// 根据分组和选项代码获取选项列表
        /// </summary>
        /// <param name="option_group"></param>
        /// <param name="option_code"></param>
        /// <returns></returns>
        public string GetOptionDetail(string option_group,string option_code)
        {         
            string detailJson = rule.GetOptionDetail(option_group, option_code);
            return detailJson;
        }

        /// <summary>
        /// 根据选项代码获取选项列表
        /// </summary>
        /// <param name="option_group"></param>
        /// <param name="option_code"></param>
        /// <returns></returns>
        public OptionSettingEntity GetValueByCode(string option_code)
        {
            OptionSettingEntity optionModel = rule.GetValueByCode(option_code);
            return optionModel;
        }
        /// <summary>
        /// 根据分组和选项代码，组织列表获取组织和参数键值对
        /// </summary>
        /// <param name="option_group"></param>
        /// <param name="option_code"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetArgumentDic(string option_group, string option_code, string[] keys)
        {           
            Dictionary<string, string> arguDic = rule.GetArgumentDic(option_group, option_code, keys);
            return arguDic;
        }

        /// <summary>
        /// 根据分组、选项代码和单个组织id获取组织参数值
        /// </summary>
        /// <param name="option_group"></param>
        /// <param name="option_code"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetSingleArgument(string option_group,string option_code,string key)
        {
            string argValue = rule.GetSingleArgument(option_group, option_code, key);
            return argValue;
        }

        /// <summary>
        /// 根据分组、选项代码获取初始化选项设置的值
        /// </summary>
        /// <param name="option_group"></param>
        /// <param name="option_code"></param>
        /// <returns></returns>
        public string GetInitSettingValue(string option_group, string option_code)
        {
            string optionValue = rule.GetInitSettingValue(option_group, option_code);
            return optionValue;
        }


        /// <summary>
        /// 根据分组、选项代码和单个组织key获取选项值
        /// </summary>
        /// <param name="option_group"></param>
        /// <param name="option_code"></param>
        /// <returns></returns>
        public string GetValueByKey(string option_group, string option_code, string key)
        {
            string option_value = rule.GetValueByKey(option_group, option_code, key);
            return option_value;
        }


        /// <summary>
        /// 根据分组、选项代码和单个组织key获取选项值
        /// </summary>
        /// <param name="option_group"></param>
        /// <param name="option_code"></param>
        /// <returns></returns>
        public string GetValueByKey(string conn,string option_group, string option_code, string key)
        {
            string option_value = rule.GetValueByKey(conn,option_group, option_code, key);
            return option_value;
        }

        /// <summary>
        /// 根据分组、单个组织key获取选项值键值对
        /// </summary>
        /// <param name="option_group"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string,string> GetDicValueByKey(string option_group,string key)
        {
            return rule.GetDicValueByKey(option_group, key);
        }

        /// <summary>
        /// 根据分组、单个组织key获取选项值键值对
        /// </summary>
        /// <param name="option_group"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetValueByGroup(string option_group)
        {
            return rule.GetValueByGroup(option_group);
        }

    }
}
