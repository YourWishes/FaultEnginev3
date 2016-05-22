using System;

namespace Fault {
	public class FaultEntityType : EntityType {
		
		public static FaultEntityType BIPED_ENTITY = new FaultEntityType("Biped Entity", typeof(BipedEntity));
		
		public FaultEntityType (String name, Type type) : base(name, type) {
		}
	}
}

