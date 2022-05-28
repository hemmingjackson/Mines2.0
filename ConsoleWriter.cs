namespace Mines2._0.Boundary
{
    /*
     * This class extends the Outputable interface and implements the functions for writing output.
     */
    public class ConsoleWriter : Outputable
    {
        public ConsoleWriter() {
        }
        // Write string output to the console and determine if a newline needs to be printed.
        public void writeOutput(String message, bool newLine)
        {
            if (newLine)
                Console.WriteLine(message);
            else
                Console.Write(message);
        }
        // Write a character output to the console and determine if a newline needs to be printed.
        public void writeOutput(Char message, bool newLine)
        {
            if (newLine)
                Console.WriteLine(message);
            else
                Console.Write(message);
        }
        public void writeToFile(String message) { } // Write text to a file. Will most likely be removed since it is unlikely to be implemented.
        public void writeToFile(Char message) { } // Write text to a file. Will most likely be removed since it is unlikely to be implemented.
        // Write the passed string message to the game's displaying textbox.
        public void writeToTextBox(String message, TextBox textBox) 
        {
            textBox.AppendText(message);
            textBox.AppendText(Environment.NewLine);
        }

    }
}
