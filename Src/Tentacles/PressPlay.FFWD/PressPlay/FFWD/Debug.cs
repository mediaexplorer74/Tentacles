// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Debug
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD.Components;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace PressPlay.FFWD
{
  public class Debug
  {
    private static List<UnityObject> gosToDisplay = new List<UnityObject>();
    private static Dictionary<string, string> _debugDisplay = new Dictionary<string, string>();
    private static List<Debug.Line> lines;
    public static bool DisplayLog = false;
    private static BasicEffect effect;

    public static void Log(params object[] message)
    {
    }

    public static void LogFormat(string format, params object[] args)
    {
    }

    public static void LogError(string message)
    {
    }

    public static void LogWarning(string message)
    {
    }

    public static void Display(string key, object value)
    {
      Debug._debugDisplay[key] = value.ToString();
    }

    public static IEnumerable<KeyValuePair<string, string>> DisplayStrings
    {
      get => Debug._debugDisplay.Concat<KeyValuePair<string, string>>(Debug.ObjectDisplays);
    }

    public static void DisplayHierarchy(UnityObject obj) => Debug.gosToDisplay.Add(obj);

    public static IEnumerable<KeyValuePair<string, string>> ObjectDisplays
    {
      get
      {
        foreach (UnityObject obj in Debug.gosToDisplay)
        {
          for (Transform trans = obj is GameObject ? (obj as GameObject).transform : (obj as Component).transform; trans != null; trans = trans.transform.parent)
            yield return new KeyValuePair<string, string>(trans.name, trans.transform.position.ToString());
        }
      }
    }

    public static void DrawLine(Vector3 start, Vector3 end)
    {
      Debug.DrawLine(start, end, Color.white);
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color)
    {
    }

    public static void DrawRay(Vector3 start, Vector3 direction, Color color)
    {
    }

    public static void DrawFilledBox(Vector3 center, Vector3 size, Color color)
    {
    }

    public static void DrawBox(
      Vector3 upperLeft,
      Vector3 upperRight,
      Vector3 lowerLeft,
      Vector3 lowerRight,
      Color color)
    {
    }

    internal static void DrawLines(GraphicsDevice device, Camera cam)
    {
      if (Debug.lines == null || Debug.lines.Count == 0)
        return;
      if (cam == null)
      {
        Debug.lines.Clear();
      }
      else
      {
        if (Debug.effect == null)
          Debug.effect = new BasicEffect(device);
        RasterizerState rasterizerState = device.RasterizerState;
        device.RasterizerState = new RasterizerState()
        {
          CullMode = CullMode.None
        };
        Debug.effect.World = Matrix.Identity;
        Debug.effect.View = cam.view;
        Debug.effect.Projection = cam.projectionMatrix;
        Debug.effect.VertexColorEnabled = true;
        Debug.effect.Alpha = 1f;
        VertexPositionColor[] vertexData = new VertexPositionColor[Debug.lines.Count * 2];
        int num1 = 0;
        for (int index1 = 0; index1 < Debug.lines.Count; ++index1)
        {
          VertexPositionColor[] vertexPositionColorArray1 = vertexData;
          int index2 = num1;
          int num2 = index2 + 1;
          vertexPositionColorArray1[index2] = new VertexPositionColor()
          {
            Position = Debug.lines[index1].start,
            Color = Debug.lines[index1].color
          };
          VertexPositionColor[] vertexPositionColorArray2 = vertexData;
          int index3 = num2;
          num1 = index3 + 1;
          vertexPositionColorArray2[index3] = new VertexPositionColor()
          {
            Position = Debug.lines[index1].end,
            Color = Debug.lines[index1].color
          };
        }
        foreach (EffectPass pass in Debug.effect.CurrentTechnique.Passes)
        {
          pass.Apply();
          device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertexData, 0, vertexData.Length / 2);
        }
        device.RasterizerState = rasterizerState;
        Debug.lines.Clear();
      }
    }

    internal struct Line
    {
      internal Microsoft.Xna.Framework.Vector3 start;
      internal Microsoft.Xna.Framework.Vector3 end;
      internal Microsoft.Xna.Framework.Color color;

      internal Line(Vector3 start, Vector3 end, Color color)
      {
        this.start = (Microsoft.Xna.Framework.Vector3) start;
        this.end = (Microsoft.Xna.Framework.Vector3) end;
        this.color = (Microsoft.Xna.Framework.Color) color;
      }
    }
  }
}
