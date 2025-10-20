// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.MonoBehaviour
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using PressPlay.FFWD.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class MonoBehaviour : Behaviour, IUpdateable, IFixedUpdateable
  {
    public virtual void Update()
    {
    }

    public virtual void LateUpdate()
    {
    }

    public virtual void FixedUpdate()
    {
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
    }

    public virtual void OnCollisionStay(Collision collision)
    {
    }

    public virtual void OnCollisionExit(Collision collision)
    {
    }

    public virtual void OnTriggerStay(Collider collider)
    {
    }

    public virtual void OnTriggerEnter(Collider collider)
    {
    }

    public virtual void OnTriggerExit(Collider collider)
    {
    }

    internal override sealed void SetNewId(Dictionary<int, UnityObject> idMap)
    {
      base.SetNewId(idMap);
    }

    internal override sealed void AfterLoad(Dictionary<int, UnityObject> idMap)
    {
      base.AfterLoad(idMap);
    }

    internal override sealed void FixReferences(Dictionary<int, UnityObject> idMap)
    {
      base.FixReferences(idMap);
    }

    public void Invoke(string methodName, float time)
    {
      Application.AddInvokeCall(this, methodName, time, 0.0f);
    }

    public void InvokeRepeating(string methodName, float time, float repeatRate)
    {
      throw new NotImplementedException("Method not implemented");
    }

    public void CancelInvoke(string methodName)
    {
      throw new NotImplementedException("Method not implemented");
    }

    public bool IsInvoking(string methodName) => Application.IsInvoking(this, methodName);

    public void StartCoroutine(IEnumerator routine)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public void StopCoroutine(string methodName)
    {
      throw new NotImplementedException("Method not implemented.");
    }
  }
}
