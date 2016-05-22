using System;

namespace Fault {
	public abstract class GUIElement : IDisposable, Locateable {
		private GUI gui;
		private Location location;
		
		public GUIElement (GUI gui) {
			this.gui = gui;
			this.location = new Location();
			this.location.setOwner(this);
		}
		
		public GUI getGUI() {return this.gui;}
		public Location getLocation() {return this.location;}
		Location Locateable.getLocation() {return getLocation();}
		Locateable Locateable.getParent() {return getGUI();}
		public abstract double getWidth();
		public abstract double getHeight();
		public abstract Color getColor();
		public void centerX() {this.getLocation().setX(Camera.getCamera().getWidth()/2.0 - this.getWidth()/2.0);}
		public void centerY() {this.getLocation().setY(Camera.getCamera().getHeight()/2.0 - this.getHeight()/2.0);}
		public void center() {this.centerX(); this.centerY();}
		public void seventyFivePercentY() {this.getLocation().setY(Camera.getCamera().getHeight() * 0.75 - this.getHeight() / 2.0);}
		
		public virtual void tick() {
			//TODO: Add Logic
		}
		
		public virtual void Dispose() {
			if(this.location != null) this.location.Dispose();
			this.getGUI().removeElement(this);
			
			this.location = null;
		}
		
		public virtual void onGUIShow() {}
		public virtual void onGUIHide() {}
	}
}

