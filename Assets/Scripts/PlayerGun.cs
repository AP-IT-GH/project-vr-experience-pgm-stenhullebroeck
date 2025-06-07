using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    private Transform bulletOrigin;
    [SerializeField]
    private ParticleSystem gunParticles;

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
    private float frDeadline;
    private bool firing = false;

    private bool active = false;

    private InputControl rightTrigger;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ammo = magazineSize;
        frDeadline = 1 / fireRate;
        rightTrigger = InputSystem.FindControl("<XRController>{RightHand}/{TriggerButton}");
    }

    private void Update()
    {
        if (!active)
            return;

        rightTrigger ??= InputSystem.FindControl("<XRController>{RightHand}/{TriggerButton}");

        if (rightTrigger is null)
            return;

        if (rightTrigger.ReadValueAsObject() is Single rtValue)
        {
            if (rtValue <= 0.5 && firing)
                StopFiring();
            else if (rtValue > 0.5 && !firing)
                StartFiring();

            if (frTimer < frDeadline)
            {
                if (firing && frTimer == 0f)
                {
                    Fire();
                }
                else if ((frTimer != 0f && !firing) || firing)
                    frTimer += Time.deltaTime;
            }
            else if (frTimer > frDeadline)
            {
                frTimer = 0f;
            }
        }
    }

    public void StartFiring()
    {

        if (fullAutomatic)
        {
            firing = true;
            frTimer = 0f;
        } 
        else
        {
            Fire();
        }
    }

    public void StopFiring()
    {
        firing = false;
    }

    private void Fire()
    {
        if (hasAmmo)
        {
            if (ammo == 0)
                return;
        }

        gunParticles.Play();

        if (Physics.Raycast(bulletOrigin.position, bulletOrigin.forward, out RaycastHit hit, 200f) && hit.collider.TryGetComponent<Character>(out Character character))
        {
            character.TakeDamage(damage);
        }

    }

    public void Activate()
    {
        active = true;
    }

    public void Deactivate()
    {
        active = false;
    }
}
