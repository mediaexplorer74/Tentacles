// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.MiniTentacle
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class MiniTentacle : PoolableObject
  {
    private SimpleLineDrawer lineDrawer;
    private Transform root;
    private Transform tip;

    public override void Create()
    {
      base.Create();
      this.lineDrawer = (SimpleLineDrawer) this.GetComponent(typeof (SimpleLineDrawer));
      this.lineDrawer.startWidth = 0.35f;
      this.lineDrawer.endWidth = 0.35f;
      this.lineDrawer.Initialize();
    }

    public virtual void Initialize(Transform _root, Transform _tip)
    {
      this.tip = _tip;
      this.root = _root;
      this.lineDrawer.start = _root;
      this.lineDrawer.end = _tip;
      this.lineDrawer.RebuildSquare();
    }

    public override void Update()
    {
      if (this.root == null || this.root.gameObject.active && this.tip.gameObject.active)
        return;
      this.Return();
    }
  }
}
