using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WaterStation.Client.Controller
{
    public class GenerateMD5
    {
        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="inputStr"></param> 
        /// <returns>加密后字符串</returns>
        public static string CreateMD5(string inputStr)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(inputStr);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new ();
                for (int i = 0; i < newBuffer.Length; i++)
                    sb.Append(newBuffer[i].ToString("x2"));
                return sb.ToString();
            }
        }

        /// <summary>
        /// MD5流加密
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public static string CreateMD5(Stream inputStream)
        {
            using (MD5 mi = MD5.Create())
            {
                //开始加密
                byte[] newBuffer = mi.ComputeHash(inputStream);
                StringBuilder sb = new ();
                for (int i = 0; i < newBuffer.Length; i++)
                    sb.Append(newBuffer[i].ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
