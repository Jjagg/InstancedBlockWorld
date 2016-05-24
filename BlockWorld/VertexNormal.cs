using System;
using System.Runtime.InteropServices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BlockWorld
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct VertexNormal : IVertexType
	{
		public Vector3 Normal;

		public static readonly VertexDeclaration VertexDeclaration;
		public VertexNormal(Vector3 normal)
		{
			this.Normal = normal;
		}

		#region IVertexType implementation

		VertexDeclaration IVertexType.VertexDeclaration
		{
			get
			{
				return VertexDeclaration;
			}
		}

		#endregion

		public override string ToString()
		{
			return "{{Normal:" + this.Normal + "}}";
		}

		public static bool operator ==(VertexNormal left, VertexNormal right)
		{
			return left.Normal == right.Normal;
		}

		public static bool operator !=(VertexNormal left, VertexNormal right)
		{
			return !(left == right);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != base.GetType())
			{
				return false;
			}
			return (this == ((VertexNormal)obj));
		}

		static VertexNormal()
		{
			VertexElement[] elements = new VertexElement[] { new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0) };
			VertexDeclaration = new VertexDeclaration(elements);
		}
	}
}

