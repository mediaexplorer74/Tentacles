// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Debugging.AccelerometerState
// Assembly: Tentacles, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 94733B2D-6956-40B2-A474-EF03B0110429
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\Tentacles.dll


#nullable disable
namespace PressPlay.Tentacles.Debugging
{
  public struct AccelerometerState
  {
    // Vector3 class is not available in MonoGame, so we replace it with a placeholder
    // public Vector3 Acceleration { get; private set; }
    public object Acceleration { get; private set; }

    public bool IsActive { get; private set; }

    // Vector3 class is not available in MonoGame, so we replace it with a placeholder
    /*
    public AccelerometerState(Vector3 acceleration, bool isActive)
      : this()
    {
      this.Acceleration = acceleration;
      this.IsActive = isActive;
    }
    */
    public AccelerometerState(object acceleration, bool isActive)
      : this()
    {
      this.Acceleration = acceleration;
      this.IsActive = isActive;
    }

    public override string ToString()
    {
      return string.Format("Acceleration: {0}, IsActive: {1}", (object) this.Acceleration, (object) this.IsActive);
    }
  }
}
