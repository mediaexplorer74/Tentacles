// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.CreatureMoverNodeWrapper
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class CreatureMoverNodeWrapper : MonoBehaviour
  {
    public CreatureMover[] creatures = new CreatureMover[0];
    public float activeAreaRadius = 20f;
    public float movementAreaRadius = 10f;
    private bool nodesCreated;
    private CreatureMoverNode[] _nodes = new CreatureMoverNode[0];

    public CreatureMoverNode[] nodes
    {
      get
      {
        if (!this.nodesCreated)
          this.CreateNodes();
        return this._nodes;
      }
    }

    public void RegisterCreatureMover(CreatureMover _creatureMover)
    {
      CreatureMover[] creatureMoverArray = new CreatureMover[this.creatures.Length + 1];
      for (int index = 0; index < this.creatures.Length; ++index)
        creatureMoverArray[index] = this.creatures[index];
      creatureMoverArray[creatureMoverArray.Length - 1] = _creatureMover;
    }

    public void CreateNodes() => this._nodes = this.GetComponentsInChildren<CreatureMoverNode>();

    public bool IsPositionInActiveArea(Vector3 _position)
    {
      return (double) (_position - this.transform.position).sqrMagnitude < (double) this.activeAreaRadius * (double) this.activeAreaRadius;
    }

    public bool IsPositionInMovementArea(Vector3 _position)
    {
      return (double) (_position - this.transform.position).sqrMagnitude < (double) this.movementAreaRadius * (double) this.movementAreaRadius;
    }
  }
}
