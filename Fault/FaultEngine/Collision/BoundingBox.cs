using System;

namespace Fault {
	public class BoundingBox : Bounding {
		public static BoundingBox calculateBox(Entity e) {
			return calculateBox(e.getModel());
		}
		
		public static BoundingBox calculateBox(Model m) {
			Location[] locations = m.getAbsoluteMinMaxPoints();
			
			Location min = locations[0];
			Location max = locations[1];
			
			BoundingBox box = new BoundingBox(min, max, m);
			return box;
		}
		
		//Instance
		private Location min;
		private Location max;
		
		public BoundingBox (Location min, Location max, Model model) : base(model) {
			this.min = min;
			this.max = max;
			
			this.min.setOwner(this.getModel().getLocation());
			this.max.setOwner(this.getModel().getLocation());
		}
		
		public BoundingBox(BoundingBox box) : this(box.min.clone(), box.max.clone(), box.getModel()) {
			
		}
		
		public Location getMinPoint() {return this.min;}
		public Location getMaxPoint() {return this.max;}
		
		public void scale(double amt) {
			min.setX(min.getX() * amt).setY(min.getY() * amt).setZ(min.getZ() * amt);
			max.setX(max.getX() * amt).setY(max.getY() * amt).setZ(max.getZ() * amt);
		}
		
		public Location getRefinedMinPoint() {
			Location min = this.min.clone();
			min.setX(min.getX() * this.getModel().getScaleX());
			min.setY(min.getY() * this.getModel().getScaleY());
			min.setZ(min.getZ() * this.getModel().getScaleZ());
			min.setOwner(this.getModel().getLocation());
			return min;
		}
		
		public Location getRefinedMaxPoint() {
			Location max = this.max.clone();
			max.setX(max.getX() * this.getModel().getScaleX());
			max.setY(max.getY() * this.getModel().getScaleY());
			max.setZ(max.getZ() * this.getModel().getScaleZ());
			max.setOwner(this.getModel().getLocation());
			return max;
		}
		
		public override void setModel (Model m) {
			base.setModel(m);
			min.setOwner(m.getLocation());
			max.setOwner(m.getLocation());
		}
		
		public override Bounding clone() {
			return new BoundingBox(this);
		}
		
		public override void Dispose () {
			base.Dispose ();
			min.Dispose();
			max.Dispose();
		}
	}
}

