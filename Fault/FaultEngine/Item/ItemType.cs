using System;
using System.Collections.Generic;

namespace Fault {
	public class ItemType {
		private static List<ItemType> ITEM_TYPES = new List<ItemType>();
		//Instances
		public static ItemType BASIC_SWORD = new ItemType(0, "Basic Sword", "Basic Sword with low level stats.", ItemCategory.WEAPONS);
		
		//Instance
		private int id;
		private String name;
		private String description;
		private ItemCategory category;
		
		private ItemType (int id, String name, String description, ItemCategory category) {
			this.id = id;
			this.name = name;
			this.description = description;
			this.category = category;
			ITEM_TYPES.Add(this);
		}
		
		public int getID() {return this.id;}
		public String getName() {return this.name;}
		public String getDescription() {return this.description;}
		public ItemCategory getItemCategory() {return this.category;}
	}
}

