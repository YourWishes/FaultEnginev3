using System;
using System.Threading;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Environment;

namespace Fault {
	public class FontModel : Sprite {
		public static Texture2D createTexture(string text, Font font, uint argb) {
			int width = font.GetTextWidth(text, 0, text.Length);
			int height = font.Metrics.Height;
			
			var image = new Image(	ImageMode.Rgba,
									new ImageSize(width, height),
									new ImageColor(0, 0, 0, 0)
          	);
			
			image.DrawText(	text,
							new ImageColor(	(int)((argb >> 16) & 0xff),
											(int)((argb >> 8) & 0xff),
											(int)((argb >> 0) & 0xff),
											(int)((argb >> 24) & 0xff)),
							font, new ImagePosition(0, 0)
           	);
			
			Texture2D texture;
			if(Thread.CurrentThread != Game.GAME_INSTANCE.getMainThread()) {
				LoadableTexture txt = new LoadableTexture(width, height, image);
				txt.loadNow();
				texture = txt.getTexture();
			} else {
				texture = new Texture2D(width, height, false, PixelFormat.Rgba);
				texture.SetPixels(0, image.ToBuffer(), 0, 0, width, height);
			}
			image.Dispose();
			
			return texture;
		}
		
		private static Font DEFAULT_FONT = null;
		public static Font getDefaultFont() {
			if(DEFAULT_FONT != null) return DEFAULT_FONT;
			DEFAULT_FONT = new Font(FontAlias.System, 24, FontStyle.Regular);
			return DEFAULT_FONT;
		}
		
		//Instance
		private String text;
		
		public FontModel (FontModel fm) : base(fm) {
			this.text = fm.text;
			this.setName(fm.text);
		}
		
		public FontModel (String text, Font font) : base(createTexture(text, font, 0xffffffff)) {
			this.text = text;
			this.setName(text);
		}
		
		public FontModel (String text) : this(text, getDefaultFont()) {
		}
		
		public FontModel () : this("null") {
		}
		
		public String getText() {return this.text;}
		
		public override void Dispose() {
			base.Dispose();
			this.text = null;
		}
	}
}

