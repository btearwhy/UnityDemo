using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect
{
    public delegate void EventHandler();
    public EventHandler OnAdded;
    public EventHandler OnTick;
    public EventHandler OnRemoved;

    
    public string data;
    [field: NonSerialized]
    public Effect_Data effect_data;

    [field: NonSerialized]
    private EffectContext context;

    public EffectContext Context { get { if (context == null) context = new EffectContext();  return context; } set { context = value; } }
    public Effect() { }

    public Effect(string data) {
        this.data = data;
        effect_data = (Effect_Data)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("effects", data);
        Initialize();
    }

    public virtual void Initialize()
    {
        
    }

    public virtual void Added()
    {
        OnAdded?.Invoke();
    }

    public virtual void Removed()
    {
        OnRemoved?.Invoke();
    }

    public virtual void Apply()
    {
        OnTick?.Invoke();
    }

    internal void SetContext(GameObject instigator, GameObject target)
    {
        Context.Instigator = instigator;
        Context.Target = target;
    }

}


[System.Serializable]
public class EffectContainer : IEnumerable
{
    private List<Effect> effects;
    List<Effect> Effects { get { return effects; } set { effects = value; } }

    public int Count { get => effects.Count; }

    public EffectContainer() { Effects = new List<Effect>(); }

    public EffectContainer(Effect effect)
    {
        Effects = new List<Effect>();
        Effects.Add(effect);
    }

    public Effect this[int key]
    {
        get => effects[key];
        set => effects[key] = value;
    }

    public void Add(Effect effect)
    {
        Effects.Add(effect);
    }

    public IEnumerator<Effect> GetEnumerator()
    {
        foreach(Effect Effect in Effects)
        {
            yield return Effect;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    internal void Remove(Effect effect)
    {
        Effects.Remove(effect);
    }
}

public class EffectContext{
    public GameObject Target;
    public GameObject Instigator;
}