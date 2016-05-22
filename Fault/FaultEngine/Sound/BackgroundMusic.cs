using System;
using System.Collections.Generic;
using System.Threading;

using Sce.PlayStation.Core.Audio;

namespace Fault {
	public class BackgroundMusic : Music {
		private static List<BackgroundMusic> LOADED_MUSICS = new List<BackgroundMusic>();
		public static List<BackgroundMusic> getLoadedBackgroundMusics() {
			lock(LOADED_MUSICS) {
				return new List<BackgroundMusic>(LOADED_MUSICS);
			}
		}
		
		private Bgm backgroundMusic;
		private BgmPlayer player;
		
		private int volume = 100;
		
		public BackgroundMusic (String resource) {
			lock(LOADED_MUSICS) {
				LOADED_MUSICS.Add(this);
			}
			
			this.backgroundMusic = new Bgm(resource);
			this.player = this.backgroundMusic.CreatePlayer();
		}
		
		public Bgm getBGM() {return this.backgroundMusic;}
		public BgmPlayer getBGMPlayer() {return this.player;}
		public int getVolume() {return this.volume;}
		
		public bool isPlaying() {return getBGMPlayer().Status.Equals(BgmStatus.Playing);}
		public bool isPaused() {return getBGMPlayer().Status.Equals(BgmStatus.Paused);}
		public bool isStopped() {return getBGMPlayer().Status.Equals(BgmStatus.Stopped);}
		
		public void play() {getBGMPlayer().Play();}
		public void pause() {getBGMPlayer().Pause();}
		public void stop() {getBGMPlayer().Stop();}
		
		public void setVolume(int vol) {this.volume = vol;}
		
		public void tick() {
			double amt = GameSettings.getSettings().getSetting("music_volume", 100).getValueAsDouble() / 100.0;
			if(this.isPlaying()) {
				double j = (double)this.volume * amt;
				j /= 100;
				this.getBGMPlayer().Volume = (float)j;
			}
		}
		
		public void Dispose() {
			lock(LOADED_MUSICS) {
				LOADED_MUSICS.Remove(this);
			}
			
			this.backgroundMusic.Dispose();
			this.player.Dispose();
			
			this.backgroundMusic = null;
			this.player = null;
		}
	}
}

