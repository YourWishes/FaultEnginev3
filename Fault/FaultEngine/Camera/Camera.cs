using System;

using Sce.PlayStation.Core;

namespace Fault {
	public class Camera : Locateable, IDisposable {
		public static Camera getCamera() {
			if(DisplayManager.getDisplayManager().getCamera() != null) return DisplayManager.getDisplayManager().getCamera();
			Camera c = new Camera();
			DisplayManager.getDisplayManager().setCamera(c);
			return c;
		}
		
		//Instance
		private Location location;
		private Location target;
		
		private float fieldOfView = 45.0f;
		private float zNear = 1.0f;
		private float zFar = 100.0f;
		
		private bool pmnr = true;//Projection Matrix Needs to be reset
		private bool vmnr = true;//View Matrix needs to be reset
		private bool p2mnr = true;//Projection 2D Matrix etc etc.
		
		private Matrix4 projectionMatrix;
		private Matrix4 twoDimensionalProjectionMatrix;
		private Matrix4 viewMatrix;
		
		public Camera () {
			this.location = new Location(10,10,10);
			this.location.setOwner(this);
			
			this.target = new Location();
			this.target.setOwner(this);
		}
		
		public Location getLocation() {return this.location;}
		Location Locateable.getLocation() {return getLocation();}
		Locateable Locateable.getParent() {return null;}
		public Location getTarget() {return this.target;}
		public float getFieldOfView() {return this.fieldOfView;}
		public float getZNear() {return this.zNear;}
		public float getZFar() {return this.zFar;}
		public int getWidth() {return getDisplayManager().getGraphicsContext().GetFrameBuffer().Rectangle.Width;}
		public int getHeight() {return getDisplayManager().getGraphicsContext().GetFrameBuffer().Rectangle.Height;}
		public DisplayManager getDisplayManager() {return DisplayManager.getDisplayManager();}
		
		public void setFieldOfView(float fov) {this.fieldOfView = fov;}
		
		public virtual void reset() {
			this.pmnr = true;
			this.vmnr = true;
			this.p2mnr = true;
		}
		
		public Matrix4 getProjectionMatrix() {
			if(!this.pmnr) return this.projectionMatrix;
			this.projectionMatrix = Matrix4.Perspective(
				FMath.Radians(this.fieldOfView),
				getDisplayManager().getGraphicsContext().GetFrameBuffer().AspectRatio,
				this.zNear,
				this.zFar
			);
			return this.projectionMatrix;
		}
		
		public Matrix4 get2DProjectionMatrix() {
			if(!this.p2mnr) return this.twoDimensionalProjectionMatrix;
			
			this.twoDimensionalProjectionMatrix = Matrix4.Ortho(
				0,
				this.getWidth(),
				this.getHeight(),
				0,
				0,
				500
			);
			return this.twoDimensionalProjectionMatrix;
		}
		
		public Matrix4 getViewMatrix() {
			if(!this.vmnr) return this.viewMatrix;
			Location l = this.getLocation().clone();
			Location t = this.getTarget().clone();
			this.viewMatrix = Matrix4.LookAt(
				l.getAsVector(),
				t.getAsVector(),
				new Vector3(0,1,0)
			);
			l.Dispose();
			t.Dispose();
			return this.viewMatrix;
		}
		
		private double prevFrame = TimeUtils.getNow();
		private double fps = 30;
		public String getFrameRate() {
			double now = TimeUtils.getNow();
			double timeTakenToRenderFrame = (now - prevFrame);
			prevFrame = now;
			fps = (fps + (1000d / (timeTakenToRenderFrame * 1000d))) / 2d;
			return "" + Math.Floor(fps);
		}
		
		public void Dispose() {
			this.location.Dispose();
			this.target.Dispose();
			
			this.location = null;
			this.target = null;
		}
	}
}

