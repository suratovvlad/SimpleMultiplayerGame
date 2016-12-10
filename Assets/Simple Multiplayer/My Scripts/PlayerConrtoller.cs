using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConrtoller : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0.0f, x, 0.0f);
        transform.Translate(0.0f, 0.0f, z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }

    public override void OnStartLocalPlayer()
    {
        //base.OnStartLocalPlayer();
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    [Command]
    void CmdFire()
    {
        // Create the bullet from the prefab
        GameObject bullet = (GameObject) Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity =  bullet.transform.forward * 6.0f;

        // Spawn the bullet on the Clients
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2);
    }
}
