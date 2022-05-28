namespace Mines2._0.Boundary
{
    /*
     * This interface declares the functions that can be called by the
     * Input managing classes that extend it.
     */
    public interface Inputable
    {
        String readLineInput(); // Read a string line from the console. Most likely will be removed now that Winforms is implemented.
        Char readCharInput(); // Read a character from the console. Most likely will be removed now that Winforms is implemented.
        int readIntInput(TextBox textBox); // Read an integer from the user's textbox input. The implemented function has error checking.
		String readTextBox(TextBox textBox); // Read the input that is in the textbox when the user presses the enter key.
	}
}
