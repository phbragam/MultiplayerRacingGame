using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform firePosition;
    public Camera PlayerCamera;



    public DeathRacePlayer DeathRacePlayerProperties;

    private float fireRate;
    private float fireTimer = 0.0f;
    private bool useLaser;
    public LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        fireRate = DeathRacePlayerProperties.fireRate;

        if (DeathRacePlayerProperties.weaponName == "Laser Gun")
        {
            useLaser = true;
        }
        else
        {
            useLaser = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("space"))
        {
            if (fireTimer > fireRate)
            {
                Fire();

                fireTimer = 0.0f;
            }
            
        }

        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }
    }

    public void Fire()
    {
        if (useLaser)
        {
            // Laser codes
            RaycastHit _hit;
            Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.Raycast(ray, out _hit, 200))
            {
                if (!lineRenderer.enabled)
                {
                    lineRenderer.enabled = true;
                }

                lineRenderer.startWidth = 0.3f;
                lineRenderer.endWidth = 0.1f;

                lineRenderer.SetPosition(0, firePosition.position);
                lineRenderer.SetPosition(1, _hit.point);

                StopAllCoroutines();
                StartCoroutine(DisableLaserAfterSecs(0.3f));
            }
        }
        else
        {
            Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            GameObject bulletGameObject = Instantiate(BulletPrefab, firePosition.position, Quaternion.identity);
            bulletGameObject.GetComponent<BulletScript>().Initialize(ray.direction, DeathRacePlayerProperties.bulletSpeed, DeathRacePlayerProperties.damage);
        }
    }

    IEnumerator DisableLaserAfterSecs(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        lineRenderer.enabled = false;
    }
}
