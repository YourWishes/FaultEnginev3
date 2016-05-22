using System;
using System.Collections.Generic;
using System.IO;

using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Graphics;

namespace Fault {
	public class DisplayManager : IDisposable {
		private static DisplayManager DISPLAY_MANAGER_INSTANCE = null;
		public static DisplayManager getDisplayManager() {
			if(DISPLAY_MANAGER_INSTANCE != null) return DISPLAY_MANAGER_INSTANCE;
			return DISPLAY_MANAGER_INSTANCE = new DisplayManager();
		}
		
		//Instance
		private GraphicsContext graphicsContext;
		private Color clearColor = Color.BLACK.clone();
		
		private Camera camera;
		private FrameBuffer mainFrameBuffer;
		
		private List<Model> models;
		private List<Light> lights;
		private List<Model> vboWaitingList;
		private List<LoadableTexture> texturesToLoad;
		
		private List<ThreadSafeFrameBuffer> frameBuffersToMake;
		private List<ThreadSafeDepthBuffer> depthBuffersToMake;
		
		private Light ambientLight;
		
		//Options
		public bool renderBounds = true;
		
		public DisplayManager () {
			DISPLAY_MANAGER_INSTANCE = this;
			
			this.graphicsContext = new GraphicsContext();
			
			this.camera = new Camera();
			
			this.models = new List<Model>();
			this.lights = new List<Light>();
			this.vboWaitingList = new List<Model>();
			this.texturesToLoad = new List<LoadableTexture>();
			
			this.frameBuffersToMake = new List<ThreadSafeFrameBuffer>();
			this.depthBuffersToMake = new List<ThreadSafeDepthBuffer>();
			
			//Generate Ambient Light
			ambientLight = new Light();
			ambientLight.setColor(Color.WHITE);
			ambientLight.setIntensity(0.2f);
			
			this.clearColor = Color.BLACK.clone();
			
		}
		
		public GraphicsContext getGraphicsContext() {return this.graphicsContext;}
		public Color getClearColor() {return this.clearColor.clone();}
		public Camera getCamera() {return this.camera;}
		public List<Model> getModels() {lock(this.models) {return new List<Model>(this.models);}}
		public List<Light> getLights() {return new List<Light>(this.lights);}
		public Light getAmbientLight() {return this.ambientLight;}
		
		public void setClearColor(Color c) {this.clearColor = c.clone();}
		public void setCamera(Camera c) {if(c==null)return; this.camera = c;}
		public void setAmbientLight(Light l) {this.ambientLight = l;}
		
		public void addModel(Model m) {if(m == null) return; lock(this.models) {this.models.Add(m);}}
		public void addLight(Light l) {this.lights.Add(l);}
		public void addToWaitingList(Model m) {lock(this.vboWaitingList) {if(this.vboWaitingList.Contains(m)) return; this.vboWaitingList.Add(m);}}
		public void addTextureToLoad(LoadableTexture t) {lock(this.texturesToLoad) {this.texturesToLoad.Add(t);}}
		public void addThreadSafeFrameBufferToMake(ThreadSafeFrameBuffer tsfb) {lock(this.frameBuffersToMake) {this.frameBuffersToMake.Add(tsfb);}}
		public void addThreadSafeDepthBufferToMake(ThreadSafeDepthBuffer dsfb) {lock(this.depthBuffersToMake) {this.depthBuffersToMake.Add(dsfb);}}
		
		public void removeModel(Model m) {lock(this.models) {this.models.Remove(m);}}
		public void removeLight(Light l) {this.lights.Remove(l);}
		
		public void init() {
			//Model.getDefaultShaderProgram();	//Initializes the default shader
			
			this.graphicsContext.Enable(EnableMode.DepthTest, true);
			//this.graphicsContext.Enable(EnableMode.CullFace, true);
			this.graphicsContext.Enable(EnableMode.Blend, true);
			//this.graphicsContext.Enable(EnableMode.ScissorTest, true);
			//his.graphicsContext.Enable(EnableMode.Dither, true);
			//this.graphicsContext.Enable(EnableMode.StencilTest, true);
			
        	this.graphicsContext.SetBlendFunc(BlendFuncMode.Add, BlendFuncFactor.SrcAlpha, BlendFuncFactor.OneMinusSrcAlpha);
			this.graphicsContext.SetCullFace(CullFaceMode.Back, CullFaceDirection.Ccw);
			
			this.mainFrameBuffer = this.graphicsContext.GetFrameBuffer();
		}
		
		public void render() {
			//Begin Rendering
			GraphicsContext graphics = getGraphicsContext();
			
			//Load Textures as Needed
			lock(this.texturesToLoad) {
				foreach(LoadableTexture lt in new List<LoadableTexture>(this.texturesToLoad)) {
					lock(lt) {try {
							Game.GAME_INSTANCE.getLogger().log ("Loading Texture " + lt.getTextureName());
							this.texturesToLoad.Remove(lt);
							if(lt.isLoaded()) continue;
							Texture2D texture;
							if(lt.getTextureName() != null) {
								texture = new Texture2D(lt.getTextureName(), false);
							} else {
								texture = new Texture2D(lt.getWidth(), lt.getHeight(), false, PixelFormat.Rgba, PixelBufferOption.Renderable);
							}
							
							if(lt.getImage() != null) {
								texture.SetPixels(0, lt.getImage().ToBuffer(), 0, 0, lt.getWidth(), lt.getHeight());
							}
							
							lt.setTexture(texture);
							Game.GAME_INSTANCE.getLogger().log ("...Done loading " + lt.getTextureName());
					} catch(Exception e) {Game.GAME_INSTANCE.getLogger().log ("Failed to load Texture..");}}
				} 
			}
			
			//Generate Depth Buffer Objects as Needed
			lock(this.depthBuffersToMake) {
				foreach(ThreadSafeDepthBuffer dsfb in new List<ThreadSafeDepthBuffer>(this.depthBuffersToMake)) {
					if(dsfb == null) continue;
					if(dsfb.getDepthBuffer() != null) continue;
					DepthBuffer db = new DepthBuffer(dsfb.getWidth(), dsfb.getHeight(), PixelFormat.Depth16);
					dsfb.setDepthBuffer(db);
					this.depthBuffersToMake.Remove(dsfb);
				}
			}
			
			//Generate Frame Buffer Objects as Needed
			lock(this.frameBuffersToMake) {
				foreach(ThreadSafeFrameBuffer tsfb in new List<ThreadSafeFrameBuffer>(this.frameBuffersToMake)) {
					if(tsfb == null) continue;
					if(tsfb.getFrameBuffer() != null) continue;
					FrameBuffer fb = new FrameBuffer();
					tsfb.setFrameBuffer(fb);
					
					if(tsfb.getLoadableTexture() != null && tsfb.getLoadableTexture().isLoaded()) {
						fb.SetColorTarget(tsfb.getLoadableTexture().getTexture(), 0);
					}
					
					if(tsfb.getThreadSafeDepthBuffer() != null && tsfb.getThreadSafeDepthBuffer().getDepthBuffer() != null) {
						fb.SetDepthTarget(tsfb.getThreadSafeDepthBuffer().getDepthBuffer());
					}
					this.frameBuffersToMake.Remove(tsfb);
				}
			}
			
			//Put Things on VBO as needed
			lock(this.vboWaitingList) {
				foreach(Model m in new List<Model>(this.vboWaitingList)) {
					Game.GAME_INSTANCE.getLogger().log ("Putting " + m.GetType() + "(" + m.getName() + ") on the VBO");
					m.putOnVBO();
					this.vboWaitingList.Remove(m);
				}
			}
			
			lock(this.getCamera()) {				
				List<Model> models = this.getModels();
				
				try {
					models.Sort(
						delegate(Model p1, Model p2) {
							if(p1 == null || p2 == null) return 0;
							lock(p1) {
								lock(p2) {
									Location p1Abs = p1.getLocation().getAbsoluteLocation();
									Location p2Abs = p2.getLocation().getAbsoluteLocation();
									double p1Dist = getCamera().getLocation().getDistance(p1Abs);
									double p2Dist = getCamera().getLocation().getDistance(p2Abs);
									return p1Dist.CompareTo(p2Dist);
								}
							}
						}
					);
				} catch(Exception e) {}
				
				//Render Objects
				if(Scene.getActiveScene() != null) {
					try {
						lock(Scene.getActiveScene()) {
							Scene.getActiveScene().onRender();
						}
					} catch(Exception e) {}
				}
				
				renderToBuffer(models, getCamera());
				
				//Add Custom Models as Needed
				if(Game.GAME_INSTANCE.getDebugMode()) {
					
					FontModel fps = new FontModel("FPS: " + this.getCamera().getFrameRate());
					fps.render(getCamera());
					fps.getTexture().Dispose();
					fps.getVBO().Dispose();
					fps.Dispose();
				}
			}
			
			graphics.SwapBuffers ();
			System.GC.Collect();
		}
		
		
		public void renderToBuffer(List<Model> models, Camera c) {
			renderToBuffer(this.mainFrameBuffer, models, c);
		}
		
		public void renderToBuffer(FrameBuffer fb, List<Model> models, Camera c) {
			this.graphicsContext.SetFrameBuffer(fb);
			if(this.getCamera() == null) return;
			
			this.getCamera().reset();
			Model.newFrame(c);
			this.graphicsContext.SetClearColor((float)clearColor.r, (float)clearColor.g, (float)clearColor.b, (float)clearColor.a);
			this.graphicsContext.Clear ();
			
			this.graphicsContext.SetViewport(0, 0, fb.Width, fb.Height);
			lock(models) {
				foreach(Model model in models) {
					try {
						lock(model) {
							model.render(c);
						}
					} catch(Exception e) {}
				}
			}
		}
		
		public void Dispose() {
			foreach(Model model in this.models) {
				model.Dispose();
			}
			graphicsContext.Dispose();
		}
	}
}

	