using System;

namespace Fault {
	public abstract class InputElement : GUIElement {
		private int tabIndex;
		
		public InputElement (GUI gui) : base(gui) {
			this.tabIndex = gui.getNextTabIndex();
			gui.addElement(this);
		}
		
		public int getTabIndex() {return this.tabIndex;}
		
		public InputElement setTabIndex(int tabIndex) {this.tabIndex = tabIndex; return this;}
	}
}

