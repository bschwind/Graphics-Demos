using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GraphicsToolkit.Graphics;
using GraphicsToolkit.Graphics.SceneGraph;
using GraphicsToolkit.Input;

namespace GraphicsDemos.Demos
{
    public class ThirdPersonCameraDemo : Demo
    {
        private PrimitiveBatch primBatch; //PrimitiveBatch contains functions which draw 3D objects to the screen
        private ThirdPersonCamera cam;
        MeshNode target;

        private Vector3 targetPos = new Vector3(5, 5, 5);

        public override void LoadContent(ContentManager content, GraphicsDevice g)
        {
            base.LoadContent(content, g);

            //Set up our camera and primitive renderer
            cam = new ThirdPersonCamera(targetPos, 5.0f, 0.2f);

            primBatch = new PrimitiveBatch(g);            //Initialize our PrimitiveBatch
            MeshBuilder mb = new MeshBuilder(g); 
            Mesh triangle;// = mb.CreateSphere(0.3f, 15, 15);
            mb.Begin();
     //       mb.AddTriangle(Vector3.Zero, new Vector3(1, 1, 0), new Vector3(1, 0, 0), true);
            mb.AddTriangle(Vector3.Zero, new Vector3(1, 1, 0), new Vector3(1, 0, 0), new Vector2(0, 0), new Vector2(1, 1), new Vector2(1, 0), false);

            triangle = mb.End();

            triangle.Texture = content.Load<Texture2D>("Cube");
            target = new MeshNode(triangle);
            target.SetPos(targetPos);
        }

        public override void Update(GameTime g)
        {
                                            //Update our camera so it acts on our mouse movement and updates the view
            float dt = (float)g.ElapsedGameTime.TotalSeconds;

            float forwardMovement = 0f;
            float sideMovement = 0f;

            if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.W))
            {
                forwardMovement = dt;
            }
            
            if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.S))
            {
                forwardMovement = -dt;
            }

            if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.A))
            {
                sideMovement = -dt;
            }

            if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.D))
            {
                sideMovement = dt;
            }

            Vector3 diff = 25 * sideMovement * cam.Right + 25 * forwardMovement * cam.Forward;
            target.SetPos(target.GetPos() + diff);

            cam.TargetPos = target.GetPos();
            cam.Update(g);
        }

        public override void Draw(GameTime g)
        {
            Device.Clear(Color.Black);                         //Clear the screen to black. This is essentially the background color, though a
          
            primBatch.Begin(PrimitiveType.LineList, cam);
            primBatch.DrawXZGrid(10, 10, Color.Blue);  
            primBatch.DrawXYGrid(10, 10, Color.Red);       
            primBatch.DrawYZGrid(10, 10, Color.Green);
            primBatch.End();

            target.Draw(g, Matrix.Identity, primBatch, cam);
        }
    }
}
