using System;
using System.Collections.Generic;

namespace Fault {
	public class HelmetStyle : DressElement {
		private static List<HelmetStyle> HELMET_STYLES = new List<HelmetStyle>();
		
		//Instances
		public static HelmetStyle HELMET_0 = new HelmetStyle(0, "Helmet 0", "Not yet set", "Helmet0");
		
		//Instance
		private int id;
		private String modelName;
		private Model model;
		
		public HelmetStyle(int id, String name, String description, String modelName) : base(name, description) {
			this.id = id;
			this.modelName = modelName;
			HELMET_STYLES.Add(this);
		}
		
		public int getID() {return this.id;}
		public String getModelName() {return this.modelName;}
		public override Model getModel() {return getLoadedModel(this.modelName);}
	}
}

