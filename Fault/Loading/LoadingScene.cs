using System;
using System.Collections.Generic;
using System.Xml;
using System.Threading;

namespace Fault {
	public class LoadingScene {
		public const String RESOURCES_DIRECTORY = Game.APPLICATION_DIRECTORY + "Resources/";
		
		private Thread loadingThread;
		private InitialScene scene;
		private String thingsToLoad;
		private List<Resource> loadedStuff;
		
		public LoadingScene (InitialScene scene, String thingsToLoad) {
			this.loadingThread = new Thread(load);
			this.scene = scene;
			this.thingsToLoad = thingsToLoad;
			this.loadedStuff = new List<Resource>();
		}
		
		public List<Resource> getLoadedThings() {return this.loadedStuff;}
		
		public void start() {
			this.loadingThread.Start();
		}
		
		private void load() {
			//Load Resource List
			List<Model> modelsToVBO = new List<Model>();
			
			XmlReader reader = XmlReader.Create(Game.APPLICATION_DIRECTORY + this.thingsToLoad);
			while(reader.Read()) {
				if(reader.Name == null || reader.Name == "" || reader.Name == "xml") continue;
				ResourceType rt = ResourceType.getTypeByXMLValue(reader.Name);
				if(rt == null) {
					continue;
				}
				
				String name = reader["name"];
				String file = reader["file"];
				
				//Load File Depending on Type
				Object data = null;
				if(rt.Equals(ResourceType.LANGUAGE)) {
					data = LanguageLoader.getLoader().loadLanguage(XmlReader.Create(RESOURCES_DIRECTORY + file));
				} else if(rt.Equals(ResourceType.MODEL)) {
					List<Model> m = ModelLoader.WAVEFRONT_LOADER.loadModels(FileUtilities.getResourceAsString(RESOURCES_DIRECTORY + file));
					foreach(Model mod in m) {
						mod.addToVBOList();
					}
					modelsToVBO.AddRange(m);
					data = m;
				} else if(rt.Equals(ResourceType.SCENE)) {
					//TODO: Finish
				} else if(rt.Equals(ResourceType.ANIMATION)) {
					String resource = FileUtilities.getResourceAsString(RESOURCES_DIRECTORY + file);
					data = AnimationLoader.getLoader().loadAnimationRules (resource);
				} else if(rt.Equals(ResourceType.BACKGROUND_MUSIC)) {
					
				} else if(rt.Equals(ResourceType.TEXTURE)) {
					data = new LoadableTexture(RESOURCES_DIRECTORY + file);
					((LoadableTexture) data).loadNow();
				}
				
				if(data == null) {
					GameLogger.getLogger().log ("Failed to load " + reader.Name + " (" + reader.Depth + ")");
					continue;
				}
				
				if(Game.GAME_INSTANCE.getDebugMode()) {
					GameLogger.getLogger().log ("Loaded " + rt.getName() + ": " + name + "(" + file + ")");
				}
				
				Resource res = new Resource(rt, name, file, data);
				this.loadedStuff.Add(res);
			}
			
			foreach(Model m in modelsToVBO) {
				while(!m.isOnVBO()) {continue;}
			}
			this.loaded();
		}
		
		public void loaded() {
			this.scene.doneLoading();
			try {
				this.loadingThread.Abort();
				this.loadingThread.Interrupt();
			} catch(Exception ex) {
				
			}
		}
	}
}

