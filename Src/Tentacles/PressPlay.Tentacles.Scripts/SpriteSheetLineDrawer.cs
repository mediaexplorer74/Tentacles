// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SpriteSheetLineDrawer
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SpriteSheetLineDrawer : UVSpriteSheetAnimator
  {
    public Material material;
    public float startWidth;
    public float endWidth;
    private float widthDif;
    private float curWidth = 1f;
    private float nextWidth = 1f;
    private Microsoft.Xna.Framework.Vector3[] vertices;
    private short[] triangles;
    private bool[] flipped;
    private float lineIncrementDeltaFraction;
    private PressPlay.FFWD.Vector3 p1;
    private PressPlay.FFWD.Vector3 p2;
    private PressPlay.FFWD.Vector3 p3;
    private PressPlay.FFWD.Vector3 tmpVerticePosUpper;
    private PressPlay.FFWD.Vector3 tmpVerticePosLower;
    internal float depth;
    private PressPlay.FFWD.Vector3 orthogonalVector = new PressPlay.FFWD.Vector3();
    private PressPlay.FFWD.Vector3[] points;
    private int pointCnt;
    private int oldPointCnt = -1;

    public override void Start()
    {
    }

    public override void Initialize()
    {
      this.InitializeLine(100);
      this.tileSize = new PressPlay.FFWD.Vector2(1f / (float) this.xCount, 1f / (float) this.yCount);
      this.DrawLine(new PressPlay.FFWD.Vector3[2]
      {
        PressPlay.FFWD.Vector3.zero,
        PressPlay.FFWD.Vector3.forward
      });
      if (!this.automaticPlay || this.automaticPlayAnim == null)
        return;
      this.Play(this.automaticPlayAnim);
    }

    public void InitializeLine(int _maxPoints)
    {
      this.meshFilter = (MeshFilter) this.gameObject.AddComponent(typeof (MeshFilter));
      this.meshFilter.sharedMesh = new Mesh();
      this.mesh = this.meshFilter.mesh;
      if (this.gameObject.renderer == null || this.gameObject.renderer.GetType() != typeof (MeshRenderer))
        this.gameObject.AddComponent(typeof (MeshRenderer));
      this.gameObject.renderer.sharedMaterial = this.material;
      this.points = new PressPlay.FFWD.Vector3[_maxPoints + 1];
      this.Rebuild();
    }

    public void AddPoint(PressPlay.FFWD.Vector3 newPos) => this.insertPoint(newPos);

    public void DrawLine(Transform[] _transforms)
    {
      PressPlay.FFWD.Vector3[] _newPositions = new PressPlay.FFWD.Vector3[_transforms.Length];
      int index = 0;
      foreach (Transform transform in _transforms)
      {
        _newPositions[index] = transform.position;
        ++index;
      }
      this.DrawLine(_newPositions);
    }

    public void DrawLine(PressPlay.FFWD.Vector3[] _newPositions)
    {
      this.pointCnt = 0;
      for (int index = 0; index < _newPositions.Length; ++index)
        this.AddPoint(this.transform.InverseTransformPoint(_newPositions[index]));
      this.Rebuild();
    }

    public void Rebuild()
    {
      this.widthDif = this.endWidth - this.startWidth;
      if (this.pointCnt < 2)
        return;
      bool flag = this.oldPointCnt != this.pointCnt;
      this.oldPointCnt = this.pointCnt;
      if (flag)
      {
        int length = this.pointCnt * 5;
        if (this.vertices == null || this.vertices.Length < length)
        {
          this.vertices = new Microsoft.Xna.Framework.Vector3[length];
          this.baseUVs = new Microsoft.Xna.Framework.Vector2[length];
          this.tmpUVs = new Microsoft.Xna.Framework.Vector2[length];
          this.triangles = new short[(this.pointCnt - 1) * 9];
          this.flipped = new bool[this.pointCnt];
        }
        this.lineIncrementDeltaFraction = (float) (1.0 / ((double) this.pointCnt - 1.0));
      }
      for (int index = 0; index < this.pointCnt - 1; ++index)
      {
        float num1 = (float) index * this.lineIncrementDeltaFraction;
        this.curWidth = (float) (((double) this.startWidth + (double) this.widthDif * (double) num1) * 0.5);
        this.nextWidth = (float) (((double) this.startWidth + (double) this.widthDif * ((double) num1 + (double) this.lineIncrementDeltaFraction)) * 0.5);
        this.p1 = this.points[index];
        this.p2 = this.points[index + 1];
        this.p3 = this.points[index + 2];
        float num2 = Mathf.Atan2(this.p2.z - this.p1.z, this.p1.x - this.p2.x);
        float num3 = num2;
        if ((double) num3 < 0.0)
          num3 += 6.28318548f;
        float num4 = Mathf.Atan2(this.p3.z - this.p2.z, this.p2.x - this.p3.x);
        if ((double) num4 < 0.0)
          num4 += 6.28318548f;
        float num5 = num3 - num4;
        if ((double) num5 < 0.0)
          num5 += 6.28318548f;
        this.flipped[index] = (double) (num5 * 57.2957764f) > 180.0;
        this.orthogonalVector.x = Mathf.Sin(num2);
        this.orthogonalVector.z = Mathf.Cos(num2);
        this.orthogonalVector.y = this.depth;
        if (index == 0)
        {
          this.tmpVerticePosUpper = this.p1 + this.orthogonalVector * this.curWidth;
          this.tmpVerticePosLower = this.p1 - this.orthogonalVector * this.curWidth;
        }
        this.vertices[index * 5] = (Microsoft.Xna.Framework.Vector3) this.tmpVerticePosUpper;
        this.vertices[index * 5 + 1] = (Microsoft.Xna.Framework.Vector3) this.tmpVerticePosLower;
        this.vertices[index * 5 + 2] = (Microsoft.Xna.Framework.Vector3) (this.p2 + this.orthogonalVector * this.nextWidth);
        this.vertices[index * 5 + 3] = (Microsoft.Xna.Framework.Vector3) (this.p2 - this.orthogonalVector * this.nextWidth);
        this.vertices[index * 5 + 4] = (Microsoft.Xna.Framework.Vector3) this.p2;
        this.tmpVerticePosUpper = (PressPlay.FFWD.Vector3) this.vertices[index * 5 + 2];
        this.tmpVerticePosLower = (PressPlay.FFWD.Vector3) this.vertices[index * 5 + 3];
      }
      if (flag)
      {
        this.tmpUVs = new Microsoft.Xna.Framework.Vector2[this.baseUVs.Length];
        for (int index = 0; index < this.baseUVs.Length; ++index)
        {
          this.baseUVs[index] = (Microsoft.Xna.Framework.Vector2) new PressPlay.FFWD.Vector2(0.0f, 0.0f);
          this.tmpUVs[index] = (Microsoft.Xna.Framework.Vector2) new PressPlay.FFWD.Vector2(0.0f, 0.0f);
        }
        for (int index = 0; index < this.pointCnt - 1; ++index)
        {
          float num = (float) index * this.lineIncrementDeltaFraction;
          this.baseUVs[index * 5].X = num / (float) this.xCount;
          this.baseUVs[index * 5].Y = 0.0f;
          this.baseUVs[index * 5 + 1].X = num / (float) this.xCount;
          this.baseUVs[index * 5 + 1].Y = 1f / (float) this.yCount;
          this.baseUVs[index * 5 + 2].X = (num + this.lineIncrementDeltaFraction) / (float) this.xCount;
          this.baseUVs[index * 5 + 2].Y = 0.0f;
          this.baseUVs[index * 5 + 3].X = (num + this.lineIncrementDeltaFraction) / (float) this.xCount;
          this.baseUVs[index * 5 + 3].Y = 1f / (float) this.yCount;
          this.baseUVs[index * 5 + 4] = (Microsoft.Xna.Framework.Vector2) new PressPlay.FFWD.Vector2(0.5f, 0.5f);
        }
      }
      if (flag)
      {
        for (int index = 0; index < this.triangles.Length; ++index)
        {
          int num6 = index / 9 * 5;
          int num7 = index % 9;
          switch (num7)
          {
            case 0:
              this.triangles[index] = (short) num6;
              break;
            case 1:
              this.triangles[index] = (short) (1 + num6);
              break;
            case 2:
              this.triangles[index] = (short) (2 + num6);
              break;
            case 3:
              this.triangles[index] = (short) (3 + num6);
              break;
            case 4:
              this.triangles[index] = (short) (2 + num6);
              break;
            case 5:
              this.triangles[index] = (short) (1 + num6);
              break;
          }
          if (index < this.triangles.Length - 9)
          {
            if (this.flipped[num6 / 5])
            {
              switch (num7)
              {
                case 6:
                  this.triangles[index] = (short) (3 + num6);
                  continue;
                case 7:
                  this.triangles[index] = (short) (6 + num6);
                  continue;
                case 8:
                  this.triangles[index] = (short) (4 + num6);
                  continue;
                default:
                  continue;
              }
            }
            else
            {
              switch (num7)
              {
                case 6:
                  this.triangles[index] = (short) (5 + num6);
                  continue;
                case 7:
                  this.triangles[index] = (short) (2 + num6);
                  continue;
                case 8:
                  this.triangles[index] = (short) (4 + num6);
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
      this.mesh.Clear();
      this.mesh.vertices = this.vertices;
      this.mesh.uv = this.CreateUVs(this.XPosFromIndex(this.currentFrameIndex), this.YPosFromIndex(this.currentFrameIndex));
      this.mesh.triangles = this.triangles;
      if (!this.isPlaying)
        return;
      this.UpdateAnim();
    }

    private void insertPoint(PressPlay.FFWD.Vector3 newpos)
    {
      this.points[this.pointCnt] = newpos;
      ++this.pointCnt;
    }
  }
}
