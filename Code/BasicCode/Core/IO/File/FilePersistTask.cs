using System;
using System.IO;

namespace GameBasic.IO
{
    public class FilePersistTask : PoolingTask
    {
        public enum Type { Read, Write, Delete }

        public Type type;
        public string path;
        public string data;

        public bool appendWrite;

        // callback
        public Action<FilePersistTask> onStart;
        public Action<FilePersistTask> onComplete;

        public FilePersistTask() { }

        public FilePersistTask(Type type, string path = null, string data = null)
        {
            this.type = type;
            this.path = path;
            this.data = data;
        }

        override protected void ExecuteImpl()
        {
            onStart?.Invoke(this);

            if (type == Type.Read)
                Read();
            else if (type == Type.Write)
                Write();
            else if (type == Type.Delete)
                Delete();

            onComplete?.Invoke(this);
        }

        void Read()
        {
            if (!File.Exists(path))
                return;

            StreamReader stream = null;
            try
            {
                stream = new StreamReader(path);
                data = stream.ReadToEnd();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
        }

        void Write()
        {
            StreamWriter stream = null;
            FileInfo fileinfo = new FileInfo(path);

            try
            {
                // create dir
                DirectoryInfo dir = fileinfo.Directory;
                if (!fileinfo.Directory.Exists)
                    dir.Create();

                stream = new StreamWriter(path, appendWrite);
                stream.Write(data);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
        }

        void Delete()
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                else if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);

                    /*
                    DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(path));
                    //返回目录中所有文件和子目录
                    FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();
                    foreach (FileSystemInfo i in fileinfo)
                    {
                        //判断是否文件夹
                        if (i is DirectoryInfo)
                        {
                            DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                            //删除子目录和文件
                            subdir.Delete(true);
                        }
                        else
                        {
                            //删除指定文件
                            File.Delete(i.FullName);
                        }
                    }
                    */
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static FilePersistTask Write(Action<FilePersistTask> onStart = null, Action<FilePersistTask> onComplete = null, bool async = true)
        {
            return Write(null, null, onStart, onComplete, async);
        }

        public static FilePersistTask Write(string path, string data, Action<FilePersistTask> onStart = null, Action<FilePersistTask> onComplete = null, bool async = true)
        {
            FilePersistTask task = new FilePersistTask(Type.Write, path, data);
            if (onStart != null)
                task.onStart += onStart;
            if (onComplete != null)
                task.onComplete += onComplete;

            Run(task, async);

            return task;
        }

        public static FilePersistTask Read(string path, Action<string> onComplete = null, bool async = true)
        {
            FilePersistTask task = new FilePersistTask(Type.Read, path);
            if (onComplete != null)
                task.onComplete += t => onComplete(t.data);

            Run(task, async);

            return task;
        }

        public static FilePersistTask Read(string path, Action<FilePersistTask> onStart = null, Action<FilePersistTask> onComplete = null, bool async = true)
        {
            FilePersistTask task = new FilePersistTask(Type.Read, path);
            if (onStart != null)
                task.onStart += onStart;
            if (onComplete != null)
                task.onComplete += onComplete;

            Run(task, async);

            return task;
        }

        static void Run(FilePersistTask task, bool async)
        {
            if (async)
                task.Submit();
            else
                task.Execute();
        }
    }
}