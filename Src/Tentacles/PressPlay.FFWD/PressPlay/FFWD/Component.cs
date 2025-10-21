// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Component
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD.Components;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace PressPlay.FFWD
{
  public abstract class Component : UnityObject
  {
    public Component() => Application.AddNewComponent(this);

    public virtual GameObject gameObject { get; internal set; }

    [ContentSerializerIgnore]
    public string name
    {
      get => this.gameObject != null ? this.gameObject.name : this.GetType().Name;
      set
      {
        if (this.gameObject == null)
          return;
        this.gameObject.name = value;
      }
    }

    public string tag => this.gameObject.tag;

    [ContentSerializerIgnore]
    public Transform transform
    {
      get => this.gameObject == null ? (Transform) null : this.gameObject.transform;
    }

    [ContentSerializerIgnore]
    public Renderer renderer => this.gameObject.renderer;

    [ContentSerializerIgnore]
    public AudioSource audio => this.gameObject.audio;

    [ContentSerializerIgnore]
    public Rigidbody rigidbody
    {
      get => this.gameObject == null ? (Rigidbody) null : this.gameObject.rigidbody;
    }

    [ContentSerializerIgnore]
    public Collider collider
    {
      get => this.gameObject == null ? (Collider) null : this.gameObject.collider;
    }

    public virtual void Awake()
    {
    }

    public virtual void Start()
    {
    }

    public bool CompareTag(string tag) => this.gameObject.CompareTag(tag);

    internal override UnityObject Clone()
    {
      UnityObject unityObject = base.Clone();
      unityObject.isPrefab = false;
      Application.AddNewComponent(unityObject as Component);
      return unityObject;
    }

    private void DoFixReferences(object objectToFix, Dictionary<int, UnityObject> idMap)
    {
      FieldInfo[] fields = objectToFix.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
      for (int index1 = 0; index1 < fields.Length; ++index1)
      {
        if (typeof (UnityObject).IsAssignableFrom(fields[index1].FieldType))
        {
          if (fields[index1].GetValue(objectToFix) is UnityObject unityObject && unityObject.isPrefab)
          {
            if (idMap.ContainsKey(unityObject.GetInstanceID()) && unityObject != idMap[unityObject.GetInstanceID()])
              fields[index1].SetValue(objectToFix, (object) idMap[unityObject.GetInstanceID()]);
          }
          else
            continue;
        }
        if (fields[index1].FieldType.IsArray && typeof (UnityObject).IsAssignableFrom(fields[index1].FieldType.GetElementType()))
        {
          UnityObject[] unityObjectArray = default;
          if (fields[index1].GetValue(objectToFix) is UnityObject[])
          {
            unityObjectArray = unityObjectArray.Clone() as UnityObject[];
            for (int index2 = 0; index2 < unityObjectArray.Length; ++index2)
            {
              if (unityObjectArray[index2] != null && unityObjectArray[index2].isPrefab 
                                && idMap.ContainsKey(unityObjectArray[index2].GetInstanceID()) && unityObjectArray[index2] != idMap[unityObjectArray[index2].GetInstanceID()])
                unityObjectArray[index2] = idMap[unityObjectArray[index2].GetInstanceID()];
            }
          }
          
          fields[index1].SetValue(objectToFix, unityObjectArray);
        }
        if (Application.fixReferences.Contains(fields[index1].FieldType.Name))
          this.DoFixReferences(fields[index1].GetValue(objectToFix), idMap);
      }
    }

    internal override void FixReferences(Dictionary<int, UnityObject> idMap)
    {
      base.FixReferences(idMap);
      if (this.gameObject == null)
        return;
      this.DoFixReferences((object) this, idMap);
    }

    public Component GetComponent(Type type) => this.gameObject.GetComponent(type);

    public Component GetComponent(string type)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public Component[] GetComponents(Type type) => this.gameObject.GetComponents(type);

    public Component GetComponentInChildren(Type type)
    {
      return this.gameObject.GetComponentInChildren(type);
    }

    public T GetComponentInChildren<T>() where T : Component
    {
      return this.gameObject.GetComponentInChildren<T>();
    }

    public T[] GetComponentsInChildren<T>() where T : Component
    {
      return this.gameObject.GetComponentsInChildren<T>();
    }

    public T GetComponent<T>() where T : Component => this.gameObject.GetComponent<T>();

    public T[] GetComponents<T>() where T : Component => this.gameObject.GetComponents<T>();

    public Component[] GetComponentsInChildren(Type type)
    {
      return this.gameObject.GetComponentsInChildren(type);
    }

    public T GetComponentInParents<T>() where T : Component
    {
      return this.gameObject.GetComponentInParents<T>();
    }

    public T[] GetComponentsInParents<T>() where T : Component
    {
      throw new NotImplementedException();
    }

    public Component[] GetComponentsInChildren(Type type, bool includeInactive)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public override string ToString()
    {
      return this.gameObject == null ? this.GetType().Name + " (" + (object) this.GetInstanceID() + ") on its own" : this.GetType().Name + " (" + (object) this.GetInstanceID() + ") on " + this.gameObject.name + " (" + (object) this.gameObject.GetInstanceID() + ") active: " + (object) this.gameObject.active;
    }

    internal bool SendMessage(string methodName, object value)
    {
      Type tp = this.GetType();
      BindingFlags flags = BindingFlags.Instance /*| BindingFlags.InvokeMethod */ | BindingFlags.NonPublic | BindingFlags.Public;
      while (tp != typeof (Component))
      {
        MethodInfo cachedMethod = tp.GetCachedMethod(methodName, flags);
        if (cachedMethod != null)
        {
          MethodInfo methodInfo = cachedMethod;
          object[] parameters;
          if (value != null)
            parameters = new object[1]{ value };
          else
            parameters = (object[]) null;
          methodInfo.Invoke((object) this, parameters);
          return true;
        }
        tp = tp.GetTypeInfo().BaseType;
        flags = BindingFlags.Instance | /*BindingFlags.InvokeMethod |*/ BindingFlags.NonPublic;
      }
      return false;
    }
  }
}
