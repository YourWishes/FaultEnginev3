using System;
using System.Collections.Generic;

namespace Fault {
	public class HairStyle : DressElement {
		private static List<HairStyle> HAIR_STYLES = new List<HairStyle>();
		
		//Instances
		public static HairStyle BOY_0 = new HairStyle(0, "Boy 0", "Not yet set", "Boy0_Hair");
		
		//Instance
		private int id;
		private Model model;
		private String modelName;
		
		public HairStyle(int id, String name, String description, String modelName) : base(name, description) {
			this.id = id;
			this.modelName = modelName;
			HAIR_STYLES.Add(this);
		}
		
		public int getID() {return this.id;}
		public String getModelName() {return this.modelName;}
		public override Model getModel() {return getLoadedModel(this.modelName);}
	}
}

