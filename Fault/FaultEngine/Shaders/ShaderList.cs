using System;
using Sce.PlayStation.Core.Graphics;

namespace Fault {
	public class ShaderList {
		private static ShaderProgram DEFAULT_SHADER_PROGRAM = null;
		public static ShaderProgram getDefaultShaderProgram() {
			if(DEFAULT_SHADER_PROGRAM != null) return DEFAULT_SHADER_PROGRAM;
			DEFAULT_SHADER_PROGRAM = new ShaderProgram(Game.APPLICATION_DIRECTORY + "FaultEngine/Shaders/DefaultShader/Shader.cgx");
			
			//Matrix's
			DEFAULT_SHADER_PROGRAM.SetUniformBinding(0, "u_ModelViewMatrix");
			DEFAULT_SHADER_PROGRAM.SetUniformBinding(1, "u_ProjectionMatrix");
			DEFAULT_SHADER_PROGRAM.SetUniformBinding(2, "u_PositionMatrix");
			
			//Material
			DEFAULT_SHADER_PROGRAM.SetUniformBinding(3, "MatColor");
			
			//Externals
			DEFAULT_SHADER_PROGRAM.SetUniformBinding(4, "lights");
			DEFAULT_SHADER_PROGRAM.SetUniformBinding(5, "lightPositions");
			DEFAULT_SHADER_PROGRAM.SetUniformBinding(6, "lightColors");
			DEFAULT_SHADER_PROGRAM.SetUniformBinding(7, "lightIntensities");
			
			DEFAULT_SHADER_PROGRAM.SetUniformBinding(8, "ambientLightColor");
			DEFAULT_SHADER_PROGRAM.SetUniformBinding(9, "ambientLightIntensity");
			
			DEFAULT_SHADER_PROGRAM.SetUniformBinding(10, "skyColor");
			
			DEFAULT_SHADER_PROGRAM.SetUniformBinding(11, "textured");
			
			return DEFAULT_SHADER_PROGRAM;
		}
		
		private static ShaderProgram SPRITE_SHADER_PROGRAM = null;
		public static ShaderProgram getSpriteShader() {
			if(SPRITE_SHADER_PROGRAM != null) return SPRITE_SHADER_PROGRAM;
			SPRITE_SHADER_PROGRAM = new ShaderProgram(Game.APPLICATION_DIRECTORY + "FaultEngine/Shaders/Sprite/Sprite.cgx");
			
			//Matrix's
			SPRITE_SHADER_PROGRAM.SetUniformBinding(0, "u_ProjectionMatrix");
			SPRITE_SHADER_PROGRAM.SetUniformBinding(1, "u_PositionMatrix");
			SPRITE_SHADER_PROGRAM.SetUniformBinding(2, "MatColor");
			SPRITE_SHADER_PROGRAM.SetUniformBinding(3, "textured");
			
			return SPRITE_SHADER_PROGRAM;
		}
	}
}

