using System;

namespace Fault {
	public class DressElement {
		private String name;
		private String description;
		
		public DressElement (String name, String description) {
			this.name = name;
			this.description = description;
		}
		
		public String getName() {return this.name;}
		public String getDescription() {return this.description;}
		public virtual Model getModel() {return null;}
		
		public Model getLoadedModel(String name) {
			return Game.GAME_INSTANCE.getLoadedModel(name);
		}
	}
}

