
using TMPro;
using UnityEngine;

public class ProjectileWeapons : MonoBehaviour
{
    public Collider playerCollider;
    //bullet
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

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
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        //reload automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

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

        //check if ray hits something
        Vector3 targetPoint;
        if(Physics.Raycast(ray,out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); // point far away from player

        //Calculate direction from attackingPoint to target
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        // Declare finalDirection outside the if statement
        Vector3 finalDirection;

        if (spread > 0)
        {
            //Calc spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            //apply spread to player direction
            Vector3 directionWithSpread = fpsCam.transform.forward + new Vector3(x, y, 0); //Just add spread to last direction

            // Normalize the direction with spread
            finalDirection = directionWithSpread.normalized;

        }
        else
        {
            // If spread is 0, use the exact direction without modifying it
            finalDirection = directionWithoutSpread.normalized;
        }

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        //Roatate bullet to shoot direction
        currentBullet.transform.forward = finalDirection.normalized;

        // Get the bullet's collider
        Collider bulletCollider = currentBullet.GetComponent<Collider>();

        if (playerCollider != null && bulletCollider != null)
        {
            Physics.IgnoreCollision(playerCollider, bulletCollider);
        }

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(finalDirection.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);
      

        //Instantiate muzzle flash
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function (if not ready invoked), with your timeBetweenShooting
        if(allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;

            //add recoil to player
            playerRb.AddForce(-finalDirection.normalized * RecoilForce, ForceMode.Impulse);
        }

        //if more than one bulletPerTap make sure to repeat shoot func
        if (bulletsShot < bulletPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        //Allow shooting and invoke again
        readyToShoot = true;
        allowInvoke = true;
    }

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
