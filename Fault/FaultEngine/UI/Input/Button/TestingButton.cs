using System;

using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Graphics;

namespace Fault {
	public class TestingButton : Button {
		public TestingButton (GUI gui) : base(gui, null) {
		}
		
		public TestingButton(GUI gui, Texture2D texture) : base(gui, texture, null, null) {
			
		}
		
		public TestingButton(GUI gui, Texture2D up, Texture2D hover, Texture2D down) : base(gui, up, hover, down) {
			
		}
		
		public override void onPress (TouchData td) {
			base.onPress (td);
			Game.GAME_INSTANCE.getLogger().log ("Held Down!");
		}
		
		public override void onHold(TouchData td) {
			base.onHold (td);
			Game.GAME_INSTANCE.getLogger().log ("Moving!");
		}
		
		public override void onRelease (TouchData td) {
			base.onRelease (td);
			Game.GAME_INSTANCE.getLogger().log ("Released!");
		}
		
		public override void onCancelling (TouchData td) {
			base.onCancelling (td);
			Game.GAME_INSTANCE.getLogger().log ("Cancelled!");
		}
	}
}

