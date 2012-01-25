using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GraphicsToolkit.Graphics;

namespace GraphicsDemos.Demos
{
    public class PrimitivesDemo : Demo
    {
        BasicEffect effect;
        FirstPersonCamera cam;
        Mesh currentMesh;

        public override void LoadContent(ContentManager c, GraphicsDevice g)
        {
            base.LoadContent(c, g);

            currentMesh = createCube(c);
            effect = new BasicEffect(g);
            effect.PreferPerPixelLighting = true;
            effect.TextureEnabled = true;
            effect.Texture = currentMesh.Texture;
            effect.EnableDefaultLighting();

            cam = new FirstPersonCamera(0.5f, 5f);
            cam.Pos = new Vector3(0, 2, 4);
        }

        private Mesh createCube(ContentManager c)
        {
            Mesh m = new Mesh();
            VertexPositionNormalTexture[] verts = new VertexPositionNormalTexture[24];
            short[] indices = new short[36];

            //Set up vertices for the front face
            VertexPositionNormalTexture[] originalVerts = new VertexPositionNormalTexture[4];
            VertexPositionNormalTexture first = new VertexPositionNormalTexture();
            first.Position = new Vector3(-0.5f, 0.5f, 0.5f);
            first.Normal = new Vector3(0, 0, 1);
            first.TextureCoordinate = new Vector2(0, 0);

            VertexPositionNormalTexture second = new VertexPositionNormalTexture();
            second.Position = new Vector3(0.5f, 0.5f, 0.5f);
            second.Normal = new Vector3(0, 0, 1);
            second.TextureCoordinate = new Vector2(1, 0);
            
            VertexPositionNormalTexture third = new VertexPositionNormalTexture();
            third.Position = new Vector3(0.5f, -0.5f, 0.5f);
            third.Normal = new Vector3(0, 0, 1);
            third.TextureCoordinate = new Vector2(1, 1);

            VertexPositionNormalTexture fourth = new VertexPositionNormalTexture();
            fourth.Position = new Vector3(-0.5f, -0.5f, 0.5f);
            fourth.Normal = new Vector3(0, 0, 1);
            fourth.TextureCoordinate = new Vector2(0, 1);

            originalVerts[0] = first;
            originalVerts[1] = second;
            originalVerts[2] = third;
            originalVerts[3] = fourth;

            Matrix[] rotations = {Matrix.CreateRotationY(0), 
                                  Matrix.CreateRotationY(MathHelper.PiOver2), 
                                  Matrix.CreateRotationY(MathHelper.Pi),
                                  Matrix.CreateRotationY(-MathHelper.PiOver2),
                                  Matrix.CreateRotationX(MathHelper.PiOver2),
                                  Matrix.CreateRotationX(-MathHelper.PiOver2)};

            short indexOffset = 0;
            short vertexOffset = 0;

            for (int i = 0; i < rotations.Length; i++)
            {
                VertexPositionNormalTexture v1 = transformVertex(originalVerts[0], rotations[i]);
                VertexPositionNormalTexture v2 = transformVertex(originalVerts[1], rotations[i]);
                VertexPositionNormalTexture v3 = transformVertex(originalVerts[2], rotations[i]);
                VertexPositionNormalTexture v4 = transformVertex(originalVerts[3], rotations[i]);

                verts[vertexOffset] = v1;
                verts[vertexOffset+1] = v2;
                verts[vertexOffset+2] = v3;
                verts[vertexOffset+3] = v4;

                indices[indexOffset] = vertexOffset;
                indices[indexOffset+1] = (short)(vertexOffset+1);
                indices[indexOffset+2] = (short)(vertexOffset + 2);

                indices[indexOffset+3] = vertexOffset;
                indices[indexOffset+4] = (short)(vertexOffset + 2);
                indices[indexOffset+5] = (short)(vertexOffset + 3);

                vertexOffset += 4;
                indexOffset += 6;
            }

            IndexBuffer iBuffer = new IndexBuffer(Device, IndexElementSize.SixteenBits, indices.Length, BufferUsage.None);
            iBuffer.SetData<short>(indices);
            VertexBuffer vBuffer = new VertexBuffer(Device, typeof(VertexPositionNormalTexture), verts.Length, BufferUsage.None);
            vBuffer.SetData<VertexPositionNormalTexture>(verts);

            m.Indices = iBuffer;
            m.Vertices = vBuffer;

            m.Texture = c.Load<Texture2D>("Cube");

            return m;
        }

        private VertexPositionNormalTexture transformVertex(VertexPositionNormalTexture original, Matrix transform)
        {
            Vector3 newPos = Vector3.Transform(original.Position, transform);
            Vector3 newNormal = Vector3.TransformNormal(original.Normal, transform);

            return new VertexPositionNormalTexture(newPos, newNormal, original.TextureCoordinate);
        }

        public override void Update(GameTime g)
        {
            cam.Update(g);
        }

        public override void Draw(GameTime g)
        {
            Device.Clear(Color.Black);

            effect.View = cam.View;
            effect.Projection = cam.Projection;
            effect.CurrentTechnique.Passes[0].Apply();

            Device.SetVertexBuffer(currentMesh.Vertices);
            Device.Indices = currentMesh.Indices;

            Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, currentMesh.Vertices.VertexCount, 0, currentMesh.Indices.IndexCount / 3);
        }
    }
}
