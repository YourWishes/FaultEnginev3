using System;

namespace Fault {
	public class BipedExpression {
		public static BipedExpression BLANK = new BipedExpression(0, "Blank", "DefaultFace");
		public static BipedExpression PAIN = new BipedExpression(1, "Pain", "PainFace");
		
		//Instance
		private int id;
		private String expression;
		private String textureName;
		
		private BipedExpression (int id, String expression, String textureName) {
			this.id = id;
			this.expression = expression;
			this.textureName = textureName;
		}
		
		public int getID() {return this.id;}
		public String getExpressionName() {return this.expression;}
		public String getTextureName() {return this.textureName;}
		public LoadableTexture getTexture() {return Game.GAME_INSTANCE.getLoadedTexture(this.textureName);}
	}
}

