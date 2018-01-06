using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DotNet.Utilities
{
    /// <summary>
    /// MD5加密
    /// </summary>
    public static class MD5Encrypt
    {
        /// <summary>
        /// MD5散列
        /// </summary>
        /// <param name="inputStr">明文</param>
        /// <returns></returns>
        public static string MD5(string inputStr)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashByte = md5.ComputeHash(Encoding.UTF8.GetBytes(inputStr));
            StringBuilder sb = new StringBuilder();
            foreach (byte item in hashByte)
                sb.Append(item.ToString("x").PadLeft(2, '0'));
            return sb.ToString();
        }
        /// <summary>
        /// 创建密码MD5
        /// </summary>
        /// <param name="Pwd">原密码</param>
        /// <param name="Salt">盐值</param>
        /// <returns></returns>
        public static string CreatePasswordMd5(string Pwd, string Salt)
        {
            return MD5(string.Format("{0}{1}", Pwd, Salt));
        }

    }
}
