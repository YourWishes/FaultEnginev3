using System;
using System.Collections.Generic;

namespace Fault {
	public class AnimationFramingType {
		private static List<AnimationFramingType> ANIMATION_TYPES = new List<AnimationFramingType>();
		
		//Instances
		public static AnimationFramingType TWEEN = new AnimationFramingType("t", "Tween");
		public static AnimationFramingType FRAME_BY_FRAME = new AnimationFramingType("f", "Frame by Frame");
		
		//Static Methods
		public static List<AnimationFramingType> getAnimationTypes() {lock(ANIMATION_TYPES) {return new List<AnimationFramingType>(ANIMATION_TYPES);}}
		
		public static AnimationFramingType getAnimationTypeByID(String id) {
			lock(ANIMATION_TYPES) {
				foreach(AnimationFramingType type in ANIMATION_TYPES) {
					if(type.getID().ToLower().Equals(id.ToLower())) return type;
				}
			}
		   return null;
		}
		
		
		//Instance
		private String id;
		private String name;
		
		private AnimationFramingType (String id, String name) {
			this.id = id;
			this.name = name;
			
			lock(ANIMATION_TYPES) {
				ANIMATION_TYPES.Add(this);	
			}
		}
		
		public String getID() {return this.id;}
		public String getName() {return this.name;}
	}
}

