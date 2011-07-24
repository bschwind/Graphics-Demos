using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsDemos.Graphics
{
    public struct VertexPositionColorNormal : IVertexType
    {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;

        public VertexPositionColorNormal(Vector3 pos, Color color, Vector3 normal)
        {
            Position = pos;
            Color = color;
            Normal = normal;
        }

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(sizeof(float) * 3 + sizeof(byte) * 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0));

        VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }
    };
}
