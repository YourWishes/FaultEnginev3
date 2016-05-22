using System;
using System.Collections.Generic;

namespace Fault {
	public abstract class ModelLoader {
		public static WavefrontLoader WAVEFRONT_LOADER = new WavefrontLoader();
		
		//Instance
		public ModelLoader () {
			
		}
		
		public abstract List<Model> loadModels(String data);
	}
}

