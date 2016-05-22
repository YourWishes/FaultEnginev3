using System;

namespace Fault {
	public class TimeUtils {
		public static DateTime getNowDateTime() {
			return DateTime.Now;
		}
		
		public static double getNow() {
			double epoch = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
			return epoch;
		}
	}
}

