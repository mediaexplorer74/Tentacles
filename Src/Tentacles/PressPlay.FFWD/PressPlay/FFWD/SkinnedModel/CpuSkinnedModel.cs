// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.SkinnedModel.CpuSkinnedModel
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace PressPlay.FFWD.SkinnedModel
{
  public class CpuSkinnedModel
  {
    private readonly List<CpuSkinnedModelPart> modelParts = new List<CpuSkinnedModelPart>();
    public Matrix BakedTransform;

    public SkinningData SkinningData { get; internal set; }

    public BoundingSphere BoundingSphere { get; internal set; }

    public ReadOnlyCollection<CpuSkinnedModelPart> Parts { get; internal set; }

    internal CpuSkinnedModel(List<CpuSkinnedModelPart> modelParts, SkinningData skinningData)
    {
      this.modelParts = modelParts;
      this.SkinningData = skinningData;
      this.Parts = new ReadOnlyCollection<CpuSkinnedModelPart>((IList<CpuSkinnedModelPart>) this.modelParts);
    }
  }
}
