using UnityEngine;

public class PlayerResources : CharacterResources
{
    [SerializeField] private Transform healthBar;

    public override float Health
    {
        get => base.Health;
        set
        {
            base.Health = value;
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.localScale = new Vector3(Health / maxHealth, 1f, 1f);
    }
}
