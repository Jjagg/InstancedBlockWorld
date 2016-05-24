using System;

namespace BlockWorld
{
	public class CubeFaceRenderer
	{
		public CubeFaceRenderer ()
		{
		}

		public void RenderCubes(Cube[] cubes, out VertexPositionNormal[] vertices, out short[] indices) {
			var count = cubes.Length;
			vertices = new VertexPositionNormal[count * 4];
			indices = new short[count * 6];
		}
	}
}

