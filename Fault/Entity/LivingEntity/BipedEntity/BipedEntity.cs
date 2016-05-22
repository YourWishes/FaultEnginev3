using System;

namespace Fault {
	public class BipedEntity : LivingEntity {
		private static BoundingBox BIPED_ENTITY_HITBOX;
		
		//Instance
		private Material skin;
		private Material hairColor;
		
		private Model leftArm;
		private Model rightArm;
		private Model head;
		private Model body;
		private Model leftLeg;
		private Model rightLeg;
		private Model pelvis;
		private Model face;
		private Model hair;
		private Model helmet;
		private Model clothesTorso;
		private Model clothesLeftArm;
		private Model clothesRightArm;
		
		private BipedExpression expression;
		private HairStyle hairStyle = HairStyle.BOY_0;
		private HelmetStyle helmetStyle = HelmetStyle.HELMET_0;
		private ClothesStyle clothesStyle = ClothesStyle.ARMOUR_0;
		
		private Item weapon;
		private Model weaponModel;
		
		private double moveSpeed = 1.0;
		
		private double jumpTime = TimeUtils.getNow();
		private bool jumping = false;
		
		public BipedEntity (Scene scene) : this(scene, FaultEntityType.BIPED_ENTITY) {
		}
		
		public BipedEntity (Scene scene, EntityType type) : base(scene, type) {
			Model model = new Model();
			
			leftArm = getGame().getLoadedModel("BipedLeftArm").clone();
			rightArm = getGame().getLoadedModel("BipedRightArm").clone();
			head = getGame().getLoadedModel("BipedHead").clone();
			body = getGame().getLoadedModel("BipedTorso").clone();
			leftLeg = getGame().getLoadedModel("BipedLeftLeg").clone();
			rightLeg = getGame().getLoadedModel("BipedRightLeg").clone();
			pelvis = getGame().getLoadedModel("BipedPelvis").clone();
			face = getGame().getLoadedModel("BipedFace").clone();
			hair = hairStyle.getModel().clone();
			helmet = helmetStyle.getModel().clone();
			clothesTorso = clothesStyle.getModel().clone();
			clothesLeftArm = clothesStyle.getLeftArmSleeve().clone();
			clothesRightArm = clothesStyle.getRightArmSleeve().clone();
			
			skin = new Material();
			skin.setColor(Color.fromHex("#FFDFC4"));
			
			hairColor = new Material();
			hairColor.setColor(Color.TREE_TRUNK_BROWN.clone());
			
			leftArm.setMaterial(skin);
			rightArm.setMaterial(skin);
			head.setMaterial(skin);
			body.setMaterial(skin);
			leftLeg.setMaterial(skin);
			rightLeg.setMaterial(skin);
			pelvis.setMaterial(skin);
			face.cloneMaterial();
			hair.setMaterial(hairColor);
			helmet.cloneMaterial();
			clothesTorso.cloneMaterial();
			clothesLeftArm.setMaterial(clothesTorso.getMaterial());
			clothesRightArm.setMaterial(clothesTorso.getMaterial());
			
			model.addChild(head);
			model.addChild(body);
			body.addChild(leftArm);
			body.addChild(rightArm);
			model.addChild(leftLeg);
			model.addChild(rightLeg);
			model.addChild(pelvis);
			head.addChild(face);
			head.addChild(hair);
			head.addChild(helmet);
			body.addChild(clothesTorso);
			leftArm.addChild(clothesLeftArm);
			rightArm.addChild(clothesRightArm);
			
			this.setModel(model);
			model.setName("Biped");
			
			if(BipedEntity.BIPED_ENTITY_HITBOX == null) {
				BoundingBox sphere = BoundingBox.calculateBox(model);
				BipedEntity.BIPED_ENTITY_HITBOX = sphere;
				this.getModel().setBounds(sphere);
			} else {
				this.getModel().setBounds(BipedEntity.BIPED_ENTITY_HITBOX.clone());
				this.getModel().getBounds().setModel(this.getModel());
			}
			
			this.setExpression(BipedExpression.BLANK);
		}
		
		public double getMovementSpeed() {return this.moveSpeed;}
		public double getLastJumpTime() {return this.jumpTime;}
		public override Location getEyeLocation () {return this.getHead().getLocation();}
		public Material getSkinColor() {return this.skin;}
		public Material getHairColor() {return this.hairColor;}
		public BipedExpression getExpression() {return this.expression;}
		public Model getHead() {return this.head;}
		public Model getFace() {return this.face;}
		public Model getLeftLeg() {return this.leftLeg;}
		public Model getRightLeg() {return this.rightLeg;}
		public Model getLeftArm() {return this.leftArm;}
		public Model getRightArm() {return this.rightArm;}
		public Model getTorso() {return this.body;}
		public Model getPelvis() {return this.pelvis;}
		public Item getWeapon() {return this.weapon;}
		
		public void setExpression(BipedExpression expression) {this.expression = expression; face.getMaterial().setTexture(expression.getTexture());}
		public void setMovementSpeed(double speed) {this.moveSpeed = speed;}
		public void setWeapon(Item weapon) {this.weapon = weapon;}
		public void setJumping(bool t) {
			if(t) this.jumpTime = TimeUtils.getNow();
			this.jumping = t;
		}
		
		public bool isJumping() {return this.jumping;}
		
		public override void tick () {
			base.tick();
		}
		
		public override void Dispose () {
			base.Dispose();
			leftArm.Dispose();
			rightArm.Dispose();
			head.Dispose();
			body.Dispose();
			leftLeg.Dispose();
			rightLeg.Dispose();
			pelvis.Dispose();
			face.Dispose();
			
			skin = null;
			leftArm = null;
			rightArm = null;
			head = null;
			body = null;
			leftLeg = null;
			rightLeg = null;
			pelvis = null;
			face = null;
		}
	}
}

