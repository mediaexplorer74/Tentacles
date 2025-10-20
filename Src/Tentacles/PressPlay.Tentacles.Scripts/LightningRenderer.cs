// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LightningRenderer
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LightningRenderer : MonoBehaviour
  {
    private Vector3 start;
    private Vector3 end;
    private float randomModAtLightningEnds;
    public Transform[] positions;
    public int joints;
    public float randomMovement = 0.3f;
    public LineDrawerXZ lineDrawer;
    private Vector3[] points;
    private Vector3 lastVector;
    private Vector3 tmpVector;
    private int updateIndex;
    private bool _isOn = true;

    public bool isOn => this._isOn;

    public override void Start() => this.Initialize();

    public void Initialize()
    {
      if (this.lineDrawer == null)
        this.lineDrawer = this.GetComponent<LineDrawerXZ>();
      this.lineDrawer.Initialize(this.joints * (this.positions.Length - 1) + 1);
      this.ForcedUpdateAllPositions();
      this.updateIndex = Random.Range(0, 2);
    }

    public void ToggleOn(bool isOn) => this._isOn = isOn;

    public override void Update()
    {
      if ((double) Time.timeScale == 0.0)
        return;
      this.updateIndex = (this.updateIndex + 1) % 1;
      if (this.updateIndex != 0)
        return;
      this.UpdatePositions();
    }

    public void ForcedUpdateAllPositions() => this.UpdatePositions();

    private void UpdatePositions()
    {
      if (this.lineDrawer == null)
        return;
      if (!this._isOn)
      {
        if (!this.lineDrawer.renderer.enabled)
          return;
        this.lineDrawer.renderer.enabled = false;
      }
      else
      {
        if (!this.lineDrawer.renderer.enabled)
          this.lineDrawer.renderer.enabled = true;
        int length = this.joints * (this.positions.Length - 1) + 1;
        if (this.points == null || this.points.Length != length)
          this.points = new Vector3[length];
        int index1 = 0;
        for (int index2 = 0; index2 < this.positions.Length - 1; ++index2)
        {
          this.start = this.positions[index2].position;
          this.end = this.positions[index2 + 1].position;
          for (int x = 0; x < this.joints; ++x)
          {
            this.randomModAtLightningEnds = (float) Mathf.Min(x, this.joints - x) / ((float) this.joints * 0.5f);
            this.lastVector = this.tmpVector;
            this.tmpVector = Vector3.Lerp(this.start, this.end, (float) x / (float) this.joints) + Random.insideUnitSphere * this.randomMovement * this.randomModAtLightningEnds;
            this.points[index1] = this.tmpVector;
            ++index1;
          }
        }
        if (this.positions.Length > 0)
        {
          this.tmpVector = this.positions[this.positions.Length - 1].position;
          this.points[index1] = this.tmpVector;
        }
        this.lineDrawer.DrawLine(this.points);
      }
    }
  }
}
