using System;
using System.Collections.Generic;
using System.Threading;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace Fault {
	public class Sprite : Model {
		private double width;
		private double height;
		public bool xf = false;
		public bool yf = false;
		
		public Sprite(Sprite sprite) : this(sprite, null) {
			
		}
		
		public Sprite(Sprite sprite, String name) : base(sprite) {
			this.cloneMaterial();
			this.setMatProperties();
			
			this.width = sprite.width;
			this.height = sprite.height;
			
			this.setName(name);
			
			this.updateFace();
		}
		
		public Sprite(Texture2D texture) : this(texture, null) {
			
		}
		
		public Sprite (Texture2D texture, String name) : base() {
			this.setTexture(texture);
			this.setMatProperties();
			this.setShaderProgram(ShaderList.getSpriteShader());
			
			this.width = texture.Width;
			this.height = texture.Height;
			
        	this.width = Math.Min(width, 2048);
			
			this.setName(name);
			
			this.updateFace();
		}
		
		public Texture2D getTexture() {return this.getMaterial().getTexture();}
		public double getWidth() {return this.width * this.getScaleX();}
		public double getHeight() {return this.height * this.getScaleY ();}
		
		public void flipX() {this.xf = !this.xf;}
		public void flipY() {this.yf = !this.yf;}
		
		public void setWidth(double width) {this.width = width;}
		public void setHeight(double height) {this.height = height;}
		
		public void setTexture(Texture2D texture) {this.getMaterial().setTexture(texture); this.updateFace();}
		
		public void updateFace() {
			Vertice topLeft = new Vertice(0, 0, 0);
			Vertice topRight = new Vertice(this.width, 0, 0);
			Vertice bottomLeft = new Vertice(0, this.height, 0);
			Vertice bottomRight = new Vertice(this.width, this.height, 0);
			
			int lx = 0;
			int ly = 0;
			int mx = 1;
			int my = 1;
			
			if(this.xf) {
				lx = 1;
				mx = 0;
			}
			
			if(this.yf) {
				ly = 1;
				my = 0;
			}
			
			TextureCoordinate topLeftTC = new TextureCoordinate(lx, ly);
			TextureCoordinate topRightTC = new TextureCoordinate(mx, ly);
			TextureCoordinate bottomLeftTC = new TextureCoordinate(lx, my);
			TextureCoordinate bottomRightTC = new TextureCoordinate(mx, my);
			
			Face topLeftFace = new Face();
			topLeftFace.addVertice(bottomLeft);
			topLeftFace.addVertice(topRight);
			topLeftFace.addVertice(topLeft);
			
			topLeftFace.addTextureCoordinate(bottomLeftTC);
			topLeftFace.addTextureCoordinate(topRightTC);
			topLeftFace.addTextureCoordinate(topLeftTC);
			
			Face topRightFace = new Face();
			topRightFace.addVertice(bottomRight);
			topRightFace.addVertice(topRight);
			topRightFace.addVertice(bottomLeft);
			
			topLeftFace.addTextureCoordinate(bottomRightTC);
			topLeftFace.addTextureCoordinate(topRightTC);
			topLeftFace.addTextureCoordinate(bottomLeftTC);
			
			foreach(Face f in this.getFaces()) {
				this.removeFace(f);
			}
			
			addFace(topLeftFace);
			addFace(topRightFace);
			
			if(Thread.CurrentThread != Game.GAME_INSTANCE.getMainThread()) {
				if(this.getVBO() != null) {lock(this.getVBO ()) {this.getVBO().Dispose();}}
				this.setVBO(null);
				this.addToVBOList();
				while(!this.isOnVBO()) {continue;}
			} else {
				if(this.getVBO() != null) this.getVBO().Dispose();
				this.newVBO();
			}
		}
		
		public override void setShaderVariables(Matrix4 offset, Matrix4 my_offset, Camera c) {
			this.set2DShaderVariables(offset, my_offset, c);
		}
		
		private void setMatProperties() {
			this.getMaterial().setRender2D(true);
			this.getMaterial().setRender3D(false);
			this.getMaterial().setCullFaced(false);
			this.getMaterial().setDepthTested(false);
		}
	}
}

