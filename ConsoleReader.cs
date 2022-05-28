namespace Mines2._0.Boundary
{
    public class ConsoleReader : Inputable
    {
        public ConsoleReader() { }
        public String readLineInput()
        {
            try
            {
                return Console.ReadLine();
            }
            catch (Exception ex)
            {
                return "invalid input";
            }
        }
        public Char readCharInput()
        {
            return Char.ToUpper(Console.ReadLine()[0]); 
        }
        public int readIntInput(TextBox textBox)
        {
            try
            {
                return Convert.ToInt32(readTextBox(textBox));
            }
            catch
            {
                return -1;
            }
        }

		public String readTextBox(TextBox textBox)
		{
            String message = textBox.Text;
            textBox.Clear();   
            return message;    
		}
	}
}
