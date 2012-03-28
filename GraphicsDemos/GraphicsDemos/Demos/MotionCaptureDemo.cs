using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GraphicsToolkit.Graphics;
using GraphicsToolkit.Animation;
using GraphicsToolkit.Graphics.SceneGraph;

namespace GraphicsDemos.Demos
{
    public class MotionCaptureDemo : Demo
    {
        private FirstPersonCamera cam;
        private PrimitiveBatch primBatch;
        private Animation animation;
        private List<MeshNode> spheres;
        private int currentFrame = 0;

        public override void LoadContent(ContentManager content, GraphicsDevice g)
        {
            base.LoadContent(content, g);

            //Set up our camera and primitive renderer
            cam = new FirstPersonCamera(0.5f, 5f);
            cam.Pos = new Vector3(5, 1, 10);
            primBatch = new PrimitiveBatch(g);

            animation = Animation.Parse("Content/MotionCaptures/BriskWalk1_All.tsv");

            spheres = new List<MeshNode>();
            MeshBuilder mb = new MeshBuilder(Device);

            for (int i = 0; i < animation.GetNumMarkers(); i++)
            {
                spheres.Add(new MeshNode(mb.CreateSphere(0.06f, 10, 10)));
            }
        }

        public override void Update(GameTime g)
        {
            cam.Update(g);

            currentFrame+=5;
            if (currentFrame >= animation.GetFrames().Length)
            {
                currentFrame = 0;
            }

            AnimationFrame f = animation.GetFrames()[currentFrame];

            for (int i = 0; i < f.Positions.Length; i++)
            {
                spheres[i].SetPos(f.Positions[i] / 1000f);
                //spheres[i].SetPos(Vector3.Zero);
            }
        }

        public override void Draw(GameTime g)
        {
            Device.Clear(Color.Black);
            primBatch.Begin(PrimitiveType.LineList, cam);
            primBatch.DrawXZGrid(10, 10, Color.LightBlue);

            //Draw our coordinate axes for fun
            primBatch.DrawLine(new Vector3(0, 1, 0), new Vector3(1, 1, 0), Color.Red);
            primBatch.DrawLine(new Vector3(0, 1, 0), new Vector3(0, 2, 0), Color.Green);
            primBatch.DrawLine(new Vector3(0, 1, 0), new Vector3(0, 1, 1), Color.Blue);

            primBatch.End();


            for (int i = 0; i < spheres.Count; i++)
            {
                spheres[i].Draw(g, Matrix.Identity, primBatch, cam);
            }
        }
    }
}
