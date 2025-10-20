// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.Sets.ConstrainedPointSet
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System.Collections.Generic;

#nullable disable
namespace Poly2Tri.Triangulation.Sets
{
  public class ConstrainedPointSet : PointSet
  {
    private List<TriangulationPoint> _constrainedPointList;

    public ConstrainedPointSet(List<TriangulationPoint> points, int[] index)
      : base(points)
    {
      this.EdgeIndex = index;
    }

    public ConstrainedPointSet(
      List<TriangulationPoint> points,
      IEnumerable<TriangulationPoint> constraints)
      : base(points)
    {
      this._constrainedPointList = new List<TriangulationPoint>();
      this._constrainedPointList.AddRange(constraints);
    }

    public int[] EdgeIndex { get; private set; }

    public override TriangulationMode TriangulationMode => TriangulationMode.Constrained;

    public override void PrepareTriangulation(TriangulationContext tcx)
    {
      base.PrepareTriangulation(tcx);
      if (this._constrainedPointList != null)
      {
        List<TriangulationPoint>.Enumerator enumerator = this._constrainedPointList.GetEnumerator();
        while (enumerator.MoveNext())
        {
          TriangulationPoint current1 = enumerator.Current;
          enumerator.MoveNext();
          TriangulationPoint current2 = enumerator.Current;
          tcx.NewConstraint(current1, current2);
        }
      }
      else
      {
        for (int index = 0; index < this.EdgeIndex.Length; index += 2)
          tcx.NewConstraint(this.Points[this.EdgeIndex[index]], this.Points[this.EdgeIndex[index + 1]]);
      }
    }

    public bool isValid() => true;
  }
}
