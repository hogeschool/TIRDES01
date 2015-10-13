using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Content;

struct Entity
{
  public Entity(Vector2 p, Texture2D a)
  {
    Position = p;
    Appearance = a;
  }

  public Vector2 Position { get; private set; }
  public Texture2D Appearance { get; private set; }

  public float X { get { return Position.X; } }
  public float Y { get { return Position.Y; } }

  public Entity CreateMoved(Vector2 deltaPosition)
  {
    return new Entity()
    {
      Position = this.Position + deltaPosition,
      Appearance = this.Appearance
    };
  }
}
