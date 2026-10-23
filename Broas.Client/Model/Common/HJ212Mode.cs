using System;

namespace WaterStation.Client.Model.Commom
{
    class HJ212Mode
    {
        #region 212协议组包
        /// <summary>
        /// 获取212协议报文
        /// </summary>
        /// <param name="MN=27">设备唯一标识</param>
        /// <param name="CN=2011">命令编号</param>
        /// <param name="CP">数据</param>
        /// <param name="ST=31">系统编号</param>
        /// <param name="PW">访问密码</param>
        /// <param name="Flag">是否分包及应答标识(0:不应答,不分包 3:应答且分包)</param>
        /// <param name="RevAllData">原报文</param>
        /// <returns></returns>
        public static string GetBackCmd(string MN, string CN, string CP, string ST = "31", string PW = "123456", string Flag = "0", string RevAllData = null)
        {
            string strHead = "##";
            string strlen = "";
            string strQN = "QN=" + DateTime.Now.ToString("yyyyMMddHHmmss000");
            string strST = "ST=" + ST;
            string strCN = "CN=" + CN;
            string strPW = "PW=" + PW;
            string strMN = "MN=" + MN;
            string strFlag = "Flag=" + Flag;
            string strCP = "CP=&&" + CP + "&&";
            //string strCRC = "";
            //从原报文中获取ST和PW
            if (RevAllData != null && RevAllData != "")
            {
                string str = "";
                str = GetParamValue(RevAllData, "ST=");
                if (str != null && str != null)
                {
                    strST = "ST=" + str;
                }
                str = GetParamValue(RevAllData, "PW=");
                if (str != null && str != null)
                {
                    strPW = "PW=" + str;
                }
            }
            //组装需要计算长度及校验的部分
            string strMode = "";
            strMode = strQN + ";" +
                    strST + ";" +
                    strCN + ";" +
                    strPW + ";" +
                    strMN + ";" +
                    strFlag + ";" +
                    strCP;
            strlen = strMode.Length.ToString("0000");
            strMode = strHead + strlen + strMode + Crc_16(strMode) + "\r\n";
            return strMode;
        }
        #endregion
        #region 获取数据报文中指定参数的值
        /**
	     * 获取数据报文中指定参数的值
	     * @param strData 完整字符串
	     * @param strParam 对应的字符串
	     * @return
	     */
        public static string GetParamValue(string strData, string strParam)
        {
            //string strDataLow = strData.ToLower();
            //##0147ST=32;CN=2011;PW=123456;MN=88888880000001;
            //CP=&&DataTime=20100623130500;010-Rtd=10.00,010-Flag=N;011-Rtd=10.00,011-Flag=N;015-Rtd=10.00,015-Flag=N&&D081
            string strDataLow = strData;
            int iPosBegin = strDataLow.IndexOf(strParam, 0);
            if (iPosBegin < 0) return null;
            int iPosC = strDataLow.IndexOf(",", iPosBegin);
            int iPosS = strDataLow.IndexOf(";", iPosBegin);
            int iPosA = strDataLow.IndexOf("&", iPosBegin);
            int iPosEnd = GetValuePos(iPosC, iPosS);
            iPosEnd = GetValuePos(iPosA, iPosEnd);
            int iPosE = strDataLow.IndexOf("=", iPosBegin);
            string strValue = null;
            try
            {
                //if (iPosS < 0)  //数据区只有一个数据
                //{
                //    strValue = strDataLow.Substring(iPosE + 1, iPosA - iPosE - 1);
                //    //System.out.println("SS"+strValue);
                //    return strValue;
                //}
                if (iPosA < 0)
                {
                    iPosA = strDataLow.Length;
                }
                if (iPosEnd == iPosA)
                {
                    iPosEnd = strDataLow.IndexOf("&", iPosA + 2);
                    strValue = strDataLow.Substring(iPosA, iPosEnd - iPosA);
                    return strValue.Replace("&&", "");
                }

                if (iPosEnd > iPosE + 1)
                {
                    strValue = strDataLow.Substring(iPosE + 1, iPosEnd - iPosE - 1);
                    return strValue.Replace(" ", "");
                }
                else return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("HJ212_Mode", ex.ToString());
                return null;
            }
        }
        #endregion
        #region 判断分隔符的正确位置
        /**
         * 判断分隔符的正确位置
         * @param iPos1
         * @param iPos2
         * @return
         */
        public static int GetValuePos(int iPos1, int iPos2)
        {
            if (iPos1 >= 0 && iPos2 >= 0)
                return iPos1 > iPos2 ? iPos2 : iPos1;
            else if (iPos1 < 0 && iPos2 < 0)
                return -1;
            else if (iPos1 >= 0 && iPos2 < 0)
                return iPos1;
            else if (iPos1 < 0 && iPos2 >= 0)
                return iPos2;
            else return -1;
        }
        #endregion
        #region 212协议校验
        /// <summary>
        /// 212协议校验
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public static string Crc_16(string strData)
        {
            byte crc16Lo, crc16Hi;
            byte saveLo, saveHi;
            //System.Text.Encoding chs = System.Text.Encoding.GetEncoding("gb2312");//chenzubo 20000801 该为utf-8
            System.Text.Encoding chs = System.Text.Encoding.UTF8;
            byte[] bits = chs.GetBytes(strData);
            crc16Hi = 0xFF;
            crc16Lo = 0x00;
            for (int i = 0; i < bits.Length; i++)
            {
                crc16Lo = crc16Hi;
                crc16Hi = 0;
                crc16Lo ^= bits[i];
                for (int g = 0; g < 8; g++)
                {
                    saveHi = crc16Hi;
                    saveLo = crc16Lo;
                    crc16Hi /= 2;
                    crc16Lo /= 2;
                    if (saveHi % 2 == 1)
                        crc16Lo |= 0x80;
                    if (saveLo % 2 == 1)
                    {
                        crc16Hi ^= 0xA0;
                        crc16Lo ^= 0x01;
                    }
                }
            }
            return crc16Hi.ToString("X2") + crc16Lo.ToString("X2");
        }
        #endregion
    }
}
