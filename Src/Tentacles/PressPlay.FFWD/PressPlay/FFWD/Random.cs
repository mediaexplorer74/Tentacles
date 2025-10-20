// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Random
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace PressPlay.FFWD
{
  public static class Random
  {
    private static System.Random _rnd;
    private static int _seed;

    private static System.Random rnd
    {
      get
      {
        if (Random._rnd == null)
          Random._rnd = new System.Random();
        return Random._rnd;
      }
    }

    public static int seed
    {
      get => Random._seed;
      set
      {
        Random._seed = value;
        Random._rnd = new System.Random(Random._seed);
      }
    }

    public static float value => (float) Random.rnd.NextDouble();

    public static Vector3 insideUnitSphere
    {
      get
      {
        Vector3 insideUnitSphere = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        if ((double) insideUnitSphere.sqrMagnitude > 1.0)
          insideUnitSphere.Normalize();
        return insideUnitSphere;
      }
    }

    public static Vector3 onUnitSphere => Random.insideUnitSphere.normalized;

    public static Vector2 insideUnitCircle
    {
      get
      {
        Vector2 insideUnitCircle = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        if ((double) insideUnitCircle.sqrMagnitude > 1.0)
          insideUnitCircle.Normalize();
        return insideUnitCircle;
      }
    }

    public static int Range(int min, int max) => Random.rnd.Next(min, max);

    public static float Range(float min, float max)
    {
      return (float) Random.rnd.NextDouble() * (max - min) + min;
    }
  }
}
