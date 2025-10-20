// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.Maelstrom
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class Maelstrom : Current
  {
    public AudioWrapper sndMaelstrom;
    public float circularSpeedFraction;
    public float centricSpeedFraction;
    public Transform vortexGfx;
    private Material vortexMaterial;
    private SphereCollider sphereCollider;
    private Vector3 currentDirection = Vector3.zero;
    private Vector3 circularDir = Vector3.zero;
    private Vector3 centerDir;
    private float rotationSpeed;
    private float vortexTextureOffsetSpeed;
    public Maelstrom.Direction direction;

    public override void Start()
    {
      this.vortexMaterial = this.vortexGfx.renderer.material;
      this.rotationSpeed = (float) (2.0 * ((double) this.currentSpeed + (double) this.currentStrength));
      if (this.direction == Maelstrom.Direction.counterClockwise)
        this.rotationSpeed = -this.rotationSpeed;
      this.vortexTextureOffsetSpeed = (float) (0.017000000923871994 * ((double) this.currentSpeed + (double) this.currentStrength) * ((double) this.centricSpeedFraction / ((double) this.centricSpeedFraction + (double) this.circularSpeedFraction)));
    }

    public override void Update()
    {
      this.vortexGfx.Rotate(Vector3.up, this.rotationSpeed * Time.deltaTime, Space.Self);
    }

    public override Vector3 GetForce(Rigidbody _rigidBody)
    {
      if (this.sphereCollider == null)
        this.sphereCollider = this.GetComponent<SphereCollider>();
      this.centerDir = this.transform.position - _rigidBody.transform.position;
      if (this.direction == Maelstrom.Direction.counterClockwise)
      {
        this.circularDir.x = this.centerDir.z;
        this.circularDir.z = -this.centerDir.x;
      }
      else
      {
        this.circularDir.x = -this.centerDir.z;
        this.circularDir.z = this.centerDir.x;
      }
      this.currentDirection = (this.circularDir * this.circularSpeedFraction + this.centerDir * this.centricSpeedFraction).normalized;
      this.tmpVelocityDifference = this.currentDirection * this.currentSpeed - _rigidBody.velocity;
      this.distToCurrentCenter = Mathf.Abs(this.centerDir.magnitude / this.sphereCollider.bounds.size.x);
      this.distToCurrentSide = Mathf.Max(1f - this.distToCurrentCenter, 0.0f);
      this.forceModifier = 1f;
      if ((double) this.distToCurrentSide < (double) this.edgeFallOff)
        this.forceModifier = 1f / this.edgeFallOff * this.distToCurrentSide;
      this.tmpForce = this.currentDirection * this.tmpVelocityDifference.magnitude * this.currentStrength * Time.deltaTime * this.forceModifier;
      return this.tmpForce;
    }

    public void OnTurnOffAtDistance() => this.sndMaelstrom.Stop();

    public void OnTurnOnAtDistance() => this.sndMaelstrom.PlaySound();

    public enum Direction
    {
      clockwise,
      counterClockwise,
    }
  }
}
