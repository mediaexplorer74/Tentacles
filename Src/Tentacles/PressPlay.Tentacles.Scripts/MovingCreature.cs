// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.MovingCreature
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class MovingCreature : BaseCreature
  {
    protected RaycastHit rh;
    protected Ray ray;
    public float obstacleAvoidanceWidth = 1f;
    public LayerMask obstacleLayers;
    [ContentSerializerIgnore]
    public CreatureMover creatureMover;
    [ContentSerializerIgnore]
    public CreatureMoverNode currentTarget;

    public override void Start()
    {
      this.creatureMover = this.GetComponent<CreatureMover>();
      this.currentTarget = this.creatureMover.startNode;
      base.Start();
    }

    protected override void UpdateStateSleeping()
    {
      base.UpdateStateSleeping();
      if (!this.creatureMover.nodeWrapper.IsPositionInActiveArea(LevelHandler.Instance.lemmy.transform.position))
        return;
      this.ChangeState(BaseCreature.CreatureState.active);
    }
  }
}
