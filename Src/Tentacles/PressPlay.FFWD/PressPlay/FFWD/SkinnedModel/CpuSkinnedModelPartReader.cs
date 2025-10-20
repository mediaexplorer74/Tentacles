// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.SkinnedModel.CpuSkinnedModelPartReader
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

#nullable disable
namespace PressPlay.FFWD.SkinnedModel
{
  public class CpuSkinnedModelPartReader : ContentTypeReader<CpuSkinnedModelPart>
  {
    protected override CpuSkinnedModelPart Read(
      ContentReader input,
      CpuSkinnedModelPart existingInstance)
    {
      CpuSkinnedModelPart modelPart = new CpuSkinnedModelPart(((BinaryReader) input).ReadString(), ((BinaryReader) input).ReadInt32(), input.ReadObject<CpuVertex[]>(), input.ReadObject<IndexBuffer>());
      input.ReadSharedResource<BasicEffect>((Action<BasicEffect>) (fx => modelPart.Effect = fx));
      return modelPart;
    }
  }
}
