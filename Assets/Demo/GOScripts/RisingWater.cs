using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingWater : MonoBehaviour
{
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<BattleSystem>(out BattleSystem battleSystem))
        {
            battleSystem.DealDamage(gameObject, 1e+7f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<BattleSystem>(out BattleSystem battleSystem))
        {
            battleSystem.DealDamage(gameObject, 1e+7f);
        }
    }
}
