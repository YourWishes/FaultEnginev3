using System;
using System.Collections.Generic;

namespace Fault {
	/*
	 * This is the Scene that is first called when the game begins loading
	 * Put intial loaders here, maybe company logo
	 */
	public class InitialScene : Scene {
		private LoadingScene loader;
		
		public InitialScene () : base() {
			this.loader = new LoadingScene(this, "Resources/ResourceList.xml");
		}
		
		public override void onSet () {
			base.onSet ();
			loader.start();
		}
		
		public void doneLoading() {
			Game.GAME_INSTANCE.setResources(loader.getLoadedThings());
			
			addEntity(new BipedEntity(this));
			
			Entity chest = new Entity(this, EntityType.UNKNOWN);
			chest.setModel(Game.GAME_INSTANCE.getLoadedModel("ChestBase").clone());
			chest.getModel().setBounds(BoundingBox.calculateBox(chest.getModel()));
			addEntity(chest);
			
			chest.getLocation().addY(-4);
		}
	}
}

