using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

interface InputController
{
  bool Quit { get; }
  Vector2 ShipMovement { get; }
  bool Shooting { get; }

  void Update(float dt);
}

class KeyboardController : InputController
{
  KeyboardState ks;

  public bool Quit
  {
    get
    {
      return ks.IsKeyDown(Keys.Escape);
    }
  }

  public Vector2 ShipMovement
  {
    get
    {
      var shipMovement = Vector2.Zero;
      if (ks.IsKeyDown(Keys.A))
        shipMovement = shipMovement + new Vector2(-1.0f, 0.0f);
      if (ks.IsKeyDown(Keys.D))
        shipMovement = shipMovement + new Vector2(1.0f, 0.0f);
      if (ks.IsKeyDown(Keys.W))
        shipMovement = shipMovement + new Vector2(0.0f, -1.0f);
      if (ks.IsKeyDown(Keys.S))
        shipMovement = shipMovement + new Vector2(0.0f, 1.0f);
      return shipMovement;
    }
  }

  public bool Shooting
  {
    get
    {
      return ks.IsKeyDown(Keys.Space);
    }
  }

  public void Update(float dt)
  {
    ks = Keyboard.GetState();
  }
}

class MouseController : InputController
{
  MouseState ms;

  public bool Quit
  {
    get
    {
      return false;
    }
  }

  public Vector2 ShipMovement
  {
    get
    {
      return new Vector2(ms.X - 400, ms.Y - 300) * 0.01f;
    }
  }

  public bool Shooting
  {
    get
    {
      return ms.LeftButton == ButtonState.Pressed;
    }
  }

  public void Update(float dt)
  {
    ms = Mouse.GetState();
  }
}

class ControllerSum : InputController
{
  InputController first, second;
  public ControllerSum(InputController a, InputController b)
  {
    first = a;
    second = b;
  }

  public bool Quit
  {
    get
    {
      return first.Quit || second.Quit;
    }
  }

  public Vector2 ShipMovement
  {
    get
    {
      return first.ShipMovement + second.ShipMovement;
    }
  }

  public bool Shooting
  {
    get
    {
      return first.Shooting || second.Shooting;
    }
  }

  public void Update(float dt)
  {
    first.Update(dt);
    second.Update(dt);
  }
}
