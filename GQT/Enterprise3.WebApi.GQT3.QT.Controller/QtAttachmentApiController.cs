using Enterprise3.Common.Base.Criterion;
using Enterprise3.Common.Model.Results;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.GQT3.QT.Model.Request;
using GData3.Common.Model;
using GQT3.QT.Model.Domain;
using GQT3.QT.Service.Interface;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;


namespace Enterprise3.WebApi.GQT3.QT.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter]
    public class QtAttachmentApiController: ApiBase
    {
        IQtAttachmentService QtAttachmentService { get; set; }


        /// <summary>
        /// 构造函数
        /// </summary>
        public QtAttachmentApiController()
        {
            QtAttachmentService = base.GetObject<IQtAttachmentService>("GQT3.QT.Service.QtAttachment");
        }

        /// <summary>
        /// 上传附件（资金拨付单据的每个项目多个附件）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostUploadFileBk()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    var data1 = new
                    {
                        Status = ResponseStatus.Error,
                        Msg = "请求数据不是multipart/form-data类型",
                        Data = ""
                    };
                    return DataConverterHelper.SerializeObject(data1);
                }
                //web api 获取项目根目录下指定的文件下
                var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/BkPayment/");
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string resourcePath = Path.Combine(root, date);
                if (!Directory.Exists(resourcePath))
                {
                    Directory.CreateDirectory(resourcePath);
                }
                var multipartMemoryStreamProvider = await Request.Content.ReadAsMultipartAsync();
                var contentsList = multipartMemoryStreamProvider.Contents;

                //获取表单数据
                Dictionary<string, string> dic = new Dictionary<string, string>();
                //List<FileParameter> paraList = new List<FileParameter>();

                List<string> files = new List<string>();
                //原始文件名
                List<string> oldnames = new List<string>();
                List<QtAttachmentModel> list = new List<QtAttachmentModel>();

                foreach (var content in contentsList)
                {
                    //通过判断fileName是否为空，判断是否为文件类型
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        string fileName = content.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                        using (Stream stream = await content.ReadAsStreamAsync())
                        {
                            //文件如果大于100MB  提示不允许
                            if (stream.Length > 104857600)
                            {
                                return DCHelper.ErrorMessage("上传的文件不能大于100MB！");
                            }
                            byte[] bytes = new byte[stream.Length];
                            stream.Read(bytes, 0, bytes.Length);
                            stream.Seek(0, SeekOrigin.Begin);

                            //获取对应文件后缀名
                            string extension = Path.GetExtension(fileName);
                            //获取文件名
                            string b_name = Path.GetFileName(fileName);

                            //修改文件名
                            string newFileName = Guid.NewGuid().ToString("N") + extension;
                            string uploadPath = Path.Combine(resourcePath, newFileName);

                            //保存文件
                            MemoryStream ms = new MemoryStream(bytes);
                            FileStream fs = new FileStream(uploadPath, FileMode.Create);
                            ms.WriteTo(fs);
                            ms.Close();
                            fs.Close();

                            string b_urlpath = "/UpLoadFiles/BkPayment/" + date + "/" + newFileName;

                            QtAttachmentModel model = new QtAttachmentModel();
                            model.BName = b_name;
                            model.BType = extension;
                            model.BSize = decimal.Round((decimal)stream.Length / (1024), 2); //保留2位小数
                            //model.BFilebody = bytes;
                            model.BUrlpath = b_urlpath;
                            model.BTable = "BK3_PAYMENT_XM";
                            model.BRemark = "";

                            list.Add(model);
                            //返回文件相对路径
                            files.Add(b_urlpath);
                            oldnames.Add(b_name);
                        }
                    }
                    else
                    {
                        string val = await content.ReadAsStringAsync();
                        dic.Add(content.Headers.ContentDisposition.Name.ToString(), val);
                    }
                }
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "附件上传成功。",
                    Data = list,
                    Attachment = files,
                    Oldnames = oldnames
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 上传附件（支付单据的每个单据上传一次）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostUploadFileGk()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    var data1 = new
                    {
                        Status = ResponseStatus.Error,
                        Msg = "请求数据不是multipart/form-data类型",
                        Data = ""
                    };
                    return DataConverterHelper.SerializeObject(data1);
                }
                //web api 获取项目根目录下指定的文件下
                var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/GkPayment/");
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string resourcePath = Path.Combine(root, date);
                if (!Directory.Exists(resourcePath))
                {
                    Directory.CreateDirectory(resourcePath);
                }
                var multipartMemoryStreamProvider = await Request.Content.ReadAsMultipartAsync();
                var contentsList = multipartMemoryStreamProvider.Contents;

                //获取表单数据
                Dictionary<string, string> dic = new Dictionary<string, string>();
                //List<FileParameter> paraList = new List<FileParameter>();

                List<string> files = new List<string>();
                //原始文件名
                List<string> oldnames = new List<string>();
                List<QtAttachmentModel> list = new List<QtAttachmentModel>();

                foreach (var content in contentsList)
                {
                    //通过判断fileName是否为空，判断是否为文件类型
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
                        string fileName = content.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                        using (Stream stream = await content.ReadAsStreamAsync())
                        {
                            //文件如果大于100MB  提示不允许
                            if (stream.Length > 104857600)
                            {
                                return DCHelper.ErrorMessage("上传的文件不能大于100MB！");
                            }
                            byte[] bytes = new byte[stream.Length];
                            stream.Read(bytes, 0, bytes.Length);
                            stream.Seek(0, SeekOrigin.Begin);

                            //获取对应文件后缀名
                            string extension = Path.GetExtension(fileName);
                            //获取文件名
                            string b_name = Path.GetFileName(fileName);

                            //修改文件名
                            string newFileName = Guid.NewGuid().ToString("N") + extension;
                            string uploadPath = Path.Combine(resourcePath, newFileName);

                            //保存文件
                            MemoryStream ms = new MemoryStream(bytes);
                            FileStream fs = new FileStream(uploadPath, FileMode.Create);
                            ms.WriteTo(fs);
                            ms.Close();
                            fs.Close();

                            string b_urlpath = "/UpLoadFiles/GkPayment/" + date + "/" + newFileName;

                            QtAttachmentModel model = new QtAttachmentModel();
                            model.BName = b_name;
                            model.BType = extension;
                            model.BSize = decimal.Round((decimal)stream.Length / (1024), 2); //保留2位小数
                            //model.BFilebody = bytes;
                            model.BUrlpath = b_urlpath;
                            model.BTable = "GK3_PAYMENT_MST";
                            model.BRemark = "";

                            list.Add(model);
                            //返回文件相对路径
                            files.Add(b_urlpath);
                            oldnames.Add(b_name);
                        }
                    }
                    else
                    {
                        string val = await content.ReadAsStringAsync();
                        dic.Add(content.Headers.ContentDisposition.Name.ToString(), val);
                    }
                }
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "附件上传成功。",
                    Data = list,
                    Attachment = files,
                    Oldnames = oldnames
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }


        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostDeleteFile([FromBody]FileApiModel parameter)
        {
            try
            {
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "附件删除成功。",
                    Data = ""
                };

                if (!string.IsNullOrEmpty(parameter.BUrlPath))
                {
                    var path = System.Web.Hosting.HostingEnvironment.MapPath("~" + parameter.BUrlPath);

                    //删除文件
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    else
                    {
                        data = new
                        {
                            Status = ResponseStatus.Error,
                            Msg = "附件不存在。",
                            Data = ""
                        };
                    }
                }
                else
                {
                    data = new
                    {
                        Status = ResponseStatus.Error,
                        Msg = "附件不存在。",
                        Data = ""
                    };
                }
                return DataConverterHelper.SerializeObject(data);

            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
    }
}