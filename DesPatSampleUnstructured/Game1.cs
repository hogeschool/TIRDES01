using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace DesPatSampleUnstructured
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

    int gameLogicScriptPC = 0;
    int rndNumberLine1, iLine1;
    float timeToWaitLine3, timeToWaitLine4,timeToWaitLine8,timeToWaitLine7;
    int rndNumberLine5, iLine5;

    Random randomGenerator = new Random();
    Texture2D shipAppearance, asteroidAppearance, plasmaAppearance;
    List<Vector2> asteroidPositions = new List<Vector2>();
    List<Vector2> plasmaPositions = new List<Vector2>();
    Vector2 shipPosition;
    float shipSpeed;
    protected override void LoadContent()
    {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);

      shipAppearance = Content.Load<Texture2D>("ship.png");
      asteroidAppearance = Content.Load<Texture2D>("asteroid.png");
      plasmaAppearance = Content.Load<Texture2D>("plasmaSmall.png");
      shipPosition = new Vector2(300.0f, 400.0f);
      shipSpeed = 100.0f;
    }

    protected override void Update(GameTime gameTime)
    {
      KeyboardState ks = Keyboard.GetState();
      float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
      if (ks.IsKeyDown(Keys.Escape))
        Exit();

      var newPlasmaPositions =
        (from plasmaPosition in plasmaPositions
         let colliders =
            from asteroidPosition in asteroidPositions
            where Vector2.Distance(plasmaPosition, asteroidPosition) < 20.0f
            select asteroidPosition
         where plasmaPosition.X > 50.0f &&
               plasmaPosition.X < 600.0f &&
               plasmaPosition.Y > 50.0f &&
               plasmaPosition.Y < 600.0f &&
               colliders.Count() == 0
         select plasmaPosition - Vector2.UnitY * 200.0f * deltaTime).ToList();
      if (ks.IsKeyDown(Keys.Space))
        newPlasmaPositions.Add(shipPosition);

      var newAsteroidPositions =
        (from asteroidPosition in asteroidPositions
         let colliders =
            from plasmaPosition in plasmaPositions
            where Vector2.Distance(plasmaPosition, asteroidPosition) < 20.0f
            select plasmaPosition
         where asteroidPosition.X > 50.0f &&
               asteroidPosition.X < 600.0f &&
               asteroidPosition.Y > 50.0f &&
               asteroidPosition.Y < 300.0f &&
               colliders.Count() == 0
         select asteroidPosition + Vector2.UnitY * 100.0f * deltaTime).ToList();

      Vector2 shipMovement = new Vector2(0.0f, 0.0f);
      if (ks.IsKeyDown(Keys.A))
        shipMovement = shipMovement + new Vector2(-1.0f, 0.0f);
      if (ks.IsKeyDown(Keys.D))
        shipMovement = shipMovement + new Vector2(1.0f, 0.0f);
      if (ks.IsKeyDown(Keys.W))
        shipMovement = shipMovement + new Vector2(0.0f, -1.0f);
      if (ks.IsKeyDown(Keys.S))
        shipMovement = shipMovement + new Vector2(0.0f, 1.0f);
      Vector2 shipVelocity = shipMovement * shipSpeed;
      shipPosition = shipPosition + shipVelocity * deltaTime;

      switch (gameLogicScriptPC)
      {
        case 0:
          if (true)
          {
            gameLogicScriptPC = 1;
            iLine1 = 1;
            rndNumberLine1 = randomGenerator.Next(20, 60);
          }
          else
            gameLogicScriptPC = 9;
          break;
        case 1:
          if (iLine1 <= rndNumberLine1)
            gameLogicScriptPC = 2;
          else { 
            gameLogicScriptPC = 4;
            timeToWaitLine4 = (float)(randomGenerator.NextDouble() * 2.0 + 5.0);
          }
          break;
        case 2:
          newAsteroidPositions.Add(new Vector2((float)(randomGenerator.NextDouble() * 500.0 + 51.0), 51.0f));
          gameLogicScriptPC = 3;
          timeToWaitLine3 = (float)(randomGenerator.NextDouble() * 0.2 + 0.1);
          break;
        case 3:
          timeToWaitLine3 -= deltaTime;
          if (timeToWaitLine3 > 0.0f)
            gameLogicScriptPC = 3;
          else
          {
            gameLogicScriptPC = 1;
            iLine1++;
          }
          break;
        case 4:
          timeToWaitLine4 -= deltaTime;
          if (timeToWaitLine4 > 0.0f)
            gameLogicScriptPC = 4;
          else
          {
            gameLogicScriptPC = 5;
            iLine5 = 1;
            rndNumberLine5 = randomGenerator.Next(10, 20);
          }
          break;
        case 5:
          if(iLine5 <= rndNumberLine5)
          {
            gameLogicScriptPC = 6;
          }
          else
          {
            gameLogicScriptPC = 8;
            timeToWaitLine8 = (float)(randomGenerator.NextDouble() * 2.0 + 5.0);
          }
          break;
        case 6:
          newAsteroidPositions.Add(new Vector2((float)(randomGenerator.NextDouble() * 500.0 + 51.0), 51.0f));
          gameLogicScriptPC = 7;
          timeToWaitLine7 = (float)(randomGenerator.NextDouble() * 1.5 + 0.5);
          break;
        case 7:
          timeToWaitLine7 -= deltaTime;
          if (timeToWaitLine7 > 0)
            gameLogicScriptPC = 7;
          else
          {
            gameLogicScriptPC = 5;
            iLine5++;
          }
          break;
        case 8:
          timeToWaitLine8 -= deltaTime;
          if (timeToWaitLine8 > 0.0f)
            gameLogicScriptPC = 8;
          else
          {
            gameLogicScriptPC = 0;
          }
          break;
        default:
          break;
      }


      // COMMIT CHANGES TO THE STATE
      plasmaPositions = newPlasmaPositions;
      asteroidPositions = newAsteroidPositions;

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Black);

      spriteBatch.Begin();
      spriteBatch.Draw(shipAppearance, shipPosition, Color.White);
      foreach (var plasmaPosition in plasmaPositions)
      {
        spriteBatch.Draw(plasmaAppearance, plasmaPosition, Color.White);
      }
      foreach (var asteroidPosition in asteroidPositions)
      {
        spriteBatch.Draw(asteroidAppearance, asteroidPosition, Color.White);
      }
      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
