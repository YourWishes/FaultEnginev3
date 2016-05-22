using System;
using System.Collections.Generic;

namespace Fault {
	public class Resource {
		private ResourceType type;
		private String name;
		private String fileName;
		private Object val;
		
		public Resource (ResourceType type, String name, String fileName, Object val) {
			this.type = type;
			this.name = name;
			this.fileName = fileName;
			this.val = val;
		}
		
		public ResourceType getType() {return this.type;}
		public String getName() {return this.name;}
		public String getFileName() {return this.fileName;}
		public Object getValue() {return this.val;}
		
		public List<Model> getAsModelList() {
			return (List<Model>) this.val;
		}
	}
}

