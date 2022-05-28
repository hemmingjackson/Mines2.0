using System.Numerics;

namespace Mines2._0.Entity
{
	public class Cave
	{
		// index 0: west
		// index 1 : east
		// index 2 : south
		// index 3 : north
		// index 4 : down
		// index 5 : up
		public List<Item> items { get; private set; }
		public List<Treasure> treasures;
		public Cave[] adjacentCaves;
		public bool isExplored{ get; set; }
		public Obstacle caveObstacle;

		public string descriptionID;

		public Vector3 position;

		public Cave() : this(0, 0, 0)
		{

		}

		public Cave(Vector3 position) : this((int)position.X, (int)position.Y, (int)position.Z)
		{

		}

		public Cave(int x, int y, int z)
		{
			items = new List<Item>();
			adjacentCaves = new Cave[6];
			position.X = x;
			position.Y = y;
			position.Z = z;

			//Setting the cave to be alrady explored or not
			if (x == 0 && y == 0 && z == 0)
				isExplored = true;
			else
				isExplored = false;

			descriptionID = "";
			treasures = new List<Treasure>();
		}

		public void removeObstacle(Player player)
		{
			caveObstacle.removeObstacle(player);
		}
		/// <summary>
		/// Adds the item the user want to drop back to cave
		/// </summary>
		/// <param name="item"></param>
		public void addDroppedItem(Item item)
		{
			this.items.Add(item);
		}
		/// <summary>
		/// Sends the items that are in the cave back to player to be put in their inventory
		/// </summary>
		/// <returns>tempItems list of cave items</returns>
		public List<Item> pickUpItem()
		{
			List<Item> tempItem = new List<Item>();
			foreach (Item item in items)
				tempItem.Add(item);
			items.Clear();
			return tempItem;
		}/// <summary>
		/// Adds the treasure the user wants to drop back to the cave
		/// </summary>
		/// <param name="treasure"></param>
		public void addDroppedTreasure(Treasure treasure)
		{
			this.treasures.Add(treasure);
		}
		/// <summary>
		/// Sends the treasure that are in the cave back to player to be put in their inventory
		/// </summary>
		/// <returns></returns>
		public List<Treasure> pickUpTreasure()
		{
			List<Treasure> tempTreasure = new List<Treasure>();
			foreach (Treasure treasure in treasures)
				tempTreasure.Add(treasure);
			treasures.Clear();
			return tempTreasure;
		}

		public Cave[] getAdjacentCaves()
		{
			return adjacentCaves;
		}

		/// <summary>
		/// This method checks if two caves are equal by comparing their coordinates.
		/// If their position values are the same, it should return true regardless of their other properties.
		/// </summary>
		/// <param name="obj">the object being compared to this cave</param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is null)
			{
				return false;
			}
			if (obj is Cave cave)
			{
				if (this == cave)
				{
					return true;
				}
				return this.position == cave.position;
			}
			return false;
		}
	}

}
