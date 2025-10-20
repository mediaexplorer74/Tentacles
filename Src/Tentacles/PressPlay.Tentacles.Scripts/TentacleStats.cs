// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.TentacleStats
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class TentacleStats : MonoBehaviour
  {
    public float dragDistMin = 2f;
    public float connectionMaxLength = 12f;
    public float tentacleLength = 9f;
    public float overMaxLengthElasticity = 0.04f;
    public float dragBodyForce = 0.5f;
    public float dragCurvePow = 0.3f;
    public float tentacleTipMoveSpeed = 30f;
    public float optimalConnectionDistance = 2.6f;
    public float connectionTimeout = 1.4f;
    public float searchForConnectionTimeout = 1.4f;
    public float wallSeekHelpDistance = 1.5f;
    public float wallSeekHelpPower = 5f;
    public float controlFlickCurvePow = 0.25f;
    public float controlFlickStrength = 1500f;
    public float minShootSpeed = 20f;
    public float maxShootSpeed = 100f;
    public float controlForce = 40f;
    public float controlForceCurvePow = 0.3f;

    public override void Start()
    {
    }
  }
}
