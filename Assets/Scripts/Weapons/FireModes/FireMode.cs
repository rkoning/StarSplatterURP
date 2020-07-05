using UnityEngine;

public abstract class FireMode : MonoBehaviour
{

    protected Weapon weapon;
    public Weapon Weapon {
        get { return weapon; }
        set { weapon = value; }
    }

    public virtual void SetProjector(Projector projector) {}
    public abstract void Fire();
    public virtual void Hold() {}
    public virtual void Release() {}
}
