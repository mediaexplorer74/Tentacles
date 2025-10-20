// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.TurnOffAtDistanceHandler
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class TurnOffAtDistanceHandler : MonoBehaviour
  {
    public float turnOffDistance = 12f;
    [ContentSerializerIgnore]
    public Bounds turnOffBounds;
    private float turnOffDistanceSqrt;
    private TurnOffAtDistance[] turnOffAtDistanceObjects;
    public int turnOffFrameSkip = 10;
    private int turnOffCurrentFrame;
    public bool DEACTIVATE_TURN_OFF_AT_DISTANCE;
    private List<TurnOffAtDistance> objectsToActivate = new List<TurnOffAtDistance>();
    private List<TurnOffAtDistance> objectsToDeActivate = new List<TurnOffAtDistance>();

    protected void InitializeDistanceHandling(GameObject _distanceObject)
    {
      if (this.DEACTIVATE_TURN_OFF_AT_DISTANCE)
        return;
      this.CreateArrayOf_TurnOffAtDistanceObjects();
      this.turnOffDistanceSqrt = this.turnOffDistance * this.turnOffDistance;
      this.InitializeTurnOffAtDistanceObjects(_distanceObject);
    }

    private void InitializeTurnOffAtDistanceObjects(GameObject _distanceObject)
    {
      if (this.turnOffAtDistanceObjects.Length == 0)
        return;
      Vector3 position1 = this.turnOffAtDistanceObjects[0].transform.position;
      Vector3 position2 = this.turnOffAtDistanceObjects[0].transform.position;
      for (int index = 0; index < this.turnOffAtDistanceObjects.Length; ++index)
      {
        this.turnOffAtDistanceObjects[index].Initialize();
        if ((double) this.turnOffAtDistanceObjects[index].transform.position.x > (double) position1.x)
          position1.x = this.turnOffAtDistanceObjects[index].transform.position.x;
        if ((double) this.turnOffAtDistanceObjects[index].transform.position.y > (double) position1.y)
          position1.y = this.turnOffAtDistanceObjects[index].transform.position.y;
        if ((double) this.turnOffAtDistanceObjects[index].transform.position.z > (double) position1.z)
          position1.z = this.turnOffAtDistanceObjects[index].transform.position.z;
        if ((double) this.turnOffAtDistanceObjects[index].transform.position.x < (double) position2.x)
          position2.x = this.turnOffAtDistanceObjects[index].transform.position.x;
        if ((double) this.turnOffAtDistanceObjects[index].transform.position.y < (double) position2.y)
          position2.y = this.turnOffAtDistanceObjects[index].transform.position.y;
        if ((double) this.turnOffAtDistanceObjects[index].transform.position.z < (double) position2.z)
          position2.z = this.turnOffAtDistanceObjects[index].transform.position.z;
      }
      List<TurnOffAtDistance> turnOffAtDistanceList = new List<TurnOffAtDistance>();
      for (int index = 0; index < this.turnOffAtDistanceObjects.Length; ++index)
      {
        if (this.turnOffAtDistanceObjects[index].markedForDestruction)
          UnityObject.Destroy((UnityObject) this.turnOffAtDistanceObjects[index]);
        else
          turnOffAtDistanceList.Add(this.turnOffAtDistanceObjects[index]);
      }
      this.turnOffAtDistanceObjects = new TurnOffAtDistance[turnOffAtDistanceList.Count];
      for (int index = 0; index < this.turnOffAtDistanceObjects.Length; ++index)
        this.turnOffAtDistanceObjects[index] = turnOffAtDistanceList[index];
      this.turnOffBounds = new Bounds(_distanceObject.transform.position, Vector3.one * this.turnOffDistance * 2f);
    }

    public void UpdateAllObjectsImmediatly(GameObject _distanceObject)
    {
      if (this.DEACTIVATE_TURN_OFF_AT_DISTANCE)
        return;
      for (int index = 0; index < this.turnOffAtDistanceObjects.Length; ++index)
      {
        if ((bool) (UnityObject) this.turnOffAtDistanceObjects[index])
        {
          this.turnOffBounds.center = _distanceObject.transform.position;
          this.AddObjectToQueue(this.turnOffAtDistanceObjects[index], this.turnOffAtDistanceObjects[index].CheckBounds(this.turnOffBounds));
        }
      }
      this.UpdateAllObjects();
    }

    protected void UpdateTurnOffAtDistanceObjects(GameObject _distanceObject)
    {
      if (this.DEACTIVATE_TURN_OFF_AT_DISTANCE)
        return;
      this.turnOffBounds.center = _distanceObject.transform.position;
      for (int turnOffCurrentFrame = this.turnOffCurrentFrame; turnOffCurrentFrame < this.turnOffAtDistanceObjects.Length; turnOffCurrentFrame += this.turnOffFrameSkip)
      {
        if ((bool) (UnityObject) this.turnOffAtDistanceObjects[turnOffCurrentFrame])
          this.AddObjectToQueue(this.turnOffAtDistanceObjects[turnOffCurrentFrame], this.turnOffAtDistanceObjects[turnOffCurrentFrame].CheckBounds(this.turnOffBounds));
      }
      this.UpdateObjects();
      this.turnOffCurrentFrame = (this.turnOffCurrentFrame + 1) % this.turnOffFrameSkip;
    }

    private void UpdateAllObjects()
    {
      int count1 = this.objectsToActivate.Count;
      for (int index = 0; index < count1; ++index)
      {
        this.objectsToActivate[0].SetActiveState(true);
        this.objectsToActivate.RemoveAt(0);
      }
      int count2 = this.objectsToDeActivate.Count;
      for (int index = 0; index < count2; ++index)
      {
        this.objectsToDeActivate[0].SetActiveState(false);
        this.objectsToDeActivate.RemoveAt(0);
      }
    }

    private void UpdateObjects()
    {
      int num1 = Mathf.Min(1, this.objectsToActivate.Count);
      for (int index = 0; index < num1; ++index)
      {
        this.objectsToActivate[0].SetActiveState(true);
        this.objectsToActivate.RemoveAt(0);
      }
      int num2 = Mathf.Min(1, this.objectsToDeActivate.Count);
      for (int index = 0; index < num2; ++index)
      {
        this.objectsToDeActivate[0].SetActiveState(false);
        this.objectsToDeActivate.RemoveAt(0);
      }
    }

    private void AddObjectToQueue(TurnOffAtDistance obj, bool enable)
    {
      if (enable && obj.gameObject.active || !enable && !obj.gameObject.active)
        return;
      if (enable && !this.objectsToActivate.Contains(obj))
      {
        if (this.objectsToDeActivate.Contains(obj))
          this.objectsToDeActivate.Remove(obj);
        this.objectsToActivate.Add(obj);
      }
      else
      {
        if (enable || this.objectsToDeActivate.Contains(obj))
          return;
        if (this.objectsToActivate.Contains(obj))
          this.objectsToActivate.Remove(obj);
        this.objectsToDeActivate.Add(obj);
      }
    }

    private void CreateArrayOf_TurnOffAtDistanceObjects()
    {
      object[] objectsOfType = (object[]) UnityObject.FindObjectsOfType(typeof (TurnOffAtDistance));
      this.turnOffAtDistanceObjects = new TurnOffAtDistance[objectsOfType.Length];
      for (int index = 0; index < objectsOfType.Length; ++index)
        this.turnOffAtDistanceObjects[index] = (TurnOffAtDistance) objectsOfType[index];
    }
  }
}
