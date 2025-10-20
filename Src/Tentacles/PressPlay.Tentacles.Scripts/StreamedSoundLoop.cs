// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.StreamedSoundLoop
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using PressPlay.FFWD;
using System;
using System.IO;
using System.Threading;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  internal class StreamedSoundLoop
  {
    protected Thread readThread;
    protected Stream waveFileStream;
    protected DynamicSoundEffectInstance dynamicSound;
    protected BinaryReader reader;
    private BinaryReader asyncReader;
    protected int position;
    protected int count;
    protected int dataSize;
    protected int headerSize = 44;
    private byte[] buffer1;
    private float bufferSizeMilliseconds = 1000f;

    public int streamPosition => this.position;

    public int streamBufferSize => this.count;

    public int streamSize => this.dataSize;

    public float Volume
    {
      get => this.dynamicSound.Volume;
      set => this.dynamicSound.Volume = value;
    }

    public SoundState State => this.dynamicSound.State;

    public StreamedSoundLoop(string fileLocation, float bufferSizeMilliseconds)
    {
      this.bufferSizeMilliseconds = bufferSizeMilliseconds;
      this.waveFileStream = TitleContainer.OpenStream(fileLocation);
      this.reader = new BinaryReader(this.waveFileStream);
      this.reader.ReadInt32();
      this.reader.ReadInt32();
      this.reader.ReadInt32();
      this.reader.ReadInt32();
      int num1 = this.reader.ReadInt32();
      int num2 = (int) this.reader.ReadInt16();
      int channels = (int) this.reader.ReadInt16();
      int sampleRate = this.reader.ReadInt32();
      this.reader.ReadInt32();
      int num3 = (int) this.reader.ReadInt16();
      int num4 = (int) this.reader.ReadInt16();
      if (num1 == 18)
        this.reader.ReadBytes((int) this.reader.ReadInt16());
      this.reader.ReadInt32();
      this.dataSize = this.reader.ReadInt32();
      this.dynamicSound = new DynamicSoundEffectInstance(sampleRate, (AudioChannels) channels);
      this.count = this.dynamicSound.GetSampleSizeInBytes(TimeSpan.FromMilliseconds((double) bufferSizeMilliseconds));
      this.reader.BaseStream.Position = (long) (this.position + this.headerSize);
      this.buffer1 = this.reader.ReadBytes(this.count);
      this.SubmitBuffer();
      this.position = this.count;
      this.dynamicSound.BufferNeeded += new EventHandler<EventArgs>(this.DynamicSound_BufferNeeded);
      this.CreateReadThread();
      do
        ;
      while (!this.readThread.IsAlive);
    }

    private void CreateReadThread()
    {
      this.readThread = new Thread(new ThreadStart(this.ASyncreadBuffer));
      this.readThread.Name = "LoopedSoundStreamReader";
      this.readThread.Start();
      this.readThread.IsBackground = true;
    }

    protected void DynamicSound_BufferNeeded(object sender, EventArgs e)
    {
      if (this.readThread.IsAlive)
        return;
      this.CreateReadThread();
    }

    protected void SubmitBuffer()
    {
      if (this.buffer1 != null && !this.dynamicSound.IsDisposed)
        this.dynamicSound.SubmitBuffer(this.buffer1);
      this.buffer1 = (byte[]) null;
    }

    protected void ASyncreadBuffer()
    {
      while (this.dynamicSound.PendingBufferCount < 2)
      {
        if (Application.isDeactivated)
          return;
        this.asyncReader = this.reader;
        this.asyncReader.BaseStream.Position = (long) (this.position + this.headerSize);
        int num = this.dataSize - this.position;
        if (num >= this.count)
        {
          this.buffer1 = this.asyncReader.ReadBytes(this.count);
          this.position += this.count;
        }
        else
        {
          this.buffer1 = new byte[this.count];
          Buffer.BlockCopy((Array) this.asyncReader.ReadBytes(num), 0, (Array) this.buffer1, 0, num);
          this.asyncReader.BaseStream.Position = (long) this.headerSize;
          Buffer.BlockCopy((Array) this.asyncReader.ReadBytes(this.count - num), 0, (Array) this.buffer1, num, this.count - num);
          this.position = this.count - num;
        }
        if (this.dynamicSound.IsDisposed || Application.isDeactivated)
          return;
        this.SubmitBuffer();
      }
      while (!Application.isDeactivated && !this.dynamicSound.IsDisposed && this.dynamicSound.PendingBufferCount >= 2)
        Thread.Sleep(TimeSpan.FromMilliseconds((double) this.bufferSizeMilliseconds));
      if (this.dynamicSound.IsDisposed || Application.isDeactivated)
        return;
      this.ASyncreadBuffer();
    }

    public void Play()
    {
      if (this.buffer1 != null)
        this.SubmitBuffer();
      this.dynamicSound.Play();
    }

    public void Stop() => this.dynamicSound.Stop();

    public void Pause() => this.dynamicSound.Pause();

    public void Resume() => this.dynamicSound.Resume();

    public void Destroy()
    {
      if (this.dynamicSound == null || this.dynamicSound.IsDisposed)
        return;
      this.dynamicSound.Dispose();
    }
  }
}
