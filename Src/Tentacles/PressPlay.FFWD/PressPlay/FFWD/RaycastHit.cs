// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.RaycastHit
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Dynamics;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.FFWD
{
  public struct RaycastHit
  {
    public Body body;
    public Vector3 point;
    public Vector3 normal;
    public float distance;
    public Transform transform;
    public Collider collider;
  }
}
