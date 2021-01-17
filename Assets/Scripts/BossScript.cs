using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField] GameObject enemyProjectile;
    public float tryShootingPeriod = 0.3f;
    public float enemySpeed = 2f;
    public int enemyHealth = 2;
    float rightMapBoundary = 6;
    float leftMapBoundary = -6;
    public int enemyPercentShootChance = 10;
    public int points = 20;
    [SerializeField] bool extraProjectiles = false;
    [SerializeField] GameObject player;
    bool isAttacking = false;
    [SerializeField]GameObject homingProjectile;
    AudioSource audio;
    
    private void Start()
    {
        //Try to shoot every 0.5s
        StartCoroutine("BossShootingPattern");
        audio = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        EnemyMovement();
    }
    IEnumerator BossShootingPattern()
    {

        yield return new WaitForSeconds(1);
        ExtraProjectilesShoot();
        yield return new WaitForSeconds(1);
        ShootBossProjectile();
        yield return new WaitForSeconds(1);
        ExtraProjectilesShoot();
        yield return new WaitForSeconds(1);
        ShootBossProjectile();
        yield return new WaitForSeconds(1);
        isAttacking = true;
        StartCoroutine(ShootHomingProjectiles());
        yield return new WaitForSeconds(6);
        StartCoroutine(EnemyMultipleshots());
        yield return new WaitForSeconds(3);
        StartCoroutine(BossShootingPattern());

    }
    #region BossAttacks
    IEnumerator ShootHomingProjectiles()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                audio.Play();
                Instantiate(homingProjectile, new Vector2(transform.position.x + 0.3f, transform.position.y), homingProjectile.transform.rotation);
                yield return new WaitForSeconds(0.3f);
            }
            yield return new WaitForSeconds(1);
        }
    }
    void ShootProjectile()
    {
        //Instantiate projectile from this ship position
        Vector2 projectilePosition = new Vector2(transform.position.x, transform.position.y);
        audio.Play();
        Instantiate(enemyProjectile, projectilePosition, Quaternion.identity);
        if (extraProjectiles == true)
        {
            ExtraProjectilesShoot();
        }

    }
    void ExtraProjectilesShoot()
    {
        //Shoot 4 projectiles
        Instantiate(enemyProjectile, new Vector2(transform.position.x + 0.2f, transform.position.y), Quaternion.identity);
        Instantiate(enemyProjectile, new Vector2(transform.position.x + 0.1f, transform.position.y), Quaternion.identity);
        Instantiate(enemyProjectile, new Vector2(transform.position.x - 0.1f, transform.position.y), Quaternion.identity);
        Instantiate(enemyProjectile, new Vector2(transform.position.x - 0.1f, transform.position.y), Quaternion.identity);
    }

    void ShootChance()
    {
        // Shoot chance is equal to enemyPercentShootChance if randomly generated number 
        //is less than enemyPercentShootChance - enemy shoots;
        if (Random.Range(0, 100) < enemyPercentShootChance)
        {
            ShootProjectile();
        }
    }

    
    IEnumerator EnemyMultipleshots()
    {
        for(int i=0;i<10;i++)
        {
            yield return new WaitForSeconds(0.3f);
            ShootBossProjectile();
        }
        isAttacking = false;
    }
    void ShootBossProjectile()
    {
        audio.Play();
        //Shoot 8 projectiles
        Instantiate(enemyProjectile, new Vector2(transform.position.x + 1.8f, transform.position.y), Quaternion.identity);
        Instantiate(enemyProjectile, new Vector2(transform.position.x + 1.3f, transform.position.y), Quaternion.identity);
        Instantiate(enemyProjectile, new Vector2(transform.position.x + 0.8f, transform.position.y), Quaternion.identity);
        Instantiate(enemyProjectile, new Vector2(transform.position.x + 0.3f, transform.position.y), Quaternion.identity);
        Instantiate(enemyProjectile, new Vector2(transform.position.x - 0.3f, transform.position.y), Quaternion.identity);
        Instantiate(enemyProjectile, new Vector2(transform.position.x - 0.8f, transform.position.y), Quaternion.identity);
        Instantiate(enemyProjectile, new Vector2(transform.position.x - 1.3f, transform.position.y), Quaternion.identity);
        Instantiate(enemyProjectile, new Vector2(transform.position.x - 1.8f, transform.position.y), Quaternion.identity);
    }
    #endregion
    void EnemyMovement()
    {
        //Move enemy through the map on x axis and add downward movement
        if(!isAttacking)
        {
            transform.Translate(Vector3.right * Time.deltaTime * enemySpeed);
        }

        //If map boundary reached move player change direction of movement
        if (((transform.position.x > rightMapBoundary) || (transform.position.x < leftMapBoundary)) )
        {
            enemySpeed = -enemySpeed;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayersProjectile")
        {
            //Destroy players Projectile
            Destroy(other.gameObject);

            //Take damage
            enemyHealth--;
            if (enemyHealth < 1)
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
        //Destroy enemy ship
        Destroy(this.gameObject);

    }
}
