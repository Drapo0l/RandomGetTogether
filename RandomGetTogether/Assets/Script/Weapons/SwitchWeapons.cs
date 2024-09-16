using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapons : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Transform> weapons; // List to store weapons
    [SerializeField] private Transform gunContainer;
    [SerializeField] private Camera fpsCam;
    [SerializeField] private Transform player;

    [Header("Keys")]
    [SerializeField] private KeyCode[] keys;

    [Header("Settings")]
    [SerializeField] private float switchTime;
    [SerializeField] private float pickUpRange;
    [SerializeField] private float dropForwardForce, dropUpwardForce;

    [Header("Weapon Settings")]
    private const int maxWeapons = 3; // Limit number of weapons
    private int selectedWeapon = 0;
    private float timeSinceLastSwitch = 0f;

    [Header("Layer Masks")]
    public LayerMask ignoreLayerMask; // Assign this in the inspector to ignore equipped weapons layer

    private Collider playerCollider;

    private void Start()
    {
        // Get the player's capsule collider (assumed to be a child of the player)
        playerCollider = player.GetComponentInChildren<Collider>();

        // Get the player's collider for future reference
        SelectWeapon(selectedWeapon);
        timeSinceLastSwitch = 0f;
    }

    private void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        // Handle switching weapons with keys
        for (int i = 0; i < Mathf.Min(keys.Length, weapons.Count); i++)
        {
            if (Input.GetKey(keys[i]) && timeSinceLastSwitch >= switchTime)
            {
                selectedWeapon = i;
            }
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon(selectedWeapon);
        }

        timeSinceLastSwitch += Time.deltaTime;

        // Handle weapon pickup
        CheckForWeaponPickup();

        // Handle weapon drop
        if (Input.GetKeyDown(KeyCode.G) && weapons.Count > 0)
        {
            DropWeapon(selectedWeapon);
        }
    }

    private void SelectWeapon(int weaponIndex)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] != null)
            {
                weapons[i].gameObject.SetActive(i == weaponIndex);

                // Ignore collision with the player collider if the weapon is equipped
                Collider weaponCollider = weapons[i].GetComponent<Collider>();
                if (weaponCollider != null)
                {
                    Physics.IgnoreCollision(playerCollider, weaponCollider, true);
                }
            }
        }

        timeSinceLastSwitch = 0;
        Debug.Log("Selected new weapon..");
    }

    private void CheckForWeaponPickup()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Raycast to detect nearby weapon to pick up
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, pickUpRange, ~ignoreLayerMask))
            {
                if (hit.transform.CompareTag("Weapon"))
                {
                    int availableIndex = weapons.FindIndex(w => w == null);
                    if (weapons.Count < maxWeapons || availableIndex != -1)
                    {
                        PickUpWeapon(hit.transform);
                    }
                    else
                    {
                        DropWeapon(selectedWeapon);  // Drop currently selected weapon if over the limit
                        PickUpWeapon(hit.transform);
                    }
                }
            }
        }
    }

    private void PickUpWeapon(Transform weapon)
    {
        // Add weapon to the list and parent it to the gun container
        weapon.SetParent(gunContainer);
        weapon.localPosition = Vector3.zero;
        weapon.localRotation = Quaternion.Euler(Vector3.zero);

        // Change layer to equipped weapon
        weapon.gameObject.layer = LayerMask.NameToLayer("EquippedWeapon");

        // Find the first empty slot (null) in the weapons list
        int availableIndex = weapons.FindIndex(w => w == null);

        if (availableIndex != -1)
        {
            // Add weapon to the list in the first available empty slot
            weapons[availableIndex] = weapon;

            // Update selected weapon to the newly picked-up weapon
            selectedWeapon = availableIndex;

            // Select the newly picked-up weapon
            SelectWeapon(selectedWeapon);
        }
        else if (weapons.Count < maxWeapons)
        {
            // Add weapon to the list if there is space
            weapons.Add(weapon);

            // Update selected weapon to the newly picked-up weapon
            selectedWeapon = weapons.Count - 1;

            // Select the newly picked-up weapon
            SelectWeapon(selectedWeapon);
        }

        // Disable physics on the weapon
        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Ensure shooting script is enabled
        ProjectileWeapons weaponScript = weapon.GetComponent<ProjectileWeapons>();
        if (weaponScript != null)
        {
            weaponScript.enabled = true; // enableshooting
        }

        Collider coll = weapon.GetComponent<Collider>();
        if (coll != null)
        {
            coll.isTrigger = true;
        }

        if (weapons.Count < 3)
            weapons.Add(weapon);  // Add to list of weapons
        //selectedWeapon = weapons.Count - 1;  // Equip the newly picked-up weapon

        Debug.Log("Picked up weapon: " + weapon.name);
    }

    private void DropWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weapons.Count || weapons[weaponIndex] == null) return;  // Invalid index

        // Detach the weapon from the player
        Transform weaponToDrop = weapons[weaponIndex];
        weaponToDrop.SetParent(null);

        // Change layer to normal weapon
        weaponToDrop.gameObject.layer = LayerMask.NameToLayer("Weapons");

        // Enable physics
        Rigidbody rb = weaponToDrop.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;// Re-enable physics
            rb.useGravity = true;    // Turn gravity back on

            rb.AddForce(fpsCam.transform.forward * dropForwardForce, ForceMode.Impulse);
            rb.AddForce(fpsCam.transform.up * dropUpwardForce, ForceMode.Impulse);

            // Add drag to slow it down over time
            rb.drag = 2f;  // Adjust drag value based on how quickly you want it to slow down
            rb.angularDrag = 1f;  // Slow down any spinning
        }

        // Ensure shooting script is disabled
        ProjectileWeapons weaponScript = weaponToDrop.GetComponent<ProjectileWeapons>();
        if (weaponScript != null)
        {
            weaponScript.enabled = false; // Disable shooting
        }

        // Re-enable the weapon's collider as a solid collider (not a trigger)
        Collider coll = weaponToDrop.GetComponent<Collider>();
        if (coll != null)
        {
            coll.isTrigger = false;

            // Turn off collision between player's collider and weapon's collider
            if (playerCollider != null)
            {
                Physics.IgnoreCollision(playerCollider, coll, true);
            }
        }

        // Instead of removing the weapon, set the slot to null
        weapons[weaponIndex] = null;

        // Adjust the selected weapon index if needed (but don’t alter the list length)
        if (weapons.Exists(w => w != null))  // Check if there are any non-null weapons
        {
            selectedWeapon = Mathf.Clamp(selectedWeapon, 0, weapons.Count - 1);
            SelectWeapon(selectedWeapon);
        }

        Debug.Log("Dropped weapon: " + weaponToDrop.name);
    }
}