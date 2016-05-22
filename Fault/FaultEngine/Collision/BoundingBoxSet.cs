using System;
using System.Collections.Generic;

namespace Fault {
	public class BoundingBoxSet : Bounding {
		private List<BoundingBox> boxes;
		
		public BoundingBoxSet (Model model) : base(model) {
			this.boxes = new List<BoundingBox>();
		}
		
		public BoundingBoxSet (BoundingBoxSet bbset) : base(bbset.getModel()) {
			this.boxes = new List<BoundingBox>();
			foreach(BoundingBox bb in bbset.getBoxes()) {
				addBox((BoundingBox)bb.clone());
			}
		}
		
		public List<BoundingBox> getBoxes() {return new List<BoundingBox>(this.boxes);}
		
		public void addBox(BoundingBox box) {this.boxes.Add(box);}
		public void removeBox(BoundingBox box) {this.boxes.Remove(box);}
		
		public override void setModel (Model m) {
			base.setModel (m);
			foreach(BoundingBox bb in this.boxes) {
				bb.setModel(m);
			}
		}
		
		public override Bounding clone() {
			return new BoundingBoxSet(this);
		}
	}
}

