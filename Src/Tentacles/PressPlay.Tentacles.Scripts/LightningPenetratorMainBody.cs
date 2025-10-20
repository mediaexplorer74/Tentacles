// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LightningPenetratorMainBody
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LightningPenetratorMainBody : BasicPenetratorMainBody
  {
    public LineDrawerXZ tailDrawerPrefab;
    private LineDrawerXZ tailDrawer;
    public Transform lightningAnchorUpper;
    public Transform lightningAnchorLower;
    public Transform upperLightning_A;
    public Transform upperLightning_B;
    public Transform upperLightning_C;
    public Transform lowerLightning_A;
    public Transform lowerLightning_B;
    public Transform lowerLightning_C;

    public override void Initialize(
      Transform[] _upperPositions,
      Transform[] _lowerPositions,
      Transform[] _tailPositions)
    {
      base.Initialize(_upperPositions, _lowerPositions, _tailPositions);
      this.tailDrawer = (LineDrawerXZ) UnityObject.Instantiate((UnityObject) this.tailDrawerPrefab, Vector3.zero, Quaternion.LookRotation(Vector3.zero));
      this.tailDrawer.Initialize(this.tailPositions.Length);
    }

    public override void Update()
    {
      base.Update();
      this.upperLightning_A.position = this.upperPositions[0].position;
      this.upperLightning_B.position = this.upperPositions[1].position + this.upperPositions[1].forward * 0.1f;
      this.upperLightning_C.position = this.upperPositions[2].position + this.upperPositions[2].forward * 0.1f;
      this.lowerLightning_A.position = this.lowerPositions[0].position;
      this.lowerLightning_B.position = this.lowerPositions[1].position + this.lowerPositions[1].forward * 0.1f;
      this.lowerLightning_C.position = this.lowerPositions[2].position + this.lowerPositions[2].forward * 0.1f;
      this.tailDrawer.DrawLine(this.tailPositions);
    }
  }
}
