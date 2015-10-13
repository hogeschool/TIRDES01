using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Content;
using DesPatSampleUnstructured.Scripts;

namespace DesPatSampleUnstructured
{
  // suggestion for extensibility of results
  //interface InstructionRes {}
  //class Running : InstructionRes { }
  //class Done : InstructionRes{ }
  //class And : InstructionRes
  //{
  //  public InstructionRes A, B;
  //  public And(InstructionRes a, InstructionRes b) { A = a;  B = b; }
  //}
  //class Nothing : InstructionRes { }
  //class CreateAsteroid : InstructionRes { }

  //new And(new Done(), new CreateAsteroid())
  //new Done()

  enum InstructionResult
  {
    Done,
    DoneAndCreateAsteroid,
    Running,
    RunningAndCreateAsteroid
  }

  public class Game1 : Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
    }

    static Random rand = new Random();
    Instruction gameLogic =
      new Repeat(
          new For(0, 10, i =>
                new Wait(() => i * 0.1f) +
                new CreateAsteroid()) +
          new Wait(() => rand.Next(1,5)) +
          new For(0, 10, i =>
                new Wait(() => (float)rand.NextDouble() * 1.0f + 0.2f) +
                new CreateAsteroid()) +
          new Wait(() => rand.Next(2, 3)));

    InputController input = new KeyboardController();

    Random randomGenerator = new Random();
    List<Entity> asteroids = new List<Entity>();
    List<Entity> plasmas = new List<Entity>();
    Entity ship;
    float shipSpeed;

    Weapon<Entity> currentWeapon;

    protected override void LoadContent()
    {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);

      ship = new Entity(new Vector2(300.0f, 400.0f),
        Content.Load<Texture2D>("ship.png"));
      currentWeapon = new DoubleBlaster(Content);
      shipSpeed = 100.0f;
    }

    protected override void Update(GameTime gameTime)
    {
      float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
      input.Update(deltaTime);
      if (input.Quit)
        Exit();

      var newPlasmas =
        (from plasma in plasmas
         let colliders =
            from asteroid in asteroids
            where Vector2.Distance(plasma.Position, asteroid.Position) < 20.0f
            select asteroid
         where plasma.X > 50.0f &&
               plasma.X < 600.0f &&
               plasma.Y > 50.0f &&
               plasma.Y < 600.0f &&
               colliders.Count() == 0
         select plasma.CreateMoved(-Vector2.UnitY * 200.0f * deltaTime)).ToList();
      currentWeapon.Update(deltaTime, ship.Position);
      if (input.Shooting)
        currentWeapon.PullTrigger();
      newPlasmas.AddRange(currentWeapon.NewBullets);

      if (Keyboard.GetState().IsKeyDown(Keys.D1))
        currentWeapon = new Blaster(Content);
      if (Keyboard.GetState().IsKeyDown(Keys.D2))
        currentWeapon = new DoubleBlaster(Content);

      var newAsteroids =
        (from asteroid in asteroids
         let colliders =
            from plasma in plasmas
            where Vector2.Distance(plasma.Position, asteroid.Position) < 20.0f
            select plasma
         where asteroid.X > 50.0f &&
               asteroid.X < 600.0f &&
               asteroid.Y > 50.0f &&
               asteroid.Y < 300.0f &&
               colliders.Count() == 0
         select asteroid.CreateMoved(Vector2.UnitY * 100.0f * deltaTime)).ToList();

      Vector2 shipVelocity = input.ShipMovement * shipSpeed;
      var newShip = ship.CreateMoved(shipVelocity * deltaTime);

      switch (gameLogic.Execute(deltaTime))
      {
        case InstructionResult.DoneAndCreateAsteroid:
          newAsteroids.Add(
            new Entity(new Vector2((float)(randomGenerator.NextDouble() * 500.0 + 51.0), 51.0f),
              Content.Load<Texture2D>("asteroid")));
          break;
        case InstructionResult.RunningAndCreateAsteroid:
          newAsteroids.Add(
            new Entity(new Vector2((float)(randomGenerator.NextDouble() * 500.0 + 51.0), 51.0f),
              Content.Load<Texture2D>("asteroid")));
          break;
      }

      // COMMIT CHANGES TO THE STATE
      plasmas = newPlasmas;
      asteroids = newAsteroids;
      ship = newShip;

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Black);

      spriteBatch.Begin();
      spriteBatch.Draw(ship.Appearance, ship.Position, Color.White);
      foreach (var plasma in plasmas)
      {
        spriteBatch.Draw(plasma.Appearance, plasma.Position, Color.White);
      }
      foreach (var asteroid in asteroids)
      {
        spriteBatch.Draw(asteroid.Appearance, asteroid.Position, Color.White);
      }
      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
