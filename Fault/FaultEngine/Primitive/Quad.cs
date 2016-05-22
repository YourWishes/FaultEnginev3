using System;
using System.Threading;

namespace Fault {
	public class Quad : Model {
	    private static Quad QUAD_MESH = null;
	    private static Quad getMesh() {
	        if(QUAD_MESH != null) return QUAD_MESH;
	        QUAD_MESH = new Quad(true);
			QUAD_MESH.addToVBOList();
			if(Thread.CurrentThread != Game.GAME_INSTANCE.getMainThread()) while(!QUAD_MESH.isOnVBO()) {}
	        return QUAD_MESH;
	    }
	    public static Triangle[] createQuad() {
	        Quad q = new Quad();
	        return q.toQuadArray();
	    }
		
		//Instance
	    private Triangle bottomLeft;
	    private Triangle topRight;
	    private Triangle[] quadArray = null;
		
		public Quad () : this(getMesh()) {
			
		}
		
		private Quad(Quad m) : base(m) {
	        foreach(Face f in this.getFaces()) {
	            this.removeFace(f);
	        }
			
	        this.bottomLeft = (Triangle)m.bottomLeft.clone();
	        this.addFace(bottomLeft);
	        
	        this.topRight = (Triangle)m.topRight.clone();
	        this.addFace(topRight);
	        
	        topRight.setVert0(bottomLeft.getVert0());
	        topRight.setVert1(bottomLeft.getVert2());
	        topRight.getVert2().set(-0.5f, 0.5f, 0.0f);
	        
	        topRight.setTextureCoordinate0(bottomLeft.getTextureCoordinate0());
	        topRight.setTextureCoordinate1(bottomLeft.getTextureCoordinate2());
	        topRight.getTextureCoordinate2().set(0.0f, 1.0f);
	        
	        this.cloneMaterial();
		}
		
		private Quad(bool t) : base() {
	        bottomLeft = new Triangle();
	        this.addFace(bottomLeft);
	        
	        topRight = new Triangle();
	        this.addFace(topRight);
	        
	        topRight.setVert0(bottomLeft.getVert0());
	        topRight.setVert1(bottomLeft.getVert2());
	        topRight.getVert2().set(-0.5, 0.5, 0.0);
	        
	        topRight.setTextureCoordinate0(bottomLeft.getTextureCoordinate0());
	        topRight.setTextureCoordinate1(bottomLeft.getTextureCoordinate2());
	        topRight.getTextureCoordinate2().set(0.0f, 1.0f);
			
	        this.cloneMaterial();
	    }
    
	    public Triangle getBottomLeft() {return this.bottomLeft;}
	    public Triangle getTopRight() {return this.topRight;}
	    
	    public Vertice getVert0() {return bottomLeft.getVert0();}
	    public Vertice getVert1() {return bottomLeft.getVert1();}
	    public Vertice getVert2() {return bottomLeft.getVert2();}
	    public Vertice getVert3() {return topRight.getVert2();}
	    
	    public void setVert0(Vertice v) {bottomLeft.setVert0(v);topRight.setVert0(v);}
	    public void setVert1(Vertice v) {bottomLeft.setVert1(v);}
	    public void setVert2(Vertice v) {bottomLeft.setVert2(v);topRight.setVert1(v);}
	    public void setVert3(Vertice v) {topRight.setVert2(v);}
	    
	    public void setTextureCoordinate0(TextureCoordinate tc) {bottomLeft.setTextureCoordinate0(tc);topRight.setTextureCoordinate0(tc);}
	    public void setTextureCoordinate1(TextureCoordinate tc) {bottomLeft.setTextureCoordinate1(tc);}
	    public void setTextureCoordinate2(TextureCoordinate tc) {bottomLeft.setTextureCoordinate2(tc);topRight.setTextureCoordinate1(tc);}
	    public void setTextureCoordinate3(TextureCoordinate tc) {topRight.setTextureCoordinate2(tc);}
	    
	    public Triangle[] toQuadArray() {
	        if(this.quadArray != null) return this.quadArray;
	        return this.quadArray = new Triangle[] {
	            bottomLeft,
	            topRight
	        };
	    }
		
	    public override Model clone() {
	        return new Quad(this);
	    }
	}
}

