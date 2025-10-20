// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.TurnOffAtDistance
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class TurnOffAtDistance : MonoBehaviour
  {
    private static float lLeft;
    private static float rLeft;
    private static float rRight;
    private static float lRight;
    private static float lTop;
    private static float rTop;
    private static float lBottom;
    private static float rBottom;
    public bool notifyGameObjectOnStatusChange;
    public bool useColliderBounds;
    public bool createBoundsFromChildTurnOff = true;
    public float distanceMod;
    public float distanceModX = -1f;
    public float distanceModY = -1f;
    public Vector3 distanceModPos;
    private Bounds ownBounds;
    private bool boundsAreImportant;
    private Vector2 distanceVector = new Vector2();
    public bool markedForDestruction;
    private bool isInitialized;

    public void Initialize()
    {
      if (this.isInitialized)
        return;
      this.isInitialized = true;
      if ((double) this.distanceModX == -1.0)
        this.distanceModX = this.distanceMod;
      if ((double) this.distanceModY == -1.0)
        this.distanceModY = this.distanceMod;
      this.ownBounds = new Bounds(this.transform.position + this.distanceModPos, new Vector3(this.distanceModX, 0.0f, this.distanceModY));
      if ((double) this.distanceModY > 0.0 || (double) this.distanceModX > 0.0)
        this.boundsAreImportant = true;
      Component[] componentsInChildren = this.GetComponentsInChildren(typeof (TurnOffAtDistance));
      for (int index = 0; index < componentsInChildren.Length; ++index)
      {
        if (componentsInChildren[index] != this)
          ((TurnOffAtDistance) componentsInChildren[index]).markedForDestruction = true;
      }
      if (!this.useColliderBounds && (!this.createBoundsFromChildTurnOff || componentsInChildren.Length <= 1))
        return;
      if ((bool) (UnityObject) this.collider)
        this.ownBounds = this.collider.bounds;
      Vector3 max = this.ownBounds.max;
      Vector3 min = this.ownBounds.min;
      for (int index = 0; index < componentsInChildren.Length; ++index)
      {
        if (!((TurnOffAtDistance) componentsInChildren[index]).isInitialized)
          ((TurnOffAtDistance) componentsInChildren[index]).Initialize();
        if ((double) ((TurnOffAtDistance) componentsInChildren[index]).ownBounds.max.x > (double) this.ownBounds.max.x)
        {
          double x1 = (double) ((TurnOffAtDistance) componentsInChildren[index]).ownBounds.max.x;
        }
        if ((double) ((TurnOffAtDistance) componentsInChildren[index]).ownBounds.max.y > (double) this.ownBounds.max.y)
        {
          double y1 = (double) ((TurnOffAtDistance) componentsInChildren[index]).ownBounds.max.y;
        }
        if ((double) ((TurnOffAtDistance) componentsInChildren[index]).ownBounds.max.z > (double) this.ownBounds.max.z)
        {
          double z1 = (double) ((TurnOffAtDistance) componentsInChildren[index]).ownBounds.max.z;
        }
        if ((double) ((TurnOffAtDistance) componentsInChildren[index]).ownBounds.min.x < (double) this.ownBounds.min.x)
        {
          double x2 = (double) ((TurnOffAtDistance) componentsInChildren[index]).ownBounds.min.x;
        }
        if ((double) ((TurnOffAtDistance) componentsInChildren[index]).ownBounds.min.y < (double) this.ownBounds.min.y)
        {
          double y2 = (double) ((TurnOffAtDistance) componentsInChildren[index]).ownBounds.min.y;
        }
        if ((double) ((TurnOffAtDistance) componentsInChildren[index]).ownBounds.min.z < (double) this.ownBounds.min.z)
        {
          double z2 = (double) ((TurnOffAtDistance) componentsInChildren[index]).ownBounds.min.z;
        }
      }
      this.boundsAreImportant = true;
    }

    public bool CheckBounds(Bounds _bounds)
    {
      if (!this.isInitialized)
        return false;
      return this.boundsAreImportant ? this.CheckBoundsIntersect(_bounds) : this.CheckInsideBounds(_bounds);
    }

    public bool CheckInsideBounds(Bounds _bounds)
    {
      return TurnOffAtDistance.PointInsideBoundsXZ(this.transform.position, _bounds);
    }

    public bool CheckBoundsIntersect(Bounds _bounds)
    {
      if (this.collider != null && this.useColliderBounds)
        this.ownBounds = this.collider.bounds;
      return TurnOffAtDistance.BoundsIntersectXZ(this.ownBounds, _bounds);
    }

    public void SetActiveState(bool state)
    {
      if (state)
      {
        this.gameObject.SetActiveRecursively(state);
        if (!this.notifyGameObjectOnStatusChange)
          return;
        this.gameObject.SendMessage("OnTurnOnAtDistance", SendMessageOptions.DontRequireReceiver);
      }
      else
      {
        if (this.notifyGameObjectOnStatusChange)
          this.gameObject.SendMessage("OnTurnOffAtDistance", SendMessageOptions.DontRequireReceiver);
        this.gameObject.SetActiveRecursively(state);
      }
    }

    public void CheckDistance(float distanceSqrt, Vector3 _pos)
    {
      if (!this.isInitialized)
        return;
      this.distanceVector.x = _pos.x - this.transform.position.x;
      this.distanceVector.y = _pos.z - this.transform.position.z;
      if ((double) this.distanceVector.sqrMagnitude > (double) distanceSqrt + (double) this.distanceMod)
      {
        if (!this.gameObject.active)
          return;
        this.gameObject.SetActiveRecursively(false);
      }
      else
      {
        if (this.gameObject.active)
          return;
        this.gameObject.SetActiveRecursively(true);
      }
    }

    public static bool BoundsIntersectXZ(Bounds b1, Bounds b2)
    {
      TurnOffAtDistance.lLeft = b1.center.x - b1.extents.x;
      TurnOffAtDistance.rLeft = b2.center.x - b2.extents.x;
      TurnOffAtDistance.lRight = b1.center.x + b1.extents.x;
      TurnOffAtDistance.rRight = b2.center.x + b2.extents.x;
      TurnOffAtDistance.lTop = b1.center.z - b1.extents.z;
      TurnOffAtDistance.rTop = b2.center.z - b2.extents.z;
      TurnOffAtDistance.lBottom = b1.center.z + b1.extents.z;
      TurnOffAtDistance.rBottom = b2.center.z + b2.extents.z;
      return (double) TurnOffAtDistance.lLeft <= (double) TurnOffAtDistance.rRight && (double) TurnOffAtDistance.lRight >= (double) TurnOffAtDistance.rLeft && (double) TurnOffAtDistance.lTop <= (double) TurnOffAtDistance.rBottom && (double) TurnOffAtDistance.lBottom >= (double) TurnOffAtDistance.rTop;
    }

    public static bool BoundsIntersectXY(Bounds b1, Bounds b2)
    {
      TurnOffAtDistance.lLeft = b1.center.x - b1.extents.x;
      TurnOffAtDistance.rLeft = b2.center.x - b2.extents.x;
      TurnOffAtDistance.lRight = b1.center.x + b1.extents.x;
      TurnOffAtDistance.rRight = b2.center.x + b2.extents.x;
      TurnOffAtDistance.lTop = b1.center.y - b1.extents.y;
      TurnOffAtDistance.rTop = b2.center.y - b2.extents.y;
      TurnOffAtDistance.lBottom = b1.center.y + b1.extents.y;
      TurnOffAtDistance.rBottom = b2.center.y + b2.extents.y;
      return (double) TurnOffAtDistance.lLeft <= (double) TurnOffAtDistance.rRight && (double) TurnOffAtDistance.lRight >= (double) TurnOffAtDistance.rLeft && (double) TurnOffAtDistance.lTop <= (double) TurnOffAtDistance.rBottom && (double) TurnOffAtDistance.lBottom >= (double) TurnOffAtDistance.rTop;
    }

    public static bool PointInsideBoundsXY(Vector3 p, Bounds bounds)
    {
      return (double) p.x >= (double) bounds.center.x - (double) bounds.extents.x && (double) p.x <= (double) bounds.center.x + (double) bounds.extents.x && (double) p.y >= (double) bounds.center.y - (double) bounds.extents.y && (double) p.y <= (double) bounds.center.y + (double) bounds.extents.y;
    }

    public static bool PointInsideBoundsXZ(Vector3 p, Bounds bounds)
    {
      return (double) p.x >= (double) bounds.center.x - (double) bounds.extents.x && (double) p.x <= (double) bounds.center.x + (double) bounds.extents.x && (double) p.z >= (double) bounds.center.z - (double) bounds.extents.z && (double) p.z <= (double) bounds.center.z + (double) bounds.extents.z;
    }
  }
}
