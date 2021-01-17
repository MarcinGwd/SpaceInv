using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField]float speed = 2f;
    [SerializeField] GameObject explosion;
    void FixedUpdate()
    {
        Move();
        CheckIfOutOfBounds();
    }

    void Move()
    {
        //Move projectile forward when instantiated
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    void CheckIfOutOfBounds()
    {
        //Check if the projectile is out of bounds and destroy it if it is out of bounds;
        if (transform.position.y < -5.5f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        Instantiate(explosion, transform.position, Quaternion.identity);
        if (collision.collider.tag == "Shield"|| collision.collider.tag == "PlayersProjectile")
        {
       
            Destroy(collision.gameObject);
            
            Destroy(this.gameObject);


        }
    }    

}
