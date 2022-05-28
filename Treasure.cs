namespace Mines2._0
{
	/// Treasure class that keeps track of the treasure, guardian, 
	/// and weapon in specific rooms.
	public class Treasure
	{
		private int id;
		private string description;

		public Treasure(int id, string description)
		{
			this.id = id;
			this.description = description;
		}
		//Gets treasure id
		public int getTreasureId()
		{
			return id;
		}
		//Sets treasure id
		public void setTreasureId(int newId)
		{
			this.id = newId;
		}
		//Gets description
		public string getDescription()
		{
			return description;
		}
		//Sets description
		public void setDescription(string newDescription)
		{
			this.description = newDescription;
		}
	}
}
