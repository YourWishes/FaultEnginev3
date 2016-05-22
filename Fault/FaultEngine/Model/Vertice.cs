using System;

using Sce.PlayStation.Core;

namespace Fault {
	public class Vertice {
		private double x;
		private double y;
		private double z;
		
		public Vertice (double x=0, double y=0, double z=0) {
			this.x = x;
			this.y = y;
			this.z = z;
		}
		
		public Vertice(Vertice v) : this() {
			this.x = v.x;
			this.y = v.y;
			this.z = v.z;
		}
		
		public Vertice(Location l) : this() {
			this.x = l.getX();
			this.y = l.getY();
			this.z = l.getZ();
		}
		
		public double getX() {return this.x;}
		public double getY() {return this.y;}
		public double getZ() {return this.z;}
		
		public Vertice setX(double x) {this.x = x; return this;}
		public Vertice setY(double y) {this.y = y; return this;}
		public Vertice setZ(double z) {this.z = z; return this;}
		
		public Vertice set(double x, double y, double z) {this.x = x; this.y = y; this.z = z; return this;}
		
		public Vector3 getAsVector() {return new Vector3((float)this.x, (float)this.y, (float)this.z);}
		
		public Vertice clone() {
			return new Vertice(this);
		}
	}
}

