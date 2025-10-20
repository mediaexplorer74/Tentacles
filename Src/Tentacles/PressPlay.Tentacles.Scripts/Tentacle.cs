// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.Tentacle
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class Tentacle : MonoBehaviour
  {
    public TentacleVisualStats visualStats;
    private TentacleJoint[] joints;
    private TentacleStats stats;
    public bool useAnchor;
    private TentacleJoint anchor;
    private TentacleJoint body;
    private TentacleJoint tip;
    private LineDrawerXZ lineDrawer;
    private Vector3 bodyNormal;
    private Vector3 tipNormal;
    private Vector3[] jointPositions;
    private bool doHoseMechanics;

    public virtual void Initialize(
      TentacleStats _stats,
      TentacleJoint _body,
      TentacleJoint _tip,
      Material _tentacleMaterial)
    {
      this.Initialize(_stats, _body, (TentacleJoint) null, _tip, false, _tentacleMaterial);
    }

    public virtual void Initialize(
      TentacleStats _stats,
      TentacleJoint _body,
      TentacleJoint _anchor,
      TentacleJoint _tip,
      bool _useAnchor,
      Material _tentacleMaterial)
    {
      this.tip = _tip;
      this.body = _body;
      this.stats = _stats;
      this.anchor = _anchor;
      this.useAnchor = _useAnchor;
      this.CreateJoints();
      this.lineDrawer = (LineDrawerXZ) this.GetComponent(typeof (LineDrawerXZ));
      if (this.lineDrawer == null)
        this.lineDrawer = this.gameObject.AddComponent<LineDrawerXZ>();
      this.lineDrawer.startWidth = this.visualStats.startWidth;
      this.lineDrawer.endWidth = this.visualStats.endWidth;
      this.lineDrawer.material = _tentacleMaterial;
      this.lineDrawer.Initialize(this.visualStats.joints + 2);
    }

    public TentacleJoint GetJoint(int index) => this.joints[index];

    public void ShowAsAvailable()
    {
    }

    public void ShowAsUnavailable()
    {
    }

    public void SetBodyNormal(Vector3 _normal)
    {
      this.bodyNormal = _normal;
      this.body.SetNormal(this.bodyNormal);
    }

    public void SetTipNormal(Vector3 _normal)
    {
      this.tipNormal = _normal;
      this.tip.SetNormal(this.tipNormal);
    }

    private void DestroyJoints()
    {
      if (this.joints == null)
        return;
      for (int index = 0; index < this.joints.Length; ++index)
        UnityObject.Destroy((UnityObject) this.joints[index]);
    }

    private void CreateJoints()
    {
      this.DestroyJoints();
      int length = Mathf.Max(this.visualStats.joints, 2);
      this.joints = new TentacleJoint[length];
      this.jointPositions = new Vector3[length];
      for (int index = 0; index < this.joints.Length; ++index)
      {
        TentacleJoint tentacleJoint = (TentacleJoint) new GameObject()
        {
          name = ("Tentacle Joint " + (object) index)
        }.AddComponent(typeof (TentacleJoint));
        this.joints[index] = tentacleJoint;
        this.jointPositions[index] = new Vector3(0.0f, 0.0f, 0.0f);
      }
      for (int _index = 0; _index < this.joints.Length; ++_index)
      {
        if (_index == 0)
          this.joints[0].Initialize(this.body, this.joints[1], this, _index, this.visualStats);
        else if (_index == this.joints.Length - 1)
          this.joints[_index].Initialize(this.joints[_index - 1], this.tip, this, _index, this.visualStats);
        else
          this.joints[_index].Initialize(this.joints[_index - 1], this.joints[_index + 1], this, _index, this.visualStats);
      }
    }

    public override void LateUpdate()
    {
      for (int index1 = 0; index1 < this.visualStats.physicsIterations; ++index1)
      {
        for (int index2 = 0; index2 < this.joints.Length; ++index2)
        {
          this.joints[index2].DoUpdate();
          if (this.doHoseMechanics)
          {
            if (index2 == 0)
              this.joints[index2].DoHoseMechanicUpdate(this.bodyNormal, this.joints[index2].frontConnection.directionFromFront);
            else if (index2 == this.joints.Length - 1)
              this.joints[index2].DoHoseMechanicUpdate(this.joints[index2].backConnection.directionFromBack, this.tip.transform.forward * 1.8f);
            else
              this.joints[index2].DoHoseMechanicUpdate(this.joints[index2].backConnection.directionFromBack, this.joints[index2].frontConnection.directionFromFront);
          }
        }
        for (int index3 = 0; index3 < this.joints.Length; ++index3)
          this.joints[this.joints.Length - index3 - 1].DoUpdate();
      }
      if (this.lineDrawer == null)
        return;
      if (this.jointPositions == null || this.jointPositions.Length != this.joints.Length + 2)
        this.jointPositions = new Vector3[this.joints.Length + 2];
      this.jointPositions[0] = this.body.transform.position;
      this.jointPositions[this.joints.Length + 1] = this.tip.transform.position;
      for (int index = 0; index < this.joints.Length; ++index)
        this.jointPositions[index + 1] = this.joints[index].transform.position;
      this.lineDrawer.DrawLine(this.jointPositions);
    }

    public void Reset()
    {
      for (int index = 0; index < this.joints.Length; ++index)
        this.joints[index].MoveTowardsBackConnection(1f);
    }
  }
}
