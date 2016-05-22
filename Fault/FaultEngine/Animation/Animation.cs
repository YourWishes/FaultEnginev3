using System;
using System.Collections.Generic;

namespace Fault {
	public class Animation : IDisposable {
		private int frame;
		private Entity entity;
		private List<AnimationRule> animationRules;
		private bool loop = true;
		
		private int nextGetObjectID = 0;
		
		public Animation (Entity entity) {
			this.frame = 0;
			this.animationRules = new List<AnimationRule>();
			this.entity = entity;
		}
		
		public Entity getEntity() {return this.entity;}
		public Location getLocation() {return this.entity.getLocation();}
		public Location getVelocity() {return this.entity.getVelocity();}
		public int getFrame() {return this.frame;}
		public List<AnimationRule> getAnimationRules() {return new List<AnimationRule>(this.animationRules);}
		public int getNextGetObjectID() {
			int q = this.nextGetObjectID;
			this.nextGetObjectID ++;
			return q;
		}
		public virtual Locateable getObject(String id) {
			if(this.getEntity() == null) return null;
			if(id == "this") return this.getEntity();
			return null;
		}
		public int getTotalFrames() {
			int total = 0;
			foreach(AnimationRule ar in this.getAnimationRules()) {
				if(ar.getAnimationType().Equals(AnimationFramingType.FRAME_BY_FRAME)) {
					total = Math.Max(total, ar.getFrame());
				} else if(ar.getAnimationType().Equals(AnimationFramingType.TWEEN)) {
					total = Math.Max(total, ar.getStartFrame() + ar.getFrame());
				}
			}
			return total;
		}
		
		public void addAnimationRule(AnimationRule animRule) {this.animationRules.Add(animRule);}
		public void removeAnimationRule(AnimationRule animRule) {this.animationRules.Remove(animRule);}
		
		public virtual void Dispose() {
			if(this.entity != null) this.entity.removeAnimation(this);
			foreach(AnimationRule rule in this.getAnimationRules()) {
				Locateable obj = this.getObject(rule.getObjectID());
				if(obj == null) continue;
				if(rule.isRelative()) obj.getLocation().set(rule.originalLocation);
			}
		}
		
		public virtual void tick() {
			foreach(AnimationRule rule in this.getAnimationRules()) {
				lock(rule) {
					Locateable obj = this.getObject(rule.getObjectID());
					if(obj == null) continue;
					if(rule.getAnimationType().Equals(AnimationFramingType.FRAME_BY_FRAME)) {
						if(this.getFrame() != rule.getFrame()) continue;
						obj.getLocation().set(rule.getTarget());
					} else if(rule.getAnimationType().Equals(AnimationFramingType.TWEEN)) {
						if(frame < rule.getStartFrame()) continue;
						if(frame > (rule.getStartFrame() + rule.getFrame())) continue;
						Location target = rule.getTarget().clone();
						target.sub(rule.getStartingPosition());
						
						double amt = 0;
						int tweenFrame = frame - rule.getStartFrame();
						double tf = (double) tweenFrame;
						
						double animation_complete_percent = (double)tweenFrame / (double)(rule.getFrame());
						double curve = (double)rule.getTweenCurve()/100.0d;
						double curve_addition_offset = curve * (1-animation_complete_percent);
						
						amt = tf / (double)rule.getFrame();
						
						target.multiply(amt);
						Location q = target.clone();
						q.multiply(curve_addition_offset);
						target.add(q);
						
						target.add (rule.getStartingPosition());
						if(rule.isRelative()) target.add(rule.originalLocation);
						
						obj.getLocation().set (target);
						
						//Target Dispose
						target.Dispose();
						q.Dispose();
					}
				}
			}
			frame++;
			if(frame > this.getTotalFrames()) {
				if(loop) {
					frame = 0;
				} else {
					this.Dispose();
				}
			}
		}
		
		public virtual void loadInData(String data) {
			String[] splitLines = data.Replace('\r', '\n').Split('\n');
			foreach(String d in splitLines) {
				addAnimationRule(loadInDataRule(d));
			}
		}
		
		public virtual AnimationRule loadInDataRule(String d) {
			String[] splitData = d.Split(' ');
			String object_id = splitData[0];
			AnimationFramingType type = AnimationFramingType.getAnimationTypeByID(splitData[1]);
			
			bool relative = false;
			String locationData = splitData[2];
			relative = locationData.StartsWith("r");
			if(relative) locationData = locationData.Replace("r", "");
			Location target = Location.ValueOf(locationData);
			
			int frame = 0;
			Location startingLocation = null;
			int start = 0;
			int curve = 0;
			if(splitData.Length > 3) frame = Convert.ToInt32(splitData[3]);
			if(splitData.Length > 4) startingLocation = Location.ValueOf(splitData[4]);
			if(splitData.Length > 5) start = Convert.ToInt32(splitData[5]);
			if(splitData.Length > 6) curve = Convert.ToInt32(splitData[6]);
			
			AnimationRule rule = new AnimationRule(object_id, type, target, relative, frame, startingLocation, start, curve);
			
			if(this.getObject(object_id) != null) {
				rule.originalLocation = this.getObject(object_id).getLocation().clone ();
			}
			
			return rule;
		}
	}
}

