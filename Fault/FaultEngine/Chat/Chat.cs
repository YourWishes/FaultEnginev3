using System;
using System.Collections.Generic;

using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

namespace Fault {
	public class Chat {
		private static Chat chat;
		public static Chat getChat() {
			if(chat != null) return chat;
			return chat = new Chat();
		}
		
		private static ThreadSafeFrameBuffer fb = null;
		private static ThreadSafeDepthBuffer db = null;
		private static LoadableTexture bufferTexture;
		public static ThreadSafeFrameBuffer getFrameBuffer(bool genbuffers=true) {if(genbuffers) {generateBuffers();} return fb;}
		
		private static void generateBuffers() {
			if(Chat.fb != null) return;
			Chat.fb = new ThreadSafeFrameBuffer();
			
			Chat.db = new ThreadSafeDepthBuffer(150,150);
			DisplayManager.getDisplayManager().addThreadSafeDepthBufferToMake(Chat.db);
			while(Chat.db.getDepthBuffer() == null) {continue;}
			
			Chat.bufferTexture = new LoadableTexture(Chat.db.getWidth(), Chat.db.getHeight());
			bufferTexture.loadNow();
			
			Chat.fb.setLoadableTextureToBind(Chat.bufferTexture);
			Chat.fb.setThreadSafeDepthBufferToBind(Chat.db);
			
			DisplayManager.getDisplayManager().addThreadSafeFrameBufferToMake(Chat.fb);
			while(Chat.fb.getFrameBuffer() == null) {continue;}
		}
		
		//Instance
		private Model chatModel;
		private Quad background;
		private Sprite characterFace;
		
		private List<ChatMessage> chatMessages;
		
		private List<String> strs;
		private List<FontModel> fontModels;
		private int current = 0;
		private ChatMessage currentMessage;
		
		private bool enterLastFrame = false;
		
		private Chat () {
			generateBuffers();
			this.chatMessages = new List<ChatMessage>();
			this.strs = new List<String>();
		}
		
		public ChatMessage getCurrentMessage() {return (this.chatMessages.Count > 0 ? this.chatMessages[0] : null);}
		public List<ChatMessage> getMessages() {return new List<ChatMessage>(this.chatMessages);}
		
		public void addMessage(ChatMessage message) {lock(this.chatMessages) {this.chatMessages.Add(message);}}
		
		public void removeMessage(ChatMessage message) {lock(this.chatMessages) {this.chatMessages.Remove(message);}}
		
		public void nextPage() {
			if(this.strs.Count < 1) {
				this.current = 0;
				this.currentMessage = null;
				return;
			}
			
			lock(this.fontModels) {
				foreach(FontModel fm in this.fontModels) {
					if(fm == null) continue;
					lock(fm) {
						this.chatModel.removeChild(fm);
						fm.getTexture().Dispose();
						fm.getVBO().Dispose();
						fm.Dispose();
					}
				}
			}
			
			this.current += 4;
			this.updateFonts();
		}
		
		//Main Thread Method
		public void updateTexture() {
			if(Chat.getFrameBuffer(false) == null) return;
			if(Chat.getFrameBuffer(false).getFrameBuffer() == null) return;
			List<Model> models = new List<Model>();
			
			Camera talkerCamera = new Camera();
			
			ChatMessage cm;
			if((cm = getCurrentMessage()) != null) {
				lock(cm) {
					Talkable talker = cm.getTalker();
					Model m = talker.getModel();
					talkerCamera.getTarget().set(m.getLocation());
					talkerCamera.getTarget().addY(1.75);
					
					Location j = talkerCamera.getTarget().getAbsoluteLocation();
					j.addYaw(-23);
					Location q = j.getRelativeInFacingDirection(3);
					j.Dispose();
					//j.addX(-1.25).addY(0).addZ(2.8);
					talkerCamera.getLocation().set(q);
					q.Dispose();
					lock(m) {
						models.Add(talker.getModel());
					}
				}
			}
			
			Color oldSky = DisplayManager.getDisplayManager().getClearColor();
			lock(talkerCamera) {
				DisplayManager.getDisplayManager().setClearColor(Color.RED.clone().setAlpha(0));
				DisplayManager.getDisplayManager().renderToBuffer(Chat.getFrameBuffer().getFrameBuffer(), models, talkerCamera);
				DisplayManager.getDisplayManager().setClearColor(oldSky);
			}
			talkerCamera.Dispose();
		}
		
		public void updateFonts() {
			this.fontModels = new List<FontModel>();
			
			lock(this.fontModels) {
				if(this.current >= this.strs.Count) {
					this.strs = new List<String>();
					this.currentMessage.onRead();
					this.chatMessages.Remove(this.currentMessage);
					this.currentMessage = null;
					this.current = 0;
					return;
				}
			
				for(int i = this.current; i < this.strs.Count; i++) {
					if((i - this.current) > 3) break;
					FontModel fm = new FontModel(strs[i]);
					int padding = 16;
					fm.getLocation().setX (-background.getScaleX()/2.0 + this.characterFace.getWidth() + padding);
					fm.getLocation().setY(-background.getScaleY()/2.0 + padding + ((i - this.current) * fm.getHeight()));
					chatModel.addChild(fm);
					this.fontModels.Add(fm);
				}
			}
		}
		
		public void tick() {			
			if(chatModel == null && this.chatMessages.Count > 0) {
				Scene.getActiveScene().pause();
				chatModel = new Model();
				chatModel.getMaterial().setRender2D(true);
				chatModel.getMaterial().setRender3D(false);
				
				Camera cmr;
				lock(cmr = Camera.getCamera()) {
					background = new Quad();
					background.setScaleX(cmr.getWidth());
					background.getMaterial().setRender2D(true);
					background.getMaterial().setRender3D(false);
					background.setScaleY(150);
					background.getMaterial().setColor(Color.BLUE.clone().setAlpha(0.1));
					
					chatModel.addChild(background);
					
					chatModel.getLocation().setX(cmr.getWidth() / 2.0);
					chatModel.getLocation().setY(cmr.getHeight() - background.getScaleY() / 2.0);
				}
				
				Sprite cu = this.characterFace = new Sprite(Chat.getFrameBuffer().getLoadableTexture().getTexture());
				cu.flipY();
				cu.updateFace();
				cu.getLocation().setX(-background.getScaleX()/2.0);
				cu.getLocation().setY(-cu.getHeight()/2.0);
				chatModel.addChild(cu);
				
				Scene.getActiveScene().addModel(chatModel);
			}
			
			if(this.chatMessages.Count < 1 && chatModel != null) {
				Scene.getActiveScene().removeModel(chatModel);
				
				background.Dispose();
				characterFace.Dispose();
				chatModel.Dispose();
				
				background = null;
				characterFace = null;
				chatModel = null;
				Scene.getActiveScene().resume();
			}
			
			if(chatModel != null) {
				Scene.getActiveScene().removeModel(chatModel);
				
				if(getCurrentMessage() != this.currentMessage) {
					if(this.fontModels != null) {
						lock(this.fontModels) {
							foreach(FontModel fm in this.fontModels) {
								if(fm == null) continue;
								lock(fm) {
									fm.getTexture().Dispose();
									fm.getVBO().Dispose();
									fm.Dispose();
								}
							}
						}
						this.fontModels = null;
					} else {
						ChatMessage c = this.currentMessage = getCurrentMessage();
						String[] s = c.getMessage().Split('\n');
						foreach(String ss in s) {
							this.strs.Add(ss);
						}
						this.updateFonts();
					}
				}
				
				Scene.getActiveScene().addModel(chatModel);
				
				if(this.strs.Count > 0) {
					if(Game.GAME_INSTANCE.isButtonDown(GamePadButtons.Enter) && !this.enterLastFrame) {
						this.nextPage();
					}
				}
			}
			
			this.enterLastFrame = Game.GAME_INSTANCE.isButtonDown(GamePadButtons.Enter);
		}
	}
}

