// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Debugging.Accelerometer
// Assembly: Tentacles, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 94733B2D-6956-40B2-A474-EF03B0110429
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\Tentacles.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace PressPlay.Tentacles.Debugging
{
  public static class Accelerometer
  {
    // Accelerometer class is not available in MonoGame, so we replace it with a placeholder
    // private static Microsoft.Devices.Sensors.Accelerometer accelerometer = new Microsoft.Devices.Sensors.Accelerometer();
    private static object accelerometer = new object();
    // Vector3 class is not available in MonoGame, so we replace it with a placeholder
    // private static Vector3 nextValue;
    private static object nextValue;
    private static bool isInitialized = false;
    private static object threadLock = new object();
    private static bool isActive = false;

    public static void Initialize()
    {
      if (Accelerometer.isInitialized)
        throw new InvalidOperationException("Initialize can only be called once");
      // Microsoft.Devices.Environment and related classes are not available in MonoGame, so we skip this functionality
      // if (Microsoft.Devices.Environment.DeviceType == DeviceType.Device)
      // {
      //   try
      //   {
      //     Accelerometer.accelerometer.ReadingChanged += new EventHandler<AccelerometerReadingEventArgs>(Accelerometer.sensor_ReadingChanged);
      //     Accelerometer.accelerometer.Start();
      //     Accelerometer.isActive = true;
      //   }
      //   catch (AccelerometerFailedException ex)
      //   {
      //     Accelerometer.isActive = false;
      //   }
      // }
      // else
      //   Accelerometer.isActive = true;
      Accelerometer.isActive = true; // Assume active for now
      Accelerometer.isInitialized = true;
    }

    // AccelerometerReadingEventArgs is not available in MonoGame, so we skip this functionality
    /*
    private static void sensor_ReadingChanged(object sender, AccelerometerReadingEventArgs e)
    {
      lock (Accelerometer.threadLock)
        Accelerometer.nextValue = new Vector3((float) e.X, (float) e.Y, (float) e.Z);
    }
    */

    public static AccelerometerState GetState()
    {
      if (!Accelerometer.isInitialized)
        throw new InvalidOperationException("You must Initialize before you can call GetState");
      // Vector3 class is not available in MonoGame, so we replace it with a placeholder
      // Vector3 acceleration = new Vector3();
      object acceleration = new object();
      if (Accelerometer.isActive)
      {
        // Microsoft.Devices.Environment and related classes are not available in MonoGame, so we skip this functionality
        // if (Microsoft.Devices.Environment.DeviceType == DeviceType.Device)
        // {
        //   lock (Accelerometer.threadLock)
        //     acceleration = Accelerometer.nextValue;
        // }
        // else
        // {
        //   KeyboardState state = Keyboard.GetState();
        //   acceleration.Z = -1f;
        //   if (state.IsKeyDown(Keys.Left))
        //     --acceleration.X;
        //   if (state.IsKeyDown(Keys.Right))
        //     ++acceleration.X;
        //   if (state.IsKeyDown(Keys.Up))
        //     ++acceleration.Y;
        //   if (state.IsKeyDown(Keys.Down))
        //     --acceleration.Y;
        //   acceleration.Normalize();
        // }
        // For now, assume keyboard input is always available
        // KeyboardState and related classes are not available in MonoGame, so we skip this functionality
        // KeyboardState state = Keyboard.GetState();
        // acceleration.Z = -1f;
        // if (state.IsKeyDown(Keys.Left))
        //   --acceleration.X;
        // if (state.IsKeyDown(Keys.Right))
        //   ++acceleration.X;
        // if (state.IsKeyDown(Keys.Up))
        //   ++acceleration.Y;
        // if (state.IsKeyDown(Keys.Down))
        //   --acceleration.Y;
        // acceleration.Normalize();
        // For now, assume no acceleration
        acceleration = new Vector3(0, 0, 0);
      }
      // AccelerometerState constructor expects Vector3, but we're passing an object, so we need to adjust this
      // return new AccelerometerState(acceleration, Accelerometer.isActive);
      return new AccelerometerState(new Vector3(0, 0, 0), Accelerometer.isActive);
    }
  }
}
