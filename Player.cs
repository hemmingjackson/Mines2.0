using Mines2._0.Entity;

namespace Mines2._0
{

	/// <summary>
	/// Player class that stores information regarding their position
	/// in the cave and their inventory.
	/// </summary>
	public class Player
	{
		private List<Item> inventoryItems;
		private List<Treasure> inventoryTreasures;
		private Cave caveLocation;
		private int score;


		public virtual int roomsVisited { get; private set; }

		public Player(Cave caveLocation)
		{
			this.caveLocation = caveLocation;
			inventoryItems = new List<Item>();
			inventoryTreasures = new List<Treasure>();
			roomsVisited = 0;
			score = 0;
		}

		/*
		 * Checks for adjacent rooms 
		 */
		public bool move(int direction)
		{
			if (caveLocation.getAdjacentCaves()[direction] != null 
				&& (caveLocation.getAdjacentCaves()[direction].caveObstacle == null || !caveLocation.getAdjacentCaves()[direction].caveObstacle.getLivingStatus() || canRemoveObstacle(caveLocation.getAdjacentCaves()[direction].caveObstacle)))
			{
				if(!(caveLocation.getAdjacentCaves()[direction].caveObstacle == null) && caveLocation.getAdjacentCaves()[direction].caveObstacle.getLivingStatus())
				{
					caveLocation.getAdjacentCaves()[direction].removeObstacle(this);
				}
				caveLocation = caveLocation.getAdjacentCaves()[direction];
				roomsVisited++;
				caveLocation.isExplored = true;
				return true;
			}
			else
				return false;
		}

		/*
		 * Returns the current cave location of the player within the mine
		 */ 
		public Cave GetCaveLocation()
		{
			return caveLocation;
		}
		/// <summary>
		/// Drops the item that corresponds to the given itemId
		/// </summary>
		/// <param name="itemId"></param>
		public void dropItem(int itemId)
		{
			if (itemId >= 0 && itemId < inventoryItems.Count)
			{
				caveLocation.addDroppedItem(inventoryItems.ElementAt(itemId));

				inventoryItems.RemoveAt(itemId);
			}

		}
		/// <summary>
		/// Adds all the items that where inside the currentCave Location if any
		/// </summary>
		public void pickUpItems()
		{
			List<Item> items = caveLocation.pickUpItem();
			for (int i = 0; i < items.Count; i++)
            {
				inventoryItems.Add(items.ElementAt(i));
            }
		}
		/// <summary>
		/// If ther is a treasure inside of a cave room it will be put 
		/// in the slot of the array that coresponds with the treasure ID
		/// </summary>
		public void pickUpTreasure()
        {
			List<Treasure> treasures = caveLocation.pickUpTreasure();
			for (int i = 0; i < treasures.Count; i++)
			{
				inventoryTreasures.Add(treasures.ElementAt(i));
			}
		}
		/// <summary>
		/// Drops treasure given a certian treasure id
		/// </summary>
		/// <param name="treasureId"></param>
		public void dropTreasures(int treasureId)
		{
			if (treasureId >= 0 && treasureId < inventoryTreasures.Count)
			{
				caveLocation.addDroppedTreasure(inventoryTreasures.ElementAt(treasureId));

				inventoryTreasures.RemoveAt(treasureId);
			}
		}

		/// <summary>
		/// retruns true if the player has all treasures
		/// </summary>
		/// <returns>Returns true if user has all treasures</returns>
		public bool hasAllTreasures()
        {
			bool allTreasures = false;
			if(inventoryTreasures.Count == 15)
				allTreasures = true;
			return allTreasures;
        }

		/// <summary>
		/// Returns the number of treasures held by the player.
		/// </summary>
		/// <returns></returns>
		public List<Treasure> getTreasureList()
		{
			return inventoryTreasures;
		}
		public List<Item> getItemList()
		{
			return inventoryItems;
		}

		/// <summary>
		/// Check if the player has the nessecary item for a given obstacle
		/// </summary>
		/// <param name="caveObstacle"></param>
		/// <returns>
		/// true if player has nessacary item
		/// </returns>
		public virtual bool canRemoveObstacle(Obstacle caveObstacle)
        {
			bool canRemove = false;
			foreach (Item item in inventoryItems)
            {
				if (item.getItemId() == caveObstacle.getObstacleId())
					canRemove = true;
            }
			return canRemove;

		}
		/// <summary>
		/// Returns the players inventory of items
		/// </summary>
		/// <returns></returns>
		public List<Item> getItemInventory()
		{
			return inventoryItems;
		}
		/// <summary>
		/// Retruns the players inventory of treasures
		/// </summary>
		/// <returns></returns>
		public List<Treasure> getTreasureInventory()
		{
			return inventoryTreasures;
		}

		/// Calculates the points the user has scored within the game
		/// <returns>points</returns>
		public int calculatePoints(int numTreasures, int numMoves, int numRooms, int treasures_recovered)
		{
			int treasures_carried = this.inventoryTreasures.Count;

			score = (25 * roomsVisited / (numRooms + 1)) + (75 * treasures_recovered / numTreasures) + (45 * treasures_carried / numTreasures);

			if (numMoves > 5 * numRooms)
			{
				score = score - numMoves / (5 * numRooms);
			}
			
			return score;
		}
	}
}

