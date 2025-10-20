// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.MSTerrain
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common
{
  public class MSTerrain
  {
    public World World;
    public Vector2 Center;
    public float Width;
    public float Height;
    public int PointsPerUnit;
    public int CellSize;
    public int SubCellSize;
    public int Iterations = 2;
    public Decomposer Decomposer;
    public sbyte[,] TerrainMap;
    public List<Body>[,] BodyMap;
    private float localWidth;
    private float localHeight;
    private int xnum;
    private int ynum;
    private AABB dirtyArea;
    private Vector2 topLeft;

    public MSTerrain(World world, AABB area)
    {
      this.World = world;
      this.Width = area.Extents.X * 2f;
      this.Height = area.Extents.Y * 2f;
      this.Center = area.Center;
    }

    public void Initialize()
    {
      this.topLeft = new Vector2(this.Center.X - this.Width * 0.5f, this.Center.Y - (float) (-(double) this.Height * 0.5));
      this.localWidth = this.Width * (float) this.PointsPerUnit;
      this.localHeight = this.Height * (float) this.PointsPerUnit;
      this.TerrainMap = new sbyte[(int) this.localWidth + 1, (int) this.localHeight + 1];
      for (int index1 = 0; (double) index1 < (double) this.localWidth; ++index1)
      {
        for (int index2 = 0; (double) index2 < (double) this.localHeight; ++index2)
          this.TerrainMap[index1, index2] = (sbyte) 1;
      }
      this.xnum = (int) ((double) this.localWidth / (double) this.CellSize);
      this.ynum = (int) ((double) this.localHeight / (double) this.CellSize);
      this.BodyMap = new List<Body>[this.xnum, this.ynum];
      this.dirtyArea = new AABB(new Vector2(float.MaxValue, float.MaxValue), new Vector2(float.MinValue, float.MinValue));
    }

    public void ApplyTexture(Texture2D texture, Vector2 position, TerrainTester tester)
    {
      Color[] data = new Color[texture.Width * texture.Height];
      texture.GetData<Color>(data);
      for (int y = (int) position.Y; y < texture.Height + (int) position.Y; ++y)
      {
        for (int x = (int) position.X; x < texture.Width + (int) position.X; ++x)
        {
          if (x >= 0 && (double) x < (double) this.localWidth && y >= 0 && (double) y < (double) this.localHeight)
            this.TerrainMap[x, y] = tester(data[(y - (int) position.Y) * texture.Width + (x - (int) position.X)]) ? (sbyte) -1 : (sbyte) 1;
        }
      }
      for (int gy = 0; gy < this.ynum; ++gy)
      {
        for (int gx = 0; gx < this.xnum; ++gx)
        {
          if (this.BodyMap[gx, gy] != null)
          {
            for (int index = 0; index < this.BodyMap[gx, gy].Count; ++index)
              this.World.RemoveBody(this.BodyMap[gx, gy][index]);
          }
          this.BodyMap[gx, gy] = (List<Body>) null;
          this.GenerateTerrain(gx, gy);
        }
      }
    }

    public void ApplyData(sbyte[,] data, Vector2 position)
    {
      for (int y = (int) position.Y; y < data.GetUpperBound(1) + (int) position.Y; ++y)
      {
        for (int x = (int) position.X; x < data.GetUpperBound(0) + (int) position.X; ++x)
        {
          if (x >= 0 && (double) x < (double) this.localWidth && y >= 0 && (double) y < (double) this.localHeight)
            this.TerrainMap[x, y] = data[x, y];
        }
      }
      for (int gy = 0; gy < this.ynum; ++gy)
      {
        for (int gx = 0; gx < this.xnum; ++gx)
        {
          if (this.BodyMap[gx, gy] != null)
          {
            for (int index = 0; index < this.BodyMap[gx, gy].Count; ++index)
              this.World.RemoveBody(this.BodyMap[gx, gy][index]);
          }
          this.BodyMap[gx, gy] = (List<Body>) null;
          this.GenerateTerrain(gx, gy);
        }
      }
    }

    public static sbyte[,] ConvertTextureToData(Texture2D texture, TerrainTester tester)
    {
      sbyte[,] data1 = new sbyte[texture.Width, texture.Height];
      Color[] data2 = new Color[texture.Width * texture.Height];
      texture.GetData<Color>(data2);
      for (int index1 = 0; index1 < texture.Height; ++index1)
      {
        for (int index2 = 0; index2 < texture.Width; ++index2)
          data1[index2, index1] = tester(data2[index1 * texture.Width + index2]) ? (sbyte) -1 : (sbyte) 1;
      }
      return data1;
    }

    public void ModifyTerrain(Vector2 location, sbyte value)
    {
      Vector2 vector2 = location - this.topLeft;
      vector2.X = vector2.X * this.localWidth / this.Width;
      vector2.Y = vector2.Y * -this.localHeight / this.Height;
      if ((double) vector2.X < 0.0 || (double) vector2.X >= (double) this.localWidth || (double) vector2.Y < 0.0 || (double) vector2.Y >= (double) this.localHeight)
        return;
      this.TerrainMap[(int) vector2.X, (int) vector2.Y] = value;
      if ((double) vector2.X < (double) this.dirtyArea.LowerBound.X)
        this.dirtyArea.LowerBound.X = vector2.X;
      if ((double) vector2.X > (double) this.dirtyArea.UpperBound.X)
        this.dirtyArea.UpperBound.X = vector2.X;
      if ((double) vector2.Y < (double) this.dirtyArea.LowerBound.Y)
        this.dirtyArea.LowerBound.Y = vector2.Y;
      if ((double) vector2.Y <= (double) this.dirtyArea.UpperBound.Y)
        return;
      this.dirtyArea.UpperBound.Y = vector2.Y;
    }

    public void RegenerateTerrain()
    {
      int num1 = (int) ((double) this.dirtyArea.LowerBound.X / (double) this.CellSize);
      int num2 = (int) ((double) this.dirtyArea.UpperBound.X / (double) this.CellSize) + 1;
      if (num1 < 0)
        num1 = 0;
      if (num2 > this.xnum)
        num2 = this.xnum;
      int num3 = (int) ((double) this.dirtyArea.LowerBound.Y / (double) this.CellSize);
      int num4 = (int) ((double) this.dirtyArea.UpperBound.Y / (double) this.CellSize) + 1;
      if (num3 < 0)
        num3 = 0;
      if (num4 > this.ynum)
        num4 = this.ynum;
      for (int gx = num1; gx < num2; ++gx)
      {
        for (int gy = num3; gy < num4; ++gy)
        {
          if (this.BodyMap[gx, gy] != null)
          {
            for (int index = 0; index < this.BodyMap[gx, gy].Count; ++index)
              this.World.RemoveBody(this.BodyMap[gx, gy][index]);
          }
          this.BodyMap[gx, gy] = (List<Body>) null;
          this.GenerateTerrain(gx, gy);
        }
      }
      this.dirtyArea = new AABB(new Vector2(float.MaxValue, float.MaxValue), new Vector2(float.MinValue, float.MinValue));
    }

    private void GenerateTerrain(int gx, int gy)
    {
      float x = (float) (gx * this.CellSize);
      float y = (float) (gy * this.CellSize);
      List<Vertices> verticesList1 = MarchingSquares.DetectSquares(new AABB(new Vector2(x, y), new Vector2(x + (float) this.CellSize, y + (float) this.CellSize)), (float) this.SubCellSize, (float) this.SubCellSize, this.TerrainMap, this.Iterations, true);
      if (verticesList1.Count == 0)
        return;
      this.BodyMap[gx, gy] = new List<Body>();
      Vector2 vector2 = new Vector2(1f / (float) this.PointsPerUnit, 1f / (float) -this.PointsPerUnit);
      foreach (Vertices vertices1 in verticesList1)
      {
        vertices1.Scale(ref vector2);
        vertices1.Translate(ref this.topLeft);
        vertices1.ForceCounterClockWise();
        Vertices vertices2 = SimplifyTools.CollinearSimplify(vertices1);
        List<Vertices> verticesList2 = new List<Vertices>();
        switch (this.Decomposer)
        {
          case Decomposer.Bayazit:
            verticesList2 = BayazitDecomposer.ConvexPartition(vertices2);
            break;
          case Decomposer.CDT:
            verticesList2 = CDTDecomposer.ConvexPartition(vertices2);
            break;
          case Decomposer.Earclip:
            verticesList2 = EarclipDecomposer.ConvexPartition(vertices2);
            break;
          case Decomposer.Flipcode:
            verticesList2 = FlipcodeDecomposer.ConvexPartition(vertices2);
            break;
          case Decomposer.Seidel:
            verticesList2 = SeidelDecomposer.ConvexPartition(vertices2, 1f / 1000f);
            break;
        }
        foreach (Vertices vertices3 in verticesList2)
        {
          if (vertices3.Count > 2)
            this.BodyMap[gx, gy].Add(BodyFactory.CreatePolygon(this.World, vertices3, 1f));
        }
      }
    }
  }
}
