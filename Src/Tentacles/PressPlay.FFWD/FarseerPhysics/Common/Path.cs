// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Path
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

#nullable disable
namespace FarseerPhysics.Common
{
  [XmlRoot("Path")]
  public class Path
  {
    [XmlElement("ControlPoints")]
    public List<Vector2> ControlPoints;
    private float _deltaT;

    public Path() => this.ControlPoints = new List<Vector2>();

    public Path(Vector2[] vertices)
    {
      this.ControlPoints = new List<Vector2>(vertices.Length);
      for (int index = 0; index < vertices.Length; ++index)
        this.Add(vertices[index]);
    }

    public Path(IList<Vector2> vertices)
    {
      this.ControlPoints = new List<Vector2>(vertices.Count);
      for (int index = 0; index < vertices.Count; ++index)
        this.Add(vertices[index]);
    }

    [XmlElement("Closed")]
    public bool Closed { get; set; }

    public int NextIndex(int index) => index == this.ControlPoints.Count - 1 ? 0 : index + 1;

    public int PreviousIndex(int index) => index == 0 ? this.ControlPoints.Count - 1 : index - 1;

    public void Translate(ref Vector2 vector)
    {
      for (int index = 0; index < this.ControlPoints.Count; ++index)
        this.ControlPoints[index] = Vector2.Add(this.ControlPoints[index], vector);
    }

    public void Scale(ref Vector2 value)
    {
      for (int index = 0; index < this.ControlPoints.Count; ++index)
        this.ControlPoints[index] = Vector2.Multiply(this.ControlPoints[index], value);
    }

    public void Rotate(float value)
    {
      Matrix result;
      Matrix.CreateRotationZ(value, out result);
      for (int index = 0; index < this.ControlPoints.Count; ++index)
        this.ControlPoints[index] = Vector2.Transform(this.ControlPoints[index], result);
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < this.ControlPoints.Count; ++index)
      {
        stringBuilder.Append(this.ControlPoints[index].ToString());
        if (index < this.ControlPoints.Count - 1)
          stringBuilder.Append(" ");
      }
      return stringBuilder.ToString();
    }

    public Vertices GetVertices(int divisions)
    {
      Vertices vertices = new Vertices();
      float num = 1f / (float) divisions;
      for (float time = 0.0f; (double) time < 1.0; time += num)
        vertices.Add(this.GetPosition(time));
      return vertices;
    }

    public Vector2 GetPosition(float time)
    {
      if (this.ControlPoints.Count < 2)
        throw new Exception("You need at least 2 control points to calculate a position.");
      Vector2 position;
      if (this.Closed)
      {
        this.Add(this.ControlPoints[0]);
        this._deltaT = 1f / (float) (this.ControlPoints.Count - 1);
        int num = (int) ((double) time / (double) this._deltaT);
        int index1 = num - 1;
        if (index1 < 0)
          index1 += this.ControlPoints.Count - 1;
        else if (index1 >= this.ControlPoints.Count - 1)
          index1 -= this.ControlPoints.Count - 1;
        int index2 = num;
        if (index2 < 0)
          index2 += this.ControlPoints.Count - 1;
        else if (index2 >= this.ControlPoints.Count - 1)
          index2 -= this.ControlPoints.Count - 1;
        int index3 = num + 1;
        if (index3 < 0)
          index3 += this.ControlPoints.Count - 1;
        else if (index3 >= this.ControlPoints.Count - 1)
          index3 -= this.ControlPoints.Count - 1;
        int index4 = num + 2;
        if (index4 < 0)
          index4 += this.ControlPoints.Count - 1;
        else if (index4 >= this.ControlPoints.Count - 1)
          index4 -= this.ControlPoints.Count - 1;
        float amount = (time - this._deltaT * (float) num) / this._deltaT;
        position = Vector2.CatmullRom(this.ControlPoints[index1], this.ControlPoints[index2], this.ControlPoints[index3], this.ControlPoints[index4], amount);
        this.RemoveAt(this.ControlPoints.Count - 1);
      }
      else
      {
        int num = (int) ((double) time / (double) this._deltaT);
        int index5 = num - 1;
        if (index5 < 0)
          index5 = 0;
        else if (index5 >= this.ControlPoints.Count - 1)
          index5 = this.ControlPoints.Count - 1;
        int index6 = num;
        if (index6 < 0)
          index6 = 0;
        else if (index6 >= this.ControlPoints.Count - 1)
          index6 = this.ControlPoints.Count - 1;
        int index7 = num + 1;
        if (index7 < 0)
          index7 = 0;
        else if (index7 >= this.ControlPoints.Count - 1)
          index7 = this.ControlPoints.Count - 1;
        int index8 = num + 2;
        if (index8 < 0)
          index8 = 0;
        else if (index8 >= this.ControlPoints.Count - 1)
          index8 = this.ControlPoints.Count - 1;
        float amount = (time - this._deltaT * (float) num) / this._deltaT;
        position = Vector2.CatmullRom(this.ControlPoints[index5], this.ControlPoints[index6], this.ControlPoints[index7], this.ControlPoints[index8], amount);
      }
      return position;
    }

    public Vector2 GetPositionNormal(float time)
    {
      float time1 = time + 0.0001f;
      Vector2 position1 = this.GetPosition(time);
      Vector2 position2 = this.GetPosition(time1);
      Vector2 result1;
      Vector2.Subtract(ref position1, ref position2, out result1);
      Vector2 result2 = new Vector2();
      result2.X = -result1.Y;
      result2.Y = result1.X;
      Vector2.Normalize(ref result2, out result2);
      return result2;
    }

    public void Add(Vector2 point)
    {
      this.ControlPoints.Add(point);
      this._deltaT = 1f / (float) (this.ControlPoints.Count - 1);
    }

    public void Remove(Vector2 point)
    {
      this.ControlPoints.Remove(point);
      this._deltaT = 1f / (float) (this.ControlPoints.Count - 1);
    }

    public void RemoveAt(int index)
    {
      this.ControlPoints.RemoveAt(index);
      this._deltaT = 1f / (float) (this.ControlPoints.Count - 1);
    }

    public float GetLength()
    {
      List<Vector2> vertices = (List<Vector2>) this.GetVertices(this.ControlPoints.Count * 25);
      float length = 0.0f;
      for (int index = 1; index < vertices.Count; ++index)
        length += Vector2.Distance(vertices[index - 1], vertices[index]);
      if (this.Closed)
        length += Vector2.Distance(vertices[this.ControlPoints.Count - 1], vertices[0]);
      return length;
    }

    public List<Vector3> SubdivideEvenly(int divisions)
    {
      List<Vector3> vector3List = new List<Vector3>();
      float num = (float) ((double) this.GetLength() / (double) divisions + 1.0 / 1000.0);
      float time = 0.0f;
      Vector2 controlPoint = this.ControlPoints[0];
      Vector2 position = this.GetPosition(time);
      while ((double) num * 0.5 >= (double) Vector2.Distance(controlPoint, position))
      {
        position = this.GetPosition(time);
        time += 0.0001f;
        if ((double) time >= 1.0)
          break;
      }
      Vector2 vector2 = position;
      for (int index = 1; index < divisions; ++index)
      {
        Vector2 positionNormal = this.GetPositionNormal(time);
        float z = (float) Math.Atan2((double) positionNormal.Y, (double) positionNormal.X);
        vector3List.Add(new Vector3(position, z));
        while ((double) num >= (double) Vector2.Distance(vector2, position))
        {
          position = this.GetPosition(time);
          time += 1E-05f;
          if ((double) time >= 1.0)
            break;
        }
        if ((double) time < 1.0)
          vector2 = position;
        else
          break;
      }
      return vector3List;
    }
  }
}
