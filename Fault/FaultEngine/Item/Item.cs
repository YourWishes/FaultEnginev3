using System;

namespace Fault {
	public class Item {
		private ItemType itemType;
		
		public Item (ItemType type) {
			this.itemType = type;
		}
		
		public ItemType getType() {return this.itemType;}
		
		public bool compare(Item item) {
			if(this.Equals(item)) return true;
			if(item == null) return false;
			return this.itemType.Equals(item.getType());
		}
	}
}

