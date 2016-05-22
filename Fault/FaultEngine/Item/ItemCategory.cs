using System;
using System.Collections.Generic;

namespace Fault {
	public class ItemCategory {
		private static List<ItemCategory> ITEM_CATEGORIES = new List<ItemCategory>();
		
		//Instances
		public static ItemCategory WEAPONS = new ItemCategory(0, "Weapons", "Not Set Yet");
		
		//Instance
		private int id;
		private String name;
		private String description;
		
		private ItemCategory (int id, String name, String description) {
			this.id = id;
			this.name = name;
			this.description = description;
			ITEM_CATEGORIES.Add(this);
		}
	}
}

