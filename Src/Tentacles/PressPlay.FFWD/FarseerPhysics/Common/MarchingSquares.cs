// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.MarchingSquares
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common
{
  public static class MarchingSquares
  {
    private static int[] look_march = new int[16]
    {
      0,
      224,
      56,
      216,
      14,
      238,
      54,
      214,
      131,
      99,
      187,
      91,
      141,
      109,
      181,
      85
    };

    public static List<Vertices> DetectSquares(
      AABB domain,
      float cell_width,
      float cell_height,
      sbyte[,] f,
      int lerp_count,
      bool combine)
    {
      MarchingSquares.CxFastList<MarchingSquares.GeomPoly> cxFastList = new MarchingSquares.CxFastList<MarchingSquares.GeomPoly>();
      List<Vertices> verticesList = new List<Vertices>();
      cxFastList.GetListOfElements();
      MarchingSquares.GeomPoly geomPoly = new MarchingSquares.GeomPoly();
      int num1 = (int) ((double) domain.Extents.X * 2.0 / (double) cell_width);
      bool flag1 = (double) num1 == (double) domain.Extents.X * 2.0 / (double) cell_width;
      int num2 = (int) ((double) domain.Extents.Y * 2.0 / (double) cell_height);
      bool flag2 = (double) num2 == (double) domain.Extents.Y * 2.0 / (double) cell_height;
      if (!flag1)
        ++num1;
      if (!flag2)
        ++num2;
      sbyte[,] fs = new sbyte[num1 + 1, num2 + 1];
      MarchingSquares.GeomPolyVal[,] geomPolyValArray = new MarchingSquares.GeomPolyVal[num1 + 1, num2 + 1];
      for (int index1 = 0; index1 < num1 + 1; ++index1)
      {
        int index2 = index1 != num1 ? (int) ((double) index1 * (double) cell_width + (double) domain.LowerBound.X) : (int) domain.UpperBound.X;
        for (int index3 = 0; index3 < num2 + 1; ++index3)
        {
          int index4 = index3 != num2 ? (int) ((double) index3 * (double) cell_height + (double) domain.LowerBound.Y) : (int) domain.UpperBound.Y;
          fs[index1, index3] = f[index2, index4];
        }
      }
      for (int ay = 0; ay < num2; ++ay)
      {
        float y0 = (float) ay * cell_height + domain.LowerBound.Y;
        float y1 = ay != num2 - 1 ? y0 + cell_height : domain.UpperBound.Y;
        MarchingSquares.GeomPoly polya = (MarchingSquares.GeomPoly) null;
        for (int ax = 0; ax < num1; ++ax)
        {
          float x0 = (float) ax * cell_width + domain.LowerBound.X;
          float x1 = ax != num1 - 1 ? x0 + cell_width : domain.UpperBound.X;
          MarchingSquares.GeomPoly P = new MarchingSquares.GeomPoly();
          int K = MarchingSquares.marchSquare(f, fs, ref P, ax, ay, x0, y0, x1, y1, lerp_count);
          if (P.length != 0)
          {
            if (combine && polya != null && (K & 9) != 0)
            {
              MarchingSquares.combLeft(ref polya, ref P);
              P = polya;
            }
            else
              cxFastList.add(P);
            geomPolyValArray[ax, ay] = new MarchingSquares.GeomPolyVal(P, K);
          }
          else
            P = (MarchingSquares.GeomPoly) null;
          polya = P;
        }
      }
      if (!combine)
      {
        foreach (MarchingSquares.GeomPoly listOfElement in cxFastList.GetListOfElements())
          verticesList.Add(new Vertices((IList<Vector2>) listOfElement.points.GetListOfElements()));
        return verticesList;
      }
      for (int index5 = 1; index5 < num2; ++index5)
      {
        int index6 = 0;
        while (index6 < num1)
        {
          MarchingSquares.GeomPolyVal geomPolyVal1 = geomPolyValArray[index6, index5];
          if (geomPolyVal1 == null)
            ++index6;
          else if ((geomPolyVal1.key & 12) == 0)
          {
            ++index6;
          }
          else
          {
            MarchingSquares.GeomPolyVal geomPolyVal2 = geomPolyValArray[index6, index5 - 1];
            if (geomPolyVal2 == null)
              ++index6;
            else if ((geomPolyVal2.key & 3) == 0)
            {
              ++index6;
            }
            else
            {
              float num3 = (float) index6 * cell_width + domain.LowerBound.X;
              float num4 = (float) index5 * cell_height + domain.LowerBound.Y;
              MarchingSquares.CxFastList<Vector2> points1 = geomPolyVal1.p.points;
              MarchingSquares.CxFastList<Vector2> points2 = geomPolyVal2.p.points;
              if (geomPolyVal2.p == geomPolyVal1.p)
              {
                ++index6;
              }
              else
              {
                MarchingSquares.CxFastListNode<Vector2> cxFastListNode1 = points1.begin();
                while ((double) MarchingSquares.square(cxFastListNode1.elem().Y - num4) > 1.4012984643248171E-45 || (double) cxFastListNode1.elem().X < (double) num3)
                  cxFastListNode1 = cxFastListNode1.next();
                cxFastListNode1.elem();
                Vector2 b = cxFastListNode1.next().elem();
                if ((double) MarchingSquares.square(b.Y - num4) > 1.4012984643248171E-45)
                {
                  ++index6;
                }
                else
                {
                  bool flag3 = true;
                  MarchingSquares.CxFastListNode<Vector2> node;
                  for (node = points2.begin(); node != points2.end(); node = node.next())
                  {
                    if ((double) MarchingSquares.vec_dsq(node.elem(), b) < 1.4012984643248171E-45)
                    {
                      flag3 = false;
                      break;
                    }
                  }
                  if (flag3)
                  {
                    ++index6;
                  }
                  else
                  {
                    MarchingSquares.CxFastListNode<Vector2> cxFastListNode2 = cxFastListNode1.next().next();
                    if (cxFastListNode2 == points1.end())
                      cxFastListNode2 = points1.begin();
                    while (cxFastListNode2 != cxFastListNode1)
                    {
                      node = points2.insert(node, cxFastListNode2.elem());
                      cxFastListNode2 = cxFastListNode2.next();
                      if (cxFastListNode2 == points1.end())
                        cxFastListNode2 = points1.begin();
                      ++geomPolyVal2.p.length;
                    }
                    float index7 = (float) (index6 + 1);
                    while ((double) index7 < (double) num1)
                    {
                      MarchingSquares.GeomPolyVal geomPolyVal3 = geomPolyValArray[(int) index7, index5];
                      if (geomPolyVal3 == null || geomPolyVal3.p != geomPolyVal1.p)
                      {
                        ++index7;
                      }
                      else
                      {
                        geomPolyVal3.p = geomPolyVal2.p;
                        ++index7;
                      }
                    }
                    float index8 = (float) (index6 - 1);
                    while ((double) index8 >= 0.0)
                    {
                      MarchingSquares.GeomPolyVal geomPolyVal4 = geomPolyValArray[(int) index8, index5];
                      if (geomPolyVal4 == null || geomPolyVal4.p != geomPolyVal1.p)
                      {
                        --index8;
                      }
                      else
                      {
                        geomPolyVal4.p = geomPolyVal2.p;
                        --index8;
                      }
                    }
                    cxFastList.remove(geomPolyVal1.p);
                    geomPolyVal1.p = geomPolyVal2.p;
                    index6 = (int) (((double) cxFastListNode1.next().elem().X - (double) domain.LowerBound.X) / (double) cell_width) + 1;
                  }
                }
              }
            }
          }
        }
      }
      foreach (MarchingSquares.GeomPoly listOfElement in cxFastList.GetListOfElements())
        verticesList.Add(new Vertices((IList<Vector2>) listOfElement.points.GetListOfElements()));
      return verticesList;
    }

    private static float lerp(float x0, float x1, float v0, float v1)
    {
      float num1 = v0 - v1;
      float num2 = (double) num1 * (double) num1 >= 1.4012984643248171E-45 ? v0 / num1 : 0.5f;
      return x0 + num2 * (x1 - x0);
    }

    private static float xlerp(
      float x0,
      float x1,
      float y,
      float v0,
      float v1,
      sbyte[,] f,
      int c)
    {
      float index = MarchingSquares.lerp(x0, x1, v0, v1);
      if (c == 0)
        return index;
      sbyte num = f[(int) index, (int) y];
      return (double) v0 * (double) num < 0.0 ? MarchingSquares.xlerp(x0, index, y, v0, (float) num, f, c - 1) : MarchingSquares.xlerp(index, x1, y, (float) num, v1, f, c - 1);
    }

    private static float ylerp(
      float y0,
      float y1,
      float x,
      float v0,
      float v1,
      sbyte[,] f,
      int c)
    {
      float index = MarchingSquares.lerp(y0, y1, v0, v1);
      if (c == 0)
        return index;
      sbyte num = f[(int) x, (int) index];
      return (double) v0 * (double) num < 0.0 ? MarchingSquares.ylerp(y0, index, x, v0, (float) num, f, c - 1) : MarchingSquares.ylerp(index, y1, x, (float) num, v1, f, c - 1);
    }

    private static float square(float x) => x * x;

    private static float vec_dsq(Vector2 a, Vector2 b)
    {
      Vector2 vector2 = a - b;
      return (float) ((double) vector2.X * (double) vector2.X + (double) vector2.Y * (double) vector2.Y);
    }

    private static float vec_cross(Vector2 a, Vector2 b)
    {
      return (float) ((double) a.X * (double) b.Y - (double) a.Y * (double) b.X);
    }

    private static int marchSquare(
      sbyte[,] f,
      sbyte[,] fs,
      ref MarchingSquares.GeomPoly poly,
      int ax,
      int ay,
      float x0,
      float y0,
      float x1,
      float y1,
      int bin)
    {
      int index1 = 0;
      sbyte f1 = fs[ax, ay];
      if (f1 < (sbyte) 0)
        index1 |= 8;
      sbyte f2 = fs[ax + 1, ay];
      if (f2 < (sbyte) 0)
        index1 |= 4;
      sbyte f3 = fs[ax + 1, ay + 1];
      if (f3 < (sbyte) 0)
        index1 |= 2;
      sbyte f4 = fs[ax, ay + 1];
      if (f4 < (sbyte) 0)
        index1 |= 1;
      int num = MarchingSquares.look_march[index1];
      if (num != 0)
      {
        MarchingSquares.CxFastListNode<Vector2> node = (MarchingSquares.CxFastListNode<Vector2>) null;
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if ((num & 1 << index2) != 0)
          {
            Vector2 vector2_1;
            if (index2 == 7 && (num & 1) == 0)
            {
              MarchingSquares.CxFastList<Vector2> points = poly.points;
              vector2_1 = new Vector2(x0, MarchingSquares.ylerp(y0, y1, x0, (float) f1, (float) f4, f, bin));
              Vector2 vector2_2 = vector2_1;
              points.add(vector2_2);
            }
            else
            {
              vector2_1 = index2 != 0 ? (index2 != 2 ? (index2 != 4 ? (index2 != 6 ? (index2 != 1 ? (index2 != 5 ? (index2 != 3 ? new Vector2(x0, MarchingSquares.ylerp(y0, y1, x0, (float) f1, (float) f4, f, bin)) : new Vector2(x1, MarchingSquares.ylerp(y0, y1, x1, (float) f2, (float) f3, f, bin))) : new Vector2(MarchingSquares.xlerp(x0, x1, y1, (float) f4, (float) f3, f, bin), y1)) : new Vector2(MarchingSquares.xlerp(x0, x1, y0, (float) f1, (float) f2, f, bin), y0)) : new Vector2(x0, y1)) : new Vector2(x1, y1)) : new Vector2(x1, y0)) : new Vector2(x0, y0);
              node = poly.points.insert(node, vector2_1);
            }
            ++poly.length;
          }
        }
      }
      return index1;
    }

    private static void combLeft(
      ref MarchingSquares.GeomPoly polya,
      ref MarchingSquares.GeomPoly polyb)
    {
      MarchingSquares.CxFastList<Vector2> points1 = polya.points;
      MarchingSquares.CxFastList<Vector2> points2 = polyb.points;
      MarchingSquares.CxFastListNode<Vector2> node = points1.begin();
      MarchingSquares.CxFastListNode<Vector2> cxFastListNode1 = points2.begin();
      Vector2 b = cxFastListNode1.elem();
      MarchingSquares.CxFastListNode<Vector2> prev1 = (MarchingSquares.CxFastListNode<Vector2>) null;
      for (; node != points1.end(); node = node.next())
      {
        Vector2 a = node.elem();
        if ((double) MarchingSquares.vec_dsq(a, b) < 1.4012984643248171E-45)
        {
          if (prev1 != null)
          {
            Vector2 vector2_1 = prev1.elem();
            Vector2 vector2_2 = cxFastListNode1.next().elem();
            float num = MarchingSquares.vec_cross(a - vector2_1, vector2_2 - a);
            if ((double) num * (double) num < 1.4012984643248171E-45)
            {
              points1.erase(prev1, node);
              --polya.length;
              node = prev1;
            }
          }
          bool flag = true;
          MarchingSquares.CxFastListNode<Vector2> prev2 = (MarchingSquares.CxFastListNode<Vector2>) null;
          while (!points2.empty())
          {
            Vector2 vector2 = points2.front();
            points2.pop();
            if (!flag && !points2.empty())
            {
              node = points1.insert(node, vector2);
              ++polya.length;
              prev2 = node;
            }
            flag = false;
          }
          MarchingSquares.CxFastListNode<Vector2> cxFastListNode2 = node.next();
          Vector2 vector2_3 = cxFastListNode2.elem();
          MarchingSquares.CxFastListNode<Vector2> cxFastListNode3 = cxFastListNode2.next();
          if (cxFastListNode3 == points1.end())
            cxFastListNode3 = points1.begin();
          Vector2 vector2_4 = cxFastListNode3.elem();
          Vector2 vector2_5 = prev2.elem();
          float num1 = MarchingSquares.vec_cross(vector2_3 - vector2_5, vector2_4 - vector2_3);
          if ((double) num1 * (double) num1 >= 1.4012984643248171E-45)
            break;
          points1.erase(prev2, prev2.next());
          --polya.length;
          break;
        }
        prev1 = node;
      }
    }

    internal class CxFastListNode<T>
    {
      internal T _elt;
      internal MarchingSquares.CxFastListNode<T> _next;

      public T elem() => this._elt;

      public MarchingSquares.CxFastListNode<T> next() => this._next;

      public CxFastListNode(T obj) => this._elt = obj;
    }

    internal class CxFastList<T>
    {
      private MarchingSquares.CxFastListNode<T> _head;
      private int count;

      public MarchingSquares.CxFastListNode<T> begin() => this._head;

      public MarchingSquares.CxFastListNode<T> end() => (MarchingSquares.CxFastListNode<T>) null;

      public T front() => this._head.elem();

      public T back() => throw new NotImplementedException();

      public T at(int i) => throw new NotImplementedException();

      public void reverse() => throw new NotImplementedException();

      public T elem() => this._head.elem();

      public MarchingSquares.CxFastListNode<T> add(T value)
      {
        MarchingSquares.CxFastListNode<T> cxFastListNode = new MarchingSquares.CxFastListNode<T>(value);
        if (this._head == null)
        {
          cxFastListNode._next = (MarchingSquares.CxFastListNode<T>) null;
          this._head = cxFastListNode;
          ++this.count;
          return cxFastListNode;
        }
        cxFastListNode._next = this._head;
        this._head = cxFastListNode;
        ++this.count;
        return cxFastListNode;
      }

      public void addAll(MarchingSquares.CxFastList<T> list) => throw new NotImplementedException();

      public bool remove(T value)
      {
        MarchingSquares.CxFastListNode<T> cxFastListNode1 = this._head;
        MarchingSquares.CxFastListNode<T> cxFastListNode2 = this._head;
        EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
        if (cxFastListNode1 != null && (object) value != null)
        {
          while (!equalityComparer.Equals(cxFastListNode1._elt, value))
          {
            cxFastListNode2 = cxFastListNode1;
            cxFastListNode1 = cxFastListNode1._next;
            if (cxFastListNode1 == null)
              goto label_6;
          }
          if (cxFastListNode1 == this._head)
          {
            this._head = cxFastListNode1._next;
            --this.count;
            return true;
          }
          cxFastListNode2._next = cxFastListNode1._next;
          --this.count;
          return true;
        }
label_6:
        return false;
      }

      public MarchingSquares.CxFastListNode<T> pop()
      {
        return this.erase((MarchingSquares.CxFastListNode<T>) null, this._head);
      }

      public MarchingSquares.CxFastListNode<T> insert(
        MarchingSquares.CxFastListNode<T> node,
        T value)
      {
        if (node == null)
          return this.add(value);
        MarchingSquares.CxFastListNode<T> cxFastListNode = new MarchingSquares.CxFastListNode<T>(value);
        cxFastListNode._next = node._next;
        node._next = cxFastListNode;
        ++this.count;
        return cxFastListNode;
      }

      public MarchingSquares.CxFastListNode<T> erase(
        MarchingSquares.CxFastListNode<T> prev,
        MarchingSquares.CxFastListNode<T> node)
      {
        MarchingSquares.CxFastListNode<T> next = node._next;
        if (prev != null)
        {
          prev._next = next;
        }
        else
        {
          if (this._head == null)
            return (MarchingSquares.CxFastListNode<T>) null;
          this._head = this._head._next;
        }
        --this.count;
        return next;
      }

      public MarchingSquares.CxFastListNode<T> splice(
        MarchingSquares.CxFastListNode<T> pre,
        MarchingSquares.CxFastListNode<T> curr,
        int cnt)
      {
        throw new NotImplementedException();
      }

      public bool empty() => this._head == null;

      public int size()
      {
        MarchingSquares.CxFastListNode<T> cxFastListNode = this.begin();
        int num = 0;
        do
        {
          ++num;
        }
        while (cxFastListNode.next() != null);
        return num;
      }

      public void clear()
      {
        MarchingSquares.CxFastListNode<T> cxFastListNode1 = this._head;
        while (cxFastListNode1 != null)
        {
          MarchingSquares.CxFastListNode<T> cxFastListNode2 = cxFastListNode1;
          cxFastListNode1 = cxFastListNode1._next;
          cxFastListNode2._next = (MarchingSquares.CxFastListNode<T>) null;
        }
        this._head = (MarchingSquares.CxFastListNode<T>) null;
        this.count = 0;
      }

      public bool has(T value) => this.Find(value) != null;

      public MarchingSquares.CxFastListNode<T> Find(T value)
      {
        MarchingSquares.CxFastListNode<T> cxFastListNode = this._head;
        EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
        if (cxFastListNode != null)
        {
          if ((object) value != null)
          {
            while (!equalityComparer.Equals(cxFastListNode._elt, value))
            {
              cxFastListNode = cxFastListNode._next;
              if (cxFastListNode == this._head)
                goto label_8;
            }
            return cxFastListNode;
          }
          while ((object) cxFastListNode._elt != null)
          {
            cxFastListNode = cxFastListNode._next;
            if (cxFastListNode == this._head)
              goto label_8;
          }
          return cxFastListNode;
        }
label_8:
        return (MarchingSquares.CxFastListNode<T>) null;
      }

      public List<T> GetListOfElements()
      {
        List<T> listOfElements = new List<T>();
        MarchingSquares.CxFastListNode<T> cxFastListNode = this.begin();
        if (cxFastListNode != null)
        {
          do
          {
            listOfElements.Add(cxFastListNode._elt);
            cxFastListNode = cxFastListNode._next;
          }
          while (cxFastListNode != null);
        }
        return listOfElements;
      }
    }

    internal class GeomPolyVal
    {
      public MarchingSquares.GeomPoly p;
      public int key;

      public GeomPolyVal(MarchingSquares.GeomPoly P, int K)
      {
        this.p = P;
        this.key = K;
      }
    }

    internal class GeomPoly
    {
      public MarchingSquares.CxFastList<Vector2> points;
      public int length;

      public GeomPoly()
      {
        this.points = new MarchingSquares.CxFastList<Vector2>();
        this.length = 0;
      }
    }
  }
}
