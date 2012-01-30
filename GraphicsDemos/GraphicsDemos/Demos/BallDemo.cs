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
    public class BallDemo : Demo
    {
        private PrimitiveBatch primBatch;
        private FirstPersonCamera cam;
        private MeshNode sphere;
        private Vector3 sVel = new Vector3(6, -10f, 4.57f) * 4;
        private float radius = 1;
        List<Plane> planes;

        public override void LoadContent(ContentManager content, GraphicsDevice g)
        {
            base.LoadContent(content, g);

            primBatch = new PrimitiveBatch(Device);
            cam = new FirstPersonCamera(0.5f, 10);
            cam.Pos = new Vector3(3, 3, 13);

            MeshBuilder mb = new MeshBuilder(Device);
            mb.Begin();
            mb.AddSphere(radius, 20, 20);
            Mesh sphereMesh = mb.End();

            sphere = new MeshNode(sphereMesh);
            sphere.SetPos(new Vector3(5, 5, 5));

            planes = new List<Plane>();
            planes.Add(new Plane(Vector3.Right, 0));
            planes.Add(new Plane(Vector3.Up, 0));
            planes.Add(new Plane(new Vector3(0, 0, 1), 0));
            planes.Add(new Plane(Vector3.Left, -10));
            planes.Add(new Plane(Vector3.Down, -10));
            planes.Add(new Plane(new Vector3(0, 0, -1), -10));
        }

        public override void Update(GameTime g)
        {
            cam.Update(g);

            float dt = (float)g.ElapsedGameTime.TotalSeconds;

            updateSphere(dt);
        }

        private void updateSphere(float dt)
        {
            float closestT = float.PositiveInfinity;
            Plane closestPlane = new Plane();
            bool foundPlane = false;

            foreach (Plane p in planes)
            {
                Vector3 q = p.Normal * p.D;

                //If the ball is running parallel to the plane, just keep the sphere moving.
                if (Math.Abs(Vector3.Dot(p.Normal, sVel)) <= float.Epsilon)
                {
                    continue;
                }

                float t = (Vector3.Dot(p.Normal, q) - Vector3.Dot(p.Normal, sphere.GetPos()) + radius * p.Normal.Length()) / Vector3.Dot(p.Normal, sVel);
                if (t < closestT && t > 0)
                {
                    closestT = t;
                    closestPlane = p;
                    foundPlane = true;
                }
            }

            if (foundPlane)
            {
                if (closestT <= dt)
                {
                    float remainingT = dt - closestT;
                    sphere.SetPos(sphere.GetPos() + sVel * closestT);
                    sVel = Vector3.Reflect(sVel, closestPlane.Normal);
                    if (remainingT > 0)
                    {
                        updateSphere(remainingT);
                    }
                }
                else
                {
                    sphere.SetPos(sphere.GetPos() + sVel * dt);
                }
            }
            else
            {
                sphere.SetPos(sphere.GetPos() + sVel * dt);
            }
        }

        public override void Draw(GameTime g)
        {
            Device.Clear(Color.Black);

            primBatch.Begin(PrimitiveType.LineList, cam);

            primBatch.DrawXZGrid(10, 10, Color.Blue);
            primBatch.DrawXYGrid(10, 10, Color.Red);
            primBatch.DrawYZGrid(10, 10, Color.Green);

            foreach (Plane p in planes)
            {
                primBatch.DrawPlane(p, Color.Pink);
            }

            primBatch.End();

            sphere.Draw(g, Matrix.Identity, primBatch, cam);
        }
    }
}
