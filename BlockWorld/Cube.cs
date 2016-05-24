using System;
using Microsoft.Xna.Framework;

namespace BlockWorld
{
	public struct Cube
	{
		public Vector3 Position;
		public float Size;

		public void PutVertices(out VertexPositionNormal[] vertices, out short[] indices) {

			var position = Position - new Vector3(Size / 2.0f);

			// VERTICES
			vertices = new VertexPositionNormal[24];

			// FRONT
			vertices[0]  = new VertexPositionNormal(position + new Vector3(0   , Size, Size), Normals[(int)CubeFace.PositiveZ]);
			vertices[1]  = new VertexPositionNormal(position + new Vector3(0,    0,    Size), Normals[(int)CubeFace.PositiveZ]);
			vertices[2]  = new VertexPositionNormal(position + new Vector3(Size, 0,    Size), Normals[(int)CubeFace.PositiveZ]);
			vertices[3]  = new VertexPositionNormal(position + new Vector3(Size, Size, Size), Normals[(int)CubeFace.PositiveZ]);

			// BACK
			vertices[4]  = new VertexPositionNormal(position + new Vector3(Size, Size, 0   ), Normals[(int)CubeFace.NegativeZ]);
			vertices[5]  = new VertexPositionNormal(position + new Vector3(Size, 0,    0   ), Normals[(int)CubeFace.NegativeZ]);
			vertices[6]  = new VertexPositionNormal(position + new Vector3(0,    0,    0   ), Normals[(int)CubeFace.NegativeZ]);
			vertices[7]  = new VertexPositionNormal(position + new Vector3(0,    Size, 0   ), Normals[(int)CubeFace.NegativeZ]);

			// RIGHT
			vertices[8]  = new VertexPositionNormal(position + new Vector3(Size, Size, Size), Normals[(int)CubeFace.PositiveX]);
			vertices[9]  = new VertexPositionNormal(position + new Vector3(Size, 0   , Size), Normals[(int)CubeFace.PositiveX]);
			vertices[10] = new VertexPositionNormal(position + new Vector3(Size, 0   , 0   ), Normals[(int)CubeFace.PositiveX]);
			vertices[11] = new VertexPositionNormal(position + new Vector3(Size, Size, 0   ), Normals[(int)CubeFace.PositiveX]);

			// LEFT
			vertices[12] = new VertexPositionNormal(position + new Vector3(0,    Size, 0   ), Normals[(int)CubeFace.NegativeX]);
			vertices[13] = new VertexPositionNormal(position + new Vector3(0,    0,    0   ), Normals[(int)CubeFace.NegativeX]);
			vertices[14] = new VertexPositionNormal(position + new Vector3(0,    0,    Size), Normals[(int)CubeFace.NegativeX]);
			vertices[15] = new VertexPositionNormal(position + new Vector3(0,    Size, Size), Normals[(int)CubeFace.NegativeX]);

			// TOP
			vertices[16] = new VertexPositionNormal(position + new Vector3(0   , Size, 0   ), Normals[(int)CubeFace.PositiveY]);
			vertices[17] = new VertexPositionNormal(position + new Vector3(0   , Size, Size), Normals[(int)CubeFace.PositiveY]);
			vertices[18] = new VertexPositionNormal(position + new Vector3(Size, Size, Size), Normals[(int)CubeFace.PositiveY]);
			vertices[19] = new VertexPositionNormal(position + new Vector3(Size, Size, 0   ), Normals[(int)CubeFace.PositiveY]);

			// BOTTOM
			vertices[20] = new VertexPositionNormal(position + new Vector3(0   , 0   , Size), Normals[(int)CubeFace.NegativeY]);
			vertices[21] = new VertexPositionNormal(position + new Vector3(0   , 0   , 0   ), Normals[(int)CubeFace.NegativeY]);
			vertices[22] = new VertexPositionNormal(position + new Vector3(Size, 0   , 0   ), Normals[(int)CubeFace.NegativeY]);
			vertices[23] = new VertexPositionNormal(position + new Vector3(Size, 0   , Size), Normals[(int)CubeFace.NegativeY]);

			// INDICES
			indices = new short[36];

			// FRONT
			indices[0]  = (short) 0;
			indices[1]  = (short) 2;
			indices[2]  = (short) 1;
			indices[3]  = (short) 0;
			indices[4]  = (short) 3;
			indices[5]  = (short) 2;

			// TOP
			indices[6]  = (short) 4;
			indices[7]  = (short) 6;
			indices[8]  = (short) 5;
			indices[9]  = (short) 4;
			indices[10] = (short) 7;
			indices[11] = (short) 6;

			// LEFT
			indices[12] = (short) 8;
			indices[13] = (short) 10;
			indices[14] = (short) 9;
			indices[15] = (short) 8;
			indices[16] = (short) 11;
			indices[17] = (short) 10;

			// RIGHT
			indices[18] = (short) 12;
			indices[19] = (short) 14;
			indices[20] = (short) 13;
			indices[21] = (short) 12;
			indices[22] = (short) 15;
			indices[23] = (short) 14;

			// BACK
			indices[24] = (short) 16;
			indices[25] = (short) 18;
			indices[26] = (short) 17;
			indices[27] = (short) 16;
			indices[28] = (short) 19;
			indices[29] = (short) 18;

			// BOTTOM
			indices[30] = (short) 20;
			indices[31] = (short) 22;
			indices[32] = (short) 21;
			indices[33] = (short) 20;
			indices[34] = (short) 23;
			indices[35] = (short) 22;
		}

		public enum CubeFace
        {
            PositiveX = 0,
            NegativeX = 1,
            PositiveY = 2,
            NegativeY = 3,
            PositiveZ = 4,
            NegativeZ = 5
        }

		public static readonly Vector3[] Normals =
        {
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, -1, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1)
        };
	}
}
