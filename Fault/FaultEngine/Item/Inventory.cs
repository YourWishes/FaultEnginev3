using System;

namespace Fault {
	public class Inventory {
		private static int INVENTORY_SIZE = 30;
		
		//Instance
		private InventoryHolder owner;
		private Item[] items = new Item[INVENTORY_SIZE];
		
		public Inventory (InventoryHolder owner) {
			this.owner = owner;
			
			for(int i = 0; i < items.Length; i++) {
				items[i] = null;
			}
		}
		
		public InventoryHolder getOwner() {return this.owner;}
		
		public Item getItem(int inventory_id) {lock(items) {return items[inventory_id];}}
		
		public void setItem(int inventory_id, Item item) {lock(items) {items[inventory_id] = item;}}
		
		public void removeItem(int inventory_id) {lock(this.items) {items[inventory_id] = null;}}
		public void removeItem(Item item) {lock(this.items) {for(int i = 0; i < this.items.Length; i++) {if(item.compare(item)) removeItem(i);}}}
		
		public bool isEmpty() {
			//TODO: Finish
			return true;
		}
	}
}

