
using System.Collections;
using TMPro;
using UnityEngine;

public class ProjectileWeapons : MonoBehaviour
{
    public Collider playerCollider;
    //bullet
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce, maxDistance;

    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    //recoil
    public Rigidbody playerRb;
    public float RecoilForce;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    //Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;



    //bug fixing :D
    public bool allowInvoke = true;

    private void Awake()
    {
        //make mag full
        bulletsLeft = magazineSize;
        readyToShoot = true;
        
    }

    private void Update()
    {
        MyInput();

        //Set amo display, if it exists
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletPerTap + " / " + magazineSize / bulletPerTap);
    }

    private void MyInput()
    {
        //check if allowed to hold down fire button
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading && this.gameObject.activeSelf) Reload();
        //reload automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0 && this.gameObject.activeSelf) Reload();

        //Shooting
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullet shot to 0
            bulletsShot = 0;

            Shoot();
        }

    }

    private void Shoot()
    {
        readyToShoot = false;

        //Find the exact hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Create a LayerMask to ignore the player layer (assuming the player's layer is named "Player")
        int playerLayer = LayerMask.NameToLayer("Player");
        int gunLayer = LayerMask.NameToLayer("EquippedWeapon"); // Add this if your gun has a specific layer
        int layerMask = ~(1 << playerLayer | 1 << gunLayer); // Ignore both player and gun layers

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
        {
            targetPoint = hit.point; // Exact hit position
        }
        else
        {
            targetPoint = ray.GetPoint(maxDistance); // Default to max distance if no hit     
        }
        //Calculate direction from attackingPoint to target
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calc spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //apply spread to player direction
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        // Normalize the direction with spread
        Vector3 finalDirection = directionWithSpread.normalized;

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        // Normalize the direction after applying spread
        directionWithSpread = directionWithSpread.normalized;

        // Rotate bullet to shoot direction
        currentBullet.transform.forward = finalDirection;

        // Get the bullet's collider
        Collider bulletCollider = currentBullet.GetComponent<Collider>();
        if (playerCollider != null && bulletCollider != null)
        {
            // Ignore collision with player
            Physics.IgnoreCollision(playerCollider, bulletCollider);
        }

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(finalDirection * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);


        //Instantiate muzzle flash
        if (muzzleFlash != null) StartCoroutine(FlashMuzzle());

        
        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function (if not ready invoked), with your timeBetweenShooting
        if(allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;

            //add recoil to player
            playerRb.AddForce(-directionWithSpread.normalized * RecoilForce, ForceMode.Impulse);
        }

        //if more than one bulletPerTap make sure to repeat shoot func
        if (bulletsShot < bulletPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    IEnumerator FlashMuzzle()
    {
        ////turn off plane with muzzle flash
        muzzleFlash.SetActive(true);

        //Wait for a brief moment to display the flash(you can adjust the time here)

        yield return new WaitForSeconds(.1f);

        //turn on plane with muzzle flash
        muzzleFlash.SetActive(false);
    }
    private void ResetShot()
    {
        //Allow shooting and invoke again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void OnDisable() => reloading = false;
    

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

   


}
