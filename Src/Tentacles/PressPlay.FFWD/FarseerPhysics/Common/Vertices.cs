// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Vertices
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace FarseerPhysics.Common
{
  [DebuggerDisplay("Count = {Count} Vertices = {ToString()}")]
  public class Vertices : List<Vector2>
  {
    public Vertices()
    {
    }

    public Vertices(int capacity) => this.Capacity = capacity;

    public Vertices(Vector2[] vector2)
    {
      for (int index = 0; index < vector2.Length; ++index)
        this.Add(vector2[index]);
    }

    public Vertices(IList<Vector2> vertices)
    {
      for (int index = 0; index < vertices.Count; ++index)
        this.Add(vertices[index]);
    }

    public int NextIndex(int index) => index == this.Count - 1 ? 0 : index + 1;

    public Vector2 NextVertex(int index) => this[this.NextIndex(index)];

    public int PreviousIndex(int index) => index == 0 ? this.Count - 1 : index - 1;

    public Vector2 PreviousVertex(int index) => this[this.PreviousIndex(index)];

    public float GetSignedArea()
    {
      float num = 0.0f;
      for (int index1 = 0; index1 < this.Count; ++index1)
      {
        int index2 = (index1 + 1) % this.Count;
        num = num + this[index1].X * this[index2].Y - this[index1].Y * this[index2].X;
      }
      return num / 2f;
    }

    public float GetArea()
    {
      float num1 = 0.0f;
      for (int index1 = 0; index1 < this.Count; ++index1)
      {
        int index2 = (index1 + 1) % this.Count;
        num1 = num1 + this[index1].X * this[index2].Y - this[index1].Y * this[index2].X;
      }
      float num2 = num1 / 2f;
      return (double) num2 >= 0.0 ? num2 : -num2;
    }

    public Vector2 GetCentroid()
    {
      Vector2 zero1 = Vector2.Zero;
      float num1 = 0.0f;
      Vector2 zero2 = Vector2.Zero;
      for (int index = 0; index < this.Count; ++index)
      {
        Vector2 vector2_1 = zero2;
        Vector2 vector2_2 = this[index];
        Vector2 vector2_3 = index + 1 < this.Count ? this[index + 1] : this[0];
        float num2 = 0.5f * MathUtils.Cross(vector2_2 - vector2_1, vector2_3 - vector2_1);
        num1 += num2;
        zero1 += num2 * 0.333333343f * (vector2_1 + vector2_2 + vector2_3);
      }
      return zero1 * (1f / num1);
    }

    public float GetRadius()
    {
      double d = (double) this.GetSignedArea() / 3.1415927410125732;
      if (d < 0.0)
        d *= -1.0;
      return (float) Math.Sqrt(d);
    }

    public AABB GetCollisionBox()
    {
      Vector2 vector2_1 = new Vector2(float.MaxValue, float.MaxValue);
      Vector2 vector2_2 = new Vector2(float.MinValue, float.MinValue);
      for (int index = 0; index < this.Count; ++index)
      {
        if ((double) this[index].X < (double) vector2_1.X)
          vector2_1.X = this[index].X;
        if ((double) this[index].X > (double) vector2_2.X)
          vector2_2.X = this[index].X;
        if ((double) this[index].Y < (double) vector2_1.Y)
          vector2_1.Y = this[index].Y;
        if ((double) this[index].Y > (double) vector2_2.Y)
          vector2_2.Y = this[index].Y;
      }
      AABB collisionBox;
      collisionBox.LowerBound = vector2_1;
      collisionBox.UpperBound = vector2_2;
      return collisionBox;
    }

    public void Translate(Vector2 vector) => this.Translate(ref vector);

    public void Translate(ref Vector2 vector)
    {
      for (int index = 0; index < this.Count; ++index)
        this[index] = Vector2.Add(this[index], vector);
    }

    public void Scale(ref Vector2 value)
    {
      for (int index = 0; index < this.Count; ++index)
        this[index] = Vector2.Multiply(this[index], value);
    }

    public void Rotate(float value)
    {
      Matrix result;
      Matrix.CreateRotationZ(value, out result);
      for (int index = 0; index < this.Count; ++index)
        this[index] = Vector2.Transform(this[index], result);
    }

    public bool IsConvex()
    {
      for (int index1 = 0; index1 < this.Count; ++index1)
      {
        int index2 = index1;
        int index3 = index1 + 1 < this.Count ? index1 + 1 : 0;
        Vector2 vector2_1 = this[index3] - this[index2];
        for (int index4 = 0; index4 < this.Count; ++index4)
        {
          if (index4 != index2 && index4 != index3)
          {
            Vector2 vector2_2 = this[index4] - this[index2];
            if ((double) vector2_1.X * (double) vector2_2.Y - (double) vector2_1.Y * (double) vector2_2.X <= 0.0)
              return false;
          }
        }
      }
      return true;
    }

    public bool IsCounterClockWise() => this.Count < 3 || (double) this.GetSignedArea() > 0.0;

    public void ForceCounterClockWise()
    {
      if (this.IsCounterClockWise())
        return;
      this.Reverse();
    }

    public bool IsSimple()
    {
      for (int index1 = 0; index1 < this.Count; ++index1)
      {
        int index2 = index1 + 1 > this.Count - 1 ? 0 : index1 + 1;
        Vector2 a0 = new Vector2(this[index1].X, this[index1].Y);
        Vector2 a1 = new Vector2(this[index2].X, this[index2].Y);
        for (int index3 = index1 + 1; index3 < this.Count; ++index3)
        {
          int index4 = index3 + 1 > this.Count - 1 ? 0 : index3 + 1;
          Vector2 b0 = new Vector2(this[index3].X, this[index3].Y);
          Vector2 b1 = new Vector2(this[index4].X, this[index4].Y);
          if (LineTools.LineIntersect2(a0, a1, b0, b1, out Vector2 _))
            return false;
        }
      }
      return true;
    }

    public bool IsSimple2()
    {
      for (int index1 = 0; index1 < this.Count; ++index1)
      {
        if (index1 < this.Count - 1)
        {
          for (int index2 = index1 + 1; index2 < this.Count; ++index2)
          {
            if (this[index1] == this[index2])
              return true;
          }
        }
        int index3 = (index1 + 1) % this.Count;
        Vector2 vector2_1 = this[index3] - this[index1];
        Vector2 vector2_2 = new Vector2(vector2_1.Y, -vector2_1.X);
        int num1 = (index3 + 1) % this.Count;
        int num2 = (index1 - 1 + this.Count) % this.Count;
        int num3 = num2 + (num1 < num2 ? 0 : num1 + 1);
        int index4 = num1;
        bool flag = (double) Vector2.Dot(this[index4] - this[index1], vector2_2) >= 0.0;
        Vector2 vector2_3 = this[index4];
        for (int index5 = index4 + 1; index5 <= num3; ++index5)
        {
          int index6 = index5 % this.Count;
          Vector2 vector2_4 = this[index6] - this[index1];
          if (flag != (double) Vector2.Dot(vector2_4, vector2_2) >= 0.0)
          {
            Vector2 vector2_5 = this[index6] - vector2_3;
            Vector2 vector2_6 = new Vector2(vector2_5.Y, -vector2_5.X);
            if ((double) Vector2.Dot(this[index1] - vector2_3, vector2_6) >= 0.0 != (double) Vector2.Dot(this[index3] - vector2_3, vector2_6) >= 0.0)
              return true;
          }
          flag = (double) Vector2.Dot(vector2_4, vector2_2) > 0.0;
          vector2_3 = this[index6];
        }
      }
      return false;
    }

    public bool CheckPolygon()
    {
      int num = -1;
      if (this.Count < 3 || this.Count > Settings.MaxPolygonVertices)
        num = 0;
      if (!this.IsConvex())
        num = 1;
      if (!this.IsSimple())
        num = 2;
      if ((double) this.GetArea() < 1.1920928955078125E-07)
        num = 3;
      Vector2[] vector2Array = new Vector2[this.Count];
      Vertices vertices = new Vertices(this.Count);
      for (int index1 = 0; index1 < this.Count; ++index1)
      {
        vertices.Add(new Vector2(this[index1].X, this[index1].Y));
        int index2 = index1;
        int index3 = index1 + 1 < this.Count ? index1 + 1 : 0;
        Vector2 a = new Vector2(this[index3].X - this[index2].X, this[index3].Y - this[index2].Y);
        vector2Array[index1] = MathUtils.Cross(a, 1f);
        vector2Array[index1].Normalize();
      }
      for (int index4 = 0; index4 < this.Count; ++index4)
      {
        int index5 = index4 == 0 ? this.Count - 1 : index4 - 1;
        if (Math.Asin((double) MathUtils.Clamp(MathUtils.Cross(vector2Array[index5], vector2Array[index4]), -1f, 1f)) <= Math.PI / 90.0)
        {
          num = 4;
          break;
        }
        for (int index6 = 0; index6 < this.Count; ++index6)
        {
          if (index6 != index4 && index6 != (index4 + 1) % this.Count && (double) Vector2.Dot(vector2Array[index4], vertices[index6] - vertices[index4]) >= -0.004999999888241291)
            num = 5;
        }
        Vector2 centroid = vertices.GetCentroid();
        Vector2 vector2_1 = vector2Array[index5];
        Vector2 vector2_2 = vector2Array[index4];
        Vector2 vector2_3 = vertices[index4] - centroid;
        Vector2 vector2_4 = new Vector2();
        vector2_4.X = Vector2.Dot(vector2_1, vector2_3);
        vector2_4.Y = Vector2.Dot(vector2_2, vector2_3);
        if ((double) vector2_4.X < 0.0 || (double) vector2_4.Y < 0.0)
          num = 6;
      }
      switch (num)
      {
        default:
          return num != -1;
      }
    }

    public Vertices TraceEdge(Vertices verts)
    {
      Vertices.PolyNode[] polyNodeArray = new Vertices.PolyNode[verts.Count * verts.Count];
      int index1 = 0;
      for (int index2 = 0; index2 < verts.Count; ++index2)
      {
        Vector2 vector2 = new Vector2(verts[index2].X, verts[index2].Y);
        polyNodeArray[index2].Position = vector2;
        ++index1;
        int index3 = index2 == verts.Count - 1 ? 0 : index2 + 1;
        int index4 = index2 == 0 ? verts.Count - 1 : index2 - 1;
        polyNodeArray[index2].AddConnection(polyNodeArray[index3]);
        polyNodeArray[index2].AddConnection(polyNodeArray[index4]);
      }
      bool flag1 = true;
      int num1 = 0;
      while (flag1)
      {
        flag1 = false;
        for (int index5 = 0; index5 < index1; ++index5)
        {
          for (int index6 = 0; index6 < polyNodeArray[index5].NConnected; ++index6)
          {
            for (int index7 = 0; index7 < index1; ++index7)
            {
              if (index7 != index5 && polyNodeArray[index7] != polyNodeArray[index5].Connected[index6])
              {
                for (int index8 = 0; index8 < polyNodeArray[index7].NConnected; ++index8)
                {
                  Vector2 intersectionPoint;
                  if (polyNodeArray[index7].Connected[index8] != polyNodeArray[index5].Connected[index6] && polyNodeArray[index7].Connected[index8] != polyNodeArray[index5] && LineTools.LineIntersect(polyNodeArray[index5].Position, polyNodeArray[index5].Connected[index6].Position, polyNodeArray[index7].Position, polyNodeArray[index7].Connected[index8].Position, out intersectionPoint))
                  {
                    flag1 = true;
                    Vertices.PolyNode polyNode1 = polyNodeArray[index5].Connected[index6];
                    Vertices.PolyNode polyNode2 = polyNodeArray[index7].Connected[index8];
                    polyNodeArray[index5].Connected[index6].RemoveConnection(polyNodeArray[index5]);
                    polyNodeArray[index5].RemoveConnection(polyNode1);
                    polyNodeArray[index7].Connected[index8].RemoveConnection(polyNodeArray[index7]);
                    polyNodeArray[index7].RemoveConnection(polyNode2);
                    polyNodeArray[index1] = new Vertices.PolyNode(intersectionPoint);
                    polyNodeArray[index1].AddConnection(polyNodeArray[index5]);
                    polyNodeArray[index5].AddConnection(polyNodeArray[index1]);
                    polyNodeArray[index1].AddConnection(polyNodeArray[index7]);
                    polyNodeArray[index7].AddConnection(polyNodeArray[index1]);
                    polyNodeArray[index1].AddConnection(polyNode1);
                    polyNode1.AddConnection(polyNodeArray[index1]);
                    polyNodeArray[index1].AddConnection(polyNode2);
                    polyNode2.AddConnection(polyNodeArray[index1]);
                    ++index1;
                    goto label_19;
                  }
                }
              }
            }
          }
        }
label_19:
        ++num1;
      }
      bool flag2 = true;
      int num2 = index1;
      while (flag2)
      {
        flag2 = false;
        for (int index9 = 0; index9 < index1; ++index9)
        {
          if (polyNodeArray[index9].NConnected != 0)
          {
            for (int index10 = index9 + 1; index10 < index1; ++index10)
            {
              if (polyNodeArray[index10].NConnected != 0 && (double) (polyNodeArray[index9].Position - polyNodeArray[index10].Position).LengthSquared() <= 1.4210854715202004E-14)
              {
                if (num2 <= 3)
                  return new Vertices();
                --num2;
                flag2 = true;
                Vertices.PolyNode toMe1 = polyNodeArray[index9];
                Vertices.PolyNode fromMe = polyNodeArray[index10];
                int nconnected = fromMe.NConnected;
                for (int index11 = 0; index11 < nconnected; ++index11)
                {
                  Vertices.PolyNode toMe2 = fromMe.Connected[index11];
                  if (toMe2 != toMe1)
                  {
                    toMe1.AddConnection(toMe2);
                    toMe2.AddConnection(toMe1);
                  }
                  toMe2.RemoveConnection(fromMe);
                }
                fromMe.NConnected = 0;
              }
            }
          }
        }
      }
      float num3 = float.MaxValue;
      float num4 = float.MinValue;
      int index12 = -1;
      for (int index13 = 0; index13 < index1; ++index13)
      {
        if ((double) polyNodeArray[index13].Position.Y < (double) num3 && polyNodeArray[index13].NConnected > 1)
        {
          num3 = polyNodeArray[index13].Position.Y;
          index12 = index13;
          num4 = polyNodeArray[index13].Position.X;
        }
        else if ((double) polyNodeArray[index13].Position.Y == (double) num3 && (double) polyNodeArray[index13].Position.X > (double) num4 && polyNodeArray[index13].NConnected > 1)
        {
          index12 = index13;
          num4 = polyNodeArray[index13].Position.X;
        }
      }
      Vector2 incomingDir = new Vector2(1f, 0.0f);
      Vector2[] vector2Array = new Vector2[4 * index1];
      int capacity1 = 0;
      Vertices.PolyNode polyNode3 = polyNodeArray[index12];
      Vertices.PolyNode polyNode4 = polyNode3;
      Vertices.PolyNode rightestConnection = polyNode3.GetRightestConnection(incomingDir);
      if (rightestConnection == null)
      {
        Vertices vertices = new Vertices(capacity1);
        for (int index14 = 0; index14 < capacity1; ++index14)
          vertices.Add(vector2Array[index14]);
        return vertices;
      }
      vector2Array[0] = polyNode4.Position;
      int capacity2 = capacity1 + 1;
      while (rightestConnection != polyNode4)
      {
        int num5 = 4 * index1;
        vector2Array[capacity2++] = rightestConnection.Position;
        Vertices.PolyNode incoming = polyNode3;
        polyNode3 = rightestConnection;
        rightestConnection = polyNode3.GetRightestConnection(incoming);
        if (rightestConnection == null)
        {
          Vertices vertices = new Vertices(capacity2);
          for (int index15 = 0; index15 < capacity2; ++index15)
            vertices.Add(vector2Array[index15]);
          return vertices;
        }
      }
      return new Vertices();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < this.Count; ++index)
      {
        stringBuilder.Append(this[index].ToString());
        if (index < this.Count - 1)
          stringBuilder.Append(" ");
      }
      return stringBuilder.ToString();
    }

    public void ProjectToAxis(ref Vector2 axis, out float min, out float max)
    {
      float num1 = Vector2.Dot(axis, this[0]);
      min = num1;
      max = num1;
      for (int index = 0; index < this.Count; ++index)
      {
        float num2 = Vector2.Dot(this[index], axis);
        if ((double) num2 < (double) min)
          min = num2;
        else if ((double) num2 > (double) max)
          max = num2;
      }
    }

    public int PointInPolygon(ref Vector2 point)
    {
      int num1 = 0;
      for (int index = 0; index < this.Count; ++index)
      {
        Vector2 a = this[index];
        Vector2 b = this[this.NextIndex(index)];
        Vector2 vector2 = b - a;
        float num2 = MathUtils.Area(ref a, ref b, ref point);
        if ((double) num2 == 0.0 && (double) Vector2.Dot(point - a, vector2) >= 0.0 && (double) Vector2.Dot(point - b, vector2) <= 0.0)
          return 0;
        if ((double) a.Y <= (double) point.Y)
        {
          if ((double) b.Y > (double) point.Y && (double) num2 > 0.0)
            ++num1;
        }
        else if ((double) b.Y <= (double) point.Y && (double) num2 < 0.0)
          --num1;
      }
      return num1 != 0 ? 1 : -1;
    }

    public bool PointInPolygonAngle(ref Vector2 point)
    {
      double num = 0.0;
      for (int index = 0; index < this.Count; ++index)
      {
        Vector2 p1 = this[index] - point;
        Vector2 p2 = this[this.NextIndex(index)] - point;
        num += MathUtils.VectorAngle(ref p1, ref p2);
      }
      return Math.Abs(num) >= Math.PI;
    }

    private class PolyNode
    {
      private const int MaxConnected = 32;
      public Vertices.PolyNode[] Connected = new Vertices.PolyNode[32];
      public int NConnected;
      public Vector2 Position;

      public PolyNode(Vector2 pos)
      {
        this.Position = pos;
        this.NConnected = 0;
      }

      private bool IsRighter(float sinA, float cosA, float sinB, float cosB)
      {
        return (double) sinA < 0.0 ? (double) sinB > 0.0 || (double) cosA <= (double) cosB : (double) sinB >= 0.0 && (double) cosA > (double) cosB;
      }

      public void AddConnection(Vertices.PolyNode toMe)
      {
        for (int index = 0; index < this.NConnected; ++index)
        {
          if (this.Connected[index] == toMe)
            return;
        }
        this.Connected[this.NConnected] = toMe;
        ++this.NConnected;
      }

      public void RemoveConnection(Vertices.PolyNode fromMe)
      {
        int num = -1;
        for (int index = 0; index < this.NConnected; ++index)
        {
          if (fromMe == this.Connected[index])
          {
            num = index;
            break;
          }
        }
        --this.NConnected;
        for (int index = num; index < this.NConnected; ++index)
          this.Connected[index] = this.Connected[index + 1];
      }

      public Vertices.PolyNode GetRightestConnection(Vertices.PolyNode incoming)
      {
        int nconnected = this.NConnected;
        if (this.NConnected == 1)
          return incoming;
        Vector2 a = this.Position - incoming.Position;
        double num1 = (double) a.Length();
        a.Normalize();
        Vertices.PolyNode rightestConnection = (Vertices.PolyNode) null;
        for (int index = 0; index < this.NConnected; ++index)
        {
          if (this.Connected[index] != incoming)
          {
            Vector2 b1 = this.Connected[index].Position - this.Position;
            double num2 = (double) b1.LengthSquared();
            b1.Normalize();
            float cosA = Vector2.Dot(a, b1);
            float sinA = MathUtils.Cross(a, b1);
            if (rightestConnection != null)
            {
              Vector2 b2 = rightestConnection.Position - this.Position;
              b2.Normalize();
              float cosB = Vector2.Dot(a, b2);
              float sinB = MathUtils.Cross(a, b2);
              if (this.IsRighter(sinA, cosA, sinB, cosB))
                rightestConnection = this.Connected[index];
            }
            else
              rightestConnection = this.Connected[index];
          }
        }
        return rightestConnection;
      }

      public Vertices.PolyNode GetRightestConnection(Vector2 incomingDir)
      {
        return this.GetRightestConnection(new Vertices.PolyNode(this.Position - incomingDir));
      }
    }
  }
}
