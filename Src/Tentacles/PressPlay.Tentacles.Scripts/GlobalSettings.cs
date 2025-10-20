// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.GlobalSettings
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class GlobalSettings : MonoBehaviour
  {
    public LayerMask allUILayers;
    public LayerMask squishLemmyLayers;
    public LayerMask allWallsLayers_ClawSpecific;
    public LayerMask allWallsAndShields_ClawSpecific;
    public LayerMask allWallsLayers;
    public LayerMask allWallsAndShields;
    public int shieldLayer;
    public LayerMask tentacleBounceColliderLayerIndex = (LayerMask) 14;
    public LayerMask tentacleBounceColliderLayers;
    public LayerMask tentacleColliderLayerInt = (LayerMask) 8;
    public LayerMask tentacleColliderLayer;
    public int tentacleColliderLayerIndex;
    public LayerMask enemyLayer;
    public int enemyLayerInt = 12;
    public LayerMask inputLayer;
    public LayerMask enemyInputLayer;
    public int enemyInputLayerInt = 16;
    public LayerMask lemmyLayer;
    public LayerMask guiMask;
    public string lemmyTag;
    public string clawTag;
    public string tentacleTipTag = "TentacleTip";
    public string enemyHitLumpTag;
    public string pickupTag;
    public string triggeredByLemmyTag;
    public static bool isLoaded = false;
    private static GlobalSettings instance;

    public static GlobalSettings Instance
    {
      get
      {
        if (GlobalSettings.instance == null)
          Debug.LogError("Attempt to access instance of GlobalSettings singleton earlier than Start or without it being attached to a GameObject.");
        return GlobalSettings.instance;
      }
    }

    public override void Awake()
    {
      if (GlobalSettings.instance != null)
      {
        Debug.LogError("Cannot have two instances of GlobalSettings. Self destruction in 3...");
        UnityObject.Destroy((UnityObject) this);
      }
      else
      {
        GlobalSettings.isLoaded = true;
        GlobalSettings.instance = this;
      }
    }
  }
}
