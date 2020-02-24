using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData3.Common.Utils
{
    public class LogHelper
    {

        public static void WriteLog(string msg)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Logs";
            
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string logPath = filePath + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

            try {
                using (StreamWriter sw = File.AppendText(logPath)) {
                    sw.WriteLine("消息:" + msg);
                    sw.WriteLine("时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    sw.WriteLine("********************************");
                    sw.WriteLine();
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (Exception e) {
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    sw.WriteLine("异常:" + msg);
                    sw.WriteLine("时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    sw.WriteLine("********************************");
                    sw.WriteLine();
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }
    }
}
