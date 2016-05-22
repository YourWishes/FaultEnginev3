using System;

using Sce.PlayStation.Core;

namespace Fault {
	public class Color {
	    public static Color BLACK = Color.fromHex("#000000");
	    public static Color BLUE = Color.fromHex("#0000FF");
	    public static Color GREEN = Color.fromHex("#00FF00");
	    public static Color WHITE = Color.fromHex("#FFFFFF");
	    public static Color PINK = Color.fromHex("#FF00FF");
	    public static Color GRAY = Color.fromHex("#CCCCCC");
	    public static Color RED = Color.fromHex("#FF0000");
	    public static Color CORNFLOWER_BLUE = Color.fromHex("#6495ED");
	    public static Color YELLOW = Color.fromHex("#FFFF00");
	    public static Color ORANGE = Color.fromHex("#E8A84F");
	    public static Color DARK_GRAY = Color.fromHex("#333333");
	    public static Color DARK_GREEN = Color.fromHex("#00750C");
	    public static Color CYAN = Color.fromHex("#00FFFF");
		public static Color GOLD = Color.fromHex("#FFD700");
		public static Color SKY_BLUE = Color.fromHex("#87CEEB");
		public static Color TREE_TRUNK_BROWN = Color.fromHex("#53350A");
		public static Color SAND_YELLOW = Color.fromHex("#EFDD6F");
		public static Color DARK_BROWN = Color.fromHex("#4D3100");
		public static Color DARK_BLUE = Color.fromHex("#000080");
		
		public static Color fromHex(string hex) {
			if(!hex.StartsWith("#")) hex = "#" + hex;
			hex = hex.ToUpper();
			int r = Convert.ToInt32(hex.Substring(1,2),16);
			int g = Convert.ToInt32(hex.Substring(3,2),16);
			int b = Convert.ToInt32(hex.Substring(5,2),16);
			var color = new Color(r,g,b,100);
			return color;
		}
		
		public double r;
		public double g;
		public double b;
		public double a;
		
		public Color (int r=0, int g=0, int b=0, int a=0) : this((double) r/255.0, (double) g/255.0, (double) b/255.0, (double)a/100.0) {
		}
		
		public Color(Color color) : this(color.r, color.g, color.b, color.a) {
		}
		
		public Color (double r=0, double g=0, double b=0, double a=0) {
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}
		
		public double getRed() {return this.r;}
		public double getGreen() {return this.g;}
		public double getBlue() {return this.b;}
		public double getAlpha() {return this.a;}
		
		public Color setRed(double r) {this.r = r; return this;}
		public Color setGreen(double g) {this.g = g; return this;}
		public Color setBlue(double b) {this.b = b; return this;}
		public Color setAlpha(double a) {this.a = a; return this;}
		
		public Color clone() {
			return new Color(this);
		}
		
		public Vector4 getAsVector() {
			return new Vector4((float)this.r, (float)this.g, (float)this.b, (float)this.a);
		}
		
		public Vector3 getAsVector3() {
			return new Vector3((float)this.r, (float)this.g, (float)this.b);
		}
		
		public Color multiplyByAlpha() {
			this.r = this.r * this.a;
			this.g = this.g * this.a;
			this.b = this.b * this.a;
			return this;
		}
	}
}

