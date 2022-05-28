namespace Mines2._0
{
	/// This class holds all the information pertaining to Obstacles,
	/// or the "guardians" that get generated in various caves in the mine. 
	public class Obstacle
	{
		private int id;
		private string description;
		private bool isAlive;
		/// <summary>
		/// Basic contructor for the obstacle
		/// </summary>
		/// <param name="id">id of the obstacle</param>
		/// <param name="description">description of the obstacle</param>

		public Obstacle(int id, string description)
		{
			this.id = id;
			this.description = description;
			isAlive = true;
		}
		/// <summary>
		/// Checks if the obstacle can be removed. If so, it is set to unalived status.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>

		public bool removeObstacle(Player player)
		{
			if(player.canRemoveObstacle(this))
			{
				bool found = false;
				for(int i = 0; !found; i++)
				{
					if(player.getItemInventory().ElementAt(i).getItemId() == id)
					{
						found = true;
						player.getItemInventory().RemoveAt(i);
					}
				}
				isAlive = false;
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// Gets obstacle id
		/// </summary>
		/// <returns>id of obstacle</returns>
		public int getObstacleId()
		{
			return id;
		}
		/// <summary>
		/// sets id of obstacle
		/// </summary>
		/// <param name="newId">new id of obstacle</param>
		public void setObstacleId(int newId)
		{
			this.id = newId;
		}
		/// <summary>
		/// Gets description of the obstacle
		/// </summary>
		/// <returns></returns>
		public string getDescription()
		{
			return description;
		}
		/// <summary>
		/// Sets the description of the obstacle
		/// </summary>
		/// <param name="newDescription"></param>
		public void setDescription(string newDescription)
		{
			this.description = newDescription;
		}
		/// <summary>
		/// Gets the status of if the obstacle is alive or not
		/// </summary>
		/// <returns></returns>

		internal bool getLivingStatus()
		{
			return isAlive;
		}
	}
}
