using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Xml;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Imaging;

namespace Fault {
	public class Game : IDisposable {
		public const String APPLICATION_DIRECTORY = "/Application/";
		public static Game GAME_INSTANCE = null;
		
		//Instance
		private string name;
		private double version;
		private bool debug;
		
		private GameLogger logger;
		private Thread mainThread;
		private Thread gameThread;
		
		private bool gameRunning = false;
		private bool renderDone = false;
		private GamePadData[] gamePadData;
		private List<TouchData>[] touchData;
		private List<Resource> resources;
		
		public Game(string name, double version) : this(name, version, false) {
		}	
		
		public Game (string name, double version, bool debug) {
			GAME_INSTANCE = this;
			
			this.name = name;
			this.version = version;
			this.debug = debug;
			
			this.logger = new GameLogger();
			this.gameThread = new Thread(tick);
			this.gameThread.IsBackground = true;
			
			this.gamePadData = new GamePadData[4];//4 = number_of_controllers
			this.touchData = new List<TouchData>[1];//4 = number_of_devices ?
		}
		
		public string getName() {return this.name;}
		public double getVersion() {return this.version;}
		public GameLogger getLogger() {return this.logger;}
		public GameSettings getSettings() {return GameSettings.getSettings();}
		public DisplayManager getDisplayManager() {return DisplayManager.getDisplayManager();}
		public Thread getMainThread() {return this.mainThread;}
		public Thread getGameThread() {return this.gameThread;}
		public bool getDebugMode() {return this.debug;}
		public String getFilePath() {return "/Documents/" + this.getName() + "/";}
		public List<Resource> getResources() {lock(resources) {return new List<Resource>(resources);}}
		//public String getProjectName() {return "MyFirst3D";}
		
		public List<TouchData>[] getTouchDatas() {return this.touchData;}
		public List<TouchData> getTouchData(int deviceID) {return getTouchDatas()[deviceID];}
		public List<TouchData> getTouchData() {return getTouchData(0);}
		public GamePadData[] getGamePadData() {return this.gamePadData;}
		public GamePadData getGamePadData(int device) {return this.gamePadData[device];}
		public GamePadData getGamePad() {return getGamePadData(0);}
		
		public Boolean isButtonDown(GamePadButtons button) {return isButtonDown(getGamePad(), button);}
		public Boolean isButtonDown(GamePadData gpd, GamePadButtons button) {return(gpd.Buttons & button) != 0;}
		public Boolean isGameRunning() {return this.gameRunning;}
		
		public void setResources(List<Resource> resources) {this.resources = resources;}
		
		public Resource getResource(String name) {
			if(name == null) return null;
			foreach(Resource res in this.getResources()) {
				if(res == null) continue;
				lock(res) {
					if(res.getName() == null) continue;
					if(!res.getName().ToLower().Equals(name.ToLower())) continue;
					return res;
				}
			}
			return null;
		}
		
		public Model getLoadedModel(String name) {
			if(name == null) return null;
			foreach(Resource res in this.getResources()) {
				if(res == null) continue;
				lock(res) {
					if(!res.getType().Equals(ResourceType.MODEL)) continue;
					Object o = res.getValue();
					if(o == null || !(o is List<Model>)) continue;
					List<Model> models = (List<Model>)o;
					foreach(Model model in models) {
						if(model == null) continue;
						if(model.getName() == null || !model.getName().Equals(name)) continue;
						return model;
					}
				}
			}
			return null;
		}
		
		public LoadableTexture getLoadedTexture(String name) {
			return (LoadableTexture)this.getResource(name).getValue();
		}
		
		public void start() {
			this.logger.log(this.getName() + " v" + this.getVersion() + " Started.");
			
			//Create User Dir
			try {
				if(!Directory.Exists(getFilePath())) Directory.CreateDirectory(getFilePath());
		   	} catch(Exception e) {
			}
			
			this.getDisplayManager().init();
			
			this.mainThread = Thread.CurrentThread;
			this.gameThread.Start();
			this.gameRunning = true;
			Scene.setActiveScene(new InitialScene());
			
			while(this.gameRunning) {
				for(int pd = 0; pd < this.gamePadData.Length; pd++) {
					try {
						gamePadData[pd] = GamePad.GetData(pd);
					} catch(Exception ex) {
					}
				}
				
				for(int td = 0; td < this.touchData.Length; td++) {
					try {
						List<TouchData> tdata = Touch.GetData(td);
						lock(tdata) {
							this.touchData[td] = new List<TouchData>(tdata);
						}
					} catch(Exception ex) {}
				}
				
				try {
					SystemEvents.CheckEvents ();
				} catch(Exception ex) {
					this.gameRunning = false;
					this.Dispose();
					break;
				}
				
				if(debug) {
					if(isButtonDown(GamePadButtons.Select)) {
						this.Dispose();
						break;
					}
				}
				
				getDisplayManager().render();
				renderDone = true;
			}
			
			this.Dispose();
		}
		
		public void stop() {
			this.Dispose();
		}
		
		public void tick() {
			Game g = Game.GAME_INSTANCE;
			while(!g.isGameRunning()) {
			}
			
			double lastTick = 0;
			while(g.isGameRunning()) {
				double now = TimeUtils.getNow();
				if((now - lastTick) < (1d / 60d)) {
					continue;
				}
				lastTick = now;
				
				double start = TimeUtils.getNow();
				
				foreach(GUI gui in GUI.getGUIs()) {
					gui.tick();
				}
				
				if(Scene.getActiveScene() != null) {
					Scene.getActiveScene().tick();
				}
				//while(!renderDone) {}
				renderDone = false;
			}
		}
		
		public void Dispose() {
			this.gameRunning = false;
			GameLogger.getLogger().log ("Closing Game Thread");
			try {this.gameThread.Interrupt();} catch(Exception e) {}
			
			GameLogger.getLogger().log ("Unloading BGMs");
			foreach(BackgroundMusic bgm in BackgroundMusic.getLoadedBackgroundMusics()) {
				try {bgm.Dispose();} catch(Exception e) {}
			}
			
			GameLogger.getLogger().log ("Destroying all GUIs");
			foreach(GUI gui in GUI.getGUIs()) {
				try {gui.Dispose();} catch(Exception e) {}
			}
			
			GameLogger.getLogger().log ("Diposing Scene");
			try {Scene.getActiveScene().Dispose();} catch(Exception e) {}
			
			getDisplayManager().Dispose();
		}
	}
}

