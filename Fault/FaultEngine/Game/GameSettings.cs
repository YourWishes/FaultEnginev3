using System;
using System.Collections.Generic;

namespace Fault {
	public class GameSettings {
		private static GameSettings GAME_SETTINGS;
		public static GameSettings getSettings() {
			if(GAME_SETTINGS == null) GAME_SETTINGS = new GameSettings();
			return GAME_SETTINGS;
		}
		
		private List<SettingValue> values = new List<SettingValue>();
		
		private GameSettings () {
		}
		
		public SettingValue getSetting(String key, Object defaultValue) {
			lock(values) {
				foreach(SettingValue sv in values) {
					if(sv.getKey().ToLower().Equals(key.ToLower())) return sv;
				}
				SettingValue sval = new SettingValue(key, defaultValue);
				values.Add(sval);
				return sval;
			}
		}
		
		public SettingValue setValue(String key, Object val) {
			return getSetting(key, val).setValue(val);
		}
	}
}

