using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    private Transform bulletOrigin;

    [Header("Ammo")]
    [SerializeField]
    [Tooltip("If false, gun uses no ammo and requires no reloading")]
    private bool hasAmmo = false;
    [SerializeField]
    private int magazineSize = 30;
    [SerializeField]
    private bool hasUnlimitedMags = true;
    [SerializeField]
    private int numOfMags = 5;
    [Header("Firing characteristics")]
    [SerializeField]
    [Tooltip("Number of bullets shot per minute")]
    private bool fullAutomatic = true;
    [SerializeField]
    private float fireRate = 30;
    [SerializeField]
    private int damage = 5;

    private int ammo;
    private float frTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ammo = magazineSize;
    }

    public void Fire()
    {
        if (hasAmmo)
        {
            if (ammo == 0)
                return;
            else
                ammo--;
        }

        if (Physics.Raycast(bulletOrigin.position, bulletOrigin.forward, out RaycastHit hit, 200f) && hit.collider.TryGetComponent<Character>(out Character character))
        {
            Debug.Log(character.TakeDamage(damage));
        }

    }
}
