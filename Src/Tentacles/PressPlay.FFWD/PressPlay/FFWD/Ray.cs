// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Ray
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace PressPlay.FFWD
{
  public struct Ray(Vector3 origin, Vector3 direction)
  {
    private Microsoft.Xna.Framework.Ray ray = new Microsoft.Xna.Framework.Ray();

    public Vector3 origin
    {
      get => (Vector3) this.ray.Position;
      set => this.ray.Position = (Microsoft.Xna.Framework.Vector3) value;
    }

    public Vector3 direction
    {
      get => (Vector3) this.ray.Direction;
      set => this.ray.Direction = (Microsoft.Xna.Framework.Vector3) value.normalized;
    }

    public Vector3 GetPoint(float distance) => this.origin + this.direction * distance;

    public float? Intersects(Plane plane) => this.ray.Intersects(plane);
  }
}
