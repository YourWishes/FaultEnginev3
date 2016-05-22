using System;

namespace Fault {
	public class LivingEntity : Entity, InventoryHolder, Talkable {
		private Location eyeTarget;
		private Entity target;
		private Inventory inventory;
		
		private double health = 20;
		private double maxHealth = 20;
		
		public LivingEntity (Scene scene, EntityType type) : base(scene, type) {
			this.eyeTarget = new Location();
			this.inventory = new Inventory(this);
		}
		
		public Location getEyeTarget() {return this.eyeTarget;}
		public Entity getTarget() {return (this.hasTarget() ? this.target : this.setTarget(null));}
		public double getHealth() {return this.health;}
		public double getMaxHealth() {return this.maxHealth;}
		public virtual Location getEyeLocation() {return this.getLocation();}
		public Inventory getInventory() {return this.inventory;}
		Inventory InventoryHolder.getInventory() {return getInventory();}
		
		public void setEyeTarget(Location l) {this.getEyeTarget().set (l);}
		public Entity setTarget(Entity target) {return this.target = target;}
		public void setHealth(double health) {this.health = health;}
		public void setMaxHealth(double maxHealth) {this.maxHealth = maxHealth;}
		
		public bool isAlive() {return this.health > 0;}
		public bool hasTarget() {return this.target != null && (this.target is LivingEntity ? ((LivingEntity) this.target).isAlive() : true);}
		
		public bool isHostile(LivingEntity against) {
			//TODO: Finish hostility
			return true;
		}
		
		public void kill() {this.setHealth(0);}
		public void kill(DeathCause cause) {this.kill();}
		
		public override void Dispose() {
			base.Dispose();
			eyeTarget.Dispose();
			
			eyeTarget = null;
			target = null;
		}
		
		public virtual void onMessageComplete (ChatMessage message) {
		}
		
		public virtual void onMessageDisplay (ChatMessage message) {
		}
	}
}

