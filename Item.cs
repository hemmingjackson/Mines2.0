namespace Mines2._0
{

	/// This class holds the information for the items, or "weapons"
	/// that get generated in various caves in the mine.
	public class Item
	{
		private int id;
		private string description;

		public Item(int id, string description)
		{
			this.id = id;
			this.description = description;

		}
		//Returns item id
		public int getItemId()
		{
			return id;
		}
		//Sets item id
		public void setItemId(int newId)
		{
			this.id = newId;
		}
		//Retruns item description
		public string getDescription()
		{
			return description;
		}
		//Sets item description
		public void setDescription(string newDescription)
		{
			this.description = newDescription;
		}
	}
}
