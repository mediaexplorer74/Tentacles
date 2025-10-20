// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Controllers.AbstractForceController
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Controllers
{
  public abstract class AbstractForceController : Controller
  {
    public Curve DecayCurve;
    public AbstractForceController.ForceTypes ForceType;
    protected Random Randomize;
    public Curve StrengthCurve;

    public AbstractForceController()
      : base(ControllerType.AbstractForceController)
    {
      this.Enabled = true;
      this.Strength = 1f;
      this.Position = new Vector2(0.0f, 0.0f);
      this.MaximumSpeed = 100f;
      this.TimingMode = AbstractForceController.TimingModes.Switched;
      this.ImpulseTime = 0.0f;
      this.ImpulseLength = 1f;
      this.Triggered = false;
      this.StrengthCurve = new Curve();
      this.Variation = 0.0f;
      this.Randomize = new Random(1234);
      this.DecayMode = AbstractForceController.DecayModes.None;
      this.DecayCurve = new Curve();
      this.DecayStart = 0.0f;
      this.DecayEnd = 0.0f;
      this.StrengthCurve.Keys.Add(new CurveKey(0.0f, 5f));
      this.StrengthCurve.Keys.Add(new CurveKey(0.1f, 5f));
      this.StrengthCurve.Keys.Add(new CurveKey(0.2f, -4f));
      this.StrengthCurve.Keys.Add(new CurveKey(1f, 0.0f));
    }

    public AbstractForceController(AbstractForceController.TimingModes mode)
      : base(ControllerType.AbstractForceController)
    {
      this.TimingMode = mode;
      switch (mode)
      {
        case AbstractForceController.TimingModes.Switched:
          this.Enabled = true;
          break;
        case AbstractForceController.TimingModes.Triggered:
          this.Enabled = false;
          break;
        case AbstractForceController.TimingModes.Curve:
          this.Enabled = false;
          break;
      }
    }

    public float Strength { get; set; }

    public Vector2 Position { get; set; }

    public float MaximumSpeed { get; set; }

    public float MaximumForce { get; set; }

    public AbstractForceController.TimingModes TimingMode { get; set; }

    public float ImpulseTime { get; private set; }

    public float ImpulseLength { get; set; }

    public bool Triggered { get; private set; }

    public float Variation { get; set; }

    public AbstractForceController.DecayModes DecayMode { get; set; }

    public float DecayStart { get; set; }

    public float DecayEnd { get; set; }

    protected float GetDecayMultiplier(Body body)
    {
      float num = (body.Position - this.Position).Length();
      switch (this.DecayMode)
      {
        case AbstractForceController.DecayModes.None:
          return 1f;
        case AbstractForceController.DecayModes.Step:
          return (double) num < (double) this.DecayEnd ? 1f : 0.0f;
        case AbstractForceController.DecayModes.Linear:
          if ((double) num < (double) this.DecayStart)
            return 1f;
          return (double) num > (double) this.DecayEnd ? 0.0f : this.DecayEnd - this.DecayStart / num - this.DecayStart;
        case AbstractForceController.DecayModes.InverseSquare:
          return (double) num < (double) this.DecayStart ? 1f : (float) (1.0 / (((double) num - (double) this.DecayStart) * ((double) num - (double) this.DecayStart)));
        case AbstractForceController.DecayModes.Curve:
          return (double) num < (double) this.DecayStart ? 1f : this.DecayCurve.Evaluate(num - this.DecayStart);
        default:
          return 1f;
      }
    }

    public void Trigger()
    {
      this.Triggered = true;
      this.ImpulseTime = 0.0f;
    }

    public override void Update(float dt)
    {
      switch (this.TimingMode)
      {
        case AbstractForceController.TimingModes.Switched:
          if (!this.Enabled)
            break;
          this.ApplyForce(dt, this.Strength);
          break;
        case AbstractForceController.TimingModes.Triggered:
          if (!this.Enabled || !this.Triggered)
            break;
          if ((double) this.ImpulseTime < (double) this.ImpulseLength)
          {
            this.ApplyForce(dt, this.Strength);
            this.ImpulseTime += dt;
            break;
          }
          this.Triggered = false;
          break;
        case AbstractForceController.TimingModes.Curve:
          if (!this.Enabled || !this.Triggered)
            break;
          if ((double) this.ImpulseTime < (double) this.ImpulseLength)
          {
            this.ApplyForce(dt, this.Strength * this.StrengthCurve.Evaluate(this.ImpulseTime));
            this.ImpulseTime += dt;
            break;
          }
          this.Triggered = false;
          break;
      }
    }

    public abstract void ApplyForce(float dt, float strength);

    public enum DecayModes
    {
      None,
      Step,
      Linear,
      InverseSquare,
      Curve,
    }

    public enum ForceTypes
    {
      Point,
      Line,
      Area,
    }

    public enum TimingModes
    {
      Switched,
      Triggered,
      Curve,
    }
  }
}
