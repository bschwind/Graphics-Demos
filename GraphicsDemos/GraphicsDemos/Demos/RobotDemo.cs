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
    public class RobotDemo : Demo
    {
        private FirstPersonCamera cam;    //Define a first person camera so we can move around in our world
        private PrimitiveBatch primBatch; //PrimitiveBatch contains functions which draw 3D objects to the screen
        private MeshNode shoulder;        //A MeshNode inherits from a SceneNode, which describes a transform in 3D space
                                          //A scenegraph is composed of SceneNodes

        public override void LoadContent(ContentManager content, GraphicsDevice g)
        {
            base.LoadContent(content, g);

            //Set up our camera and primitive renderer
            cam = new FirstPersonCamera(0.5f, 5f);        //0.5f is the turn speed for the camera, and 5f is the translational speed
            cam.Pos = new Vector3(3, 3, 13);              //Move our camera to the point (3, 3, 13)
            primBatch = new PrimitiveBatch(g);            //Initialize our PrimitiveBatch


            //Define the objects in our scene
            Mesh sphere, cylinder, box;                   //Define the meshes we will use for the robot arm
                                                          //A mesh holds a list of vertices, and a list of indices which represent triangles
            
            MeshNode arm, elbow, forearm, wrist, hand;    //Define all the parts of the robot arm we will use

            MeshBuilder mb = new MeshBuilder(g);          //MeshBuilder is a helper class for generating 3D geometry
                                                          //It has some built in functions to create primitives, as well as
                                                          //functions to create your own arbitrary meshes

            //Genererate our geometry
            //All geometry created is centered around the origin in its local coordinate system
            sphere = mb.CreateSphere(0.5f, 15, 15);       //Create a sphere mesh, with radius 0.5 and with 15 subdivisions
            cylinder = mb.CreateCylinder(0.3f, 2.0f, 15); //Create a cylinder with radius 0.3, a height of 2, and 15 subdivisions
            box = mb.CreateBox(0.8f, 1.4f, 0.1f);         //Create a box with width 0.8, height of 1.4, and depth of 0.1

            shoulder = new MeshNode(sphere);              //Assign the sphere mesh to our shoulder node

            arm = new MeshNode(cylinder);                 //Assign the cylinder mesh to our arm node
            arm.SetPos(new Vector3(0, -1, 0));            //Translate the arm down 1 on the y axis

            elbow = new MeshNode(sphere);                 //Assign a sphere to our elbow node
            elbow.SetPos(new Vector3(0, -2, 0));          //Translate the elbow down 2 on the y axis

            forearm = new MeshNode(cylinder);             //Assign a cylinder to our forearm node
            forearm.SetPos(new Vector3(0, -1, 0));        //Translate the forearm down 1 on the y axis

            wrist = new MeshNode(sphere);                 //Assign a sphere for the wrist node
            wrist.SetPos(new Vector3(0, -2, 0));          //Translate the wrist down 2 on the y axis

            hand = new MeshNode(box);                     //Assign the box to the hand node
            hand.SetPos(new Vector3(0, -0.7f, 0));        //Translate the hand down 0.7 (half the height of the box) on the y axis

            shoulder.AddChild(arm);                       //The shoulder is the root of this scene, in our case. It is the parent
            shoulder.AddChild(elbow);                     //of both the arm and the elbow

            elbow.AddChild(forearm);                      //The elbow is the parent of the forearm and wrist
            elbow.AddChild(wrist);                        

            wrist.AddChild(hand);                         //The wrist is the parent of the hand


            shoulder.SetPos(new Vector3(0, 5, 0));        //This call effectively translates the entire arm up 5 on the y axis
        }

        public override void Update(GameTime g)
        {
            cam.Update(g);                                //Update our camera so it acts on our mouse movement and updates the view
        }

        public override void Draw(GameTime g)
        {
            Device.Clear(Color.Black);                         //Clear the screen to black. This is essentially the background color, though a
                                                               //bit more is going on behind the scenes      

            primBatch.Begin(PrimitiveType.LineList, cam);      //We have to call Begin() on primitive batch in order to start drawing.
                                                               //We're telling it that we're going to be drawing lines (LineList), and
                                                               //to use our first person camera as the viewpoint

            primBatch.DrawXZGrid(10, 10, Color.Blue);          //This draws a blue grid on the XZ plane, so we can visualize our space. 
                                                               //All of these grids will be 10x10
            primBatch.DrawXYGrid(10, 10, Color.Red);           //Draw a red grid on the XY plane
            primBatch.DrawYZGrid(10, 10, Color.Green);         //Draw a green grid on the YZ plane

            primBatch.End();                                   //We're done drawing lines...call End() to submit our drawings to the GPU

            shoulder.Draw(g, Matrix.Identity, primBatch, cam); //Finally, tell our shoulder node to render itself.
                                                               //Because the shoulder is the root, this call to Draw() propagates down to its children
                                                               //Draw() is a recursive function
                                                               //We pass it g, the elapsed game time
                                                               //           Matrix.Identity, the transformation for the shoulder, if we wish to have one
                                                               //           primBatch, for rendering the mesh, and
                                                               //           cam, to specify which camera will be used to render the scene
        }
    }
}
