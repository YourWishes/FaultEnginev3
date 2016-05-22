using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Fault {
	public class AnimationLoader {
		private static AnimationLoader instance;
		public static AnimationLoader getLoader() {
			if(instance != null) return instance;
			return instance = new AnimationLoader();
		}
		
		private AnimationLoader () {
		}
		
		public List<AnimationRule> loadAnimationRules(String data) {
			List<AnimationRule> rules = new List<AnimationRule>();
			String[] splitLines = data.Replace('\r', '\n').Split('\n');
			foreach(String d in splitLines) {
				AnimationRule ar = loadAnimationRule(d);
				if(ar == null) continue;
				rules.Add(ar);
			}
			return rules;
		}
		
		public AnimationRule loadAnimationRule(String data) {
			return null;
		}
	}
}

