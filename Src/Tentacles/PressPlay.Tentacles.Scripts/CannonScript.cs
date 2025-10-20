// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.CannonScript
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class CannonScript : MonoBehaviour
  {
    public PPAnimationHandler[] nozzleAnimations;
    public GameObject[] nozzles;
    private int currentNozzle;
    public CannonScript.FireOrder fireOrder;

    public void SetNozzles(GameObject[] newNozzles) => this.nozzles = newNozzles;

    public void NextNozzle()
    {
      if (this.nozzles.Length == 0)
        return;
      switch (this.fireOrder)
      {
        case CannonScript.FireOrder.Cyclic:
          ++this.currentNozzle;
          this.currentNozzle %= this.nozzles.Length;
          break;
        case CannonScript.FireOrder.EveryOther:
          ++this.currentNozzle;
          ++this.currentNozzle;
          if (this.currentNozzle % this.nozzles.Length == 0)
          {
            ++this.currentNozzle;
            this.currentNozzle %= this.nozzles.Length;
          }
          if (this.currentNozzle <= this.nozzles.Length - 1)
            break;
          this.currentNozzle %= this.nozzles.Length;
          break;
        case CannonScript.FireOrder.Random:
          this.currentNozzle = Random.Range(0, this.nozzles.Length);
          break;
        default:
          ++this.currentNozzle;
          this.currentNozzle %= this.nozzles.Length;
          break;
      }
    }

    public void PlayNozzleAnimation(string animation, int index)
    {
      if (index >= this.nozzleAnimations.Length || this.nozzleAnimations[index] == null)
        return;
      this.nozzleAnimations[index].Stop(animation);
      this.nozzleAnimations[index].Play(animation);
    }

    public Transform getNozzle()
    {
      return this.nozzles.Length == 0 ? this.transform : this.nozzles[this.currentNozzle].transform;
    }

    public Vector3 getNozzlePosition()
    {
      return this.nozzles.Length == 0 ? this.transform.position : this.nozzles[this.currentNozzle].transform.position;
    }

    public Vector3 getNozzlePosition(int index)
    {
      return this.nozzles.Length == 0 ? this.transform.position : this.nozzles[index].transform.position;
    }

    public Vector3 getNozzleDirection()
    {
      return this.nozzles.Length == 0 ? this.transform.forward : this.nozzles[this.currentNozzle].transform.forward;
    }

    public Vector3 getNozzleDirection(int index)
    {
      return this.nozzles.Length == 0 ? this.transform.forward : this.nozzles[index].transform.forward;
    }

    public Quaternion getNozzleRotation()
    {
      return this.nozzles.Length == 0 ? this.transform.rotation : this.nozzles[this.currentNozzle].transform.rotation;
    }

    public Quaternion getNozzleRotation(int index)
    {
      return this.nozzles.Length == 0 ? this.transform.rotation : this.nozzles[index].transform.rotation;
    }

    public override void Update()
    {
      base.Update();
      for (int index = 0; index < this.nozzles.Length; ++index)
        this.nozzles[index].transform.DebugDrawLocal();
    }

    public enum FireOrder
    {
      Cyclic,
      EveryOther,
      Random,
    }
  }
}
