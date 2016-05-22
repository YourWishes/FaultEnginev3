using System;
using System.Collections.Generic;

namespace Fault {
	public class Scene {
		private static Scene ACTIVE_SCENE;
		public static Scene getActiveScene() {
			return ACTIVE_SCENE;
		}
		
		public static void setActiveScene(Scene scene) {
			ACTIVE_SCENE = scene;
			if(scene == null) return;
			scene.onSet();
		}
		
		//Instance
		private List<Model> MODELS;
		private List<Entity> ENTITIES;
		private List<Light> LIGHTS;
		
		private bool paused = false;
		private bool fo = false;
		
		public Scene () {
			MODELS = new List<Model>();
			ENTITIES = new List<Entity>();
			LIGHTS = new List<Light>();
		}
		
		public virtual void onSet() {}
		public virtual void onRender() {}
		public virtual void tick() {
			foreach(BackgroundMusic bgm in BackgroundMusic.getLoadedBackgroundMusics()) {
				if(!bgm.isPlaying()) continue;
				bgm.tick();
			}
			
			foreach(Entity entity in this.getEntities()) {
				if(entity == null) continue;
				entity.tick();
			}
			
			if(fo) {
				foreach(Model m in this.getModels()) {
					m.getMaterial().getColor().a -= 0.01;
					foreach(Model mo in m.getChildren(true)) {
						mo.getMaterial().getColor().a -= 0.01;
					}
				}
			}
		}
		
		public DisplayManager getDisplayManager() {return Game.GAME_INSTANCE.getDisplayManager();}
		
		public List<Model> getModels() {return new List<Model>(MODELS);}
		public List<Entity> getEntities() {lock(ENTITIES) {return new List<Entity>(ENTITIES);}}
		public List<Light> getLights() {return new List<Light>(LIGHTS);}
		public Camera getCamera() {return getDisplayManager().getCamera();}
		public Game getGame() {return Game.GAME_INSTANCE;}
		public GameLogger getLogger() {return getGame().getLogger();}
		public Color getSkyColor() {return getDisplayManager().getClearColor().clone();}
		public Light getAmbientLight() {return getDisplayManager().getAmbientLight();}
		
		public void setSkyColor(Color c) {getDisplayManager().setClearColor(c.clone());}
		public void setAmbientLight(Light l) {getDisplayManager().setAmbientLight(l);}
		
		public void addModel(Model m) {MODELS.Add(m); if(this.isActiveScene()) getDisplayManager().addModel(m);}
		public void addEntity(Entity e) {lock(ENTITIES) {ENTITIES.Add(e);} if(e.hasModel()) addModel (e.getModel());}
		public void addLight(Light l) {LIGHTS.Add(l); if(this.isActiveScene()) getDisplayManager().addLight(l);}
		
		public void removeModel(Model m) {MODELS.Remove(m); if(this.isActiveScene()) getDisplayManager().removeModel(m);}
		public void removeEntity(Entity e) {ENTITIES.Remove(e); if(e.hasModel()) removeModel (e.getModel());}
		public void removeLight(Light l) {LIGHTS.Remove(l); if(this.isActiveScene()) getDisplayManager().removeLight(l);}
		
		public bool isActiveScene() {return ACTIVE_SCENE != null && ACTIVE_SCENE.Equals(this);}
		public bool isFadingOut() {return this.fo;}
		public bool isPaused() {return this.paused;}
		
		public void pause() {this.paused = true;}
		public void resume() {this.paused = false;}
		
		public virtual void Dispose() {
			if(this.isActiveScene()) Scene.setActiveScene(null);
			foreach(Entity entity in this.getEntities()) {
				entity.Dispose();
			}
			
			foreach(Model m in this.getModels()) {
				m.Dispose();
			}
			
			foreach(Light l in this.getLights()) {
				l.Dispose();
			}
			
			foreach(GUI gui in GUI.getGUIs()) {
				if(gui == null || gui.getScene() == null) continue;
				if(!gui.getScene().Equals(this)) continue;
				gui.Dispose();
			}
			
			ENTITIES = null;
			MODELS = null;
			LIGHTS = null;
		}
		
		public void fadeOut() {
			fo = true;
		}
	}
}

