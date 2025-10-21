// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.RaycastHelper
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Dynamics;
using PressPlay.FFWD.Components;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD
{
  internal static class RaycastHelper
  {
    private static bool _findClosest;
    private static float _distance;
    private static int _layerMask;
    private static List<RaycastHit> _hits = new List<RaycastHit>(10);
    private static RaycastHit _hit;
    private static float _nearest = float.PositiveInfinity;
    private static bool _didHit = false;

    public static void SetValues(float distance, bool findClosest, int layerMask)
    {
      RaycastHelper._distance = distance;
      RaycastHelper._findClosest = findClosest;
      RaycastHelper._layerMask = layerMask;
      RaycastHelper._hits.Clear();
      RaycastHelper._nearest = float.PositiveInfinity;
      RaycastHelper._didHit = false;
    }

    internal static float rayCastCallback(
      Fixture fixture,
      Microsoft.Xna.Framework.Vector2 point,
      Microsoft.Xna.Framework.Vector2 normal,
      float fraction)
    {
      float num = RaycastHelper._distance * fraction;
      Component userData = fixture.Body.UserData;

        //TODO
        Collider collider = default;
      if (!(userData is Collider) && userData is Rigidbody)
        collider = (userData as Rigidbody).collider;
      if (collider == null || collider.gameObject == null || (RaycastHelper._layerMask & 1 << collider.gameObject.layer) <= 0)
        return -1f;
      if (RaycastHelper._findClosest)
      {
        if ((double) num < (double) RaycastHelper._nearest)
        {
          RaycastHelper._nearest = num;
          RaycastHelper._hit.body = fixture.Body;
          RaycastHelper._hit.point = (Vector3) point;
          RaycastHelper._hit.normal = (Vector3) normal;
          RaycastHelper._hit.distance = num;
          RaycastHelper._hit.collider = collider;
          RaycastHelper._didHit = true;
        }
        return fraction;
      }
      RaycastHelper._hits.Add(new RaycastHit()
      {
        body = fixture.Body,
        point = (Vector3) point,
        normal = (Vector3) normal,
        distance = num,
        collider = collider
      });
      return 1f;
    }

    internal static int HitCount
    {
      get => RaycastHelper._findClosest && RaycastHelper._didHit ? 1 : RaycastHelper._hits.Count;
    }

    internal static RaycastHit[] Hits => RaycastHelper._hits.ToArray();

    internal static RaycastHit ClosestHit() => RaycastHelper._hit;

    public static bool pointCastCallback(Fixture fixture)
    {
      Component userData = fixture.Body.UserData;
      Collider collider = default;
      if (!(userData is Collider) && userData is Rigidbody)
        collider = (userData as Rigidbody).collider;
      if (collider != null && collider.gameObject != null && (RaycastHelper._layerMask & 1 << collider.gameObject.layer) > 0)
      {
        RaycastHelper._didHit = true;
        RaycastHelper._hit = new RaycastHit()
        {
          body = fixture.Body,
          collider = collider
        };
      }
      return true;
    }
  }
}
