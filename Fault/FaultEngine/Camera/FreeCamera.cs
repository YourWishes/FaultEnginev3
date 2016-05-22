using System;

using Sce.PlayStation.Core.Input;

namespace Fault {
	public class FreeCamera : Camera {
		public FreeCamera () : base() {
		}
		
		public override void reset () {
			base.reset ();
			Game game = Game.GAME_INSTANCE;
			Location l = this.getLocation();
			
			l.addYaw(-game.getGamePad().AnalogRightX);
			l.addPitch(game.getGamePad().AnalogRightY);
			
			if(game.getGamePad().AnalogLeftX != 0) {
				Location clone = l.clone();
				clone.addYaw(90);
				double p = clone.getPitch();
				clone.setPitch(0);
				clone = clone.getRelativeInFacingDirection(-game.getGamePad().AnalogLeftX / 10.0);
				clone.addYaw(-90);
				clone.setPitch(p);
				l.set(clone);
			}
			
			if(game.getGamePad().AnalogLeftY != 0) {
				Location clone = l.clone();
				clone = clone.getRelativeInFacingDirection(-game.getGamePad().AnalogLeftY / 10.0);
				l.set(clone);
			}
			
			Location newTarget = l.getRelativeInFacingDirection(1);
			this.getTarget().set(newTarget);
			newTarget.Dispose();
		}
	}
}

