// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.PathFollowCam
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class PathFollowCam : MonoBehaviour
  {
    private PathFollowCamStats defaultStats;
    public PathFollowCamStats stats;
    public float[] followObjectWeights;
    public Transform[] followObjects;
    private GameObject rotationAnchor;
    public Transform rotationAnchorPosition;
    private GameObject _followObject;
    private Vector3 followObjectVelocity;
    private Vector3 followObjectLastPosition;
    public GameObject camShakeObject;
    public GameObject childCam;
    public Camera backgroundCamera;
    public Camera raycastCamera;
    public Camera GUICamera;
    private PathFollowCamNodeConnection currentActiveConnection;
    private PathFollowCamNodeConnection lastActiveConnection;
    private PathFollowCamNode[] nodes;
    private PathFollowCamNodeConnection[] nodeConnections;
    public static bool isLoaded = false;
    private static PathFollowCam instance;
    private Vector3 childCamPos;
    private Vector3 gotoPos;
    private Quaternion gotoRotation;
    private bool isLocked;
    private Vector3 followObjectPositionInViewPort = Vector3.zero;
    private PathFollowCam.State state;

    [ContentSerializerIgnore]
    public GameObject followObject
    {
      get => this._followObject;
      set
      {
        this._followObject = value;
        this.followObjectLastPosition = this._followObject.transform.position;
      }
    }

    [ContentSerializerIgnore]
    public PathFollowCamNodeConnection getCurrentActiveConnection => this.currentActiveConnection;

    public string getCurrentNodeName
    {
      get
      {
        return this.currentActiveConnection.backNode == null ? "NULL" : this.currentActiveConnection.backNode.name;
      }
    }

    public static PathFollowCam Instance
    {
      get
      {
        if (PathFollowCam.instance == null)
          Debug.LogError("Attempt to access instance of PathFollowCam singleton earlier than Start or without it being attached to a GameObject.");
        return PathFollowCam.instance;
      }
    }

    public void SetCamState(CamState _camState)
    {
      this.stats = _camState.stats;
      this.ChangeState(_camState.state);
      this.followObjectPositionInViewPort = _camState.objectPositionInViewport;
    }

    private void ChangeState(PathFollowCam.State _state) => this.state = _state;

    public void SetBackgroundColor(Color _color) => this.backgroundCamera.backgroundColor = _color;

    public Vector3 GetForwardDirection() => this.currentActiveConnection.transform.right;

    public override void Awake()
    {
      if ((bool) (UnityObject) PathFollowCam.instance)
      {
        Debug.LogError("Cannot have two instances of PathFollowCamHandler. Self destruction in 3...");
        UnityObject.Destroy((UnityObject) this);
      }
      else
      {
        PathFollowCam.isLoaded = true;
        PathFollowCam.instance = this;
        this.Initialize();
      }
    }

    public void Initialize()
    {
      if (this.followObject == null)
        this.followObject = ((Component) UnityObject.FindObjectOfType(typeof (Lemmy))).gameObject;
      this.CreateArrayOf_PathFollowCamNodes();
      this.defaultStats = this.stats;
    }

    public void ChangeToDefaultStats() => this.stats = this.defaultStats;

    public void PlaceObjectInViewPort(Vector3 _viewPortPosition)
    {
      this.followObjectPositionInViewPort = _viewPortPosition;
      this.ChangeState(PathFollowCam.State.placeFollowObjectInViewPort);
    }

    public void FollowPathDefaultStats()
    {
      this.ChangeToDefaultStats();
      this.ChangeState(PathFollowCam.State.followPath);
    }

    public void PlaceBetweenFollowObjects(Transform[] _followObjects, Transform _rotationAnchor)
    {
      float[] _weights = new float[_followObjects.Length];
      for (int index = 0; index < _weights.Length; ++index)
        _weights[index] = 1f;
      this.PlaceBetweenFollowObjects(_followObjects, _weights, _rotationAnchor);
    }

    public void PlaceBetweenFollowObjects(
      Transform[] _followObjects,
      float[] _weights,
      Transform _rotationAnchor)
    {
      if (_followObjects.Length != _weights.Length)
      {
        Debug.LogError("_followObjects and _weights have different length. They MUST have same length");
      }
      else
      {
        this.rotationAnchorPosition = _rotationAnchor;
        this.followObjects = _followObjects;
        this.followObjectWeights = _weights;
        this.ChangeState(PathFollowCam.State.placeBetweenFollowObjects);
      }
    }

    public Quaternion GetRotationFromAnchor(Vector3 _camPosition)
    {
      if (this.rotationAnchor == null)
      {
        this.rotationAnchor = new GameObject();
        this.rotationAnchor.name = "Camera rotation anchor";
      }
      this.rotationAnchor.transform.position = this.rotationAnchorPosition.position;
      Vector3 vector3 = this.rotationAnchor.transform.position - _camPosition;
      this.rotationAnchor.transform.LookAt(this.rotationAnchor.transform.position + Vector3.down, new Vector3(vector3.x, 0.0f, vector3.z));
      return this.rotationAnchor.transform.rotation;
    }

    public void ShakeCamera(Vector3 amount, float time)
    {
      iTween.ShakeRotation(this.camShakeObject, iTween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    private void CreateArrayOf_PathFollowCamNodes()
    {
      object[] objectsOfType = (object[]) UnityObject.FindObjectsOfType(typeof (PathFollowCamNode));
      this.nodes = new PathFollowCamNode[objectsOfType.Length];
      for (int index = 0; index < objectsOfType.Length; ++index)
      {
        this.nodes[index] = (PathFollowCamNode) objectsOfType[index];
        if (((PathFollowCamNode) objectsOfType[index]).nextNode != null)
          ((PathFollowCamNode) objectsOfType[index]).nextNode.previousNode = (PathFollowCamNode) objectsOfType[index];
      }
      PathFollowCamNode node = this.nodes[0];
      for (int index = 0; index < this.nodes.Length; ++index)
      {
        if (this.nodes[index].previousNode == null)
          node = this.nodes[index];
      }
      List<PathFollowCamNode> pathFollowCamNodeList = new List<PathFollowCamNode>();
      PathFollowCamNode pathFollowCamNode;
      for (pathFollowCamNode = node; pathFollowCamNode.nextNode != null; pathFollowCamNode = pathFollowCamNode.nextNode)
        pathFollowCamNodeList.Add(pathFollowCamNode);
      pathFollowCamNodeList.Add(pathFollowCamNode);
      this.nodes = new PathFollowCamNode[pathFollowCamNodeList.Count];
      for (int index = 0; index < this.nodes.Length; ++index)
        this.nodes[index] = pathFollowCamNodeList[index];
      this.nodeConnections = new PathFollowCamNodeConnection[this.nodes.Length - 1];
      for (int index = 0; index < this.nodeConnections.Length; ++index)
      {
        PathFollowCamNodeConnection camNodeConnection = (PathFollowCamNodeConnection) new GameObject().AddComponent(typeof (PathFollowCamNodeConnection));
        camNodeConnection.Initialize(this.nodes[index], this.nodes[index].nextNode);
        camNodeConnection.name = ("Node Connection " + (object) index).ToString();
        this.nodeConnections[index] = camNodeConnection;
      }
      foreach (PathFollowCamNodeConnection nodeConnection in this.nodeConnections)
      {
        if (nodeConnection.backNode.backNodeConnection != null)
        {
          nodeConnection.backNode.transform.rotation = Quaternion.Lerp(nodeConnection.backNode.backNodeConnection.transform.rotation, nodeConnection.transform.rotation, 0.5f);
        }
        else
        {
          nodeConnection.backNode.isFirstNode = true;
          nodeConnection.backNode.transform.rotation = nodeConnection.transform.rotation;
        }
        if (nodeConnection.frontNode.frontNodeConnection == null)
        {
          nodeConnection.backNode.isLastNode = true;
          nodeConnection.frontNode.transform.rotation = nodeConnection.transform.rotation;
        }
      }
      this.currentActiveConnection = this.GetClosestConnection(this.followObject.transform.position);
    }

    public void ActivateClosestConnection(Vector3 _pos)
    {
      this.lastActiveConnection = this.currentActiveConnection;
      this.currentActiveConnection = this.GetClosestConnection(_pos);
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

    public override void FixedUpdate()
    {
      if (this.isLocked)
        return;
      this.followObjectVelocity = (double) Time.deltaTime != 0.0 ? (this.followObject.transform.position - this.followObjectLastPosition) / Time.deltaTime : Vector3.zero;
      this.followObjectLastPosition = this.followObject.transform.position;
      PathFollowCamNodeConnection activeConnection = this.currentActiveConnection;
      this.currentActiveConnection = this.currentActiveConnection.CheckDistanceOnConnections(this.followObject.transform.position, this.lastActiveConnection, this.stats.changeToLastConnectionThresshold);
      if (this.currentActiveConnection != activeConnection)
        this.lastActiveConnection = activeConnection;
      switch (this.state)
      {
        case PathFollowCam.State.followPath:
          this.MoveCameraTowards(this.GetFollowPathPosition(this.currentActiveConnection));
          break;
        case PathFollowCam.State.placeFollowObjectInViewPort:
          this.MoveCameraTowards(this.GetPositionForPlaceInViewport(this.currentActiveConnection));
          break;
        case PathFollowCam.State.placeBetweenFollowObjects:
          PathFollowCamPosition pos = new PathFollowCamPosition()
          {
            gotoPos = this.GetPositionBetweenFollowObjects(),
            childCamPos = new Vector3(0.0f, 0.0f, -this.stats.defaultHeight)
          };
          pos.gotoRotation = this.GetRotationFromAnchor(pos.gotoPos);
          this.MoveCameraTowards(pos);
          break;
      }
    }

    public void MoveToStablePosition()
    {
      PathFollowCamNodeConnection activeConnection = this.currentActiveConnection;
      this.currentActiveConnection = this.currentActiveConnection.CheckDistanceOnConnections(this.followObject.transform.position, this.lastActiveConnection, this.stats.changeToLastConnectionThresshold);
      if (this.currentActiveConnection != activeConnection)
        this.lastActiveConnection = activeConnection;
      switch (this.state)
      {
        case PathFollowCam.State.followPath:
          this.MoveCameraTo(this.GetFollowPathPosition(this.currentActiveConnection));
          break;
        case PathFollowCam.State.placeFollowObjectInViewPort:
          this.MoveCameraTo(this.GetPositionForPlaceInViewport(this.currentActiveConnection));
          break;
        case PathFollowCam.State.placeBetweenFollowObjects:
          PathFollowCamPosition pos = new PathFollowCamPosition()
          {
            gotoPos = this.GetPositionBetweenFollowObjects(),
            childCamPos = new Vector3(0.0f, 0.0f, -this.stats.defaultHeight)
          };
          pos.gotoRotation = this.GetRotationFromAnchor(pos.gotoPos);
          this.MoveCameraTo(pos);
          break;
      }
    }

    private Vector3 GetPositionBetweenFollowObjects()
    {
      float num = 0.0f;
      Vector3 zero = Vector3.zero;
      for (int index = 0; index < this.followObjects.Length; ++index)
      {
        zero += this.followObjects[index].position;
        ++num;
      }
      return zero / num;
    }

    private PathFollowCamPosition GetFollowPathPosition(PathFollowCamNodeConnection _nodeConnection)
    {
      PathFollowCamPosition followPathPosition = new PathFollowCamPosition();
      float sqrMagnitude = this.followObjectVelocity.sqrMagnitude;
      Vector3 vector3 = Vector3.zero;
      float num1 = 0.0f;
      if (this.followObject.rigidbody != null)
      {
        num1 = Mathf.Pow(sqrMagnitude + 1f, this.stats.speedCurvePow) - 1f - this.stats.speedModThresshold;
        if ((double) num1 < 0.0)
          num1 = 0.0f;
        vector3 = this.stats.speedLookAhead * num1 * this.followObjectVelocity.normalized;
      }
      this.gotoPos = !this.currentActiveConnection.centerFollowObjectInViewPort ? _nodeConnection.GetPositionOnCameraPath(this.followObject.transform.position + vector3) : this.followObject.transform.position;
      float magnitude1 = (this.gotoPos - _nodeConnection.frontNode.transform.position).magnitude;
      float magnitude2 = (this.gotoPos - _nodeConnection.backNode.transform.position).magnitude;
      float num2 = magnitude2 + magnitude1;
      if (_nodeConnection.frontNode.affectCameraRotation)
      {
        this.gotoRotation = _nodeConnection.transform.rotation;
        float num3 = 3f;
        if ((double) magnitude2 < (double) num3)
          this.gotoRotation = Quaternion.Lerp(_nodeConnection.backNode.transform.rotation, _nodeConnection.transform.rotation, magnitude2 / num3);
        if ((double) magnitude1 < (double) num3)
          this.gotoRotation = Quaternion.Lerp(_nodeConnection.frontNode.transform.rotation, _nodeConnection.transform.rotation, magnitude1 / num3);
      }
      followPathPosition.gotoPos = this.gotoPos;
      followPathPosition.gotoRotation = this.gotoRotation;
      followPathPosition.speedMod = num1;
      float num4 = (float) -((double) _nodeConnection.frontNode.camHeight - (double) magnitude1 / (double) num2 * ((double) _nodeConnection.frontNode.camHeight - (double) _nodeConnection.backNode.camHeight));
      this.childCamPos.x = this.stats.defaultLookAhead;
      this.childCamPos.y = _nodeConnection.frontNode.yOffset;
      this.childCamPos.z = (float) (-(double) this.stats.defaultHeight - (double) num1 * (double) this.stats.speedZoomOut);
      if (_nodeConnection.frontNode.forceCamLookAhead)
        this.childCamPos.x = -_nodeConnection.frontNode.camLookAheadFraction * num4;
      if (_nodeConnection.frontNode.forceCamHeight)
        this.childCamPos.z = num4;
      followPathPosition.childCamPos = this.childCamPos;
      return followPathPosition;
    }

    private PathFollowCamPosition GetPositionForPlaceInViewport(
      PathFollowCamNodeConnection _nodeConnection)
    {
      PathFollowCamPosition forPlaceInViewport = new PathFollowCamPosition();
      Vector3 zero = Vector3.zero;
      float num1 = Mathf.Pow(this.followObjectVelocity.sqrMagnitude + 1f, this.stats.speedCurvePow) - 1f - this.stats.speedModThresshold;
      if ((double) num1 < 0.0)
        num1 = 0.0f;
      Vector3 vector3 = this.stats.speedLookAhead * num1 * this.followObject.rigidbody.velocity.normalized;
      this.gotoPos = this.followObject.transform.position;
      float magnitude1 = (this.gotoPos - _nodeConnection.frontNode.transform.position).magnitude;
      float magnitude2 = (this.gotoPos - _nodeConnection.backNode.transform.position).magnitude;
      if (_nodeConnection.frontNode.affectCameraRotation)
      {
        this.gotoRotation = _nodeConnection.transform.rotation;
        float num2 = 3f;
        if ((double) magnitude2 < (double) num2)
          this.gotoRotation = Quaternion.Lerp(_nodeConnection.backNode.transform.rotation, _nodeConnection.transform.rotation, magnitude2 / num2);
        if ((double) magnitude1 < (double) num2)
          this.gotoRotation = Quaternion.Lerp(_nodeConnection.frontNode.transform.rotation, _nodeConnection.transform.rotation, magnitude1 / num2);
      }
      this.childCamPos.x = -this.followObjectPositionInViewPort.x;
      this.childCamPos.y = -this.followObjectPositionInViewPort.y;
      this.childCamPos.z = -this.followObjectPositionInViewPort.z;
      forPlaceInViewport.gotoPos = this.gotoPos;
      forPlaceInViewport.gotoRotation = this.gotoRotation;
      forPlaceInViewport.childCamPos = this.childCamPos;
      if (!float.IsNaN(forPlaceInViewport.gotoPos.x) && !float.IsNaN(forPlaceInViewport.gotoPos.y) && !float.IsNaN(forPlaceInViewport.gotoPos.z) && !float.IsNaN(forPlaceInViewport.gotoRotation.x) && !float.IsNaN(forPlaceInViewport.gotoRotation.y) && !float.IsNaN(forPlaceInViewport.gotoRotation.z) && !float.IsNaN(forPlaceInViewport.gotoRotation.w) && !float.IsNaN(forPlaceInViewport.childCamPos.x) && !float.IsNaN(forPlaceInViewport.childCamPos.y) && !float.IsNaN(forPlaceInViewport.childCamPos.z))
        return forPlaceInViewport;
      Debug.LogWarning("TRYING TO MOVE CAMERA TO NaN POSITION!!!!!");
      return forPlaceInViewport;
    }

    private void MoveCameraTowards(PathFollowCamPosition pos)
    {
      if (Keyboard.GetState().IsKeyDown(Keys.T))
      {
        this.MoveCameraTo(pos);
      }
      else
      {
        this.transform.position = Vector3.Lerp(this.transform.position, pos.gotoPos, (this.stats.moveStiffness + pos.speedMod * this.stats.speedMoveStiffnessMod) * Time.deltaTime);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, pos.gotoRotation, (this.stats.turnStiffness + pos.speedMod * this.stats.speedTurnStiffnessMod) * Time.deltaTime);
        this.childCam.transform.localPosition = Vector3.Lerp(this.childCam.transform.localPosition, pos.childCamPos, this.stats.lookAheadAndHeightStiffnes * Time.deltaTime);
      }
    }

    private void MoveCameraTo(PathFollowCamPosition pos)
    {
      this.transform.position = pos.gotoPos;
      this.transform.rotation = pos.gotoRotation;
      this.childCam.transform.localPosition = pos.childCamPos;
    }

    public enum State
    {
      followPath,
      placeFollowObjectInViewPort,
      placeBetweenFollowObjects,
    }
  }
}
