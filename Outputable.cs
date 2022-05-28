namespace Mines2._0.Boundary
{
    /*
     * This interface declares the functions that can be called by the
     * Output managing classes that extend it.
     */
    public interface Outputable
    {
        void writeOutput(String message, bool newLine); // Write a string output to the console and pass if a newline needs to be printed. Most likely will be removed.
        void writeOutput(Char message, bool newLine); // Write a character output to the console and pass if a newline needs to be printed. Most likely will be removed.
        void writeToFile(String message); // Write some text to a file. Has yet to be implemented by an extending classes and will most likely be removed.
        void writeToTextBox(String message,TextBox textBox); // Append text into the game's textbox that handles all displayed information.
    }
}
