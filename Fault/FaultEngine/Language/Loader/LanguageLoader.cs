using System;
using System.Collections.Generic;
using System.Xml;

namespace Fault {
	public class LanguageLoader {
		private static LanguageLoader LOADER = null;
		public static LanguageLoader getLoader() {
			if(LOADER != null) return LOADER;
			return LOADER = new LanguageLoader();
		}
		
		private LanguageLoader () {
		}
		
		public List<LanguageObject> loadLanguage(XmlReader xmlReader) {
			List<LanguageObject> los = new List<LanguageObject>();
			while(xmlReader.Read()) {
				String type = xmlReader.Name;
				if(type.ToLower().Equals("message")) {
					String key = xmlReader["key"];
					String message = xmlReader["message"];
					los.Add(new LanguageObject(key, message));
				}
			}
			return los;
		}
	}
}

