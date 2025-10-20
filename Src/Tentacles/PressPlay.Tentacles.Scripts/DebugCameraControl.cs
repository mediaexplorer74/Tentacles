// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.DebugCameraControl
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System.Linq;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class DebugCameraControl : MonoBehaviour
  {
    private string standardDebugCamName;
    [ContentSerializerIgnore]
    public float moveSpeed = 50f;
    public Camera camera;
    private static DebugCameraControl instance;

    public static DebugCameraControl Instance
    {
      get
      {
        if (DebugCameraControl.instance == null)
          DebugCameraControl.CreateFromResources();
        return DebugCameraControl.instance;
      }
    }

    public override void Awake()
    {
      if (DebugCameraControl.instance != null)
      {
        Debug.LogError("Cannot have two instances of DebugCameraControl. Self destruction in 3...");
        UnityObject.Destroy((UnityObject) this);
      }
      else
      {
        DebugCameraControl.instance = this;
        UnityObject.DontDestroyOnLoad((UnityObject) this);
      }
    }

    public override void Start() => this.gameObject.active = true;

    public override void Update()
    {
      KeyboardState state = Keyboard.GetState();
      if (state.IsKeyDown(Keys.Up))
        this.transform.position += this.transform.up * Time.deltaTime * this.moveSpeed;
      if (state.IsKeyDown(Keys.Down))
        this.transform.position -= this.transform.up * Time.deltaTime * this.moveSpeed;
      if (state.IsKeyDown(Keys.Right))
        this.transform.position += this.transform.right * Time.deltaTime * this.moveSpeed;
      if (state.IsKeyDown(Keys.Left))
        this.transform.position -= this.transform.right * Time.deltaTime * this.moveSpeed;
      if (state.IsKeyDown(Keys.A))
        this.transform.position += this.transform.forward * Time.deltaTime * this.moveSpeed;
      if (state.IsKeyDown(Keys.Z))
        this.transform.position -= this.transform.forward * Time.deltaTime * this.moveSpeed;
      Debug.Display("debug cam pos", (object) this.transform.position);
    }

    public void ToggleDebugCam()
    {
      if (this.gameObject.active)
        this.DisableDebugCam();
      else
        this.EnableDebugCam();
    }

    public void EnableDebugCam()
    {
      Debug.Log((object) nameof (EnableDebugCam));
      this.gameObject.active = true;
      this.transform.rotation = Camera.allCameras.First<Camera>().transform.rotation;
      ApplicationSettings.DebugCamera = this.camera;
    }

    public void DisableDebugCam()
    {
      Debug.Log((object) nameof (DisableDebugCam));
      ApplicationSettings.DebugCamera = LevelHandler.Instance.cam.GUICamera;
      this.gameObject.active = false;
    }

    public static void CreateFromResources()
    {
      if (DebugCameraControl.instance != null)
        return;
      Debug.Log((object) "**************************** CREATING DEBUG CAMERA ********************************");
      DebugCameraControl.instance = ((GameObject) UnityObject.Instantiate(Resources.Load("DebugCamera"))).GetComponent<DebugCameraControl>();
      DebugCameraControl.instance.DisableDebugCam();
    }
  }
}
