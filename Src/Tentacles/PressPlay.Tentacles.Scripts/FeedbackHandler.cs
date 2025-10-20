// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.FeedbackHandler
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class FeedbackHandler : MonoBehaviour
  {
    public ChallengeBar challengeBarPrefab;
    [ContentSerializerIgnore]
    public ChallengeBar challengeBar;

    public override void Start()
    {
      this.challengeBar = new GameObject().AddComponent<ChallengeBar>();
      this.challengeBar.transform.parent = this.transform;
    }

    public PoolableObject Show(PoolableObject prefab, Vector3 position, Quaternion rotation)
    {
      return ObjectPool.Instance.Draw(prefab, position, rotation);
    }

    public PoolableText Show(
      PoolableText prefab,
      Vector3 position,
      Quaternion rotation,
      string text)
    {
      PoolableText poolableText = (PoolableText) ObjectPool.Instance.Draw((PoolableObject) prefab, position, rotation);
      poolableText.SetText(text);
      return poolableText;
    }
  }
}
