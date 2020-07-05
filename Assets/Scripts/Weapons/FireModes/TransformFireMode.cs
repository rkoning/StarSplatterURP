using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFireMode : FireMode
{
    public FireMode nextFireMode;

    public enum InputTransform {
        Fire, Hold, Release
    }

    public InputTransform FireTo = InputTransform.Fire;
    public InputTransform HoldTo = InputTransform.Hold;
    public InputTransform ReleaseTo = InputTransform.Release;

    private Dictionary<InputTransform, Action> mapping = new Dictionary<InputTransform, Action>();

    private void Start() {
        mapping.Add(InputTransform.Fire, nextFireMode.Fire);
        mapping.Add(InputTransform.Hold, nextFireMode.Hold);    
        mapping.Add(InputTransform.Release, nextFireMode.Release);    
    }

    public override void Fire() {
        mapping[FireTo]();
    }

    public override void Hold() {
        if (HoldTo == InputTransform.Fire) {
            weapon.Fire();
        } else {
            mapping[HoldTo]();
        }
    }

    public override void Release() {
        mapping[ReleaseTo]();
    }

}