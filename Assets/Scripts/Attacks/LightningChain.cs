using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningChain : MonoBehaviour
{
    public enum chainLightningState
    {
        STAY,
        SEARCH,
        CHASE,
        ATTACK,
        DESPAWN
    }

    [Header("Current state values")]
    public float timeTillCheck = 2.0f;
    public float attackRate = 0.6f;
    public float searchRadius = 5.0f;
    private chainLightningState currentState = chainLightningState.SEARCH;
    private float timer = 0.0f;
    private PlayerScript player;

    [Header("Movement")]
    public float moveSpeed = 20.0f;
    public EnemyScript target;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    //void Update()
    //{
    //    timer += Time.deltaTime
    //}

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer >= timeTillCheck)
        {
            switch (currentState)
            {
                case chainLightningState.SEARCH:
                    SearchForEnemies();
                    break;
                case chainLightningState.STAY:
                    currentState = chainLightningState.SEARCH;
                    break;
            }

            timer = 0;
        }
        else
        {
            switch(currentState)
            {
                case chainLightningState.CHASE:
                    Vector3 direction = (target.transform.position - transform.position).normalized;
                    direction.y = transform.position.y;

                    transform.position += (direction * moveSpeed) * Time.fixedDeltaTime;
                    break;
                //case chainLightningState.ATTACK:
                    //target.DamageEnemy()
            }
        }
    }

    public void SearchForEnemies()
    {
        float closestDistance = searchRadius;
        LayerMask enemy = LayerMask.GetMask("Enemy");
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius, enemy);

        //Don't bother continuing this function if there aren't any enemies nearby
        if (colliders.Length == 0) return;

        foreach(Collider nearbyEnemy in colliders)
        {
            //Trying to find the nearest enemy
            float currentDistance = Vector3.Distance(transform.position, nearbyEnemy.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                target = nearbyEnemy.GetComponent<EnemyScript>();
            }
        }

        CurrentState = chainLightningState.CHASE;
    }

    //Set/get the current lightning state
    public chainLightningState CurrentState
    {
        set { currentState = value; }
        get { return currentState; }
    }
}
