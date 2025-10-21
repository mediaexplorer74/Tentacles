// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Transform
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD
{
  public class Transform : Component, IEnumerable
  {
    [ContentSerializer(ElementName = "p", Optional = true)]
    private Vector3 _localPosition;
    [ContentSerializer(ElementName = "s", Optional = true)]
    private Vector3 _localScale = Vector3.one;
    [ContentSerializer(ElementName = "r", Optional = true)]
    private Quaternion _localRotation = Quaternion.identity;
    internal Transform _parent;
    private Matrix _world = Matrix.Identity;
    private bool _hasDirtyWorld = true;

    internal Transform()
    {
      this.localRotation = Quaternion.identity;
      this.localScale = Vector3.one;
    }

    [ContentSerializer(Optional = true, CollectionItemName = "go", FlattenContent = true)]
    private List<GameObject> children { get; set; }

    [ContentSerializerIgnore]
    public Vector3 localPosition
    {
      get => this._localPosition;
      set
      {
        if (float.IsNaN(value.x) || float.IsNaN(value.y) || float.IsNaN(value.z))
          throw new InvalidOperationException();
        this._localPosition = value;
        this.hasDirtyWorld = true;
      }
    }

    [ContentSerializerIgnore]
    public Vector3 localScale
    {
      get => this._localScale;
      set
      {
        if (float.IsNaN(value.x) || float.IsNaN(value.y) || float.IsNaN(value.z))
          throw new InvalidOperationException();
        this._localScale = value;
        this.hasDirtyWorld = true;
      }
    }

    [ContentSerializerIgnore]
    public Quaternion localRotation
    {
      get => this._localRotation;
      set
      {
        if (float.IsNaN(value.x) || float.IsNaN(value.y) || float.IsNaN(value.z) || float.IsNaN(value.w))
          throw new InvalidOperationException();
        this._localRotation = value;
        this.hasDirtyWorld = true;
      }
    }

    [ContentSerializerIgnore]
    public Transform parent
    {
      get => this._parent;
      set
      {
        if (this._parent == value)
          return;
        if (this._parent != null)
          this._parent.children.Remove(this.gameObject);
        Vector3 position = this.position;
        Quaternion rotation = this.rotation;
        Vector3 lossyScale = this.lossyScale;
        this._parent = value;
        if (this._parent == null)
          return;
        if (this._parent.children == null)
          this._parent.children = new List<GameObject>();
        this._parent.children.Add(this.gameObject);
        this.position = position;
        this.rotation = rotation;
        this.hasDirtyWorld = true;
      }
    }

    internal bool hasDirtyWorld
    {
      get => this._hasDirtyWorld;
      private set
      {
        if (this.gameObject != null)
          this.gameObject.isStatic = false;
        this._hasDirtyWorld = value;
        if (this.children == null)
          return;
        for (int index = 0; index < this.children.Count; ++index)
          this.children[index].transform.hasDirtyWorld = true;
      }
    }

    [ContentSerializerIgnore]
    internal Matrix world
    {
      get
      {
        if (this.hasDirtyWorld)
          this.calculateWorld();
        return this._world;
      }
    }

    [ContentSerializerIgnore]
    public Vector3 position
    {
      get => (Vector3) this.world.Translation;
      set
      {
        if (float.IsNaN(value.x) || float.IsNegativeInfinity(value.x))
          value.x = 0.0f;
        if (float.IsNaN(value.y) || float.IsNegativeInfinity(value.y))
          value.y = 0.0f;
        if (float.IsNaN(value.z) || float.IsNegativeInfinity(value.z))
          value.z = 0.0f;
        this.localPosition = this.parent != null ? (Vector3) Microsoft.Xna.Framework.Vector3.Transform((Microsoft.Xna.Framework.Vector3) value, Matrix.Invert(this.parent.world)) : value;
        if (this.rigidbody == null)
          return;
        this.rigidbody.MovePosition(this.position);
      }
    }

    [ContentSerializerIgnore]
    public Vector3 lossyScale
    {
      get
      {
        if (this.parent == null)
          return this.localScale;
        Microsoft.Xna.Framework.Vector3 scale;
        this.world.Decompose(out scale, out Microsoft.Xna.Framework.Quaternion _, out Microsoft.Xna.Framework.Vector3 _);
        return (Vector3) scale;
      }
    }

    [ContentSerializerIgnore]
    public Quaternion rotation
    {
      get
      {
        if (this.parent == null)
          return this.localRotation;
        Microsoft.Xna.Framework.Quaternion rotation;
        this.world.Decompose(out Microsoft.Xna.Framework.Vector3 _, out rotation, out Microsoft.Xna.Framework.Vector3 _);
        return (Quaternion) rotation;
      }
      set
      {
        if (float.IsNaN(value.x) || float.IsNaN(value.y) || float.IsNaN(value.z) || float.IsNaN(value.w))
          throw new InvalidOperationException();
        if (this.parent == null)
          this.localRotation = value;
        else
          this.localRotation = Quaternion.Inverse(this.parent.rotation) * value;
      }
    }

    [ContentSerializerIgnore]
    public Vector3 eulerAngles
    {
      get => this.rotation.eulerAngles;
      set
      {
        if (float.IsNaN(value.x) || float.IsNaN(value.y) || float.IsNaN(value.z))
          throw new InvalidOperationException();
        this.rotation = Quaternion.Euler(value);
      }
    }

    [ContentSerializerIgnore]
    public Vector3 localEulerAngles
    {
      get => this.localRotation.eulerAngles;
      set
      {
        if (float.IsNaN(value.x) || float.IsNaN(value.y) || float.IsNaN(value.z))
          throw new InvalidOperationException();
        this.localRotation = Quaternion.Euler(value);
      }
    }

    [ContentSerializerIgnore]
    public Vector3 right => (Vector3) Microsoft.Xna.Framework.Vector3.Normalize(this.world.Right);

    [ContentSerializerIgnore]
    public Vector3 forward => (Vector3)(-Microsoft.Xna.Framework.Vector3.Normalize(this.world.Forward));

    [ContentSerializerIgnore]
    public Vector3 up => (Vector3)(-Microsoft.Xna.Framework.Vector3.Normalize(this.world.Up));

    [ContentSerializerIgnore]
    public Transform root => this.parent != null ? this.parent.root : this;

    public int childCount => this.children == null ? 0 : this.children.Count;

    private void calculateWorld()
    {
      this._hasDirtyWorld = false;
      this._world = Matrix.CreateScale((Microsoft.Xna.Framework.Vector3) this.localScale) * Matrix.CreateFromQuaternion((Microsoft.Xna.Framework.Quaternion) this.localRotation) * Matrix.CreateTranslation((Microsoft.Xna.Framework.Vector3) this.localPosition);
      if (this._parent == null)
        return;
      this._world *= this._parent.world;
    }

    internal void SetLocalTransform(Matrix m)
    {
      Microsoft.Xna.Framework.Vector3 scale;
      Microsoft.Xna.Framework.Quaternion rotation;
      Microsoft.Xna.Framework.Vector3 translation;
      if (!m.Decompose(out scale, out rotation, out translation))
        return;
      this._localScale = (Vector3) scale;
      this._localRotation = new Quaternion(rotation);
      this._localPosition = (Vector3) translation;
      this.hasDirtyWorld = true;
    }

    internal void SetPositionFromPhysics(Vector3 pos, float ang)
    {
      this.localPosition = this.parent != null ? pos - this.parent.position : pos;
      this.localRotation = Quaternion.AngleAxis(ang, Vector3.up);
    }

    internal override void AfterLoad(Dictionary<int, UnityObject> idMap)
    {
      base.AfterLoad(idMap);
      if (this.children == null)
        return;
      for (int index = 0; index < this.children.Count; ++index)
      {
        this.children[index].isPrefab = this.isPrefab;
        this.children[index].transform._parent = this;
        this.children[index].AfterLoad(idMap);
      }
    }

    internal override UnityObject Clone()
    {
      Transform transform = base.Clone() as Transform;
      if (this.children != null)
      {
        transform.children = new List<GameObject>();
        for (int index = 0; index < this.children.Count; ++index)
          (this.children[index].Clone() as GameObject).transform.parent = transform;
      }
      return (UnityObject) transform;
    }

    internal override void SetNewId(Dictionary<int, UnityObject> idMap)
    {
      base.SetNewId(idMap);
      if (this.children == null)
        return;
      for (int index = 0; index < this.children.Count; ++index)
        this.children[index].SetNewId(idMap);
    }

    internal override void FixReferences(Dictionary<int, UnityObject> idMap)
    {
      if (this.children == null)
        return;
      for (int index = 0; index < this.children.Count; ++index)
        this.children[index].FixReferences(idMap);
    }

    internal void SetActiveRecursively(bool state)
    {
      if (this.children == null)
        return;
      for (int index = 0; index < this.children.Count; ++index)
        this.children[index].SetActiveRecursively(state);
    }

    protected override void Destroy()
    {
      base.Destroy();
      if (this.children == null)
        return;
      for (int index = 0; index < this.children.Count; ++index)
        UnityObject.Destroy((UnityObject) this.children[index]);
    }

    public void Translate(Vector3 translation) => this.Translate(translation, Space.Self);

    public void Translate(Vector3 translation, Space space)
    {
      if (space != Space.Self)
        throw new NotImplementedException("Not implemented yet");
      this.localPosition += translation;
    }

    public void Translate(float x, float y, float z)
    {
      this.Translate(new Vector3(x, y, z), Space.Self);
    }

    public void Translate(float x, float y, float z, Space space)
    {
      this.Translate(new Vector3(x, y, z), space);
    }

    public void Translate(Vector3 translation, Transform relativeTo)
    {
      throw new NotImplementedException("Not implemented yet");
    }

    public void Translate(float x, float y, float z, Transform relativeTo)
    {
      this.Translate(new Vector3(x, y, z), relativeTo);
    }

    public void Rotate(Vector3 axis, float angle, Space relativeTo)
    {
      if (relativeTo == Space.World)
      {
        Matrix fromAxisAngle = Matrix.CreateFromAxisAngle((Microsoft.Xna.Framework.Vector3) axis, angle);
        Matrix.Multiply(ref this._world, ref fromAxisAngle, out this._world);
        this.WorldChanged();
      }
      else
        this.localRotation *= Quaternion.AngleAxis(angle, axis);
    }

    public void Rotate(Vector3 axis, float angle) => this.Rotate(axis, angle, Space.Self);

    public void Rotate(Vector3 eulerAngles, Space space)
    {
      this.Rotate(eulerAngles.x, eulerAngles.y, eulerAngles.z, space);
    }

    public void Rotate(Vector3 eulerAngles)
    {
      this.Rotate(eulerAngles.x, eulerAngles.y, eulerAngles.z, Space.Self);
    }

    public void Rotate(float x, float y, float z) => this.Rotate(x, y, z, Space.Self);

    public void Rotate(float x, float y, float z, Space relativeTo)
    {
      if (relativeTo == Space.World)
      {
        Matrix result;
        Matrix.CreateFromYawPitchRoll(y, x, z, out result);
        Matrix.Multiply(ref this._world, ref result, out this._world);
        this.WorldChanged();
      }
      else
        this.localRotation *= Quaternion.Euler(x, y, z);
    }

    public void LookAt(Vector3 worldPosition, Vector3 worldUp)
    {
      Microsoft.Xna.Framework.Quaternion rotation;
      if (worldPosition == this.position || !Matrix.CreateWorld((Microsoft.Xna.Framework.Vector3) this.position, (Microsoft.Xna.Framework.Vector3) (worldPosition - this.position), (Microsoft.Xna.Framework.Vector3) worldUp).Decompose(out Microsoft.Xna.Framework.Vector3 _, out rotation, out Microsoft.Xna.Framework.Vector3 _) || float.IsNaN(rotation.W))
        return;
      this.localRotation = new Quaternion(rotation);
      if (this.rigidbody == null)
        return;
      this.rigidbody.MoveRotation(this.localRotation);
    }

    public void LookAt(Transform target, Vector3 worldUp)
    {
    }

    public void LookAt(Vector3 worldPosition) => this.LookAt(worldPosition, Vector3.up);

    private void WorldChanged()
    {
      if (this.parent != null)
        throw new NotImplementedException();
      Microsoft.Xna.Framework.Vector3 scale;
      Microsoft.Xna.Framework.Quaternion rotation;
      Microsoft.Xna.Framework.Vector3 translation;
      if (!this._world.Decompose(out scale, out rotation, out translation))
        return;
      this._localScale = (Vector3) scale;
      this._localRotation = new Quaternion(rotation);
      this._localPosition = (Vector3) translation;
      this.hasDirtyWorld = false;
    }

    public IEnumerator GetEnumerator()
    {
      return this.children == null ? (IEnumerator) new List<Transform>().GetEnumerator() : (IEnumerator) this.children.GetEnumerator();
    }

    public Vector3 TransformDirection(Vector3 position)
    {
      return (Vector3) Microsoft.Xna.Framework.Vector3.Transform((Microsoft.Xna.Framework.Vector3) position, this.rotation.quaternion);
    }

    public Vector3 TransformDirection(float x, float y, float z)
    {
      return (Vector3) Microsoft.Xna.Framework.Vector3.Transform(new Microsoft.Xna.Framework.Vector3(x, y, z), this.rotation.quaternion);
    }

    public Vector3 InverseTransformDirection(Vector3 position)
    {
      return (Vector3) Microsoft.Xna.Framework.Vector3.Transform((Microsoft.Xna.Framework.Vector3) position, Microsoft.Xna.Framework.Quaternion.Inverse(this.rotation.quaternion));
    }

    public Vector3 InverseTransformDirection(float x, float y, float z)
    {
      return (Vector3) Microsoft.Xna.Framework.Vector3.Transform(new Microsoft.Xna.Framework.Vector3(x, y, z), Microsoft.Xna.Framework.Quaternion.Inverse(this.rotation.quaternion));
    }

    public Vector3 TransformPoint(Vector3 position)
    {
      return (Vector3) Microsoft.Xna.Framework.Vector3.Transform((Microsoft.Xna.Framework.Vector3) position, this.world);
    }

    public Vector3 TransformPoint(float x, float y, float z)
    {
      return (Vector3) Microsoft.Xna.Framework.Vector3.Transform(new Microsoft.Xna.Framework.Vector3(x, y, z), this.world);
    }

    public Vector3 InverseTransformPoint(Vector3 position)
    {
      return (Vector3) Microsoft.Xna.Framework.Vector3.Transform((Microsoft.Xna.Framework.Vector3) position, Matrix.Invert(this.world));
    }

    public Vector3 InverseTransformPoint(float x, float y, float z)
    {
      return (Vector3) Microsoft.Xna.Framework.Vector3.Transform(new Microsoft.Xna.Framework.Vector3(x, y, z), Matrix.Invert(this.world));
    }

    public void DetachChildren() => throw new NotImplementedException("Not implemented yet");

    public Transform Find(string name)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public bool IsChildOf(Transform parent)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    internal void GetComponentsInChildrenInt(Type type, List<Component> list)
    {
      if (this.children == null)
        return;
      for (int index = 0; index < this.children.Count; ++index)
        this.children[index].GetComponentsInChildren(type, list);
    }

    internal void GetComponentsInChildrenInt<T>(List<Component> list) where T : Component
    {
      if (this.children == null)
        return;
      for (int index = 0; index < this.children.Count; ++index)
        this.children[index].GetComponentsInChildren<T>(list);
    }

    internal Component GetComponentInChildrenInt(Type type)
    {
      if (this.transform.children != null)
      {
        for (int index = 0; index < this.transform.children.Count; ++index)
        {
          Component componentInChildren = this.transform.children[index].GetComponentInChildren(type);
          if (componentInChildren != null)
            return componentInChildren;
        }
      }
      return (Component) null;
    }

    internal void BroadcastMessage(
      string methodName,
      object value,
      SendMessageOptions sendMessageOptions)
    {
      if (this.transform.children == null)
        return;
      for (int index = 0; index < this.children.Count; ++index)
        this.children[index].BroadcastMessage(methodName, value, sendMessageOptions);
    }

    public void DebugDrawLocal()
    {
    }
  }
}
