// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Asset
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace PressPlay.FFWD
{
  public abstract class Asset : UnityObject
  {
    private bool _isLoaded;

    public Asset() => Application.AddNewAsset(this);

    public string name { get; set; }

    internal void LoadAsset(AssetHelper assetHelper)
    {
      if (this._isLoaded)
        return;
      this.DoLoadAsset(assetHelper);
      this._isLoaded = true;
    }

    protected abstract void DoLoadAsset(AssetHelper assetHelper);

    public override string ToString()
    {
      return string.Format("{0} ({1})", (object) this.GetType().Name, (object) this.GetInstanceID());
    }
  }
}
