// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Bounds
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace PressPlay.FFWD
{
  public struct Bounds
  {
    private BoundingBox box;

    public Bounds(BoundingBox b) => this.box = b;

    public Bounds(Vector3 center, Vector3 size)
    {
      Vector3 vector3 = size / 2f;
      this.box = new BoundingBox((Microsoft.Xna.Framework.Vector3) (center - vector3), (Microsoft.Xna.Framework.Vector3) (center + vector3));
    }

    public Vector3 center
    {
      get => (Vector3) ((this.box.Min + this.box.Max) / 2f);
      set
      {
        this.box = new BoundingBox((Microsoft.Xna.Framework.Vector3) (value - this.extents), (Microsoft.Xna.Framework.Vector3) (value + this.extents));
      }
    }

    public Vector3 size => (Vector3) (this.box.Max - this.box.Min);

    public Vector3 extents => this.size / 2f;

    public Vector3 min => (Vector3) this.box.Min;

    public Vector3 max => (Vector3) this.box.Max;

    public void SetMinMax(Vector3 min, Vector3 max)
    {
      this.box = new BoundingBox((Microsoft.Xna.Framework.Vector3) min, (Microsoft.Xna.Framework.Vector3) max);
    }

    public void Encapsulate(Vector3 point)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public void Expand(float amount)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public bool Intersects(Bounds b)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public void Contains(Vector3 point)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public void SqrDistance(Vector3 point)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public bool IntersectRay(Microsoft.Xna.Framework.Ray r)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public void DebugDraw(Color color) => Debug.DrawFilledBox(this.center, this.size, color);

    public override string ToString() => this.box.ToString();
  }
}
