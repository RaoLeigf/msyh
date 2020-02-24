using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace WebApiSite.Controllers
{
    public class DemoController : ApiController
    {
        // POST api/values
        [HttpPost]
        public async Task<string> PostUploadFile(int id = 0)
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {

                    return "error";
                }

                //web api 获取项目根目录下指定的文件下
                var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/SysOrganize/");
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string resourcePath = Path.Combine(root, date);
                if (!Directory.Exists(resourcePath))
                {
                    Directory.CreateDirectory(resourcePath);
                }

                //对上传文件重新命名 根据需求对应修改文件名称 不包含后缀名 只是前缀名
                var provider = new RenamingMultipartFormDataStreamProvider(resourcePath);

                // 接收数据，并保存文件
                var bodyparts = await Request.Content.ReadAsMultipartAsync(provider);

                //if (bodyparts.FormData.Count <= 0 || provider.FileData == null && !provider.FileData.Any())
                //{

                //    return "error";
                //}
                List<string> files = new List<string>();
                foreach (MultipartFileData file in provider.FileData)
                {
                    if (file != null)
                    {
                        //上传的文件名
                        string name = file.Headers.ContentDisposition.FileName.Replace("\"", "");
                        if (name != "")
                        {
                            //获取对应文件后缀名
                            string extension = Path.GetExtension(name);

                            //修改文件名
                            string newFileName = Guid.NewGuid().ToString("N") + extension;
                            string uploadPath = Path.Combine(resourcePath, newFileName);
                            //保存文件
                            File.Move(file.LocalFileName, uploadPath);


                            files.Add("/UpLoadFiles/SysOrganize/" + date + "/" + newFileName);
                        }
                    }
                }

                return "success";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public HttpResponseMessage GetExportFile(string filePath, string fileName)
        {
            HttpResponseMessage httpResponseMessage = null;
            DirectoryInfo directoryInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/DownLoadFiles/" + filePath));
            FileInfo foundFileInfo = directoryInfo.GetFiles().Where(x => x.Name == fileName).FirstOrDefault();

            if (foundFileInfo != null)
            {
                var path = foundFileInfo.FullName;
                httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                FileStream fileStream = File.OpenRead(path);
                httpResponseMessage.Content = new StreamContent(fileStream);
                httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                //httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                //{
                //    FileName = HttpUtility.UrlEncode(Path.GetFileName(path))
                //};
                //fileStream.Close();
                httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                httpResponseMessage.Content.Headers.ContentDisposition.FileName = foundFileInfo.Name;

            }
            else
            {
                httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            return httpResponseMessage;
        }
    }

    public class RenamingMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        /// <summary>
        /// 文件重命名
        /// </summary>
        /// <param name="root"></param>
        public RenamingMultipartFormDataStreamProvider(string root) : base(root)
        {

        }
        /// <summary>
        /// 文件重命名
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            string fileName = headers.ContentDisposition.Name;
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                fileName = Guid.NewGuid().ToString();
            }
            return fileName.Replace("\"", string.Empty);
        }
    }
}