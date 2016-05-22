using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Imaging;

namespace Fault {
	public class AppMain {
		public static void Main (string[] args) {
			Game game = new Game("Fault", 1.00, true);
			game.start();
		}
	}
}
