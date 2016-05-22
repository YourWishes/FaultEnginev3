using System;
using System.Collections.Generic;

using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

namespace Fault {
	public class Button : InputElement {
		private Texture2D up;
		private Texture2D hover;
		private Texture2D down;
		private Color color;
		
		private List<TouchData> oldData;
		
		private bool isHovered = false;
		private bool isDown = false;
		
		private Sprite sprite;
		
		public Button (GUI gui) : this(gui, null) {
		}
		
		public Button(GUI gui, Texture2D texture) : this(gui, texture, null, null) {
			
		}
		
		public Button(GUI gui, Texture2D up, Texture2D hover, Texture2D down) : base(gui) {
			this.up = up;
			this.hover = hover;
			this.down = down;
			this.color = Color.WHITE.clone();
		}
		
		public Texture2D getUp() {return this.up;}
		public Texture2D getHover() {return this.hover;}
		public Texture2D getDown() {return this.down;}
		
		public override double getWidth () {return (sprite != null ? sprite.getWidth() : (getTexture() != null ? getTexture().Width : 0));}
		public override double getHeight () {return (sprite != null ? sprite.getHeight() : (getTexture() != null ? getTexture().Height : 0));}
		public override Color getColor () {return this.color;}
		
		private Texture2D getTexture() {
			if(!isDown && !isHovered && up != null) return up;
			if(isDown && down != null) return down;
			if(isHovered && hover != null) return hover;
			if(up != null) return up;
			if(down != null) return down;
			if(hover != null) return hover;
			return null;
		}
		
		private void updateSprite() {
			lock(this) {
				if(this.sprite == null) {
					if(this.getTexture() != null) {
						this.sprite = new Sprite(getTexture(), "Button");
						this.sprite.getMaterial().setColor(this.color);
					}
				} else {
					this.sprite.getMaterial().setColor(this.color);
					if(getTexture() != null && getTexture() != this.sprite.getTexture()) {
						lock(this.getTexture()) {
							this.sprite.setTexture(getTexture());
						}
					} else if(getTexture() != this.sprite.getTexture()) {
						DisplayManager.getDisplayManager().removeModel(this.sprite);
						this.sprite.getVBO().Dispose();
						this.sprite.Dispose();
						this.sprite = null;
					}
				}
			}
		}
		
		public override void onGUIShow() {
			base.onGUIShow();
			this.updateSprite();
			if(this.sprite != null && !this.getGUI().getScene().getModels().Contains(this.sprite)) this.getGUI().getScene().addModel(this.sprite);
		}
		
		public override void onGUIHide () {
			base.onGUIHide ();
			if(this.sprite != null) {
				DisplayManager.getDisplayManager().removeModel(this.sprite);
				this.sprite.getVBO().Dispose();
				this.sprite.Dispose();
				this.sprite = null;
			}
		}
		
		public virtual void onPress(TouchData td) {this.isDown = true; this.updateSprite();}
		public virtual void onHold(TouchData td) {this.isDown = true; this.updateSprite();}
		public virtual void onRelease(TouchData td) {this.isDown = false; this.updateSprite();}
		public virtual void onCancelling(TouchData td) {this.isDown = false; this.updateSprite();}
		
		public override void tick () {
			base.tick ();
			this.updateSprite();
			
			//Update Location
			if(this.sprite != null) {
				lock(this.sprite) {
					this.sprite.getLocation().set(this.getLocation());
					if(!this.getGUI().getScene().getModels().Contains(this.sprite)) this.getGUI().getScene().addModel(this.sprite);
				}
			}
			
			if(oldData != null &&Game.GAME_INSTANCE.getTouchData().Equals(oldData)) return;
			oldData = Game.GAME_INSTANCE.getTouchData();
			int touching = 0;
			foreach(TouchData td in Game.GAME_INSTANCE.getTouchData()) {
				float x = td.X;
				float y = td.Y;
				x += 0.5f;
				y += 0.5f;
				x *= (float)DisplayManager.getDisplayManager().getCamera().getWidth();
				y *= (float)DisplayManager.getDisplayManager().getCamera().getHeight();
				Location l = new Location(x,y);
				
				if(isInButton(l)) {
					touching++;
					if(td.Status.Equals(TouchStatus.Down)) {
						this.onPress(td);
					} else if(td.Status.Equals(TouchStatus.Move)) {
						this.onHold(td);
					} else if(td.Status.Equals(TouchStatus.Up)) {
						this.onRelease(td);
					} else if(td.Status.Equals(TouchStatus.Canceled)) {
						this.onCancelling(td);
					}
				}
			}
			
			if(this.isDown && touching < 1) {
				this.isDown = false;
				this.updateSprite();
			}
		}
		
		public bool isInButton(Location l) {
			if(this.sprite == null) return false;
			if(this.sprite.getTexture() == null) return false;
			if(this.sprite.getWidth() <= 0) return false;
			if(this.sprite.getHeight() <= 0) return false;
			
			l = l.getAbsoluteLocation();
			
			Location sl = this.sprite.getLocation().getAbsoluteLocation();
			double minX = sl.getX();
			double minY = sl.getY();
			double maxX = minX + this.sprite.getWidth();
			double maxY = minY + this.sprite.getHeight();
			
			bool xVal = minX <= l.getX() && maxX >= l.getX();
			bool yVal = minY <= l.getY() && maxY >= l.getY();
			sl.Dispose();
			l.Dispose();
			return xVal && yVal;
		}
		
		public override void Dispose () {
			base.Dispose();
			if(this.sprite != null) {
				DisplayManager.getDisplayManager().removeModel(this.sprite);
				if(this.sprite.getVBO() != null) this.sprite.getVBO().Dispose();
				this.sprite.Dispose();
			}
			
			up = null;
			hover = null;
			down = null;
			sprite = null;
		}
	}
}

