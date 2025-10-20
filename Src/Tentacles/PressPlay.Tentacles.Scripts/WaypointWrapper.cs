// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.WaypointWrapper
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD.Components;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class WaypointWrapper : MonoBehaviour
  {
    public Waypoint startWaypoint;
    private Waypoint[] _waypoints = new Waypoint[0];
    private bool waypointsCreated;

    [ContentSerializerIgnore]
    public Waypoint[] waypoints
    {
      get
      {
        if (!this.waypointsCreated)
          this.CreateWaypoints();
        return this._waypoints;
      }
    }

    private void CreateWaypoints()
    {
      List<Waypoint> waypointList = new List<Waypoint>();
      Waypoint waypoint = this.startWaypoint;
      if (waypoint != null)
      {
        waypointList.Add(waypoint);
        for (; waypoint.nextWaypoint != null && !waypointList.Contains(waypoint.nextWaypoint); waypoint = waypoint.nextWaypoint)
          waypointList.Add(waypoint.nextWaypoint);
        this._waypoints = new Waypoint[waypointList.Count];
        for (int index = 0; index < this._waypoints.Length; ++index)
          this._waypoints[index] = waypointList[index];
      }
      this.waypointsCreated = true;
    }
  }
}
