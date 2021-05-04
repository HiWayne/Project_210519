using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace GameBasic.IO
{

    public class FileUtil
    {

        public static string[] ListSubdir(string path, bool fullname = false)
        {
            string[] result = null;

            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (dirInfo.Exists)
            {
                DirectoryInfo[] subDirs = dirInfo.GetDirectories();
                if (subDirs.Length > 0)
                {
                    result = new string[subDirs.Length];
                    for (int i = 0; i < subDirs.Length; i++)
                    {
                        result[i] = fullname ? subDirs[i].FullName : subDirs[i].Name;
                    }
                }
            }

            return result;
        }

        public static List<string> ListSub(string path, bool fullname = false)
        {

            List<string> result = null;

            FileSystemInfo[] sub = GetSub(path);

            if (sub != null && sub.Length > 0)
            {
                result = new List<string>(sub.Length);
                for (int i = 0; i < sub.Length; i++)
                {
                    string itemName = fullname ? sub[i].FullName : sub[i].Name;
                    result.Add(itemName);
                }
            }

            return result;
        }

        public static List<string> ListSubFile(string path, bool fullname = false, bool ignoreMeta = true)
        {

            List<string> result = null;

            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] sub = null;
            if (dirInfo.Exists)
            {
                sub = dirInfo.GetFiles();
            }

            if (sub != null && sub.Length > 0)
            {
                result = new List<string>(sub.Length);
                for (int i = 0; i < sub.Length; i++)
                {
                    string itemName = fullname ? sub[i].FullName : sub[i].Name;

                    if (ignoreMeta)
                        if (itemName.EndsWith(".meta"))
                            itemName = null;
                    if (itemName != null)
                        result.Add(itemName);
                }
            }

            return result;
        }

        public static FileSystemInfo[] GetSub(string path, string searchPattern = null)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (dirInfo.Exists)
            {
                return searchPattern == null ? dirInfo.GetFileSystemInfos() : dirInfo.GetFileSystemInfos(searchPattern);
            }
            return null;
        }
    }
}
