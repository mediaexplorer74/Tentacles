// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.SendMessageMethodCache
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace PressPlay.FFWD
{
  internal static class SendMessageMethodCache
  {
    private static Dictionary<Type, Dictionary<string, MethodInfo>> cache = new Dictionary<Type, Dictionary<string, MethodInfo>>();

    internal static MethodInfo GetCachedMethod(this Type tp, string methodName, BindingFlags flags)
    {
      if (!SendMessageMethodCache.cache.ContainsKey(tp))
        SendMessageMethodCache.cache.Add(tp, new Dictionary<string, MethodInfo>());
      if (SendMessageMethodCache.cache[tp].ContainsKey(methodName))
        return SendMessageMethodCache.cache[tp][methodName];
      MethodInfo method = tp.GetMethod(methodName, flags);
      SendMessageMethodCache.cache[tp][methodName] = method;
      return method;
    }
  }
}
