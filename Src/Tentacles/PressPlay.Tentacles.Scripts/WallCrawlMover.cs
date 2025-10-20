// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.WallCrawlMover
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class WallCrawlMover : MonoBehaviour
  {
    public Transform target;
    protected Ray ray;
    protected RaycastHit rh;
    protected RaycastHit rightHit;
    protected RaycastHit leftHit;
    public float wallDist = 0.7f;
    public float maxWallDist = 2f;
    public float moveSpeed = 2f;
    public WallCrawlMover.MoveDir moveDir;
    private float rayOriginHeight = 0.25f;
    private float rayOriginWidth = 0.5f;
    private CreatureMoverNodeWrapper activeArea;

    public void Initialize(CreatureMoverNodeWrapper _activeArea) => this.activeArea = _activeArea;

    public void DoMovement() => this.MoveTwoRaycasts();

    protected void MoveTwoRaycasts()
    {
      Vector3 position = this.transform.position;
      switch (this.moveDir)
      {
        case WallCrawlMover.MoveDir.right:
          position += this.transform.right * this.moveSpeed * Time.deltaTime;
          break;
        case WallCrawlMover.MoveDir.left:
          position += -this.transform.right * this.moveSpeed * Time.deltaTime;
          break;
      }
      if (this.activeArea.IsPositionInMovementArea(position))
      {
        this.transform.position = position;
        this.transform.DebugDrawLocal();
        this.ray.direction = -this.target.forward;
        this.ray.origin = this.target.position + this.target.forward * this.rayOriginHeight + this.target.right * this.rayOriginWidth;
        bool flag1 = Physics.Raycast(this.ray, out this.rightHit, this.maxWallDist + this.wallDist, (int) GlobalSettings.Instance.allWallsLayers);
        if (flag1)
          Debug.DrawLine(this.ray.origin, this.rightHit.point, Color.red);
        this.ray.origin = this.target.position + this.target.forward * this.rayOriginHeight - this.target.right * this.rayOriginWidth;
        this.ray.direction = -this.target.forward;
        bool flag2 = Physics.Raycast(this.ray, out this.leftHit, this.maxWallDist + this.wallDist, (int) GlobalSettings.Instance.allWallsLayers);
        if (flag2)
          Debug.DrawLine(this.ray.origin, this.leftHit.point, Color.red);
        if (flag1 && flag2)
        {
          Vector3 vector3 = this.rightHit.point - this.leftHit.point;
          Vector3 forward = new Vector3(vector3.z, 0.0f, -vector3.x);
          Vector3 start = this.rightHit.point - vector3 * 0.5f - forward * this.wallDist;
          Debug.DrawRay(start, forward * 2f, Color.green);
          Debug.DrawRay(start, vector3 * 2f, Color.magenta);
          this.target.position = start;
          this.target.rotation = Quaternion.LookRotation(forward);
        }
        else if (flag1)
          this.target.rotation = Quaternion.LookRotation(-this.rightHit.normal);
        else if (flag2)
          this.target.rotation = Quaternion.LookRotation(-this.leftHit.normal);
        switch (this.moveDir)
        {
          case WallCrawlMover.MoveDir.right:
            if (flag1)
              break;
            this.SwitchDirection();
            break;
          case WallCrawlMover.MoveDir.left:
            if (flag2)
              break;
            this.SwitchDirection();
            break;
        }
      }
      else
        this.SwitchDirection();
    }

    public void SwitchDirection()
    {
      switch (this.moveDir)
      {
        case WallCrawlMover.MoveDir.right:
          this.SetDirection(WallCrawlMover.MoveDir.left);
          break;
        case WallCrawlMover.MoveDir.left:
          this.SetDirection(WallCrawlMover.MoveDir.right);
          break;
      }
    }

    public void SetDirection(WallCrawlMover.MoveDir _newDir) => this.moveDir = _newDir;

    public override void FixedUpdate()
    {
    }

    public enum MoveDir
    {
      right,
      left,
    }
  }
}
