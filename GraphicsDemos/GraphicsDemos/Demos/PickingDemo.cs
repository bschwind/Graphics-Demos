using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using GraphicsToolkit.Graphics;
using GraphicsToolkit.Graphics.SceneGraph;
using GraphicsToolkit;
using GraphicsToolkit.Input;

namespace GraphicsDemos.Demos
{
    public struct Triangle
    {
        public Vector3 v1, v2, v3;
    }

    public class PickingDemo : Demo
    {
        private PrimitiveBatch primBatch;
        private FirstPersonCamera cam;
        MeshNode body, handle, trigger;
        Ray camRay;
        List<MeshNode> pickableObjects;
        List<Triangle> bodyTris, handleTris, triggerTris;
        SpriteBatch batch;
        Texture2D crosshair;
        MeshNode selectedObject;
        bool dragging = false;
        float triggerRot;
        SoundEffect fire;

        public override void LoadContent(ContentManager content, GraphicsDevice g)
        {
            base.LoadContent(content, g);

            primBatch = new PrimitiveBatch(Device);
            cam = new FirstPersonCamera(0.5f, 10);
            //cam.Pos = new Vector3(3, 3, 13);

            Model bodyModel = content.Load<Model>("Models/Body");
            Model handleModel = content.Load<Model>("Models/Handle");
            Model triggerModel = content.Load<Model>("Models/Trigger");
            crosshair = content.Load<Texture2D>("Crosshair");
            fire = content.Load<SoundEffect>("fire_silenced");
            batch = new SpriteBatch(Device);

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
            body.SetPos(new Vector3(0, 5, 0));

            pickableObjects = new List<MeshNode>();
            pickableObjects.Add(body);
            pickableObjects.Add(handle);
            pickableObjects.Add(trigger);

            //Extract triangles
            bodyTris = GetTriangles(bodyMesh);
            handleTris = GetTriangles(handleMesh);
            triggerTris = GetTriangles(triggerMesh);
        }

        public List<Triangle> GetTriangles(Mesh m)
        {
            List<Triangle> tris = new List<Triangle>();

            VertexPositionNormalTexture[] verts = new VertexPositionNormalTexture[m.Vertices.VertexCount];
            m.Vertices.GetData<VertexPositionNormalTexture>(verts);

            short[] indices = new short[m.Indices.IndexCount];
            m.Indices.GetData<short>(indices);

            for (int i = 0; i < indices.Length; i += 3)
            {
                Vector3 v1 = verts[indices[i]].Position;
                Vector3 v2 = verts[indices[i+1]].Position;
                Vector3 v3 = verts[indices[i+2]].Position;
                Triangle t = new Triangle();
                t.v1 = v1;
                t.v2 = v2;
                t.v3 = v3;
                tris.Add(t);
            }

            return tris;
        }

        //Returns -1 if there is no intersection, otherwise returns the distance to the triangle
        public float IntersectRayTriangle(Vector3 p, Vector3 q, Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 ab = b - a;
            Vector3 ac = c - a;
            Vector3 qp = p - q;

            Vector3 n = Vector3.Cross(ab, ac);

            float d = Vector3.Dot(qp, n);
            if (d <= 0.0f)
            {
                return -1;
            }

            Vector3 ap = p - a;
            float t = Vector3.Dot(ap, n);
            if (t < 0.0f)
            {
                return -1;
            }

            Vector3 e = Vector3.Cross(qp, ap);
            float v = Vector3.Dot(ac, e);
            if (v < 0.0f || v > d)
            {
                return -1;
            }
            float w = -Vector3.Dot(ab, e);
            if (w < 0.0f || v + w > d)
            {
                return -1;
            }

            float ood = 1.0f / d;
            t *= ood;
            return ((p + (q - p)) * t).Length();
            //return t;
        }

        private Ray GetRayFromScreen(Vector2 screenPos)
        {
            Vector3 farVector = Device.Viewport.Unproject(new Vector3(screenPos.X, screenPos.Y, 1), cam.Projection, cam.View, Matrix.Identity);
            Vector3 nearVector = Device.Viewport.Unproject(new Vector3(screenPos.X, screenPos.Y, 0), cam.Projection, cam.View, Matrix.Identity);
            return new Ray(nearVector, Vector3.Normalize(farVector - nearVector));
        }

        public override void Update(GameTime g)
        {
            cam.Update(g);

            float dt = (float)g.ElapsedGameTime.TotalSeconds;
            Vector2 mousePos = new Vector2(InputHandler.MouseState.X, InputHandler.MouseState.Y);
            camRay = GetRayFromScreen(new Vector2(Config.ScreenWidth / 2f, Config.ScreenHeight / 2f));

            updatePicking();

            if (InputHandler.LastMouseState.LeftButton == ButtonState.Pressed && InputHandler.MouseState.LeftButton == ButtonState.Pressed)
            {
                dragging = true;
            }

            if (InputHandler.LastMouseState.LeftButton == ButtonState.Pressed && InputHandler.MouseState.LeftButton == ButtonState.Released)
            {
                selectedObject = null;
                dragging = false;
            }

            if (dragging && selectedObject != null)
            {
                Vector3 gunDirection = Vector3.TransformNormal(new Vector3(0, 0, 1), body.GetAbsoluteTransform());

                if (trigger.Equals(selectedObject))
                {
                    Ray diffRay = GetRayFromScreen(mousePos + cam.MouseDelta);
                    Vector3 diffPoint = diffRay.Position + (diffRay.Direction * 10f);
                    Vector3 camPoint = camRay.Position + (camRay.Direction * 10f);
                    Vector3 diff = diffPoint - camPoint;
                    float amt = Vector3.Dot(diff, -gunDirection);
                    float triggerMax = 0.6f;
                    triggerRot += amt;
                    if (triggerRot >= triggerMax)
                    {
                        Console.WriteLine("BANG!");
                        fire.Play();
                        triggerRot = 0;
                        //dragging = false;
                        //5selectedObject = null;
                    }
                    triggerRot = MathHelper.Clamp(triggerRot, 0, triggerMax);

                    trigger.SetRotation(Quaternion.CreateFromAxisAngle(Vector3.Right, triggerRot));
                }
                else if (handle.Equals(selectedObject))
                {
                    Ray diffRay = GetRayFromScreen(mousePos + cam.MouseDelta);
                    Vector3 diffPoint = diffRay.Position + (diffRay.Direction * 10f);
                    Vector3 camPoint = camRay.Position + (camRay.Direction * 10f);
                    Vector3 diff = diffPoint - camPoint;
                    handle.SetPos(handle.GetPos() + new Vector3(0, 0, 1) * Vector3.Dot(new Vector3(0, 0, 1), diff));
                    float clampedZ = MathHelper.Clamp(handle.GetPos().Z, 0.0f, 0.477f);
                    Vector3 newPos = handle.GetPos();
                    newPos.Z = clampedZ;
                    handle.SetPos(newPos);
                }
                else if (body.Equals(selectedObject))
                {
                    Ray diffRay = GetRayFromScreen(mousePos + cam.MouseDelta);
                    Vector3 cross = Vector3.Cross(camRay.Direction*10, diffRay.Direction*10);
                    if (cross.Length() > 0)
                    {
                        Quaternion rotDiff = Quaternion.CreateFromAxisAngle(Vector3.Normalize(cross), -0.06f);
                        body.SetRotation(body.GetRotation() * rotDiff);
                    }
                }
            }
        }

        private void updatePicking()
        {
            float closest = float.MaxValue;
            if (dragging || InputHandler.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
            {
                return;
            }

            foreach (MeshNode mesh in pickableObjects)
            {
                if (camRay.Intersects(mesh.GetBounds()) != null)
                {
                    if (mesh.Equals(trigger))
                    {
                        //Check trigger triangles
                        foreach (Triangle tri in triggerTris)
                        {
                            Matrix meshInverseTransform = Matrix.Invert(mesh.GetAbsoluteTransform());
                            Vector3 transformedRayStart = Vector3.Transform(camRay.Position, meshInverseTransform);
                            Vector3 transformedRayDir = Vector3.TransformNormal(camRay.Direction, meshInverseTransform);
                            float dist = IntersectRayTriangle(transformedRayStart, transformedRayStart + (transformedRayDir * 1000f), tri.v1, tri.v2, tri.v3);
                            if (dist > 0 && dist < closest)
                            {
                                closest = dist;
                                selectedObject = trigger;
                            }
                        }
                    }
                    else if (mesh.Equals(handle))
                    {
                        //Check handle triangles
                        foreach (Triangle tri in handleTris)
                        {
                            Matrix meshInverseTransform = Matrix.Invert(mesh.GetAbsoluteTransform());
                            Vector3 transformedRayStart = Vector3.Transform(camRay.Position, meshInverseTransform);
                            Vector3 transformedRayDir = Vector3.TransformNormal(camRay.Direction, meshInverseTransform);
                            float dist = IntersectRayTriangle(transformedRayStart, transformedRayStart + (transformedRayDir * 100f), tri.v1, tri.v2, tri.v3);
                            if (dist > 0 && dist < closest)
                            {
                                closest = dist;
                                selectedObject = handle;
                            }
                        }
                    }
                    else if (mesh.Equals(body))
                    {
                        //Check body triangles
                        foreach (Triangle tri in bodyTris)
                        {
                            Matrix meshInverseTransform = Matrix.Invert(mesh.GetAbsoluteTransform());
                            Vector3 transformedRayStart = Vector3.Transform(camRay.Position, meshInverseTransform);
                            Vector3 transformedRayDir = Vector3.TransformNormal(camRay.Direction, meshInverseTransform);
                            float dist = IntersectRayTriangle(transformedRayStart, transformedRayStart + (transformedRayDir * 1000f), tri.v1, tri.v2, tri.v3);
                            if (dist > 0 && dist < closest)
                            {
                                closest = dist;
                                selectedObject = body;
                            }
                        }
                    }
                }
            }
        }

        public override void Draw(GameTime g)
        {
            Device.Clear(Color.Black);

            Device.BlendState = BlendState.Opaque;
            Device.DepthStencilState = DepthStencilState.Default;

            primBatch.Begin(PrimitiveType.LineList, cam);

            primBatch.DrawXZGrid(10, 10, Color.Blue);
            primBatch.DrawXYGrid(10, 10, Color.Red);
            primBatch.DrawYZGrid(10, 10, Color.Green);

            primBatch.End();
            body.Draw(g, Matrix.Identity, primBatch, cam);

            batch.Begin();
            batch.Draw(crosshair, new Vector2((Config.ScreenWidth / 2f) - (crosshair.Width / 2f), (Config.ScreenHeight / 2f) - (crosshair.Height / 2f)), Color.White);
            batch.End();
        }
    }
}
