// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.SkinnedModel.CpuSkinnedModelReader
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD.SkinnedModel
{
  public class CpuSkinnedModelReader : ContentTypeReader<CpuSkinnedModel>
  {
    protected override CpuSkinnedModel Read(ContentReader input, CpuSkinnedModel existingInstance)
    {
      return new CpuSkinnedModel(input.ReadObject<List<CpuSkinnedModelPart>>(), input.ReadObject<SkinningData>())
      {
        BakedTransform = input.ReadMatrix(),
        BoundingSphere = input.ReadObject<BoundingSphere>()
      };
    }
  }
}
