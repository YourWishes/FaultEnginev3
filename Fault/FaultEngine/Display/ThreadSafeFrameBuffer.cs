using System;

using Sce.PlayStation.Core.Graphics;

namespace Fault {
	public class ThreadSafeFrameBuffer {
		private FrameBuffer fb;
		
		private ThreadSafeDepthBuffer db;
		private LoadableTexture texture;
		
		public ThreadSafeFrameBuffer () {
		}
		
		public FrameBuffer getFrameBuffer() {return this.fb;}
		public ThreadSafeDepthBuffer getThreadSafeDepthBuffer() {return this.db;}
		public LoadableTexture getLoadableTexture() {return this.texture;}
		
		public void setFrameBuffer(FrameBuffer fb) {this.fb = fb;}
		public void setThreadSafeDepthBufferToBind(ThreadSafeDepthBuffer db) {this.db = db;}
		public void setLoadableTextureToBind(LoadableTexture lt) {this.texture = lt;}
	}
}

