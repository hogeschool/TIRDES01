using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Main
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        Vector2 shipPosition;
        Texture2D shipAppearance;
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            shipPosition = new Vector2(400, 400);
            shipAppearance = Content.Load<Texture2D>("ship.png");

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();
            Func<bool, float> toUnit = b => b ? 1.0f : 0.0f;
            var shipDelta = Vector2.Zero;
            shipDelta += Vector2.UnitX * toUnit(keyboardState.IsKeyDown(Keys.Right));
            shipDelta -= Vector2.UnitX * toUnit(keyboardState.IsKeyDown(Keys.Left));
            shipDelta += Vector2.UnitY * toUnit(keyboardState.IsKeyDown(Keys.Down));
            shipDelta -= Vector2.UnitY * toUnit(keyboardState.IsKeyDown(Keys.Up));
            shipPosition += shipDelta * 5.0f;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(shipAppearance, shipPosition, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
