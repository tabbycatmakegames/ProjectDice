using UnityEngine;

public class Enemy : Character
{
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }

    public override void Die()
    {
        base.Die();

        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());
    }
}
