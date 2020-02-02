using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIScript : MonoBehaviour
{
    [Header("Starting State")]
    [SerializeField] private bool isSleeping;

    [Header("Timing And Speed")]
    [SerializeField] private float searchTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float searchingMoveSpeed;

    private Player target = null;
    private NavMeshAgent thisAgent;
    private SphereCollider robotTrigger;

    //private Vector3[] navigationPoints = new Vector3[20];
    private Vector3 currentDestination;
    private Vector3 randomPosition;
    private Vector3 lastPosition;

    private bool isChasing = false;
    private bool idle = false;
    private bool playerInRange = false;
    private bool onBreak = false;
    private bool chasePlayer = false;

    private void Awake()
    {
        robotTrigger = GetComponent<SphereCollider>();
        thisAgent = GetComponent<NavMeshAgent>();
        if (isSleeping)
        {
            StartCoroutine(AIStates("Sleep", null));
        }
        else
        {
            StartCoroutine(AIStates("Idle", null));
        }
    }

    void Start()
    {
        currentDestination = Game.M.navPointVectors[Random.Range(0, Game.M.navPointVectors.Count)];
    }

    void Update()
    {
        if (!isSleeping)
        {
            if (idle)
            {
                MoveTowardsRandomLocation(moveSpeed);
            }
            else if (target && isChasing)
            {
                if (playerInRange)
                {
                    MoveTowardsTarget(searchingMoveSpeed, target.transform.position);
                }
                else
                {
                    MoveTowardsTarget(searchingMoveSpeed, lastPosition);
                }
            }
            else if (!target && isChasing)
            {
                MoveTowardsRandomLocation(searchingMoveSpeed);
            }
        }       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            Debug.Log("Player entered trigger. Aggro!");
            playerInRange = true;
            isChasing = true;
            idle = false;
            StopAllCoroutines();
            StartCoroutine(AIStates("Chase", other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            Debug.Log("Player left trigger. Lost aggro!");
            playerInRange = false;
            lastPosition = other.transform.position;
            StartCoroutine(AIStates("Chase", other));
        }
    }

    private void MoveTowardsTarget(float MoveSpeed, Vector3 TargetPos)
    {
        if (Vector3.Distance(transform.position, TargetPos) < 1)
        {
            StartCoroutine(AIStates("Alert", null));
        }
        else
        {
            thisAgent.speed = MoveSpeed;
            thisAgent.destination = new Vector3(TargetPos.x, 0, TargetPos.z);
            thisAgent.updateRotation = true;
        }
    }

    private void MoveTowardsRandomLocation(float MoveSpeed)
    {
        if (!onBreak)
        {
            if (Vector3.Distance(transform.position, currentDestination) < 2)
            {
                onBreak = true;
                StartCoroutine(Break());
            }
            else
            {
                thisAgent.updateRotation = true;
                thisAgent.speed = MoveSpeed;
                thisAgent.destination = currentDestination;
            }
        }
    }

    private IEnumerator Break()
    {
        GetComponent<Animator>().SetBool("Walk_Anim", false);
        yield return new WaitForSeconds(5.0f);
        GetComponent<Animator>().SetBool("Walk_Anim", true);
        Vector3 tmp = currentDestination;
        while (tmp == currentDestination)
        {
            currentDestination = Game.M.navPointVectors[Random.Range(0, Game.M.navPointVectors.Count)];
        }
        onBreak = false;
    }

    private IEnumerator AIStates(string state, Collider playerCol)
    {
        switch (state)
        {
            case "Sleep":
                Debug.Log("Sleeping!");
                isSleeping = true;
                robotTrigger.radius = (0.25f);
                GetComponent<Animator>().SetBool("Open_Anim", false);
                yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);
                break;

            case "Idle":
                Debug.Log("Idle Roaming!");
                robotTrigger.radius = (0.75f);
                if (!GetComponent<Animator>().GetBool("Open_Anim"))
                {
                    GetComponent<Animator>().SetBool("Open_Anim", true);                   
                    yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);
                }
                GetComponent<Animator>().SetBool("Walk_Anim", true);
                idle = true;
                break;

            case "Chase":
                Debug.Log("Chase Player!");
                robotTrigger.radius = (1.5f);
                if (isSleeping)
                {
                    GetComponent<Animator>().SetBool("Open_Anim", true);
                    yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);
                    isSleeping = false;
                }
                GetComponent<Animator>().SetBool("Walk_Anim", true);
                if (playerInRange)
                {
                    target = playerCol.GetComponent<Player>();                 
                }
                break;

            case "Alert":
                Debug.Log("Alerted Roaming!");
                GetComponent<Animator>().SetBool("Walk_Anim", false);
                yield return new WaitForSeconds(searchTime);
                GetComponent<Animator>().SetBool("Walk_Anim", true);
                target = null;
                break;            
        }     
    }
}
