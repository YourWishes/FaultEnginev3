using System;

namespace Fault {
	public interface CollisionListener {
		void onCollisionAttempt(Entity entity);
	}
}

