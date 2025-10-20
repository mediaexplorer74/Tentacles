// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.iTween
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using PressPlay.FFWD.Components;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD
{
  public class iTween : MonoBehaviour
  {
    public static List<Dictionary<string, object>> tweens = new List<Dictionary<string, object>>();
    private static GameObject cameraFade = (GameObject) null;
    public string id;
    public string type;
    public string method;
    public iTween.EaseType easeType;
    public float time;
    public float delay;
    public iTween.LoopType loopType;
    public bool isRunning;
    public bool isPaused;
    public bool isDelayed;
    private float runningTime;
    private float percentage;
    private float delayStarted;
    private bool kinematic;
    private bool isLocal;
    private bool loop;
    private bool reverse;
    private bool physics;
    private Dictionary<string, object> tweenArguments;
    private Space space;
    private iTween.EasingFunction ease;
    private iTween.ApplyTween apply;
    private AudioSource audioSource;
    private Vector3[] vector3s;
    private Vector2[] vector2s;
    private Color[,] colors;
    private float[] floats;
    private Rectangle[] rects;
    private iTween.CRSpline path;
    private Vector3 preUpdate;
    private Vector3 postUpdate;
    private iTween.NamedValueColor namedcolorvalue;
    private float lastRealTime;
    private bool useRealTime;

    public static void CameraFadeFrom(float amount, float time)
    {
      if (iTween.cameraFade != null)
        iTween.CameraFadeFrom(iTween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
      else
        Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
    }

    public static void CameraFadeFrom(Dictionary<string, object> args)
    {
      if (iTween.cameraFade != null)
        iTween.ColorFrom(iTween.cameraFade, args);
      else
        Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
    }

    public static void CameraFadeTo(float amount, float time)
    {
      if (iTween.cameraFade != null)
        iTween.CameraFadeTo(iTween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
      else
        Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
    }

    public static void CameraFadeTo(Dictionary<string, object> args)
    {
      if (iTween.cameraFade != null)
        iTween.ColorTo(iTween.cameraFade, args);
      else
        Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
    }

    public static void ValueTo(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      if (!args.ContainsKey("onupdate") || !args.ContainsKey("from") || !args.ContainsKey("to"))
      {
        Debug.LogError("iTween Error: ValueTo() requires an 'onupdate' callback function and a 'from' and 'to' property.  The supplied 'onupdate' callback must accept a single argument that is the same type as the supplied 'from' and 'to' properties!");
      }
      else
      {
        args["type"] = (object) "value";
        if (args["from"].GetType() == typeof (Vector2))
          args["method"] = (object) "vector2";
        else if (args["from"].GetType() == typeof (Vector3))
          args["method"] = (object) "vector3";
        else if (args["from"].GetType() == typeof (Rectangle))
          args["method"] = (object) "rect";
        else if (args["from"].GetType() == typeof (float))
          args["method"] = (object) "float";
        else if (args["from"].GetType() == typeof (Color))
        {
          args["method"] = (object) "color";
        }
        else
        {
          Debug.LogError("iTween Error: ValueTo() only works with interpolating Vector3s, Vector2s, floats, ints, Rects and Colors!");
          return;
        }
        if (!args.ContainsKey("easetype"))
          args.Add("easetype", (object) iTween.EaseType.linear);
        iTween.Launch(target, args);
      }
    }

    public static void FadeFrom(GameObject target, float alpha, float time)
    {
      iTween.FadeFrom(target, iTween.Hash((object) nameof (alpha), (object) alpha, (object) nameof (time), (object) time));
    }

    public static void FadeFrom(GameObject target, Dictionary<string, object> args)
    {
      iTween.ColorFrom(target, args);
    }

    public static void FadeTo(GameObject target, float alpha, float time)
    {
      iTween.FadeTo(target, iTween.Hash((object) nameof (alpha), (object) alpha, (object) nameof (time), (object) time));
    }

    public static void FadeTo(GameObject target, Dictionary<string, object> args)
    {
      iTween.ColorTo(target, args);
    }

    public static void ColorFrom(GameObject target, Color color, float time)
    {
      iTween.ColorFrom(target, iTween.Hash((object) nameof (color), (object) color, (object) nameof (time), (object) time));
    }

    public static void ColorFrom(GameObject target, Dictionary<string, object> args)
    {
      Color color1 = new Color();
      Color color2 = new Color();
      args = iTween.CleanArgs(args);
      if (!args.ContainsKey("includechildren") || (bool) args["includechildren"])
      {
        foreach (Transform transform in target.transform)
        {
          Dictionary<string, object> args1 = iTween.CloneDictionary(args);
          args1["ischild"] = (object) true;
          iTween.ColorFrom(transform.gameObject, args1);
        }
      }
      if (!args.ContainsKey("easetype"))
        args.Add("easetype", (object) iTween.EaseType.linear);
      if (target.renderer != null)
        color2 = color1 = target.renderer.material.color;
      if (args.ContainsKey("color"))
      {
        color1 = (Color) args["color"];
      }
      else
      {
        if (args.ContainsKey("r"))
          color1.r = (float) args["r"];
        if (args.ContainsKey("g"))
          color1.r = (float) args["g"];
        if (args.ContainsKey("b"))
          color1.r = (float) args["b"];
        if (args.ContainsKey("a"))
          color1.r = (float) args["a"];
      }
      if (args.ContainsKey("amount"))
      {
        color1.a = (float) args["amount"];
        args.Remove("amount");
      }
      else if (args.ContainsKey("alpha"))
      {
        color1.a = (float) args["alpha"];
        args.Remove("alpha");
      }
      if (target.renderer != null)
        target.renderer.material.color = color1;
      args["color"] = (object) color2;
      args["type"] = (object) "color";
      args["method"] = (object) "to";
      iTween.Launch(target, args);
    }

    public static void ColorTo(GameObject target, Color color, float time)
    {
      iTween.ColorTo(target, iTween.Hash((object) nameof (color), (object) color, (object) nameof (time), (object) time));
    }

    public static void ColorTo(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      if (!args.ContainsKey("includechildren") || (bool) args["includechildren"])
      {
        foreach (Transform transform in target.transform)
        {
          Dictionary<string, object> args1 = iTween.CloneDictionary(args);
          args1["ischild"] = (object) true;
          iTween.ColorTo(transform.gameObject, args1);
        }
      }
      if (!args.ContainsKey("easetype"))
        args.Add("easetype", (object) iTween.EaseType.linear);
      args["type"] = (object) "color";
      args["method"] = (object) "to";
      iTween.Launch(target, args);
    }

    public static void AudioFrom(GameObject target, float volume, float pitch, float time)
    {
      iTween.AudioFrom(target, iTween.Hash((object) nameof (volume), (object) volume, (object) nameof (pitch), (object) pitch, (object) nameof (time), (object) time));
    }

    public static void AudioFrom(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      AudioSource audio;
      if (args.ContainsKey("audiosource"))
        audio = (AudioSource) args["audiosource"];
      else if (target.GetComponent(typeof (AudioSource)) != null)
      {
        audio = target.audio;
      }
      else
      {
        Debug.LogError("iTween Error: AudioFrom requires an AudioSource.");
        return;
      }
      Vector2 vector2_1;
      Vector2 vector2_2;
      vector2_1.x = vector2_2.x = audio.volume;
      vector2_1.y = vector2_2.y = audio.pitch;
      if (args.ContainsKey("volume"))
        vector2_2.x = (float) args["volume"];
      if (args.ContainsKey("pitch"))
        vector2_2.y = (float) args["pitch"];
      audio.volume = vector2_2.x;
      audio.pitch = vector2_2.y;
      args["volume"] = (object) vector2_1.x;
      args["pitch"] = (object) vector2_1.y;
      if (!args.ContainsKey("easetype"))
        args.Add("easetype", (object) iTween.EaseType.linear);
      args["type"] = (object) "audio";
      args["method"] = (object) "to";
      iTween.Launch(target, args);
    }

    public static void AudioTo(GameObject target, float volume, float pitch, float time)
    {
      iTween.AudioTo(target, iTween.Hash((object) nameof (volume), (object) volume, (object) nameof (pitch), (object) pitch, (object) nameof (time), (object) time));
    }

    public static void AudioTo(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      if (!args.ContainsKey("easetype"))
        args.Add("easetype", (object) iTween.EaseType.linear);
      args["type"] = (object) "audio";
      args["method"] = (object) "to";
      iTween.Launch(target, args);
    }

    public static void Stab(GameObject target, AudioClip audioclip, float delay)
    {
      iTween.Stab(target, iTween.Hash((object) nameof (audioclip), (object) audioclip, (object) nameof (delay), (object) delay));
    }

    public static void Stab(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      args["type"] = (object) "stab";
      iTween.Launch(target, args);
    }

    public static void LookFrom(GameObject target, Vector3 looktarget, float time)
    {
      iTween.LookFrom(target, iTween.Hash((object) nameof (looktarget), (object) looktarget, (object) nameof (time), (object) time));
    }

    public static void LookFrom(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      Vector3 eulerAngles1 = target.transform.eulerAngles;
      if (args["looktarget"].GetType() == typeof (Transform))
        target.transform.LookAt((Transform) args["looktarget"], (Vector3?) args["up"] ?? iTween.Defaults.up);
      else if (args["looktarget"].GetType() == typeof (Vector3))
        target.transform.LookAt((Vector3) args["looktarget"], (Vector3?) args["up"] ?? iTween.Defaults.up);
      if (args.ContainsKey("axis"))
      {
        Vector3 eulerAngles2 = target.transform.eulerAngles;
        switch ((string) args["axis"])
        {
          case "x":
            eulerAngles2.y = eulerAngles1.y;
            eulerAngles2.z = eulerAngles1.z;
            break;
          case "y":
            eulerAngles2.x = eulerAngles1.x;
            eulerAngles2.z = eulerAngles1.z;
            break;
          case "z":
            eulerAngles2.x = eulerAngles1.x;
            eulerAngles2.y = eulerAngles1.y;
            break;
        }
        target.transform.eulerAngles = eulerAngles2;
      }
      args["rotation"] = (object) eulerAngles1;
      args["type"] = (object) "rotate";
      args["method"] = (object) "to";
      iTween.Launch(target, args);
    }

    public static void LookTo(GameObject target, Vector3 looktarget, float time)
    {
      iTween.LookTo(target, iTween.Hash((object) nameof (looktarget), (object) looktarget, (object) nameof (time), (object) time));
    }

    public static void LookTo(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      if (args.ContainsKey("looktarget") && args["looktarget"].GetType() == typeof (Transform))
      {
        Transform transform = (Transform) args["looktarget"];
        args["position"] = (object) new Vector3(transform.position.x, transform.position.y, transform.position.z);
        args["rotation"] = (object) new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
      }
      args["type"] = (object) "look";
      args["method"] = (object) "to";
      iTween.Launch(target, args);
    }

    public static void MoveTo(GameObject target, Vector3 position, float time)
    {
      iTween.MoveTo(target, iTween.Hash((object) nameof (position), (object) position, (object) nameof (time), (object) time));
    }

    public static void MoveTo(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      if (args.ContainsKey("position") && args["position"].GetType() == typeof (Transform))
      {
        Transform transform = (Transform) args["position"];
        args["position"] = (object) new Vector3(transform.position.x, transform.position.y, transform.position.z);
        args["rotation"] = (object) new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        args["scale"] = (object) new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
      }
      args["type"] = (object) "move";
      args["method"] = (object) "to";
      iTween.Launch(target, args);
    }

    public static void MoveFrom(GameObject target, Vector3 position, float time)
    {
      iTween.MoveFrom(target, iTween.Hash((object) nameof (position), (object) position, (object) nameof (time), (object) time));
    }

    public static void MoveFrom(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      bool flag = !args.ContainsKey("islocal") ? iTween.Defaults.isLocal : (bool) args["islocal"];
      if (args.ContainsKey("path"))
      {
        Vector3[] vector3Array;
        if (args["path"].GetType() == typeof (Vector3[]))
        {
          Vector3[] sourceArray = (Vector3[]) args["path"];
          vector3Array = new Vector3[sourceArray.Length];
          Array.Copy((Array) sourceArray, (Array) vector3Array, sourceArray.Length);
        }
        else
        {
          Transform[] transformArray = (Transform[]) args["path"];
          vector3Array = new Vector3[transformArray.Length];
          for (int index = 0; index < transformArray.Length; ++index)
            vector3Array[index] = transformArray[index].position;
        }
        if (vector3Array[vector3Array.Length - 1] != target.transform.position)
        {
          Vector3[] destinationArray = new Vector3[vector3Array.Length + 1];
          Array.Copy((Array) vector3Array, (Array) destinationArray, vector3Array.Length);
          if (flag)
          {
            destinationArray[destinationArray.Length - 1] = target.transform.localPosition;
            target.transform.localPosition = destinationArray[0];
          }
          else
          {
            destinationArray[destinationArray.Length - 1] = target.transform.position;
            target.transform.position = destinationArray[0];
          }
          args["path"] = (object) destinationArray;
        }
        else
        {
          if (flag)
            target.transform.localPosition = vector3Array[0];
          else
            target.transform.position = vector3Array[0];
          args["path"] = (object) vector3Array;
        }
      }
      else
      {
        Vector3 vector3_1;
        Vector3 vector3_2 = !flag ? (vector3_1 = target.transform.position) : (vector3_1 = target.transform.localPosition);
        if (args.ContainsKey("position"))
        {
          if (args["position"].GetType() == typeof (Transform))
            vector3_1 = ((Transform) args["position"]).position;
          else if (args["position"].GetType() == typeof (Vector3))
            vector3_1 = (Vector3) args["position"];
        }
        else
        {
          if (args.ContainsKey("x"))
            vector3_1.x = (float) args["x"];
          if (args.ContainsKey("y"))
            vector3_1.y = (float) args["y"];
          if (args.ContainsKey("z"))
            vector3_1.z = (float) args["z"];
        }
        if (flag)
          target.transform.localPosition = vector3_1;
        else
          target.transform.position = vector3_1;
        args["position"] = (object) vector3_2;
      }
      args["type"] = (object) "move";
      args["method"] = (object) "to";
      iTween.Launch(target, args);
    }

    public static void MoveAdd(GameObject target, Vector3 amount, float time)
    {
      iTween.MoveAdd(target, iTween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void MoveAdd(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      args["type"] = (object) "move";
      args["method"] = (object) "add";
      iTween.Launch(target, args);
    }

    public static void MoveBy(GameObject target, Vector3 amount, float time)
    {
      iTween.MoveBy(target, iTween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void MoveBy(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      args["type"] = (object) "move";
      args["method"] = (object) "by";
      iTween.Launch(target, args);
    }

    public static void ScaleTo(GameObject target, Vector3 scale, float time)
    {
      iTween.ScaleTo(target, iTween.Hash((object) nameof (scale), (object) scale, (object) nameof (time), (object) time));
    }

    public static void ScaleTo(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      if (args.ContainsKey("scale") && args["scale"].GetType() == typeof (Transform))
      {
        Transform transform = (Transform) args["scale"];
        args["position"] = (object) new Vector3(transform.position.x, transform.position.x, transform.position.z);
        args["rotation"] = (object) new Vector3(transform.eulerAngles.x, transform.eulerAngles.x, transform.eulerAngles.z);
        args["scale"] = (object) new Vector3(transform.localScale.x, transform.localScale.x, transform.localScale.z);
      }
      args["type"] = (object) "scale";
      args["method"] = (object) "to";
      iTween.Launch(target, args);
    }

    public static void ScaleFrom(GameObject target, Vector3 scale, float time)
    {
      iTween.ScaleFrom(target, iTween.Hash((object) nameof (scale), (object) scale, (object) nameof (time), (object) time));
    }

    public static void ScaleFrom(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      Vector3 localScale;
      Vector3 vector3 = localScale = target.transform.localScale;
      if (args.ContainsKey("scale"))
      {
        if (args["scale"].GetType() == typeof (Transform))
          localScale = ((Transform) args["scale"]).localScale;
        else if (args["scale"].GetType() == typeof (Vector3))
          localScale = (Vector3) args["scale"];
      }
      else
      {
        if (args.ContainsKey("x"))
          localScale.x = (float) args["x"];
        if (args.ContainsKey("y"))
          localScale.y = (float) args["y"];
        if (args.ContainsKey("z"))
          localScale.z = (float) args["z"];
      }
      target.transform.localScale = localScale;
      args["scale"] = (object) vector3;
      args["type"] = (object) "scale";
      args["method"] = (object) "to";
      iTween.Launch(target, args);
    }

    public static void ScaleAdd(GameObject target, Vector3 amount, float time)
    {
      iTween.ScaleAdd(target, iTween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void ScaleAdd(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      args["type"] = (object) "scale";
      args["method"] = (object) "add";
      iTween.Launch(target, args);
    }

    public static void ScaleBy(GameObject target, Vector3 amount, float time)
    {
      iTween.ScaleBy(target, iTween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void ScaleBy(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      args["type"] = (object) "scale";
      args["method"] = (object) "by";
      iTween.Launch(target, args);
    }

    public static void RotateTo(GameObject target, Vector3 rotation, float time)
    {
      iTween.RotateTo(target, iTween.Hash((object) nameof (rotation), (object) rotation, (object) nameof (time), (object) time));
    }

    public static void RotateTo(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      if (args.ContainsKey("rotation") && args["rotation"].GetType() == typeof (Transform))
      {
        Transform transform = (Transform) args["rotation"];
        args["position"] = (object) new Vector3(transform.position.x, transform.position.y, transform.position.z);
        args["rotation"] = (object) new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        args["scale"] = (object) new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
      }
      args["type"] = (object) "rotate";
      args["method"] = (object) "to";
      iTween.Launch(target, args);
    }

    public static void RotateFrom(GameObject target, Vector3 rotation, float time)
    {
      iTween.RotateFrom(target, iTween.Hash((object) nameof (rotation), (object) rotation, (object) nameof (time), (object) time));
    }

    public static void RotateFrom(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      bool flag = !args.ContainsKey("islocal") ? iTween.Defaults.isLocal : (bool) args["islocal"];
      Vector3 vector3_1;
      Vector3 vector3_2 = !flag ? (vector3_1 = target.transform.eulerAngles) : (vector3_1 = target.transform.localEulerAngles);
      if (args.ContainsKey("rotation"))
      {
        if (args["rotation"].GetType() == typeof (Transform))
          vector3_1 = ((Transform) args["rotation"]).eulerAngles;
        else if (args["rotation"].GetType() == typeof (Vector3))
          vector3_1 = (Vector3) args["rotation"];
      }
      else
      {
        if (args.ContainsKey("x"))
          vector3_1.x = (float) args["x"];
        if (args.ContainsKey("y"))
          vector3_1.y = (float) args["y"];
        if (args.ContainsKey("z"))
          vector3_1.z = (float) args["z"];
      }
      if (flag)
        target.transform.localEulerAngles = vector3_1;
      else
        target.transform.eulerAngles = vector3_1;
      args["rotation"] = (object) vector3_2;
      args["type"] = (object) "rotate";
      args["method"] = (object) "to";
      iTween.Launch(target, args);
    }

    public static void RotateAdd(GameObject target, Vector3 amount, float time)
    {
      iTween.RotateAdd(target, iTween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void RotateAdd(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      args["type"] = (object) "rotate";
      args["method"] = (object) "add";
      iTween.Launch(target, args);
    }

    public static void RotateBy(GameObject target, Vector3 amount, float time)
    {
      iTween.RotateBy(target, iTween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void RotateBy(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      args["type"] = (object) "rotate";
      args["method"] = (object) "by";
      iTween.Launch(target, args);
    }

    public static void ShakePosition(GameObject target, Vector3 amount, float time)
    {
      iTween.ShakePosition(target, iTween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void ShakePosition(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      args["type"] = (object) "shake";
      args["method"] = (object) "position";
      iTween.Launch(target, args);
    }

    public static void ShakeScale(GameObject target, Vector3 amount, float time)
    {
      iTween.ShakeScale(target, iTween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void ShakeScale(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      args["type"] = (object) "shake";
      args["method"] = (object) "scale";
      iTween.Launch(target, args);
    }

    public static void ShakeRotation(GameObject target, Vector3 amount, float time)
    {
      iTween.ShakeRotation(target, iTween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void ShakeRotation(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      args["type"] = (object) "shake";
      args["method"] = (object) "rotation";
      iTween.Launch(target, args);
    }

    public static void PunchPosition(GameObject target, Vector3 amount, float time)
    {
      iTween.PunchPosition(target, iTween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void PunchPosition(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      args["type"] = (object) "punch";
      args["method"] = (object) "position";
      args["easetype"] = (object) iTween.EaseType.punch;
      iTween.Launch(target, args);
    }

    public static void PunchRotation(GameObject target, Vector3 amount, float time)
    {
      iTween.PunchRotation(target, iTween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void PunchRotation(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      args["type"] = (object) "punch";
      args["method"] = (object) "rotation";
      args["easetype"] = (object) iTween.EaseType.punch;
      iTween.Launch(target, args);
    }

    public static void PunchScale(GameObject target, Vector3 amount, float time)
    {
      iTween.PunchScale(target, iTween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }

    public static void PunchScale(GameObject target, Dictionary<string, object> args)
    {
      args = iTween.CleanArgs(args);
      args["type"] = (object) "punch";
      args["method"] = (object) "scale";
      args["easetype"] = (object) iTween.EaseType.punch;
      iTween.Launch(target, args);
    }

    private void GenerateTargets()
    {
      switch (this.type)
      {
        case "value":
          switch (this.method)
          {
            case null:
              return;
            case "float":
              this.GenerateFloatTargets();
              this.apply = new iTween.ApplyTween(this.ApplyFloatTargets);
              return;
            case "vector2":
              this.GenerateVector2Targets();
              this.apply = new iTween.ApplyTween(this.ApplyVector2Targets);
              return;
            case "vector3":
              this.GenerateVector3Targets();
              this.apply = new iTween.ApplyTween(this.ApplyVector3Targets);
              return;
            case "color":
              this.GenerateColorTargets();
              this.apply = new iTween.ApplyTween(this.ApplyColorTargets);
              return;
            case "rect":
              this.GenerateRectTargets();
              this.apply = new iTween.ApplyTween(this.ApplyRectTargets);
              return;
            default:
              return;
          }
        case "color":
          switch (this.method)
          {
            case null:
              return;
            case "to":
              this.GenerateColorToTargets();
              this.apply = new iTween.ApplyTween(this.ApplyColorToTargets);
              return;
            default:
              return;
          }
        case "audio":
          switch (this.method)
          {
            case null:
              return;
            case "to":
              this.GenerateAudioToTargets();
              this.apply = new iTween.ApplyTween(this.ApplyAudioToTargets);
              return;
            default:
              return;
          }
        case "move":
          switch (this.method)
          {
            case null:
              return;
            case "to":
              if (this.tweenArguments.ContainsKey("path"))
              {
                this.GenerateMoveToPathTargets();
                this.apply = new iTween.ApplyTween(this.ApplyMoveToPathTargets);
                return;
              }
              this.GenerateMoveToTargets();
              this.apply = new iTween.ApplyTween(this.ApplyMoveToTargets);
              return;
            case "by":
            case "add":
              this.GenerateMoveByTargets();
              this.apply = new iTween.ApplyTween(this.ApplyMoveByTargets);
              return;
            default:
              return;
          }
        case "scale":
          switch (this.method)
          {
            case null:
              return;
            case "to":
              this.GenerateScaleToTargets();
              this.apply = new iTween.ApplyTween(this.ApplyScaleToTargets);
              return;
            case "by":
              this.GenerateScaleByTargets();
              this.apply = new iTween.ApplyTween(this.ApplyScaleToTargets);
              return;
            case "add":
              this.GenerateScaleAddTargets();
              this.apply = new iTween.ApplyTween(this.ApplyScaleToTargets);
              return;
            default:
              return;
          }
        case "rotate":
          switch (this.method)
          {
            case null:
              return;
            case "to":
              this.GenerateRotateToTargets();
              this.apply = new iTween.ApplyTween(this.ApplyRotateToTargets);
              return;
            case "add":
              this.GenerateRotateAddTargets();
              this.apply = new iTween.ApplyTween(this.ApplyRotateAddTargets);
              return;
            case "by":
              this.GenerateRotateByTargets();
              this.apply = new iTween.ApplyTween(this.ApplyRotateAddTargets);
              return;
            default:
              return;
          }
        case "shake":
          switch (this.method)
          {
            case null:
              return;
            case "position":
              this.GenerateShakePositionTargets();
              this.apply = new iTween.ApplyTween(this.ApplyShakePositionTargets);
              return;
            case "scale":
              this.GenerateShakeScaleTargets();
              this.apply = new iTween.ApplyTween(this.ApplyShakeScaleTargets);
              return;
            case "rotation":
              this.GenerateShakeRotationTargets();
              this.apply = new iTween.ApplyTween(this.ApplyShakeRotationTargets);
              return;
            default:
              return;
          }
        case "punch":
          switch (this.method)
          {
            case null:
              return;
            case "position":
              this.GeneratePunchPositionTargets();
              this.apply = new iTween.ApplyTween(this.ApplyPunchPositionTargets);
              return;
            case "rotation":
              this.GeneratePunchRotationTargets();
              this.apply = new iTween.ApplyTween(this.ApplyPunchRotationTargets);
              return;
            case "scale":
              this.GeneratePunchScaleTargets();
              this.apply = new iTween.ApplyTween(this.ApplyPunchScaleTargets);
              return;
            default:
              return;
          }
        case "look":
          switch (this.method)
          {
            case null:
              return;
            case "to":
              this.GenerateLookToTargets();
              this.apply = new iTween.ApplyTween(this.ApplyLookToTargets);
              return;
            default:
              return;
          }
        case "stab":
          this.GenerateStabTargets();
          this.apply = new iTween.ApplyTween(this.ApplyStabTargets);
          break;
      }
    }

    private void GenerateRectTargets()
    {
      this.rects = new Rectangle[3];
      this.rects[0] = (Rectangle) this.tweenArguments["from"];
      this.rects[1] = (Rectangle) this.tweenArguments["to"];
    }

    private void GenerateColorTargets()
    {
      this.colors = new Color[1, 3];
      this.colors[0, 0] = (Color) this.tweenArguments["from"];
      this.colors[0, 1] = (Color) this.tweenArguments["to"];
    }

    private void GenerateVector3Targets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = (Vector3) this.tweenArguments["from"];
      this.vector3s[1] = (Vector3) this.tweenArguments["to"];
      if (!this.tweenArguments.ContainsKey("speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments["speed"];
    }

    private void GenerateVector2Targets()
    {
      this.vector2s = new Vector2[3];
      this.vector2s[0] = (Vector2) this.tweenArguments["from"];
      this.vector2s[1] = (Vector2) this.tweenArguments["to"];
      if (!this.tweenArguments.ContainsKey("speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(new Vector3(this.vector2s[0].x, this.vector2s[0].y, 0.0f), new Vector3(this.vector2s[1].x, this.vector2s[1].y, 0.0f))) / (float) this.tweenArguments["speed"];
    }

    private void GenerateFloatTargets()
    {
      this.floats = new float[3];
      this.floats[0] = (float) this.tweenArguments["from"];
      this.floats[1] = (float) this.tweenArguments["to"];
      if (!this.tweenArguments.ContainsKey("speed"))
        return;
      this.time = Math.Abs(this.floats[0] - this.floats[1]) / (float) this.tweenArguments["speed"];
    }

    private void GenerateColorToTargets()
    {
      if (this.renderer != null)
      {
        this.colors = new Color[this.renderer.materials.Length, 3];
        for (int index = 0; index < this.renderer.materials.Length; ++index)
        {
          this.colors[index, 0] = this.renderer.materials[index].color;
          this.colors[index, 1] = this.renderer.materials[index].color;
        }
      }
      else
        this.colors = new Color[1, 3];
      if (this.tweenArguments.ContainsKey("color"))
      {
        for (int index = 0; index < this.colors.GetLength(0); ++index)
          this.colors[index, 1] = (Color) this.tweenArguments["color"];
      }
      else
      {
        if (this.tweenArguments.ContainsKey("r"))
        {
          for (int index = 0; index < this.colors.GetLength(0); ++index)
            this.colors[index, 1].r = (float) this.tweenArguments["r"];
        }
        if (this.tweenArguments.ContainsKey("g"))
        {
          for (int index = 0; index < this.colors.GetLength(0); ++index)
            this.colors[index, 1].g = (float) this.tweenArguments["g"];
        }
        if (this.tweenArguments.ContainsKey("b"))
        {
          for (int index = 0; index < this.colors.GetLength(0); ++index)
            this.colors[index, 1].b = (float) this.tweenArguments["b"];
        }
        if (this.tweenArguments.ContainsKey("a"))
        {
          for (int index = 0; index < this.colors.GetLength(0); ++index)
            this.colors[index, 1].a = (float) this.tweenArguments["a"];
        }
      }
      if (this.tweenArguments.ContainsKey("amount"))
      {
        for (int index = 0; index < this.colors.GetLength(0); ++index)
          this.colors[index, 1].a = (float) this.tweenArguments["amount"];
      }
      else
      {
        if (!this.tweenArguments.ContainsKey("alpha"))
          return;
        for (int index = 0; index < this.colors.GetLength(0); ++index)
          this.colors[index, 1].a = (float) this.tweenArguments["alpha"];
      }
    }

    private void GenerateAudioToTargets()
    {
      this.vector2s = new Vector2[3];
      if (this.tweenArguments.ContainsKey("audiosource"))
        this.audioSource = (AudioSource) this.tweenArguments["audiosource"];
      else if (this.GetComponent(typeof (AudioSource)) != null)
      {
        this.audioSource = this.audio;
      }
      else
      {
        Debug.LogError("iTween Error: AudioTo requires an AudioSource.");
        this.Dispose();
      }
      this.vector2s[0] = this.vector2s[1] = new Vector2(this.audioSource.volume, this.audioSource.pitch);
      if (this.tweenArguments.ContainsKey("volume"))
        this.vector2s[1].x = (float) this.tweenArguments["volume"];
      if (!this.tweenArguments.ContainsKey("pitch"))
        return;
      this.vector2s[1].y = (float) this.tweenArguments["pitch"];
    }

    private void GenerateStabTargets()
    {
      if (this.tweenArguments.ContainsKey("audiosource"))
        this.audioSource = (AudioSource) this.tweenArguments["audiosource"];
      else if (this.GetComponent(typeof (AudioSource)) != null)
      {
        this.audioSource = this.audio;
      }
      else
      {
        this.gameObject.AddComponent<AudioSource>(new AudioSource());
        this.audioSource = this.audio;
        this.audioSource.playOnAwake = false;
      }
      this.audioSource.clip = (AudioClip) this.tweenArguments["audioclip"];
      if (this.tweenArguments.ContainsKey("pitch"))
        this.audioSource.pitch = (float) this.tweenArguments["pitch"];
      if (this.tweenArguments.ContainsKey("volume"))
        this.audioSource.volume = (float) this.tweenArguments["volume"];
      this.time = this.audioSource.clip.length / this.audioSource.pitch;
    }

    private void GenerateLookToTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = this.transform.eulerAngles;
      if (this.tweenArguments.ContainsKey("looktarget"))
      {
        if (this.tweenArguments["looktarget"].GetType() == typeof (Transform))
          this.transform.LookAt((Transform) this.tweenArguments["looktarget"], (Vector3?) this.tweenArguments["up"] ?? iTween.Defaults.up);
        else if (this.tweenArguments["looktarget"].GetType() == typeof (Vector3))
          this.transform.LookAt((Vector3) this.tweenArguments["looktarget"], (Vector3?) this.tweenArguments["up"] ?? iTween.Defaults.up);
      }
      else
      {
        Debug.LogError("iTween Error: LookTo needs a 'looktarget' property!");
        this.Dispose();
      }
      this.vector3s[1] = this.transform.eulerAngles;
      this.transform.eulerAngles = this.vector3s[0];
      if (this.tweenArguments.ContainsKey("axis"))
      {
        switch ((string) this.tweenArguments["axis"])
        {
          case "x":
            this.vector3s[1].y = this.vector3s[0].y;
            this.vector3s[1].z = this.vector3s[0].z;
            break;
          case "y":
            this.vector3s[1].x = this.vector3s[0].x;
            this.vector3s[1].z = this.vector3s[0].z;
            break;
          case "z":
            this.vector3s[1].x = this.vector3s[0].x;
            this.vector3s[1].y = this.vector3s[0].y;
            break;
        }
      }
      this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f));
      if (!this.tweenArguments.ContainsKey("speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments["speed"];
    }

    private void GenerateMoveToPathTargets()
    {
      Vector3[] vector3Array1;
      if (this.tweenArguments["path"].GetType() == typeof (Vector3[]))
      {
        Vector3[] tweenArgument = (Vector3[]) this.tweenArguments["path"];
        if (tweenArgument.Length == 1)
        {
          Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
          this.Dispose();
        }
        vector3Array1 = new Vector3[tweenArgument.Length];
        Array.Copy((Array) tweenArgument, (Array) vector3Array1, tweenArgument.Length);
      }
      else
      {
        Transform[] tweenArgument = (Transform[]) this.tweenArguments["path"];
        if (tweenArgument.Length == 1)
        {
          Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
          this.Dispose();
        }
        vector3Array1 = new Vector3[tweenArgument.Length];
        for (int index = 0; index < tweenArgument.Length; ++index)
          vector3Array1[index] = tweenArgument[index].position;
      }
      bool flag;
      int num;
      if (this.transform.position != vector3Array1[0])
      {
        if (!this.tweenArguments.ContainsKey("movetopath") || (bool) this.tweenArguments["movetopath"])
        {
          flag = true;
          num = 3;
        }
        else
        {
          flag = false;
          num = 2;
        }
      }
      else
      {
        flag = false;
        num = 2;
      }
      this.vector3s = new Vector3[vector3Array1.Length + num];
      int destinationIndex;
      if (flag)
      {
        this.vector3s[1] = this.transform.position;
        destinationIndex = 2;
      }
      else
        destinationIndex = 1;
      Array.Copy((Array) vector3Array1, 0, (Array) this.vector3s, destinationIndex, vector3Array1.Length);
      this.vector3s[0] = this.vector3s[1] + (this.vector3s[1] - this.vector3s[2]);
      this.vector3s[this.vector3s.Length - 1] = this.vector3s[this.vector3s.Length - 2] + (this.vector3s[this.vector3s.Length - 2] - this.vector3s[this.vector3s.Length - 3]);
      if (this.vector3s[1] == this.vector3s[this.vector3s.Length - 2])
      {
        Vector3[] vector3Array2 = new Vector3[this.vector3s.Length];
        Array.Copy((Array) this.vector3s, (Array) vector3Array2, this.vector3s.Length);
        vector3Array2[0] = vector3Array2[vector3Array2.Length - 3];
        vector3Array2[vector3Array2.Length - 1] = vector3Array2[2];
        this.vector3s = new Vector3[vector3Array2.Length];
        Array.Copy((Array) vector3Array2, (Array) this.vector3s, vector3Array2.Length);
      }
      this.path = new iTween.CRSpline(this.vector3s);
      if (!this.tweenArguments.ContainsKey("speed"))
        return;
      this.time = iTween.PathLength(this.vector3s) / (float) this.tweenArguments["speed"];
    }

    private void GenerateMoveToTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = !this.isLocal ? (this.vector3s[1] = this.transform.position) : (this.vector3s[1] = this.transform.localPosition);
      if (this.tweenArguments.ContainsKey("position"))
      {
        if (this.tweenArguments["position"].GetType() == typeof (Transform))
          this.vector3s[1] = ((Transform) this.tweenArguments["position"]).position;
        else if (this.tweenArguments["position"].GetType() == typeof (Vector3))
          this.vector3s[1] = (Vector3) this.tweenArguments["position"];
      }
      else
      {
        if (this.tweenArguments.ContainsKey("x"))
          this.vector3s[1].x = (float) this.tweenArguments["x"];
        if (this.tweenArguments.ContainsKey("y"))
          this.vector3s[1].y = (float) this.tweenArguments["y"];
        if (this.tweenArguments.ContainsKey("z"))
          this.vector3s[1].z = (float) this.tweenArguments["z"];
      }
      if (this.tweenArguments.ContainsKey("orienttopath") && (bool) this.tweenArguments["orienttopath"])
        this.tweenArguments["looktarget"] = (object) this.vector3s[1];
      if (!this.tweenArguments.ContainsKey("speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments["speed"];
    }

    private void GenerateMoveByTargets()
    {
      this.vector3s = new Vector3[6];
      this.vector3s[4] = this.transform.eulerAngles;
      this.vector3s[0] = this.vector3s[1] = this.vector3s[3] = this.transform.position;
      if (this.tweenArguments.ContainsKey("amount"))
      {
        this.vector3s[1] = this.vector3s[0] + (Vector3) this.tweenArguments["amount"];
      }
      else
      {
        if (this.tweenArguments.ContainsKey("x"))
          this.vector3s[1].x = this.vector3s[0].x + (float) this.tweenArguments["x"];
        if (this.tweenArguments.ContainsKey("y"))
          this.vector3s[1].y = this.vector3s[0].y + (float) this.tweenArguments["y"];
        if (this.tweenArguments.ContainsKey("z"))
          this.vector3s[1].z = this.vector3s[0].z + (float) this.tweenArguments["z"];
      }
      this.transform.Translate(this.vector3s[1], this.space);
      this.vector3s[5] = this.transform.position;
      this.transform.position = this.vector3s[0];
      if (this.tweenArguments.ContainsKey("orienttopath") && (bool) this.tweenArguments["orienttopath"])
        this.tweenArguments["looktarget"] = (object) this.vector3s[1];
      if (!this.tweenArguments.ContainsKey("speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments["speed"];
    }

    private void GenerateScaleToTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = this.vector3s[1] = this.transform.localScale;
      if (this.tweenArguments.ContainsKey("scale"))
      {
        if (this.tweenArguments["scale"].GetType() == typeof (Transform))
          this.vector3s[1] = ((Transform) this.tweenArguments["scale"]).localScale;
        else if (this.tweenArguments["scale"].GetType() == typeof (Vector3))
          this.vector3s[1] = (Vector3) this.tweenArguments["scale"];
      }
      else
      {
        if (this.tweenArguments.ContainsKey("x"))
          this.vector3s[1].x = (float) this.tweenArguments["x"];
        if (this.tweenArguments.ContainsKey("y"))
          this.vector3s[1].y = (float) this.tweenArguments["y"];
        if (this.tweenArguments.ContainsKey("z"))
          this.vector3s[1].z = (float) this.tweenArguments["z"];
      }
      if (!this.tweenArguments.ContainsKey("speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments["speed"];
    }

    private void GenerateScaleByTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = this.vector3s[1] = this.transform.localScale;
      if (this.tweenArguments.ContainsKey("amount"))
      {
        this.vector3s[1] = Vector3.Scale(this.vector3s[1], (Vector3) this.tweenArguments["amount"]);
      }
      else
      {
        if (this.tweenArguments.ContainsKey("x"))
          this.vector3s[1].x *= (float) this.tweenArguments["x"];
        if (this.tweenArguments.ContainsKey("y"))
          this.vector3s[1].y *= (float) this.tweenArguments["y"];
        if (this.tweenArguments.ContainsKey("z"))
          this.vector3s[1].z *= (float) this.tweenArguments["z"];
      }
      if (!this.tweenArguments.ContainsKey("speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments["speed"];
    }

    private void GenerateScaleAddTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = this.vector3s[1] = this.transform.localScale;
      if (this.tweenArguments.ContainsKey("amount"))
      {
        this.vector3s[1] += (Vector3) this.tweenArguments["amount"];
      }
      else
      {
        if (this.tweenArguments.ContainsKey("x"))
          this.vector3s[1].x += (float) this.tweenArguments["x"];
        if (this.tweenArguments.ContainsKey("y"))
          this.vector3s[1].y += (float) this.tweenArguments["y"];
        if (this.tweenArguments.ContainsKey("z"))
          this.vector3s[1].z += (float) this.tweenArguments["z"];
      }
      if (!this.tweenArguments.ContainsKey("speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments["speed"];
    }

    private void GenerateRotateToTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = !this.isLocal ? (this.vector3s[1] = this.transform.eulerAngles) : (this.vector3s[1] = this.transform.localEulerAngles);
      if (this.tweenArguments.ContainsKey("rotation"))
      {
        if (this.tweenArguments["rotation"].GetType() == typeof (Transform))
          this.vector3s[1] = ((Transform) this.tweenArguments["rotation"]).eulerAngles;
        else if (this.tweenArguments["rotation"].GetType() == typeof (Vector3))
          this.vector3s[1] = (Vector3) this.tweenArguments["rotation"];
      }
      else
      {
        if (this.tweenArguments.ContainsKey("x"))
          this.vector3s[1].x = (float) this.tweenArguments["x"];
        if (this.tweenArguments.ContainsKey("y"))
          this.vector3s[1].y = (float) this.tweenArguments["y"];
        if (this.tweenArguments.ContainsKey("z"))
          this.vector3s[1].z = (float) this.tweenArguments["z"];
      }
      this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].z, this.vector3s[1].z, 1f));
      if (!this.tweenArguments.ContainsKey("speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments["speed"];
    }

    private void GenerateRotateAddTargets()
    {
      this.vector3s = new Vector3[5];
      this.vector3s[0] = this.vector3s[1] = this.vector3s[3] = this.transform.eulerAngles;
      if (this.tweenArguments.ContainsKey("amount"))
      {
        this.vector3s[1] += (Vector3) this.tweenArguments["amount"];
      }
      else
      {
        if (this.tweenArguments.ContainsKey("x"))
          this.vector3s[1].x += (float) this.tweenArguments["x"];
        if (this.tweenArguments.ContainsKey("y"))
          this.vector3s[1].y += (float) this.tweenArguments["y"];
        if (this.tweenArguments.ContainsKey("z"))
          this.vector3s[1].z += (float) this.tweenArguments["z"];
      }
      if (!this.tweenArguments.ContainsKey("speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments["speed"];
    }

    private void GenerateRotateByTargets()
    {
      this.vector3s = new Vector3[4];
      this.vector3s[0] = this.vector3s[1] = this.vector3s[3] = this.transform.eulerAngles;
      if (this.tweenArguments.ContainsKey("amount"))
      {
        this.vector3s[1] += Vector3.Scale((Vector3) this.tweenArguments["amount"], new Vector3(360f, 360f, 360f));
      }
      else
      {
        if (this.tweenArguments.ContainsKey("x"))
          this.vector3s[1].x += 360f * (float) this.tweenArguments["x"];
        if (this.tweenArguments.ContainsKey("y"))
          this.vector3s[1].y += 360f * (float) this.tweenArguments["y"];
        if (this.tweenArguments.ContainsKey("z"))
          this.vector3s[1].z += 360f * (float) this.tweenArguments["z"];
      }
      if (!this.tweenArguments.ContainsKey("speed"))
        return;
      this.time = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1])) / (float) this.tweenArguments["speed"];
    }

    private void GenerateShakePositionTargets()
    {
      this.vector3s = new Vector3[4];
      this.vector3s[3] = this.transform.eulerAngles;
      this.vector3s[0] = new Vector3((Microsoft.Xna.Framework.Vector3) this.transform.position);
      if (this.tweenArguments.ContainsKey("amount"))
      {
        this.vector3s[1] = (Vector3) this.tweenArguments["amount"];
      }
      else
      {
        if (this.tweenArguments.ContainsKey("x"))
          this.vector3s[1].x = (float) this.tweenArguments["x"];
        if (this.tweenArguments.ContainsKey("y"))
          this.vector3s[1].y = (float) this.tweenArguments["y"];
        if (!this.tweenArguments.ContainsKey("z"))
          return;
        this.vector3s[1].z = (float) this.tweenArguments["z"];
      }
    }

    private void GenerateShakeScaleTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = new Vector3((Microsoft.Xna.Framework.Vector3) this.transform.localScale);
      if (this.tweenArguments.ContainsKey("amount"))
      {
        this.vector3s[1] = (Vector3) this.tweenArguments["amount"];
      }
      else
      {
        if (this.tweenArguments.ContainsKey("x"))
          this.vector3s[1].x = (float) this.tweenArguments["x"];
        if (this.tweenArguments.ContainsKey("y"))
          this.vector3s[1].y = (float) this.tweenArguments["y"];
        if (!this.tweenArguments.ContainsKey("z"))
          return;
        this.vector3s[1].z = (float) this.tweenArguments["z"];
      }
    }

    private void GenerateShakeRotationTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = new Vector3((Microsoft.Xna.Framework.Vector3) this.transform.eulerAngles);
      if (this.tweenArguments.ContainsKey("amount"))
      {
        this.vector3s[1] = (Vector3) this.tweenArguments["amount"];
      }
      else
      {
        if (this.tweenArguments.ContainsKey("x"))
          this.vector3s[1].x = (float) this.tweenArguments["x"];
        if (this.tweenArguments.ContainsKey("y"))
          this.vector3s[1].y = (float) this.tweenArguments["y"];
        if (!this.tweenArguments.ContainsKey("z"))
          return;
        this.vector3s[1].z = (float) this.tweenArguments["z"];
      }
    }

    private void GeneratePunchPositionTargets()
    {
      this.vector3s = new Vector3[5];
      this.vector3s[4] = this.transform.eulerAngles;
      this.vector3s[0] = this.transform.position;
      this.vector3s[1] = this.vector3s[3] = Vector3.zero;
      if (this.tweenArguments.ContainsKey("amount"))
      {
        this.vector3s[1] = (Vector3) this.tweenArguments["amount"];
      }
      else
      {
        if (this.tweenArguments.ContainsKey("x"))
          this.vector3s[1].x = (float) this.tweenArguments["x"];
        if (this.tweenArguments.ContainsKey("y"))
          this.vector3s[1].y = (float) this.tweenArguments["y"];
        if (!this.tweenArguments.ContainsKey("z"))
          return;
        this.vector3s[1].z = (float) this.tweenArguments["z"];
      }
    }

    private void GeneratePunchRotationTargets()
    {
      this.vector3s = new Vector3[4];
      this.vector3s[0] = this.transform.eulerAngles;
      this.vector3s[1] = this.vector3s[3] = Vector3.zero;
      if (this.tweenArguments.ContainsKey("amount"))
      {
        this.vector3s[1] = (Vector3) this.tweenArguments["amount"];
      }
      else
      {
        if (this.tweenArguments.ContainsKey("x"))
          this.vector3s[1].x = (float) this.tweenArguments["x"];
        if (this.tweenArguments.ContainsKey("y"))
          this.vector3s[1].y = (float) this.tweenArguments["y"];
        if (!this.tweenArguments.ContainsKey("z"))
          return;
        this.vector3s[1].z = (float) this.tweenArguments["z"];
      }
    }

    private void GeneratePunchScaleTargets()
    {
      this.vector3s = new Vector3[3];
      this.vector3s[0] = this.transform.localScale;
      this.vector3s[1] = Vector3.zero;
      if (this.tweenArguments.ContainsKey("amount"))
      {
        this.vector3s[1] = (Vector3) this.tweenArguments["amount"];
      }
      else
      {
        if (this.tweenArguments.ContainsKey("x"))
          this.vector3s[1].x = (float) this.tweenArguments["x"];
        if (this.tweenArguments.ContainsKey("y"))
          this.vector3s[1].y = (float) this.tweenArguments["y"];
        if (!this.tweenArguments.ContainsKey("z"))
          return;
        this.vector3s[1].z = (float) this.tweenArguments["z"];
      }
    }

    private void ApplyRectTargets()
    {
      this.rects[2].X = (int) this.ease((float) this.rects[0].X, (float) this.rects[1].X, this.percentage);
      this.rects[2].Y = (int) this.ease((float) this.rects[0].Y, (float) this.rects[1].Y, this.percentage);
      this.rects[2].Width = (int) this.ease((float) this.rects[0].Width, (float) this.rects[1].Width, this.percentage);
      this.rects[2].Height = (int) this.ease((float) this.rects[0].Height, (float) this.rects[1].Height, this.percentage);
      this.tweenArguments["onupdateparams"] = (object) this.rects[2];
      if ((double) this.percentage != 1.0)
        return;
      this.tweenArguments["onupdateparams"] = (object) this.rects[1];
    }

    private void ApplyColorTargets()
    {
      this.colors[0, 2].r = this.ease(this.colors[0, 0].r, this.colors[0, 1].r, this.percentage);
      this.colors[0, 2].g = this.ease(this.colors[0, 0].g, this.colors[0, 1].g, this.percentage);
      this.colors[0, 2].b = this.ease(this.colors[0, 0].b, this.colors[0, 1].b, this.percentage);
      this.colors[0, 2].a = this.ease(this.colors[0, 0].a, this.colors[0, 1].a, this.percentage);
      this.tweenArguments["onupdateparams"] = (object) this.colors[0, 2];
      if ((double) this.percentage != 1.0)
        return;
      this.tweenArguments["onupdateparams"] = (object) this.colors[0, 1];
    }

    private void ApplyVector3Targets()
    {
      this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
      this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
      this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
      this.tweenArguments["onupdateparams"] = (object) this.vector3s[2];
      if ((double) this.percentage != 1.0)
        return;
      this.tweenArguments["onupdateparams"] = (object) this.vector3s[1];
    }

    private void ApplyVector2Targets()
    {
      this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
      this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
      this.tweenArguments["onupdateparams"] = (object) this.vector2s[2];
      if ((double) this.percentage != 1.0)
        return;
      this.tweenArguments["onupdateparams"] = (object) this.vector2s[1];
    }

    private void ApplyFloatTargets()
    {
      this.floats[2] = this.ease(this.floats[0], this.floats[1], this.percentage);
      this.tweenArguments["onupdateparams"] = (object) this.floats[2];
      if ((double) this.percentage != 1.0)
        return;
      this.tweenArguments["onupdateparams"] = (object) this.floats[1];
    }

    private void ApplyColorToTargets()
    {
      for (int index = 0; index < this.colors.GetLength(0); ++index)
      {
        this.colors[index, 2].r = this.ease(this.colors[index, 0].r, this.colors[index, 1].r, this.percentage);
        this.colors[index, 2].g = this.ease(this.colors[index, 0].g, this.colors[index, 1].g, this.percentage);
        this.colors[index, 2].b = this.ease(this.colors[index, 0].b, this.colors[index, 1].b, this.percentage);
        this.colors[index, 2].a = this.ease(this.colors[index, 0].a, this.colors[index, 1].a, this.percentage);
      }
      if (this.renderer != null)
      {
        for (int index = 0; index < this.colors.GetLength(0); ++index)
          this.renderer.materials[index].SetColor(this.namedcolorvalue.ToString(), this.colors[index, 2]);
      }
      if ((double) this.percentage != 1.0 || this.renderer == null)
        return;
      for (int index = 0; index < this.colors.GetLength(0); ++index)
        this.renderer.materials[index].SetColor(this.namedcolorvalue.ToString(), this.colors[index, 1]);
    }

    private void ApplyAudioToTargets()
    {
      this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
      this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
      this.audioSource.volume = this.vector2s[2].x;
      this.audioSource.pitch = this.vector2s[2].y;
      if ((double) this.percentage != 1.0)
        return;
      this.audioSource.volume = this.vector2s[1].x;
      this.audioSource.pitch = this.vector2s[1].y;
    }

    private void ApplyStabTargets()
    {
    }

    private void ApplyMoveToPathTargets()
    {
      this.preUpdate = this.transform.position;
      float num = this.ease(0.0f, 1f, this.percentage);
      if (this.isLocal)
        this.transform.localPosition = this.path.Interp(Mathf.Clamp(num, 0.0f, 1f));
      else
        this.transform.position = this.path.Interp(Mathf.Clamp(num, 0.0f, 1f));
      if (this.tweenArguments.ContainsKey("orienttopath") && (bool) this.tweenArguments["orienttopath"])
        this.tweenArguments["looktarget"] = (object) this.path.Interp(Mathf.Clamp(this.ease(0.0f, 1f, Mathf.Min(1f, this.percentage + (!this.tweenArguments.ContainsKey("lookahead") ? iTween.Defaults.lookAhead : (float) this.tweenArguments["lookahead"]))), 0.0f, 1f));
      this.postUpdate = this.transform.position;
      if (!this.physics)
        return;
      this.transform.position = this.preUpdate;
      this.rigidbody.MovePosition(this.postUpdate);
    }

    private void ApplyMoveToTargets()
    {
      this.preUpdate = this.transform.position;
      this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
      this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
      this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
      if (this.isLocal)
        this.transform.localPosition = this.vector3s[2];
      else
        this.transform.position = this.vector3s[2];
      if ((double) this.percentage == 1.0)
      {
        if (this.isLocal)
          this.transform.localPosition = this.vector3s[1];
        else
          this.transform.position = this.vector3s[1];
      }
      this.postUpdate = this.transform.position;
      if (!this.physics)
        return;
      this.transform.position = this.preUpdate;
      this.rigidbody.MovePosition(this.postUpdate);
    }

    private void ApplyMoveByTargets()
    {
      this.preUpdate = this.transform.position;
      Vector3 vector3 = new Vector3();
      if (this.tweenArguments.ContainsKey("looktarget"))
      {
        vector3 = this.transform.eulerAngles;
        this.transform.eulerAngles = this.vector3s[4];
      }
      this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
      this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
      this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
      this.transform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
      this.vector3s[3] = this.vector3s[2];
      if (this.tweenArguments.ContainsKey("looktarget"))
        this.transform.eulerAngles = vector3;
      this.postUpdate = this.transform.position;
      if (!this.physics)
        return;
      this.transform.position = this.preUpdate;
      this.rigidbody.MovePosition(this.postUpdate);
    }

    private void ApplyScaleToTargets()
    {
      this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
      this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
      this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
      this.transform.localScale = this.vector3s[2];
      if ((double) this.percentage != 1.0)
        return;
      this.transform.localScale = this.vector3s[1];
    }

    private void ApplyLookToTargets()
    {
      this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
      this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
      this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
      if (this.isLocal)
        this.transform.localRotation = Quaternion.Euler(this.vector3s[2]);
      else
        this.transform.rotation = Quaternion.Euler(this.vector3s[2]);
    }

    private void ApplyRotateToTargets()
    {
      this.preUpdate = this.transform.eulerAngles;
      this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
      this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
      this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
      if (this.isLocal)
        this.transform.localRotation = Quaternion.Euler(this.vector3s[2]);
      else
        this.transform.rotation = Quaternion.Euler(this.vector3s[2]);
      if ((double) this.percentage == 1.0)
      {
        if (this.isLocal)
          this.transform.localRotation = Quaternion.Euler(this.vector3s[1]);
        else
          this.transform.rotation = Quaternion.Euler(this.vector3s[1]);
      }
      this.postUpdate = this.transform.eulerAngles;
      if (!this.physics)
        return;
      this.transform.eulerAngles = this.preUpdate;
      this.rigidbody.MoveRotation(Quaternion.Euler(this.postUpdate));
    }

    private void ApplyRotateAddTargets()
    {
      this.preUpdate = this.transform.eulerAngles;
      this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
      this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
      this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
      this.transform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
      this.vector3s[3] = this.vector3s[2];
      this.postUpdate = this.transform.eulerAngles;
      if (!this.physics)
        return;
      this.transform.eulerAngles = this.preUpdate;
      this.rigidbody.MoveRotation(Quaternion.Euler(this.postUpdate));
    }

    private void ApplyShakePositionTargets()
    {
      if ((double) Time.timeScale == 0.0)
        return;
      this.preUpdate = this.transform.position;
      Vector3 vector3 = new Vector3();
      if (this.tweenArguments.ContainsKey("looktarget"))
      {
        vector3 = this.transform.eulerAngles;
        this.transform.eulerAngles = this.vector3s[3];
      }
      if ((double) this.percentage == 0.0)
        this.transform.Translate(this.vector3s[1], this.space);
      this.transform.position = this.vector3s[0];
      float num = 1f - this.percentage;
      this.vector3s[2].x = Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
      this.vector3s[2].y = Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
      this.vector3s[2].z = Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
      this.transform.Translate(this.vector3s[2], this.space);
      if (this.tweenArguments.ContainsKey("looktarget"))
        this.transform.eulerAngles = vector3;
      this.postUpdate = this.transform.position;
      if (!this.physics)
        return;
      this.transform.position = this.preUpdate;
      this.rigidbody.MovePosition(this.postUpdate);
    }

    private void ApplyShakeScaleTargets()
    {
      if ((double) Time.timeScale == 0.0)
        return;
      if ((double) this.percentage == 0.0)
        this.transform.localScale = this.vector3s[1];
      this.transform.localScale = this.vector3s[0];
      float num = 1f - this.percentage;
      this.vector3s[2].x = Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
      this.vector3s[2].y = Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
      this.vector3s[2].z = Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
      this.transform.localScale += this.vector3s[2];
    }

    private void ApplyShakeRotationTargets()
    {
      if ((double) Time.timeScale == 0.0)
        return;
      this.preUpdate = this.transform.eulerAngles;
      if ((double) this.percentage == 0.0)
        this.transform.Rotate(this.vector3s[1], this.space);
      this.transform.eulerAngles = this.vector3s[0];
      float num = 1f - this.percentage;
      this.vector3s[2].x = Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
      this.vector3s[2].y = Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
      this.vector3s[2].z = Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
      this.transform.Rotate(this.vector3s[2], this.space);
      this.postUpdate = this.transform.eulerAngles;
      if (!this.physics)
        return;
      this.transform.eulerAngles = this.preUpdate;
      this.rigidbody.MoveRotation(Quaternion.Euler(this.postUpdate));
    }

    private void ApplyPunchPositionTargets()
    {
      this.preUpdate = this.transform.position;
      Vector3 vector3 = new Vector3();
      if (this.tweenArguments.ContainsKey("looktarget"))
      {
        vector3 = this.transform.eulerAngles;
        this.transform.eulerAngles = this.vector3s[4];
      }
      if ((double) this.vector3s[1].x > 0.0)
        this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
      else if ((double) this.vector3s[1].x < 0.0)
        this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
      if ((double) this.vector3s[1].y > 0.0)
        this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
      else if ((double) this.vector3s[1].y < 0.0)
        this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
      if ((double) this.vector3s[1].z > 0.0)
        this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
      else if ((double) this.vector3s[1].z < 0.0)
        this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
      this.transform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
      this.vector3s[3] = this.vector3s[2];
      if (this.tweenArguments.ContainsKey("looktarget"))
        this.transform.eulerAngles = vector3;
      this.postUpdate = this.transform.position;
      if (!this.physics)
        return;
      this.transform.position = this.preUpdate;
      this.rigidbody.MovePosition(this.postUpdate);
    }

    private void ApplyPunchRotationTargets()
    {
      this.preUpdate = this.transform.eulerAngles;
      if ((double) this.vector3s[1].x > 0.0)
        this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
      else if ((double) this.vector3s[1].x < 0.0)
        this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
      if ((double) this.vector3s[1].y > 0.0)
        this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
      else if ((double) this.vector3s[1].y < 0.0)
        this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
      if ((double) this.vector3s[1].z > 0.0)
        this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
      else if ((double) this.vector3s[1].z < 0.0)
        this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
      this.transform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
      this.vector3s[3] = this.vector3s[2];
      this.postUpdate = this.transform.eulerAngles;
      if (!this.physics)
        return;
      this.transform.eulerAngles = this.preUpdate;
      this.rigidbody.MoveRotation(Quaternion.Euler(this.postUpdate));
    }

    private void ApplyPunchScaleTargets()
    {
      if ((double) this.vector3s[1].x > 0.0)
        this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
      else if ((double) this.vector3s[1].x < 0.0)
        this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
      if ((double) this.vector3s[1].y > 0.0)
        this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
      else if ((double) this.vector3s[1].y < 0.0)
        this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
      if ((double) this.vector3s[1].z > 0.0)
        this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
      else if ((double) this.vector3s[1].z < 0.0)
        this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
      this.transform.localScale = this.vector3s[0] + this.vector3s[2];
    }

    private void TweenStart()
    {
      this.CallBack("onstart");
      if (!this.loop)
      {
        this.ConflictCheck();
        this.GenerateTargets();
      }
      if (this.type == "stab")
        this.audioSource.PlayOneShot(this.audioSource.clip);
      if (this.type == "move" || this.type == "scale" || this.type == "rotate" || this.type == "punch" || this.type == "shake" || this.type == "curve" || this.type == "look")
        this.EnableKinematic();
      this.isRunning = true;
    }

    private void TweenUpdate()
    {
      if (this.isDelayed)
      {
        if (this.useRealTime)
          this.runningTime += Time.realtimeSinceStartup - this.lastRealTime;
        else
          this.runningTime += Time.deltaTime;
        if ((double) this.runningTime <= (double) this.delay)
          return;
        this.isDelayed = false;
        this.runningTime = 0.0f;
      }
      else
      {
        this.apply();
        this.CallBack("onupdate");
        this.UpdatePercentage();
      }
    }

    private void TweenComplete()
    {
      this.isRunning = false;
      this.percentage = (double) this.percentage <= 0.5 ? 0.0f : 1f;
      this.apply();
      if (this.type == "value")
        this.CallBack("onupdate");
      if (this.loopType == iTween.LoopType.none)
        this.Dispose();
      else
        this.TweenLoop();
      this.CallBack("oncomplete");
    }

    private void TweenLoop()
    {
      this.DisableKinematic();
      switch (this.loopType)
      {
        case iTween.LoopType.loop:
          this.percentage = 0.0f;
          this.runningTime = 0.0f;
          this.apply();
          break;
        case iTween.LoopType.pingPong:
          this.reverse = !this.reverse;
          this.runningTime = 0.0f;
          break;
      }
    }

    public static Vector3 Vector3Update(Vector3 currentValue, Vector3 targetValue, float speed)
    {
      Vector3 vector3 = targetValue - currentValue;
      currentValue += vector3 * speed * Time.deltaTime;
      return currentValue;
    }

    public static Vector2 Vector2Update(Vector2 currentValue, Vector2 targetValue, float speed)
    {
      Vector2 vector2 = targetValue - currentValue;
      currentValue += vector2 * speed * Time.deltaTime;
      return currentValue;
    }

    public static float FloatUpdate(float currentValue, float targetValue, float speed)
    {
      float num = targetValue - currentValue;
      currentValue += num * speed * Time.deltaTime;
      return currentValue;
    }

    public static void FadeUpdate(GameObject target, Dictionary<string, object> args)
    {
      args["a"] = args["alpha"];
      iTween.ColorUpdate(target, args);
    }

    public static void FadeUpdate(GameObject target, float alpha, float time)
    {
      iTween.FadeUpdate(target, iTween.Hash((object) nameof (alpha), (object) alpha, (object) nameof (time), (object) time));
    }

    public static void ColorUpdate(GameObject target, Dictionary<string, object> args)
    {
      iTween.CleanArgs(args);
      Color[] colorArray = new Color[4];
      if (!args.ContainsKey("includechildren") || (bool) args["includechildren"])
      {
        foreach (Component component in target.transform)
          iTween.ColorUpdate(component.gameObject, args);
      }
      float smoothTime = !args.ContainsKey("time") ? iTween.Defaults.updateTime : (float) args["time"] * iTween.Defaults.updateTimePercentage;
      if (target.renderer != null)
        colorArray[0] = colorArray[1] = target.renderer.material.color;
      if (args.ContainsKey("color"))
      {
        colorArray[1] = (Color) args["color"];
      }
      else
      {
        if (args.ContainsKey("r"))
          colorArray[1].r = (float) args["r"];
        if (args.ContainsKey("g"))
          colorArray[1].g = (float) args["g"];
        if (args.ContainsKey("b"))
          colorArray[1].b = (float) args["b"];
        if (args.ContainsKey("a"))
          colorArray[1].a = (float) args["a"];
      }
      colorArray[3].r = Mathf.SmoothDamp(colorArray[0].r, colorArray[1].r, ref colorArray[2].r, smoothTime);
      colorArray[3].g = Mathf.SmoothDamp(colorArray[0].g, colorArray[1].g, ref colorArray[2].g, smoothTime);
      colorArray[3].b = Mathf.SmoothDamp(colorArray[0].b, colorArray[1].b, ref colorArray[2].b, smoothTime);
      colorArray[3].a = Mathf.SmoothDamp(colorArray[0].a, colorArray[1].a, ref colorArray[2].a, smoothTime);
      if (target.renderer == null)
        return;
      target.renderer.material.color = colorArray[3];
    }

    public static void ColorUpdate(GameObject target, Color color, float time)
    {
      iTween.ColorUpdate(target, iTween.Hash((object) nameof (color), (object) color, (object) nameof (time), (object) time));
    }

    public static void AudioUpdate(GameObject target, Dictionary<string, object> args)
    {
      iTween.CleanArgs(args);
      Vector2[] vector2Array = new Vector2[4];
      float smoothTime = !args.ContainsKey("time") ? iTween.Defaults.updateTime : (float) args["time"] * iTween.Defaults.updateTimePercentage;
      AudioSource audio;
      if (args.ContainsKey("audiosource"))
        audio = (AudioSource) args["audiosource"];
      else if (target.GetComponent(typeof (AudioSource)) != null)
      {
        audio = target.audio;
      }
      else
      {
        Debug.LogError("iTween Error: AudioUpdate requires an AudioSource.");
        return;
      }
      vector2Array[0] = vector2Array[1] = new Vector2(audio.volume, audio.pitch);
      if (args.ContainsKey("volume"))
        vector2Array[1].x = (float) args["volume"];
      if (args.ContainsKey("pitch"))
        vector2Array[1].y = (float) args["pitch"];
      vector2Array[3].x = Mathf.SmoothDampAngle(vector2Array[0].x, vector2Array[1].x, ref vector2Array[2].x, smoothTime);
      vector2Array[3].y = Mathf.SmoothDampAngle(vector2Array[0].y, vector2Array[1].y, ref vector2Array[2].y, smoothTime);
      audio.volume = vector2Array[3].x;
      audio.pitch = vector2Array[3].y;
    }

    public static void AudioUpdate(GameObject target, float volume, float pitch, float time)
    {
      iTween.AudioUpdate(target, iTween.Hash((object) nameof (volume), (object) volume, (object) nameof (pitch), (object) pitch, (object) nameof (time), (object) time));
    }

    public static void RotateUpdate(GameObject target, Dictionary<string, object> args)
    {
      iTween.CleanArgs(args);
      Vector3[] vector3Array = new Vector3[4];
      Vector3 eulerAngles1 = target.transform.eulerAngles;
      float smoothTime = !args.ContainsKey("time") ? iTween.Defaults.updateTime : (float) args["time"] * iTween.Defaults.updateTimePercentage;
      bool flag = !args.ContainsKey("islocal") ? iTween.Defaults.isLocal : (bool) args["islocal"];
      vector3Array[0] = !flag ? target.transform.eulerAngles : target.transform.localEulerAngles;
      if (args.ContainsKey("rotation"))
      {
        if (args["rotation"].GetType() == typeof (Transform))
        {
          Transform transform = (Transform) args["rotation"];
          vector3Array[1] = transform.eulerAngles;
        }
        else if (args["rotation"].GetType() == typeof (Vector3))
          vector3Array[1] = (Vector3) args["rotation"];
      }
      vector3Array[3].x = Mathf.SmoothDampAngle(vector3Array[0].x, vector3Array[1].x, ref vector3Array[2].x, smoothTime);
      vector3Array[3].y = Mathf.SmoothDampAngle(vector3Array[0].y, vector3Array[1].y, ref vector3Array[2].y, smoothTime);
      vector3Array[3].z = Mathf.SmoothDampAngle(vector3Array[0].z, vector3Array[1].z, ref vector3Array[2].z, smoothTime);
      if (flag)
        target.transform.localEulerAngles = vector3Array[3];
      else
        target.transform.eulerAngles = vector3Array[3];
      if (target.rigidbody == null)
        return;
      Vector3 eulerAngles2 = target.transform.eulerAngles;
      target.transform.eulerAngles = eulerAngles1;
      target.rigidbody.MoveRotation(Quaternion.Euler(eulerAngles2));
    }

    public static void RotateUpdate(GameObject target, Vector3 rotation, float time)
    {
      iTween.RotateUpdate(target, iTween.Hash((object) nameof (rotation), (object) rotation, (object) nameof (time), (object) time));
    }

    public static void ScaleUpdate(GameObject target, Dictionary<string, object> args)
    {
      iTween.CleanArgs(args);
      Vector3[] vector3Array = new Vector3[4];
      float smoothTime = !args.ContainsKey("time") ? iTween.Defaults.updateTime : (float) args["time"] * iTween.Defaults.updateTimePercentage;
      vector3Array[0] = vector3Array[1] = target.transform.localScale;
      if (args.ContainsKey("scale"))
      {
        if (args["scale"].GetType() == typeof (Transform))
        {
          Transform transform = (Transform) args["scale"];
          vector3Array[1] = transform.localScale;
        }
        else if (args["scale"].GetType() == typeof (Vector3))
          vector3Array[1] = (Vector3) args["scale"];
      }
      else
      {
        if (args.ContainsKey("x"))
          vector3Array[1].x = (float) args["x"];
        if (args.ContainsKey("y"))
          vector3Array[1].y = (float) args["y"];
        if (args.ContainsKey("z"))
          vector3Array[1].z = (float) args["z"];
      }
      vector3Array[3].x = Mathf.SmoothDamp(vector3Array[0].x, vector3Array[1].x, ref vector3Array[2].x, smoothTime);
      vector3Array[3].y = Mathf.SmoothDamp(vector3Array[0].y, vector3Array[1].y, ref vector3Array[2].y, smoothTime);
      vector3Array[3].z = Mathf.SmoothDamp(vector3Array[0].z, vector3Array[1].z, ref vector3Array[2].z, smoothTime);
      target.transform.localScale = vector3Array[3];
    }

    public static void ScaleUpdate(GameObject target, Vector3 scale, float time)
    {
      iTween.ScaleUpdate(target, iTween.Hash((object) nameof (scale), (object) scale, (object) nameof (time), (object) time));
    }

    public static void MoveUpdate(GameObject target, Dictionary<string, object> args)
    {
      iTween.CleanArgs(args);
      Vector3[] vector3Array = new Vector3[4];
      Vector3 position1 = target.transform.position;
      float smoothTime = !args.ContainsKey("time") ? iTween.Defaults.updateTime : (float) args["time"] * iTween.Defaults.updateTimePercentage;
      bool flag = !args.ContainsKey("islocal") ? iTween.Defaults.isLocal : (bool) args["islocal"];
      vector3Array[0] = !flag ? (vector3Array[1] = target.transform.position) : (vector3Array[1] = target.transform.localPosition);
      if (args.ContainsKey("position"))
      {
        if (args["position"].GetType() == typeof (Transform))
        {
          Transform transform = (Transform) args["position"];
          vector3Array[1] = transform.position;
        }
        else if (args["position"].GetType() == typeof (Vector3))
          vector3Array[1] = (Vector3) args["position"];
      }
      else
      {
        if (args.ContainsKey("x"))
          vector3Array[1].x = (float) args["x"];
        if (args.ContainsKey("y"))
          vector3Array[1].y = (float) args["y"];
        if (args.ContainsKey("z"))
          vector3Array[1].z = (float) args["z"];
      }
      vector3Array[3].x = Mathf.SmoothDamp(vector3Array[0].x, vector3Array[1].x, ref vector3Array[2].x, smoothTime);
      vector3Array[3].y = Mathf.SmoothDamp(vector3Array[0].y, vector3Array[1].y, ref vector3Array[2].y, smoothTime);
      vector3Array[3].z = Mathf.SmoothDamp(vector3Array[0].z, vector3Array[1].z, ref vector3Array[2].z, smoothTime);
      if (args.ContainsKey("orienttopath") && (bool) args["orienttopath"])
        args["looktarget"] = (object) vector3Array[3];
      if (args.ContainsKey("looktarget"))
        iTween.LookUpdate(target, args);
      if (flag)
        target.transform.localPosition = vector3Array[3];
      else
        target.transform.position = vector3Array[3];
      if (target.rigidbody == null)
        return;
      Vector3 position2 = target.transform.position;
      target.transform.position = position1;
      target.rigidbody.MovePosition(position2);
    }

    public static void MoveUpdate(GameObject target, Vector3 position, float time)
    {
      iTween.MoveUpdate(target, iTween.Hash((object) nameof (position), (object) position, (object) nameof (time), (object) time));
    }

    public static void LookUpdate(GameObject target, Dictionary<string, object> args)
    {
      iTween.CleanArgs(args);
      Vector3[] vector3Array = new Vector3[5];
      float smoothTime = !args.ContainsKey("looktime") ? (!args.ContainsKey("time") ? iTween.Defaults.updateTime : (float) args["time"] / 2f * iTween.Defaults.updateTimePercentage) : (float) args["looktime"] * iTween.Defaults.updateTimePercentage;
      vector3Array[0] = target.transform.eulerAngles;
      if (args.ContainsKey("looktarget"))
      {
        if (args["looktarget"].GetType() == typeof (Transform))
          target.transform.LookAt((Transform) args["looktarget"], (Vector3?) args["up"] ?? iTween.Defaults.up);
        else if (args["looktarget"].GetType() == typeof (Vector3))
          target.transform.LookAt((Vector3) args["looktarget"], (Vector3?) args["up"] ?? iTween.Defaults.up);
        vector3Array[1] = target.transform.eulerAngles;
        target.transform.eulerAngles = vector3Array[0];
        vector3Array[3].x = Mathf.SmoothDampAngle(vector3Array[0].x, vector3Array[1].x, ref vector3Array[2].x, smoothTime);
        vector3Array[3].y = Mathf.SmoothDampAngle(vector3Array[0].y, vector3Array[1].y, ref vector3Array[2].y, smoothTime);
        vector3Array[3].z = Mathf.SmoothDampAngle(vector3Array[0].z, vector3Array[1].z, ref vector3Array[2].z, smoothTime);
        target.transform.eulerAngles = vector3Array[3];
        if (!args.ContainsKey("axis"))
          return;
        vector3Array[4] = target.transform.eulerAngles;
        switch ((string) args["axis"])
        {
          case "x":
            vector3Array[4].y = vector3Array[0].y;
            vector3Array[4].z = vector3Array[0].z;
            break;
          case "y":
            vector3Array[4].x = vector3Array[0].x;
            vector3Array[4].z = vector3Array[0].z;
            break;
          case "z":
            vector3Array[4].x = vector3Array[0].x;
            vector3Array[4].y = vector3Array[0].y;
            break;
        }
        target.transform.eulerAngles = vector3Array[4];
      }
      else
        Debug.LogError("iTween Error: LookUpdate needs a 'looktarget' property!");
    }

    public static void LookUpdate(GameObject target, Vector3 looktarget, float time)
    {
      iTween.LookUpdate(target, iTween.Hash((object) nameof (looktarget), (object) looktarget, (object) nameof (time), (object) time));
    }

    public static float PathLength(Transform[] path)
    {
      Vector3[] path1 = new Vector3[path.Length];
      float num1 = 0.0f;
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].position;
      Vector3[] pts = iTween.PathControlPointGenerator(path1);
      Vector3 a = iTween.Interp(pts, 0.0f);
      int num2 = path.Length * 20;
      for (int index = 1; index <= num2; ++index)
      {
        float t = (float) index / (float) num2;
        Vector3 b = iTween.Interp(pts, t);
        num1 += Vector3.Distance(a, b);
        a = b;
      }
      return num1;
    }

    public static float PathLength(Vector3[] path)
    {
      float num1 = 0.0f;
      Vector3[] pts = iTween.PathControlPointGenerator(path);
      Vector3 a = iTween.Interp(pts, 0.0f);
      int num2 = path.Length * 20;
      for (int index = 1; index <= num2; ++index)
      {
        float t = (float) index / (float) num2;
        Vector3 b = iTween.Interp(pts, t);
        num1 += Vector3.Distance(a, b);
        a = b;
      }
      return num1;
    }

    public static void PutOnPath(GameObject target, Vector3[] path, float percent)
    {
      target.transform.position = iTween.Interp(iTween.PathControlPointGenerator(path), percent);
    }

    public static void PutOnPath(Transform target, Vector3[] path, float percent)
    {
      target.position = iTween.Interp(iTween.PathControlPointGenerator(path), percent);
    }

    public static void PutOnPath(GameObject target, Transform[] path, float percent)
    {
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].position;
      target.transform.position = iTween.Interp(iTween.PathControlPointGenerator(path1), percent);
    }

    public static void PutOnPath(Transform target, Transform[] path, float percent)
    {
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].position;
      target.position = iTween.Interp(iTween.PathControlPointGenerator(path1), percent);
    }

    public static Vector3 PointOnPath(Transform[] path, float percent)
    {
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].position;
      return iTween.Interp(iTween.PathControlPointGenerator(path1), percent);
    }

    public static void DrawLine(Vector3[] line)
    {
      if (line.Length <= 0)
        return;
      iTween.DrawLineHelper(line, iTween.Defaults.color, "gizmos");
    }

    public static void DrawLine(Vector3[] line, Color color)
    {
      if (line.Length <= 0)
        return;
      iTween.DrawLineHelper(line, color, "gizmos");
    }

    public static void DrawLine(Transform[] line)
    {
      if (line.Length <= 0)
        return;
      Vector3[] line1 = new Vector3[line.Length];
      for (int index = 0; index < line.Length; ++index)
        line1[index] = line[index].position;
      iTween.DrawLineHelper(line1, iTween.Defaults.color, "gizmos");
    }

    public static void DrawLine(Transform[] line, Color color)
    {
      if (line.Length <= 0)
        return;
      Vector3[] line1 = new Vector3[line.Length];
      for (int index = 0; index < line.Length; ++index)
        line1[index] = line[index].position;
      iTween.DrawLineHelper(line1, color, "gizmos");
    }

    public static void DrawLineGizmos(Vector3[] line)
    {
      if (line.Length <= 0)
        return;
      iTween.DrawLineHelper(line, iTween.Defaults.color, "gizmos");
    }

    public static void DrawLineGizmos(Vector3[] line, Color color)
    {
      if (line.Length <= 0)
        return;
      iTween.DrawLineHelper(line, color, "gizmos");
    }

    public static void DrawLineGizmos(Transform[] line)
    {
      if (line.Length <= 0)
        return;
      Vector3[] line1 = new Vector3[line.Length];
      for (int index = 0; index < line.Length; ++index)
        line1[index] = line[index].position;
      iTween.DrawLineHelper(line1, iTween.Defaults.color, "gizmos");
    }

    public static void DrawLineGizmos(Transform[] line, Color color)
    {
      if (line.Length <= 0)
        return;
      Vector3[] line1 = new Vector3[line.Length];
      for (int index = 0; index < line.Length; ++index)
        line1[index] = line[index].position;
      iTween.DrawLineHelper(line1, color, "gizmos");
    }

    public static void DrawLineHandles(Vector3[] line)
    {
      if (line.Length <= 0)
        return;
      iTween.DrawLineHelper(line, iTween.Defaults.color, "handles");
    }

    public static void DrawLineHandles(Vector3[] line, Color color)
    {
      if (line.Length <= 0)
        return;
      iTween.DrawLineHelper(line, color, "handles");
    }

    public static void DrawLineHandles(Transform[] line)
    {
      if (line.Length <= 0)
        return;
      Vector3[] line1 = new Vector3[line.Length];
      for (int index = 0; index < line.Length; ++index)
        line1[index] = line[index].position;
      iTween.DrawLineHelper(line1, iTween.Defaults.color, "handles");
    }

    public static void DrawLineHandles(Transform[] line, Color color)
    {
      if (line.Length <= 0)
        return;
      Vector3[] line1 = new Vector3[line.Length];
      for (int index = 0; index < line.Length; ++index)
        line1[index] = line[index].position;
      iTween.DrawLineHelper(line1, color, "handles");
    }

    public static Vector3 PointOnPath(Vector3[] path, float percent)
    {
      return iTween.Interp(iTween.PathControlPointGenerator(path), percent);
    }

    public static void DrawPath(Vector3[] path)
    {
      if (path.Length <= 0)
        return;
      iTween.DrawPathHelper(path, iTween.Defaults.color, "gizmos");
    }

    public static void DrawPath(Vector3[] path, Color color)
    {
      if (path.Length <= 0)
        return;
      iTween.DrawPathHelper(path, color, "gizmos");
    }

    public static void DrawPath(Transform[] path)
    {
      if (path.Length <= 0)
        return;
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].position;
      iTween.DrawPathHelper(path1, iTween.Defaults.color, "gizmos");
    }

    public static void DrawPath(Transform[] path, Color color)
    {
      if (path.Length <= 0)
        return;
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].position;
      iTween.DrawPathHelper(path1, color, "gizmos");
    }

    public static void DrawPathGizmos(Vector3[] path)
    {
      if (path.Length <= 0)
        return;
      iTween.DrawPathHelper(path, iTween.Defaults.color, "gizmos");
    }

    public static void DrawPathGizmos(Vector3[] path, Color color)
    {
      if (path.Length <= 0)
        return;
      iTween.DrawPathHelper(path, color, "gizmos");
    }

    public static void DrawPathGizmos(Transform[] path)
    {
      if (path.Length <= 0)
        return;
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].position;
      iTween.DrawPathHelper(path1, iTween.Defaults.color, "gizmos");
    }

    public static void DrawPathGizmos(Transform[] path, Color color)
    {
      if (path.Length <= 0)
        return;
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].position;
      iTween.DrawPathHelper(path1, color, "gizmos");
    }

    public static void DrawPathHandles(Vector3[] path)
    {
      if (path.Length <= 0)
        return;
      iTween.DrawPathHelper(path, iTween.Defaults.color, "handles");
    }

    public static void DrawPathHandles(Vector3[] path, Color color)
    {
      if (path.Length <= 0)
        return;
      iTween.DrawPathHelper(path, color, "handles");
    }

    public static void DrawPathHandles(Transform[] path)
    {
      if (path.Length <= 0)
        return;
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].position;
      iTween.DrawPathHelper(path1, iTween.Defaults.color, "handles");
    }

    public static void DrawPathHandles(Transform[] path, Color color)
    {
      if (path.Length <= 0)
        return;
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].position;
      iTween.DrawPathHelper(path1, color, "handles");
    }

    public static void Resume(GameObject target)
    {
      foreach (Behaviour component in target.GetComponents(typeof (iTween)))
        component.enabled = true;
    }

    public static void Resume(GameObject target, bool includechildren)
    {
      iTween.Resume(target);
      if (!includechildren)
        return;
      foreach (Component component in target.transform)
        iTween.Resume(component.gameObject, true);
    }

    public static void Resume(GameObject target, string type)
    {
      foreach (iTween component in target.GetComponents(typeof (iTween)))
      {
        if ((component.type + component.method).Substring(0, type.Length).ToLower() == type.ToLower())
          component.enabled = true;
      }
    }

    public static void Resume(GameObject target, string type, bool includechildren)
    {
      foreach (iTween component in target.GetComponents(typeof (iTween)))
      {
        if ((component.type + component.method).Substring(0, type.Length).ToLower() == type.ToLower())
          component.enabled = true;
      }
      if (!includechildren)
        return;
      foreach (Component component in target.transform)
        iTween.Resume(component.gameObject, type, true);
    }

    public static void Resume()
    {
      for (int index = 0; index < iTween.tweens.Count; ++index)
        iTween.Resume((GameObject) iTween.tweens[index]["target"]);
    }

    public static void Resume(string type)
    {
      List<GameObject> gameObjectList = new List<GameObject>();
      for (int index = 0; index < iTween.tweens.Count; ++index)
      {
        GameObject gameObject = (GameObject) iTween.tweens[index]["target"];
        gameObjectList.Insert(gameObjectList.Count, gameObject);
      }
      for (int index = 0; index < gameObjectList.Count; ++index)
        iTween.Resume(gameObjectList[index], type);
    }

    public static void Pause(GameObject target)
    {
      foreach (iTween component in target.GetComponents(typeof (iTween)))
      {
        if ((double) component.delay > 0.0)
          component.delay -= Time.time - component.delayStarted;
        component.isPaused = true;
        component.enabled = false;
      }
    }

    public static void Pause(GameObject target, bool includechildren)
    {
      iTween.Pause(target);
      if (!includechildren)
        return;
      foreach (Component component in target.transform)
        iTween.Pause(component.gameObject, true);
    }

    public static void Pause(GameObject target, string type)
    {
      foreach (iTween component in target.GetComponents(typeof (iTween)))
      {
        if ((component.type + component.method).Substring(0, type.Length).ToLower() == type.ToLower())
        {
          if ((double) component.delay > 0.0)
            component.delay -= Time.time - component.delayStarted;
          component.isPaused = true;
          component.enabled = false;
        }
      }
    }

    public static void Pause(GameObject target, string type, bool includechildren)
    {
      foreach (iTween component in target.GetComponents(typeof (iTween)))
      {
        if ((component.type + component.method).Substring(0, type.Length).ToLower() == type.ToLower())
        {
          if ((double) component.delay > 0.0)
            component.delay -= Time.time - component.delayStarted;
          component.isPaused = true;
          component.enabled = false;
        }
      }
      if (!includechildren)
        return;
      foreach (Component component in target.transform)
        iTween.Pause(component.gameObject, type, true);
    }

    public static void Pause()
    {
      for (int index = 0; index < iTween.tweens.Count; ++index)
        iTween.Pause((GameObject) iTween.tweens[index]["target"]);
    }

    public static void Pause(string type)
    {
      List<GameObject> gameObjectList = new List<GameObject>();
      for (int index = 0; index < iTween.tweens.Count; ++index)
      {
        GameObject gameObject = (GameObject) iTween.tweens[index]["target"];
        gameObjectList.Insert(gameObjectList.Count, gameObject);
      }
      for (int index = 0; index < gameObjectList.Count; ++index)
        iTween.Pause(gameObjectList[index], type);
    }

    public static int Count() => iTween.tweens.Count;

    public static int Count(string type)
    {
      int num = 0;
      for (int index = 0; index < iTween.tweens.Count; ++index)
      {
        Dictionary<string, object> tween = iTween.tweens[index];
        if (((string) tween[nameof (type)] + (string) tween["method"]).Substring(0, type.Length).ToLower() == type.ToLower())
          ++num;
      }
      return num;
    }

    public static int Count(GameObject target) => target.GetComponents(typeof (iTween)).Length;

    public static int Count(GameObject target, string type)
    {
      int num = 0;
      foreach (iTween component in target.GetComponents(typeof (iTween)))
      {
        if ((component.type + component.method).Substring(0, type.Length).ToLower() == type.ToLower())
          ++num;
      }
      return num;
    }

    public static void Stop()
    {
      for (int index = 0; index < iTween.tweens.Count; ++index)
        iTween.Stop((GameObject) iTween.tweens[index]["target"]);
      iTween.tweens.Clear();
    }

    public static void Stop(string type)
    {
      List<GameObject> gameObjectList = new List<GameObject>();
      for (int index = 0; index < iTween.tweens.Count; ++index)
      {
        GameObject gameObject = (GameObject) iTween.tweens[index]["target"];
        gameObjectList.Insert(gameObjectList.Count, gameObject);
      }
      for (int index = 0; index < gameObjectList.Count; ++index)
        iTween.Stop(gameObjectList[index], type);
    }

    public static void Stop(GameObject target)
    {
      foreach (iTween component in target.GetComponents(typeof (iTween)))
        component.Dispose();
    }

    public static void Stop(GameObject target, bool includechildren)
    {
      iTween.Stop(target);
      if (!includechildren)
        return;
      foreach (Component component in target.transform)
        iTween.Stop(component.gameObject, true);
    }

    public static void Stop(GameObject target, string type)
    {
      foreach (iTween component in target.GetComponents(typeof (iTween)))
      {
        if ((component.type + component.method).ToLower() == type.ToLower())
          component.Dispose();
      }
    }

    public static void Stop(GameObject target, string type, bool includechildren)
    {
      foreach (iTween component in target.GetComponents(typeof (iTween)))
      {
        if ((component.type + component.method).Substring(0, type.Length).ToLower() == type.ToLower())
          component.Dispose();
      }
      if (!includechildren)
        return;
      foreach (Component component in target.transform)
        iTween.Stop(component.gameObject, type, true);
    }

    public static Dictionary<string, object> Hash(params object[] args)
    {
      Dictionary<string, object> dictionary = new Dictionary<string, object>(args.Length / 2);
      if (args.Length % 2 != 0)
      {
        Debug.LogError("Tween Error: Hash requires an even number of arguments!");
        return (Dictionary<string, object>) null;
      }
      for (int index = 0; index < args.Length - 1; index += 2)
      {
        if (args[index + 1].GetType().ToString() == "System.Int32")
          dictionary.Add((string) args[index], (object) (float) (int) args[index + 1]);
        else
          dictionary.Add((string) args[index], args[index + 1]);
      }
      return dictionary;
    }

    public static Dictionary<string, object> CloneDictionary(Dictionary<string, object> args)
    {
      Dictionary<string, object> dictionary = new Dictionary<string, object>(args.Count);
      foreach (KeyValuePair<string, object> keyValuePair in args)
        dictionary.Add(keyValuePair.Key, keyValuePair.Value);
      return dictionary;
    }

    public override void Awake()
    {
      this.RetrieveArgs();
      this.lastRealTime = Time.realtimeSinceStartup;
    }

    public override void Start() => this.TweenStart();

    public override void Update()
    {
      if (!this.isRunning || this.physics)
        return;
      if (!this.reverse)
      {
        if ((double) this.percentage < 1.0)
          this.TweenUpdate();
        else
          this.TweenComplete();
      }
      else if ((double) this.percentage > 0.0)
        this.TweenUpdate();
      else
        this.TweenComplete();
    }

    public override void FixedUpdate()
    {
      if (!this.isRunning || !this.physics)
        return;
      if (!this.reverse)
      {
        if ((double) this.percentage < 1.0)
          this.TweenUpdate();
        else
          this.TweenComplete();
      }
      else if ((double) this.percentage > 0.0)
        this.TweenUpdate();
      else
        this.TweenComplete();
    }

    public override void LateUpdate()
    {
      base.LateUpdate();
      if (!this.tweenArguments.ContainsKey("looktarget") || !this.isRunning || !(this.type == "move") && !(this.type == "shake") && !(this.type == "punch"))
        return;
      iTween.LookUpdate(this.gameObject, this.tweenArguments);
    }

    private void OnEnable()
    {
      if (this.isRunning)
        this.EnableKinematic();
      if (!this.isPaused)
        return;
      this.isPaused = false;
      if ((double) this.delay <= 0.0)
        return;
      this.ResumeDelay();
    }

    private void OnDisable() => this.DisableKinematic();

    private static void DrawLineHelper(Vector3[] line, Color color, string method)
    {
      throw new NotImplementedException();
    }

    private static void DrawPathHelper(Vector3[] path, Color color, string method)
    {
      throw new NotImplementedException();
    }

    private static Vector3[] PathControlPointGenerator(Vector3[] path)
    {
      Vector3[] sourceArray = path;
      int num = 2;
      Vector3[] vector3Array1 = new Vector3[sourceArray.Length + num];
      Array.Copy((Array) sourceArray, 0, (Array) vector3Array1, 1, sourceArray.Length);
      vector3Array1[0] = vector3Array1[1] + (vector3Array1[1] - vector3Array1[2]);
      vector3Array1[vector3Array1.Length - 1] = vector3Array1[vector3Array1.Length - 2] + (vector3Array1[vector3Array1.Length - 2] - vector3Array1[vector3Array1.Length - 3]);
      if (vector3Array1[1] == vector3Array1[vector3Array1.Length - 2])
      {
        Vector3[] vector3Array2 = new Vector3[vector3Array1.Length];
        Array.Copy((Array) vector3Array1, (Array) vector3Array2, vector3Array1.Length);
        vector3Array2[0] = vector3Array2[vector3Array2.Length - 3];
        vector3Array2[vector3Array2.Length - 1] = vector3Array2[2];
        vector3Array1 = new Vector3[vector3Array2.Length];
        Array.Copy((Array) vector3Array2, (Array) vector3Array1, vector3Array2.Length);
      }
      return vector3Array1;
    }

    private static Vector3 Interp(Vector3[] pts, float t)
    {
      int num1 = pts.Length - 3;
      int index = Mathf.Min(Mathf.FloorToInt(t * (float) num1), num1 - 1);
      float num2 = t * (float) num1 - (float) index;
      Vector3 pt1 = pts[index];
      Vector3 pt2 = pts[index + 1];
      Vector3 pt3 = pts[index + 2];
      Vector3 pt4 = pts[index + 3];
      return 0.5f * ((-pt1 + 3f * pt2 - 3f * pt3 + pt4) * (num2 * num2 * num2) + (2f * pt1 - 5f * pt2 + 4f * pt3 - pt4) * (num2 * num2) + (-pt1 + pt3) * num2 + 2f * pt2);
    }

    private static void Launch(GameObject target, Dictionary<string, object> args)
    {
      if (!args.ContainsKey("id"))
        args["id"] = (object) iTween.GenerateID();
      if (!args.ContainsKey(nameof (target)))
        args[nameof (target)] = (object) target;
      iTween.tweens.Insert(0, args);
      target.AddComponent<iTween>(new iTween()
      {
        id = (string) args["id"]
      });
    }

    private static Dictionary<string, object> CleanArgs(Dictionary<string, object> args)
    {
      Dictionary<string, object> dictionary1 = new Dictionary<string, object>(args.Count);
      Dictionary<string, object> dictionary2 = new Dictionary<string, object>(args.Count);
      foreach (KeyValuePair<string, object> keyValuePair in args)
        dictionary1.Add(keyValuePair.Key, keyValuePair.Value);
      foreach (KeyValuePair<string, object> keyValuePair in dictionary1)
      {
        if (keyValuePair.Value.GetType() == typeof (int))
        {
          float num = (float) (int) keyValuePair.Value;
          args[keyValuePair.Key] = (object) num;
        }
        if (keyValuePair.Value.GetType() == typeof (double))
        {
          float num = (float) (double) keyValuePair.Value;
          args[keyValuePair.Key] = (object) num;
        }
      }
      foreach (KeyValuePair<string, object> keyValuePair in dictionary1)
        dictionary2.Add(keyValuePair.Key.ToString().ToLower(), keyValuePair.Value);
      args = dictionary2;
      return args;
    }

    private static string GenerateID() => Guid.NewGuid().ToString();

    private void RetrieveArgs()
    {
      foreach (Dictionary<string, object> tween in iTween.tweens)
      {
        if ((GameObject) tween["target"] == this.gameObject && !tween.ContainsKey("hasBeenAdded"))
        {
          tween.Add("hasBeenAdded", (object) true);
          this.tweenArguments = tween;
          break;
        }
      }
      if (this.tweenArguments == null)
      {
        UnityObject.Destroy((UnityObject) this);
      }
      else
      {
        this.id = (string) this.tweenArguments["id"];
        this.type = (string) this.tweenArguments["type"];
        this.method = (string) this.tweenArguments["method"];
        this.time = !this.tweenArguments.ContainsKey("time") ? iTween.Defaults.time : (float) this.tweenArguments["time"];
        if (this.rigidbody != null)
          this.physics = true;
        this.delay = !this.tweenArguments.ContainsKey("delay") ? iTween.Defaults.delay : (float) this.tweenArguments["delay"];
        if ((double) this.delay > 0.0)
          this.isDelayed = true;
        if (this.tweenArguments.ContainsKey("namedcolorvalue"))
        {
          if (this.tweenArguments["namedcolorvalue"].GetType() == typeof (iTween.NamedValueColor))
          {
            this.namedcolorvalue = (iTween.NamedValueColor) this.tweenArguments["namedcolorvalue"];
          }
          else
          {
            try
            {
              this.namedcolorvalue = (iTween.NamedValueColor) Enum.Parse(typeof (iTween.NamedValueColor), (string) this.tweenArguments["namedcolorvalue"], true);
            }
            catch
            {
              Debug.LogWarning("iTween: Unsupported namedcolorvalue supplied! Default will be used.");
              this.namedcolorvalue = iTween.NamedValueColor._Color;
            }
          }
        }
        else
          this.namedcolorvalue = iTween.Defaults.namedColorValue;
        if (this.tweenArguments.ContainsKey("looptype"))
        {
          if (this.tweenArguments["looptype"].GetType() == typeof (iTween.LoopType))
          {
            this.loopType = (iTween.LoopType) this.tweenArguments["looptype"];
          }
          else
          {
            try
            {
              this.loopType = (iTween.LoopType) Enum.Parse(typeof (iTween.LoopType), (string) this.tweenArguments["looptype"], true);
            }
            catch
            {
              Debug.LogWarning("iTween: Unsupported loopType supplied! Default will be used.");
              this.loopType = iTween.LoopType.none;
            }
          }
        }
        else
          this.loopType = iTween.LoopType.none;
        if (this.tweenArguments.ContainsKey("easetype"))
        {
          if (this.tweenArguments["easetype"].GetType() == typeof (iTween.EaseType))
          {
            this.easeType = (iTween.EaseType) this.tweenArguments["easetype"];
          }
          else
          {
            try
            {
              this.easeType = (iTween.EaseType) Enum.Parse(typeof (iTween.EaseType), (string) this.tweenArguments["easetype"], true);
            }
            catch
            {
              Debug.LogWarning("iTween: Unsupported easeType supplied! Default will be used.");
              this.easeType = iTween.Defaults.easeType;
            }
          }
        }
        else
          this.easeType = iTween.Defaults.easeType;
        if (this.tweenArguments.ContainsKey("space"))
        {
          if (this.tweenArguments["space"].GetType() == typeof (Space))
          {
            this.space = (Space) this.tweenArguments["space"];
          }
          else
          {
            try
            {
              this.space = (Space) Enum.Parse(typeof (Space), (string) this.tweenArguments["space"], true);
            }
            catch
            {
              Debug.LogWarning("iTween: Unsupported space supplied! Default will be used.");
              this.space = iTween.Defaults.space;
            }
          }
        }
        else
          this.space = iTween.Defaults.space;
        this.isLocal = !this.tweenArguments.ContainsKey("islocal") ? iTween.Defaults.isLocal : (bool) this.tweenArguments["islocal"];
        this.useRealTime = !this.tweenArguments.ContainsKey("ignoretimescale") ? iTween.Defaults.useRealTime : (bool) this.tweenArguments["ignoretimescale"];
        this.GetEasingFunction();
      }
    }

    private void GetEasingFunction()
    {
      switch (this.easeType)
      {
        case iTween.EaseType.easeInQuad:
          this.ease = new iTween.EasingFunction(this.easeInQuad);
          break;
        case iTween.EaseType.easeOutQuad:
          this.ease = new iTween.EasingFunction(this.easeOutQuad);
          break;
        case iTween.EaseType.easeInOutQuad:
          this.ease = new iTween.EasingFunction(this.easeInOutQuad);
          break;
        case iTween.EaseType.easeInCubic:
          this.ease = new iTween.EasingFunction(this.easeInCubic);
          break;
        case iTween.EaseType.easeOutCubic:
          this.ease = new iTween.EasingFunction(this.easeOutCubic);
          break;
        case iTween.EaseType.easeInOutCubic:
          this.ease = new iTween.EasingFunction(this.easeInOutCubic);
          break;
        case iTween.EaseType.easeInQuart:
          this.ease = new iTween.EasingFunction(this.easeInQuart);
          break;
        case iTween.EaseType.easeOutQuart:
          this.ease = new iTween.EasingFunction(this.easeOutQuart);
          break;
        case iTween.EaseType.easeInOutQuart:
          this.ease = new iTween.EasingFunction(this.easeInOutQuart);
          break;
        case iTween.EaseType.easeInQuint:
          this.ease = new iTween.EasingFunction(this.easeInQuint);
          break;
        case iTween.EaseType.easeOutQuint:
          this.ease = new iTween.EasingFunction(this.easeOutQuint);
          break;
        case iTween.EaseType.easeInOutQuint:
          this.ease = new iTween.EasingFunction(this.easeInOutQuint);
          break;
        case iTween.EaseType.easeInSine:
          this.ease = new iTween.EasingFunction(this.easeInSine);
          break;
        case iTween.EaseType.easeOutSine:
          this.ease = new iTween.EasingFunction(this.easeOutSine);
          break;
        case iTween.EaseType.easeInOutSine:
          this.ease = new iTween.EasingFunction(this.easeInOutSine);
          break;
        case iTween.EaseType.easeInExpo:
          this.ease = new iTween.EasingFunction(this.easeInExpo);
          break;
        case iTween.EaseType.easeOutExpo:
          this.ease = new iTween.EasingFunction(this.easeOutExpo);
          break;
        case iTween.EaseType.easeInOutExpo:
          this.ease = new iTween.EasingFunction(this.easeInOutExpo);
          break;
        case iTween.EaseType.easeInCirc:
          this.ease = new iTween.EasingFunction(this.easeInCirc);
          break;
        case iTween.EaseType.easeOutCirc:
          this.ease = new iTween.EasingFunction(this.easeOutCirc);
          break;
        case iTween.EaseType.easeInOutCirc:
          this.ease = new iTween.EasingFunction(this.easeInOutCirc);
          break;
        case iTween.EaseType.linear:
          this.ease = new iTween.EasingFunction(this.linear);
          break;
        case iTween.EaseType.spring:
          this.ease = new iTween.EasingFunction(this.spring);
          break;
        case iTween.EaseType.bounce:
          this.ease = new iTween.EasingFunction(this.bounce);
          break;
        case iTween.EaseType.easeInBack:
          this.ease = new iTween.EasingFunction(this.easeInBack);
          break;
        case iTween.EaseType.easeOutBack:
          this.ease = new iTween.EasingFunction(this.easeOutBack);
          break;
        case iTween.EaseType.easeInOutBack:
          this.ease = new iTween.EasingFunction(this.easeInOutBack);
          break;
        case iTween.EaseType.elastic:
          this.ease = new iTween.EasingFunction(this.elastic);
          break;
      }
    }

    private void UpdatePercentage()
    {
      if (this.useRealTime)
        this.runningTime += Time.realtimeSinceStartup - this.lastRealTime;
      else
        this.runningTime += Time.deltaTime;
      this.percentage = !this.reverse ? this.runningTime / this.time : (float) (1.0 - (double) this.runningTime / (double) this.time);
      this.lastRealTime = Time.realtimeSinceStartup;
    }

    private void CallBack(string callbackType)
    {
      if (this.tweenArguments == null)
      {
        Debug.Log((object) "Destroy tween without callback arguments");
        UnityObject.Destroy((UnityObject) this);
      }
      else
      {
        if (!this.tweenArguments.ContainsKey(callbackType) || this.tweenArguments.ContainsKey("ischild"))
          return;
        string key = string.Empty;
        switch (callbackType)
        {
          case "onstart":
            key = "onstarttarget";
            break;
          case "onupdate":
            key = "onupdatetarget";
            break;
          case "oncomplete":
            key = "oncompletetarget";
            break;
        }
        GameObject gameObject = !this.tweenArguments.ContainsKey(key) ? this.gameObject : (GameObject) this.tweenArguments[key];
        if (this.tweenArguments[callbackType].GetType() == typeof (string))
        {
          object tweenArgument = this.tweenArguments.ContainsKey(callbackType + "params") ? this.tweenArguments[callbackType + "params"] : (object) null;
          gameObject.SendMessage((string) this.tweenArguments[callbackType], tweenArgument, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
          Debug.LogError("iTween Error: Callback method references must be passed as a String!");
          UnityObject.Destroy((UnityObject) this);
        }
      }
    }

    private void Dispose()
    {
      for (int index = 0; index < iTween.tweens.Count; ++index)
      {
        if ((string) iTween.tweens[index]["id"] == this.id)
        {
          iTween.tweens.RemoveAt(index);
          break;
        }
      }
      UnityObject.Destroy((UnityObject) this);
    }

    private void ConflictCheck()
    {
      foreach (iTween component in (Component[]) this.GetComponents<iTween>())
      {
        if (component.type == "value")
          break;
        if (component.isRunning && component.type == this.type)
        {
          if (component.tweenArguments == null || component.tweenArguments.Count != this.tweenArguments.Count)
          {
            component.Dispose();
            break;
          }
          foreach (KeyValuePair<string, object> tweenArgument in this.tweenArguments)
          {
            if (!component.tweenArguments.ContainsKey(tweenArgument.Key))
            {
              component.Dispose();
              return;
            }
            if (!component.tweenArguments[tweenArgument.Key].Equals(this.tweenArguments[tweenArgument.Key]) && tweenArgument.Key != "id")
            {
              component.Dispose();
              return;
            }
          }
          this.Dispose();
        }
      }
    }

    private void EnableKinematic()
    {
      if (!(bool) (UnityObject) this.gameObject.GetComponent(typeof (Rigidbody)) || this.rigidbody.isKinematic)
        return;
      this.kinematic = true;
      this.rigidbody.isKinematic = true;
    }

    private void DisableKinematic()
    {
      if (!this.kinematic || !this.rigidbody.isKinematic)
        return;
      this.kinematic = false;
      this.rigidbody.isKinematic = false;
    }

    private void ResumeDelay()
    {
    }

    private float linear(float start, float end, float value) => Mathf.Lerp(start, end, value);

    private float clerp(float start, float end, float value)
    {
      float num1 = 0.0f;
      float num2 = 360f;
      float num3 = Mathf.Abs((float) (((double) num2 - (double) num1) / 2.0));
      float num4;
      if ((double) end - (double) start < -(double) num3)
      {
        float num5 = (num2 - start + end) * value;
        num4 = start + num5;
      }
      else if ((double) end - (double) start > (double) num3)
      {
        float num6 = (float) -((double) num2 - (double) end + (double) start) * value;
        num4 = start + num6;
      }
      else
        num4 = start + (end - start) * value;
      return num4;
    }

    private float spring(float start, float end, float value)
    {
      value = Mathf.Clamp01(value);
      value = (float) (((double) Mathf.Sin((float) ((double) value * 3.1415927410125732 * (0.20000000298023224 + 2.5 * (double) value * (double) value * (double) value))) * (double) Mathf.Pow(1f - value, 2.2f) + (double) value) * (1.0 + 1.2000000476837158 * (1.0 - (double) value)));
      return start + (end - start) * value;
    }

    private float easeInQuad(float start, float end, float value)
    {
      end -= start;
      return end * value * value + start;
    }

    private float easeOutQuad(float start, float end, float value)
    {
      end -= start;
      return (float) (-(double) end * (double) value * ((double) value - 2.0)) + start;
    }

    private float easeInOutQuad(float start, float end, float value)
    {
      value /= 0.5f;
      end -= start;
      if ((double) value < 1.0)
        return end / 2f * value * value + start;
      --value;
      return (float) (-(double) end / 2.0 * ((double) value * ((double) value - 2.0) - 1.0)) + start;
    }

    private float easeInCubic(float start, float end, float value)
    {
      end -= start;
      return end * value * value * value + start;
    }

    private float easeOutCubic(float start, float end, float value)
    {
      --value;
      end -= start;
      return end * (float) ((double) value * (double) value * (double) value + 1.0) + start;
    }

    private float easeInOutCubic(float start, float end, float value)
    {
      value /= 0.5f;
      end -= start;
      if ((double) value < 1.0)
        return end / 2f * value * value * value + start;
      value -= 2f;
      return (float) ((double) end / 2.0 * ((double) value * (double) value * (double) value + 2.0)) + start;
    }

    private float easeInQuart(float start, float end, float value)
    {
      end -= start;
      return end * value * value * value * value + start;
    }

    private float easeOutQuart(float start, float end, float value)
    {
      --value;
      end -= start;
      return (float) (-(double) end * ((double) value * (double) value * (double) value * (double) value - 1.0)) + start;
    }

    private float easeInOutQuart(float start, float end, float value)
    {
      value /= 0.5f;
      end -= start;
      if ((double) value < 1.0)
        return end / 2f * value * value * value * value + start;
      value -= 2f;
      return (float) (-(double) end / 2.0 * ((double) value * (double) value * (double) value * (double) value - 2.0)) + start;
    }

    private float easeInQuint(float start, float end, float value)
    {
      end -= start;
      return end * value * value * value * value * value + start;
    }

    private float easeOutQuint(float start, float end, float value)
    {
      --value;
      end -= start;
      return end * (float) ((double) value * (double) value * (double) value * (double) value * (double) value + 1.0) + start;
    }

    private float easeInOutQuint(float start, float end, float value)
    {
      value /= 0.5f;
      end -= start;
      if ((double) value < 1.0)
        return end / 2f * value * value * value * value * value + start;
      value -= 2f;
      return (float) ((double) end / 2.0 * ((double) value * (double) value * (double) value * (double) value * (double) value + 2.0)) + start;
    }

    private float easeInSine(float start, float end, float value)
    {
      end -= start;
      return -end * Mathf.Cos((float) ((double) value / 1.0 * 1.5707963705062866)) + end + start;
    }

    private float easeOutSine(float start, float end, float value)
    {
      end -= start;
      return end * Mathf.Sin((float) ((double) value / 1.0 * 1.5707963705062866)) + start;
    }

    private float easeInOutSine(float start, float end, float value)
    {
      end -= start;
      return (float) (-(double) end / 2.0 * ((double) Mathf.Cos((float) (3.1415927410125732 * (double) value / 1.0)) - 1.0)) + start;
    }

    private float easeInExpo(float start, float end, float value)
    {
      end -= start;
      return end * Mathf.Pow(2f, (float) (10.0 * ((double) value / 1.0 - 1.0))) + start;
    }

    private float easeOutExpo(float start, float end, float value)
    {
      end -= start;
      return end * (float) (-(double) Mathf.Pow(2f, (float) (-10.0 * (double) value / 1.0)) + 1.0) + start;
    }

    private float easeInOutExpo(float start, float end, float value)
    {
      value /= 0.5f;
      end -= start;
      if ((double) value < 1.0)
        return end / 2f * Mathf.Pow(2f, (float) (10.0 * ((double) value - 1.0))) + start;
      --value;
      return (float) ((double) end / 2.0 * (-(double) Mathf.Pow(2f, -10f * value) + 2.0)) + start;
    }

    private float easeInCirc(float start, float end, float value)
    {
      end -= start;
      return (float) (-(double) end * ((double) Mathf.Sqrt((float) (1.0 - (double) value * (double) value)) - 1.0)) + start;
    }

    private float easeOutCirc(float start, float end, float value)
    {
      --value;
      end -= start;
      return end * Mathf.Sqrt((float) (1.0 - (double) value * (double) value)) + start;
    }

    private float easeInOutCirc(float start, float end, float value)
    {
      value /= 0.5f;
      end -= start;
      if ((double) value < 1.0)
        return (float) (-(double) end / 2.0 * ((double) Mathf.Sqrt((float) (1.0 - (double) value * (double) value)) - 1.0)) + start;
      value -= 2f;
      return (float) ((double) end / 2.0 * ((double) Mathf.Sqrt((float) (1.0 - (double) value * (double) value)) + 1.0)) + start;
    }

    private float bounce(float start, float end, float value)
    {
      value /= 1f;
      end -= start;
      if ((double) value < 0.36363637447357178)
        return end * (121f / 16f * value * value) + start;
      if ((double) value < 0.72727274894714355)
      {
        value -= 0.545454562f;
        return end * (float) (121.0 / 16.0 * (double) value * (double) value + 0.75) + start;
      }
      if ((double) value < 10.0 / 11.0)
      {
        value -= 0.8181818f;
        return end * (float) (121.0 / 16.0 * (double) value * (double) value + 15.0 / 16.0) + start;
      }
      value -= 0.954545438f;
      return end * (float) (121.0 / 16.0 * (double) value * (double) value + 63.0 / 64.0) + start;
    }

    private float easeInBack(float start, float end, float value)
    {
      end -= start;
      value /= 1f;
      float num = 1.70158f;
      return (float) ((double) end * (double) value * (double) value * (((double) num + 1.0) * (double) value - (double) num)) + start;
    }

    private float easeOutBack(float start, float end, float value)
    {
      float num = 1.70158f;
      end -= start;
      value = (float) ((double) value / 1.0 - 1.0);
      return end * (float) ((double) value * (double) value * (((double) num + 1.0) * (double) value + (double) num) + 1.0) + start;
    }

    private float easeInOutBack(float start, float end, float value)
    {
      float num1 = 1.70158f;
      end -= start;
      value /= 0.5f;
      if ((double) value < 1.0)
      {
        float num2 = num1 * 1.525f;
        return (float) ((double) end / 2.0 * ((double) value * (double) value * (((double) num2 + 1.0) * (double) value - (double) num2))) + start;
      }
      value -= 2f;
      float num3 = num1 * 1.525f;
      return (float) ((double) end / 2.0 * ((double) value * (double) value * (((double) num3 + 1.0) * (double) value + (double) num3) + 2.0)) + start;
    }

    private float punch(float amplitude, float value)
    {
      if ((double) value == 0.0 || (double) value == 1.0)
        return 0.0f;
      float num1 = 0.3f;
      float num2 = num1 / 6.28318548f * Mathf.Asin(0.0f);
      return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((float) (((double) value * 1.0 - (double) num2) * 6.2831854820251465) / num1);
    }

    private float elastic(float start, float end, float value)
    {
      end -= start;
      float num1 = 1f;
      float num2 = num1 * 0.3f;
      float num3 = 0.0f;
      if ((double) value == 0.0)
        return start;
      if ((double) (value /= num1) == 1.0)
        return start + end;
      float num4;
      if ((double) num3 == 0.0 || (double) num3 < (double) Mathf.Abs(end))
      {
        num3 = end;
        num4 = num2 / 4f;
      }
      else
        num4 = num2 / 6.28318548f * Mathf.Asin(end / num3);
      return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((float) (((double) value * (double) num1 - (double) num4) * 6.2831854820251465) / num2) + end + start;
    }

    private delegate float EasingFunction(float start, float end, float value);

    private delegate void ApplyTween();

    public enum EaseType
    {
      easeInQuad,
      easeOutQuad,
      easeInOutQuad,
      easeInCubic,
      easeOutCubic,
      easeInOutCubic,
      easeInQuart,
      easeOutQuart,
      easeInOutQuart,
      easeInQuint,
      easeOutQuint,
      easeInOutQuint,
      easeInSine,
      easeOutSine,
      easeInOutSine,
      easeInExpo,
      easeOutExpo,
      easeInOutExpo,
      easeInCirc,
      easeOutCirc,
      easeInOutCirc,
      linear,
      spring,
      bounce,
      easeInBack,
      easeOutBack,
      easeInOutBack,
      elastic,
      punch,
    }

    public enum LoopType
    {
      none,
      loop,
      pingPong,
    }

    public enum NamedValueColor
    {
      _Color,
      _SpecColor,
      _Emission,
      _ReflectColor,
    }

    public static class Defaults
    {
      public static float time = 1f;
      public static float delay = 0.0f;
      public static iTween.NamedValueColor namedColorValue = iTween.NamedValueColor._Color;
      public static iTween.LoopType loopType = iTween.LoopType.none;
      public static iTween.EaseType easeType = iTween.EaseType.easeOutExpo;
      public static float lookSpeed = 3f;
      public static bool isLocal = false;
      public static Space space = Space.Self;
      public static bool orientToPath = false;
      public static Color color = Color.white;
      public static float updateTimePercentage = 0.05f;
      public static float updateTime = 1f * iTween.Defaults.updateTimePercentage;
      public static int cameraFadeDepth = 999999;
      public static float lookAhead = 0.05f;
      public static bool useRealTime = false;
      public static Vector3 up = Vector3.up;
    }

    private class CRSpline
    {
      public Vector3[] pts;

      public CRSpline(params Vector3[] pts)
      {
        this.pts = new Vector3[pts.Length];
        Array.Copy((Array) pts, (Array) this.pts, pts.Length);
      }

      public Vector3 Interp(float t)
      {
        int num1 = this.pts.Length - 3;
        int index = Mathf.Min(Mathf.FloorToInt(t * (float) num1), num1 - 1);
        float num2 = t * (float) num1 - (float) index;
        Vector3 pt1 = this.pts[index];
        Vector3 pt2 = this.pts[index + 1];
        Vector3 pt3 = this.pts[index + 2];
        Vector3 pt4 = this.pts[index + 3];
        return 0.5f * ((-pt1 + 3f * pt2 - 3f * pt3 + pt4) * (num2 * num2 * num2) + (2f * pt1 - 5f * pt2 + 4f * pt3 - pt4) * (num2 * num2) + (-pt1 + pt3) * num2 + 2f * pt2);
      }
    }
  }
}
