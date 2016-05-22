using System;
using System.Collections.Generic;

namespace Fault {
	public class LoadedAnimationRules {
		private String name;
		private List<AnimationRule> rules;
		
		public LoadedAnimationRules (String name, List<AnimationRule> rules) {
			this.name = name;
			this.rules = rules;
		}
		
		public String getName() {return this.name;}
		public List<AnimationRule> getRules() {return new List<AnimationRule>(this.rules);}
	}
}

