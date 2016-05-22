using System;
using System.Collections.Generic;

namespace Fault {
	public class Region {
		//Static
		private static List<Region> REGIONS = new List<Region>();
		
		public static Region getByID(int id) {
			lock(REGIONS) {
				foreach(Region r in REGIONS) {
					if(r == null) continue;
					lock(r) {if(r.id == id) return r;}
				}
			}
			return null;
		}
		
		//Instances
		public static Region ENGLISH = new Region(0, "English", "en-US");
		
		//Instance
		private int id;
		private String name;
		private String regionCode;
		
		private Region (int id, String name, String regionCode) {
			this.id = id;
			this.name = name;
			this.regionCode = regionCode;
			
			lock(REGIONS) {
				REGIONS.Add(this);
			}
		}
		
		public int getID() {return this.id;}
		public String getName() {return this.name;}
		public String getRegionCode() {return this.regionCode;}
	}
}

