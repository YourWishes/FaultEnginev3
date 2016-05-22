using System;

using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace Fault {
	public class Material {
		private Color color = Color.WHITE.clone();
		private Texture2D texture = null;
		private bool render2D = false;
		private bool render3D = true;
		private bool depthTested = true;
		private bool cullFaced = false;
		
		private String name = null;
		
		public Material () {
			
		}
		
		public Material(Material material) : this() {
			this.color = material.getColor().clone();
			this.texture = material.getTexture();
			this.render2D = material.render2D;
			this.render3D = material.render3D;
			this.cullFaced = material.cullFaced;
		}
		
		public Color getColor() {return this.color;}
		public Texture2D getTexture() {return this.texture;}
		public String getName() {return this.name;}
		public bool getRender2D() {return this.render2D;}
		public bool getRender3D() {return this.render3D;}
		public bool getDepthTested() {return this.depthTested;}
		public bool getCullFaced() {return this.cullFaced;}
		
		public void setColor(Color c) {this.color = c;}
		public void setTexture(Texture2D texture) {this.texture = texture;}
		public void setTexture(LoadableTexture texture) {if(!texture.isLoaded()) return; this.texture = texture.getTexture();}
		public void setName(String name) {this.name = name;}
		public void setRender2D(bool t) {this.render2D = t;}
		public void setRender3D(bool t) {this.render3D = t;}
		public void setDepthTested(bool t) {this.depthTested = t;}
		public void setCullFaced(bool t) {this.cullFaced = t;}
		
		public bool isTextured() {return this.texture != null;}
		
		public Material clone() {
			return new Material(this);
		}
	}
}

