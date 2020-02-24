using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Model
{
    /// <summary>
    /// 银行返回信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BankReturnDetailModel<T>: object where T:class
    {

        /// <summary>
        /// 指令状态 
        /// </summary>
        public string resultCode { get; set; }
        /*
         * 工行状态 result
         
            0：提交成功,等待银行处理
            1：授权成功, 等待银行处理
            2：等待授权
            3：等待二次授权
            4：等待银行答复
            5：主机返回待处理
            6：被银行拒绝
            7：处理成功
            8：指令被拒绝授权
            9：银行正在处理
            10：预约指令
            11：预约取消
            86：等待电话核实
            95:待核查
            98:区域中心通讯可疑
        */

        /// <summary>
        /// 返回码
        /// </summary>
        public string iRetCode { get; set; }

        /// <summary>
        /// 返回描述
        /// </summary>
        public string iRetMsg { get; set; }

        /// <summary>
        /// 交易返回码
        /// </summary>
        public string instrRetCode { get; set; }

        /// <summary>
        /// 交易返回描述
        /// </summary>
        public string instrRetMsg { get; set; }

        /// <summary>
        /// 业务参考号 ref
        /// </summary>
        public string businessRefNo { get; set; }

        /// <summary>
        /// 银行反馈时间
        /// </summary>
        public string bankRetTime { get; set; }

        /// <summary>
        /// 序列化的对象
        /// </summary>
        public T infoData { get; set; }

    }
}
