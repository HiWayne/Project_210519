using UnityEngine;

namespace GameBasic.IO
{
    public class GamePersist
    {
        // Path
        public static string PATH_ROOT = Application.persistentDataPath;
        public static string PATH_SAVE = Application.persistentDataPath + "/Save";

        public static string GetSavePath(string filename)
        {
            return PATH_SAVE + "/" + filename;
        }

        public static GamePersist instance;

        public static void Init()
        {
            if (instance == null)
                instance = new GamePersist();

            //instance.Save("Test.txt", JsonUtility.ToJson(new LevelData()));
        }

        //Queue<object> saveQueue = new Queue<object>();

        GamePersist()
        {

        }

        #region Utility

        /// <summary>
        /// Delete all saved game data
        /// </summary>
        public FilePersistTask DeleteGameData()
        {
            FilePersistTask task = new FilePersistTask(FilePersistTask.Type.Delete, PATH_SAVE);
            return task;
        }

        public FilePersistTask DeleteSave(string filename)
        {
            return Delete(GetSavePath(filename));
        }

        public FilePersistTask Delete(string path)
        {
            FilePersistTask task = new FilePersistTask(FilePersistTask.Type.Delete, path);
            return task;
        }

        public FilePersistTask Save(string filename, string text)
        {
            return Write(GetSavePath(filename), text);
        }

        public FilePersistTask Write(string path, string text)
        {
            FilePersistTask task = new FilePersistTask(FilePersistTask.Type.Write, path, text);
            return task;
        }

        public FilePersistTask ReadSave(string filename)
        {
            return Read(GetSavePath(filename));
        }

        public FilePersistTask Read(string path)
        {
            FilePersistTask task = new FilePersistTask(FilePersistTask.Type.Read, path);
            return task;
        }
        #endregion
    }
}