using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Imaging;

namespace Fault {
	public class Face {
		//Instance
		private List<Vertice> vertices;
		private List<TextureCoordinate> textureCoordinates;
		
		public Face () {
			this.vertices = new List<Vertice>();
			this.textureCoordinates = new List<TextureCoordinate>();
		}
		
		public Face (Face aThis) : this() {
			this.vertices = new List<Vertice>(aThis.vertices);
			this.textureCoordinates = new List<TextureCoordinate>(aThis.textureCoordinates);
		}
		
		public List<Vertice> getVertices() {lock(this.vertices) {return new List<Vertice>(this.vertices);}}
		public List<TextureCoordinate> getTextureCoordinates() {return new List<TextureCoordinate>(this.textureCoordinates);}
		
		public void addVertice(Vertice v) {lock(this.vertices) {this.vertices.Add(v);}}
		public void addTextureCoordinate(TextureCoordinate tc) {lock(this.textureCoordinates) {this.textureCoordinates.Add(tc);}}
		
		public void removeVertice(Vertice v) {this.vertices.Remove(v);}
		public void removeTextureCoordinate(TextureCoordinate tc) {this.textureCoordinates.Remove(tc);}
		
		public int getVerticeIndex(Vertice ver) {
			int i = 0;
			lock(this.vertices) {
				foreach(Vertice v in this.vertices) {
					if(v.Equals(ver)) {
						break;
					}
					i++;
				}
			}
			return i;
		}
	    
	    public virtual Face clone() {
	        return new Face(this);
	    }
		
		public void clean() {
			//Clone the list of Vertices, then remove them
			this.vertices = new List<Vertice>();
			this.textureCoordinates = new List<TextureCoordinate>();
		}
	}
}

