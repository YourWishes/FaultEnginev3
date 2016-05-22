using System;

namespace Fault {
	public class Light : Locateable, IDisposable {
		private Location location;
		private Color color;
		private float intensity;
		
		public Light () {
			this.location = new Location();
			this.location.setOwner(this);
			
			this.color = new Color(1.0,1.0,1.0,1.0);
			this.intensity = 1;
		}
		
		public Location getLocation() {return this.location;}
		Location Locateable.getLocation() {return getLocation();}
		Locateable Locateable.getParent() {return null;}		
		public Color getColor() {return this.color;}
		public float getIntensity() {return this.intensity;}
		
		public void setColor(Color c) {this.color = c;}
		public void setIntensity(float i) {this.intensity = i;}
		
		public void Dispose() {
			this.location.Dispose();
			
			this.location = null;
			this.color = null;
		}
	}
}

