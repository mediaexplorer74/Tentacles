// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.DetectedVertices
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common
{
  public class DetectedVertices : Vertices
  {
    private List<Vertices> _holes;

    public List<Vertices> Holes
    {
      get => this._holes;
      set => this._holes = value;
    }

    public DetectedVertices()
    {
    }

    public DetectedVertices(Vertices vertices)
      : base((IList<Vector2>) vertices)
    {
    }

    public void Transform(Matrix transform)
    {
      for (int index = 0; index < this.Count; ++index)
        this[index] = Vector2.Transform(this[index], transform);
      if (this._holes == null || this._holes.Count <= 0)
        return;
      for (int index = 0; index < this._holes.Count; ++index)
      {
        Vector2[] array = this._holes[index].ToArray();
        Vector2.Transform(array, ref transform, array);
        this._holes[index] = new Vertices(array);
      }
    }
  }
}
