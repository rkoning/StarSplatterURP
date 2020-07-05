public class HookFireMode : FireMode
{
    private HookProjector projector;

    public override void SetProjector(Projector projector) {
        this.projector = (HookProjector) projector;
    }

    public override void Fire() {
        if (projector.projectile.attached) {
            projector.Detach();
        } else {
            projector.Emit();
        }
    }
}