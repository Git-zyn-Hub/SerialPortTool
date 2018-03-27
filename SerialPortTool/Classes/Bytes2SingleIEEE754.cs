using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortTool.Classes
{
    public class Bytes2SingleIEEE754
    {
        /// <summary>
        /// 根据IEEE754将4个字节，转换为单精度浮点数
        /// </summary>
        /// <param name="data">要转换的字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns></returns>
        public static float Bytes2Single(byte[] datas, int startIndex)
        {
            if (datas.Length < startIndex + 4)
            {
                throw new Exception("起始索引超出数组界限。");
            }
            int data1 = datas[startIndex];
            int data2 = datas[startIndex + 1];
            int data3 = datas[startIndex + 2];
            int data4 = datas[startIndex + 3];

            int data = data1 << 24 | data2 << 16 | data3 << 8 | data4;
            int nSign;
            if ((data & 0x80000000) > 0)
            {
                nSign = -1;
            }
            else
            {
                nSign = 1;
            }
            int nExp = data & (0x7F800000);
            nExp = nExp >> 23;
            float nMantissa = data & (0x7FFFFF);

            if (nMantissa != 0)
                nMantissa = 1 + nMantissa / 8388608;

            float value = nSign * nMantissa * (2 << (nExp - 128));
            return value;
        }
    }
}
