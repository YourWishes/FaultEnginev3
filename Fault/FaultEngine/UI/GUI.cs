using System;
using System.Collections.Generic;
using System.Threading;

namespace Fault {
	public class GUI : IDisposable, Locateable {
		private static List<GUI> OPEN_GUIS = new List<GUI>();
		private static List<GUI> GUIS = new List<GUI>();
		
		public static List<GUI> getOpenGUIs() {lock(OPEN_GUIS) {return new List<GUI>(OPEN_GUIS);}}
		public static List<GUI> getGUIs() {lock(GUIS) {return new List<GUI>(GUIS);}}
		
		//Instance
		private int nextTabIndex = 0;
		private Location location;
		private Scene scene;
		private List<GUIElement> elements;
		
		private bool showing = false;
		
		public GUI (Scene scene) {
			this.scene = scene;
			this.elements = new List<GUIElement>();
			
			this.location = new Location();
			this.location.setOwner(this);
			
			lock(GUIS) {GUIS.Add(this);}
		}
		
		public Scene getScene() {return this.scene;}
		public Location getLocation() {return this.location;}
		Location Locateable.getLocation() {return getLocation();}
		Locateable Locateable.getParent() {return null;}
		public List<GUIElement> getElements() {lock(this.elements) {return new List<GUIElement>(this.elements);}}
		public int getNextTabIndex() {return this.nextTabIndex++;}
		
		public GUIElement addElement(GUIElement element) {lock(this.elements) {this.elements.Add(element); if(this.showing) element.onGUIShow(); return element;}}
		
		public GUIElement removeElement(GUIElement element) {lock(this.elements) {this.elements.Remove(element); return element;}}
		
		public void show() {
			if(this.showing) return;
			lock(OPEN_GUIS) {OPEN_GUIS.Add(this);}
			this.showing = true;
			foreach(GUIElement element in this.getElements()) {
				lock(element) {
					element.onGUIShow();
				}
			}
		}
		
		public void hide() {
			if(!this.showing) return;
			lock(OPEN_GUIS) {OPEN_GUIS.Remove(this);}
			this.showing = false;
			foreach(GUIElement element in this.getElements()) {
				element.onGUIHide();
			}
		}
		
		public void tick() {
			//Tick Children
			foreach(GUIElement element in this.getElements()) {
				element.tick();
			}
		}
		
		public void Dispose() {
			lock(OPEN_GUIS) {OPEN_GUIS.Remove(this);}
			lock(GUIS) {GUIS.Remove(this);}
			
			foreach(GUIElement guiElement in this.getElements()) {
				guiElement.Dispose();
			}
		}
	}
}

