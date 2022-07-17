using System.Collections;
using UnityEngine;

public class WandererMovement : CharacterMovement
{
    [Header("Wander Properties")]
    [SerializeField] private Transform[] wanderPoints;
    [SerializeField] private float wanderDelay = 1f;

    [SerializeField] private Vector2 initDirection = Vector2.right;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        foreach (var point in wanderPoints) point.SetParent(transform.parent, true);
    }

    public override void Start()
    {
        base.Start();

        StartRunning(initDirection);
    }

    #endregion

    private IEnumerator ChangeDirection()
    {
        var tempDirection = CurrentDirection;
        StopRunning();

        yield return new WaitForSeconds(wanderDelay);   
        StartRunning(-tempDirection);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WanderPoint"))
        {
            StartCoroutine(ChangeDirection());
        }
    }
}
