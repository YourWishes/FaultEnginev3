using System;
using System.Collections.Generic;

//TODO: Finish
namespace Fault {
	public class Language {
		private static List<Language> LANGUAGES = new List<Language>();
		
		//Regions
		public static Language ENGLISH = new Language(Region.ENGLISH);
		
		//Static
		public static Language getCurrentLanguage() {
			return ENGLISH;
		}
		
		public static Language getByRegion(Region region) {
			lock(LANGUAGES) {
				foreach(Language l in LANGUAGES) {
					if(l == null || l.region == null) continue;
					if(!l.region.Equals(region)) continue;
					return l;
				}
			}
			return null;
		}
		
		private static void loadLanguage(Region region, Language l) {
			if(region.Equals(Region.ENGLISH)) loadEnglish(l);
		}
		
		/** * English * **/
		private static void loadEnglish(Language l) {
		}
		
		//Instance
		private Region region;
		private List<LanguageObject> languageObjects;
		
		private Language (Region region) : base() {
			this.region = region;
			this.languageObjects = new List<LanguageObject>();
			
			loadLanguage(region, this);
			
			lock(LANGUAGES) {
				LANGUAGES.Add(this);
			}
		}
		
		public List<LanguageObject> getLanguageObjects() {lock(this.languageObjects) {return new List<LanguageObject>(this.languageObjects);}}
		public Region getRegion() {return this.region;}
		
		public LanguageObject getObject(String key) {
			lock(this.languageObjects) {
				foreach(LanguageObject lo in this.languageObjects) {
					if(lo.getKey().ToLower().Equals(key.ToLower())) return lo;
				}
			}
			return null;
		}
		
		public String getMessage(String key) {return getObject(key).getMessage();}
		
		public void addLanguageObject(LanguageObject lo) {lock(languageObjects) {this.languageObjects.Add(lo);}}
		
		public void removeLanguageObject(LanguageObject lo) {lock(languageObjects) {this.languageObjects.Remove(lo);}}
	}
}

