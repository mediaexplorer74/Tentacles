// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Collision
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.FFWD
{
  public class Collision
  {
    public Vector3 relativeVelocity { get; internal set; }

    public Collider collider { get; internal set; }

    public ContactPoint[] contacts { get; set; }

    public Transform transform => this.collider.transform;

    public Rigidbody rigidbody => this.collider.rigidbody;

    public GameObject gameObject => this.collider.gameObject;

    internal void SetColliders(Collider a, Collider b)
    {
      this.collider = b;
      for (int index = 0; index < this.contacts.Length; ++index)
      {
        this.contacts[index].thisCollider = a;
        this.contacts[index].otherCollider = b;
      }
    }
  }
}
