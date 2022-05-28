namespace Mines2._0.Boundary
{
    /// <summary>
    /// This class is responsible for reading from text files.
    /// </summary>
    public class GameDataReader
    {
        private static GameDataReader instance;

        public static string directory;

        private GameDataReader() {
            directory = Path.Combine(Environment.CurrentDirectory, @"Entity\GameData");
        }

        public static GameDataReader getInstance()
        {
            if (instance == null)
                instance = new GameDataReader();
            return instance;
        }

        /// <summary>
        /// This method reads from a text file.
        /// </summary>
        /// <param name="fileName">the name of the file being read</param>
        /// <returns>the file's contents in an array</returns>
        public string[] readFile(string fileName)
        {
            if (!fileName.EndsWith(".txt"))
            {
                fileName += ".txt";
            }
            string textFile = Path.Combine(directory, fileName);
            if (File.Exists(textFile))
            {
                return File.ReadAllLines(textFile);
            }
            else
            {
                return null;
            }
        }
    }
}
