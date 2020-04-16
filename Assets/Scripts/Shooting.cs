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

    // Start is called before the first frame update
    void Start()
    {
        fireRate = DeathRacePlayerProperties.fireRate;
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
        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        GameObject bulletGameObject = Instantiate(BulletPrefab, firePosition.position, Quaternion.identity);
        bulletGameObject.GetComponent<BulletScript>().Initialize(ray.direction, DeathRacePlayerProperties.bulletSpeed, DeathRacePlayerProperties.damage);
    }
}
