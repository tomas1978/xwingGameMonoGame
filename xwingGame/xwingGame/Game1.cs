﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace xwingGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D xwingTexture;
        Texture2D tiefighterTexture;
        Player xwing;
        Enemy tiefighter;
        KeyboardState kstate = new KeyboardState();
        Texture2D shotTexture;
        List<Shot> shots = new List<Shot>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            xwingTexture = Content.Load<Texture2D>("xwing");
            //xwing = new Player(xwingTexture, new Vector2(100, 300), 3);
            xwing = new Player(xwingTexture, new Vector2(
                                                    Window.ClientBounds.Width/2-xwingTexture.Width, 
                                                    Window.ClientBounds.Height-xwingTexture.Height), 3);

            shotTexture = Content.Load<Texture2D>("shot");
            tiefighterTexture = Content.Load<Texture2D>("tiefighter");
            tiefighter = new Enemy(tiefighterTexture, new Vector2(100, 50),3);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.Left)) {
                xwing.MoveX(-1);
            }
            if (kstate.IsKeyDown(Keys.Right))
            {
                xwing.MoveX(1);
            }
            
            if (kstate.IsKeyDown(Keys.Space))
            {
                shots.Add(new Shot(shotTexture, new Vector2(xwing.Position.X+5, xwing.Position.Y+8)));
                shots.Add(new Shot(shotTexture, new Vector2(xwing.Position.X+xwing.Texture.Bounds.Width-12, 
                                                                xwing.Position.Y + 8)));
            }


            //Check if the enemy is moving outside the bounds of the game windows
            if (tiefighter.Position.X <= 0 || tiefighter.Position.X>=Window.ClientBounds.Width-tiefighter.Texture.Width)
            {
                tiefighter.Speed *= -1; //Change direction
            }

            tiefighter.MoveX();

            //Loop through all shots, to move them upwards and check if a shot hits an enemy
            foreach (Shot s in shots)
            {
                s.Move();
                if (s.BoundingBox.Intersects(tiefighter.BoundingBox))
                {
                    tiefighter.Position = new Vector2(10,0);
                }
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            xwing.Draw(spriteBatch);
            tiefighter.Draw(spriteBatch);
            
            foreach(Shot s in shots)
            {
                s.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
