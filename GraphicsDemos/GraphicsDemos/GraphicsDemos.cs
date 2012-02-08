using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GraphicsDemos.Demos;
using GraphicsToolkit.Input;
using GraphicsToolkit;

namespace GraphicsDemos
{
    public class GraphicsDemos : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Demo> demos;
        int currentDemo = 0;

        public GraphicsDemos()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Components.Add(new InputHandler(this));

            //Enable anti-aliasing
            graphics.PreferMultiSampling = true;

            //Put our resolution at 720p
            graphics.PreferredBackBufferWidth = Config.ScreenWidth = 800;
            graphics.PreferredBackBufferHeight = Config.ScreenHeight = 600;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            demos = new List<Demo>();
            demos.Add(new RobotDemo());
            currentDemo = 0;
            demos[currentDemo].LoadContent(Content, GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            demos[currentDemo].UnloadContent();
        }

        private void ChangeDemo(int demo)
        {
            if (demo < 0 || demo >= demos.Count)
            {
                return;
            }

            demos[currentDemo].UnloadContent();
            currentDemo = demo;
            demos[currentDemo].LoadContent(Content, GraphicsDevice);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            //Update our current demo
            demos[currentDemo].Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Draw our current demo
            demos[currentDemo].Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
