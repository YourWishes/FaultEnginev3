using System;
using System.Collections.Generic;

namespace Fault {
	public class ResourceType {
		private static List<ResourceType> RESOURCE_TYPES = new List<ResourceType>();
		public static ResourceType getTypeByName(String name) {
			if(name == null) return null;
			lock(RESOURCE_TYPES) {
				foreach(ResourceType rt in RESOURCE_TYPES) {
					if(rt == null || rt.getName() == null) continue;
					lock(rt) {
						if(rt.getName().ToLower().Equals(name.ToLower())) return rt;
					}
				}
			}
			return null;
		}
		
		public static ResourceType getTypeByXMLValue(String name) {
			if(name == null) return null;
			lock(RESOURCE_TYPES) {
				foreach(ResourceType rt in RESOURCE_TYPES) {
					if(rt == null || rt.getName() == null) continue;
					lock(rt) {
						if(rt.getXMLValue().ToLower().Equals(name.ToLower())) return rt;
					}
				}
			}
			return null;
		}
		
		//Instances
		public static ResourceType ANIMATION = new ResourceType("Animation", "animation");
		public static ResourceType BACKGROUND_MUSIC = new ResourceType("Background Music", "bgm");
		public static ResourceType TEXTURE = new ResourceType("Texture", "texture");
		public static ResourceType LANGUAGE = new ResourceType("Language", "language");
		public static ResourceType MODEL = new ResourceType("Model", "model");
		public static ResourceType SCENE = new ResourceType("Scene", "scene");
		
		//Instance
		private String name;
		private String xmlValue;
		
		private ResourceType (String name, String xmlValue) {
			this.name = name;
			this.xmlValue = xmlValue;
			lock(RESOURCE_TYPES) {
				RESOURCE_TYPES.Add(this);
			}
		}
		
		public String getName() {return this.name;}
		public String getXMLValue() {return this.xmlValue;}
	}
}

