using Mines2._0.Boundary;
using Mines2._0.Entity;
using System.Numerics;

namespace Mines2._0.Control
{
    /// <summary>
    /// This class is responsible for handling the creation and logic of
    /// the game mines.
    /// </summary>
    public class MinesController
    {
        private const int ROOM_SIZE_MOD = 150;
        private const int CAVES_TO_GENERATE = 114;
        private const int TREASURE_TO_GENERATE = 18;
        private const int SQUARE_ROOM_WIDTH = 100;
        public bool dropItemFlag { get; private set; } = false;
        public bool dropTreasureFlag { get; private set; } = false;
        private IOManager IO;
        private Cave[] mine;
        private int caveSeed;
        private Player player;
        public int playerTurns { get; private set; }
        public int maximumPlayerTurns { get;  set; }
        private static GameData data = new GameData();
        private TextBox outputTextBox;


        /// <summary>
        /// Default constructor for the mines controller
        /// </summary>
        public MinesController(TextBox output, TextBox input, Label label)
        {
            IO = IOManager.getInstance();
            playerTurns = 0;
            caveSeed = -1;
            this.outputTextBox = output;
        }


        /// <summary>
        /// This method handles the instatiation of the game
        /// </summary>
        /// <param name="seed">The seed of the map</param>
        /// <param name="textBox">Textbox where dialog will be outputed into</param>
        public void initializeGame(int seed)
        {
            caveSeed = seed;
            generateMines(seed);
            player = new Player(mine[0]);
            IO.getOutputStream().writeToTextBox("Mine generated.", outputTextBox);
            printCaveInformation();
        }
        /// <summary>
        /// Draws the map for whatever layer the player is on
        /// </summary>
        /// <param name="e"></param>
		internal void drawMap(PaintEventArgs e, int Width, int Height)
		{
			Graphics G = e.Graphics;
            Pen pen = new Pen(Color.White);
            if(player != null)
			{
                float zPosition = player.GetCaveLocation().position.Z;
                for (int i = 0; i < mine.Length; i++)
                {
                    int xOffset = Width / 2 - 50 + -((int)player.GetCaveLocation().position.X * ROOM_SIZE_MOD) + (int)mine[i].position.X * ROOM_SIZE_MOD;
                    int yOffset = Height / 2 - 50 + ((int)player.GetCaveLocation().position.Y * ROOM_SIZE_MOD) - ((int)mine[i].position.Y * ROOM_SIZE_MOD);
                    
                    if (zPosition == mine[i].position.Z &&  mine[i].isExplored)
                    {
                        for (int j = 0; j < mine[i].getAdjacentCaves().Length; j++)
                        {
                            drawCorridor(G, pen, mine[i], xOffset, yOffset);
                        }
                        drawRoom(G, pen, xOffset, yOffset, mine[i]);
                        drawObstacle(G, pen, xOffset, yOffset, mine[i]);
                    }


                }
                pen.Color = Color.Purple;
                G.DrawRectangle(pen, player.GetCaveLocation().position.X * ROOM_SIZE_MOD + 25 + Width / 2 - 50 + -((int)player.GetCaveLocation().position.X * ROOM_SIZE_MOD), -(player.GetCaveLocation().position.Y * ROOM_SIZE_MOD) + 25 + Height / 2 - 50 + ((int)player.GetCaveLocation().position.Y * ROOM_SIZE_MOD), 50, 50);
                pen.Color = Color.White;
            }
         
            
        }
        /// <summary>
        /// Returns the player
        /// </summary>
        /// <returns></returns>
		public Player getPlayer()
		{
            return player;
		}

		/// <summary>
		/// Draws a room. If the room is the entrance, it receives another color
		/// </summary>
		/// <param name="G">Graphics to draw with</param>
		/// <param name="pen">Pen used to draw</param>
		/// <param name="position">position of the cave</param>
		/// <param name="xOffset">xoffset of screen</param>
		/// <param name="yOffset">yoffset of screen</param>
		public void drawRoom(Graphics G, Pen pen, int xOffset, int yOffset, Cave  cave)
		{
            if (cave.position.X == 0 && cave.position.Y == 0 && cave.position.Z == 0)
            {
                pen.Color = Color.Cyan;
                G.DrawRectangle(pen, xOffset, yOffset, SQUARE_ROOM_WIDTH, SQUARE_ROOM_WIDTH);
                pen.Color = Color.White;
            }
            else
            { 
                G.DrawRectangle(pen, xOffset, yOffset, SQUARE_ROOM_WIDTH, SQUARE_ROOM_WIDTH);
            }
            drawTreasureMarker(G, pen, xOffset,yOffset, cave);
            drawItemMarker(G, pen, xOffset, yOffset, cave);
        }
        /// <summary>
        /// Drawing Obstacles on the map using an X
        /// </summary>
        /// <param name="G">Graphics we use to draw</param>
        /// <param name="pen">Pen we draw with</param>
        /// <param name="cavePosX">X pos to draw</param>
        /// <param name="cavePosY">Y pos to draw</param>
        /// <param name="cave">cave we are checking</param>
        public void drawObstacle(Graphics G, Pen pen, int cavePosX, int cavePosY, Cave cave)
		{
            // index 0: west
            // index 1 : east
            // index 2 : south
            // index 3 : north
            // index 4 : down
            // index 5 : up
            //NORTH OB
            pen.Color = Color.DeepPink;
            if (cave.getAdjacentCaves()[0]?.caveObstacle is not null && cave.adjacentCaves[0].caveObstacle.getLivingStatus())
            {
                //Draw West
                G.DrawLine(pen, cavePosX + 5, cavePosY + 45,
                    cavePosX + 15, cavePosY + 55);
                G.DrawLine(pen, cavePosX + 5, cavePosY + 55,
                    cavePosX + 15, cavePosY + 45);
            }
            if (cave.getAdjacentCaves()[1]?.caveObstacle is not null && cave.adjacentCaves[1].caveObstacle.getLivingStatus())
            {
                //Draw East
                G.DrawLine(pen, cavePosX + 85, cavePosY + 55,
                    cavePosX + 95, cavePosY + 45);
                G.DrawLine(pen, cavePosX + 85, cavePosY + 45,
                    cavePosX + 95, cavePosY + 55);
            }
            if (cave.getAdjacentCaves()[2]?.caveObstacle is not null && cave.adjacentCaves[2].caveObstacle.getLivingStatus())
            {
                //Draw South
                G.DrawLine(pen, cavePosX + 45, cavePosY + 95,
                    cavePosX + 55, cavePosY + 85);
                G.DrawLine(pen, cavePosX + 45, cavePosY + 85,
                    cavePosX + 55, cavePosY + 95);
            }
            if (cave.getAdjacentCaves()[3]?.caveObstacle is not null && cave.adjacentCaves[3].caveObstacle.getLivingStatus())
            {
                //Draw North
                G.DrawLine(pen, cavePosX + 45, cavePosY + 5,
                    cavePosX + 55, cavePosY + 15);
                G.DrawLine(pen, cavePosX + 45, cavePosY + 15,
                    cavePosX + 55, cavePosY + 5);
            }
            if (cave.getAdjacentCaves()[4]?.caveObstacle is not null && cave.adjacentCaves[4].caveObstacle.getLivingStatus())
            {
                //Draw down
                G.DrawLine(pen, cavePosX + 80, cavePosY + 80,
                    cavePosX + 90, cavePosY + 80);

                G.DrawLine(pen, cavePosX + 90, cavePosY + 80,
                    cavePosX + 85, cavePosY + 85);

                G.DrawLine(pen, cavePosX + 80, cavePosY + 80,
                    cavePosX + 85, cavePosY + 85);
            }
            if (cave.getAdjacentCaves()[5]?.caveObstacle is not null && cave.adjacentCaves[5].caveObstacle.getLivingStatus())
            {
                //Draw up
                G.DrawLine(pen, cavePosX + 70, cavePosY + 85,
                    cavePosX + 80, cavePosY + 85);

                G.DrawLine(pen, cavePosX + 70, cavePosY + 85,
                    cavePosX + 75, cavePosY + 80);

                G.DrawLine(pen, cavePosX + 80, cavePosY + 85,
                    cavePosX + 75, cavePosY + 80);
            }
            pen.Color = Color.White;
        }
        /// <summary>
        /// Draws the item marker in a room.
        /// </summary>
        /// <param name="G">Graphics used to draw with</param>
        /// <param name="pen">Pen used to draw</param>
        /// <param name="cavePosX">Xcord of where to paint</param>
        /// <param name="cavePosY">YCord of where to paint</param>
        public void drawItemMarker(Graphics G, Pen pen, int cavePosX, int cavePosY, Cave cave)
		{
            if(cave.items.Count() > 0)
            {
                G.DrawLine(pen, cavePosX + 20, cavePosY + 10,
                    cavePosX + 26, cavePosY + 10);
                G.DrawLine(pen, cavePosX + 23, cavePosY + 10,
                    cavePosX + 23, cavePosY + 15);
                G.DrawLine(pen, cavePosX + 20, cavePosY + 15,
                   cavePosX + 26, cavePosY + 15);
            }
        }
        /// <summary>
        /// Draws the treasure marker in a room.
        /// </summary>
        /// <param name="G">Graphics used to draw with</param>
        /// <param name="pen">Pen used to draw</param>
        /// <param name="cavePosX">Xcord of where to paint</param>
        /// <param name="cavePosY">YCord of where to paint</param>
        public void drawTreasureMarker(Graphics G, Pen pen, int cavePosX, int cavePosY, Cave cave)
		{
            if (cave.treasures.Count() > 0)
            {
                G.DrawLine(pen, cavePosX + 10, cavePosY + 10,
                    cavePosX + 16, cavePosY + 10);
                G.DrawLine(pen, cavePosX + 13, cavePosY + 10,
                    cavePosX + 13, cavePosY + 15);
            }
        }
        /// <summary>
        /// Paints corridors of adjacent caves to the cave that is being passed
        /// </summary>
        /// <param name="G">Graphics that are being passed to draw to</param>
        /// <param name="pen">Pen that is used to draw</param>
        /// <param name="cave">cave that we are painting corridors of</param>
        /// <param name="cavePosY">xOffset of screen</param>
        /// <param name="cavePosX">yOffset of screen</param>
        public void drawCorridor(Graphics G, Pen pen, Cave cave, int cavePosX, int cavePosY)
		{
            // index 0: west
            // index 1 : east
            // index 2 : south
            // index 3 : north
            // index 4 : down
            // index 5 : up

            if (cave.getAdjacentCaves()[0] != null)
			{
                //Drawing west corridor
                G.DrawRectangle(pen, cavePosX - 50, cavePosY + 40, 49, 20);
            }
            if (cave.getAdjacentCaves()[1] != null)
            {
                //Drawing east corridor
                G.DrawRectangle(pen, cavePosX + SQUARE_ROOM_WIDTH, cavePosY + 40, 49, 20);
            }
            if (cave.getAdjacentCaves()[2] != null)
            {
                //Drawing south corridor
                G.DrawRectangle(pen, cavePosX + 40, cavePosY + SQUARE_ROOM_WIDTH, 20, 49);
            }   
            if (cave.getAdjacentCaves()[3] != null)
            {
                //Drawing north corridor
                G.DrawRectangle(pen, cavePosX + 40, cavePosY - 50, 20, 49);
            }
            if (cave.getAdjacentCaves()[4] != null)
            {
                //Drawing down corridor
                G.DrawLine(pen, cavePosX + 80, cavePosY + 80,
                    cavePosX + 90, cavePosY + 80);

                G.DrawLine(pen, cavePosX + 90, cavePosY + 80,
                    cavePosX + 85, cavePosY + 85);

                G.DrawLine(pen, cavePosX + 80, cavePosY + 80,
                    cavePosX + 85, cavePosY + 85);

            }
            if (cave.getAdjacentCaves()[5] != null)
            {
                //Drawing up corridor
                G.DrawLine(pen, cavePosX + 70, cavePosY + 85,
                    cavePosX + 80, cavePosY + 85);

                G.DrawLine(pen, cavePosX + 70, cavePosY + 85,
                    cavePosX + 75, cavePosY + 80);

                G.DrawLine(pen, cavePosX + 80, cavePosY + 85,
                    cavePosX + 75, cavePosY + 80);

            }
        }
        /// <summary>
        /// Outputs room discriptions along with which rooms the player can travel to
        /// </summary>
        /// <param name="textBox">Textbox to write to</param>
        public void outputRoomInfo()
		{
            string message = "There are mine entrances leading";
            bool isFirst = true;
            if (player.GetCaveLocation().getAdjacentCaves()[(int)Directions.NORTH] != null)
            {
                if (isFirst)
                {
                    message += " North";
                    isFirst = false;
                }
                else
                    message += ", North";
            }
            if (player.GetCaveLocation().getAdjacentCaves()[(int)Directions.SOUTH] != null)
            {
                if (isFirst)
                {
                    message += " South";
                    isFirst = false;
                }
                else
                    message += ", South";
            }
            if (player.GetCaveLocation().getAdjacentCaves()[(int)Directions.WEST] != null)
            {
                if (isFirst)
                {
                    message += " West";
                    isFirst = false;
                }
                else
                    message += ", West";
            }
            if (player.GetCaveLocation().getAdjacentCaves()[(int)Directions.EAST] != null)
            {
                if (isFirst)
                {
                    message += " East";
                    isFirst = false;
                }
                else
                    message += ", East";
            }
            if (player.GetCaveLocation().getAdjacentCaves()[(int)Directions.UP] != null)
            {
                if (isFirst)
                {
                    message += " Up";
                    isFirst = false;
                }
                else
                    message += ", Up";
            }
            if (player.GetCaveLocation().getAdjacentCaves()[(int)Directions.DOWN] != null)
            {
                if (isFirst)
                {
                    message += " Down";
                    isFirst = false;
                }
                else
                    message += ", Down";
            }

            IO.getOutputStream().writeToTextBox(message, outputTextBox);
            IO.getOutputStream().writeToTextBox(player.GetCaveLocation().descriptionID, outputTextBox);
        }


        /// <summary>
        /// Trys to convert a string into an integer. If possible, returns the integer. If not, return -1.
        /// </summary>
        /// <param name="value">value to be parsed</param>
        /// <returns></returns>
        public int convertToInt(String value)
        {
            try
            {
               return Convert.ToInt32(value);
            }
            catch
            {
                return -1;
            }
        }


        /// <summary>
        /// Returns a string of the user inventory of treasures
        /// </summary>
        /// <returns></returns>
		public string getPlayerTreasures()
		{
            //return player.showInventory();
            //TODO next iteration
            return "Hello World";
        }


        /// <summary>
        /// Responsible for listing commands to the user
        /// </summary>
        /// <param name="textBox">Textbox to write to</param>
		public void listCommands()
        {
            IO.getOutputStream().writeToTextBox("\nCommands in the game are \"N\", \"E\", \"S\", \"W\", \"U\", and " +
            "\"D\" to move North, East, South, West, Up, or Down, " +
            "respectively.  Other commands are \"C\" to carry things, \"L\" to leave items," +
            " \"K\" to leave treasures, \"P\" to " +
            "get the points you\'ve scored, \"O\" for help getting out of the " +
            "mine, \"H\" for help, and \"Q\" to quit.\n", outputTextBox);
        }


        /// <summary>
        /// Displays the list of commands and also prints out basic information on gameplay.
        /// </summary>
        /// <param name="textBox">Textbox to be written to</param>
        public void displayHelp()
        {
            listCommands();

            IO.getOutputStream().writeToTextBox("In a mine, the passages are straight.  So, for example, if " +
            "you go North to leave a room, you can go South to reenter it. " +
            "The rooms are not evenly spaced.  However, the distance between " +
            "adjacent rooms is always a multiple of the minimum distance " +
            "between adjacent rooms." + Environment.NewLine, outputTextBox);
        }


        /// <summary>
        /// Prints the list of weapons in the specific room.
        /// </summary>
        /// <param name="textBox">Text box to write to</param>
        /// <param name="roomNum">Room number to print treasures of</param>
        public void listTreasures(int roomNum)
        {
            IO.getOutputStream().writeToTextBox("There is a " + data.getTreasure(roomNum) + "here", outputTextBox);
        }


        /// Prints the list of weapons in the specific room.
        /// <param name="textBox"></param>
        /// <param name="roomNum"></param>
        public void listWeapons(int roomNum)
        {
            IO.getOutputStream().writeToTextBox("There is a " + data.getWeapon(roomNum) + "here", outputTextBox);
        }


        /// <summary>
        /// Takes in a command and determines what functionality to call.
        /// Returns true for all commands except the quit command, which signals
        /// that the while loop should finish.
        /// </summary>
        /// <param name="command">Command to parse</param>
        /// <param name="textBox">Textbox to write to</param>
        public void parseCommand(string userCommand)
        {
            char command = userCommand[0];
            if(!dropItemFlag && !dropTreasureFlag)
            {
                switch (Char.ToUpper(command))
                {
                    case 'N':           // Player move to the north
                        if (player.move((int)Directions.NORTH))
                        {
                            IOManager.getInstance().getOutputStream().writeToTextBox("You moved into the next cave", outputTextBox);
                            playerTurns++;
                        }
                        else
                            IOManager.getInstance().getOutputStream().writeToTextBox("You cannot move that way", outputTextBox);
                        printCaveInformation();
                        break;
                    case 'E':           // Player move to the east
                        if (player.move((int)Directions.EAST))
                        {
                            IOManager.getInstance().getOutputStream().writeToTextBox("You moved into the next cave", outputTextBox);
                            playerTurns++;
                        }
                        else
                        {
                            IOManager.getInstance().getOutputStream().writeToTextBox("You cannot move that way", outputTextBox);
                        }
                        printCaveInformation();
                        break;
                    case 'S':           // Player move to the south
                        if (player.move((int)Directions.SOUTH))
                        {
                            IOManager.getInstance().getOutputStream().writeToTextBox("You moved into the next cave", outputTextBox);
                            playerTurns++;
                        }
                        else
                            IOManager.getInstance().getOutputStream().writeToTextBox("You cannot move that way", outputTextBox);
                        printCaveInformation();
                        break;
                    case 'W':           // Player move to the west
                        if (player.move((int)Directions.WEST))
                        {
                            IOManager.getInstance().getOutputStream().writeToTextBox("You moved into the next cave", outputTextBox);
                            playerTurns++;
                        }
                        else
                            IOManager.getInstance().getOutputStream().writeToTextBox("You cannot move that way", outputTextBox);
                        printCaveInformation();
                        break;
                    case 'U':           // Player move up a level
                        if (player.move((int)Directions.UP))
                        {
                            IOManager.getInstance().getOutputStream().writeToTextBox("You moved into the next cave", outputTextBox);
                            playerTurns++;
                        }
                        else
                            IOManager.getInstance().getOutputStream().writeToTextBox("You cannot move that way", outputTextBox);
                        printCaveInformation();
                        break;
                    case 'D':           // Player move down a level
                        if (player.move((int)Directions.DOWN))
                        {
                            IOManager.getInstance().getOutputStream().writeToTextBox("You moved into the next cave", outputTextBox);
                            playerTurns++;
                        }
                        else
                            IOManager.getInstance().getOutputStream().writeToTextBox("You cannot move that way", outputTextBox);
                        printCaveInformation();
                        break;
                    case 'C':           // Player carries all items in the room. They are placed into the inventory.
                        foreach (Item item in player.GetCaveLocation().items)
                        {
                            IOManager.getInstance().getOutputStream().writeToTextBox($"You picked up a { item.getDescription()}", outputTextBox);
                        }
                        player.pickUpItems();
                        foreach (Treasure treasure in player.GetCaveLocation().treasures)
                        {
                            IOManager.getInstance().getOutputStream().writeToTextBox($"You picked up a { treasure.getDescription()}", outputTextBox);
                        }
                        player.pickUpTreasure();
               
                        printCaveInformation();
                        break;
                    case 'L':           // Leave item in current room that user selects.
                        IOManager.getInstance().getOutputStream().writeToTextBox("Select item to drop (enter number next to item)", outputTextBox);

                        if (player.getItemInventory().Count != 0)
                        {
                            dropItemFlag = true;
                            int index = 1;
                            foreach (Item item in player.getItemInventory())
                            {
                                IOManager.getInstance().getOutputStream().writeToTextBox($"{index + " : " + data.getWeapon(item.getItemId())}", outputTextBox);
                                index++;
                            }
                        }
                        break;
                    case 'K':           //Leave treasure in current room that user selects
                        IOManager.getInstance().getOutputStream().writeToTextBox("Select treasure to drop (enter number next to treasure)", outputTextBox);
                        if (player.getTreasureInventory().Count != 0)
                        {
                            dropTreasureFlag = true;
                            int index = 1;
                            foreach (Treasure treasure in player.getTreasureInventory())
                            {
                                IOManager.getInstance().getOutputStream().writeToTextBox($"{index + " : " + data.getTreasure(treasure.getTreasureId())}", outputTextBox);
                                index++;
                            }
                            if (mine[0].treasures.Count() == TREASURE_TO_GENERATE)
                            {
                                IOManager.getInstance().getOutputStream().writeToTextBox("Congratulations, you win!", outputTextBox);
                                parseCommand("Q");
                            }    
                        }
                        break;
                    case 'P':           // Display the points the player has scored.
                                        //int points = player.calculatePoints(numRooms, numTreasures, playerTurns);
                        Points();
                        printCaveInformation();
                        break;
                    case 'O':           // Display help for the player to exit the mine.
                        IOManager.getInstance().getOutputStream().writeToTextBox("That feature hasn't been implemented yet :(", outputTextBox);
                        break;
                    case 'H':           // Display help: a list of commands and description of the game.
                        displayHelp();
                        printCaveInformation();
                        break;
                    case 'Q':
                        IOManager.getInstance().getOutputStream().writeToTextBox("Thanks for playing! Here are your results: ", outputTextBox);
                        Points();
                        IOManager.getInstance().getOutputStream().writeToTextBox("Game will close automatically in 10 seconds", outputTextBox);
                        Thread.Sleep(10000);
                        Environment.Exit(0);
                        break;

                    default:            // By default any command that is not handled prior to now is not a valid command.
                        IO.getOutputStream().writeToTextBox("I don't recognize that command, " +
                            "press 'H' to see a list of commands", outputTextBox);
                        return;
                }
            }
            else if(dropItemFlag)
            {
                //Item to be dropped
                int itemToDrop = 0;
                try
                {
                    //Converts players command from string to int
                    itemToDrop = Convert.ToInt32(userCommand);
                    //If the user enters a valid number
                    if (itemToDrop <= player.getItemInventory().Count() && itemToDrop > 0)
                    {
                        IO.getOutputStream().writeToTextBox($"You dropped { player.getItemInventory().ElementAt(itemToDrop - 1).getDescription() } here.", outputTextBox);
                        player.dropItem(itemToDrop - 1);
                        dropItemFlag = false;
                        printCaveInformation();
                    }
                    //User wants to exit drop dialog
                    else if (itemToDrop < 0)
                    {
                        dropItemFlag = false;
                    }
                    //Invalid number entered
                    else
                    {
                        IO.getOutputStream().writeToTextBox("Invalid index of item. Please enter a proper index or -1 to exit the drop item prompt", outputTextBox);
                        IOManager.getInstance().getOutputStream().writeToTextBox("Select item to drop (enter number next to item)", outputTextBox);
                        //Re lists the items in players inventory
                        if (player.getItemInventory().Count != 0)
                        {
                            dropItemFlag = true;
                            int index = 1;
                            foreach (Item item in player.getItemInventory())
                            {
                                IOManager.getInstance().getOutputStream().writeToTextBox($"{index + " : " + data.getWeapon(item.getItemId())}", outputTextBox);
                                index++;
                            }
                        }
                    }
                }
                catch
                {
                    //Invalid input entered
                    IO.getOutputStream().writeToTextBox("Not a valid number!", outputTextBox);
                }
                
                
            }
            else if(dropTreasureFlag)
            {
                //Treasure to drop
                int treasureToDrop = 0;
                try
                {
                    //Converts players command from string to int
                    treasureToDrop = Convert.ToInt32(userCommand);
                    if (treasureToDrop <= player.getTreasureInventory().Count() && treasureToDrop > 0)
                    {
                        IO.getOutputStream().writeToTextBox($"You dropped { player.getTreasureInventory().ElementAt(treasureToDrop - 1).getDescription() } here.", outputTextBox);
                        player.dropTreasures(treasureToDrop - 1);
                        dropTreasureFlag = false;
                        printCaveInformation();
                    }
                    //User wants to exit drop dialog
                    else if (treasureToDrop < 0)
                    {
                        dropTreasureFlag = false;
                    }
                    //Invalid number entered
                    else
                    {
                        IO.getOutputStream().writeToTextBox("Invalid index of treasure. Please enter a proper index or -1 to exit the drop item prompt", outputTextBox);
                        IOManager.getInstance().getOutputStream().writeToTextBox("Select treasure to drop (enter number next to item)", outputTextBox);
                        //Re lists the treasures in players inventory
                        if (player.getTreasureInventory().Count != 0)
                        {
                            dropTreasureFlag = true;
                            int index = 1;
                            foreach (Treasure treasure in player.getTreasureInventory())
                            {
                                IOManager.getInstance().getOutputStream().writeToTextBox($"{index + " : " + data.getTreasure(treasure.getTreasureId())}", outputTextBox);
                                index++;
                            }
                        }
                    }
                }
                catch
                {   
                    //Invalid input entered
                    IO.getOutputStream().writeToTextBox("Not a valid number! Please enter a proper index or -1 to exit the drop item prompt", outputTextBox);
                }
            }
        }
        /// <summary>
        /// This method prints item, treasure, and obstacle information to the textbox.
        /// </summary>
        public void printCaveInformation()
        {
            Cave currentCave = player.GetCaveLocation();
            Cave[] adjacentCaves = currentCave.getAdjacentCaves();
            Outputable outputStream = IOManager.getInstance().getOutputStream();
            foreach (Item item in currentCave.items)
            {
                outputStream.writeToTextBox($"There is a {item.getDescription()} here.", outputTextBox);
            }
            foreach (Treasure treasure in currentCave.treasures)
            {
                outputStream.writeToTextBox($"There is a {treasure.getDescription()} here.", outputTextBox);
            }
            if (adjacentCaves[0]?.caveObstacle is not null && adjacentCaves[0].caveObstacle.getLivingStatus())
            {
                outputStream.writeToTextBox($"The cave west is guarded by a {adjacentCaves[0].caveObstacle.getDescription()}.", outputTextBox);
            }
            if (adjacentCaves[1]?.caveObstacle is not null && adjacentCaves[1].caveObstacle.getLivingStatus())
            {
                outputStream.writeToTextBox($"The cave east is guarded by a {adjacentCaves[1].caveObstacle.getDescription()}.", outputTextBox);
            }
            if (adjacentCaves[2]?.caveObstacle is not null && adjacentCaves[2].caveObstacle.getLivingStatus())
            {
                outputStream.writeToTextBox($"The cave south is guarded by a {adjacentCaves[2].caveObstacle.getDescription()}.", outputTextBox);
            }
            if (adjacentCaves[3]?.caveObstacle is not null && adjacentCaves[3].caveObstacle.getLivingStatus())
            {
                outputStream.writeToTextBox($"The cave north is guarded by a {adjacentCaves[3].caveObstacle.getDescription()}.", outputTextBox);
            }
            if (adjacentCaves[4]?.caveObstacle is not null && adjacentCaves[4].caveObstacle.getLivingStatus())
            {
                outputStream.writeToTextBox($"The cave down is guarded by a {adjacentCaves[4].caveObstacle.getDescription()}.", outputTextBox);
            }
            if (adjacentCaves[5]?.caveObstacle is not null && adjacentCaves[5].caveObstacle.getLivingStatus())
            {
                outputStream.writeToTextBox($"The cave up is guarded by a {adjacentCaves[5].caveObstacle.getDescription()}.", outputTextBox);
            }
        }
        /// <summary>
        /// Prints the points the users has scored so far
        /// </summary>
        public void Points()
        {
            IOManager.getInstance().getOutputStream().writeToTextBox("You have moved " + playerTurns + " times to visit " + player.roomsVisited + " out of " + mine.Length + " locations", outputTextBox);
            IOManager.getInstance().getOutputStream().writeToTextBox("You hold " + player.getTreasureInventory().Count + " out of " + TREASURE_TO_GENERATE + " treasures.", outputTextBox);
            IOManager.getInstance().getOutputStream().writeToTextBox("You have returned " + Decimal.Round((Decimal)mine[0].treasures.Count/TREASURE_TO_GENERATE * 100) + "% of " + TREASURE_TO_GENERATE + " treasures to the entrance of the mine.", outputTextBox);
            IOManager.getInstance().getOutputStream().writeToTextBox("You have scored " + player.calculatePoints(TREASURE_TO_GENERATE, playerTurns, CAVES_TO_GENERATE, mine[0].treasures.Count) + " out of 100 points.", outputTextBox);
        }

        /// <summary>
        /// Formerly, "ExcavateMine" method in original cave.
        /// Generates mine layout based on number of mines;
        /// </summary>
        /// <param name="seed">the seed used to randomize the mine</param>
        public void generateMines(int seed)
        {
            Vector3 westTranslate = new Vector3(-1, 0, 0);
            Vector3 southTranslate = new Vector3(0, -1, 0);
            Vector3 downTranslate = new Vector3(0, 0, -1);
            mine = new Cave[CAVES_TO_GENERATE];
            Dictionary<Vector3, Cave> caves = new Dictionary<Vector3, Cave>();
            Dictionary<Vector3, Cave> potentialCaves = new Dictionary<Vector3, Cave>();
            Dictionary<Cave, SortedSet<int>> cavesBehindObstacles = new Dictionary<Cave, SortedSet<int>>();
            List<string> descriptions = new List<string>();
            Dictionary<int, string> items = new Dictionary<int, string>();
            Dictionary<int, string> obstacles = new Dictionary<int, string>();
            Dictionary<int, string> treasures = new Dictionary<int, string>();
            for (int descriptionID = 0; descriptionID < mine.Length; descriptionID++)
            {
                descriptions.Add(data.getRoomDescription(descriptionID));
            }
            for (int itemID = 0; itemID < TREASURE_TO_GENERATE; itemID++)
            {
                items.Add(itemID, data.getWeapon(itemID));
                obstacles.Add(itemID, data.getObstacle(itemID));
                treasures.Add(itemID, data.getTreasure(itemID));
            }
            Random random = new Random(seed);
            mine[0] = new Cave();
            caves.Add(mine[0].position, mine[0]);
            Cave recentCave = mine[0];
            cavesBehindObstacles.Add(recentCave, new SortedSet<int>());
            int caveCount = 1;
            while (caveCount < mine.Length)
            {
                // This part of the function adds potential positions of caves to a list as long as the position isn't already used by an existing cave.
                Vector3 oldPosition = recentCave.position;
                Cave westCave = new Cave(oldPosition + westTranslate);
                Cave eastCave = new Cave(oldPosition - westTranslate);
                Cave southCave = new Cave(oldPosition + southTranslate);
                Cave northCave = new Cave(oldPosition - southTranslate);
                Cave downCave = new Cave(oldPosition + downTranslate);
                Cave upCave = new Cave(oldPosition - downTranslate);
                if (!caves.ContainsKey(westCave.position))
                {
                    potentialCaves.TryAdd(westCave.position, westCave);
                }
                if (!caves.ContainsKey(eastCave.position))
                {
                    potentialCaves.TryAdd(eastCave.position, eastCave);
                }
                if (!caves.ContainsKey(southCave.position))
                {
                    potentialCaves.TryAdd(southCave.position, southCave);
                }
                if (!caves.ContainsKey(northCave.position))
                {
                    potentialCaves.TryAdd(northCave.position, northCave);
                }
                if (!caves.ContainsKey(downCave.position))
                {
                    potentialCaves.TryAdd(downCave.position, downCave);
                }
                if (!caves.ContainsKey(upCave.position))
                {
                    potentialCaves.TryAdd(upCave.position, upCave);
                }
                // This part of the function picks a random position from the list and creates a cave there.
                int caveIndex = random.Next(potentialCaves.Count);
                recentCave = potentialCaves.ElementAt(caveIndex).Value;
                potentialCaves.Remove(recentCave.position);
                caves.Add(recentCave.position, recentCave);
                cavesBehindObstacles.Add(recentCave, new SortedSet<int>());
                mine[caveCount] = recentCave;
                caveCount++;
                // This section of code will decide whether to place an obstacle in the recently generated cave. If so, it
                // also generates a treasure in that cave and puts the item that overcomes the obstacle elsewhere in the mine.
                int treasureGeneration = random.Next(CAVES_TO_GENERATE - caveCount);
                if (treasureGeneration < treasures.Count)
                {
                    int itemCave = random.Next(caveCount - 1);
                    int itemID = random.Next(items.Count);
                    mine[itemCave].items.Add(new Item(items.Keys.ElementAt(itemID), items.Values.ElementAt(itemID)));
                    recentCave.caveObstacle = new Obstacle(obstacles.Keys.ElementAt(itemID), obstacles.Values.ElementAt(itemID));
                    items.Remove(items.Keys.ElementAt(itemID));
                    obstacles.Remove(obstacles.Keys.ElementAt(itemID));
                    int treasureID = random.Next(treasures.Count);
                    recentCave.treasures.Add(new Treasure(treasures.Keys.ElementAt(itemID), treasures.Values.ElementAt(itemID)));
                    treasures.Remove(treasures.Keys.ElementAt(itemID));
                }
                // This part of the code connects the newly generated cave to other caves adjacent to it, if necessary.
                bool initialObstacleAdd = false;
                for (int i = 0; i < 6; i++)
                {
                    // This part translates the cave's vector so that it will point to a cave next to the most recent one (0 = west, 1 = east, etc.).
                    Vector3 position = recentCave.position;
                    Vector3 newPosition = position + new Vector3(-(i + 4) / 6 + 2 * ((i + 5) / 6) - (i + 6) / 6,
                                                                -(i + 2) / 6 + 2 * ((i + 3) / 6) - (i + 4) / 6,
                                                                -i / 6 + 2 * ((i + 1) / 6) - (i + 2) / 6);
                    Cave cave;
                    if (caves.TryGetValue(newPosition, out cave))
                    {
                        if (!initialObstacleAdd && cavesBehindObstacles[recentCave].Count() == 0)
                        {
                            cavesBehindObstacles[recentCave].UnionWith(cavesBehindObstacles[cave]);
                            initialObstacleAdd = true;
                        }
                        // The two caves are connected if and only if they're behind the same set of obstacles.
                        if (SortedSet<int>.CreateSetComparer().Equals(cavesBehindObstacles[recentCave], cavesBehindObstacles[cave]))
                        {
                            recentCave.getAdjacentCaves()[i] = cave;
                            cave.getAdjacentCaves()[i + 2 - (2 * i + 1) % 4] = recentCave;
                            // If the most recent cave contains an obstacle, the loop is terminated and the most recent cave won't be connected to any other caves.
                            if (recentCave.caveObstacle is not null)
                            {
                                cavesBehindObstacles[recentCave].Add(recentCave.caveObstacle.getObstacleId());
                                break;
                            }
                        }
                    }
                }
                // The cave is given a random description.
                int descriptionID = random.Next(descriptions.Count);
                recentCave.descriptionID = descriptions[descriptionID];
                descriptions.RemoveAt(descriptionID);
            }
            mine[0].descriptionID = descriptions[0];
        }

        /// <summary>
        /// Returns the seed of the cave
        /// </summary>
        /// <returns>Cave seed which is an int</returns>
        public int getCaveSeed()
        {
            return this.caveSeed;
        }
    }
}
