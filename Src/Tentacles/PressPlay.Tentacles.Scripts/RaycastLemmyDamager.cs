// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.RaycastLemmyDamager
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class RaycastLemmyDamager : BasicLemmyDamager
  {
    public Transform rayStart;
    public Transform rayEnd;
    public LayerMask rayStopColliders;
    private RaycastHit rh;
    private Ray ray = new Ray(Vector3.zero, Vector3.zero);
    private Vector3 dir;
    private float rayLength;
    private Vector3 tmpVector;
    private bool hitLemmyLastFrame;
    private GameObject rayDirectionObject;
    private GameObject hitTrackerObject;

    public override void Start()
    {
      this.rayDirectionObject = new GameObject();
      this.rayDirectionObject.transform.position = this.rayStart.transform.position;
      this.rayDirectionObject.transform.parent = this.transform;
      this.rayDirectionObject.name = "rayDirectionObject";
      this.hitTrackerObject = new GameObject();
      this.hitTrackerObject.transform.position = this.rayStart.transform.position;
      this.hitTrackerObject.transform.parent = this.rayDirectionObject.transform;
      this.hitTrackerObject.name = "hitTrackerObject";
    }

    public override void FixedUpdate()
    {
      if (!this.doDamage)
        return;
      this.dir = this.rayEnd.position - this.rayStart.position;
      this.rayLength = this.dir.magnitude;
      this.ray.origin = this.rayStart.position;
      this.ray.direction = this.dir;
      this.rayDirectionObject.transform.position = this.rayStart.position;
      this.rayDirectionObject.transform.LookAt(this.rayEnd.position, Vector3.up);
      if (Physics.Raycast(this.ray, out this.rh, this.rayLength, (int) this.rayStopColliders))
      {
        if (this.rh.collider.gameObject.tag == GlobalSettings.Instance.lemmyTag)
        {
          this.hitTrackerObject.transform.position = this.rh.collider.transform.position;
          this.tmpVector = this.hitTrackerObject.transform.localPosition;
          this.tmpVector.y = 0.0f;
          this.tmpVector.z = 0.0f;
          this.tmpVector.x = (double) this.tmpVector.x >= 0.0 ? 1f : -1f;
          this.tmpVector = this.hitTrackerObject.transform.TransformDirection(this.tmpVector);
          Debug.DrawRay(this.rh.point, this.tmpVector, Color.red);
          if (this.hitLemmyLastFrame)
            this.StayOnLemmy(this.tmpVector, this.rh.point);
          else
            this.HitLemmy(this.tmpVector, this.rh.point);
          this.hitLemmyLastFrame = true;
        }
        else
          this.hitLemmyLastFrame = false;
      }
      else
        this.hitLemmyLastFrame = false;
    }
  }
}
