using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace Fault {
	public class Model : Locateable, IDisposable {	
		
		//Render Variables
		private static Vector3[] lightPositions;
		private static Vector3[] lightColors;
		private static float[] lightIntensities;
		
		public static void newFrame(Camera c) {
			List<Light> lights = getDisplayManager().getLights();
			lightPositions = new Vector3[lights.Count];
			lightColors = new Vector3[lights.Count];
			lightIntensities = new float[lights.Count];
			for(int i = 0; i < lights.Count; i++) {
				if(lights[i] == null) continue;
				if(lights[i].getLocation() == null) continue;
				lock(lights[i]) {
					lightPositions[i] = Matrix4.TransformPoint(c.getViewMatrix(), lights[i].getLocation().getAsVector());
					lightColors[i] = lights[i].getColor().getAsVector3();
					lightIntensities[i] = lights[i].getIntensity();
				}
			}
		}
		
		public static DisplayManager getDisplayManager() {return DisplayManager.getDisplayManager();}
		
		//Instance
		private List<Face> faces;
		private VBO vertexBuffer = null;
		private ShaderProgram shaderProgram;
		private int size;
		private Location location;
		private Material material = new Material();
		private List<Model> children = new List<Model>();
		private String name;
		private Model parent;
		public Bounding bounds = null;
		
		private double scaleX = 1;
		private double scaleY = 1;
		private double scaleZ = 1;
		
		private Location min;
		private Location max;
		
		public Model(Model m) : this() {
			this.faces = m.getFaces();
			this.vertexBuffer = m.vertexBuffer;
			this.shaderProgram = m.shaderProgram;
			this.size = m.size;
			this.location = m.getLocation().clone();
			this.location.setOwner(this);
			this.material = m.material;
			this.children = m.getChildren();
			this.name = m.name;
			this.bounds = m.bounds != null ? m.bounds.clone() : null;
			if(this.bounds != null) this.bounds.setModel(this);
			
			this.scaleX = m.scaleX;
			this.scaleY = m.scaleY;
			this.scaleZ = m.scaleZ;
			
			this.min = m.min != null ? m.min.clone() : null;
			this.max = m.max != null ? m.max.clone() : null;
		}
		
		public Model () {
			this.faces = new List<Face>();
			this.children = new List<Model>();
			this.shaderProgram = ShaderList.getDefaultShaderProgram();
			
			this.location = new Location();
			this.location.setOwner(this);
			this.parent = null;
		}
		
		public List<Face> getFaces() {return new List<Face>(this.faces == null ? new List<Face>() : this.faces);}
		public VBO getVBO() {return this.vertexBuffer;}
		public Location getLocation() {return this.location;}
		public Material getMaterial() {return this.material;}
		public double getScaleX() {return this.scaleX;}
		public double getScaleY() {return this.scaleY;}
		public double getScaleZ() {return this.scaleZ;}
		public ShaderProgram getShaderProgram() {return this.shaderProgram;}
		public GraphicsContext getGraphics() {return getDisplayManager().getGraphicsContext();}
		public List<Model> getChildren() {return new List<Model>(this.children);}
		Location Locateable.getLocation() {return getLocation();}
		Locateable Locateable.getParent() {return getParent();}
		public Bounding getBounds() {return this.bounds;}
		public String getName() {return this.name;}
		public Model getParent() {return this.parent;}
		
		//Related to bounding/sizing stuff
		public Location getMinPoint() {return this.min;}
		public Location getMaxPoint() {return this.max;}
		public Location[] getAbsoluteMinMaxPoints() {
			/*
			 * This will check children for their min X Y and Z values RELATIVE TO THIS OBJECT
			 * THE RETURNED VALUE WILL BE RELATIVE TO THIS OBJECT, EXAMPLE;
			 * THIS OBJECT IS AT 14, 14, 14
			 * THIS MIN POINT IS -4, -2, -4 (MAKING ITS ABS POSITION AT 10, 12, 10)
			 * THE CHILD IS AT 3, 3, 3 OF THIS, MAKING ITS ABS 17, 17, 17
			 * THE CHILD MIN POINT IS -6, -6, -6 OF IT, MAKING ITS POSITION RELATIVE TO ITS OBJECT -3, -3, -3
			 * THAT MAKES THE RELATAIVE MIN POINT -4, -3, -4 (X FROM THIS MIN, Y FROM CHILD MIN, Z FROM THIS MIN)
			 * THAT MAKES THE ABSOLUTE MIN POINT AT 10, 11, 10
			 * 
			 * RETURNS ARRAY TO INCREASE EFFICIENCY, BE SURE TO DISPOSE BOTH
			 * getAbsoluteMinMaxPoints()[0] = min_point;
			 * getAbsoluteMinMaxPoints()[1] = max_point;
			 */
			double minX = (this.min == null ? 0 : this.min.getX());
			double minY = (this.min == null ? 0 : this.min.getY());
			double minZ = (this.min == null ? 0 : this.min.getZ());
			double maxX = (this.max == null ? 0 : this.max.getX());
			double maxY = (this.max == null ? 0 : this.max.getY());
			double maxZ = (this.max == null ? 0 : this.max.getZ());
			
			foreach(Model child in this.children) {
				Location[] minMaxPoints = child.getAbsoluteMinMaxPoints();
				Location minPoint = minMaxPoints[0];
				Location maxPoint = minMaxPoints[1];
				minPoint.add(child.getLocation());
				maxPoint.add(child.getLocation());
				//COMPARE VALUES
				if(minPoint.getX () < minX) minX = minPoint.getX();
				if(minPoint.getY () < minY) minY = minPoint.getY();
				if(minPoint.getZ () < minZ) minZ = minPoint.getZ();
				//MAX VALUES
				if(maxPoint.getX () > maxX) maxX = maxPoint.getX();
				if(maxPoint.getY () > maxY) maxY = maxPoint.getY();
				if(maxPoint.getZ () > maxZ) maxZ = maxPoint.getZ();
				
				//Clean.. soo clean
				minPoint.Dispose();
				maxPoint.Dispose();
			}
			Location aMinPoint = new Location(minX, minY, minZ);
			Location aMaxPoint = new Location(maxX, maxY, maxZ);
			
			return new Location[]{aMinPoint, aMaxPoint};
		}
		public List<Model> getChildren(bool deep) {
			if(!deep) return getChildren();
			List<Model> children = this.getChildren();
			foreach(Model m in new List<Model>(children)) {
				children.AddRange(m.getChildren(deep));
			}
			return children;
		}
		
		public virtual Material setMaterial(Material m) {this.material = m; return this.material;}
		public void setScale(double scale) {this.setScaleX(scale); this.setScaleY(scale); this.setScaleZ(scale);}
		public double setScaleX(double j) {return this.scaleX = j;}
		public double setScaleY(double j) {return this.scaleY = j;}
		public double setScaleZ(double j) {return this.scaleZ = j;}
		public void setShaderProgram(ShaderProgram sp) {this.shaderProgram = sp;}
		public void setBounds(Bounding bs) {this.bounds = bs;}
		public void setName(String s) {this.name = s;}
		public void setParent(Model p) {if(this.parent != null) {this.parent.removeChild(this);} p.addChild(this);}
		public void setVBO(VBO vbo) {this.vertexBuffer = vbo;}
		
		public void addFace(Face f) {lock(this.faces) {this.faces.Add(f);}}
		public void addFaces(Face[] f) {for(int i = 0; i < f.Length; i++) {addFace(f[i]);}}
		public void addChild(Model m) {lock(this.children) {this.children.Add(m); if(m != null && m.parent != null) {m.parent.removeChild(this);} m.parent = this;}}
		
		public void removeFace(Face f) {lock(this.faces) {this.faces.Remove (f);}}
		public void removeChild(Model m) {lock(this.children) {this.children.Remove(m); m.parent = null;}}
		
		public Material cloneMaterial() {this.setMaterial(this.getMaterial().clone()); return this.getMaterial();}
		
		public List<Vertice> getAllVertices() {
			List<Vertice> verts = new List<Vertice>();
			lock(this.faces) {
				foreach(Face f in this.faces) {
					if(f == null) continue;
					lock(f) {
						verts.AddRange(f.getVertices());
					}
				}
			}
			return verts;
		}
		
		public List<TextureCoordinate> getAllTextureCoordinates() {
			List<TextureCoordinate> cords = new List<TextureCoordinate>();
			foreach(Face f in this.faces) {
				cords.AddRange(f.getTextureCoordinates());
			}
			return cords;
		}
		
		public Matrix4 getPositionalMatrix(Matrix4 offset) {
			lock(this.location) {
				Location l = this.getLocation().clone();
				Matrix4 positionMatrix = Matrix4.Transformation(
					l.getAsVector(),
					l.getRotationQuaternion(),
					new Vector3((float)this.scaleX, (float)this.scaleY, (float)this.scaleZ)
				);
				l.Dispose();
				return offset * positionMatrix;
			}
		}
		
		public bool isOnVBO() {return this.vertexBuffer != null && this.vertexBuffer.getVBO() != null;}
		
		public void putOnVBO() {
			if(this.isOnVBO()) return;
			double maxX = 0;
			double minX = 0;
			double maxY = 0;
			double minY = 0;
			double maxZ = 0;
			double minZ = 0;
			
			foreach(Vertice vertice in this.getAllVertices()) {
				if(vertice.getX() > maxX) maxX = vertice.getX();
				if(vertice.getY() > maxY) maxY = vertice.getY();
				if(vertice.getZ() > maxZ) maxZ = vertice.getZ();
				if(vertice.getX() < minX) minX = vertice.getX();
				if(vertice.getY() < minY) minY = vertice.getY();
				if(vertice.getZ() < minZ) minZ = vertice.getZ();
			}
			
			this.min = new Location(minX, minY, minZ);
			this.max = new Location(maxX, maxY, maxZ);
			
			this.min.setOwner(this.getLocation());
			this.max.setOwner(this.getLocation());
		
			this.vertexBuffer = new VBO(this);
			lock(this.vertexBuffer) {this.vertexBuffer.generateVBO();}
			if(this.isOnVBO()) {
				this.clean(false);
			}
		}
		
		public void newVBO() {
			this.vertexBuffer = null;
			this.putOnVBO();
		}
		
		public void addToVBOList() {
			getDisplayManager().addToWaitingList(this);
		}
		
		public void render(Camera c) {
			Location l = new Location();
			this.render(l.getPositionMatrix(), c);
			l.Dispose();
		}
		
		public void render(Matrix4 offset, Camera c) {
			Matrix4 my_offset = this.getPositionalMatrix(offset);
			
			if(this.faces.Count > 0) {
				if(!this.isOnVBO()) {
					this.putOnVBO();
				}
			}
			
			//Set onto VBO
			if(this.isOnVBO()) {
				getDisplayManager().getGraphicsContext().SetVertexBuffer(0, this.vertexBuffer.getVBO());
				
				if(this.getMaterial().getColor().a < 1 || this.getMaterial().isTextured()) {
					getGraphics().Enable(EnableMode.Blend);
				} else {
					getGraphics().Disable(EnableMode.Blend);
				}
				
				if(this.getMaterial().getCullFaced()) {
					getGraphics().Enable(EnableMode.CullFace);
				} else {
					getGraphics().Disable(EnableMode.CullFace);	
				}
				
				if(this.getMaterial().getDepthTested()) {
					getGraphics().Enable(EnableMode.DepthTest);
				} else {
					getGraphics().Disable(EnableMode.DepthTest);
				}
				
				//Set Shader and render mesh
				if(this.getMaterial().getRender3D()) {
					getDisplayManager().getGraphicsContext().SetShaderProgram(this.shaderProgram);
					this.setShaderVariables(offset, my_offset, c);
					this.renderMesh();
				} else if(this.getMaterial().getRender2D()) {
					getDisplayManager().getGraphicsContext().SetShaderProgram(this.shaderProgram);
					this.set2DShaderVariables(offset, my_offset, c);
					this.renderMesh();
				}
			}
			
			foreach(Model model in this.getChildren()) {
				model.render(my_offset, c);
			}
		}
		
		public void renderMesh(DrawMode drawMode) {
			if(!this.isOnVBO()) this.putOnVBO();
			DisplayManager dm = DisplayManager.getDisplayManager();
			dm.getGraphicsContext().DrawArrays(drawMode, 0, this.vertexBuffer.getVertexSize());
		}
		
		public void renderMesh() {
			this.renderMesh(DrawMode.Triangles);
		}
		
		public virtual void setShaderVariables(Matrix4 offset, Matrix4 my_offset, Camera c) {
			this.setDefaultshaderVariables(offset, my_offset, c);
		}
		
		public virtual void setDefaultshaderVariables(Matrix4 offset, Matrix4 my_offset, Camera c) {
			getDisplayManager().getGraphicsContext().SetTexture(0, this.getMaterial().getTexture());
			
			Matrix4 modelMatrix = c.getViewMatrix();
			Matrix4 projectionMatrix = c.getProjectionMatrix();
			Vector4 color = this.getMaterial().getColor().getAsVector();
			
			//Matrix's
			this.shaderProgram.SetUniformValue(0, ref modelMatrix);
			this.shaderProgram.SetUniformValue(1, ref projectionMatrix);
			this.shaderProgram.SetUniformValue(2, ref my_offset);
			
			//Material
			this.shaderProgram.SetUniformValue(3, ref color);
			
			//Externals
			this.shaderProgram.SetUniformValue(4, lightPositions.Length);
			this.shaderProgram.SetUniformValue(5, lightPositions, 0, 0, lightPositions.Length);
			this.shaderProgram.SetUniformValue(6, lightColors, 0, 0, lightColors.Length);
			this.shaderProgram.SetUniformValue(7, lightIntensities, 0, 0, lightIntensities.Length);
			
			//Ambient
			Vector3 ambientColor = getDisplayManager().getAmbientLight().getColor().getAsVector3();
			this.shaderProgram.SetUniformValue(8, ref ambientColor);
			this.shaderProgram.SetUniformValue(9, getDisplayManager().getAmbientLight().getIntensity());
			
			Vector3 skyColor = getDisplayManager().getClearColor().getAsVector3();
			this.shaderProgram.SetUniformValue(10, ref skyColor);
			
			this.getShaderProgram().SetUniformValue(11, this.getMaterial().getTexture() != null ? 1 : 0);
		}
		
		public virtual void set2DShaderVariables(Matrix4 offset, Matrix4 my_offset, Camera c) {
			getDisplayManager().getGraphicsContext().SetTexture(0, this.getMaterial().getTexture());
			
			Matrix4 projectionMatrix = c.get2DProjectionMatrix();
			
			Vector4 color = this.getMaterial().getColor().getAsVector();
			
			this.getShaderProgram().SetUniformValue(0, ref projectionMatrix);
			this.getShaderProgram().SetUniformValue(1, ref my_offset);
			this.getShaderProgram().SetUniformValue(2, ref color);
			this.getShaderProgram().SetUniformValue(3, this.getMaterial().getTexture() != null ? 1 : 0);
		}
		
		public virtual void Dispose() {
			if(this.location != null) this.location.Dispose();
			this.clean();
			
			this.vertexBuffer = null;
			this.location = null;
			this.faces = null;
			this.children = null;
			this.material = null;
			this.bounds = null;
			this.shaderProgram = null;
		}
		
		public void clean() {this.clean (false);}
		
		//Due to Memory Issues I'm adding the option to clean vertices
		public void clean(bool cleanChildren) {
			//Clone the List of Faces, clean the faces, then remove them
			foreach(Face f in this.getFaces()) {
				f.clean();
			}
			this.faces = new List<Face>();
			
			if(cleanChildren) {
				foreach(Model child in this.children) {
					lock(child) {child.clean(cleanChildren);}
				}
			}
		}
		
		public virtual Model clone() {return new Model(this);}
	}
}

