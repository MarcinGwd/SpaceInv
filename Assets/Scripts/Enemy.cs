using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject enemyProjectile;
    public float tryShootingPeriod=0.3f;
    public float enemySpeed = 2f;
    public int enemyHealth = 2;
    float rightMapBoundary = 6;
    float leftMapBoundary = -6;
    public int enemyPercentShootChance = 10;
    public int points = 20;
    [SerializeField]bool extraProjectiles = false;
    [SerializeField]GameObject explosion;
    AudioSource audio;
    private void FixedUpdate()
    {
        EnemyMovement();
    }
    private void Start()
    {
        //Try to shoot every 0.5s
        InvokeRepeating("ShootChance", 0.2f, tryShootingPeriod);
        audio = GetComponent<AudioSource>();
    }

    void ShootProjectile()
    {

        //Instantiate projectile from this ship position
        Vector2 projectilePosition = new Vector2(transform.position.x,transform.position.y);
        audio.Play();
        Instantiate(enemyProjectile,projectilePosition, Quaternion.identity);
        if (extraProjectiles == true)
        {
            ExtraProjectilesShoot();
        }

    }
    void ExtraProjectilesShoot()
    {
        Instantiate(enemyProjectile, new Vector2(transform.position.x+0.2f, transform.position.y), Quaternion.identity);
        Instantiate(enemyProjectile, new Vector2(transform.position.x+0.1f, transform.position.y), Quaternion.identity);
        Instantiate(enemyProjectile, new Vector2(transform.position.x-0.1f, transform.position.y), Quaternion.identity);
        Instantiate(enemyProjectile, new Vector2(transform.position.x-0.1f, transform.position.y), Quaternion.identity);
    }

    void ShootChance()
    {
        // Shoot chance is equal to enemyPercentShootChance if randomly generated number 
        //is less than enemyPercentShootChance - enemy shoots;
        if (Random.Range(0,100)<enemyPercentShootChance)
        {
            ShootProjectile();
        }
    }    

    void EnemyMovement()
    {
        //Move enemy through the map on x axis and add downward movement
       
            transform.Translate(Vector3.right * Time.deltaTime * enemySpeed);
        transform.Translate(Vector3.down * Time.deltaTime *0.2f);

        //If map boundary reached move player change direction of movement
        if ((transform.position.x > rightMapBoundary) || (transform.position.x < leftMapBoundary))
        {
            enemySpeed = -enemySpeed;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayersProjectile")
        {
            //Destroy players Projectile
            Destroy(other.gameObject);

            Instantiate(explosion, transform.position, Quaternion.identity);
            //Take damage
            enemyHealth--;
           if(enemyHealth < 1)
            {
                Die();
            }
        }
        
    }
    void Die()
    {
        //Add points to player score
        GameManager.Instance.AddPointsToScore(points);
        //Explosion particle effect
        Instantiate(explosion, transform.position, Quaternion.identity);
        //Destroy enemy ship
        Destroy(this.gameObject);
    }
}
