using UnityEngine;

public class CharacterResources : MonoBehaviour
{
    private Character _character;

    [Header("Health")]
    public float maxHealth = 100f;
    private float _health;
    public virtual float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (value <= 0) _character.Die();
        }
    }

    #region Unity Event

    public virtual void Awake()
    {
        _character = GetComponent<Character>();
    }

    public virtual void Start()
    {
        Health = maxHealth;
    }

    #endregion
}
