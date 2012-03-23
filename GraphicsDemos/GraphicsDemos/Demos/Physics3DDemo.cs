using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GraphicsToolkit.Graphics;
using GraphicsToolkit.Graphics.SceneGraph;
using GraphicsToolkit.Physics._3D;
using GraphicsToolkit.Physics._3D.Partitions;
using GraphicsToolkit.Physics._3D.Bodies;

namespace GraphicsDemos.Demos
{
    public class Physics3DDemo : Demo
    {
        private PrimitiveBatch primBatch;
        private FirstPersonCamera cam;
        private PhysicsEngine3D engine;
        private Mesh sphere;

        public override void LoadContent(ContentManager content, GraphicsDevice g)
        {
            base.LoadContent(content, g);

            primBatch = new PrimitiveBatch(Device);
            cam = new FirstPersonCamera(0.5f, 10);
            cam.Pos = new Vector3(3, 3, 13);

            MeshBuilder mb = new MeshBuilder(g);
            sphere = mb.CreateSphere(1f, 10, 10);

            setupEngine();
        }

        private void setupEngine()
        {
            engine = new PhysicsEngine3D(new BruteForcePartition3D());

            float boxSize = 10;

            engine.AddRigidBody(new PlaneBody(Vector3.Up, Vector3.Zero));
            engine.AddRigidBody(new PlaneBody(Vector3.Right, Vector3.Zero));
            engine.AddRigidBody(new PlaneBody(new Vector3(0, 0, 1), Vector3.Zero));

            engine.AddRigidBody(new PlaneBody(-Vector3.Up, new Vector3(0, boxSize, 0)));
            engine.AddRigidBody(new PlaneBody(-Vector3.Right, new Vector3(boxSize, 0, 0)));
            engine.AddRigidBody(new PlaneBody(new Vector3(0, 0, -1), new Vector3(0, 0, boxSize)));

            int bodyCount = 4;
            for (int i = 0; i < bodyCount; i++)
            {
                engine.AddRigidBody(new SphereBody(new Vector3((i*2) + 1, 7, 2), new Vector3(i, 0, 0), 1f, 1f));
            }
        }

        public override void Update(GameTime g)
        {
            cam.Update(g);

            float dt = (float)g.ElapsedGameTime.TotalSeconds;
            engine.Update(g);
        }

        public override void Draw(GameTime g)
        {
            Device.Clear(Color.Black);

            primBatch.Begin(PrimitiveType.LineList, cam);

            primBatch.DrawXZGrid(10, 10, Color.Blue);
            primBatch.DrawXYGrid(10, 10, Color.Red);
            primBatch.DrawYZGrid(10, 10, Color.Green);

            primBatch.End();

            for (int i = 0; i < engine.GetBodies().Count; i++)
            {
                primBatch.DrawMesh(sphere, Matrix.CreateTranslation(engine.GetBodies()[i].Pos), cam);
            }
        }
    }
}
