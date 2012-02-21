using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GraphicsToolkit.Graphics;
using GraphicsToolkit.Graphics.SceneGraph;

namespace GraphicsDemos.Demos
{
    public class PickingDemo : Demo
    {
        private PrimitiveBatch primBatch;
        private FirstPersonCamera cam;
        MeshNode body, handle, trigger;

        public override void LoadContent(ContentManager content, GraphicsDevice g)
        {
            base.LoadContent(content, g);

            primBatch = new PrimitiveBatch(Device);
            cam = new FirstPersonCamera(0.5f, 10);
            cam.Pos = new Vector3(3, 3, 13);

            Model bodyModel = content.Load<Model>("Models/Body");
            Model handleModel = content.Load<Model>("Models/Handle");
            Model triggerModel = content.Load<Model>("Models/Trigger");

            Mesh bodyMesh = new Mesh();
            bodyMesh.Indices = bodyModel.Meshes[0].MeshParts[0].IndexBuffer;
            bodyMesh.Vertices = bodyModel.Meshes[0].MeshParts[0].VertexBuffer;
            body = new MeshNode(bodyMesh);

            Mesh handleMesh = new Mesh();
            handleMesh.Indices = handleModel.Meshes[0].MeshParts[0].IndexBuffer;
            handleMesh.Vertices = handleModel.Meshes[0].MeshParts[0].VertexBuffer;
            handle = new MeshNode(handleMesh);
            handle.SetPos(new Vector3(0, 0.13f, 0.477f));

            Mesh triggerMesh = new Mesh();
            triggerMesh.Indices = triggerModel.Meshes[0].MeshParts[0].IndexBuffer;
            triggerMesh.Vertices = triggerModel.Meshes[0].MeshParts[0].VertexBuffer;
            trigger = new MeshNode(triggerMesh);
            trigger.SetPos(new Vector3(0, 0.033f, -1.561f));

            body.AddChild(handle);
            body.AddChild(trigger);
        }

        public override void Update(GameTime g)
        {
            cam.Update(g);

            float dt = (float)g.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(GameTime g)
        {
            Device.Clear(Color.Black);

            primBatch.Begin(PrimitiveType.LineList, cam);

            primBatch.DrawXZGrid(10, 10, Color.Blue);
            primBatch.DrawXYGrid(10, 10, Color.Red);
            primBatch.DrawYZGrid(10, 10, Color.Green);

            primBatch.End();

            body.Draw(g, Matrix.Identity, primBatch, cam);
            trigger.Draw(g, Matrix.Identity, primBatch, cam);
        }
    }
}
