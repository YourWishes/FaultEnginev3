using System;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace Fault {
	public class LoadableTexture : IDisposable {
		private String textureName;
		private Texture2D texture;
		
		private int width, height;
		private Image image;
		
		public LoadableTexture(int width, int height) : this(width, height, null) {
		}
		
		public LoadableTexture(int width, int height, Image image) {
			this.width = width;
			this.height = height;
			this.image = image;
		}
		
		public LoadableTexture (String textureName) {
			this.textureName = textureName;
		}
		
		public LoadableTexture () {
		}
		
		public String getTextureName() {return this.textureName;}
		public Texture2D getTexture() {return this.texture;}
		public int getWidth() {return width;}
		public int getHeight() {return height;}
		public Image getImage() {return this.image;}
		
		public void setTexture(Texture2D texture) {this.texture = texture;}
		
		public void set(LoadableTexture data) {
			this.textureName = data.textureName;
			this.texture = data.texture;
			this.width = data.width;
			this.height = data.height;
			this.image = data.image;
		}
		
		public bool isLoaded() {return this.texture != null;}
		
		public void loadNow() {
			DisplayManager.getDisplayManager().addTextureToLoad(this);
			while(!this.isLoaded()) {continue;}
		}
		
		public void Dispose () {
			if(this.texture != null) this.texture.Dispose();
		}
	}
}

