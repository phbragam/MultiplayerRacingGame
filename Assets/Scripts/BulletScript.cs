using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletScript : MonoBehaviour
{
    float bulletDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        Destroy(gameObject);

        if (collision.gameObject.CompareTag("Player"))
        {
            // This if() guarantees that only DoDamage from the targeted object will be called
            // Without this, DoDamage would be called in each targeted player in the scene multiplying the damage
            // Remember that the targeted object, with PhotonView component, exists for all players (differents game windows), but there is only one PhotonView that "is mine"
            if (collision.gameObject.GetComponent<PhotonView>().IsMine)
            {
                collision.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, bulletDamage);
            }
        }
    }

    public void Initialize(Vector3 _direction, float speed, float damage)
    {
        bulletDamage = damage;

        transform.forward = _direction;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = _direction * speed;

    }
}
