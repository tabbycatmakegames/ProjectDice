using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] private float liftForce = 50f;
    private Rigidbody2D _target;

    private Conditioner _conditioner;
    private Player _player;

    #region Unity Event

    private void Awake()
    {
        _conditioner = GetComponent<Conditioner>();
    }

    private void FixedUpdate()
    {
        if (_target && _conditioner.Evaluate(_player.CurrentDice))
        {
            _target.AddForce(transform.up * liftForce);
        }
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _target = other.GetComponent<Rigidbody2D>();
            _player = other.GetComponent<Player>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _target = null;
        }
    }
}
