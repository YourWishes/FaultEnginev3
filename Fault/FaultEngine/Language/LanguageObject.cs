using System;

namespace Fault {
	public class LanguageObject {
		private String key;
		private String message;
		
		public LanguageObject (String key, String message="") {
			this.key = key;
			this.message = message;
		}
		
		public String getKey() {return this.key;}
		public String getMessage() {return this.message;}
		
		public void setMessage(String message) {this.message = message;}
	}
}

