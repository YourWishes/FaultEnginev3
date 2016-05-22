using System;
using System.Collections.Generic;

namespace Fault {
	public class EntityType {
		private static List<EntityType> ENTITY_TYPES = new List<EntityType>();
		
		public static EntityType getByID(int id) {
			lock(ENTITY_TYPES) {
				foreach(EntityType et in ENTITY_TYPES) {
					if(et.id == id) return et;
				}
			}
			return null;
		}
		
		private static int getNextID() {
			int i = 0;
			foreach(EntityType et in ENTITY_TYPES) {
				i = Math.Max(i, et.getID());
			}
			return i + 1;
		}
		
		//Instances
		public static EntityType UNKNOWN = new EntityType(0, "Unknown", typeof(Entity));
		
		//Instance
		private int id;
		private String name;
		private Type type;
		
		public EntityType (int id, String name, Type type) {
			this.id = id;
			this.name = name;
			this.type = type;
			
			lock(ENTITY_TYPES) {
				ENTITY_TYPES.Add(this);
			}
		}
		
		public EntityType (String name, Type type) : this(getNextID(), name, type) {
			
		}
		
		public int getID() {return this.id;}
		public String getName() {return this.name;}
		public Type getType() {return this.type;}
		
		public Entity createNewInstance(Scene scene) {
			System.Reflection.ConstructorInfo ci = this.type.GetConstructor(new Type[] {typeof(Scene)});
			Object instance = ci.Invoke(new Object[]{scene});
			return (Entity) instance;
		}
	}
}

