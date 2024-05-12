using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDisguise
{
    public class FileNameDisguise
    {
        /// <summary>
        /// 递归获取目录下所有文件及子目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="includeDir">是否包含目录</param>
        /// <returns></returns>
        public static List<string> GetAllFiles(string path, bool includeDir)
        {
            List<string> files = new List<string>();

            // path 是目录
            if (Directory.Exists(path))
            {
                List<string> subFiles = Directory.GetFiles(path).ToList();
                files.AddRange(subFiles);

                List<string> subDirs = Directory.GetDirectories(path).ToList();
                foreach (var item in subDirs)
                {
                    files.AddRange(GetAllFiles(item, includeDir));
                }

                if (includeDir)
                {
                    files.Add(path);
                }
            }

            // path 是文件
            if (File.Exists(path))
            {
                files.Add(path);
            }

            return files;
        }

        public static void EncodeFileName(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            string newName = fileInfo.DirectoryName + Path.DirectorySeparatorChar + ToBase64Name(fileInfo.Name);
            fileInfo.MoveTo(newName);
        }

        public static void DecodeFileName(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            string newName = fileInfo.DirectoryName + Path.DirectorySeparatorChar + FromBase64Name(fileInfo.Name);
            fileInfo.MoveTo(newName);
        }

        static string ToHexString(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        static string FromHexString(string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];
            string tmp;
            for (int i = 0; i < bytes.Length; i++)
            {
                tmp = hexString.Substring(i * 2, 2);
                bytes[i] = Convert.ToByte(tmp, 16);
            }
            return Encoding.UTF8.GetString(bytes);
        }

        static string ToBase64Name(string str)
        {
            string base64String = ToBase64String(str);
            return base64String.Replace("/", "%");
        }

        static string ToBase64String(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(bytes);

        }

        static string FromBase64Name(string str)
        {
            return FromBase64String(str.Replace("%", "/"));
        }

        static string FromBase64String(string str)
        {
            byte[] decBytes = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(decBytes);
        }
    }
}
