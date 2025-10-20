// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.EnergyContainer
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class EnergyContainer : MonoBehaviour
  {
    public List<EnergyCell> cells;
    public BasicEnemyHitLump lumpPrefab;
    private BasicEnemyHitLump closestLump;
    private BaseCreature host;
    private bool isInitialized;

    public int hitpoints
    {
      get
      {
        int hitpoints = 0;
        foreach (EnergyCell cell in this.cells)
          hitpoints += cell.hitpoints;
        return hitpoints;
      }
    }

    public bool isDead => this.hitpoints == 0;

    public bool isAlmostDead => this.hitpoints == 1;

    public override void Start() => this.Initialize();

    public void Initialize()
    {
      if (this.isInitialized)
        return;
      this.ForcedInitialize();
    }

    public void ForcedInitialize()
    {
      EnergyCell[] componentsInChildren = this.GetComponentsInChildren<EnergyCell>();
      this.host = this.GetComponent<BaseCreature>();
      this.cells = new List<EnergyCell>();
      foreach (EnergyCell energyCell in componentsInChildren)
      {
        if (!energyCell.hasHost)
        {
          energyCell.Init(this.lumpPrefab, this.host);
          this.cells.Add(energyCell);
        }
      }
      this.isInitialized = true;
    }

    public EnergyCell GetRandomEnergyCell()
    {
      List<EnergyCell> energyCellList = new List<EnergyCell>();
      foreach (EnergyCell cell in this.cells)
      {
        if (cell.hitpoints > 0)
          energyCellList.Add(cell);
      }
      return energyCellList.Count == 0 ? (EnergyCell) null : energyCellList[Random.Range(0, energyCellList.Count)];
    }

    public void ResetHealth()
    {
      foreach (EnergyCell cell in this.cells)
        cell.ResetHealth(true);
    }

    public BasicEnemyHitLump GetClosestHitlump(Vector3 position)
    {
      this.closestLump = (BasicEnemyHitLump) null;
      float num = -1f;
      foreach (EnergyCell cell in this.cells)
      {
        BasicEnemyHitLump closestHitlump = cell.GetClosestHitlump(position);
        if (closestHitlump != null)
        {
          float magnitude = (closestHitlump.transform.position - position).magnitude;
          if ((double) magnitude < (double) num || (double) num == -1.0)
          {
            num = magnitude;
            this.closestLump = closestHitlump;
          }
        }
      }
      return this.closestLump;
    }

    public void PlayAnimation(string anim)
    {
      foreach (EnergyCell cell in this.cells)
        cell.PlayAnimation(anim);
    }

    public void PlayAnimationQueued(string anim, QueueMode mode)
    {
      foreach (EnergyCell cell in this.cells)
        cell.PlayAnimationQueued(anim, mode);
    }

    public void SetVulnerability(bool _isVulnurable)
    {
      foreach (EnergyCell cell in this.cells)
        cell.SetVulnerability(_isVulnurable);
    }

    public void RemoveAllLumps()
    {
      for (int index = 0; index < this.cells.Count; ++index)
        this.cells[index].RemoveAllLumps();
    }
  }
}
