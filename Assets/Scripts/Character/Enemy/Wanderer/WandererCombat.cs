using UnityEngine;

public class WandererCombat : CharacterCombat
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float knockBackForce = 10f;
    [SerializeField] private ParticleSystem bloodSplashPrefab;

    private Wanderer _wanderer;
    private Conditioner _conditioner;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _wanderer = GetComponent<Wanderer>();
        _conditioner = GetComponent<Conditioner>();
    }

    #endregion

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            var point = other.contacts[0].point;
            var player = other.transform.GetComponent<Player>();
            var direction = (player.transform.position - transform.position).normalized;

            if (_conditioner.Evaluate(player.CurrentDice))
            {
                _wanderer.KnockBack(-direction, player.PlayerCombat.knockBackForce);
                _wanderer.TakeDamage(player.PlayerCombat.damage);
                Instantiate(bloodSplashPrefab, point, Quaternion.identity).transform.up = -direction;
            }
            else
            {
                player.KnockBack(direction, knockBackForce);
                player.TakeDamage(damage);
                Instantiate(bloodSplashPrefab, point, Quaternion.identity).transform.up = -direction;
            }
        }
    }
}
