using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //Instantiate particle explosion and destroy shield element and enemy projectile
        //Particle explosion
        Destroy(collision.gameObject);
        Destroy(this.gameObject);
        
    }
}
