// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.ObjectMover
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class ObjectMover : MonoBehaviour
  {
    private float speedThresshold = 0.1f;
    private bool velocityBelowThresshold = true;
    private bool hasMovedThisFrame;
    private Vector3 velocity = Vector3.zero;
    private float dampening = 1f;
    private Vector3 activeDampeningVector;
    public float rayCastCollisionDistance = 1f;

    public bool isSleeping => this.velocityBelowThresshold;

    public void AddVelocity(Vector3 _velocity)
    {
      this.velocity += _velocity;
      this.velocityBelowThresshold = false;
      this.RecalcActiveDampeningVector();
    }

    public void SetVelocity(Vector3 _velocity)
    {
      this.velocity = _velocity;
      this.velocityBelowThresshold = false;
      this.RecalcActiveDampeningVector();
    }

    public Vector3 GetVelocity() => this.velocity;

    public void SetDampening(float _dampening)
    {
      this.dampening = _dampening;
      this.RecalcActiveDampeningVector();
    }

    public bool DoMovement(float _deltaTime)
    {
      this.Dampening(_deltaTime);
      this.Movement(_deltaTime);
      return false;
    }

    public bool DoMovement(float _deltaTime, RaycastHit _rh, LayerMask _layersToHit)
    {
      this.Dampening(_deltaTime);
      bool flag = Physics.Raycast(this.transform.position, this.velocity * _deltaTime, out _rh);
      this.Movement(_deltaTime);
      return flag;
    }

    private void RecalcActiveDampeningVector()
    {
      this.activeDampeningVector = Vector3.Lerp(Vector3.zero, this.velocity, this.dampening);
    }

    private void Dampening(float _deltaTime)
    {
      if (this.velocityBelowThresshold)
        return;
      if ((double) this.velocity.sqrMagnitude < (double) (this.activeDampeningVector * _deltaTime).sqrMagnitude)
      {
        this.velocity = Vector3.zero;
        this.velocityBelowThresshold = true;
      }
      else
        this.velocity -= this.activeDampeningVector * _deltaTime;
    }

    private void Movement(float _deltaTime)
    {
      if (this.velocityBelowThresshold || this.hasMovedThisFrame)
        return;
      this.hasMovedThisFrame = true;
      if ((double) this.velocity.sqrMagnitude < (double) this.speedThresshold)
      {
        this.velocity = Vector3.zero;
        this.velocityBelowThresshold = true;
      }
      else
        this.transform.position = this.transform.position + this.velocity * _deltaTime;
    }

    public override void LateUpdate() => this.hasMovedThisFrame = false;
  }
}
