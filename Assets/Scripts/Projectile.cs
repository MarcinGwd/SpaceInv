using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]float speed = 4f;
    [SerializeField] GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        CheckIfOutOfBounds();
    }

    void Move()
    {
        //Move projectile forward when instantiated
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void CheckIfOutOfBounds()
    {
        //Check if the projectile is out of bounds and destroy it if it is out of bounds;
        if(transform.position.y>5.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Particle effect
        Instantiate(explosion, transform.position, Quaternion.identity);
        if (collision.collider.tag == "Shield")
        {
            //Destroy shield
            Destroy(collision.gameObject);
            //Destroy projectile
            Destroy(this.gameObject);


        }
        Destroy(this.gameObject);

    }
}
