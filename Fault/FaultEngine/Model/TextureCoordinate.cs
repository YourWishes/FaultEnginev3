using System;

namespace Fault {
	public class TextureCoordinate {
		private double x;
		private double y;
		
		public TextureCoordinate () : this(0,0) {
		}
		
		public TextureCoordinate(double x, double y) {
			this.x = x;
			this.y = y;
		}
		
		public TextureCoordinate(TextureCoordinate tc) : this(tc.x, tc.y) {
			
		}
		
		public double getX() {return this.x;}
		public double getY() {return this.y;}
		
		public TextureCoordinate setX(double x) {this.x = x; return this;}
		public TextureCoordinate setY(double y) {this.y = y; return this;}
		
		public TextureCoordinate set(double x, double y) {this.x = x; this.y = y; return this;}
		
		public TextureCoordinate clone() {
			return new TextureCoordinate(this);
		}
	}
}

