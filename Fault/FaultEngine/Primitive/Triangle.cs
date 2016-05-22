using System;

namespace Fault {
	public class Triangle : Face {
	    private Vertice v0;
	    private Vertice v1;
	    private Vertice v2;
	    
	    private TextureCoordinate t0;
	    private TextureCoordinate t1;
	    private TextureCoordinate t2;
    
		public Triangle () : base() {
	        //Right Angle Triangle /|
	        Vertice bottomLeft = new Vertice();
	        Vertice bottomRight = new Vertice();
	        Vertice topRight = new Vertice();
	        
	        bottomLeft.set(-0.5f, -0.5f, 0);
	        bottomRight.set(0.5f, -0.5f, 0);
	        topRight.set(0.5f, 0.5f, 0);
	        
	        this.v0 = bottomLeft;
	        this.v1 = bottomRight;
	        this.v2 = topRight;
	        
	        TextureCoordinate bottomLeftT = new TextureCoordinate();
	        TextureCoordinate bottomRightT = new TextureCoordinate();
	        TextureCoordinate topRightT = new TextureCoordinate();
	        
	        bottomLeftT.set(0.0f, 0.0f);
	        bottomRightT.set(1.0f, 0.0f);
	        topRightT.set(1.0f, 1.0f);
	        
	        this.t0 = bottomLeftT;
	        this.t1 = bottomRightT;
	        this.t2 = topRightT;
	        
	        this.reset();
		}
		
		private Triangle(Triangle aThis) : base(aThis) {
	        this.v0 = aThis.v0.clone();
	        this.v1 = aThis.v1.clone();
	        this.v2 = aThis.v2.clone();
	        
	        this.t0 = aThis.t0.clone();
	        this.t1 = aThis.t1.clone();
	        this.t2 = aThis.t2.clone();
	        this.reset();
		}
    
	    public Vertice getVert0() {return this.v0;}
	    public Vertice getVert1() {return this.v1;}
	    public Vertice getVert2() {return this.v2;}
	    
	    public TextureCoordinate getTextureCoordinate0() {return this.t0;}
	    public TextureCoordinate getTextureCoordinate1() {return this.t1;}
	    public TextureCoordinate getTextureCoordinate2() {return this.t2;}
	    
	    public void setVert0(Vertice v) {
	        this.v0 = v;
	        this.reset();
	    }
	    public void setVert1(Vertice v) {
	        this.v1 = v;
	        this.reset();
	    }
	    public void setVert2(Vertice v) {
	        this.v2 = v;
	        this.reset();
	    }
	    
	    public void setTextureCoordinate0(TextureCoordinate v) {
	        this.t0 = v;
	        this.reset();
	    }
	    public void setTextureCoordinate1(TextureCoordinate v) {
	        this.t1 = v;
	        this.reset();
	    }
	    public void setTextureCoordinate2(TextureCoordinate v) {
	        this.t2 = v;
	        this.reset();
	    }
	    
	    public override Face clone() {
	        return new Triangle(this);
	    }
	    
	    private void reset() {
	        foreach(Vertice v in this.getVertices()) {
	            this.removeVertice(v);
	        }
	        foreach(TextureCoordinate v in this.getTextureCoordinates()) {
	            this.removeTextureCoordinate(v);
	        }
	        
	        this.addVertice(v0);
	        this.addVertice(v1);
	        this.addVertice(v2);
	        
	        this.addTextureCoordinate(t0);
	        this.addTextureCoordinate(t1);
	        this.addTextureCoordinate(t2);
	        
	    }
	}
}

