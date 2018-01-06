using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Utilities
{
    /// <summary>
    /// Json返回值类
    /// </summary>
    public class JsonResponse
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 返回内容
        /// </summary>
        public string Data { get; set; }
    }
}
