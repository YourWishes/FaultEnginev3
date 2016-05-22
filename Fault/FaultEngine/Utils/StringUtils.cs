using System;
using System.Text.RegularExpressions;

namespace Fault {
	public class StringUtils {
		public static String ReplaceFirst(String what, String with, String inThis) {
			var regex = new Regex(Regex.Escape(what));
			return regex.Replace(inThis, with, 1);
		}
		
		public static String join(String[] what, String seperator = " ") {
			String rv = "";
			for(int i = 0; i < what.Length; i++) {
				rv += what[i];
				if(i < (what.Length - 1)) {
					rv += seperator;
				}
			}
			return rv;
		}
	}
}

