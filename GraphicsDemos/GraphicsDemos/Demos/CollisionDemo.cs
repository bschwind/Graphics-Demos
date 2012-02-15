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
    public class CollisionDemo : Demo
    {
        private PrimitiveBatch primBatch;
        private FirstPersonCamera cam;
        private List<Ball> balls;
        private List<Plane> planes;
        private Mesh sphere;

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager c, Microsoft.Xna.Framework.Graphics.GraphicsDevice g)
        {
            base.LoadContent(c, g);

            primBatch = new PrimitiveBatch(Device);
            cam = new FirstPersonCamera(0.5f, 10);
            cam.Pos = new Vector3(3, 3, 13);

            balls = new List<Ball>();

            for (int i = 0; i < 50; i++)
            {
                balls.Add(new Ball(new Vector3(i * 2, 2, 2), new Vector3(i + 1, -i, i % 2) * 0.1f, 0.5f));
            }

            planes = new List<Plane>();

            planes.Add(new Plane(Vector3.Right, 0));
            planes.Add(new Plane(Vector3.Up, 0));
            planes.Add(new Plane(new Vector3(0, 0, 1), 0));
            planes.Add(new Plane(Vector3.Left, -10));
            planes.Add(new Plane(Vector3.Down, -10));
            planes.Add(new Plane(new Vector3(0, 0, -1), -10));

            MeshBuilder mb = new MeshBuilder(g);
            sphere = mb.CreateSphere(1f, 10, 10);
        }

        public override void Update(GameTime g)
        {
            cam.Update(g);

            float dt = (float)g.ElapsedGameTime.TotalSeconds;

            foreach (Ball b in balls)
            {
                b.Pos += b.Vel * dt;
            }

            for (int i = 0; i < balls.Count; i++)
            {
                //Fix collisions with other balls
                for (int j = i + 1; j < balls.Count; j++)
                {
                    //Check for intersection
                    Ball a = balls[i];
                    Ball b = balls[j];
                    if ((b.Pos - a.Pos).LengthSquared() <= (a.Radius + b.Radius) * (a.Radius + b.Radius))
                    {
                        //If intersecting, find normal
                        Vector3 normal = a.Pos - b.Pos;
                        normal.Normalize();

                        //Separate spheres so they're not intersecting
                        float separation = (a.Radius + b.Radius) - (a.Pos - b.Pos).Length();
                        separation /= 2f;
                        a.Pos += normal * separation;
                        b.Pos -= normal * separation;
                        //Reflect velocities off of the normal to find the new velocities
                        a.Vel = Vector3.Reflect(a.Vel, normal);
                        b.Vel = Vector3.Reflect(b.Vel, -normal);
                    }
                }

                //Fix collisions with the planes
                foreach (Plane p in planes)
                {
                    //Check for intersection
                    Ball a = balls[i];
                    float dist = Vector3.Dot(a.Pos, p.Normal) - p.D;
                    if (dist <= a.Radius)
                    {
                        //If intersecting, find normal
                        Vector3 normal = p.Normal;

                        //Separate sphere from plane so they are not intersecting
                        float separation = a.Radius - dist;
                        a.Pos += normal * separation;

                        //Reflect velocity off of the normal to find the new velocity
                        a.Vel = Vector3.Reflect(a.Vel, normal);
                    }
                }
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
                primBatch.DrawPlane(p, Color.White);
            }

            primBatch.End();

            foreach (Ball b in balls)
            {
                primBatch.DrawMesh(sphere, Matrix.CreateScale(b.Radius) * Matrix.CreateTranslation(b.Pos), cam);
            }
        }
    }
}
