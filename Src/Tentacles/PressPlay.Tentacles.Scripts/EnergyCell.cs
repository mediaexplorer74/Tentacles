// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.EnergyCell
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
  public class EnergyCell : MonoBehaviour
  {
    private List<BasicEnemyHitLump> lumps;
    private BasicEnemyHitLump closestLump;
    private Collider inputTrigger;
    private Collider clawCollider;
    private BaseCreature host;
    private GameObject listener;
    private string methodToCall;
    private BasicEnemyHitLump prefab;
    private List<HitLumpPosition> removedLumps;
    private int maxNumberOfHitpoints;
    private bool collidersAreInitiated;

    public bool hasHost => this.host != null;

    [ContentSerializerIgnore]
    public int hitpoints
    {
      get => this.lumps.Count;
      set
      {
        while (this.hitpoints > 0)
          this.RemoveLump(this.lumps[0]);
      }
    }

    [ContentSerializerIgnore]
    public bool isDead => this.hitpoints == 0;

    [ContentSerializerIgnore]
    public bool isAlmostDead => this.hitpoints == 1;

    public override void Start()
    {
    }

    public void Init(BasicEnemyHitLump prefab) => this.Init(prefab, (BaseCreature) null);

    public void Init(BasicEnemyHitLump prefab, BaseCreature host)
    {
      this.host = host;
      this.prefab = prefab;
      this.lumps = new List<BasicEnemyHitLump>();
      this.removedLumps = new List<HitLumpPosition>();
      BasicEnemyHitLump[] componentsInChildren = this.GetComponentsInChildren<BasicEnemyHitLump>();
      this.maxNumberOfHitpoints = componentsInChildren.Length;
      int num = 0;
      foreach (BasicEnemyHitLump basicEnemyHitLump in componentsInChildren)
      {
        this.AddLump(new HitLumpPosition(basicEnemyHitLump.transform.parent, basicEnemyHitLump.transform.localScale, basicEnemyHitLump.transform.position, basicEnemyHitLump.transform.localPosition, basicEnemyHitLump.transform.rotation));
        UnityObject.Destroy((UnityObject) basicEnemyHitLump.gameObject);
        ++num;
      }
      this.InitColliders();
    }

    private void InitColliders()
    {
      if (this.collidersAreInitiated)
        return;
      this.InitClawCollider();
      this.InitInputTrigger();
      this.collidersAreInitiated = true;
    }

    private void AddLump(HitLumpPosition pos)
    {
      BasicEnemyHitLump basicEnemyHitLump = (BasicEnemyHitLump) UnityObject.Instantiate((UnityObject) this.prefab, pos.position, pos.rotation);
      basicEnemyHitLump.transform.localScale = pos.localScale;
      basicEnemyHitLump.transform.parent = pos.parent;
      basicEnemyHitLump.transform.localPosition = pos.localPosition;
      this.lumps.Add(basicEnemyHitLump);
    }

    public void ResetHealth(bool setColliders)
    {
      foreach (HitLumpPosition removedLump in this.removedLumps)
        this.AddLump(removedLump);
      this.removedLumps = new List<HitLumpPosition>();
      this.SetColliderLayers(setColliders);
    }

    public void SetListener(GameObject target, string method)
    {
      this.listener = target;
      this.methodToCall = method;
    }

    private void InitInputTrigger()
    {
      if (this.inputTrigger == null)
      {
        foreach (Collider componentsInChild in this.gameObject.GetComponentsInChildren<Collider>())
        {
          if (componentsInChild != this.clawCollider)
            this.inputTrigger = componentsInChild;
        }
        if (this.inputTrigger == null || this.inputTrigger == this.clawCollider)
        {
          this.inputTrigger = (Collider) new GameObject("InputTrigger")
          {
            transform = {
              parent = this.transform,
              localPosition = Vector3.zero
            }
          }.AddComponent<SphereCollider>();
          this.inputTrigger.gameObject.layer = GlobalSettings.Instance.enemyInputLayerInt;
          this.inputTrigger.isTrigger = true;
          ((SphereCollider) this.inputTrigger).center = this.GetTriggerCenter();
          ((SphereCollider) this.inputTrigger).radius = this.GetTriggerRadius();
        }
      }
      this.inputTrigger.gameObject.layer = GlobalSettings.Instance.enemyInputLayerInt;
      this.inputTrigger.isTrigger = true;
    }

    private void InitClawCollider()
    {
      if (this.clawCollider == null)
        this.clawCollider = this.GetComponent<Collider>();
      if (this.clawCollider == null)
      {
        this.clawCollider = (Collider) this.gameObject.AddComponent<SphereCollider>();
        this.clawCollider.gameObject.layer = GlobalSettings.Instance.enemyLayerInt;
        this.clawCollider.isTrigger = true;
        ((SphereCollider) this.clawCollider).center = this.GetTriggerCenter();
        ((SphereCollider) this.clawCollider).radius = this.GetTriggerRadius();
      }
      else
        this.clawCollider.gameObject.layer = this.clawCollider.gameObject.layer = GlobalSettings.Instance.enemyLayerInt;
      this.clawCollider.isTrigger = true;
    }

    private Vector3 GetTriggerCenter()
    {
      if (this.lumps.Count == 0)
        return Vector3.zero;
      Vector3 zero = Vector3.zero;
      foreach (BasicEnemyHitLump lump in this.lumps)
        zero += lump.transform.localPosition;
      return zero / (float) this.lumps.Count;
    }

    private float GetTriggerRadius()
    {
      float x = -1f;
      foreach (BasicEnemyHitLump lump1 in this.lumps)
      {
        foreach (BasicEnemyHitLump lump2 in this.lumps)
        {
          float magnitude = (lump1.transform.position - lump2.transform.position).magnitude;
          if ((double) magnitude < (double) x || (double) x == -1.0)
            x = magnitude;
        }
      }
      return Mathf.Max(x, 1f);
    }

    public void RemoveLump(BasicEnemyHitLump lump)
    {
      this.lumps.Remove(lump);
      if (this.host != null)
        this.host.OnLemmyAttack();
      if (this.listener != null)
        this.listener.SendMessage(this.methodToCall, SendMessageOptions.DontRequireReceiver);
      if (!this.isDead)
        return;
      this.SetColliderLayers(false);
    }

    public void SetVulnerability(bool _isVulnurable)
    {
      if (!this.collidersAreInitiated)
        this.InitColliders();
      if (this.isDead)
        return;
      if (_isVulnurable)
      {
        this.clawCollider.gameObject.layer = GlobalSettings.Instance.enemyLayerInt;
        this.PlayAnimation("idle");
      }
      else
      {
        this.clawCollider.gameObject.layer = GlobalSettings.Instance.shieldLayer;
        this.PlayAnimation("closed");
      }
    }

    public void SetColliderLayers(bool active)
    {
      if (!this.collidersAreInitiated)
        this.InitColliders();
      if (active && !this.isDead)
      {
        this.clawCollider.gameObject.layer = GlobalSettings.Instance.enemyLayerInt;
        this.inputTrigger.gameObject.layer = GlobalSettings.Instance.enemyInputLayerInt;
      }
      else
      {
        this.clawCollider.gameObject.layer = 0;
        this.inputTrigger.gameObject.layer = 0;
      }
    }

    public BasicEnemyHitLump GetClosestHitlump(Vector3 position)
    {
      this.closestLump = (BasicEnemyHitLump) null;
      float num = -1f;
      foreach (BasicEnemyHitLump lump in this.lumps)
      {
        float magnitude = (lump.transform.position - position).magnitude;
        if ((double) magnitude < (double) num || (double) num == -1.0)
        {
          num = magnitude;
          this.closestLump = lump;
        }
      }
      this.removedLumps.Add(new HitLumpPosition(this.transform, this.closestLump.transform.localScale, this.closestLump.transform.position, this.closestLump.transform.localPosition, this.closestLump.transform.rotation));
      return this.closestLump;
    }

    public void PlayAnimation(string anim)
    {
      foreach (BasicEnemyHitLump lump in this.lumps)
      {
        if ((bool) (UnityObject) lump.anim)
          lump.anim.Play(anim);
      }
    }

    public void PlayAnimationQueued(string anim, QueueMode mode)
    {
      foreach (BasicEnemyHitLump lump in this.lumps)
      {
        if ((bool) (UnityObject) lump.anim)
          lump.anim.PlayQueued(anim, mode);
      }
    }

    public void RemoveAllLumps()
    {
      for (int index = 0; index < this.lumps.Count; ++index)
        this.RemoveLump(this.lumps[index]);
    }
  }
}
