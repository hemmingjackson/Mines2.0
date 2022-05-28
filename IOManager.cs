namespace Mines2._0.Boundary
{
    // This Singleton class contains references to an object of both the Inputable and Outputable interfaces.
    // All functions in the reading and writing classes will be first called through here.
    public class IOManager
    {
        private static IOManager instance;
        private Inputable inputStream;
        private Outputable outputStream;
        private IOManager()
        {
            this.inputStream = new ConsoleReader();
            this.outputStream = new ConsoleWriter();
        }
        public static IOManager getInstance()
        {
            if (instance == null)
                instance = new IOManager();
            return instance;
        }
        
        public Inputable getInputStream() { return inputStream; }
        public Outputable getOutputStream() { return outputStream; }
    }
}
