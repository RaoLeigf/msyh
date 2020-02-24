//ICBC_NetSafeClient
using GData.YQHL.Common;
using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Web;

namespace GData.YQHL.Service.ICNBCNC
{
    public class ICBC_NetSafeClient
    {
        private string httpsIP;

        private string signIP;

        private int httpPort;

        private int signPort;

        private int httpTimeOut;

        private int signTimeOut;

        private StreamWriter logFile;

        private bool logSwitch;

        private string logDir;

        private string xmlVersion;

        private string GroupID;

        private string CertID;

        private string YMDTime;

        private string logFN;

        private Random RanSeed;

        private TimeSpan Local_Bank;

        private string LastBankDate;

        private string ThirtySix;

        private int N36P2;

        private int N36P3;

        private string HttpHeadString;

        private string myInfo = "ICBC_NetSafeClient.dll";

        private string inputXML;

        private string outputXML;

        private string BankReturnEncoding;

        public string GetHttpHeadString()
        {
            return HttpHeadString;
        }

        public ICBC_NetSafeClient()
        {
            httpsIP = (signIP = "127.0.0.1");
            httpPort = 448;
            signPort = 449;
            httpTimeOut = 30;
            signTimeOut = 10;
            logSwitch = false;
            logFile = null;
            xmlVersion = "0.0.0.1";
            logDir = "D:\\";
            GroupID = null;
            CertID = null;
            YMDTime = "yyyymmdd";
            LastBankDate = null;
            Local_Bank = TimeSpan.Zero;
            RanSeed = new Random();
            ThirtySix = "";
            for (int j = 0; j < 10; j++)
            {
                ThirtySix += Convert.ToChar(48 + j).ToString();
            }
            for (int i = 0; i < 26; i++)
            {
                ThirtySix += Convert.ToChar(65 + i).ToString();
            }
            N36P2 = 1296;
            N36P3 = 46656;
        }

        public string GetHttpsIP()
        {
            return httpsIP;
        }

        public void SetHttpsIP(string ip)
        {
            httpsIP = ip;
        }

        public string GetSignIP()
        {
            return signIP;
        }

        public void SetSignIP(string ip)
        {
            signIP = ip;
        }

        public int GetHttpsPort()
        {
            return httpPort;
        }

        public void SetHttpsPort(int port)
        {
            httpPort = port;
        }

        public int GetSignPort()
        {
            return signPort;
        }

        public void SetSignPort(int port)
        {
            signPort = port;
        }

        public int GetHttpsTimeOut()
        {
            return httpTimeOut;
        }

        public void SetHttpsTimeOut(int time)
        {
            httpTimeOut = time;
            if (httpTimeOut <= 0 || httpTimeOut > 60)
            {
                httpTimeOut = 30;
            }
        }

        public int GetSignTimeOut()
        {
            return signTimeOut;
        }

        public void SetSignTimeOut(int time)
        {
            signTimeOut = time;
            if (signTimeOut <= 0 || signTimeOut > 60)
            {
                signTimeOut = 10;
            }
        }

        public string GetInterfaceVersion()
        {
            return xmlVersion;
        }

        public void SetInterfaceVersion(string ver)
        {
            xmlVersion = ver.Trim();
        }

        public void SetLogSwitch(bool log)
        {
            logSwitch = log;
        }

        public void SetLogFilePath(string logPath)
        {
            logDir = logPath;
            if (logPath[logPath.Length - 1] != '\\')
            {
                logDir += "\\";
            }
        }

        public string GetLogFileName()
        {
            return logFN;
        }

        public void SetGroupID(string gid)
        {
            GroupID = gid.Trim();
        }

        public string GetGroupID()
        {
            return GroupID;
        }

        public void SetCertID(string cid)
        {
            CertID = cid.Trim();
        }

        public string GetCertID()
        {
            return CertID;
        }

        public void SetYMDTime(string ymdt)
        {
            YMDTime = ymdt;
        }

        public void Run(string xmlString, out string seqNo, out string result)
        {
            outputXML = (result = null);
            inputXML = xmlString;
            XML_EASY reqXML = new XML_EASY(xmlString);
            bool needSign = reqXML.GetXMLNode("SignTime") != null;
            string sysDateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string TransCode = reqXML.GetXMLNode("TransCode");
            if (TransCode == null)
            {
                TransCode = "";
            }
            seqNo = reqXML.GetXMLNode("fSeqno");
            bool createSeq = false;
            if (seqNo == null || seqNo.Trim().Length <= 3)
            {
                seqNo = GetSeqno(sysDateTime + TransCode);
                createSeq = true;
                reqXML.SetXMLNode("fSeqno", seqNo);
            }
            logFN = null;
            if (logSwitch)
            {
                string LFN = createSeq ? seqNo : GetSeqno(sysDateTime + TransCode);
                //logFN = logDir + LFN.Substring(0, 6) + "-" + LFN.Substring(6, 4) + "-" + LFN.Substring(10, 4) + "@" + TransCode + ".log";
                logFN = logDir + LFN + "@" + TransCode + ".log";
                if (logFile != null)
                {
                    logFile.Close();
                }
                logFile = new StreamWriter(logFN, false, Encoding.GetEncoding("GBK"));
                //logFile.WriteLine(myInfo);
                logFile.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss fff]"));
                logFile.WriteLine();
            }
            if (GetGroupID() != null)
            {
                reqXML.SetXMLNode("CIS", GetGroupID());
            }
            if (GetCertID() != null)
            {
                reqXML.SetXMLNode("ID", GetCertID());
            }
            string url2;
            if (LastBankDate == null && needSign)
            {
                if (logFile != null)
                {
                    logFile.WriteLine("本交易需签名，未获取过银行服务器时间。检测银行主机时间（Detect Time Info From Bank）。");
                }
                url2 = "http://" + GetHttpsIP() + ":" + GetHttpsPort().ToString() + "/servlet/ICBCCMPAPIReqServlet?userID=" + GetCertID() + "&PackageID=" + sysDateTime + "&SendTime=" + sysDateTime;
                ncPost(url2, sysDateTime, GetHttpsTimeOut(), sign: false);
                if (logFile != null)
                {
                    logFile.WriteLine();
                }
            }
            string usingDateTime;
            if (YMDTime.ToLower() == "yyyymmdd")
            {
                if (Local_Bank.TotalMinutes >= 3.0 || Local_Bank.TotalMinutes <= -3.0)
                {
                    usingDateTime = (DateTime.Now - Local_Bank).ToString("yyyyMMddHHmmssfff");
                    if (logFile != null)
                    {
                        logFile.WriteLine("时间误差较大，XML报文日期时间调整到（Follow Bank's DateTime）:{0}", usingDateTime);
                        logFile.WriteLine();
                    }
                }
                else
                {
                    usingDateTime = sysDateTime;
                }
            }
            else
            {
                usingDateTime = YMDTime + sysDateTime.Substring(YMDTime.Length);
            }
            if (logFile != null)
            {
                logFile.WriteLine("xml原始报文信息（raw xml）：");
                logFile.WriteLine(xmlString);
                logFile.WriteLine();
                logFile.WriteLine();
            }
            reqXML.SetXMLNode("TranDate", usingDateTime.Substring(0, 8));
            reqXML.SetXMLNode("TranTime", usingDateTime.Substring(8));
            reqXML.SetXMLNode("SignTime", usingDateTime);
            if (reqXML.LocateString("%fSeqno%") > 0)
            {
                string tmpReq = reqXML.GetXML();
                reqXML.SetXML(tmpReq.Replace("%fSeqno%", seqNo));
            }
            string zipped = reqXML.GetXMLNode("zip");
            bool needZip = zipped != null;
            if (logFile != null)
            {
                logFile.WriteLine("补充信息后的xml（decorated xml）：");
                logFile.WriteLine(reqXML.GetXML());
                logFile.WriteLine();
                logFile.WriteLine();
            }
            if (needZip)
            {
                string afterZip = ZipBase64enc(zipped);
                reqXML.SetXMLNode("zip", afterZip);
                if (logFile != null)
                {
                    logFile.WriteLine("存在需要压缩的信息（zip string）。");
                    logFile.WriteLine();
                    logFile.WriteLine("压缩后的信息（after zipped）：");
                    logFile.WriteLine(afterZip);
                    logFile.WriteLine();
                    logFile.WriteLine();
                }
            }
            string reqData2 = "";
            string ncReturn2 = "U";
            if (needSign)
            {
                if (logFile != null)
                {
                    logFile.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss fff]"));
                    logFile.WriteLine("签名-发送地址（sign server address）：");
                    logFile.WriteLine("http://" + GetSignIP() + ":" + GetSignPort().ToString());
                    logFile.WriteLine();
                    logFile.WriteLine();
                    logFile.WriteLine("签名-发送内容（sign server content）：");
                    logFile.WriteLine(reqXML.GetXML());
                    logFile.WriteLine();
                    logFile.WriteLine();
                }
                XML_EASY signStr = new XML_EASY(ncPost("http://" + GetSignIP() + ":" + GetSignPort().ToString(), reqXML.GetXML(), GetSignTimeOut(), sign: true));
                if (logFile != null)
                {
                    logFile.WriteLine();
                }
                reqData2 = signStr.GetXMLNode("sign");
                if (reqData2 == null)
                {
                    ncReturn2 = "error<error>银行签名交易失败!请检查银行签名服务是否正确!</error>";
                    if (logFile != null)
                    {
                        logFile.WriteLine(ncReturn2);
                    }
                }
                else if (logFile != null)
                {
                    logFile.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss fff]"));
                    logFile.WriteLine("签名-结果（sign server return）：");
                    logFile.WriteLine(signStr.GetXML());
                    logFile.WriteLine();
                    logFile.WriteLine();
                }
            }
            else
            {
                reqData2 = HttpUtility.UrlEncode(reqXML.GetXML(), Encoding.GetEncoding("GBK"));
            }
            if (reqData2 == null || reqData2.Length == 0)
            {
                outputXML = (result = ncReturn2);
                if (logFile != null)
                {
                    logFile.Close();
                    logFile = null;
                }
                return;
            }
            url2 = "http://" + GetHttpsIP() + ":" + GetHttpsPort().ToString() + "/servlet/ICBCCMPAPIReqServlet?userID=" + GetCertID() + "&PackageID=" + seqNo + "&SendTime=" + usingDateTime;
            if (needZip)
            {
                url2 += "&zipFlag=1";
            }
            string cont = "Version=" + GetInterfaceVersion() + "&TransCode=" + TransCode + "&BankCode=102&GroupCIS=" + GetGroupID() + "&ID=" + GetCertID() + "&PackageID=" + seqNo + "&Cert=&reqData=" + reqData2;
            if (logFile != null)
            {
                logFile.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss fff]"));
                logFile.WriteLine("安全HTTP服务-发送地址（https server address）：");
                logFile.WriteLine(url2);
                logFile.WriteLine();
                logFile.WriteLine();
                logFile.WriteLine("安全HTTP服务-发送内容（https server content）：");
                logFile.WriteLine(cont);
                logFile.WriteLine();
                logFile.WriteLine();
            }
            ncReturn2 = ncPost(url2, cont, GetHttpsTimeOut(), sign: false);
            if (logFile != null)
            {
                logFile.WriteLine();
            }
            if (ncReturn2 == null || ncReturn2.Length == 0)
            {
                outputXML = (result = "error<error>未接受到银行返回信息！https server failed!</error>");
                if (logFile != null)
                {
                    logFile.WriteLine(result);
                    logFile.Close();
                    logFile = null;
                }
                return;
            }
            if (logFile != null)
            {
                logFile.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss fff]"));
                logFile.WriteLine("银行返回结果（bank return mesage）：");
                logFile.WriteLine(ncReturn2);
                logFile.WriteLine();
                logFile.WriteLine();
            }
            BankReturnEncoding = "GBK";
            string headInfo = HttpHeadString.ToUpper();
            int cs = headInfo.IndexOf("CHARSET");
            if (cs > 0)
            {
                int p3 = headInfo.IndexOf('=', cs);
                char[] endCharSet = new char[2]
                {
                ';',
                '<'
                };
                int p2 = headInfo.IndexOfAny(endCharSet, p3);
                BankReturnEncoding = headInfo.Substring(p3 + 1, p2 - p3 - 1).Trim();
            }
            byte[] bytes = Convert.FromBase64String(ncReturn2.Substring(ncReturn2.IndexOf('=') + 1));
            if (ncReturn2.IndexOf("errorCode") >= 0)
            {
                result = "<error>errorCode=" + Encoding.GetEncoding(BankReturnEncoding).GetString(bytes) + "</error>";
            }
            else
            {
                result = Encoding.GetEncoding(BankReturnEncoding).GetString(bytes);
            }
            if (logFile != null)
            {
                logFile.WriteLine("BASE64解码（base64dec）{0}：", BankReturnEncoding);
                logFile.WriteLine(result);
                logFile.WriteLine();
                logFile.WriteLine();
            }
            XML_EASY resultXML = new XML_EASY(result);
            string resultZip = resultXML.GetXMLNode("zip");
            if (resultZip != null)
            {
                resultXML.SetXMLNode("zip", Base64decUnzip(resultZip));
                result = resultXML.GetXML();
                if (logFile != null)
                {
                    logFile.WriteLine("解压缩（after zipped）：");
                    logFile.WriteLine(result);
                    logFile.WriteLine();
                    logFile.WriteLine();
                }
            }
            if (logFile != null)
            {
                logFile.Close();
                logFile = null;
            }
            outputXML = result;
        }

        public bool TryNextTag(out string reqID, out string result)
        {
            reqID = null;
            result = null;
            XML_EASY xML_EASY = new XML_EASY(outputXML);
            string lastNextTag = xML_EASY.GetXMLNode("NextTag");
            if (xML_EASY.GetXMLNode("RetCode") != "0" || lastNextTag == null || lastNextTag.Trim().Length == 0)
            {
                return false;
            }
            XML_EASY readLastInput = new XML_EASY(inputXML);
            readLastInput.SetXMLNode("NextTag", lastNextTag);
            Run(readLastInput.GetXML(), out reqID, out result);
            return true;
        }

        private string GetSeqno(string TimeStr)
        {
            string seq6 = TimeStr.Substring(2, 10);
            int ssfff4 = Convert.ToInt32(TimeStr.Substring(12, 5)) * 27 + RanSeed.Next(27);
            int x3 = ssfff4 / N36P3;
            seq6 += ThirtySix[x3].ToString();
            ssfff4 -= x3 * N36P3;
            x3 = ssfff4 / N36P2;
            seq6 += ThirtySix[x3].ToString();
            ssfff4 -= x3 * N36P2;
            x3 = ssfff4 / 36;
            seq6 += ThirtySix[x3].ToString();
            ssfff4 -= x3 * 36;
            seq6 += ThirtySix[ssfff4].ToString();
            seq6 += "@";
            for (int i = 17; i < TimeStr.Length; i += 3)
            {
                seq6 += TimeStr[i].ToString();
                if (seq6.Length >= 20)
                {
                    break;
                }
            }
            return seq6;
        }

        public string GetSignStr(string str)
        {
            return ncPost("http://" + GetSignIP() + ":" + GetSignPort().ToString(), str, GetSignTimeOut(), sign: true);
        }

        private string ncPost(string url, string postCont, int timeOut, bool sign)
        {
            byte[] bytesToPost = Encoding.GetEncoding("GBK").GetBytes(postCont);
            string cookieheader2 = string.Empty;
            CookieContainer cookieCon = new CookieContainer();
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.CookieContainer = cookieCon;
            httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0;)";
            httpRequest.Method = "POST";
            httpRequest.Timeout = timeOut * 1000;
            httpRequest.ServicePoint.Expect100Continue = false;
            if (sign)
            {
                httpRequest.ContentLength = bytesToPost.Length;
                httpRequest.ContentType = "INFOSEC_SIGN/1.0";
            }
            else
            {
                httpRequest.ContentType = "application/x-www-form-urlencoded;charset=GBK";
            }
            if (cookieheader2.Equals(string.Empty))
            {
                cookieheader2 = httpRequest.CookieContainer.GetCookieHeader(new Uri(url));
            }
            else
            {
                httpRequest.CookieContainer.SetCookies(new Uri(url), cookieheader2);
            }
            string stringResponse = "";
            try
            {
                DateTime SendDT = DateTime.Now;
                httpRequest.ContentLength = bytesToPost.Length;
                Stream requestStream = httpRequest.GetRequestStream();
                requestStream.Write(bytesToPost, 0, bytesToPost.Length);
                requestStream.Close();
                WebResponse wr = httpRequest.GetResponse();
                WebHeaderCollection whc = wr.Headers;
                string[] allKeys = whc.AllKeys;
                string BankDateTime = null;
                HttpHeadString = "";
                string[] array = allKeys;
                foreach (string info in array)
                {
                    if (logFile != null)
                    {
                        logFile.WriteLine("{0}:{1}", info, whc.Get(info));
                    }
                    if (info == "Date")
                    {
                        BankDateTime = (LastBankDate = whc.Get(info));
                    }
                    HttpHeadString = HttpHeadString + "<WebHead:" + info + ">" + whc.Get(info) + "</WebHead:" + info + ">";
                }
                if (BankDateTime != null)
                {
                    string pattern = "";
                    if (BankDateTime.IndexOf("+0") != -1)
                    {
                        BankDateTime = BankDateTime.Replace("GMT", "");
                        pattern = "ddd, dd MMM yyyy HH':'mm':'ss zzz";
                    }
                    if (BankDateTime.ToUpper().IndexOf("GMT") != -1)
                    {
                        pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
                    }
                    if (pattern != "")
                    {
                        DateTime bankDT = DateTime.ParseExact(BankDateTime, pattern, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal).ToLocalTime();
                        Local_Bank = SendDT - bankDT;
                        HttpHeadString = HttpHeadString + "<WebHead:LocalDate>" + bankDT.ToString("yyyy-MM-dd HH:mm:ss") + "</WebHead:LocalDate>";
                        if (logFile != null)
                        {
                            logFile.WriteLine("Convert Date Info To Local:{0}", bankDT.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                    }
                }
                Stream responseStream = wr.GetResponseStream();
                using (StreamReader responseReader = new StreamReader(responseStream, Encoding.GetEncoding("GBK")))
                {
                    stringResponse = responseReader.ReadToEnd();
                }
                responseStream.Close();
                return stringResponse;
            }
            catch (Exception)
            {
                return stringResponse;
            }
        }

        public static string ZipBase64enc(string str)
        {
            MemoryStream memoryStream = new MemoryStream();
            GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress);
            byte[] bs = Encoding.GetEncoding("GBK").GetBytes(str);
            gZipStream.Write(bs, 0, bs.Length);
            gZipStream.Close();
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static string Base64decUnzip(string str)
        {
            GZipStream zips = new GZipStream(new MemoryStream(Convert.FromBase64String(str)), CompressionMode.Decompress);
            byte[] block = new byte[1024];
            int bytesRead2 = 0;
            MemoryStream outBuffer = new MemoryStream();
            while (true)
            {
                bytesRead2 = zips.Read(block, 0, block.Length);
                if (bytesRead2 <= 0)
                {
                    break;
                }
                outBuffer.Write(block, 0, bytesRead2);
            }
            zips.Close();
            return Encoding.GetEncoding("GBK").GetString(outBuffer.ToArray());
        }
    }
}
