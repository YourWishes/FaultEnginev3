using System;

using Sce.PlayStation.Core.Graphics;

namespace Fault {
	public class ChatMessage {
		private String message;
		private Talkable talker;
		
		public ChatMessage(String message, Talkable talker) {
			this.message = message;
			this.talker = talker;
		}
		
		public String getMessage() {return this.message;}
		public Talkable getTalker() {return this.talker;}
		
		public virtual void onSet(Talkable toWho) {
			this.talker.onMessageDisplay(this);
		}
		public virtual void onRead() {
			this.talker.onMessageComplete(this);
		}
	}
}

