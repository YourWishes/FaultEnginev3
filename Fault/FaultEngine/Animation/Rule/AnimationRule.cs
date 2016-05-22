using System;

namespace Fault {
	public class AnimationRule {
		private String name;
		private String object_id;
		private AnimationFramingType anim_type;
		private Location target;
		private bool moveRelative;
		private int frame;
		private Location startPos;
		private int start;
		private int curve;
		
		public Location originalLocation;
		
		public AnimationRule (String object_id, AnimationFramingType animation_type, Location target, bool moveRelative, int frame, Location startPos = null, int startFrame = 0, int tweenCurve = 0) {
			this.object_id = object_id;
			this.anim_type = animation_type;
			this.target = target;
			this.moveRelative = moveRelative;
			this.frame = frame;
			this.startPos = startPos;
			this.start = startFrame;
			this.curve = tweenCurve;
		}
		
		public AnimationRule(AnimationRule rule) : this(
			rule.object_id,
			rule.anim_type,
			(rule.target == null ? null : rule.target.clone()),
			rule.moveRelative,
			rule.frame,
			(rule.startPos == null ? null : rule.startPos.clone()),
			rule.start,
			rule.curve
			) {
			
		}
		
		public String getName() {return this.name;}
		public bool isRelative() {return this.moveRelative;}
		public String getObjectID() {return this.object_id;}
		public AnimationFramingType getAnimationType() {return this.anim_type;}
		public Location getTarget() {return this.target;}
		public int getFrame() {return this.frame;}
		public Location getStartingPosition() {return startPos;}
		public int getStartFrame() {return this.start;}
		public int getTweenCurve() {return this.curve;}
		
		public void setName(String name) {this.name = name;}
		
		public AnimationRule clone() {return new AnimationRule(this);}
	}
}

