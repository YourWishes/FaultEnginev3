using System;

namespace Fault {
	public class DeathCause {
		public static DeathCause UKNOWN = new DeathCause(1, "Uknown");
		public static DeathCause DROWNING = new DeathCause(0, "Drowning");
		
		//Instance
		private int id;
		private String name;
		
		private DeathCause (int id, String name) {
			this.id = id;
			this.name = name;
		}
		
		public int getID() {return this.id;}
		public String getName() {return this.name;}
	}
}

