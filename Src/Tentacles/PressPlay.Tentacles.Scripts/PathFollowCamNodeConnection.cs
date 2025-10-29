// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.PathFollowCamNodeConnection
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class PathFollowCamNodeConnection : MonoBehaviour
  {
    public PathFollowCamNode frontNode;
    public PathFollowCamNode backNode;
    public float connectionDistance;
    private List<PathFollowCamNodeConnection> connectionsToDistanceTest = new List<PathFollowCamNodeConnection>();
    public bool centerFollowObjectInViewPort;

    public bool isFirstConnection => this.backNode.isFirstNode || this.frontNode.isFirstNode;

    public bool isLastConnection => this.backNode.isLastNode || this.frontNode.isLastNode;

    public void Initialize(PathFollowCamNode _backNode, PathFollowCamNode _frontNode)
    {
      this.frontNode = _frontNode;
      this.backNode = _backNode;
      this.frontNode.backNodeConnection = this;
      this.backNode.frontNodeConnection = this;
      if (this.backNode.centerFollowObjectInViewPort && this.frontNode.centerFollowObjectInViewPort)
        this.centerFollowObjectInViewPort = true;
      Microsoft.Xna.Framework.Vector3 vector3 = this.frontNode.transform.position - this.backNode.transform.position;
      this.connectionDistance = vector3.Length();
      this.transform.position = _backNode.transform.position;
      this.transform.LookAt(this.transform.position + Microsoft.Xna.Framework.Vector3.Down, new Microsoft.Xna.Framework.Vector3(vector3.Z, 0.0f, -vector3.X));
    }

    public PathFollowCamNodeConnection CheckDistanceOnConnections(Microsoft.Xna.Framework.Vector3 _pos)
    {
      return this.CheckDistanceOnConnections(_pos, (PathFollowCamNodeConnection) null, 0.0f);
    }

    public PathFollowCamNodeConnection CheckDistanceOnConnections(
      Microsoft.Xna.Framework.Vector3 _pos,
      PathFollowCamNodeConnection _lastConnection,
      float _changeToLastConnectionThresshold)
    {
      int index1 = 0;
      this.connectionsToDistanceTest.Clear();
      this.connectionsToDistanceTest.Add(this);
      if (this.frontNode.frontNodeConnection != null)
        this.connectionsToDistanceTest.Add(this.frontNode.frontNodeConnection);
      if (this.backNode.backNodeConnection != null)
        this.connectionsToDistanceTest.Add(this.backNode.backNodeConnection);
      float num = this.connectionsToDistanceTest[0].GetOrthogonalDistanceVector(_pos).Length();
      for (int index2 = 1; index2 < this.connectionsToDistanceTest.Count; ++index2)
      {
        float magnitude = this.connectionsToDistanceTest[index2].GetOrthogonalDistanceVector(_pos).Length();
        if (_lastConnection != null && this.connectionsToDistanceTest[index2] != this && this.connectionsToDistanceTest[index2] == _lastConnection)
          magnitude += _changeToLastConnectionThresshold;
        if ((double) magnitude <= (double) num)
        {
          index1 = index2;
          num = magnitude;
        }
      }
      return this.connectionsToDistanceTest[index1];
    }

    public Microsoft.Xna.Framework.Vector3 GetPositionOnCameraPath(Microsoft.Xna.Framework.Vector3 _pos)
    {
      Microsoft.Xna.Framework.Vector3 position = this.transform.InverseTransformPoint(_pos);
      position.Y = 0.0f;
      if (this.isFirstConnection)
        position.X = Math.Max(0.0f, position.X);
      if (this.isLastConnection)
        position.X = Math.Min(this.connectionDistance, position.X);
      return this.transform.TransformPoint(position);
    }

    public Microsoft.Xna.Framework.Vector3 GetOrthogonalDistanceVector(Microsoft.Xna.Framework.Vector3 _pos)
    {
      Microsoft.Xna.Framework.Vector3 vector3 = this.transform.InverseTransformPoint(_pos);
      if ((double) vector3.X < 0.0)
        return this.backNode.transform.position - _pos;
      return (double) vector3.X > (double) this.connectionDistance ? this.frontNode.transform.position - _pos : this.GetPositionOnCameraPath(_pos) - _pos;
    }

    public Microsoft.Xna.Framework.Quaternion GetRotation(Microsoft.Xna.Framework.Vector3 _pos)
    {
      float progression = this.GetProgression(_pos);
      return (double) progression < 0.5 ? Microsoft.Xna.Framework.Quaternion.Lerp(this.backNode.transform.rotation, this.transform.rotation, progression * 2f) : Microsoft.Xna.Framework.Quaternion.Lerp(this.transform.rotation, this.frontNode.transform.rotation, (float) (((double) progression - 0.5) * 2.0));
    }

    public float GetProgression(Microsoft.Xna.Framework.Vector3 _pos)
    {
      return MathHelper.Clamp(this.transform.InverseTransformPoint(_pos).X / this.connectionDistance, 0f, 1f);
    }
  }
}
