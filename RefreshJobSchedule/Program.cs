using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace RefreshJobSchedule
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            //如果传递了参数 start 就启动服务
            if (args.Length > 0)
            {               
                var rs = args[0];
                string strServiceName = "银行状态刷新服务";
                switch (rs)
                {
                    case "start":
                        ServiceBase[] ServicesToRun;
                        ServicesToRun = new ServiceBase[] { new WinRefreshDataService() };
                        ServiceBase.Run(ServicesToRun);
                        break;
                    case "install":
                        //取当前可执行文件路径，加上"start"参数，证明是从windows服务启动该程序
                        var path = Process.GetCurrentProcess().MainModule.FileName + " start";
                        var cmd = "create " + strServiceName + " binpath= \"" + path + "\" displayName= " + strServiceName + " start= auto";
                        Process.Start("sc", cmd);
                        //Console.WriteLine("安装成功");
                        break;
                    case "delete":
                        Process.Start("sc", "delete " + strServiceName + "");
                        //Console.WriteLine("卸载成功");
                        break;
                }
            }          

        }
    }
}
