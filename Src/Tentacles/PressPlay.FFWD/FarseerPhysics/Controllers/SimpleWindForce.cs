// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Controllers.SimpleWindForce
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics.Controllers
{
  public class SimpleWindForce : AbstractForceController
  {
    public Vector2 Direction { get; set; }

    public float Divergence { get; set; }

    public bool IgnorePosition { get; set; }

    public override void ApplyForce(float dt, float strength)
    {
      foreach (Body body in this.World.BodyList)
      {
        float decayMultiplier = this.GetDecayMultiplier(body);
        if ((double) decayMultiplier != 0.0)
        {
          Vector2 vector2;
          if (this.ForceType == AbstractForceController.ForceTypes.Point)
          {
            vector2 = body.Position - this.Position;
          }
          else
          {
            this.Direction.Normalize();
            vector2 = this.Direction;
            if ((double) vector2.Length() == 0.0)
              vector2 = new Vector2(0.0f, 1f);
          }
          if ((double) this.Variation != 0.0)
          {
            float num = (float) this.Randomize.NextDouble() * MathHelper.Clamp(this.Variation, 0.0f, 1f);
            vector2.Normalize();
            body.ApplyForce(vector2 * strength * decayMultiplier * num);
          }
          else
          {
            vector2.Normalize();
            body.ApplyForce(vector2 * strength * decayMultiplier);
          }
        }
      }
    }
  }
}
