	using System;
using System.Collections.Generic;

using Sce.PlayStation.Core.Input;

namespace Fault {
	public class Controller : IDisposable {
		private Entity entity;
		
		public Controller (Entity what) {
			this.entity = what;
		}
		
		public Entity getControlling() {return this.entity;}
		public Location getLocation() {return this.entity.getLocation();}
		public Location getVelocity() {return this.entity.getVelocity();}
		public Scene getScene() {return getControlling().getScene();}
		public GameSettings getSettings() {return GameSettings.getSettings();}
		public List<Animation> getAnimations() {return this.getControlling().getAnimations();}
		
		public virtual void tick() {
			if(Scene.getActiveScene().isPaused()) return;
		}
		
		public virtual void Dispose() {}
		
		public Boolean isButtonDown(GamePadButtons button) {return Game.GAME_INSTANCE.isButtonDown(button);}
		public bool isButtonDown(String key, GamePadButtons defaultBinding) {
			return isButtonDown(getSettings().getSetting(key, defaultBinding).getAsGamePadButtons());
		}
		public double getLeftStickX() {return Game.GAME_INSTANCE.getGamePad().AnalogLeftX;}
		public double getLeftStickY() {return Game.GAME_INSTANCE.getGamePad().AnalogLeftY;}
		public double getRightStickX() {return Game.GAME_INSTANCE.getGamePad().AnalogRightX;}
		public double getRightStickY() {return Game.GAME_INSTANCE.getGamePad().AnalogRightY;}
	}
}

