using Newtonsoft.Json;
using SUP.Common.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GData3.Common.Utils
{
    public class XMLUtil
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        private string filePath = "";
        /// <summary>
        /// 根节点
        /// </summary>
        private XElement root = null;
        /// <summary>
        /// 文件对象
        /// </summary>
        private XDocument document = null;

        private XMLUtil() { }

        public XMLUtil(string filePath) {
            this.filePath = filePath;
            //获取文件
            GetDocument(filePath);
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        private void GetDocument(string filePath) {
            document = XDocument.Load(filePath);
            if (document == null) {
                throw new Exception("文件加载失败！");
            }
            root = document.Root;
        }

        /// <summary>
        /// 返回根节点
        /// </summary>
        /// <returns>根节点</returns>
        public XElement GetRoot() {
            return this.root;
        }

        /// <summary>
        /// 获取匹配的节点
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns>返回匹配的第一个节点</returns>
        public XElement GetElement(string nodeName) {
            if (root == null)
                return null;
            //root.Name是XName的对象,要先转成string,否则判断结果一直为false
            if (root.Name.ToString() == nodeName)
                return root;

            XElement element = GetElement(root, nodeName);
            return element;
        }

        private XElement GetElement(XElement element, string nodeName) {
            if (element == null)
                return null;
            if (element.Name.ToString() == nodeName)
                return element;

            IEnumerable<XElement> elements = element.Elements();
            foreach (XElement ele in elements) {
                XElement result = GetElement(ele, nodeName);
                if (result != null)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// 获取匹配的节点的所有下级节点
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns>返回匹配的第一个节点的所有下级节点</returns>
        public IEnumerable<XElement> GetElements(string nodeName) {
            if (root == null)
                return null;
            if (root.Name.ToString() == nodeName)
                return root.Elements();

            IEnumerable<XElement> elements = GetElements(root, nodeName);
            return elements;
        }

        private IEnumerable<XElement> GetElements(XElement element, string nodeName) {
            if (element == null)
                return null;
            if (element.Name.ToString() == nodeName)
                return element.Elements();

            IEnumerable<XElement> elements = element.Elements();
            foreach (XElement ele in elements)
            {
                IEnumerable<XElement> result = GetElements(ele, nodeName);
                if (result != null)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// 获取节点的内容
        /// </summary>
        /// <param name="element">节点</param>
        /// <returns>返回节点的内容</returns>
        public Dictionary<string, string> GetElementValue(XElement element) {
            Dictionary<string, string> map = new Dictionary<string, string>();
            if (element == null)
                return map;

            var key = element.Name.ToString();
            var value = element.Value;
            map.Add(key, value);

            return map;
        }

        /// <summary>
        /// 获取节点的内容
        /// </summary>
        /// <param name="nodeName">节点名</param>
        /// <returns>返回节点的内容</returns>
        public Dictionary<string, string> GetElementValue(string nodeName) {
            Dictionary<string, string> map = new Dictionary<string, string>();
            XElement element = GetElement(root,nodeName);
            if (element == null)
                return map;

            string key = element.Name.ToString();
            string value = element.Value;
            map.Add(key, value);

            return map;
        }

        /// <summary>
        /// 获取节点的下级所有节点的内容
        /// </summary>
        /// <param name="element">节点</param>
        /// <returns>返回节点的内容</returns>
        public Dictionary<string, string> GetElementsValue(XElement element) {
            Dictionary<string, string> map = new Dictionary<string, string>();
            if (element == null)
                return map;

            IEnumerable<XElement> elements = element.Elements();
            
            foreach (XElement ele in elements) {
                string key = ele.Name.ToString();
                string value = ele.Value;
                map.Add(key, value);
            }

            return map;
        }

        /// <summary>
        /// 通过节点的集合获取所有下级节点的内容
        /// </summary>
        /// <param name="elements">节点的集合</param>
        /// <returns>所有下级节点的内容</returns>
        public Dictionary<object, object> GetElementsValue(IList<XElement> elements) {
            Dictionary<object, object> map = new Dictionary<object, object>();
            foreach (XElement element in elements)
            {
                Dictionary<string, string> pairs = GetElementsValue(element);
                map.Add(element, pairs);
            }
            return map;
        }

        /// <summary>
        /// 获取节点的下级所有节点的内容
        /// </summary>
        /// <param name="element">节点</param>
        /// <returns>返回节点的内容</returns>
        public Dictionary<string, string> GetElementsValue(string nodeName)
        {
            Dictionary<string, string> map = new Dictionary<string, string>();
            XElement element = GetElement(root,nodeName);
            if (element == null)
                return map;

            IEnumerable<XElement> elements = element.Elements();
            foreach (XElement ele in elements)
            {
                string key = ele.Name.ToString();
                string value = ele.Value;
                map.Add(key, value);
            }

            return map;
        }

        /// <summary>
        /// 通过节点名字的集合获取所有下级节点的内容
        /// </summary>
        /// <param name="nodeNames">节点名字的集合</param>
        /// <returns>所有下级节点的内容</returns>
        public Dictionary<string, object> GetElementsValue(IEnumerable<string> nodeNames) {
            Dictionary<string, object> map = new Dictionary<string, object>();
            foreach (string nodeName in nodeNames) {
                Dictionary<string, string> pairs = GetElementsValue(nodeName);
                map.Add(nodeName, pairs);
            }
            return map;
        }

        /// <summary>
        /// 各个节点的属性值
        /// </summary>
        /// <param name="nodeName">节点名</param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, string>> GetElementsManyValue(string nodeName)
        {
            Dictionary<string, Dictionary<string,string>> map = new Dictionary<string, Dictionary<string, string>>();
            XElement element = GetElement(root, nodeName);
            if (element == null)
                return map;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string show = "isshow";
            string remark = "remarks";
            IEnumerable<XElement> elements = element.Elements();
            foreach (XElement ele in elements)
            {
                dic.Clear();
                string key = ele.Name.ToString();                
                string value1 = ele.Attribute(show).Value.ToString();
                dic.Add(show, value1);
                string value2 = ele.Attribute(remark).Value.ToString();
                dic.Add(remark, value2);
                map.Add(key, dic);
            }

            return map;
        }

        /// <summary>
        /// 根据上级节点获取所有下级节点的属性值
        /// </summary>
        /// <param name="nodeNames">节点名</param>
        /// <returns></returns>
        public Dictionary<string, object> GetElementsManyValue(IEnumerable<string> nodeNames)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            foreach (string nodeName in nodeNames)
            {
                Dictionary<string, Dictionary<string, string>> pairs = GetElementsManyValue(nodeName);
                map.Add(nodeName, pairs);
            }
            return map;
        }

        /// <summary>
        /// 获取当前节点的属性值
        /// </summary>
        /// <param name="nodeName">节点名</param>
        /// <param name="arrtibutes">属性集合</param>
        /// <returns></returns>
        public Dictionary<string, string> GetElementAttributesValue(string nodeName,IEnumerable<string> arrtibutes) {
            Dictionary<string, string> attributesMap = new Dictionary<string, string>();

            XElement element = GetElement(root, nodeName);
            if (element == null)
                return attributesMap;

            if (arrtibutes.Count() == 0)
            {
                return attributesMap;
            }
            else {
                foreach (string node in arrtibutes) {
                    XAttribute attribute = element.Attribute(node);
                    if (attribute != null)
                    {
                        attributesMap.Add(node, attribute.Value);
                    }
                    else {
                        attributesMap.Add(node, "");
                    }
                }
            }
            return attributesMap;
        }

        /// <summary>
        /// 获取当前节点的属性值
        /// </summary>
        /// <param name="nodeName">节点</param>
        /// <param name="arrtibutes">属性集合</param>
        /// <returns></returns>
        public Dictionary<string, string> GetElementAttributesValue(XElement element, IEnumerable<string> arrtibutes)
        {
            Dictionary<string, string> attributesMap = new Dictionary<string, string>();

            if (element == null)
                return attributesMap;

            if (arrtibutes.Count() == 0)
            {
                return attributesMap;
            }
            else
            {
                foreach (string node in arrtibutes)
                {
                    XAttribute attribute = element.Attribute(node);
                    if (attribute != null)
                    {
                        attributesMap.Add(node, attribute.Value);
                    }
                    else {
                        attributesMap.Add(node, "");
                    }
                }
            }
            return attributesMap;
        }

        /// <summary>
        /// 获取当前节点的属性值
        /// </summary>
        /// <param name="nodeName">节点名的集合</param>
        /// <param name="arrtibutes">属性集合</param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, string>> GetElementsAttributesValue(IEnumerable<string> nodeNames, IEnumerable<string> arrtibutes) {
            Dictionary<string, Dictionary<string, string>> map = new Dictionary<string, Dictionary<string, string>>();
            foreach (string nodeName in nodeNames)
            {
                IEnumerable<XElement> elements = GetElements(nodeName);
                if (elements == null)
                    continue;

                foreach (XElement ele in elements)
                {
                    Dictionary<string, string> nodeMap = GetElementAttributesValue(ele, arrtibutes);
                    map.Add(ele.Name.ToString(), nodeMap);
                }
            }
            return map;
        }

        /// <summary>
        /// 获取当前节点的所有下级的属性值
        /// </summary>
        /// <param name="nodeName">节点名</param>
        /// <param name="arrtibutes">属性集合</param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, string>> GetElementsAttributesValue(string nodeName, IEnumerable<string> arrtibutes)
        {
            Dictionary<string, Dictionary<string, string>> map = new Dictionary<string, Dictionary<string, string>>();
            IEnumerable<XElement> elements = GetElements(nodeName);
            if (elements == null)
                return map;

            foreach (XElement ele in elements) {
                Dictionary<string, string> nodeMap = GetElementAttributesValue(ele, arrtibutes);
                map.Add(ele.Name.ToString(), nodeMap);
            }
            return map;
        }

        /// <summary>
        /// 修改文件节点的value,并保存
        /// </summary>
        /// <param name="map"></param>
        public void Save(Dictionary<string,string> map) {
            if (map != null && map.Count > 0) {
                foreach (string key in map.Keys) {
                    string value = map[key];
                    XElement element = GetElement(key);
                    element.SetValue(value);
                }
                if (!File.Exists(filePath)) {
                    throw new Exception("文件不存在！");
                }
                root.Save(filePath);
            }
        }

        public void Save(Dictionary<string, string> map, IEnumerable<string> arrtibutes)
        {
            if (arrtibutes.Count() == 0)
            {
                return ;
            }
            if (map != null && map.Count > 0)
            {
                foreach (string key in map.Keys)
                {
                    string value = map[key];
                    dynamic json = JsonConvert.DeserializeObject<dynamic>(value.ToString());
                    XElement element = GetElement(key);
                    foreach(string node in arrtibutes)
                    {
                        element.SetAttributeValue(node, json[node].Value);
                    }                  
                }
                root.Save(filePath);
            }
        }
    }
}
