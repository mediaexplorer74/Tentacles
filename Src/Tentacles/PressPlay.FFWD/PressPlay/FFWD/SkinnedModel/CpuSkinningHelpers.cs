// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.SkinnedModel.CpuSkinningHelpers
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace PressPlay.FFWD.SkinnedModel
{
  public static class CpuSkinningHelpers
  {
    public static void SkinVertex(
      Matrix[] bones,
      ref Microsoft.Xna.Framework.Vector3 position,
      ref Microsoft.Xna.Framework.Vector3 normal,
      ref Matrix bakedTransform,
      ref Vector4 blendIndices,
      ref Vector4 blendWeights,
      out Microsoft.Xna.Framework.Vector3 outPosition,
      out Microsoft.Xna.Framework.Vector3 outNormal)
    {
      int x = (int) blendIndices.X;
      int y = (int) blendIndices.Y;
      int z = (int) blendIndices.Z;
      int w = (int) blendIndices.W;
      Matrix matrix;
      CpuSkinningHelpers.Blend4x3Matrix(ref bones[x], ref bones[y], ref bones[z], ref bones[w], ref blendWeights, out matrix);
      Matrix.Multiply(ref matrix, ref bakedTransform, out matrix);
      Microsoft.Xna.Framework.Vector3.Transform(ref position, ref matrix, out outPosition);
      Microsoft.Xna.Framework.Vector3.TransformNormal(ref normal, ref matrix, out outNormal);
    }

    private static void Blend4x3Matrix(
      ref Matrix m1,
      ref Matrix m2,
      ref Matrix m3,
      ref Matrix m4,
      ref Vector4 weights,
      out Matrix blended)
    {
      float x = weights.X;
      float y = weights.Y;
      float z = weights.Z;
      float w = weights.W;
      float num1 = (float) ((double) m1.M11 * (double) x + (double) m2.M11 * (double) y + (double) m3.M11 * (double) z + (double) m4.M11 * (double) w);
      float num2 = (float) ((double) m1.M12 * (double) x + (double) m2.M12 * (double) y + (double) m3.M12 * (double) z + (double) m4.M12 * (double) w);
      float num3 = (float) ((double) m1.M13 * (double) x + (double) m2.M13 * (double) y + (double) m3.M13 * (double) z + (double) m4.M13 * (double) w);
      float num4 = (float) ((double) m1.M21 * (double) x + (double) m2.M21 * (double) y + (double) m3.M21 * (double) z + (double) m4.M21 * (double) w);
      float num5 = (float) ((double) m1.M22 * (double) x + (double) m2.M22 * (double) y + (double) m3.M22 * (double) z + (double) m4.M22 * (double) w);
      float num6 = (float) ((double) m1.M23 * (double) x + (double) m2.M23 * (double) y + (double) m3.M23 * (double) z + (double) m4.M23 * (double) w);
      float num7 = (float) ((double) m1.M31 * (double) x + (double) m2.M31 * (double) y + (double) m3.M31 * (double) z + (double) m4.M31 * (double) w);
      float num8 = (float) ((double) m1.M32 * (double) x + (double) m2.M32 * (double) y + (double) m3.M32 * (double) z + (double) m4.M32 * (double) w);
      float num9 = (float) ((double) m1.M33 * (double) x + (double) m2.M33 * (double) y + (double) m3.M33 * (double) z + (double) m4.M33 * (double) w);
      float num10 = (float) ((double) m1.M41 * (double) x + (double) m2.M41 * (double) y + (double) m3.M41 * (double) z + (double) m4.M41 * (double) w);
      float num11 = (float) ((double) m1.M42 * (double) x + (double) m2.M42 * (double) y + (double) m3.M42 * (double) z + (double) m4.M42 * (double) w);
      float num12 = (float) ((double) m1.M43 * (double) x + (double) m2.M43 * (double) y + (double) m3.M43 * (double) z + (double) m4.M43 * (double) w);
      blended = new Matrix();
      blended.M11 = num1;
      blended.M12 = num2;
      blended.M13 = num3;
      blended.M14 = 0.0f;
      blended.M21 = num4;
      blended.M22 = num5;
      blended.M23 = num6;
      blended.M24 = 0.0f;
      blended.M31 = num7;
      blended.M32 = num8;
      blended.M33 = num9;
      blended.M34 = 0.0f;
      blended.M41 = num10;
      blended.M42 = num11;
      blended.M43 = num12;
      blended.M44 = 1f;
    }
  }
}
