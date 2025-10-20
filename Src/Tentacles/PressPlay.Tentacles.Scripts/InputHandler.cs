// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.InputHandler
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class InputHandler : MonoBehaviour
  {
    [ContentSerializerIgnore]
    public Plane screenInputPlane;
    private bool collectInputFlickVectors;
    private List<PressPlay.FFWD.Vector2> inputList = new List<PressPlay.FFWD.Vector2>();
    private List<PressPlay.FFWD.Vector2> inputDifferenceList = new List<PressPlay.FFWD.Vector2>();
    [ContentSerializerIgnore]
    public InputHandler.ControlSchemeButton[] inputFlickActivationScheme = new InputHandler.ControlSchemeButton[1]
    {
      InputHandler.ControlSchemeButton.leftMouse
    };
    [ContentSerializerIgnore]
    public InputHandler.ControlSchemeScreenPosition inputFlickPositionScheme;
    [ContentSerializerIgnore]
    public InputHandler.ControlSchemeButton[] keepConnectionScheme = new InputHandler.ControlSchemeButton[1]
    {
      InputHandler.ControlSchemeButton.leftMouse
    };
    [ContentSerializerIgnore]
    public InputHandler.ControlSchemeButton[] shootClawScheme = new InputHandler.ControlSchemeButton[1]
    {
      InputHandler.ControlSchemeButton.rightMouseDown
    };
    [ContentSerializerIgnore]
    public InputHandler.ControlSchemeButton[] shootTentacleScheme = new InputHandler.ControlSchemeButton[1]
    {
      InputHandler.ControlSchemeButton.leftMouseDown
    };
    [ContentSerializerIgnore]
    public InputHandler.ControlSchemeButton[] controlTentacleScheme = new InputHandler.ControlSchemeButton[1]
    {
      InputHandler.ControlSchemeButton.leftMouse
    };
    [ContentSerializerIgnore]
    public InputHandler.ControlSchemeScreenPosition inputPositionScheme;
    [ContentSerializerIgnore]
    public InputHandler.ControlSchemeVector tentacleDirectionScheme = InputHandler.ControlSchemeVector.leftAnalog;
    [ContentSerializerIgnore]
    public InputHandler.ControlSchemeVector clawDirectionScheme = InputHandler.ControlSchemeVector.leftAnalog;
    [ContentSerializerIgnore]
    public bool turnOff;
    private float lastUpdateTime;
    private PressPlay.FFWD.Vector2 inputPosition;
    private PressPlay.FFWD.Vector2 lastInputPosition;
    [ContentSerializerIgnore]
    public float dTime;
    private float leftAnalogXAxis;
    private float leftAnalogYAxis;
    [ContentSerializerIgnore]
    public float leftAnalogDeadZone = 0.2f;
    private float leftAnalogMagnitude;
    [ContentSerializerIgnore]
    public float leftAnalogCurvePow = 1.6f;
    private PressPlay.FFWD.Vector2 leftAnalogVector = PressPlay.FFWD.Vector2.zero;
    private float rightAnalogXAxis;
    private float rightAnalogYAxis;
    [ContentSerializerIgnore]
    public float rightAnalogDeadZone = 0.2f;
    private float rightAnalogMagnitude;
    [ContentSerializerIgnore]
    public float rightAnalogCurvePow = 1.6f;
    private PressPlay.FFWD.Vector2 rightAnalogVector = PressPlay.FFWD.Vector2.zero;
    private PressPlay.FFWD.Vector2 wasdVector = PressPlay.FFWD.Vector2.zero;
    private PressPlay.FFWD.Vector2 arrowKeysVector = PressPlay.FFWD.Vector2.zero;
    private float leftTriggerAxis = -1f;
    private float rightTriggerAxis = -1f;
    [ContentSerializerIgnore]
    public float leftTriggerAxisCurvePow = 1f;
    [ContentSerializerIgnore]
    public float rightTriggerAxisCurvePow = 1f;
    private static InputHandler instance;
    public static bool isLoaded = false;
    private static InputHandler preInstance;

    public static InputHandler Instance
    {
      get
      {
        if (InputHandler.instance == null)
          Debug.LogError("Attempt to access instance of InputHandler singleton earlier than Start or without it being attached to a GameObject.");
        return InputHandler.instance;
      }
    }

    public override void Awake()
    {
      if (InputHandler.instance != null)
      {
        Debug.LogError("Cannot have two instances of InputHandler. Self destruction in 3...");
      }
      else
      {
        InputHandler.isLoaded = true;
        InputHandler.instance = this;
        this.lastUpdateTime = Time.realtimeSinceStartup;
      }
    }

    private bool GetControlSchemeButton(InputHandler.ControlSchemeButton _scheme)
    {
      if (this.turnOff)
        return false;
      switch (_scheme)
      {
        case InputHandler.ControlSchemeButton.buttonA:
          return Input.GetButton("ButtonA");
        case InputHandler.ControlSchemeButton.buttonB:
          return Input.GetButton("ButtonB");
        case InputHandler.ControlSchemeButton.buttonX:
          return Input.GetButton("ButtonX");
        case InputHandler.ControlSchemeButton.buttonY:
          return Input.GetButton("ButtonY");
        case InputHandler.ControlSchemeButton.rightTriggerButton:
          return Input.GetButton("RightTriggerButton");
        case InputHandler.ControlSchemeButton.leftTriggerButton:
          return Input.GetButton("LeftTriggerButton");
        case InputHandler.ControlSchemeButton.rightTriggerAxisHalfWay:
          return (double) this.rightTriggerAxis > 0.0;
        case InputHandler.ControlSchemeButton.rightTriggerAxisHairTrigger:
          return (double) this.rightTriggerAxis > -0.800000011920929;
        case InputHandler.ControlSchemeButton.leftTriggerAxisHalfWay:
          return (double) this.leftTriggerAxis > 0.0;
        case InputHandler.ControlSchemeButton.leftTriggerAxisHairTrigger:
          return (double) this.leftTriggerAxis > -0.800000011920929;
        case InputHandler.ControlSchemeButton.rightAnalogButton:
          return Input.GetButton("RightAnalogButton");
        case InputHandler.ControlSchemeButton.leftAnalogButton:
          return Input.GetButton("LeftAnalogButton");
        case InputHandler.ControlSchemeButton.bothAnalogButtons:
          return Input.GetButton("LeftAnalogButton") && Input.GetButton("RightAnalogButton");
        case InputHandler.ControlSchemeButton.buttonDownA:
          return Input.GetButtonDown("ButtonA");
        case InputHandler.ControlSchemeButton.buttonDownB:
          return Input.GetButtonDown("ButtonB");
        case InputHandler.ControlSchemeButton.buttonDownX:
          return Input.GetButtonDown("ButtonX");
        case InputHandler.ControlSchemeButton.buttonDownY:
          return Input.GetButtonDown("ButtonY");
        case InputHandler.ControlSchemeButton.rightAnalogButtonDown:
          return Input.GetButtonDown("RightAnalogButton");
        case InputHandler.ControlSchemeButton.leftAnalogButtonDown:
          return Input.GetButtonDown("LeftAnalogButton");
        case InputHandler.ControlSchemeButton.leftMouse:
          return Input.GetMouseButton(0);
        case InputHandler.ControlSchemeButton.rightMouse:
          return Input.GetMouseButton(1);
        case InputHandler.ControlSchemeButton.middleMouse:
          return Input.GetMouseButton(2);
        case InputHandler.ControlSchemeButton.leftMouseDown:
          return Input.GetMouseButtonDown(0);
        case InputHandler.ControlSchemeButton.rightMouseDown:
          return Input.GetMouseButtonDown(1);
        case InputHandler.ControlSchemeButton.middleMouseDown:
          return Input.GetMouseButtonDown(2);
        case InputHandler.ControlSchemeButton.leftMouseUp:
          return Input.GetMouseButtonUp(0);
        case InputHandler.ControlSchemeButton.rightMouseUp:
          return Input.GetMouseButtonUp(1);
        case InputHandler.ControlSchemeButton.middleMouseUp:
          return Input.GetMouseButtonUp(2);
        default:
          return false;
      }
    }

    private float GetControlSchemeAxis(InputHandler.ControlSchemeAxis _scheme)
    {
      if (this.turnOff)
        return 0.0f;
      switch (_scheme)
      {
        case InputHandler.ControlSchemeAxis.rightAnalogX:
          return this.rightAnalogXAxis;
        case InputHandler.ControlSchemeAxis.rightAnalogY:
          return this.rightAnalogYAxis;
        case InputHandler.ControlSchemeAxis.rightAnalogMagnitude:
          return this.rightAnalogMagnitude;
        case InputHandler.ControlSchemeAxis.leftAnalogX:
          return this.leftAnalogXAxis;
        case InputHandler.ControlSchemeAxis.leftAnalogY:
          return this.leftAnalogYAxis;
        case InputHandler.ControlSchemeAxis.leftAnalogMagnitude:
          return this.leftAnalogMagnitude;
        case InputHandler.ControlSchemeAxis.rightTriggerAxis:
          return this.rightTriggerAxis;
        case InputHandler.ControlSchemeAxis.leftTriggerAxis:
          return this.leftTriggerAxis;
        case InputHandler.ControlSchemeAxis.rightTriggerAxis01:
          return (float) ((0.800000011920929 + (double) this.rightTriggerAxis) / 1.7999999523162842);
        case InputHandler.ControlSchemeAxis.leftTriggerAxis01:
          return (float) ((0.800000011920929 + (double) this.leftTriggerAxis) / 1.7999999523162842);
        default:
          return 0.0f;
      }
    }

    private PressPlay.FFWD.Vector2 GetControlSchemeScreenPosition(
      InputHandler.ControlSchemeScreenPosition _scheme)
    {
      return this.turnOff || _scheme != InputHandler.ControlSchemeScreenPosition.mousePosition ? PressPlay.FFWD.Vector2.zero : Input.mousePosition;
    }

    private PressPlay.FFWD.Vector2 GetControlSchemeVector(InputHandler.ControlSchemeVector _scheme)
    {
      if (this.turnOff)
        return PressPlay.FFWD.Vector2.zero;
      switch (_scheme)
      {
        case InputHandler.ControlSchemeVector.rightAnalog:
          return this.rightAnalogVector;
        case InputHandler.ControlSchemeVector.leftAnalog:
          return this.leftAnalogVector;
        case InputHandler.ControlSchemeVector.wasd:
          return this.wasdVector;
        case InputHandler.ControlSchemeVector.arrowKeys:
          return this.arrowKeysVector;
        default:
          return PressPlay.FFWD.Vector2.zero;
      }
    }

    public override void FixedUpdate()
    {
      if (this.turnOff)
        return;
      this.DoUpdate();
    }

    public void DoUpdate()
    {
      this.dTime = Time.realtimeSinceStartup - this.lastUpdateTime;
      this.lastUpdateTime = Time.realtimeSinceStartup;
      if (!this.collectInputFlickVectors && this.GetOneButtonActive(this.inputFlickActivationScheme))
      {
        this.inputList.Clear();
        this.inputDifferenceList.Clear();
        this.collectInputFlickVectors = true;
      }
      if (this.collectInputFlickVectors)
      {
        this.inputList.Add(this.GetInputScreenPosition());
        if (!this.GetOneButtonActive(this.inputFlickActivationScheme))
          this.collectInputFlickVectors = false;
      }
      this.rightAnalogXAxis = Input.GetAxis("RightAnalogX");
      this.rightAnalogYAxis = -Input.GetAxis("RightAnalogY");
      this.rightAnalogVector.x = this.rightAnalogXAxis;
      this.rightAnalogVector.y = this.rightAnalogYAxis;
      this.rightAnalogMagnitude = this.rightAnalogVector.magnitude;
      this.rightAnalogMagnitude = (float) (((double) this.rightAnalogMagnitude - (double) this.rightAnalogDeadZone) / (1.0 - (double) this.rightAnalogDeadZone));
      if ((double) this.rightAnalogMagnitude < 0.0)
      {
        this.rightAnalogMagnitude = 0.0f;
        this.rightAnalogVector.x = 0.0f;
        this.rightAnalogVector.y = 0.0f;
      }
      if ((double) this.rightAnalogMagnitude > 0.0)
        this.rightAnalogVector /= this.rightAnalogMagnitude;
      this.rightAnalogMagnitude = Mathf.Pow(this.rightAnalogMagnitude, this.rightAnalogCurvePow);
      this.rightAnalogVector *= this.rightAnalogMagnitude;
      this.leftAnalogXAxis = Input.GetAxis("LeftAnalogX");
      this.leftAnalogYAxis = -Input.GetAxis("LeftAnalogY");
      this.leftAnalogVector.x = this.leftAnalogXAxis;
      this.leftAnalogVector.y = this.leftAnalogYAxis;
      this.leftAnalogMagnitude = this.leftAnalogVector.magnitude;
      this.leftAnalogMagnitude = (float) (((double) this.leftAnalogMagnitude - (double) this.leftAnalogDeadZone) / (1.0 - (double) this.leftAnalogDeadZone));
      if ((double) this.leftAnalogMagnitude < 0.0)
      {
        this.leftAnalogMagnitude = 0.0f;
        this.leftAnalogVector.x = 0.0f;
        this.leftAnalogVector.y = 0.0f;
      }
      if ((double) this.leftAnalogMagnitude > 0.0)
        this.leftAnalogVector /= this.leftAnalogMagnitude;
      this.leftAnalogMagnitude = Mathf.Pow(this.leftAnalogMagnitude, this.leftAnalogCurvePow);
      this.leftAnalogVector *= this.leftAnalogMagnitude;
      this.leftTriggerAxis = Mathf.Pow(Input.GetAxis("LeftTriggerAxis"), this.leftAnalogCurvePow);
      this.rightTriggerAxis = Mathf.Pow(Input.GetAxis("RightTriggerAxis"), this.rightAnalogCurvePow);
      this.lastInputPosition = this.inputPosition;
      this.inputPosition = this.GetControlSchemeScreenPosition(this.inputPositionScheme);
    }

    private PressPlay.FFWD.Vector2 GetLongestVectorScheme(
      InputHandler.ControlSchemeVector[] _schemes)
    {
      if (_schemes.Length == 0)
        return PressPlay.FFWD.Vector2.zero;
      PressPlay.FFWD.Vector2 controlSchemeVector = this.GetControlSchemeVector(_schemes[0]);
      for (int index = 1; index < _schemes.Length; ++index)
      {
        if ((double) this.GetControlSchemeVector(_schemes[index]).magnitude > (double) controlSchemeVector.magnitude)
          controlSchemeVector = this.GetControlSchemeVector(_schemes[index]);
      }
      return controlSchemeVector;
    }

    private float GetHighestAxisScheme(InputHandler.ControlSchemeAxis[] _schemes)
    {
      if (_schemes.Length == 0)
        return 0.0f;
      float controlSchemeAxis = this.GetControlSchemeAxis(_schemes[0]);
      for (int index = 1; index < _schemes.Length; ++index)
      {
        if ((double) this.GetControlSchemeAxis(_schemes[index]) > (double) controlSchemeAxis)
          controlSchemeAxis = this.GetControlSchemeAxis(_schemes[index]);
      }
      return controlSchemeAxis;
    }

    private bool GetOneButtonActive(InputHandler.ControlSchemeButton[] _schemes)
    {
      for (int index = 0; index < _schemes.Length; ++index)
      {
        if (this.GetControlSchemeButton(_schemes[index]))
          return true;
      }
      return false;
    }

    public bool GetKeepConnection() => this.GetOneButtonActive(this.keepConnectionScheme);

    public bool GetShootTentacle() => this.GetOneButtonActive(this.shootTentacleScheme);

    public bool GetShootClaw() => this.GetOneButtonActive(this.shootClawScheme);

    public bool GetControlTentacle() => this.GetOneButtonActive(this.controlTentacleScheme);

    public PressPlay.FFWD.Vector2 GetInputScreenPositionDifference()
    {
      return this.inputPosition - this.lastInputPosition;
    }

    public PressPlay.FFWD.Vector2 GetInputScreenPosition()
    {
      return this.GetControlSchemeScreenPosition(this.inputPositionScheme);
    }

    public List<PressPlay.FFWD.Vector2> GetInputArraylist() => this.inputList;

    public PressPlay.FFWD.Vector2 GetTentacleDirection()
    {
      return this.GetControlSchemeVector(this.tentacleDirectionScheme);
    }

    public PressPlay.FFWD.Vector2 GetClawDirection()
    {
      return this.GetControlSchemeVector(this.clawDirectionScheme);
    }

    public static PressPlay.FFWD.Vector3 InputVecToWorldVec(Camera _veiwingCam, PressPlay.FFWD.Vector2 _inputVec)
    {
      return _inputVec.x * _veiwingCam.transform.right + _inputVec.y * _veiwingCam.transform.up;
    }

    public ScreenRayCheckHit ScreenRayCheck(PressPlay.FFWD.Ray _ray, LayerMask _layerMask)
    {
      this.screenInputPlane.Normal = (Microsoft.Xna.Framework.Vector3) PressPlay.FFWD.Vector3.up;
      ScreenRayCheckHit screenRayCheckHit = new ScreenRayCheckHit();
      float enter;
      this.screenInputPlane.Raycast(_ray, out enter);
      screenRayCheckHit.position = _ray.origin + _ray.direction * enter;
      RaycastHit hitInfo;
      if (Physics.Pointcast((PressPlay.FFWD.Vector2) screenRayCheckHit.position, out hitInfo, (int) _layerMask))
      {
        screenRayCheckHit.hitObjectInLayer = true;
        screenRayCheckHit.position = hitInfo.point;
        screenRayCheckHit.obj = hitInfo.collider.gameObject;
        return screenRayCheckHit;
      }
      screenRayCheckHit.hitObjectInLayer = false;
      return screenRayCheckHit;
    }

    public enum ControlSchemeButton
    {
      buttonA,
      buttonB,
      buttonX,
      buttonY,
      rightTriggerButton,
      leftTriggerButton,
      rightTriggerAxisHalfWay,
      rightTriggerAxisHairTrigger,
      leftTriggerAxisHalfWay,
      leftTriggerAxisHairTrigger,
      rightAnalogButton,
      leftAnalogButton,
      bothAnalogButtons,
      buttonDownA,
      buttonDownB,
      buttonDownX,
      buttonDownY,
      rightAnalogButtonDown,
      leftAnalogButtonDown,
      leftMouse,
      rightMouse,
      middleMouse,
      leftMouseDown,
      rightMouseDown,
      middleMouseDown,
      leftMouseUp,
      rightMouseUp,
      middleMouseUp,
    }

    public enum ControlSchemeAxis
    {
      rightAnalogX,
      rightAnalogY,
      rightAnalogMagnitude,
      leftAnalogX,
      leftAnalogY,
      leftAnalogMagnitude,
      rightTriggerAxis,
      leftTriggerAxis,
      rightTriggerAxis01,
      leftTriggerAxis01,
    }

    public enum ControlSchemeVector
    {
      rightAnalog,
      leftAnalog,
      wasd,
      arrowKeys,
    }

    public enum ControlSchemeScreenPosition
    {
      mousePosition,
    }
  }
}
