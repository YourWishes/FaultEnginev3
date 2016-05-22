using System;
using System.Collections.Generic;

namespace Fault {
	public class WavefrontLoader : ModelLoader {
		public int BAD_ID;
		
		public WavefrontLoader () : base() {
		}
		
		public override List<Model> loadModels (string data) {
	        String[] lines = removeBlankElements(data.Replace("\r","\n").Split('\n'));
	        
	        List<Model> models = new List<Model>();
	        Model m = null;
			Model g = null;
			
			List<Material> materials = new List<Material>();
        	
        	List<Vertice> loadedVertices = new List<Vertice>();
        	List<TextureCoordinate> loadedTextureCoords = new List<TextureCoordinate>();
        	List<Material> loadedMaterials = new List<Material>();
        
	        for(int i = 0; i < lines.Length; i++) {
	            String line = lines[i];
	            
	            BAD_ID = i;
				
				if(line.StartsWith("ml")) {
					line = getLine(line, "ml");
					if(m == null) continue;
					Location location = Location.ValueOf(line);
					m.getLocation().set(location);
					continue;
				}
	            
	            if(line.StartsWith("o")) {
	                line = getLine(line, "o");
	                m = new Model();
					m.setName(line);
	                models.Add (m);
					g = null;
					continue;
	            }
				
				if(line.StartsWith("usemtl")) {
					line = getLine (line, "usemtl");
					Material guess = null;
					foreach(Material mat in materials) {
						if(mat.getName() != null && mat.getName().Equals(line)) {
							guess = mat;
							break;
						}
					}
					if(guess == null) {
						guess = new Material();
						guess.setName(line);
						materials.Add(guess);
					}
					m.setMaterial(guess);
				}
	            
	            if(line.StartsWith("s")) {
	                line = getLine(line, "s");
					if(g == null) {
						g = new Model();
						models.Add(g);
					}
					Material mat = g.getMaterial();
	                m = new Model();
	                m.setMaterial(mat);
	                g.addChild(m);
					continue;
	            }
	            
	            if(line.StartsWith("g")) {
	                line = getLine(line, "g");
					g = new Model();
					models.Add(g);
					m = g;
					continue;
	            }
	            
	            if(line.StartsWith("mtllib")) {
					continue;
	            }
	            
	            if(line.StartsWith("usemtl")) {
					continue;
	            }
				
	            if(line.StartsWith("vt")) {
	                line = getLine(line, "vt");
	                String[] coords = removeBlankElements(line.Split(' '));
	                TextureCoordinate tc = new TextureCoordinate(Convert.ToDouble(coords[0]),Convert.ToDouble(coords[1]));
	                loadedTextureCoords.Add(tc);
	                continue;
	            }
	            
	            if(line.StartsWith("vn")) {
	                continue;
	            }
	            
	            if(line.StartsWith("v")) {
	                line = getLine(line, "v");
	                String[] coordinates = removeBlankElements(line.Split(' '));
	                Vertice v = new Vertice();
	                v.set(Convert.ToDouble(coordinates[0]),Convert.ToDouble(coordinates[1]),Convert.ToDouble(coordinates[2]));
	                loadedVertices.Add(v);
	                continue;
	            }
	            
	            if(line.StartsWith("f")) {
	                line = getLine(line, "f");
	                String[] vertIndexs = removeBlankElements(line.Split(' '));
	                
					if(m == null) {
						m = new Model();
						models.Add(m);
					}
					
	                List<Vertice> selectedVertices = new List<Vertice>();
	                List<TextureCoordinate> selectedTextureCoordinates = new List<TextureCoordinate>();
	                
	                for(int index = 0; index < vertIndexs.Length; index++ ) {
	                    String indexLine = vertIndexs[index];
	                    
	                    Vertice selectedVertice = null;
	                    TextureCoordinate selectedTextureCoordinate = null;
	                    
	                    if(indexLine.Contains("//")) {
	                        //Form of v1//vn1
	                        String[] parts = indexLine.Split(new String[]{"//"}, StringSplitOptions.RemoveEmptyEntries);
	                        selectedVertice = loadedVertices[Convert.ToInt32(parts[0]) - 1];
	                    } else if(indexLine.Contains("/")) {
	                        String[] requesting = indexLine.Split('/');
	                        
	                        if(requesting.Length == 2) {
	                            //Form of v1/vt1
	                            selectedVertice = loadedVertices[Convert.ToInt32(requesting[0]) - 1];
	                            selectedTextureCoordinate = loadedTextureCoords[Convert.ToInt32(requesting[1]) - 1];
	                        } else if(requesting.Length == 3) {
	                            //Form of v1/vt1/vn1
	                            selectedVertice = loadedVertices[Convert.ToInt32(requesting[0]) - 1];
	                            selectedTextureCoordinate = loadedTextureCoords[Convert.ToInt32(requesting[1]) - 1];
	                        }
	                    } else {
	                        //PURE VERTICE GETTING
	                        selectedVertice = loadedVertices[Convert.ToInt32(indexLine)-1];
	                    }
	                    
	                    //Now to add these things to the face
	                    if(selectedVertice != null) selectedVertices.Add(selectedVertice);
	                    if(selectedTextureCoordinate != null) selectedTextureCoordinates.Add(selectedTextureCoordinate);
	                }
	                
	                //Create a Face
	                if(selectedVertices.Count == 3) {
	                    //This is a triangle (yay)
	                    Face f = new Face();
	                    for(int index = 0; index < selectedVertices.Count; index++) {
	                        try {f.addVertice(selectedVertices[index]);}catch(Exception t) {}
	                        try {f.addTextureCoordinate(selectedTextureCoordinates[index]);}catch(Exception t) {}
	                    }
	                    m.addFace(f);
	                } else if(selectedVertices.Count == 4) {
	                    //Convert to Triangle and add
	                    Quad quad = new Quad();
	                    
	                    quad.setVert0(selectedVertices[0]);
	                    quad.setVert1(selectedVertices[1]);
	                    quad.setVert2(selectedVertices[2]);
	                    quad.setVert3(selectedVertices[3]);
	                    
	                    try {quad.setTextureCoordinate0(selectedTextureCoordinates[0]);}catch(Exception t) {}
	                    try {quad.setTextureCoordinate1(selectedTextureCoordinates[1]);}catch(Exception t) {}
	                    try {quad.setTextureCoordinate2(selectedTextureCoordinates[2]);}catch(Exception t) {}
	                    try {quad.setTextureCoordinate3(selectedTextureCoordinates[3]);}catch(Exception t) {}
						
	                    m.addFaces(quad.toQuadArray());
	                } else {
	                    //Something that isn't a quad or a triangle?
	                }
	            }
	        }
	        
	        //Logging
	        Game.GAME_INSTANCE.getLogger().log("Model loaded.");
	        return models;
	    }
    
	    private String getLine(String line, String code) {
	        line = StringUtils.ReplaceFirst(code, "", line);
	        while(line.StartsWith(" ")) line = StringUtils.ReplaceFirst(" ", "", line);
	        return line;
	    }
	    
	    private String[] removeBlankElements(String[] parts) {
	        List<String> cleanParts = new List<String>();
	        foreach(String p in parts) {
	            if(p == null || p.Replace(" ", "").Equals("")) continue;
	            if(p.Replace("\n", "").Equals("")) continue;
	            if(p.Replace("\r", "").Equals("")) continue;
	            cleanParts.Add(p);
	        }
	        
	        return cleanParts.ToArray();
	    }
	}
}

