// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.CreatureMoverNode
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class CreatureMoverNode : MonoBehaviour
  {
    public float rangeSqrt = 4f;
    private CreatureMover currentCreature;
    private CreatureMoverNode.NodeState _nodeState;

    public CreatureMoverNode.NodeState nodeState => this._nodeState;

    public override void Update()
    {
      if (this._nodeState != CreatureMoverNode.NodeState.free && (this.currentCreature == null || this.currentCreature.gameObject == null))
      {
        this._nodeState = CreatureMoverNode.NodeState.free;
        this.currentCreature = (CreatureMover) null;
      }
      if (this._nodeState != CreatureMoverNode.NodeState.free && this.currentCreature != null && this.currentCreature.currentTargetNode != this && (double) (this.currentCreature.transform.position - this.transform.position).sqrMagnitude > (double) this.rangeSqrt)
      {
        this._nodeState = CreatureMoverNode.NodeState.free;
        this.currentCreature = (CreatureMover) null;
      }
      switch (this._nodeState)
      {
        case CreatureMoverNode.NodeState.creatureOn:
          if ((double) (this.currentCreature.transform.position - this.transform.position).sqrMagnitude <= (double) this.rangeSqrt)
            break;
          this._nodeState = CreatureMoverNode.NodeState.free;
          this.currentCreature = (CreatureMover) null;
          break;
        case CreatureMoverNode.NodeState.creatureMovingTowards:
          if ((double) (this.currentCreature.transform.position - this.transform.position).sqrMagnitude >= (double) this.rangeSqrt)
            break;
          this._nodeState = CreatureMoverNode.NodeState.creatureOn;
          break;
      }
    }

    public void ReserveNodeForCreatureMover(CreatureMover _creatureMover)
    {
      this.currentCreature = _creatureMover;
      this._nodeState = CreatureMoverNode.NodeState.creatureMovingTowards;
    }

    public enum NodeState
    {
      free,
      creatureOn,
      creatureMovingTowards,
    }
  }
}
