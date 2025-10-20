// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.GridElement
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class GridElement
  {
    private bool _activationStatus = true;
    public int x;
    public int y;
    public TurnOffAtDistance[] objects;

    public bool activationStatus => this._activationStatus;

    public GridElement(int _x, int _y, List<TurnOffAtDistance> _objects)
    {
      this.x = _x;
      this.y = _y;
      this.objects = new TurnOffAtDistance[_objects.Count];
      for (int index = 0; index < this.objects.Length; ++index)
        this.objects[index] = _objects[index];
    }

    public void CheckDistanceOnObjects(float _distanceSqrt, GameObject _distanceObject)
    {
      for (int index = 0; index < this.objects.Length; ++index)
        this.objects[index].CheckDistance(_distanceSqrt, _distanceObject.transform.position);
    }

    public void SetStatus(bool _status)
    {
      if (this._activationStatus == _status)
        return;
      this._activationStatus = _status;
      for (int index = 0; index < this.objects.Length; ++index)
        this.objects[index].gameObject.SetActiveRecursively(_status);
    }
  }
}
