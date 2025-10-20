// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BaseBullet
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public abstract class BaseBullet : ABaseBehaviour
  {
    [ContentSerializerIgnore]
    public BulletData data;
    public int range;
    public int lifetime = 5;
    public int speed = 50;
    public bool rotateToVelocity = true;
    protected Vector3 spawnPosition;
    protected float spawnTime;
    protected bool fired;
    protected bool is_exploded;
    protected Vector3 velocity;

    public override void Start() => this.DoStart();

    public override void Update() => this.DoUpdate();

    protected virtual void DoStart()
    {
    }

    protected virtual void DoUpdate()
    {
      if (this.is_exploded)
      {
        this.gameObject.active = false;
      }
      else
      {
        if (!this.fired)
          return;
        if (this.rotateToVelocity && this.rigidbody != null && (double) this.rigidbody.velocity.magnitude > 0.0)
          this.transform.LookAt(this.transform.position - this.rigidbody.velocity);
        if (this.rotateToVelocity && this.rigidbody == null && (double) this.velocity.magnitude > 0.0)
          this.transform.LookAt(this.transform.position - this.velocity);
        if (this.range > 0 && (double) (this.spawnPosition - this.transform.position).sqrMagnitude > (double) this.range)
          this.Explode(this.data);
        if (this.lifetime <= 0 || (double) Time.time <= (double) this.spawnTime + (double) this.lifetime)
          return;
        this.Explode(this.data);
      }
    }

    public void init(BulletData _data, Vector3 _direction) => this.init(_data, _direction, 1f);

    public virtual void init(BulletData _data, Vector3 _direction, float _charge)
    {
      this.data = _data;
      this.spawnPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
      this.spawnTime = Time.time;
      this.velocity = _direction.normalized * (float) this.speed * _charge;
      this.fired = true;
    }

    public virtual void Scale(float factor)
    {
      this.transform.localScale *= factor;
      if (this.rigidbody == null)
        return;
      this.rigidbody.mass *= factor;
    }

    protected virtual void Explode(BulletData data)
    {
      UnityObject.Destroy((UnityObject) this.gameObject);
    }
  }
}
