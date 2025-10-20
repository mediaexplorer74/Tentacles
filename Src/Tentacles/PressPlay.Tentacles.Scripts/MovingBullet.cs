// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.MovingBullet
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class MovingBullet : BaseBullet
  {
    public static int zFightCycle;
    private int zFightCycleIndex;
    public DamageLemmyStats damageStats;
    public PoolableObject createOnHitLemmy;
    public PoolableObject createOnDestroy;
    public LayerMask layersToHit;
    private Vector3 lastPosition;
    private RaycastHit rh;
    private Ray ray = new Ray(Vector3.zero, Vector3.zero);
    private bool doRaycast = true;
    private float wallHitDistance = -1f;
    private float distanceTraversed;
    private bool checkForWallDistance = true;

    public override void Start()
    {
      base.Start();
      Collider componentInChildren = this.GetComponentInChildren<Collider>();
      if (componentInChildren != null && componentInChildren.isTrigger)
      {
        this.doRaycast = false;
        componentInChildren.allowTurnOff = true;
      }
      ObjectPool.Instance.Grow(this.createOnHitLemmy, 1, 5);
      ObjectPool.Instance.Grow(this.createOnDestroy, 1, 5);
    }

    public override void Activate()
    {
      base.Activate();
      this.checkForWallDistance = true;
      this.distanceTraversed = 0.0f;
      this.zFightCycleIndex = MovingBullet.zFightCycle++ % 1000;
    }

    private void CheckForWallDist()
    {
      this.ray.origin = this.transform.position;
      this.ray.direction = this.velocity;
      this.wallHitDistance = -1f;
      if (Physics.Raycast(this.ray, out this.rh, 10000f, (int) GlobalSettings.Instance.allWallsLayers))
      {
        Debug.DrawLine(this.transform.position, this.rh.point, Color.red);
        this.wallHitDistance = this.rh.distance;
      }
      this.checkForWallDistance = false;
    }

    protected override void DoUpdate()
    {
      base.DoUpdate();
      this.lastPosition = this.transform.position;
      this.transform.position += this.velocity * Time.deltaTime;
      this.distanceTraversed += (this.velocity * Time.deltaTime).magnitude;
      if (this.checkForWallDistance)
        this.CheckForWallDist();
      if (!this.doRaycast && (double) this.wallHitDistance != -1.0 && (double) this.distanceTraversed > (double) this.wallHitDistance)
        this.DoHitWall();
      if (!this.doRaycast)
        return;
      this.DoCollisionDetection();
    }

    public override void init(BulletData _data, Vector3 _direction, float _charge)
    {
      base.init(_data, _direction, _charge);
      this.lastPosition = this.transform.position;
    }

    public void DoCollisionDetection()
    {
      Vector3 vector3 = this.transform.position - this.lastPosition;
      this.ray.origin = this.lastPosition;
      this.ray.direction = vector3;
      if (!Physics.Raycast(this.ray, out this.rh, vector3.magnitude, (int) this.layersToHit))
        return;
      if (this.rh.collider.gameObject.tag == GlobalSettings.Instance.lemmyTag)
        this.DoHitLemmy(this.ray.direction);
      else
        this.DoHitWall();
    }

    public override void OnTriggerEnter(Collider collider)
    {
      if (collider.tag == GlobalSettings.Instance.lemmyTag)
      {
        this.DoHitLemmy(this.transform.position - this.lastPosition);
      }
      else
      {
        if (!LayerMaskOperations.CheckLayerMaskContainsLayer(this.layersToHit, collider.gameObject.layer))
          return;
        this.DoHitWall();
      }
    }

    public virtual void DoHitByClaw() => throw new NotImplementedException();

    public virtual void DoHitWall()
    {
      if (this.createOnDestroy != null)
        ObjectPool.Instance.Draw(this.createOnDestroy, this.transform.position, this.transform.rotation);
      this.Explode(this.data);
    }

    public virtual void DoHitLemmy(Vector3 _hitDir)
    {
      LevelHandler.Instance.lemmy.Damage(this.damageStats.damage, _hitDir);
      LevelHandler.Instance.lemmy.Push(this.damageStats.push * _hitDir);
      if (this.damageStats.breakLemmysConnections)
        LevelHandler.Instance.lemmy.BreakConnections();
      if (this.createOnHitLemmy != null)
        ObjectPool.Instance.Draw(this.createOnHitLemmy, this.transform.position, this.transform.rotation);
      this.Explode(this.data);
    }

    protected override void Explode(BulletData data)
    {
      ObjectPool.Instance.Return((PoolableObject) this);
    }
  }
}
