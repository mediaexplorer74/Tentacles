// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.CreatureMover
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
  public class CreatureMover : MonoBehaviour
  {
    private Ray ray;
    private RaycastHit[] rayHits;
    private CreatureMover.MoveFinished moveCallback;
    public MovingCreature creature;
    private CreatureMoverNode _currentTargetNode;
    public CreatureMoverNodeWrapper nodeWrapper;
    public CreatureMoverNode startNode;
    private float rotationSpeed;
    private bool isInitialized;
    private CreatureMover.MoveType moveType;

    [ContentSerializerIgnore]
    public CreatureMoverNode currentTargetNode => this._currentTargetNode;

    public override void Awake() => this.Initialize();

    public void Initialize()
    {
      if (this.isInitialized)
        return;
      this.isInitialized = true;
      if (this.nodeWrapper == null)
      {
        UnityObject[] objectsOfType = UnityObject.FindObjectsOfType(typeof (CreatureMoverNodeWrapper));
        CreatureMoverNodeWrapper[] moverNodeWrapperArray = new CreatureMoverNodeWrapper[objectsOfType.Length];
        for (int index = 0; index < moverNodeWrapperArray.Length; ++index)
        {
          moverNodeWrapperArray[index] = (CreatureMoverNodeWrapper) objectsOfType[index];
          if (moverNodeWrapperArray[index].IsPositionInActiveArea(this.transform.position) || moverNodeWrapperArray[index].IsPositionInMovementArea(this.transform.position))
          {
            this.nodeWrapper = moverNodeWrapperArray[index];
            break;
          }
        }
      }
      if (this.startNode == null)
      {
        float num = -1f;
        for (int index = 0; index < this.nodeWrapper.nodes.Length; ++index)
        {
          float sqrMagnitude = (this.transform.position - this.nodeWrapper.nodes[index].transform.position).sqrMagnitude;
          if ((double) num == -1.0 || (double) sqrMagnitude < (double) num)
          {
            num = sqrMagnitude;
            this.startNode = this.nodeWrapper.nodes[index];
          }
        }
      }
      this.nodeWrapper.RegisterCreatureMover(this);
      if (this.startNode != null)
      {
        this._currentTargetNode = this.startNode;
        this._currentTargetNode.ReserveNodeForCreatureMover(this);
        this.creature.transform.position = this.startNode.transform.position;
      }
      this.nodeWrapper.CreateNodes();
    }

    public void EasedMove(
      Vector3 _targetPos,
      Ease _ease,
      float _duration,
      CreatureMover.MoveFinished _callback)
    {
      this.moveCallback = _callback;
      iTween.MoveTo(this.creature.gameObject, iTween.Hash((object) "position", (object) _targetPos, (object) "time", (object) _duration, (object) "easetype", (object) _ease, (object) "oncomplete", (object) "FinishMove"));
      this.moveType = CreatureMover.MoveType.easedMovement;
    }

    public void EasedMoveToNode(
      CreatureMoverNode _node,
      Ease _ease,
      float _duration,
      CreatureMover.MoveFinished _callback)
    {
      if (_node == null)
      {
        Debug.LogError("cannot burst move towards null node");
      }
      else
      {
        this._currentTargetNode = _node;
        this.currentTargetNode.ReserveNodeForCreatureMover(this);
        this.EasedMove(_node.transform.position, _ease, _duration, _callback);
      }
    }

    public void LinearMoveToNode(
      CreatureMoverNode _node,
      CreatureMover.MoveFinished _callback,
      float _velocity)
    {
      if (_node == null)
      {
        Debug.LogError("cannot move towards null node");
      }
      else
      {
        this.moveCallback = _callback;
        this._currentTargetNode = _node;
        this.currentTargetNode.ReserveNodeForCreatureMover(this);
        this.creature.mover.SetDampening(0.0f);
        this.creature.mover.SetVelocity((this.currentTargetNode.transform.position - this.creature.transform.position).normalized * _velocity);
        this.moveType = CreatureMover.MoveType.linearMovement;
      }
    }

    public void BurstMove(
      Vector3 _velocity,
      float _dampening,
      CreatureMover.MoveFinished _callback)
    {
      this.moveCallback = _callback;
      this.creature.mover.SetDampening(_dampening);
      this.creature.mover.SetVelocity(_velocity);
      this.moveType = CreatureMover.MoveType.burstMove;
    }

    public void BurstMoveToNode(
      CreatureMoverNode _node,
      CreatureMover.MoveFinished _callback,
      float _burstSpeed)
    {
      if (_node == null)
      {
        Debug.LogError("cannot burst move towards null node");
      }
      else
      {
        this.moveCallback = _callback;
        this._currentTargetNode = _node;
        this.currentTargetNode.ReserveNodeForCreatureMover(this);
        float _dampening = (float) (0.2199999988079071 * ((double) Mathf.Pow(_burstSpeed + 1f, 1.1f) - 1.0));
        this.BurstMove((_node.transform.position - this.creature.transform.position) * _burstSpeed, _dampening, _callback);
      }
    }

    public void RotateToNode(
      CreatureMoverNode _node,
      CreatureMover.MoveFinished _callback,
      float _rotationSpeed)
    {
      if (_node == null)
      {
        Debug.LogError("cannot rotate towards null node");
      }
      else
      {
        this.rotationSpeed = _rotationSpeed;
        this.moveCallback = _callback;
        this._currentTargetNode = _node;
        this.currentTargetNode.ReserveNodeForCreatureMover(this);
        this.creature.mover.SetVelocity(Vector3.zero);
        this.moveType = CreatureMover.MoveType.rotate;
      }
    }

    public override void Update()
    {
      switch (this.moveType)
      {
        case CreatureMover.MoveType.rotate:
          this.creature.transform.rotation = Quaternion.Lerp(this.creature.transform.rotation, Quaternion.LookRotation(-(this.currentTargetNode.transform.position - this.creature.transform.position)), Time.deltaTime * this.rotationSpeed);
          this.creature.transform.eulerAngles = new Vector3(0.0f, this.creature.transform.eulerAngles.y, 0.0f);
          if ((double) Vector3.Angle(this.currentTargetNode.transform.position - this.creature.transform.position, this.transform.forward) >= 2.0)
            break;
          this.FinishMove();
          break;
        case CreatureMover.MoveType.burstMove:
          this.creature.mover.DoMovement(Time.deltaTime);
          if (!this.creature.mover.isSleeping)
            break;
          this.FinishMove();
          break;
        case CreatureMover.MoveType.linearMovement:
          if ((double) (this.creature.transform.position - this.currentTargetNode.transform.position).magnitude <= (double) (this.creature.transform.position + this.creature.mover.GetVelocity() * Time.deltaTime - this.currentTargetNode.transform.position).magnitude)
          {
            this.FinishMove();
            break;
          }
          this.creature.mover.DoMovement(Time.deltaTime);
          break;
      }
    }

    private void FinishMove()
    {
      this.creature.mover.SetVelocity(Vector3.zero);
      this.moveType = CreatureMover.MoveType.none;
      CreatureMover.MoveFinished moveCallback = this.moveCallback;
      this.moveCallback = (CreatureMover.MoveFinished) null;
      if (moveCallback == null)
        return;
      moveCallback();
    }

    public CreatureMoverNode GetFirstNodeInSight(
      CreatureMoverNode _originNode,
      LayerMask _obstacleLayers,
      RaycastHit rh)
    {
      for (int index = 0; index < this.nodeWrapper.nodes.Length; ++index)
      {
        if (_originNode != this.nodeWrapper.nodes[index] && !Physics.Linecast(_originNode.transform.position, this.nodeWrapper.nodes[index].transform.position, out rh, (int) _obstacleLayers))
          return this.nodeWrapper.nodes[index];
      }
      return (CreatureMoverNode) null;
    }

    public CreatureMoverNode GetBestNodeInSight(
      Vector3 _targetPosition,
      CreatureMoverNode _originNode,
      float _maxDist)
    {
      return this.GetBestNodeInSight(_targetPosition, _originNode.transform.position, _originNode, _maxDist);
    }

    public CreatureMoverNode GetBestNodeInSight(
      Vector3 _targetPosition,
      Vector3 _originPosition,
      CreatureMoverNode _excludeNode,
      float _maxDist)
    {
      float num = -1f;
      int index1 = -1;
      for (int index2 = 0; index2 < this.nodeWrapper.nodes.Length; ++index2)
      {
        if (_excludeNode != this.nodeWrapper.nodes[index2] && this.nodeWrapper.nodes[index2].nodeState == CreatureMoverNode.NodeState.free)
        {
          if ((double) (this.nodeWrapper.nodes[index2].transform.position - _originPosition).magnitude > (double) _maxDist)
            Debug.DrawLine(_originPosition, this.nodeWrapper.nodes[index2].transform.position, Color.gray);
          else if (this.FreePathCheckNoEnemies(this.nodeWrapper.nodes[index2].transform.position))
          {
            Debug.DrawLine(_originPosition, this.nodeWrapper.nodes[index2].transform.position, Color.green);
            float sqrMagnitude = (this.nodeWrapper.nodes[index2].transform.position - _targetPosition).sqrMagnitude;
            if ((double) num == -1.0 || (double) sqrMagnitude < (double) num)
            {
              num = sqrMagnitude;
              index1 = index2;
            }
          }
          else
            Debug.DrawLine(_originPosition, this.nodeWrapper.nodes[index2].transform.position, Color.red);
        }
      }
      return index1 != -1 ? this.nodeWrapper.nodes[index1] : (CreatureMoverNode) null;
    }

    public bool CheckLineOfSight(
      Vector3 _origin,
      Vector3 _destination,
      List<EnergyCell> _ignoreCells,
      LayerMask _obstacles,
      float _width)
    {
      this.ray.origin = _origin;
      this.ray.direction = _destination - _origin;
      this.rayHits = Physics.RaycastAll(this.ray, (_destination - _origin).magnitude, (int) _obstacles);
      for (int index = 0; index < this.rayHits.Length; ++index)
      {
        bool flag = false;
        foreach (EnergyCell ignoreCell in _ignoreCells)
        {
          if (this.rayHits[index].collider == ignoreCell.collider)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return false;
      }
      return true;
    }

    public bool FreePathCheckNoEnemies(Vector3 _position)
    {
      Vector3 vector3 = _position - this.creature.transform.position;
      vector3.Normalize();
      return !Physics.CheckCapsule(this.creature.transform.position + vector3 * this.creature.obstacleAvoidanceWidth, _position - vector3 * this.creature.obstacleAvoidanceWidth, this.creature.obstacleAvoidanceWidth, this.creature.obstacleLayers) && this.nodeWrapper.IsPositionInMovementArea(_position);
    }

    public bool StandardFreePathCheck(Vector3 _position)
    {
      Vector3 vector3 = _position - this.creature.transform.position;
      vector3.Normalize();
      return !Physics.CheckCapsule(this.creature.transform.position + vector3 * this.creature.obstacleAvoidanceWidth, _position - vector3 * this.creature.obstacleAvoidanceWidth, this.creature.obstacleAvoidanceWidth, this.creature.obstacleLayers) && this.CheckLineOfSight(this.creature.transform.position, _position, this.creature.life.cells, GlobalSettings.Instance.enemyLayer, this.creature.obstacleAvoidanceWidth) && this.nodeWrapper.IsPositionInMovementArea(_position);
    }

    public delegate void MoveFinished();

    public enum MoveType
    {
      none,
      rotate,
      burstMove,
      easedMovement,
      linearMovement,
    }
  }
}
