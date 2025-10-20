// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SimpleLineDrawer
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SimpleLineDrawer : MonoBehaviour
  {
    public Material material;
    public float startWidth;
    public float endWidth;
    public Transform start;
    public Transform end;
    private MeshFilter meshFilter;
    private Mesh mesh;
    private Microsoft.Xna.Framework.Vector3[] vertices;
    private Microsoft.Xna.Framework.Vector2[] uvs;
    private short[] triangles;
    private PressPlay.FFWD.Vector3 dir;
    private PressPlay.FFWD.Vector3 orthoDir;
    private bool isInitialized;
    public SimpleLineDrawer.TextureOrientation textureOrientation = SimpleLineDrawer.TextureOrientation.leftRight;

    public override void Start()
    {
      if (this.end == null || this.start == null)
        return;
      this.Initialize();
    }

    public void Initialize()
    {
      if (this.isInitialized)
        return;
      this.isInitialized = true;
      this.meshFilter = (MeshFilter) this.gameObject.AddComponent(typeof (MeshFilter));
      this.meshFilter.sharedMesh = new Mesh();
      this.mesh = this.meshFilter.sharedMesh;
      if (this.gameObject.renderer == null || this.gameObject.renderer.GetType() != typeof (MeshRenderer))
        this.gameObject.AddComponent(typeof (MeshRenderer));
      this.gameObject.renderer.sharedMaterial = this.material;
      this.vertices = new Microsoft.Xna.Framework.Vector3[4];
      this.uvs = new Microsoft.Xna.Framework.Vector2[4]
      {
        new Microsoft.Xna.Framework.Vector2(0.0f, 0.0f),
        new Microsoft.Xna.Framework.Vector2(1f, 0.0f),
        new Microsoft.Xna.Framework.Vector2(0.0f, 1f),
        new Microsoft.Xna.Framework.Vector2(1f, 1f)
      };
      this.triangles = new short[6]
      {
        (short) 0,
        (short) 2,
        (short) 1,
        (short) 1,
        (short) 2,
        (short) 3
      };
    }

    public override void LateUpdate()
    {
      if (!this.isInitialized)
        return;
      this.RebuildSquare();
    }

    public void RebuildSquare()
    {
      this.dir = this.end.position - this.start.position;
      this.orthoDir = new PressPlay.FFWD.Vector3(-this.dir.z, 0.0f, this.dir.x).normalized;
      switch (this.textureOrientation)
      {
        case SimpleLineDrawer.TextureOrientation.bottomTop:
          this.vertices[0] = (Microsoft.Xna.Framework.Vector3) this.transform.InverseTransformPoint(this.start.position + this.orthoDir * this.startWidth * 0.5f);
          this.vertices[1] = (Microsoft.Xna.Framework.Vector3) this.transform.InverseTransformPoint(this.start.position - this.orthoDir * this.startWidth * 0.5f);
          this.vertices[2] = (Microsoft.Xna.Framework.Vector3) this.transform.InverseTransformPoint(this.end.position + this.orthoDir * this.endWidth * 0.5f);
          this.vertices[3] = (Microsoft.Xna.Framework.Vector3) this.transform.InverseTransformPoint(this.end.position - this.orthoDir * this.endWidth * 0.5f);
          break;
        case SimpleLineDrawer.TextureOrientation.leftRight:
          this.vertices[2] = (Microsoft.Xna.Framework.Vector3) this.transform.InverseTransformPoint(this.start.position + this.orthoDir * this.startWidth * 0.5f);
          this.vertices[0] = (Microsoft.Xna.Framework.Vector3) this.transform.InverseTransformPoint(this.start.position - this.orthoDir * this.startWidth * 0.5f);
          this.vertices[3] = (Microsoft.Xna.Framework.Vector3) this.transform.InverseTransformPoint(this.end.position + this.orthoDir * this.endWidth * 0.5f);
          this.vertices[1] = (Microsoft.Xna.Framework.Vector3) this.transform.InverseTransformPoint(this.end.position - this.orthoDir * this.endWidth * 0.5f);
          break;
      }
      this.mesh.vertices = this.vertices;
      this.mesh.normals = this.vertices;
      this.mesh.uv = this.uvs;
      this.mesh.triangles = this.triangles;
      this.meshFilter.mesh = this.mesh;
    }

    private void OnDrawGizmos()
    {
      if (this.start == null || this.end == null)
        return;
      Debug.DrawLine(this.start.position, this.end.position, PressPlay.FFWD.Color.grey);
    }

    public enum TextureOrientation
    {
      bottomTop,
      leftRight,
    }
  }
}
