// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.PathFollowCamNode
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class PathFollowCamNode : MonoBehaviour
  {
    [ContentSerializerIgnore]
    public bool isFirstNode;
    [ContentSerializerIgnore]
    public bool isLastNode;
    [ContentSerializerIgnore]
    public PathFollowCamNode nextNode;
    [ContentSerializerIgnore]
    public PathFollowCamNode previousNode;
    [ContentSerializerIgnore]
    public PathFollowCamNodeConnection frontNodeConnection;
    [ContentSerializerIgnore]
    public PathFollowCamNodeConnection backNodeConnection;
    public bool forceCamHeight;
    public bool forceCamLookAhead;
    public float camHeight = 15f;
    public float camLookAheadFraction = 0.6f;
    public bool useCamLookAheadFraction = true;
    public float yOffset;
    public bool affectCameraRotation = true;
    public bool centerFollowObjectInViewPort;

    public float SqrtDistanceTo(Vector3 _pos) => (this.transform.position - _pos).sqrMagnitude;

    public Vector3 GetPosition(Vector3 _followObjectPos)
    {
      return this.transform.TransformPoint(this.transform.InverseTransformPoint(_followObjectPos) with
      {
        y = 0.0f
      });
    }

    public override void Update() => base.Update();
  }
}
