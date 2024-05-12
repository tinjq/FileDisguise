using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace FileDisguise
{
    public class FileContentDisguise
    {
        public const string DISGUISE = "@DISGUISED@";
        public static int disguiseLength = 4;
        public static byte[] disguiseByte = new byte[] { 0x40, 0x44, 0x49, 0x53, 0x47, 0x55, 0x49, 0x53, 0x45, 0x44, 0x40 };

        public static void FileDisguiseRecover(string path, int length)
        {
            bool disguiseBool = true;
            byte[] disguiseLengthBytes = new byte[disguiseLength];
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                if (fs.Length >= disguiseByte.Length + disguiseLength)
                {
                    fs.Seek(-disguiseLength, SeekOrigin.End);
                    fs.Read(disguiseLengthBytes, 0, disguiseLengthBytes.Length);

                    fs.Seek(-disguiseByte.Length - disguiseLength, SeekOrigin.End);
                    byte[] disguiseBuff = new byte[disguiseByte.Length];
                    fs.Read(disguiseBuff, 0, disguiseBuff.Length);
                    if (byteEqual(disguiseBuff, disguiseByte))
                    {
                        disguiseBool = false;
                        fs.SetLength(fs.Length - disguiseByte.Length - disguiseLength);
                    }
                }

                if (disguiseBool)
                {
                    // 伪装类型
                    byte[] buffer = new byte[length];
                    fs.Seek(0, SeekOrigin.Begin);
                    int rLength = fs.Read(buffer, 0, length);

                    EncodeByte(buffer, rLength);

                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Write(buffer, 0, rLength);

                    fs.Seek(0, SeekOrigin.End);
                    fs.Write(disguiseByte, 0, disguiseByte.Length);

                    byte[] lengthBytes = BitConverter.GetBytes(length);
                    fs.Write(lengthBytes, 0, lengthBytes.Length);
                }
                else
                {
                    length = BitConverter.ToInt32(disguiseLengthBytes, 0);
                    // 还原类型
                    byte[] buffer = new byte[length];
                    fs.Seek(0, SeekOrigin.Begin);
                    int rLength = fs.Read(buffer, 0, length);
                    DecodeByte(buffer, rLength);

                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Write(buffer, 0, rLength);
                }
            }
        }

        /// <summary>
        /// 伪装文件类型
        /// </summary>
        /// <param name="path"></param>
        /// <param name="length">伪装长度</param>
        public static void FileDisguise(string path, int length)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                byte[] buffer = new byte[length];
                //设置流当前位置为文件开始位置
                fs.Seek(0, SeekOrigin.Begin);
                int rLength = fs.Read(buffer, 0, length);

                EncodeByte(buffer, rLength);

                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(buffer, 0, rLength);

                fs.Seek(0, SeekOrigin.End);
                fs.Write(disguiseByte, 0, disguiseByte.Length);
            }
        }

        /// <summary>
        /// 还原文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="length"></param>
        public static void FileRecover(string path, int length)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                byte[] buffer = new byte[length];
                //设置流当前位置为文件开始位置
                fs.Seek(0, SeekOrigin.Begin);
                int rLength = fs.Read(buffer, 0, length);

                DecodeByte(buffer, rLength);

                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(buffer, 0, rLength);
            }
        }

        static void EncodeByte(byte[] arr, int length)
        {
            if (arr.Length >= length)
            {
                for (int i = 0; i < length; i++)
                {
                    arr[i]++;
                }
            }
        }

        static void DecodeByte(byte[] arr, int length)
        {
            if (arr.Length >= length)
            {
                for (int i = 0; i < length; i++)
                {
                    arr[i]--;
                }
            }
        }

        static bool byteEqual(byte[] b1, byte[] b2)
        {
            if (b1.Length != b2.Length) return false;

            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
