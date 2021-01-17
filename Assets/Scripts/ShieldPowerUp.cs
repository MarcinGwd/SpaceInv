using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    [SerializeField]int shieldDurability;
    private void OnEnable()
    {
        shieldDurability = 5;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyProjectile")
        {
            shieldDurability--;
            if (shieldDurability == 0)
            {
                this.gameObject.SetActive(false);
            }
            Destroy(other.gameObject);
        }
    }

}

