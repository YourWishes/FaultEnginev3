using System;
using System.Collections.Generic;

namespace Fault {
	public class Bounding : IDisposable {
		public static bool isHitting(BoundingBox box1, BoundingBox box2) {
			Location box1RefinedMin = box1.getRefinedMinPoint();
			Location box1RefinedMax = box1.getRefinedMaxPoint();
			Location box2RefinedMin = box2.getRefinedMinPoint();
			Location box2RefinedMax = box2.getRefinedMaxPoint();
			
			Location box1Min = box1RefinedMin.getAbsoluteLocation();
			Location box1Max = box1RefinedMax.getAbsoluteLocation();
			Location box2Min = box2RefinedMin.getAbsoluteLocation();
			Location box2Max = box2RefinedMax.getAbsoluteLocation();
			
	        double myMinX = box1Min.getX();
	        double myMinY = box1Min.getY();
	        double myMinZ = box1Min.getZ();
	        
	        double myMaxX = box1Max.getX();
	        double myMaxY = box1Max.getY();
	        double myMaxZ = box1Max.getZ();
	        
	        double notMinX = box2Min.getX();
	        double notMinY = box2Min.getY();
	        double notMinZ = box2Min.getZ();
	        
	        double notMaxX = box2Max.getX();
	        double notMaxY = box2Max.getY();
	        double notMaxZ = box2Max.getZ();
			
			box1RefinedMin.Dispose();
			box1RefinedMax.Dispose();
			box2RefinedMin.Dispose();
			box2RefinedMax.Dispose();
			
			box1Min.Dispose();
			box1Max.Dispose();
			box2Min.Dispose();
			box2Max.Dispose();
			
	        bool myXInside = (myMinX >= notMinX && myMinX <= notMaxX) || (myMaxX >= notMinX && myMaxX <= notMaxX);
	        bool myYInside = (myMinY >= notMinY && myMinY <= notMaxY) || (myMaxY >= notMinY && myMaxY <= notMaxY);
	        bool myZinside = (myMinZ >= notMinZ && myMinZ <= notMaxZ) || (myMaxZ >= notMinZ && myMaxZ <= notMaxZ);
	        if(myXInside && myYInside && myZinside) return true;
	        
	        bool notXInside = (notMinX >= myMinX && notMinX <= myMaxX) || (notMaxX >= myMinX && notMaxX <= myMaxX);
	        bool notYInside = (notMinY >= myMinY && notMinY <= myMaxY) || (notMaxY >= myMinY && notMaxY <= myMaxY);
	        bool notZInside = (notMinZ >= myMinZ && notMinZ <= myMaxZ) || (notMaxZ >= myMinZ && notMaxZ <= myMaxZ);
	        if(notXInside && notYInside && notZInside) return true;
			return false;
		}
		
		public static bool isHitting(BoundingBoxSet boxset, BoundingBox hittingbox) {
			foreach(BoundingBox box in boxset.getBoxes()) {
				bool t = box.isHitting(hittingbox);
				if(!t) continue;
				return t;
			}
			return false;
		}
		
		public static bool isHitting(BoundingBoxSet boxset1, BoundingBoxSet boxset2) {
			foreach(BoundingBox box in boxset1.getBoxes()) {
				bool t = box.isHitting(boxset2);
				if(!t) continue;
				return t;
			}
			return false;
		}
		
		//Instance
		private Model model;
		
		private Entity entity;
		
		public Bounding (Model model) {
			this.model = model;
		}
		
		public Bounding(Bounding b) : this(b.model) {
			
		}
		
		public Model getModel() {return this.model;}
		public Entity getEntity() {return this.entity;}
		
		public virtual void setModel(Model m) {this.model = m;}
		public void setEntity(Entity e) {this.entity = e;}
		
		//Hit Methods
		public virtual bool isHitting(Bounding b) {
			if(b == null) return false;
			if(b.Equals(this)) return false;
		
			if((this is BoundingBox) && (b is BoundingBox)) {
				return isHitting((BoundingBox) this, (BoundingBox) b);
			}
			
			if((this is BoundingBox) && (b is BoundingBoxSet)) {
				return isHitting((BoundingBoxSet) b, (BoundingBox) this);
			}
			
			if((this is BoundingBoxSet) && (b is BoundingBox)) {
				return isHitting((BoundingBoxSet)this, (BoundingBox)b);
			}
			
			if((this is BoundingBoxSet) && (b is BoundingBoxSet)) {
				return isHitting((BoundingBoxSet) this, (BoundingBoxSet) b);
			}
			
			return false;
		}
		
		public bool isHitting(Model m) {
			return isHitting(m.getBounds());
		}
		
		public bool isHitting(Entity e) {
			return isHitting(e.getBounds());
     	}
		
		public List<Model> getHittingModels() {
			List<Model> models = new List<Model>();
			foreach(Model m in Scene.getActiveScene().getModels()) {
				if(m == null) continue;
				if(!isHitting(m)) continue;
				models.Add(m);
			}
			return models;
		}
		
		public List<Entity> getHittingEntities() {
			List<Entity> entities = new List<Entity>();
			if(Scene.getActiveScene() == null) return entities;
			foreach(Entity e in Scene.getActiveScene().getEntities()) {
				if(e == null) continue;
				if(!isHitting(e)) continue;
				entities.Add(e);
			}
			return entities;
		}
		
		//Extra Methods
		public virtual void Dispose () {
		}
		
		public virtual Bounding clone() {
			return new Bounding(this);
		}
	}
}

