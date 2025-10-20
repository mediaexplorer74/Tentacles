// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Time
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace PressPlay.FFWD
{
  public static class Time
  {
    public static float time = 0.0f;
    public static float deltaTime = 0.0f;
    public static float fixedDeltaTime = 0.0f;
    public static float timeScale = 1f;
    public static float realtimeSinceStartup = 0.0f;
    public static int frameCount = 0;

    internal static void Reset()
    {
      Time.frameCount = 0;
      Time.time = 0.0f;
      Time.deltaTime = 0.0f;
      Time.timeScale = 1f;
      Time.realtimeSinceStartup = 0.0f;
    }

    internal static void Update(float elapsedSeconds)
    {
      Time.deltaTime = elapsedSeconds * Time.timeScale;
      ++Time.frameCount;
    }

    internal static void FixedUpdate(float elapsedSeconds, float totalSeconds)
    {
      Time.realtimeSinceStartup = totalSeconds;
      Time.deltaTime = elapsedSeconds * Time.timeScale;
      Time.fixedDeltaTime = Time.deltaTime;
      Time.time += Time.deltaTime;
    }
  }
}
