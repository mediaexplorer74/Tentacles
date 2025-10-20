// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.PathFollowObject
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class PathFollowObject : MonoBehaviour
  {
    public GameObject followObject;
    private PathFollowCamNode[] nodes;
    private PathFollowCamNodeConnection[] nodeConnections;
    private Vector3 gotoPos;
    private Quaternion gotoRotation;
    private bool isLocked;
    public bool automaticMovement;
    public float moveSpeed = 4f;
    private PathFollowCamNodeConnection currentActiveConnection;
    private PathFollowCamNodeConnection startConnection;

    public override void Awake() => this.Initialize();

    public void Initialize()
    {
      if (this.automaticMovement)
      {
        this.followObject = (GameObject) UnityObject.Instantiate((UnityObject) new GameObject(), this.transform.position, this.transform.rotation);
        this.followObject.name = "Object mover target";
      }
      else
        this.followObject = ((Component) UnityObject.FindObjectOfType(typeof (Lemmy))).gameObject;
      this.CreateArrayOf_PathFollowCamNodes();
    }

    private void CreateArrayOf_PathFollowCamNodes()
    {
      UnityObject[] objectsOfType = UnityObject.FindObjectsOfType(typeof (PathFollowCamNode));
      this.nodes = new PathFollowCamNode[objectsOfType.Length];
      for (int index = 0; index < objectsOfType.Length; ++index)
        this.nodes[index] = (PathFollowCamNode) objectsOfType[index];
      this.nodeConnections = new PathFollowCamNodeConnection[this.nodes.Length - 1];
      int index1 = 0;
      for (int index2 = 0; index2 < this.nodes.Length; ++index2)
      {
        if (this.nodes[index2].nextNode != null)
        {
          PathFollowCamNodeConnection camNodeConnection = (PathFollowCamNodeConnection) ((GameObject) UnityObject.Instantiate((UnityObject) new GameObject())).AddComponent(typeof (PathFollowCamNodeConnection));
          camNodeConnection.Initialize(this.nodes[index2], this.nodes[index2].nextNode);
          camNodeConnection.name = ("Node Connection " + (object) index2).ToString();
          this.nodeConnections[index1] = camNodeConnection;
          ++index1;
        }
      }
      this.currentActiveConnection = this.GetClosestConnection(this.followObject.transform.position);
      this.startConnection = this.currentActiveConnection;
    }

    public PathFollowCamNodeConnection GetClosestConnection(Vector3 _pos)
    {
      if (this.nodeConnections.Length == 0)
        return (PathFollowCamNodeConnection) null;
      int index1 = 0;
      float num = this.nodeConnections[0].GetOrthogonalDistanceVector(_pos).sqrMagnitude;
      for (int index2 = 0; index2 < this.nodeConnections.Length; ++index2)
      {
        float sqrMagnitude = this.nodeConnections[index2].GetOrthogonalDistanceVector(_pos).sqrMagnitude;
        if ((double) sqrMagnitude < (double) num)
        {
          index1 = index2;
          num = sqrMagnitude;
        }
      }
      return this.nodeConnections[index1];
    }

    public void Lock() => this.isLocked = true;

    public void Unlock() => this.isLocked = false;

    public override void Update()
    {
      if (this.isLocked)
        return;
      if (this.automaticMovement)
      {
        this.MoveFollowObjectInAutomaticMoveMode();
        this.MoveCameraAccordingToNodeConnection(this.currentActiveConnection);
      }
      else
      {
        this.currentActiveConnection = this.currentActiveConnection.CheckDistanceOnConnections(this.followObject.transform.position);
        this.MoveCameraAccordingToNodeConnection(this.currentActiveConnection);
      }
    }

    private void MoveFollowObjectInAutomaticMoveMode()
    {
      if (this.followObject == null)
        return;
      this.followObject.transform.position += (this.currentActiveConnection.frontNode.transform.position - this.followObject.transform.position).normalized * Time.deltaTime * this.moveSpeed;
      if ((double) this.currentActiveConnection.transform.InverseTransformPoint(this.followObject.transform.position).x <= (double) this.currentActiveConnection.connectionDistance - 0.20000000298023224)
        return;
      this.currentActiveConnection = this.currentActiveConnection.frontNode.frontNodeConnection;
    }

    private void MoveCameraAccordingToNodeConnection(PathFollowCamNodeConnection _nodeConnection)
    {
      this.gotoPos = _nodeConnection.GetPositionOnCameraPath(this.followObject.transform.position);
      this.gotoRotation = _nodeConnection.transform.rotation;
      this.transform.position = Vector3.Lerp(this.transform.position, this.gotoPos, 0.04f);
      this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.gotoRotation, 0.02f);
    }

    public void Reset()
    {
      this.currentActiveConnection = this.startConnection;
      this.transform.position = this.startConnection.backNode.transform.position;
      this.followObject.transform.position = this.startConnection.backNode.transform.position;
      this.followObject.transform.rotation = this.startConnection.backNode.transform.rotation;
    }

    public void GotoNode(PathFollowCamNode _node)
    {
      this.currentActiveConnection = _node.frontNodeConnection;
      this.transform.position = _node.transform.position;
      this.followObject.transform.position = _node.frontNodeConnection.transform.position;
      this.followObject.transform.rotation = _node.frontNodeConnection.transform.rotation;
    }
  }
}
