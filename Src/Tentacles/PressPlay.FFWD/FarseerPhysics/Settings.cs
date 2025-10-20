// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Settings
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;

#nullable disable
namespace FarseerPhysics
{
  public static class Settings
  {
    public const float MaxFloat = 3.40282347E+38f;
    public const float Epsilon = 1.1920929E-07f;
    public const float Pi = 3.14159274f;
    public const bool ConserveMemory = false;
    public const int MaxManifoldPoints = 2;
    public const float AABBExtension = 0.1f;
    public const float AABBMultiplier = 2f;
    public const float LinearSlop = 0.005f;
    public const float AngularSlop = 0.0349065848f;
    public const float PolygonRadius = 0.01f;
    public const int MaxTOIContacts = 32;
    public const float VelocityThreshold = 1f;
    public const float MaxLinearCorrection = 0.2f;
    public const float MaxAngularCorrection = 0.139626339f;
    public const float ContactBaumgarte = 0.2f;
    public const float TimeToSleep = 0.5f;
    public const float LinearSleepTolerance = 0.01f;
    public const float AngularSleepTolerance = 0.0349065848f;
    public const float MaxTranslation = 2f;
    public const float MaxTranslationSquared = 4f;
    public const float MaxRotation = 1.57079637f;
    public const float MaxRotationSquared = 2.46740127f;
    public const int MaxSubSteps = 8;
    public static bool EnableDiagnostics = true;
    public static int VelocityIterations = 8;
    public static int PositionIterations = 3;
    public static bool ContinuousPhysics = true;
    public static int TOIVelocityIterations = 8;
    public static int TOIPositionIterations = 20;
    public static bool EnableWarmstarting = true;
    public static bool AllowSleep = true;
    public static int MaxPolygonVertices = 8;
    public static bool UseFPECollisionCategories = false;

    public static float MixFriction(float friction1, float friction2)
    {
      return (float) Math.Sqrt((double) friction1 * (double) friction2);
    }

    public static float MixRestitution(float restitution1, float restitution2)
    {
      return (double) restitution1 <= (double) restitution2 ? restitution2 : restitution1;
    }
  }
}
