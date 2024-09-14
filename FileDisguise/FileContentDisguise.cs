using FileDisguise.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace FileDisguise
{
    public class FileContentDisguise
    {
        public static int disguiseLength = 4;
        // @DISGUISED@ 的 byte[]
        public static byte[] disguiseByte = new byte[] { 0x40, 0x44, 0x49, 0x53, 0x47, 0x55, 0x49, 0x53, 0x45, 0x44, 0x40 };

        public static void FileDisguiseRecover(string path, int length)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                if (fs.Length == 0)
                {
                    return;
                }
                if (fs.Length >= disguiseLength + disguiseByte.Length + 16 && HasBeanDisguised(fs))
                {
                    // 还原类型
                    // 读取加密长度
                    fs.Seek(-disguiseLength, SeekOrigin.End);
                    byte[] disguiseLengthBytes = new byte[disguiseLength];
                    fs.Read(disguiseLengthBytes, 0, disguiseLength);
                    length = BitConverter.ToInt32(disguiseLengthBytes, 0);

                    byte[] buffer = new byte[length + 16];
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Read(buffer, 0, length);

                    fs.Seek(-disguiseLength - disguiseByte.Length - 16, SeekOrigin.End);
                    fs.Read(buffer, length, 16);

                    byte[] plainBytes = AesUtil.Decrypt(buffer);

                    fs.Seek(0, SeekOrigin.Begin);
                    fs.SetLength(fs.Length - disguiseLength - disguiseByte.Length - 16);
                    fs.Write(plainBytes, 0, plainBytes.Length);
                }
                else
                {
                    // 伪装类型
                    if (fs.Length < length)
                    {
                        length = (int)fs.Length;
                        byte[] buffer = new byte[length];
                        fs.Seek(0, SeekOrigin.Begin);
                        fs.Read(buffer, 0, buffer.Length);

                        byte[] cypherBytes = AesUtil.Encrypt(buffer);

                        // 写入密文
                        fs.Seek(0, SeekOrigin.Begin);
                        fs.Write(cypherBytes, 0, cypherBytes.Length);

                        // 写入加密标识符
                        fs.Write(disguiseByte, 0, disguiseByte.Length);

                        // 写入加密长度
                        byte[] lengthBytes = BitConverter.GetBytes(cypherBytes.Length - 16);
                        fs.Write(lengthBytes, 0, lengthBytes.Length);
                    }
                    else
                    {
                        byte[] buffer = new byte[length];
                        fs.Seek(0, SeekOrigin.Begin);
                        fs.Read(buffer, 0, buffer.Length);

                        byte[] cypherBytes = AesUtil.Encrypt(buffer);

                        // 写入密文
                        fs.Seek(0, SeekOrigin.Begin);
                        fs.Write(cypherBytes, 0, cypherBytes.Length - 16);

                        fs.Seek(0, SeekOrigin.End);
                        fs.Write(cypherBytes, cypherBytes.Length - 16, 16);

                        // 写入加密标识符
                        fs.Write(disguiseByte, 0, disguiseByte.Length);

                        // 写入加密长度
                        byte[] lengthBytes = BitConverter.GetBytes(cypherBytes.Length - 16);
                        fs.Write(lengthBytes, 0, lengthBytes.Length);
                    }
                }
            }
        }

        static bool HasBeanDisguised(FileStream fs)
        {
            fs.Seek(-disguiseLength - disguiseByte.Length, SeekOrigin.End);
            byte[] disguiseBuff = new byte[disguiseByte.Length];
            fs.Read(disguiseBuff, 0, disguiseBuff.Length);
            return byteEqual(disguiseBuff, disguiseByte);
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
