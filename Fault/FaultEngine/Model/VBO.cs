using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace Fault {
	public class VBO : IDisposable {
		private static List<VBO> vertexBufferObjects = new List<VBO>();
		public static int generateVBOID() {
			int id = 0;
			return id;
		}
		
		//Instance
		private Model model;
		private int id;
		
		private int vSize;
		private int tSize;
		
		private VertexBuffer vb = null;
		
		public VBO (Model m) {
			this.model = m;
		}
		
		public int getVBOID() {return this.id;}
		public int getVertexSize() {return this.vSize;}
		public VertexBuffer getVBO() {return this.vb;}
		
		public void generateVBO() {
			lock(model) {
				List<Vertice> verts = model.getAllVertices();
				List<TextureCoordinate> tcors =  model.getAllTextureCoordinates();
				
				this.vSize = verts.Count;
				this.tSize = tcors.Count;
				
				if(this.vSize < 1) return;
				
				float[] vertices = new float[this.vSize * 3];
				for(int i = 0; i < this.vSize; i++) {
					Vertice v = verts[i];
					int j = i * 3;
					vertices[j+0] = (float)v.getX();
					vertices[j+1] = (float)v.getY();
					vertices[j+2] = (float)v.getZ();
				}
				
				float[] coords = new float[this.tSize * 2];
				for(int i = 0; i < this.tSize; i++) {
					TextureCoordinate t = tcors[i];
					int j = i * 2;
					coords[j+0] = (float)t.getX();
					coords[j+1] = (float)t.getY();
				}
				
				this.vb = new VertexBuffer(this.vSize, VertexFormat.Float3, VertexFormat.Float2);
				lock(this.vb) {
					this.vb.SetVertices(0, vertices);
					if(this.vSize == this.tSize) this.vb.SetVertices(1, coords);
				}
			}
		}
		
		public void Dispose() {
			try {vb.Dispose();} catch(Exception e) {}
			vb = null;
			this.vSize = 0;
			this.id = -1;
		}
	}
}

