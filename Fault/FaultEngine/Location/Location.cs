using System;

using Sce.PlayStation.Core;

namespace Fault {
	public class Location : Locateable, IDisposable {
		public static int INSTANCES;
		
		public static Location ValueOf(String st) {
			Location l = null;
			try {
				String[] comma_split = st.Split(',');
				st = StringUtils.join(comma_split, " ");
				
				String[] parts = st.Split(' ');
				
				double x = 0;
				double y = 0;
				double z = 0;
				double pitch = 0;
				double yaw = 0;
				double roll = 0;
				
				if(parts.Length > 0) x = Convert.ToDouble(parts[0]);
				if(parts.Length > 1) y = Convert.ToDouble(parts[1]);
				if(parts.Length > 2) z = Convert.ToDouble(parts[2]);
				if(parts.Length > 3) pitch = Convert.ToDouble(parts[3]);
				if(parts.Length > 4) yaw = Convert.ToDouble(parts[4]);
				if(parts.Length > 5) roll = Convert.ToDouble(parts[5]);
				
				l = new Location(x,y,z,pitch,yaw,roll);
			} catch(Exception e) {
				throw new Exception("Invalid Location String", e);
			}
			return l;
		}
		
		public static Location lookAt(Location fro, Location to) {
			Location l = fro.clone();
			l.lookAt(to);
			return l;
		}
		
		//Variables
		private double x = 0;
		private double y = 0;
		private double z = 0;
		
		private double pitch = 0;
		private double yaw = 0;
		private double roll = 0;
		
		private Locateable owner;
		
		//Constructors
		public Location () : this(0,0) {
		}
		
		public Location (Locateable l) : this(0,0) {
			this.setOwner(l);
		}
		
		public Location (Vertice v) : this(v.getX(), v.getY(), v.getZ ()) {
		}
		
		public Location(Location l) : this(l.x, l.y, l.z, l.pitch, l.yaw, l.roll) {
		}
		
		public Location(double x, double y) : this(x, y, 0) {
		}
		
		public Location(double x, double y, double z) : this(x, y, z, 0, 0, 0) {
		}
		
		public Location(double x, double y, double z, double pitch, double yaw, double roll) {
			this.x = x;
			this.y = y;
			this.z = z;
			
			this.pitch = pitch;
			this.yaw = yaw;
			this.roll = roll;
			
			this.owner = null;
			
			INSTANCES++;
		}
		
		//Methods
		public double getX() {return this.x;}
		public double getY() {return this.y;}
		public double getZ() {return this.z;}
	    public double getPitch() {return this.pitch;}
	    public double getYaw() {return this.yaw;}
	    public double getRoll() {return this.roll;}
		//Radians
	    public double getRPitch() {return MathUtils.toRadians(this.pitch);}
	    public double getRYaw() {return MathUtils.toRadians(this.yaw);}
	    public double getRRoll() {return MathUtils.toRadians(this.roll);}
		//Conversion
		public Vector3 getAsVector() {return new Vector3((float)this.x, (float)this.y, (float)this.z);}
		public Vertice getAsVertice() {return new Vertice(this);}
		public Locateable getOwner() {return this.owner;}
		public bool getHasOwner() {return getOwner() != null;}
		public Location getOwnerLocation() {return this.getOwner().getLocation();}
		Location Locateable.getLocation() {return this;}
		Locateable Locateable.getParent() {return getOwner();}
		public Location getAbsoluteLocation () {
			if(this.getOwner() != null && this.getOwner().getParent() != null) {
				Location parentAbs = this.getOwner().getParent().getLocation().getAbsoluteLocation();
				Location l = this.clone().add(parentAbs);
				parentAbs.Dispose();
				//if(!this.Equals(this.getOwner().getLocation())) l.add (this);
				return l;
			}
			return this.clone();
		}
    
	    public Location setX(double x) {this.x = x; return this;}
	    public Location setY(double y) {this.y = y; return this;}
	    public Location setZ(double z) {this.z = z; return this;}    
	    public Location setPitch(double pitch) {this.pitch = pitch; return this;}
	    public Location setYaw(double yaw) {this.yaw = yaw; return this;}
	    public Location setRoll(double roll) {this.roll = roll; return this;}
		public Location setOwner(Locateable parent) {this.owner = parent; return this;}
		
	    public Location addX(double x) {this.x += x; return this;}
	    public Location addY(double y) {this.y += y; return this;}
    	public Location addZ(double z) {this.z += z; return this;}
	    public Location addYaw(double yaw) {this.yaw += yaw; return this;}
	    public Location addPitch(double pitch) {this.pitch += pitch; return this;}
	    public Location addRoll(double roll) {this.roll += roll; return this;}
		
		public Location add(Location l) {if(l == null) {return this;} this.x += l.x; this.y += l.y; this.z += l.z; this.pitch += l.pitch; this.yaw += l.yaw; this.roll += l.roll; return this;}
		public Location add(double x, double y, double z) {this.x += x; this.y += y; this.z += z; return this;}
		
		public Location sub(Location l) {this.x -= l.x; this.y -= l.y; this.z -= l.z; this.pitch -= l.pitch; this.yaw -= l.yaw; this.roll -= l.roll; return this;}
		
		public Location set(Location l) {this.x = l.x; this.y = l.y; this.z = l.z; this.pitch = l.pitch; this.yaw = l.yaw; this.roll = l.roll; return this;}
    	public Location set(double x, double y, double z) {this.x = x; this.y = y; this.z = z; return this;}
		public Location set(Locateable l) {return set (l.getLocation());}
		
		public Location divide(double amt) {this.x /= amt; this.y /= amt; this.z /= amt; this.pitch /= amt; this.yaw /= amt; this.roll /= amt; return this;}
		public Location divide(double amt, bool rotationMatrix) {if(rotationMatrix) return divide(amt); this.x /= amt; this.y /= amt; this.z /= amt; return this;}
		public Location multiply(double amt) {this.x *= amt; this.y *= amt; this.z *= amt; this.pitch *= amt; this.yaw *= amt; this.roll *= amt; return this;}
		
    	public Location clone() {return new Location(this);}
		
		public Quaternion getRotationQuaternion() {
			/*Quaternion rot = Quaternion.RotationXyz(
				(float) this.pitch * FMath.DegToRad,
				(float) this.yaw * FMath.DegToRad,
				(float) this.roll * FMath.DegToRad
			);*/
			Quaternion rot = Quaternion.RotationZyx(
				(float) this.getRPitch(),
				(float) this.getRYaw(),
				(float) this.getRRoll()
			);
			return rot;
		}
		
		public Matrix4 getPositionMatrix() {
			Matrix4 mat = Matrix4.Translation(
				(float) this.x,
				(float) this.y,
				(float) this.z
			);
			return mat;
		}
		
		public double getDistance(Location between) {
			Location abs = this.getAbsoluteLocation();
			Location bAbs = between.getAbsoluteLocation();
			double dist = Math.Sqrt(
				Math.Pow(abs.x - bAbs.x, 2) +
				Math.Pow(abs.y - bAbs.y, 2) +
				Math.Pow(abs.z - bAbs.z, 2)
			);
			
			abs.Dispose();
			bAbs.Dispose();
			
			return dist;
		}
		
		public double getDistanceX(Location between) {
			Location abs = this.getAbsoluteLocation();
			Location bAbs = between.getAbsoluteLocation();
			double dist = bAbs.x - abs.x;
			abs.Dispose();
			bAbs.Dispose();
			return dist;
		}
		public double getDistanceY(Location between) {
			Location abs = this.getAbsoluteLocation();
			Location bAbs = between.getAbsoluteLocation();
			double dist = bAbs.y - abs.y;
			abs.Dispose();
			bAbs.Dispose();
			return dist;
		}
		public double getDistanceZ(Location between) {
			Location abs = this.getAbsoluteLocation();
			Location bAbs = between.getAbsoluteLocation();
			double dist = bAbs.z - abs.z;
			abs.Dispose();
			bAbs.Dispose();
			return dist;
		}
		
		public Location getRelative(double x, double y, double z) {
			return this.getAbsoluteLocation().add(x, y, z);
		}
		
		public Location getRelativeInFacingDirection(double distance) {
			//TODO: Add in Parent look matrix
			double pitch = this.getPitch();
			double yaw = this.getYaw();
			while(pitch > 360) pitch -= 360;
			while(pitch < 0) pitch += 360;
			while(yaw > 360) yaw -= 360;
			while(yaw < 0) yaw += 360;
			
			pitch = MathUtils.toRadians(pitch + 0.00001);
			yaw = MathUtils.toRadians(yaw + 0.00001);
			
			double adjacent = Math.Cos(pitch) *  distance;
			double opposite = Math.Tan(pitch) * adjacent;
			
			double z = (Math.Cos(yaw) * adjacent);
			double x = Math.Tan(yaw) * z;
			
			Location t = this.getAbsoluteLocation();
			t.addX(x);
			t.addY(-opposite);
			t.addZ(z);
			return t;
		}
		
		public Location getCrossProduct(Location between) {
			Location thisabs = this;
			Location betabs = between;
			double x = thisabs.getY() * betabs.getZ() - thisabs.getZ() * betabs.getY();
			double y = thisabs.getZ() * betabs.getX() - thisabs.getX() * betabs.getZ();
			double z = thisabs.getX() * betabs.getY() - thisabs.getY() * betabs.getX();
			//thisabs.Dispose();
			//betabs.Dispose();
			return new Location(x, y, z);
		}
		
		public Location lookAt(Location l) {
			return this.lookAt(l, 1.0);
		}
		
		public Location lookAt(Location l, double multiplier) {
			return this.yawLookAt(l, multiplier).pitchLookAt(l, multiplier);
		}
		
		public Location yawLookAt(Location l) {
			return this.yawLookAt(l, 1.0);
		}
		
		public Location pitchLookAt(Location l) {
			return this.pitchLookAt(l, 1.0);
		}
		
		public Location yawLookAt(Location l, double multiplier) {
			double yaw = this.determineYaw(l);
			yaw *= multiplier;
			this.setYaw(yaw);
			return this;
		}
		
		public Location pitchLookAt(Location l, double multiplier) {
			double pitch = this.determinePitch(l);
			pitch *= multiplier;
			this.setPitch(pitch);
			return this;
		}
		
		public double determinePitch(Location target, double offset = 90) {
			double dX = this.getDistanceX(target);
			double dY = this.getDistanceY(target);
			double dZ = this.getDistanceZ(target);
			double pitch = Math.Atan2(Math.Sqrt(dZ * dZ + dX * dX), dY) + Math.PI;
			pitch = MathUtils.toDegrees(pitch) + offset;
			if(this.getOwner() != null && this.getOwner().getParent() != null) {
				Location pAbs = this.getOwner().getParent().getLocation().getAbsoluteLocation();
				pitch -= pAbs.getPitch();
				pAbs.Dispose();
			}
			return pitch;
		}
		
		public double determineYaw(Location target, double offset = 90) {
			double dX = this.getDistanceX(target);
			double dZ = this.getDistanceZ(target);
			double yaw = Math.Atan2(dZ, dX);
			yaw = (-MathUtils.toDegrees(yaw)) + offset;
			if(this.getOwner() != null && this.getOwner().getParent() != null) {
				Location pAbs = this.getOwner().getParent().getLocation().getAbsoluteLocation();
				yaw -= pAbs.getYaw();
				pAbs.Dispose();
			}
			return yaw;
		}
		
		//If x > this.x return;, else set X x
		public Location enforceX(double x) {
			if(x > 0) {
				if(x <= this.getX()) return this;
			} else if(x < 0) {
				if(x >= this.getX()) return this;
			}
			this.setX(x);
			return this;
		}
		
		public Location enforceZ(double z) {
			if(z > 0) {
				if(z <= this.getX()) return this;
			} else if(z < 0) {
				if(z >= this.getZ()) return this;
			}
			this.setZ(z);
			return this;
		}
		
		public Location limit(double amt) {
			if(this.x > amt) this.x = amt;
			if(this.y > amt) this.y = amt;
			if(this.z > amt) this.z = amt;
			if(this.x < -amt) this.x = -amt;
			if(this.y < -amt) this.y = -amt;
			if(this.z < -amt) this.z = -amt;
			return this;
		}
		
		public bool betweenX(double a, double b) {
			double min = Math.Min(a, b);
			double max = Math.Max(a, b);
			return this.getX() <= max && this.getX() >= min;
		}
		
		public bool betweenY(double a, double b) {
			double min = Math.Min(a, b);
			double max = Math.Max(a, b);
			return this.getY() <= max && this.getY() >= min;
		}
		
		public bool betweenZ(double a, double b) {
			double min = Math.Min(a, b);
			double max = Math.Max(a, b);
			return this.getZ() <= max && this.getZ() >= min;
		}
		
		public override string ToString () {
			return "" + this.getX() + "," + this.getY() + "," + this.getZ () + "," + this.getYaw() + "," + 
				this.getPitch() + "," + this.getRoll();
		}
		
		public void Dispose () {
			this.owner = null;
			
			INSTANCES--;
		}
	}
}

