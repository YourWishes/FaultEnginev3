using System;
using System.Collections.Generic;

namespace Fault {
	public class Entity : Locateable, IDisposable {
		private EntityType type;
		private Model model = null;
		private Controller controller = null;
		private List<Animation> animations;
		private Scene scene;
		private double weight = 50.0;
		private bool onFloor = false;
		
		
		private Location velocity;
		
		private Location entityLocation;
		
		public Entity (Scene scene, EntityType type) {
			this.animations = new List<Animation>();
			this.scene = scene;
			this.type = type;
			
			this.velocity =  new Location(this);
		}
		
		public EntityType getType() {return this.type;}
		public Model getModel() {return this.model;}
		public Controller getController() {return this.controller;}
		public List<Animation> getAnimations() {lock(this.animations) {return new List<Animation>(this.animations);}}
		public Scene getScene() {return this.scene;}
		public Location getLocation() {if(this.hasModel()) return this.getModel().getLocation(); return getEntityLocation();}
		public Location getEntityLocation() {return (this.entityLocation != null ? this.entityLocation : this.entityLocation = new Location());}
		public double getWeight() {return this.weight;}
		public Location getVelocity() {return this.velocity;}
		public Bounding getBounds() {return (model == null ? null : model.getBounds());}
		Location Locateable.getLocation() {return getModel().getLocation();}
		Locateable Locateable.getParent() {return getModel().getParent();}
		public Game getGame() {return Game.GAME_INSTANCE;}
		
		public void setModel(Model m) {this.model = m;}
		public void setController(Controller c) {this.controller = c;}
		public void setScene(Scene scene) {this.scene = scene;}
		public void setWeight(double w) {this.weight = w;}
		public void setIsOnFloor(bool t) {this.onFloor = t;}
		
		public bool hasModel() {return this.model != null;}
		public bool hasController() {return this.controller != null;}
		
		public void addAnimation(Animation animation) {lock(this.animations) {this.animations.Add(animation);}}
		
		public void removeAnimation(Animation animation) {lock(this.animations) {this.animations.Remove(animation);}}
		
		public virtual void Dispose() {
			foreach(Animation a in this.getAnimations()) {
				a.Dispose();
			}
			
			velocity.Dispose();
			if(controller != null) controller.Dispose();
			scene.removeEntity(this);
			
			this.model.Dispose();
			
			scene = null;
			model = null;
			controller = null;
			velocity = null;
		}
		
		public virtual void tick() {
			if(this.hasController()) this.getController().tick();
			foreach(Animation anim in this.getAnimations()) {
				anim.tick();
			}
		}
		
		//TODO: Finish
		public bool isOnFloor() {return this.onFloor;}
	}
}

