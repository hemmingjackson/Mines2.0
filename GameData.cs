using Mines2._0.Boundary;

namespace Mines2._0.Entity
{
    /// <summary>
    /// This class is responsible for interacting with holding the data read from text files.
    /// </summary>
    public class GameData
    {
        public string[] roomDescriptions { get; private set; }
        public string[] treasures { get; private set; }
        public string[] weapons { get; private set; }
        public string[] obstacles { get; private set; }

        public GameData()
        {
            GameDataReader reader = GameDataReader.getInstance();
            roomDescriptions = reader.readFile("RoomDescriptions");
            treasures = reader.readFile("Treasures");
            weapons = reader.readFile("Weapons");
            obstacles = reader.readFile("Obstacles");
        }

        /// <summary>
        /// This method retrieves a room description.
        /// </summary>
        /// <param name="row">the index of the desired room description</param>
        /// <returns>the room description</returns>
        public string getRoomDescription(int row)
        {
            if (row >= 0 && row < roomDescriptions.Length)
                return roomDescriptions[row];
            return "";
        }

        /// <summary>
        /// This method retrieves a treasure.
        /// </summary>
        /// <param name="row">the index of the desired treasure</param>
        /// <returns>the treasure</returns>
        public string getTreasure(int row)
        {
            if (row >= 0 && row < treasures.Length)
                return treasures[row];
            return "";
        }

        /// <summary>
        /// This method retrieves a weapon.
        /// </summary>
        /// <param name="row">the index of the weapon</param>
        /// <returns>the weapon</returns>
        public string getWeapon(int row)
        {
            if (row >= 0 && row < weapons.Length)
                return weapons[row];
            return "";
        }

        /// <summary>
        /// This method retrieves an obstacle.
        /// </summary>
        /// <param name="row">the index of the desired obstacle</param>
        /// <returns>the obstacle</returns>
        public string getObstacle(int row)
        {
            if (row >= 0 && row < obstacles.Length)
                return obstacles[row];
            return "";
        }
    }
}
