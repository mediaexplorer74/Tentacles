// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.MeshDataReader
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD.SkinnedModel;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD
{
  public class MeshDataReader : ContentTypeReader<MeshData>
  {
    protected override MeshData Read(ContentReader input, MeshData existingInstance)
    {
      return new MeshData()
      {
        skinnedModel = input.ReadObject<CpuSkinnedModel>(),
        model = input.ReadObject<Model>(),
        meshParts = input.ReadObject<Dictionary<string, MeshDataPart>>(),
        boundingSphere = input.ReadObject<BoundingSphere>()
      };
    }
  }
}
