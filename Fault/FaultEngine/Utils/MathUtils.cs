using System;

namespace Fault {
	public class MathUtils {
		public static double toDegrees(double rads) {
			return rads * (180.0 / Math.PI);
		}
		
		public static double toRadians(double degs) {
			return (degs / 180.0) * Math.PI;
		}
		
		public static double randomNumber(double range=double.MaxValue) {
			Random r  = new Random();
			//Generate the seed
			double seed = ((r.NextDouble() * 2.0 - 1.0) * range);
			r = null;
			return seed;
		}
	}
}

