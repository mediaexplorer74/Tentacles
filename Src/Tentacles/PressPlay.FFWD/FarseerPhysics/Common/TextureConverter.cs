// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.TextureConverter
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common
{
  public sealed class TextureConverter
  {
    private const int _CLOSEPIXELS_LENGTH = 8;
    private static int[,] ClosePixels = new int[8, 2]
    {
      {
        -1,
        -1
      },
      {
        0,
        -1
      },
      {
        1,
        -1
      },
      {
        1,
        0
      },
      {
        1,
        1
      },
      {
        0,
        1
      },
      {
        -1,
        1
      },
      {
        -1,
        0
      }
    };
    private uint[] _data;
    private int _dataLength;
    private int _width;
    private int _height;
    private VerticesDetectionType _polygonDetectionType;
    private uint _alphaTolerance;
    private float _hullTolerance;
    private bool _holeDetection;
    private bool _multipartDetection;
    private bool _pixelOffsetOptimization;
    private Matrix _transform = Matrix.Identity;
    private int _tempIsSolidX;
    private int _tempIsSolidY;

    public VerticesDetectionType PolygonDetectionType
    {
      get => this._polygonDetectionType;
      set => this._polygonDetectionType = value;
    }

    public bool HoleDetection
    {
      get => this._holeDetection;
      set => this._holeDetection = value;
    }

    public bool MultipartDetection
    {
      get => this._multipartDetection;
      set => this._multipartDetection = value;
    }

    public bool PixelOffsetOptimization
    {
      get => this._pixelOffsetOptimization;
      set => this._pixelOffsetOptimization = value;
    }

    public Matrix Transform
    {
      get => this._transform;
      set => this._transform = value;
    }

    public byte AlphaTolerance
    {
      get => (byte) (this._alphaTolerance >> 24);
      set => this._alphaTolerance = (uint) value << 24;
    }

    public float HullTolerance
    {
      get => this._hullTolerance;
      set
      {
        if ((double) value > 4.0)
          this._hullTolerance = 4f;
        else if ((double) value < 0.89999997615814209)
          this._hullTolerance = 0.9f;
        else
          this._hullTolerance = value;
      }
    }

    public TextureConverter()
    {
      this.Initialize((uint[]) null, new int?(), new byte?(), new float?(), new bool?(), new bool?(), new bool?(), new Matrix?());
    }

    public TextureConverter(
      byte? alphaTolerance,
      float? hullTolerance,
      bool? holeDetection,
      bool? multipartDetection,
      bool? pixelOffsetOptimization,
      Matrix? transform)
    {
      this.Initialize((uint[]) null, new int?(), alphaTolerance, hullTolerance, holeDetection, multipartDetection, pixelOffsetOptimization, transform);
    }

    public TextureConverter(uint[] data, int width)
    {
      this.Initialize(data, new int?(width), new byte?(), new float?(), new bool?(), new bool?(), new bool?(), new Matrix?());
    }

    public TextureConverter(
      uint[] data,
      int width,
      byte? alphaTolerance,
      float? hullTolerance,
      bool? holeDetection,
      bool? multipartDetection,
      bool? pixelOffsetOptimization,
      Matrix? transform)
    {
      this.Initialize(data, new int?(width), alphaTolerance, hullTolerance, holeDetection, multipartDetection, pixelOffsetOptimization, transform);
    }

    private void Initialize(
      uint[] data,
      int? width,
      byte? alphaTolerance,
      float? hullTolerance,
      bool? holeDetection,
      bool? multipartDetection,
      bool? pixelOffsetOptimization,
      Matrix? transform)
    {
      if (data != null && !width.HasValue)
        throw new ArgumentNullException(nameof (width), "'width' can't be null if 'data' is set.");
      if (data == null && width.HasValue)
        throw new ArgumentNullException(nameof (data), "'data' can't be null if 'width' is set.");
      if (data != null && width.HasValue)
        this.SetTextureData(data, width.Value);
      this.AlphaTolerance = !alphaTolerance.HasValue ? (byte) 20 : alphaTolerance.Value;
      this.HullTolerance = !hullTolerance.HasValue ? 1.5f : hullTolerance.Value;
      this.HoleDetection = holeDetection.HasValue && holeDetection.Value;
      this.MultipartDetection = multipartDetection.HasValue && multipartDetection.Value;
      this.PixelOffsetOptimization = pixelOffsetOptimization.HasValue && pixelOffsetOptimization.Value;
      if (transform.HasValue)
        this.Transform = transform.Value;
      else
        this.Transform = Matrix.Identity;
    }

    private void SetTextureData(uint[] data, int width)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data), "'data' can't be null.");
      if (data.Length < 4)
        throw new ArgumentOutOfRangeException(nameof (data), "'data' length can't be less then 4. Your texture must be at least 2 x 2 pixels in size.");
      if (width < 2)
        throw new ArgumentOutOfRangeException(nameof (width), "'width' can't be less then 2. Your texture must be at least 2 x 2 pixels in size.");
      if (data.Length % width != 0)
        throw new ArgumentException("'width' has an invalid value.");
      this._data = data;
      this._dataLength = this._data.Length;
      this._width = width;
      this._height = this._dataLength / width;
    }

    public static Vertices DetectVertices(uint[] data, int width)
    {
      return (Vertices) new TextureConverter(data, width).DetectVertices()[0];
    }

    public static Vertices DetectVertices(uint[] data, int width, bool holeDetection)
    {
      return (Vertices) new TextureConverter(data, width)
      {
        HoleDetection = holeDetection
      }.DetectVertices()[0];
    }

    public static List<Vertices> DetectVertices(
      uint[] data,
      int width,
      float hullTolerance,
      byte alphaTolerance,
      bool multiPartDetection,
      bool holeDetection)
    {
      List<DetectedVertices> detectedVerticesList = new TextureConverter(data, width)
      {
        HullTolerance = hullTolerance,
        AlphaTolerance = alphaTolerance,
        MultipartDetection = multiPartDetection,
        HoleDetection = holeDetection
      }.DetectVertices();
      List<Vertices> verticesList = new List<Vertices>();
      for (int index = 0; index < detectedVerticesList.Count; ++index)
        verticesList.Add((Vertices) detectedVerticesList[index]);
      return verticesList;
    }

    public List<DetectedVertices> DetectVertices()
    {
      if (this._data == null)
        throw new Exception("'_data' can't be null. You have to use SetTextureData(uint[] data, int width) before calling this method.");
      if (this._data.Length < 4)
        throw new Exception("'_data' length can't be less then 4. Your texture must be at least 2 x 2 pixels in size. You have to use SetTextureData(uint[] data, int width) before calling this method.");
      if (this._width < 2)
        throw new Exception("'_width' can't be less then 2. Your texture must be at least 2 x 2 pixels in size. You have to use SetTextureData(uint[] data, int width) before calling this method.");
      if (this._data.Length % this._width != 0)
        throw new Exception("'_width' has an invalid value. You have to use SetTextureData(uint[] data, int width) before calling this method.");
      List<DetectedVertices> detectedPolygons = new List<DetectedVertices>();
      Vector2? lastHoleEntrance = new Vector2?();
      Vector2? entrance = new Vector2?();
      List<Vector2> vector2List = new List<Vector2>();
      bool flag;
      do
      {
        DetectedVertices polygon;
        if (detectedPolygons.Count == 0)
        {
          polygon = new DetectedVertices(this.CreateSimplePolygon(Vector2.Zero, Vector2.Zero));
          if (polygon.Count > 2)
            entrance = this.GetTopMostVertex((Vertices) polygon);
        }
        else if (entrance.HasValue)
          polygon = new DetectedVertices(this.CreateSimplePolygon(entrance.Value, new Vector2(entrance.Value.X - 1f, entrance.Value.Y)));
        else
          break;
        flag = false;
        if (polygon.Count > 2)
        {
          if (this._holeDetection)
          {
            while (true)
            {
              Vertices simplePolygon;
              int vertex2Index;
              do
              {
                do
                {
                  lastHoleEntrance = this.SearchHoleEntrance((Vertices) polygon, lastHoleEntrance);
                  if (lastHoleEntrance.HasValue && !vector2List.Contains(lastHoleEntrance.Value))
                  {
                    vector2List.Add(lastHoleEntrance.Value);
                    simplePolygon = this.CreateSimplePolygon(lastHoleEntrance.Value, new Vector2(lastHoleEntrance.Value.X + 1f, lastHoleEntrance.Value.Y));
                  }
                  else
                    goto label_24;
                }
                while (simplePolygon == null || simplePolygon.Count <= 2);
                switch (this._polygonDetectionType)
                {
                  case VerticesDetectionType.Integrated:
                    simplePolygon.Add(simplePolygon[0]);
                    continue;
                  case VerticesDetectionType.Separated:
                    goto label_21;
                  default:
                    continue;
                }
              }
              while (!this.SplitPolygonEdge((Vertices) polygon, lastHoleEntrance.Value, out int _, out vertex2Index));
              polygon.InsertRange(vertex2Index, (IEnumerable<Vector2>) simplePolygon);
              continue;
label_21:
              if (polygon.Holes == null)
                polygon.Holes = new List<Vertices>();
              polygon.Holes.Add(simplePolygon);
            }
          }
label_24:
          detectedPolygons.Add(polygon);
        }
        if ((this._multipartDetection || polygon.Count <= 2) && this.SearchNextHullEntrance(detectedPolygons, entrance.Value, out entrance))
          flag = true;
      }
      while (flag);
      if (detectedPolygons == null || detectedPolygons != null && detectedPolygons.Count == 0)
        throw new Exception("Couldn't detect any vertices.");
      if (this.PolygonDetectionType == VerticesDetectionType.Separated)
        this.ApplyTriangulationCompatibleWinding(ref detectedPolygons);
      if (this._pixelOffsetOptimization)
        this.ApplyPixelOffsetOptimization(ref detectedPolygons);
      if (this._transform != Matrix.Identity)
        this.ApplyTransform(ref detectedPolygons);
      return detectedPolygons;
    }

    private void ApplyTriangulationCompatibleWinding(ref List<DetectedVertices> detectedPolygons)
    {
      for (int index1 = 0; index1 < detectedPolygons.Count; ++index1)
      {
        detectedPolygons[index1].Reverse();
        if (detectedPolygons[index1].Holes != null && detectedPolygons[index1].Holes.Count > 0)
        {
          for (int index2 = 0; index2 < detectedPolygons[index1].Holes.Count; ++index2)
            detectedPolygons[index1].Holes[index2].Reverse();
        }
      }
    }

    private void ApplyPixelOffsetOptimization(ref List<DetectedVertices> detectedPolygons)
    {
    }

    private void ApplyTransform(ref List<DetectedVertices> detectedPolygons)
    {
      for (int index = 0; index < detectedPolygons.Count; ++index)
        detectedPolygons[index].Transform(this._transform);
    }

    public bool IsSolid(ref Vector2 v)
    {
      this._tempIsSolidX = (int) v.X;
      this._tempIsSolidY = (int) v.Y;
      return this._tempIsSolidX >= 0 && this._tempIsSolidX < this._width && this._tempIsSolidY >= 0 && this._tempIsSolidY < this._height && this._data[this._tempIsSolidX + this._tempIsSolidY * this._width] >= this._alphaTolerance;
    }

    public bool IsSolid(ref int x, ref int y)
    {
      return x >= 0 && x < this._width && y >= 0 && y < this._height && this._data[x + y * this._width] >= this._alphaTolerance;
    }

    public bool IsSolid(ref int index)
    {
      return index >= 0 && index < this._dataLength && this._data[index] >= this._alphaTolerance;
    }

    public bool InBounds(ref Vector2 coord)
    {
      return (double) coord.X >= 0.0 && (double) coord.X < (double) this._width && (double) coord.Y >= 0.0 && (double) coord.Y < (double) this._height;
    }

    private Vector2? SearchHoleEntrance(Vertices polygon, Vector2? lastHoleEntrance)
    {
      if (polygon == null)
        throw new ArgumentNullException("'polygon' can't be null.");
      if (polygon.Count < 3)
        throw new ArgumentException("'polygon.MainPolygon.Count' can't be less then 3.");
      int x1 = 0;
      int num1 = !lastHoleEntrance.HasValue ? (int) this.GetTopMostCoord(polygon) : (int) lastHoleEntrance.Value.Y;
      int bottomMostCoord = (int) this.GetBottomMostCoord(polygon);
      if (num1 > 0 && num1 < this._height && bottomMostCoord > 0 && bottomMostCoord < this._height)
      {
        for (int y = num1; y <= bottomMostCoord; ++y)
        {
          List<float> floatList = this.SearchCrossingEdges(polygon, y);
          if (floatList.Count > 1 && floatList.Count % 2 == 0)
          {
            for (int index = 0; index < floatList.Count; index += 2)
            {
              bool flag1 = false;
              bool flag2 = false;
              for (int x2 = (int) floatList[index]; x2 <= (int) floatList[index + 1]; ++x2)
              {
                if (this.IsSolid(ref x2, ref y))
                {
                  if (!flag2)
                  {
                    flag1 = true;
                    x1 = x2;
                  }
                  if (flag1 && flag2)
                  {
                    Vector2? nullable = new Vector2?(new Vector2((float) x1, (float) y));
                    if (this.DistanceToHullAcceptable(polygon, nullable.Value, true))
                      return nullable;
                    nullable = new Vector2?();
                    break;
                  }
                }
                else if (flag1)
                  flag2 = true;
              }
            }
          }
          else
          {
            int num2 = floatList.Count % 2;
          }
        }
      }
      return new Vector2?();
    }

    private bool DistanceToHullAcceptable(
      DetectedVertices polygon,
      Vector2 point,
      bool higherDetail)
    {
      if (polygon == null)
        throw new ArgumentNullException(nameof (polygon), "'polygon' can't be null.");
      if (polygon.Count < 3)
        throw new ArgumentException("'polygon.MainPolygon.Count' can't be less then 3.");
      if (!this.DistanceToHullAcceptable((Vertices) polygon, point, higherDetail))
        return false;
      if (polygon.Holes != null)
      {
        for (int index = 0; index < polygon.Holes.Count; ++index)
        {
          if (!this.DistanceToHullAcceptable(polygon.Holes[index], point, higherDetail))
            return false;
        }
      }
      return true;
    }

    private bool DistanceToHullAcceptable(Vertices polygon, Vector2 point, bool higherDetail)
    {
      if (polygon == null)
        throw new ArgumentNullException(nameof (polygon), "'polygon' can't be null.");
      Vector2 lineEndPoint2 = polygon.Count >= 3 ? polygon[polygon.Count - 1] : throw new ArgumentException("'polygon.Count' can't be less then 3.");
      if (higherDetail)
      {
        for (int index = 0; index < polygon.Count; ++index)
        {
          Vector2 vector2 = polygon[index];
          if ((double) LineTools.DistanceBetweenPointAndLineSegment(ref point, ref vector2, ref lineEndPoint2) <= (double) this._hullTolerance || (double) LineTools.DistanceBetweenPointAndPoint(ref point, ref vector2) <= (double) this._hullTolerance)
            return false;
          lineEndPoint2 = polygon[index];
        }
        return true;
      }
      for (int index = 0; index < polygon.Count; ++index)
      {
        Vector2 lineEndPoint1 = polygon[index];
        if ((double) LineTools.DistanceBetweenPointAndLineSegment(ref point, ref lineEndPoint1, ref lineEndPoint2) <= (double) this._hullTolerance)
          return false;
        lineEndPoint2 = polygon[index];
      }
      return true;
    }

    private bool InPolygon(DetectedVertices polygon, Vector2 point)
    {
      if (!this.DistanceToHullAcceptable(polygon, point, true))
        return true;
      List<float> floatList = this.SearchCrossingEdges(polygon, (int) point.Y);
      if (floatList.Count > 0 && floatList.Count % 2 == 0)
      {
        for (int index = 0; index < floatList.Count; index += 2)
        {
          if ((double) floatList[index] <= (double) point.X && (double) floatList[index + 1] >= (double) point.X)
            return true;
        }
      }
      return false;
    }

    private Vector2? GetTopMostVertex(Vertices vertices)
    {
      float num = float.MaxValue;
      Vector2? topMostVertex = new Vector2?();
      for (int index = 0; index < vertices.Count; ++index)
      {
        if ((double) num > (double) vertices[index].Y)
        {
          num = vertices[index].Y;
          topMostVertex = new Vector2?(vertices[index]);
        }
      }
      return topMostVertex;
    }

    private float GetTopMostCoord(Vertices vertices)
    {
      float topMostCoord = float.MaxValue;
      for (int index = 0; index < vertices.Count; ++index)
      {
        if ((double) topMostCoord > (double) vertices[index].Y)
          topMostCoord = vertices[index].Y;
      }
      return topMostCoord;
    }

    private float GetBottomMostCoord(Vertices vertices)
    {
      float bottomMostCoord = float.MinValue;
      for (int index = 0; index < vertices.Count; ++index)
      {
        if ((double) bottomMostCoord < (double) vertices[index].Y)
          bottomMostCoord = vertices[index].Y;
      }
      return bottomMostCoord;
    }

    private List<float> SearchCrossingEdges(DetectedVertices polygon, int y)
    {
      if (polygon == null)
        throw new ArgumentNullException(nameof (polygon), "'polygon' can't be null.");
      List<float> floatList = polygon.Count >= 3 ? this.SearchCrossingEdges((Vertices) polygon, y) : throw new ArgumentException("'polygon.MainPolygon.Count' can't be less then 3.");
      if (polygon.Holes != null)
      {
        for (int index = 0; index < polygon.Holes.Count; ++index)
          floatList.AddRange((IEnumerable<float>) this.SearchCrossingEdges(polygon.Holes[index], y));
      }
      floatList.Sort();
      return floatList;
    }

    private List<float> SearchCrossingEdges(Vertices polygon, int y)
    {
      List<float> floatList = new List<float>();
      if (polygon.Count > 2)
      {
        Vector2 vector2_1 = polygon[polygon.Count - 1];
        for (int index = 0; index < polygon.Count; ++index)
        {
          Vector2 vector2_2 = polygon[index];
          if (((double) vector2_2.Y >= (double) y && (double) vector2_1.Y <= (double) y || (double) vector2_2.Y <= (double) y && (double) vector2_1.Y >= (double) y) && (double) vector2_2.Y != (double) vector2_1.Y)
          {
            bool flag = true;
            Vector2 vector2_3 = vector2_1 - vector2_2;
            if ((double) vector2_2.Y == (double) y)
            {
              Vector2 vector2_4 = polygon[(index + 1) % polygon.Count];
              Vector2 vector2_5 = vector2_2 - vector2_4;
              flag = (double) vector2_3.Y <= 0.0 ? (double) vector2_5.Y >= 0.0 : (double) vector2_5.Y <= 0.0;
            }
            if (flag)
              floatList.Add(((float) y - vector2_2.Y) / vector2_3.Y * vector2_3.X + vector2_2.X);
          }
          vector2_1 = vector2_2;
        }
      }
      floatList.Sort();
      return floatList;
    }

    private bool SplitPolygonEdge(
      Vertices polygon,
      Vector2 coordInsideThePolygon,
      out int vertex1Index,
      out int vertex2Index)
    {
      int index1 = 0;
      int index2 = 0;
      bool flag1 = false;
      float num1 = float.MaxValue;
      bool flag2 = false;
      Vector2 zero = Vector2.Zero;
      List<float> floatList = this.SearchCrossingEdges(polygon, (int) coordInsideThePolygon.Y);
      vertex1Index = 0;
      vertex2Index = 0;
      zero.Y = coordInsideThePolygon.Y;
      if (floatList != null && floatList.Count > 1 && floatList.Count % 2 == 0)
      {
        for (int index3 = 0; index3 < floatList.Count; ++index3)
        {
          if ((double) floatList[index3] < (double) coordInsideThePolygon.X)
          {
            float num2 = coordInsideThePolygon.X - floatList[index3];
            if ((double) num2 < (double) num1)
            {
              num1 = num2;
              zero.X = floatList[index3];
              flag2 = true;
            }
          }
        }
        if (flag2)
        {
          float num3 = float.MaxValue;
          int index4 = polygon.Count - 1;
          for (int index5 = 0; index5 < polygon.Count; ++index5)
          {
            Vector2 lineEndPoint1 = polygon[index5];
            Vector2 lineEndPoint2 = polygon[index4];
            float num4 = LineTools.DistanceBetweenPointAndLineSegment(ref zero, ref lineEndPoint1, ref lineEndPoint2);
            if ((double) num4 < (double) num3)
            {
              num3 = num4;
              index1 = index5;
              index2 = index4;
              flag1 = true;
            }
            index4 = index5;
          }
          if (flag1)
          {
            Vector2 vector2 = polygon[index2] - polygon[index1];
            vector2.Normalize();
            Vector2 point1 = polygon[index1];
            float num5 = LineTools.DistanceBetweenPointAndPoint(ref point1, ref zero);
            vertex1Index = index1;
            vertex2Index = index1 + 1;
            polygon.Insert(index1, num5 * vector2 + polygon[vertex1Index]);
            polygon.Insert(index1, num5 * vector2 + polygon[vertex2Index]);
            return true;
          }
        }
      }
      return false;
    }

    private Vertices CreateSimplePolygon(Vector2 entrance, Vector2 last)
    {
      bool flag1 = false;
      bool flag2 = false;
      Vertices simplePolygon = new Vertices(32);
      Vertices vertices1 = new Vertices(32);
      Vertices vertices2 = new Vertices(32);
      Vector2 current = Vector2.Zero;
      if (entrance == Vector2.Zero || !this.InBounds(ref entrance))
      {
        flag1 = this.SearchHullEntrance(out entrance);
        if (flag1)
          current = new Vector2(entrance.X - 1f, entrance.Y);
      }
      else if (this.IsSolid(ref entrance))
      {
        if (this.IsNearPixel(ref entrance, ref last))
        {
          current = last;
          flag1 = true;
        }
        else
        {
          Vector2 foundPixel;
          if (this.SearchNearPixels(false, ref entrance, out foundPixel))
          {
            current = foundPixel;
            flag1 = true;
          }
          else
            flag1 = false;
        }
      }
      if (flag1)
      {
        simplePolygon.Add(entrance);
        vertices1.Add(entrance);
        Vector2 next = entrance;
        while (true)
        {
          do
          {
            do
            {
              Vector2 outstanding;
              if (this.SearchForOutstandingVertex(vertices1, out outstanding))
              {
                if (flag2)
                {
                  if (vertices2.Contains(outstanding))
                  {
                    simplePolygon.Add(outstanding);
                    goto label_20;
                  }
                  else
                    goto label_20;
                }
                else
                {
                  simplePolygon.Add(outstanding);
                  vertices1.RemoveRange(0, vertices1.IndexOf(outstanding));
                }
              }
              last = current;
              current = next;
              if (this.GetNextHullPoint(ref last, ref current, out next))
                vertices1.Add(next);
              else
                goto label_20;
            }
            while (!(next == entrance) || flag2);
            flag2 = true;
            vertices2.AddRange((IEnumerable<Vector2>) vertices1);
          }
          while (!vertices2.Contains(entrance));
          vertices2.Remove(entrance);
        }
      }
label_20:
      return simplePolygon;
    }

    private bool SearchNearPixels(
      bool searchingForSolidPixel,
      ref Vector2 current,
      out Vector2 foundPixel)
    {
      for (int index = 0; index < 8; ++index)
      {
        int x = (int) current.X + TextureConverter.ClosePixels[index, 0];
        int y = (int) current.Y + TextureConverter.ClosePixels[index, 1];
        if (!searchingForSolidPixel ^ this.IsSolid(ref x, ref y))
        {
          foundPixel = new Vector2((float) x, (float) y);
          return true;
        }
      }
      foundPixel = Vector2.Zero;
      return false;
    }

    private bool IsNearPixel(ref Vector2 current, ref Vector2 near)
    {
      for (int index = 0; index < 8; ++index)
      {
        int num1 = (int) current.X + TextureConverter.ClosePixels[index, 0];
        int num2 = (int) current.Y + TextureConverter.ClosePixels[index, 1];
        if (num1 >= 0 && num1 <= this._width && num2 >= 0 && num2 <= this._height && num1 == (int) near.X && num2 == (int) near.Y)
          return true;
      }
      return false;
    }

    private bool SearchHullEntrance(out Vector2 entrance)
    {
      for (int y = 0; y <= this._height; ++y)
      {
        for (int x = 0; x <= this._width; ++x)
        {
          if (this.IsSolid(ref x, ref y))
          {
            entrance = new Vector2((float) x, (float) y);
            return true;
          }
        }
      }
      entrance = Vector2.Zero;
      return false;
    }

    private bool SearchNextHullEntrance(
      List<DetectedVertices> detectedPolygons,
      Vector2 start,
      out Vector2? entrance)
    {
      bool flag1 = false;
      for (int index1 = (int) start.X + (int) start.Y * this._width; index1 <= this._dataLength; ++index1)
      {
        if (this.IsSolid(ref index1))
        {
          if (flag1)
          {
            int x = index1 % this._width;
            entrance = new Vector2?(new Vector2((float) x, (float) (index1 - x) / (float) this._width));
            bool flag2 = false;
            for (int index2 = 0; index2 < detectedPolygons.Count; ++index2)
            {
              if (this.InPolygon(detectedPolygons[index2], entrance.Value))
              {
                flag2 = true;
                break;
              }
            }
            if (!flag2)
              return true;
            flag1 = false;
          }
        }
        else
          flag1 = true;
      }
      entrance = new Vector2?();
      return false;
    }

    private bool GetNextHullPoint(ref Vector2 last, ref Vector2 current, out Vector2 next)
    {
      int firstPixelToCheck = this.GetIndexOfFirstPixelToCheck(ref last, ref current);
      for (int index1 = 0; index1 < 8; ++index1)
      {
        int index2 = (firstPixelToCheck + index1) % 8;
        int x = (int) current.X + TextureConverter.ClosePixels[index2, 0];
        int y = (int) current.Y + TextureConverter.ClosePixels[index2, 1];
        if (x >= 0 && x < this._width && y >= 0 && y <= this._height && this.IsSolid(ref x, ref y))
        {
          next = new Vector2((float) x, (float) y);
          return true;
        }
      }
      next = Vector2.Zero;
      return false;
    }

    private bool SearchForOutstandingVertex(Vertices hullArea, out Vector2 outstanding)
    {
      Vector2 zero = Vector2.Zero;
      bool flag = false;
      if (hullArea.Count > 2)
      {
        int index1 = hullArea.Count - 1;
        Vector2 lineEndPoint1 = hullArea[0];
        Vector2 lineEndPoint2 = hullArea[index1];
        for (int index2 = 1; index2 < index1; ++index2)
        {
          Vector2 point = hullArea[index2];
          if ((double) LineTools.DistanceBetweenPointAndLineSegment(ref point, ref lineEndPoint1, ref lineEndPoint2) >= (double) this._hullTolerance)
          {
            zero = hullArea[index2];
            flag = true;
            break;
          }
        }
      }
      outstanding = zero;
      return flag;
    }

    private int GetIndexOfFirstPixelToCheck(ref Vector2 last, ref Vector2 current)
    {
      switch ((double) current.X - (double) last.X)
      {
        case -1.0:
          switch ((double) current.Y - (double) last.Y)
          {
            case -1.0:
              return 5;
            case 0.0:
              return 4;
            case 1.0:
              return 3;
          }
          break;
        case 0.0:
          switch ((double) current.Y - (double) last.Y)
          {
            case -1.0:
              return 6;
            case 1.0:
              return 2;
          }
          break;
        case 1.0:
          switch ((double) current.Y - (double) last.Y)
          {
            case -1.0:
              return 7;
            case 0.0:
              return 0;
            case 1.0:
              return 1;
          }
          break;
      }
      return 0;
    }
  }
}
