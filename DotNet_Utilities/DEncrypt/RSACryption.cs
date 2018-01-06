using System;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.IO;
namespace DotNet.Utilities
{
    /// <summary>
    /// RSACryption,.netFramework中提供的RSA算法规定，每次加密的字节数不能超过密钥的长度值减去11，而每次加密得到的密文长度恰恰是密钥的长度，可采用分段加密
    /// </summary>
    public class RSACryption
    {

        private static Encoding Encod = Encoding.Unicode;

        /// <summary>
        /// RSA产生密钥
        /// </summary>
        /// <param name="xmlPrivateKeys">xml私钥</param>
        /// <param name="xmlPublicKey">xml公钥</param>
        public static void RSAKey(out string xmlPrivateKeys, out string xmlPublicKey)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                xmlPrivateKeys = rsa.ToXmlString(true);
                xmlPublicKey = rsa.ToXmlString(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 加密

        /// <summary>
        /// RSA的加密函数，KEY必须是XML的形式,返回的是字符串，该加密方式有长度限制的！
        /// </summary>
        /// <param name="xmlPublicKey">xml公钥</param>
        /// <param name="encryptString">待加密的字符串</param>
        /// <returns>密文（Base64）</returns>
        public static string RSAEncrypt(string xmlPublicKey, string encryptString)
        {
            return RSAEncrypt(xmlPublicKey, encryptString, Encod);
        }

        /// <summary>
        /// RSA的加密函数，KEY必须是XML的形式,返回的是字符串，该加密方式有长度限制的！
        /// </summary>
        /// <param name="xmlPublicKey">xml公钥</param>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="Encod">编码方式</param>
        /// <returns>密文（Base64）</returns>
        public static string RSAEncrypt(string xmlPublicKey, string encryptString, Encoding Encod)
        {
            try
            {
                byte[] PlainTextBArray;
                byte[] CypherTextBArray;
                string Result;
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPublicKey);
                PlainTextBArray = Encod.GetBytes(encryptString);
                CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
                Result = Convert.ToBase64String(CypherTextBArray);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 解密

        /// <summary>
        /// RSA的解密函数,KEY必须是XML的形式
        /// </summary>
        /// <param name="xmlPrivateKey">私钥</param>
        /// <param name="decryptString">待解密的字符串(Base64)</param>
        /// <returns></returns>
        public static string RSADecrypt(string xmlPrivateKey, string decryptString)
        {
            return RSADecrypt(xmlPrivateKey, decryptString, Encod);
        }

        /// <summary>
        /// RSA的解密函数,KEY必须是XML的形式
        /// </summary>
        /// <param name="xmlPrivateKey">私钥</param>
        /// <param name="decryptString">待解密的字符串(Base64)</param>
        /// <param name="Encod">编码方式</param>
        /// <returns></returns>
        public static string RSADecrypt(string xmlPrivateKey, string decryptString, Encoding Encod)
        {
            try
            {
                byte[] PlainTextBArray;
                byte[] DypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPrivateKey);
                PlainTextBArray = Convert.FromBase64String(decryptString);
                DypherTextBArray = rsa.Decrypt(PlainTextBArray, false);
                Result = Encod.GetString(DypherTextBArray);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 分段加密解密

        /// <summary>
        /// 分段加密，KEY必须是XML的形式
        /// </summary>
        /// <param name="xmlPublicKey">公钥</param>
        /// <param name="encryptString">待加密的字符串</param>
        /// <returns>密文（Base64）</returns>
        public static String RSABlockEncrypt(string xmlPublicKey, String encryptString)
        {
            return RSABlockEncrypt(xmlPublicKey, encryptString, Encod);
        }

        /// <summary>
        /// 分段加密，KEY必须是XML的形式
        /// </summary>
        /// <param name="xmlPublicKey">公钥</param>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="Encod">编码方式</param>
        /// <returns>密文（Base64）</returns>
        public static String RSABlockEncrypt(string xmlPublicKey, String encryptString, Encoding Encod)
        {
            try
            {
                RSACryptoServiceProvider RSACryptography = new RSACryptoServiceProvider();
                RSACryptography.FromXmlString(xmlPublicKey);
                byte[] PlaintextData = Encod.GetBytes(encryptString);
                int MaxBlockSize = RSACryptography.KeySize / 8 - 11;    //加密块最大长度限制
                if (PlaintextData.Length <= MaxBlockSize)
                    return Convert.ToBase64String(RSACryptography.Encrypt(PlaintextData, false));

                using (MemoryStream PlaiStream = new MemoryStream(PlaintextData))
                {
                    using (MemoryStream CrypStream = new MemoryStream())
                    {
                        Byte[] Buffer = new Byte[MaxBlockSize];
                        int BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);

                        while (BlockSize > 0)
                        {
                            Byte[] ToEncrypt = new Byte[BlockSize];
                            Array.Copy(Buffer, 0, ToEncrypt, 0, BlockSize);

                            Byte[] Cryptograph = RSACryptography.Encrypt(ToEncrypt, false);
                            CrypStream.Write(Cryptograph, 0, Cryptograph.Length);

                            BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);
                        }

                        return Convert.ToBase64String(CrypStream.ToArray(), Base64FormattingOptions.None);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// RSA的分段解密函数,KEY必须是XML的形式
        /// </summary>
        /// <param name="xmlPrivateKey">密钥</param>
        /// <param name="decryptString">待解密的字符串(Base64)</param>
        /// <returns></returns>
        public static String RSABlockDecrypt(string xmlPrivateKey, String decryptString)
        {
            return RSABlockDecrypt(xmlPrivateKey, decryptString, Encod);
        }

        /// <summary>
        /// RSA的分段解密函数,KEY必须是XML的形式
        /// </summary>
        /// <param name="xmlPrivateKey">密钥</param>
        /// <param name="decryptString">待解密的字符串(Base64)</param>
        /// <param name="Encod">编码方式</param>
        /// <returns></returns>
        public static String RSABlockDecrypt(string xmlPrivateKey, String decryptString, Encoding Encod)
        {
            RSACryptoServiceProvider RSACryptography = new RSACryptoServiceProvider();
            RSACryptography.FromXmlString(xmlPrivateKey);

            Byte[] CiphertextData = Convert.FromBase64String(decryptString);
            int MaxBlockSize = RSACryptography.KeySize / 8;    //解密块最大长度限制

            if (CiphertextData.Length <= MaxBlockSize)
                return Encod.GetString(RSACryptography.Decrypt(CiphertextData, false));

            using (MemoryStream CrypStream = new MemoryStream(CiphertextData))
            {
                using (MemoryStream PlaiStream = new MemoryStream())
                {
                    Byte[] Buffer = new Byte[MaxBlockSize];
                    int BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);

                    while (BlockSize > 0)
                    {
                        Byte[] ToDecrypt = new Byte[BlockSize];
                        Array.Copy(Buffer, 0, ToDecrypt, 0, BlockSize);

                        Byte[] Plaintext = RSACryptography.Decrypt(ToDecrypt, false);
                        PlaiStream.Write(Plaintext, 0, Plaintext.Length);

                        BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);
                    }

                    return Encod.GetString(PlaiStream.ToArray());
                }
            }
        }

        #endregion

        #region RSA数字签名

        #region 获取Hash描述表

        /// <summary>
        /// 获取Hash描述表
        /// </summary>
        /// <param name="strSource">待签名的字符串</param>
        /// <param name="HashData">Hash描述</param>
        /// <returns></returns>
        public static bool GetHash(string strSource, ref byte[] HashData)
        {
            try
            {
                byte[] Buffer;
                HashAlgorithm MD5 = HashAlgorithm.Create("MD5");
                Buffer = Encoding.GetEncoding("GB2312").GetBytes(strSource);
                HashData = MD5.ComputeHash(Buffer);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取Hash描述表
        /// </summary>
        /// <param name="strSource">待签名的字符串</param>
        /// <param name="strHashData">Hash描述</param>
        /// <returns></returns>
        public static bool GetHash(string strSource, ref string strHashData)
        {
            try
            {
                //从字符串中取得Hash描述 
                byte[] Buffer;
                byte[] HashData;
                HashAlgorithm MD5 = HashAlgorithm.Create("MD5");
                Buffer = Encoding.GetEncoding("GB2312").GetBytes(strSource);
                HashData = MD5.ComputeHash(Buffer);
                strHashData = Convert.ToBase64String(HashData);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取Hash描述表
        /// </summary>
        /// <param name="objFile">待签名的文件</param>
        /// <param name="HashData">Hash描述</param>
        /// <returns></returns>
        public static bool GetHash(System.IO.FileStream objFile, ref byte[] HashData)
        {
            try
            {
                //从文件中取得Hash描述 
                HashAlgorithm MD5 = HashAlgorithm.Create("MD5");
                HashData = MD5.ComputeHash(objFile);
                objFile.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取Hash描述表
        /// </summary>
        /// <param name="objFile">待签名的文件</param>
        /// <param name="strHashData">Hash描述</param>
        /// <returns></returns>
        public static bool GetHash(System.IO.FileStream objFile, ref string strHashData)
        {
            try
            {
                //从文件中取得Hash描述 
                byte[] HashData;
                System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
                HashData = MD5.ComputeHash(objFile);
                objFile.Close();
                strHashData = Convert.ToBase64String(HashData);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region RSA签名

        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="strKeyPrivate">私钥</param>
        /// <param name="HashbyteSignature">待签名Hash描述</param>
        /// <param name="EncryptedSignatureData">签名后的结果</param>
        /// <returns></returns>
        public static bool SignatureFormatter(string strKeyPrivate, byte[] HashbyteSignature, ref byte[] EncryptedSignatureData)
        {
            try
            {
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(strKeyPrivate);
                System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
                //设置签名的算法为MD5 
                RSAFormatter.SetHashAlgorithm("MD5");
                //执行签名 
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="strKeyPrivate">私钥</param>
        /// <param name="HashbyteSignature">待签名Hash描述</param>
        /// <param name="strEncryptedSignatureData">签名后的结果</param>
        /// <returns></returns>
        public static bool SignatureFormatter(string strKeyPrivate, byte[] HashbyteSignature, ref string strEncryptedSignatureData)
        {
            byte[] EncryptedSignatureData = Convert.FromBase64String(strEncryptedSignatureData);

            bool bol = SignatureFormatter(strKeyPrivate, HashbyteSignature, ref  EncryptedSignatureData);
            strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);
            if (bol)
                return true;
            else
                return false;
        }

        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="strKeyPrivate">私钥</param>
        /// <param name="strHashbyteSignature">待签名Hash描述</param>
        /// <param name="EncryptedSignatureData">签名后的结果</param>
        /// <returns></returns>
        public static bool SignatureFormatter(string strKeyPrivate, string strHashbyteSignature, ref byte[] EncryptedSignatureData)
        {
            try
            {
                byte[] HashbyteSignature;

                HashbyteSignature = Convert.FromBase64String(strHashbyteSignature);
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(strKeyPrivate);
                System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
                //设置签名的算法为MD5 
                RSAFormatter.SetHashAlgorithm("MD5");
                //执行签名 
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="strKeyPrivate">私钥</param>
        /// <param name="strHashbyteSignature">待签名Hash描述</param>
        /// <param name="strEncryptedSignatureData">签名后的结果</param>
        /// <returns></returns>
        public static bool SignatureFormatter(string strKeyPrivate, string strHashbyteSignature, ref string strEncryptedSignatureData)
        {
            try
            {
                byte[] HashbyteSignature;
                byte[] EncryptedSignatureData;
                HashbyteSignature = Convert.FromBase64String(strHashbyteSignature);
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();
                RSA.FromXmlString(strKeyPrivate);
                System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
                //设置签名的算法为MD5 
                RSAFormatter.SetHashAlgorithm("MD5");
                //执行签名 
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);
                strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region RSA 签名验证
        /// <summary>
        /// RSA签名验证
        /// </summary>
        /// <param name="strKeyPublic">公钥</param>
        /// <param name="HashbyteDeformatter">Hash描述</param>
        /// <param name="DeformatterData">签名后的结果</param>
        /// <returns></returns>
        public static bool SignatureDeformatter(string strKeyPublic, byte[] HashbyteDeformatter, byte[] DeformatterData)
        {
            try
            {
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();
                RSA.FromXmlString(strKeyPublic);
                System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
                //指定解密的时候HASH算法为MD5 
                RSADeformatter.SetHashAlgorithm("MD5");
                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// RSA签名验证
        /// </summary>
        /// <param name="strKeyPublic">公钥</param>
        /// <param name="strHashbyteDeformatter">Hash描述</param>
        /// <param name="DeformatterData">签名后的结果</param>
        /// <returns></returns>
        public static bool SignatureDeformatter(string strKeyPublic, string strHashbyteDeformatter, byte[] DeformatterData)
        {
            return SignatureDeformatter(strKeyPublic, Convert.FromBase64String(strHashbyteDeformatter), DeformatterData);
        }
        /// <summary>
        /// RSA签名验证
        /// </summary>
        /// <param name="strKeyPublic">公钥</param>
        /// <param name="HashbyteDeformatter">Hash描述</param>
        /// <param name="strDeformatterData">签名后的结果</param>
        /// <returns></returns>
        public static bool SignatureDeformatter(string strKeyPublic, byte[] HashbyteDeformatter, string strDeformatterData)
        {
            return SignatureDeformatter(strKeyPublic, HashbyteDeformatter, Convert.FromBase64String(strDeformatterData));
        }
        /// <summary>
        /// RSA签名验证
        /// </summary>
        /// <param name="strKeyPublic">公钥</param>
        /// <param name="strHashbyteDeformatter">Hash描述</param>
        /// <param name="strDeformatterData">签名后的结果</param>
        /// <returns></returns>
        public static bool SignatureDeformatter(string strKeyPublic, string strHashbyteDeformatter, string strDeformatterData)
        {
            return SignatureDeformatter(strKeyPublic, Convert.FromBase64String(strHashbyteDeformatter), Convert.FromBase64String(strDeformatterData));
        }
        #endregion

        #endregion

    }
}
