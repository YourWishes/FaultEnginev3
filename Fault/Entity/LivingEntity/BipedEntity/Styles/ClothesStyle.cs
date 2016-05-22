using System;
using System.Collections.Generic;

namespace Fault {
	public class ClothesStyle : DressElement {
		private static List<ClothesStyle> CLOTHES_STYLES = new List<ClothesStyle>();
		
		//Instances
		public static ClothesStyle ARMOUR_0 = new ClothesStyle(0, "Armour 0", "Not yet set", "Armour0_Torso", "Armour0_LeftArm", "Armour0_RightArm");
		
		//Instance
		private int id;
		private String torso;
		private String leftArmSleeve;
		private String rightArmSleeve;
		
		public ClothesStyle(int id, String name, String description, 
		                    String torso, String leftArmSleeve, String rightArmSleeve) : base(name, description) {
			this.id = id;
			
			this.torso = torso;
			this.leftArmSleeve = leftArmSleeve;
			this.rightArmSleeve = rightArmSleeve;
			
			CLOTHES_STYLES.Add(this);
		}
		
		public int getID() {return this.id;}
		public String getTorsoModelName() {return this.torso;}
		public String getLeftSleeveModelName() {return this.leftArmSleeve;}
		public String getRightSleeveModelName() {return this.rightArmSleeve;}
		public override Model getModel() {return this.getTorso();}
		public Model getTorso() {return getLoadedModel(this.torso);}
		public Model getLeftArmSleeve() {return getLoadedModel(this.leftArmSleeve);}
		public Model getRightArmSleeve() {return getLoadedModel(this.rightArmSleeve);}
	}
}

