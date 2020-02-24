using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NG3.SDK
{
   internal class LogManager {
       private static string dir = @"C:\LOG\", suffix = ".txt", path;

       public static LogManager Instance {
           get {
               if (instance == null) {
                   instance = new LogManager();
                   instance.Init();
               }
               return instance;
           }
       }

       private static LogManager instance;

       private  System.Threading.Mutex mutx;
       private bool locked = false;
       private string mutxName;
       private void Init() {
           suffix =  "-webapi.txt";
           mutxName = "webapi";
           mutx = new System.Threading.Mutex(false, mutxName, out locked);
       }
       /// <summary>
       /// ׼��
       /// </summary>
       /// <returns></returns>
       private StreamWriter GetReady()
       {
           if (!Directory.Exists(dir)) {
               Directory.CreateDirectory(dir);
           }
           path = dir + DateTime.Now.ToString("yyyy-MM-dd") + suffix;
           if (!File.Exists(path)) {
               File.Create(path).Close();
           }
           FileInfo fi = new FileInfo(path);
           return fi.AppendText();
       }
       StreamWriter sw;

       /// <summary>
       /// �ļ�ʱ��������
       /// </summary>
       private string Limit {
           get {
               return DateTime.Now.ToShortDateString();
           }
       }

       /// <summary>
       /// ��־����ʱ��
       /// </summary>
       private string LogTime {
           get {
               return DateTime.Now.ToString()+"\r\n";
           }
       }

       /// <summary>
       /// д��־
       /// </summary>
       /// <param name="msg"></param>
       private void Write(string msg) {
           if (sw == null)
               sw = GetReady();
           if (path.LastIndexOf(Limit) < 0) {
               sw.Close();
               sw = GetReady();
           }
           if (msg != null && msg.Length > 0)
               msg = LogTime + msg;
           sw.WriteLine(msg);
           sw.Flush();
       }

       /// <summary>
       /// д��־,д���ر���
       /// </summary>
       /// <param name="msg">Ҫд�����Ϣ</param>
       /// <param name="close">�Ƿ������ر�</param>
       public void Write(string msg,bool close) {

           try
           {
               //mutx = new System.Threading.Mutex(false, "Logw");
               if (mutx.WaitOne())
               {
                   Write(msg);
                   if (close)
                   {
                       Close();
                   }

                   mutx.ReleaseMutex();
               }
           }
           catch
           {
               mutx.Close();
           }
           finally {
               mutx.Close();
               mutx = new System.Threading.Mutex(false, mutxName, out locked);
           }
       }

       /// <summary>
       /// �ر���
       /// </summary>
       private void Close()
       {
           if (sw != null)
               sw.Close();
           sw = null;
       }
    }
}
