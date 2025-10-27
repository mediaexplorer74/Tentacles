// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.TurnOffAtDistance
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class TurnOffAtDistance : MonoBehaviour
  {
    public bool notifyGameObjectOnStatusChange;
    public bool useColliderBounds;
    public bool createBoundsFromChildTurnOff = true;
    public float distanceMod;
    public float distanceModX = -1f;
    public float distanceModY = -1f;
    private Microsoft.Xna.Framework.Vector3 distanceModPos;
    private Microsoft.Xna.Framework.BoundingBox ownBounds;
    private bool boundsAreImportant;
    private Microsoft.Xna.Framework.Vector2 distanceVector = new Microsoft.Xna.Framework.Vector2();
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
      this.ownBounds = new Microsoft.Xna.Framework.BoundingBox((Microsoft.Xna.Framework.Vector3) (this.transform.position + this.distanceModPos - new Microsoft.Xna.Framework.Vector3(this.distanceModX / 2f, 0.0f, this.distanceModY / 2f)), (Microsoft.Xna.Framework.Vector3) (this.transform.position + this.distanceModPos + new Microsoft.Xna.Framework.Vector3(this.distanceModX / 2f, 0.0f, this.distanceModY / 2f)));
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
        this.ownBounds = new Microsoft.Xna.Framework.BoundingBox((Microsoft.Xna.Framework.Vector3) this.collider.bounds.min, (Microsoft.Xna.Framework.Vector3) this.collider.bounds.max);
    }

    public bool CheckBounds(Microsoft.Xna.Framework.BoundingBox _bounds)
    {
      if (!this.isInitialized)
        return false;
      return (double) Microsoft.Xna.Framework.Vector3.Distance(this.transform.position, (Microsoft.Xna.Framework.Vector3) _bounds.Min) < (double) this.distanceMod ||
             (double) Microsoft.Xna.Framework.Vector3.Distance(this.transform.position, (Microsoft.Xna.Framework.Vector3) _bounds.Max) < (double) this.distanceMod ||
             _bounds.Contains((Microsoft.Xna.Framework.Vector3) this.transform.position) != Microsoft.Xna.Framework.ContainmentType.Disjoint;
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

    public void CheckDistance(float distanceSqrt, Microsoft.Xna.Framework.Vector3 _pos)
    {
      if (!this.isInitialized)
        return;
      this.distanceVector.X = _pos.X - this.transform.position.X; 
      this.distanceVector.Y = _pos.Z - this.transform.position.Z; 
      if ((double) this.distanceVector.LengthSquared() > (double) distanceSqrt + (double) this.distanceMod) 
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

    public static bool PointInsideBoundsXY(Microsoft.Xna.Framework.Vector3 p, Microsoft.Xna.Framework.BoundingBox bounds)
    {
      return p.X >= bounds.Min.X && p.X <= bounds.Max.X && p.Y >= bounds.Min.Y && p.Y <= bounds.Max.Y;
    }

    public static bool PointInsideBoundsXZ(Microsoft.Xna.Framework.Vector3 p, Microsoft.Xna.Framework.BoundingBox bounds)
    {
      return p.X >= bounds.Min.X && p.X <= bounds.Max.X && p.Z >= bounds.Min.Z && p.Z <= bounds.Max.Z;
    }
  }
}
