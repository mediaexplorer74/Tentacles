// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.GameObject
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
  public class GameObject : UnityObject
  {
    [ContentSerializer(Optional = true)]
    public string name;
    [ContentSerializer(Optional = true)]
    public int layer;
    [ContentSerializer(ElementName = "active", Optional = true)]
    private bool _active = true;
    [ContentSerializer(Optional = true)]
    public string tag;
    [ContentSerializer(ElementName = "isStatic", Optional = true)]
    private bool _isStatic = true;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private Collider _collider;
    private Renderer _renderer;
    protected AudioSource _audio;
    [ContentSerializer(ElementName = "cs", CollectionItemName = "c", Optional = true)]
    private List<Component> components;
    private static List<Component> locatorList = new List<Component>(50);

    public GameObject()
    {
      this.components = new List<Component>();
      if (Application.loadingScene)
        return;
      this.AddComponent<Transform>(new Transform());
    }

    internal GameObject(bool isPrefab)
    {
      this.isPrefab = isPrefab;
      this.components = new List<Component>();
      this.AddComponent<Transform>(new Transform());
    }

    public GameObject(string name)
    {
      this.name = name;
      this.components = new List<Component>();
      this.AddComponent<Transform>(new Transform());
    }

    [ContentSerializerIgnore]
    public bool active
    {
      get => this._active;
      set
      {
        this._active = value;
        Application.UpdateGameObjectActive(this.components);
      }
    }

    [ContentSerializerIgnore]
    public bool isStatic
    {
      get => this._isStatic;
      set
      {
        if (value == this._isStatic)
          return;
        this._isStatic = value;
        if (this.collider == null || this.rigidbody != null)
          return;
        this.collider.SetStatic(this._isStatic);
      }
    }

    [ContentSerializerIgnore]
    internal int ComponentCount => this.components.Count;

    [ContentSerializerIgnore]
    public Rigidbody rigidbody
    {
      get
      {
        if (this._rigidbody == null)
          this._rigidbody = this.GetComponent<Rigidbody>();
        return this._rigidbody;
      }
    }

    [ContentSerializerIgnore]
    public Transform transform
    {
      get
      {
        if (this._transform == null)
          this._transform = this.GetComponent<Transform>();
        return this._transform;
      }
    }

    [ContentSerializerIgnore]
    public Collider collider
    {
      get
      {
        if (this._collider == null)
          this._collider = this.GetComponent<Collider>();
        return this._collider;
      }
    }

    [ContentSerializerIgnore]
    public Renderer renderer
    {
      get
      {
        if (this._renderer == null)
          this._renderer = this.GetComponent<Renderer>();
        return this._renderer;
      }
    }

    [ContentSerializerIgnore]
    public AudioSource audio
    {
      get
      {
        if (this._audio == null)
          this._audio = this.GetComponent<AudioSource>();
        return this._audio;
      }
    }

    internal override void AfterLoad(Dictionary<int, UnityObject> idMap)
    {
      base.AfterLoad(idMap);
      for (int index = 0; index < this.components.Count; ++index)
      {
        this.components[index].isPrefab = this.isPrefab;
        this.components[index].AfterLoad(idMap);
        this.components[index].gameObject = this;
      }
    }

    public T AddComponent<T>() where T : Component => (T) this.AddComponent(typeof (T));

    public T AddComponent<T>(T component) where T : Component
    {
      if ((object) component is Transform && this.components.Count > 0)
        throw new InvalidOperationException("A GameObject already has a Transform");
      this.components.Add((Component) component);
      component.gameObject = this;
      component.isPrefab = this.isPrefab;
      return component;
    }

    public Component AddComponent(Type tp)
    {
      return this.AddComponent<Component>(Activator.CreateInstance(tp) as Component);
    }

    internal void RemoveComponent(Component component)
    {
      if (!this.components.Remove(component))
        return;
      component.gameObject = (GameObject) null;
    }

    internal override UnityObject Clone()
    {
      GameObject gameObject = base.Clone() as GameObject;
      gameObject.name = this.name + "(Clone)";
      gameObject.isPrefab = false;
      gameObject._transform = (Transform) null;
      gameObject._rigidbody = (Rigidbody) null;
      gameObject._collider = (Collider) null;
      gameObject._renderer = (Renderer) null;
      gameObject._audio = (AudioSource) null;
      gameObject.components = new List<Component>();
      for (int index = 0; index < this.components.Count; ++index)
        gameObject.AddComponent<Component>(this.components[index].Clone() as Component);
      gameObject.active = true;
      return (UnityObject) gameObject;
    }

    internal override void SetNewId(Dictionary<int, UnityObject> idMap)
    {
      base.SetNewId(idMap);
      for (int index = 0; index < this.components.Count; ++index)
        this.components[index].SetNewId(idMap);
    }

    internal override void FixReferences(Dictionary<int, UnityObject> idMap)
    {
      base.FixReferences(idMap);
      for (int index = 0; index < this.components.Count; ++index)
        this.components[index].FixReferences(idMap);
    }

    internal void OnTriggerEnter(Collider collider)
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (this.components[index] is MonoBehaviour)
          (this.components[index] as MonoBehaviour).OnTriggerEnter(collider);
      }
    }

    internal void OnTriggerStay(Collider collider)
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (this.components[index] is MonoBehaviour)
          (this.components[index] as MonoBehaviour).OnTriggerStay(collider);
      }
    }

    internal void OnTriggerExit(Collider collider)
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (this.components[index] is MonoBehaviour)
          (this.components[index] as MonoBehaviour).OnTriggerExit(collider);
      }
    }

    internal void OnCollisionEnter(Collision collision)
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (this.components[index] is MonoBehaviour)
          (this.components[index] as MonoBehaviour).OnCollisionEnter(collision);
      }
    }

    internal void OnCollisionStay(Collision collision)
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (this.components[index] is MonoBehaviour)
          (this.components[index] as MonoBehaviour).OnCollisionStay(collision);
      }
    }

    internal void OnCollisionExit(Collision collision)
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (this.components[index] is MonoBehaviour)
          (this.components[index] as MonoBehaviour).OnCollisionExit(collision);
      }
    }

    public T GetComponent<T>() where T : Component
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (this.components[index] is T)
          return this.components[index] as T;
      }
      return default (T);
    }

    public Component GetComponent(Type type)
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (type.IsAssignableFrom(this.components[index].GetType()))
          return this.components[index];
      }
      return (Component) null;
    }

    public T[] GetComponents<T>() where T : Component
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (this.components[index] is T)
          GameObject.locatorList.Add((Component) (this.components[index] as T));
      }
      T[] components = new T[GameObject.locatorList.Count];
      for (int index = 0; index < GameObject.locatorList.Count; ++index)
        components[index] = (T) GameObject.locatorList[index];
      GameObject.locatorList.Clear();
      return components;
    }

    public Component[] GetComponents(Type type)
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (type.IsAssignableFrom(this.components[index].GetType()))
          GameObject.locatorList.Add(this.components[index]);
      }
      Component[] array = GameObject.locatorList.ToArray();
      GameObject.locatorList.Clear();
      return array;
    }

    public Component[] GetComponentsInChildren(Type type)
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (type.IsAssignableFrom(this.components[index].GetType()))
          GameObject.locatorList.Add(this.components[index]);
      }
      this.transform.GetComponentsInChildrenInt(type, GameObject.locatorList);
      Component[] array = GameObject.locatorList.ToArray();
      GameObject.locatorList.Clear();
      return array;
    }

    internal void GetComponentsInChildren(Type type, List<Component> list)
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (type.IsAssignableFrom(this.components[index].GetType()))
          list.Add(this.components[index]);
      }
      this.transform.GetComponentsInChildrenInt(type, list);
    }

    public T[] GetComponentsInChildren<T>() where T : Component
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (this.components[index] is T component)
          GameObject.locatorList.Add((Component) component);
      }
      this.transform.GetComponentsInChildrenInt<T>(GameObject.locatorList);
      T[] componentsInChildren = new T[GameObject.locatorList.Count];
      for (int index = 0; index < GameObject.locatorList.Count; ++index)
        componentsInChildren[index] = (T) GameObject.locatorList[index];
      GameObject.locatorList.Clear();
      return componentsInChildren;
    }

    internal void GetComponentsInChildren<T>(List<Component> list) where T : Component
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (this.components[index] is T component)
          list.Add((Component) component);
      }
      this.transform.GetComponentsInChildrenInt<T>(list);
    }

    public Component GetComponentInChildren(Type type)
    {
      Component componentInChildrenInt = this.transform.GetComponentInChildrenInt(type);
      if (componentInChildrenInt != null)
        return componentInChildrenInt;
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (type.IsAssignableFrom(this.components[index].GetType()))
          return this.components[index];
      }
      return (Component) null;
    }

    public T GetComponentInChildren<T>() where T : Component
    {
      return (T) this.GetComponentInChildren(typeof (T));
    }

    public T GetComponentInParents<T>() where T : Component
    {
      GameObject gameObject = this;
      do
      {
        T component = gameObject.GetComponent<T>();
        if ((object) component != null)
          return component;
        gameObject = gameObject.transform.parent != null ? gameObject.transform.parent.gameObject : (GameObject) null;
      }
      while (gameObject != null);
      return default (T);
    }

    public T[] GetComponentsInParents<T>() where T : Component
    {
      for (GameObject gameObject = this; gameObject != null; gameObject = gameObject.GetParent())
        GameObject.locatorList.AddRange((IEnumerable<Component>) gameObject.GetComponents<T>());
      T[] componentsInParents = new T[GameObject.locatorList.Count];
      for (int index = 0; index < GameObject.locatorList.Count; ++index)
        componentsInParents[index] = (T) GameObject.locatorList[index];
      GameObject.locatorList.Clear();
      return componentsInParents;
    }

    private GameObject GetParent()
    {
      return this.transform.parent == null ? (GameObject) null : this.transform.parent.gameObject;
    }

    public void SetActiveRecursively(bool state)
    {
      this.active = state;
      this.transform.SetActiveRecursively(state);
    }

    public bool CompareTag(string tag)
    {
      return this.tag.Equals(tag, StringComparison.CurrentCultureIgnoreCase);
    }

    public static GameObject FindWithTag(string tag)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public static GameObject FindGameObjectWithTag(string p) => throw new NotImplementedException();

    public static GameObject[] FindGameObjectsWithTag(string tag)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public static GameObject Find(string name)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    protected override void Destroy()
    {
      for (int index = 0; index < this.components.Count; ++index)
        UnityObject.Destroy((UnityObject) this.components[index]);
      base.Destroy();
    }

    public void SendMessageUpwards(string methodName)
    {
      this.SendMessageUpwards(methodName, (object) null, SendMessageOptions.RequireReceiver);
    }

    public void SendMessageUpwards(string methodName, SendMessageOptions sendMessageOptions)
    {
      this.SendMessageUpwards(methodName, (object) null, sendMessageOptions);
    }

    public void SendMessageUpwards(
      string methodName,
      object value,
      SendMessageOptions sendMessageOptions)
    {
      this.SendMessage(methodName, value, sendMessageOptions);
      if (this.transform.parent == null)
        return;
      this.transform.parent.gameObject.SendMessageUpwards(methodName, value, sendMessageOptions);
    }

    public void SendMessage(string methodName)
    {
      this.SendMessage(methodName, (object) null, SendMessageOptions.RequireReceiver);
    }

    public void SendMessage(string methodName, SendMessageOptions sendMessageOptions)
    {
      this.SendMessage(methodName, (object) null, sendMessageOptions);
    }

    public void SendMessage(string methodName, object value)
    {
      this.SendMessage(methodName, value, SendMessageOptions.RequireReceiver);
    }

    public void SendMessage(string methodName, object value, SendMessageOptions sendMessageOptions)
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        Component component = this.components[index];
        if (!(component is Transform))
          component.SendMessage(methodName, value);
      }
    }

    public void BroadcastMessage(string methodName)
    {
      this.BroadcastMessage(methodName, (object) null, SendMessageOptions.RequireReceiver);
    }

    public void BroadcastMessage(string methodName, object value)
    {
      this.BroadcastMessage(methodName, value, SendMessageOptions.RequireReceiver);
    }

    public void BroadcastMessage(
      string methodName,
      object value,
      SendMessageOptions sendMessageOptions)
    {
      this.SendMessage(methodName, value, sendMessageOptions);
      this.transform.BroadcastMessage(methodName, value, sendMessageOptions);
    }

    public override string ToString()
    {
      return this.name + "(" + (object) this.GetInstanceID() + ") " + (object) this.active;
    }
  }
}
