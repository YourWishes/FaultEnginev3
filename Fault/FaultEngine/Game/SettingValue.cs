using System;
using Sce.PlayStation.Core.Input;

namespace Fault {
	public class SettingValue {
		private String key;
		private Object val;
		
		public SettingValue (String key) : this(key, null) {
		}
		
		public SettingValue (String key, Object val) {
			this.key = key;
			this.val = val;
		}
		
		public String getKey() {return this.key;}
		public Object getValue() {return this.val;}
		
		public SettingValue setValue(Object val) {this.val = val; return this;}
		
		//Conversions
		public String getValueAsString() {return this.getValue().ToString();}
		public double getValueAsDouble() {return Convert.ToDouble(val.ToString());}
		public GamePadButtons getAsGamePadButtons() {return (GamePadButtons) this.val;}
	}
}

