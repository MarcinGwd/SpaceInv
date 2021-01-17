using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    float rightMapBoundary = 6;
    [SerializeField] float shipRowSize = 0.5f;
    float leftMapBoundary = -6;
    float enemyMovePeriod=0.5f;
    Transform enemyHolder;
    float enemySpeed;
    private void Start()
    {
        InvokeRepeating("MoveEnemy",0.3f,enemyMovePeriod);
        
    }
    public void Spawn(int enemies)
    {
        
    }
    // Update is called once per frame
    void MoveEnemy()
    {
        
            transform.Translate(Vector3.right * Time.deltaTime * enemySpeed);
       
        //If map boundary reached move player 1 row lower and change direction of movement
        if ((transform.position.x > rightMapBoundary) || (transform.position.x < leftMapBoundary))
        {
            enemySpeed = -enemySpeed;
            enemyHolder.position += Vector3.down * shipRowSize;
        }
    }
}
