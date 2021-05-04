using UnityEditor;
using UnityEngine;

namespace GameBasic
{
    public class ExportUnitypackage
    {
        [MenuItem("Window/General/ExprotUnitypackage")]
        private static void Exprot()
        {
            string path = EditorUtility.OpenFolderPanel("选择需要导出的文件夹", "Assets", "");
            string savePath = EditorUtility.SaveFilePanel("选择保存路径", "G:\\", "KB", "unitypackage");
            string[] pathSplit = path.Split('/');

            for (int i = 0, length = pathSplit.Length; i < length; i++)
            {
                if (pathSplit[i] == "Assets")
                {
                    path = pathSplit[i];

                    for (int j = i + 1; j < length; j++)
                        path += "\\" + pathSplit[j];

                    break;
                }
            }
            savePath.Replace('/', '\\');

            AssetDatabase.ExportPackage(path, savePath, ExportPackageOptions.Recurse);
        }
    }
}