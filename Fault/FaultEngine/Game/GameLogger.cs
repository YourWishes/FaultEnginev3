using System;

namespace Fault {
	public class GameLogger {
		private static GameLogger logger = null;
		public static GameLogger getLogger() {return logger;}
		
		//Instance
		public GameLogger () {
			logger = this;
		}
		
		public string getDatePrefix() {
			DateTime time = TimeUtils.getNowDateTime();
			
			return time.ToString("[yyyy-MM-dd HH:mm:ss]");
		}
		
		public void log(Object o) {
			this.log(o.ToString());
		}
		
		public void log(string str) {
			Console.Out.WriteLine(getDatePrefix() + " " + str);
		}
	}
}

