using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace GData.YQHL.Common
{
    public class XML_EASY
    {
        private string xmlString;
        private int idx;

        public XML_EASY()
        {
            this.xmlString = null;
        }

        public XML_EASY(string str)
        {
            this.xmlString = str;
            this.idx = 0;
        }

        public string GetXML() => 
            this.xmlString;

        public string GetXMLNode(string node, int fromIdx = 0)
        {
            if ((this.xmlString != null) && (node != null))
            {
                string str = "<" + node + ">";
                int index = this.xmlString.IndexOf(str, fromIdx);
                if (index >= 0)
                {
                    string str2 = "</" + node + ">";
                    int num2 = this.xmlString.IndexOf(str2, (int) (index + str.Length));
                    if (num2 > index)
                    {
                        this.idx = num2 + str2.Length;
                        return this.xmlString.Substring(index + str.Length, (num2 - index) - str.Length);
                    }
                }
            }
            return null;
        }

        public int Index() => 
            this.idx;

        public void LoadXMLFile(string xmlFile)
        {
            StreamReader reader = new StreamReader(xmlFile, Encoding.GetEncoding("GBK"));
            this.xmlString = reader.ReadToEnd();
            reader.Close();
            reader = null;
            this.idx = 0;
        }

        public int LocateString(string str, int fromIdx = 0)
        {
            int index = this.xmlString.IndexOf(str, fromIdx);
            if (index >= 0)
            {
                this.idx = index;
            }
            return index;
        }

        
        public bool NextLeafNode(out string node, out string nodeContent)
        {
            int t;
            do
            {
                t = xmlString.IndexOf("</", idx);
                if (t >= idx)
                {
                    int a = xmlString.IndexOf(">", t + 2);
                    node = xmlString.Substring(t + 2, a - t - 2);
                    string head = "<" + node + ">";
                    int b = xmlString.LastIndexOf(head, t, t - idx + 1);
                    if (b >= idx)
                    {
                        nodeContent = xmlString.Substring(b + head.Length, t - b - head.Length);
                        idx = a + 1;
                        return true;
                    }
                    idx = a + 1;
                }
            }
            while (t >= 0);
            node = null;
            nodeContent = null;
            return false;
        }

        public bool NextXMLNode(out string node, out string nodeContent)
        {
            node = (string) (nodeContent = null);
            int index = this.xmlString.IndexOf('<', this.idx);
            if (index < 0)
            {
                return false;
            }
            int num2 = this.xmlString.IndexOf('>', index + 1);
            if (num2 < 0)
            {
                return false;
            }
            string str = this.xmlString.Substring(index + 1, (num2 - index) - 1);
            string str2 = "</" + str + ">";
            int num3 = this.xmlString.IndexOf(str2, (int) (num2 + 1));
            if (num3 < 0)
            {
                return false;
            }
            node = str;
            nodeContent = this.xmlString.Substring(num2 + 1, (num3 - num2) - 1);
            this.idx = num3 + str2.Length;
            return true;
        }

        public void RepXMLNode(string oldNode, string newNodeContent, int fromIdx = 0)
        {
            if ((oldNode != null) && (newNodeContent != null))
            {
                string str = "<" + oldNode + ">";
                int index = this.xmlString.IndexOf(str, fromIdx);
                if (index >= 0)
                {
                    string str2 = "</" + oldNode + ">";
                    int num2 = this.xmlString.IndexOf(str2, (int) (index + str.Length));
                    if (num2 > index)
                    {
                        string str3 = this.xmlString.Substring(0, index) + newNodeContent + this.xmlString.Substring(num2 + str2.Length);
                        this.xmlString = str3;
                        this.idx = index + newNodeContent.Length;
                    }
                }
            }
        }

        public void ResetIndex()
        {
            this.idx = 0;
        }

        public void SetXML(string str)
        {
            this.xmlString = str;
            this.idx = 0;
        }

        public void SetXMLNode(string node, string nodeContent, int fromIdx = 0)
        {
            string str = "<" + node + ">";
            int index = this.xmlString.IndexOf(str, fromIdx);
            if (index >= 0)
            {
                string str2 = "</" + node + ">";
                int startIndex = this.xmlString.IndexOf(str2, (int) (index + str.Length));
                if (startIndex > index)
                {
                    string str3 = this.xmlString.Substring(0, index + str.Length) + nodeContent + this.xmlString.Substring(startIndex);
                    this.xmlString = str3;
                    this.idx = ((index + str.Length) + nodeContent.Length) + str2.Length;
                }
            }
        }
    }
}

