using System;
using System.Collections.Generic;
using System.Linq;
using static WaterStation.Client.AIMonitor;

namespace WaterStation.Client
{
    /// <summary>
    /// Modbus 从站寄存器映射与 FC04 响应拼装（RTU/TCP 共用）
    /// </summary>
    public class ModbusSlaveRegisterService
    {
        private readonly object _syncRoot = new object();

        public int MaxStartAddress { get; } = Registers.Blank4;

        public Dictionary<ushort, byte[]> DefaultValue { get; } = new Dictionary<ushort, byte[]>()
        {
            { Registers.AI_TestTime, new byte[] { 0x00, 0x00, 0x00, 0x00 } },
            { Registers.AI_ZD, new byte[] { 0, 0, 0, 0 } },
            { Registers.AI_AD, new byte[] { 0, 0, 0, 0 } },
            { Registers.AI_COD, new byte[] { 0, 0, 0, 0 } },
            { Registers.AI_ZL, new byte[] { 0, 0, 0, 0 } },
            { Registers.AI_XS, new byte[] { 0, 0, 0, 0 } },
            { Registers.AI_Zhuodu, new byte[] { 0, 0, 0, 0 } },
            { Registers.AI_Blank3, new byte[] { 0, 0, 0, 0 } },
            { Registers.AI_Blank4, new byte[] { 0, 0, 0, 0 } },
            { Registers.AI_Blank5, new byte[] { 0, 0, 0, 0 } },
            { Registers.AI_Blank6, new byte[] { 0, 0, 0, 0 } },
            { Registers.AI_Blank7, new byte[] { 0, 0, 0, 0 } },
            { Registers.AI_Blank8, new byte[] { 0, 0, 0, 0 } },
            { Registers.AI_Blank9, new byte[] { 0, 0, 0, 0 } },
            { Registers.AI_Blank10, new byte[] { 0, 0, 0, 0 } },
            { Registers.AI_Blank11, new byte[] { 0, 0, 0, 0 } },
            { Registers.ZD_TestTime, new byte[] { 0x00, 0x00, 0x00, 0x00 } },
            { Registers.ZD, new byte[] { 0, 0, 0, 0 } },
            { Registers.AD_TestTime, new byte[] { 0x00, 0x00, 0x00, 0x00 } },
            { Registers.AD, new byte[] { 0, 0, 0, 0 } },
            { Registers.COD_TestTime, new byte[] { 0x00, 0x00, 0x00, 0x00 } },
            { Registers.COD, new byte[] { 0, 0, 0, 0 } },
            { Registers.ZL_TestTime, new byte[] { 0x00, 0x00, 0x00, 0x00 } },
            { Registers.ZL, new byte[] { 0, 0, 0, 0 } },
            { Registers.Blank1_TestTime, new byte[] { 0x00, 0x00, 0x00, 0x00 } },
            { Registers.Blank1, new byte[] { 0, 0, 0, 0 } },
            { Registers.Blank2_TestTime, new byte[] { 0x00, 0x00, 0x00, 0x00 } },
            { Registers.Blank2, new byte[] { 0, 0, 0, 0 } },
            { Registers.Blank3_TestTime, new byte[] { 0x00, 0x00, 0x00, 0x00 } },
            { Registers.Blank3, new byte[] { 0, 0, 0, 0 } },
            { Registers.Blank4_TestTime, new byte[] { 0x00, 0x00, 0x00, 0x00 } },
            { Registers.Blank4, new byte[] { 0, 0, 0, 0 } }
        };

        /// <summary>
        /// 校验 FC04 读输入寄存器请求，返回异常码（0 表示正常）
        /// </summary>
        public byte ValidateFc04Request(byte functionCode, ushort startAddress, ushort count)
        {
            if (functionCode != 0x04)
                return 1;
            if (startAddress > MaxStartAddress)
                return 2;
            if (count == 0)
                return 3;
            return 0;
        }

        /// <summary>
        /// 从 Global 刷新寄存器实时值
        /// </summary>
        public void RefreshFromGlobal(AIMonitor encoder)
        {
            lock (_syncRoot)
            {
#if DEBUG
                DefaultValue[Registers.AI_ZD] = encoder.FloatToHexMaxHead(4.965f);
                DefaultValue[Registers.AI_AD] = encoder.FloatToHexMaxHead(6.589f);
                DefaultValue[Registers.AI_COD] = encoder.FloatToHexMaxHead(8.595f);
                DefaultValue[Registers.AI_ZL] = encoder.FloatToHexMaxHead(0.256f);
                DefaultValue[Registers.AI_XS] = encoder.FloatToHexMaxHead(1.23f);
                DefaultValue[Registers.AI_Zhuodu] = encoder.FloatToHexMaxHead(2.45f);
                DefaultValue[Registers.ZD_TestTime] = encoder.TimeToTimespanHex(DateTime.Parse("2025-01-02 00:00:00"));
                DefaultValue[Registers.ZD] = encoder.FloatToHexMaxHead(23.695f);
                DefaultValue[Registers.AD] = encoder.FloatToHexMaxHead(125.692f);
                DefaultValue[Registers.COD_TestTime] = encoder.TimeToTimespanHex(DateTime.Parse("2025-01-02 01:00:00"));
                DefaultValue[Registers.COD] = encoder.FloatToHexMaxHead(635.2f);
                DefaultValue[Registers.ZL_TestTime] = encoder.TimeToTimespanHex(DateTime.Parse("2025-01-02 02:00:00"));
                DefaultValue[Registers.ZL] = encoder.FloatToHexMaxHead(5.789f);
#else
                DefaultValue[Registers.AI_TestTime] = encoder.TimeToTimespanHex(DateTime.Now);
                DefaultValue[Registers.AI_ZD] = encoder.FloatToHexMaxHead((float)Global.RecentValZD);
                DefaultValue[Registers.AI_AD] = encoder.FloatToHexMaxHead((float)Global.RecentValAD);
                DefaultValue[Registers.AI_COD] = encoder.FloatToHexMaxHead((float)Global.RecentValCOD);
                DefaultValue[Registers.AI_ZL] = encoder.FloatToHexMaxHead((float)Global.RecentValZL);
                DefaultValue[Registers.AI_XS] = encoder.FloatToHexMaxHead((float)Global.RecentValXS);
                DefaultValue[Registers.AI_Zhuodu] = encoder.FloatToHexMaxHead((float)Global.RecentValZhuodu);

                if (Global.EQNames.Contains("总氮"))
                {
                    if (DateTime.TryParse(Global.RecentValTimeZD, out DateTime zdTime))
                        DefaultValue[Registers.ZD_TestTime] = encoder.TimeToTimespanHex(zdTime);
                    DefaultValue[Registers.ZD] = encoder.FloatToHexMaxHead((float)Global.YBRecentValZD);
                }

                if (Global.EQNames.Contains("氨氮"))
                {
                    if (DateTime.TryParse(Global.RecentValTimeAD, out DateTime adTime))
                        DefaultValue[Registers.AD_TestTime] = encoder.TimeToTimespanHex(adTime);
                    DefaultValue[Registers.AD] = encoder.FloatToHexMaxHead((float)Global.YBRecentValAD);
                }

                if (Global.EQNames.Contains("COD"))
                {
                    if (DateTime.TryParse(Global.RecentValTimeCOD, out DateTime codTime))
                        DefaultValue[Registers.COD_TestTime] = encoder.TimeToTimespanHex(codTime);
                    DefaultValue[Registers.COD] = encoder.FloatToHexMaxHead((float)Global.YBRecentValCOD);
                }

                if (Global.EQNames.Contains("总磷"))
                {
                    if (DateTime.TryParse(Global.RecentValTimeZL, out DateTime zlTime))
                        DefaultValue[Registers.ZL_TestTime] = encoder.TimeToTimespanHex(zlTime);
                    DefaultValue[Registers.ZL] = encoder.FloatToHexMaxHead((float)Global.YBRecentValZL);
                }
#endif
            }
        }

        /// <summary>
        /// 构建 FC04 响应 PDU（不含 MBAP/CRC）
        /// </summary>
        public byte[] BuildFc04Response(byte slaveId, ushort startAddress, ushort count)
        {
            lock (_syncRoot)
            {
                var list = new List<byte> { slaveId, 0x04, (byte)(count * 2) };
                List<ushort> keys = DefaultValue.Keys.ToList();

                for (int i = 0; i < keys.Count; i++)
                {
                    if (keys[i] == startAddress)
                    {
                        AppendRegisterData(list, keys, i, count);
                        break;
                    }

                    if (keys[i] == startAddress - 1)
                    {
                        list.Add(DefaultValue[keys[i]][2]);
                        list.Add(DefaultValue[keys[i]][3]);
                        AppendRegisterDataFromOffset(list, keys, i + 1, (ushort)(count - 1));
                        break;
                    }
                }

                return list.ToArray();
            }
        }

        /// <summary>
        /// 构建 FC04 异常响应 PDU
        /// </summary>
        public static byte[] BuildFc04ExceptionResponse(byte slaveId, byte errorType)
        {
            return new byte[] { slaveId, 0x84, errorType };
        }

        private void AppendRegisterData(List<byte> list, List<ushort> keys, int startIndex, ushort count)
        {
            int readableCountMax = keys.Count * 2 - startIndex * 2;
            if (count <= readableCountMax)
            {
                if (count % 2 == 0)
                {
                    for (int j = startIndex; j < count / 2 + startIndex; j++)
                        list.AddRange(DefaultValue[keys[j]]);
                }
                else
                {
                    int j = startIndex;
                    for (; j < (count - 1) / 2 + startIndex; j++)
                        list.AddRange(DefaultValue[keys[j]]);
                    list.Add(DefaultValue[keys[j]][0]);
                    list.Add(DefaultValue[keys[j]][1]);
                }
            }
            else
            {
                for (int j = startIndex; j < keys.Count; j++)
                    list.AddRange(DefaultValue[keys[j]]);
                for (int j = 1; j <= count - (keys.Count - startIndex) * 2; j++)
                    list.AddRange(new byte[] { 0x00, 0x00 });
            }
        }

        private void AppendRegisterDataFromOffset(List<byte> list, List<ushort> keys, int startIndex, ushort count)
        {
            int readableCountMax = keys.Count * 2 - (startIndex - 1) * 2 - 2;
            if (count <= readableCountMax)
            {
                if (count % 2 == 0)
                {
                    for (int j = startIndex; j < count / 2 + startIndex; j++)
                        list.AddRange(DefaultValue[keys[j]]);
                }
                else
                {
                    int j = startIndex;
                    for (; j < (count - 1) / 2 + startIndex; j++)
                        list.AddRange(DefaultValue[keys[j]]);
                    list.Add(DefaultValue[keys[j]][0]);
                    list.Add(DefaultValue[keys[j]][1]);
                }
            }
            else
            {
                for (int j = startIndex; j < keys.Count; j++)
                    list.AddRange(DefaultValue[keys[j]]);
                for (int j = 1; j <= count - (keys.Count - startIndex) * 2; j++)
                    list.AddRange(new byte[] { 0x00, 0x00 });
            }
        }
    }
}
