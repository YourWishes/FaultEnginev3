using System;

using Sce.PlayStation.Core.Graphics;

namespace Fault {
	public class ThreadSafeDepthBuffer {
		private DepthBuffer db;
		
		private int width;
		private int height;
		
		public ThreadSafeDepthBuffer (int width, int height) {
			this.width = width;
			this.height = height;
		}
		
		public DepthBuffer getDepthBuffer() {return this.db;}
		public int getWidth() {return width;}
		public int getHeight() {return height;}
		
		public void setDepthBuffer(DepthBuffer db) {this.db = db;}
	}
}

