using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIScript : MonoBehaviour
{
    private Player target = null;
    private bool isSearching = false;
    private Vector3 randomPosition;

    [SerializeField] private Player testController;

    [Header("Starting State")]
    [SerializeField] private bool isSleeping;

    [Header("Triggers For Player Detection")]
    [SerializeField] private SphereCollider sleepStateTrigger, idlingTrigger, searchingStateTrigger;

    [Header("Timing And Speed")]
    [SerializeField] private float searchTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float searchingMoveSpeed;

    private NavMeshAgent thisAgent;

    private Vector3[] navigationPoints = new Vector3[20];
    [SerializeField] private Vector3 currentDestination;

    private bool onBreak = false;

    private void Awake()
    {
        thisAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        for (int i = 0; i < navigationPoints.Length; i++)
        {
            navigationPoints[i] = new Vector3(Random.Range(5, 50), 0, Random.Range(5, 50));
        }
        currentDestination = navigationPoints[Random.Range(0, navigationPoints.Length)];

        if(!Game.M.AllEnemies.Contains(this))
        {
            Game.M.AllEnemies.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {        
        //if (Input.GetKeyDown(KeyCode.E) && target == null)
        //{
        //    StartCoroutine(DetectPlayer(new Collider()));
        //}

        if (isSleeping && !target && !isSearching)
        {
            GetComponent<Animator>().SetBool("Open_Anim", false);
            // sleeping state.
            sleepStateTrigger.enabled = true;
            searchingStateTrigger.enabled = false;
            idlingTrigger.enabled = false;
        }
        else if (target && !isSearching && !isSleeping)
        {
            // alerted state.
            searchingStateTrigger.enabled = true;
            idlingTrigger.enabled = false;
            sleepStateTrigger.enabled = false;

            moveTowardsTarget(searchingMoveSpeed, target.transform.position);
            //moveTowardsRandomLocation(searchingMoveSpeed);
        }
        else if (isSearching && !target && !isSleeping)
        {
            Debug.Log("searching state");
            //alerted roaming state.
            searchingStateTrigger.enabled = true;
            idlingTrigger.enabled = false;
            sleepStateTrigger.enabled = false;
            moveTowardsRandomLocation(searchingMoveSpeed);
        }
        else if (!isSearching && !target && !isSleeping)
        {
            Debug.Log("idle state");
            GetComponent<Animator>().SetBool("Open_Anim", true);
            // idle roaming state.
            idlingTrigger.enabled = true;
            searchingStateTrigger.enabled = false;
            sleepStateTrigger.enabled = false;
            moveTowardsRandomLocation(searchingMoveSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            StartCoroutine(DetectPlayer(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            StartCoroutine(searchForPlayer());
        }
    }

    private IEnumerator DetectPlayer(Collider other)
    {
        if (isSleeping)
        {
            //set animation bool.
            //wait for animation to finish.
            isSleeping = false;
            GetComponent<Animator>().SetBool("Open_Anim", true);
            yield return new WaitForSeconds(3.2f);
        }
        target = other.GetComponent<Player>();             
        GetComponent<Animator>().SetBool("Walk_Anim", true);
        yield return new WaitForSeconds(1f);
        target = testController;
        isSearching = false;
        StopAllCoroutines();
        yield return null;
    }

    private IEnumerator searchForPlayer()
    {
        isSearching = true;
        yield return new WaitForSeconds(searchTime);
    }

    private void moveTowardsTarget(float MoveSpeed, Vector3 TargetPos)
    {
        thisAgent.speed = MoveSpeed;
        thisAgent.destination = new Vector3(TargetPos.x, 0, TargetPos.z);
        thisAgent.updateRotation = true;
    }

    private void moveTowardsRandomLocation(float MoveSpeed)
    {
        if (!onBreak)
        {
            if (Vector3.Distance(transform.position, currentDestination) < 3)
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

    private int count = 0;

    private IEnumerator Break()
    {
        count++;
        Debug.Log(count);
        GetComponent<Animator>().SetBool("Walk_Anim", false);
        yield return new WaitForSeconds(5.0f);
        GetComponent<Animator>().SetBool("Walk_Anim", true);
        Vector3 tmp = currentDestination;
        while (tmp == currentDestination)
        {
            currentDestination = navigationPoints[Random.Range(0, navigationPoints.Length)];
        }
        onBreak = false;
    }
}
