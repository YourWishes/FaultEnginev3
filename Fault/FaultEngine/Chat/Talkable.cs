using System;

namespace Fault {
	public interface Talkable {
		Model getModel();
		
		void onMessageDisplay(ChatMessage message);
		void onMessageComplete(ChatMessage message);
	}
}

