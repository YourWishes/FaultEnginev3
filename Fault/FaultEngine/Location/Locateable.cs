using System;

namespace Fault {
	public interface Locateable {
		Location getLocation();
		Locateable getParent();
	}
}