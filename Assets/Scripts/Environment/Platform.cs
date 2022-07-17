using UnityEngine;

public class Platform : MonoBehaviour
{
    #region Unity Event

    public virtual void Awake()
    {
        var collider = GetComponent<BoxCollider2D>();
        if (!collider) return;

        var sprite = GetComponentInChildren<SpriteRenderer>();
        collider.size = new Vector2(sprite.size.x, sprite.size.y);
    }

    #endregion
}
