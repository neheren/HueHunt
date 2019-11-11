using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{

    private NavMeshAgent navAgent;

    public float height = 50;
    public float width = 50;

    public float fieldOfView = 10f;

    private bool chasing = false;

    private AudioSource audioSource;

    public bool debug;
    public AudioClip walk;
    public AudioClip chase;

    public float speed = 3.5f;
    public GameObject player;

    private Vector3 distVector;
    private Vector3[] positions;

    [SerializeField]
    public float waitTimer = 1f;
    public bool npcPauseAtPoint = false;

    private float timer = 0f;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        positions = new Vector3[4];
        audioSource = GetComponent<AudioSource>();
        navAgent.speed = speed;
    }

    void Start()
    {
        CreateSquareOfPoints(height, width);
        distVector = positions[0];

        debug = true;
    }

    void Update()
    {
        
        //If distance between centers of the circles is grerater than sum of its radiuses, the circles do not touch. if its smaller, they do
        if (Vector3.Distance(GetComponent<Transform>().position, player.GetComponent<Transform>().position) < (fieldOfView + player.GetComponent<DetectionRadius>().detectionRadius)){

            AttackEnemy();
        }
        else
        Patrol();
       
    }

    private void AttackEnemy()
    {
        if (audioSource.clip == walk)
        {
            audioSource.clip = chase;
            audioSource.Play();

        }
        chasing = true;
        navAgent.SetDestination(player.GetComponent<Transform>().position);


        if (Vector3.Distance(GetComponent<Transform>().position, player.GetComponent<Transform>().position) < 2f)
        {
            Debug.Log((GetComponent<Transform>().position + ", " + player.GetComponent<Transform>().position));
            GetComponent<KillPLayer>().KillThePlayer();
        }
    }

    private void Patrol(){
        if (navAgent.remainingDistance <= 0.01f || chasing == true){
            if (npcPauseAtPoint){

                timer += Time.deltaTime; 
                if (timer >= waitTimer){
                    SetNewDestination();
                    timer = 0f;
                    chasing = false;
                }
            }
            else{
                SetNewDestination();
               // Debug.Log(GetComponent<Transform>().position + "," + player.GetComponent<Transform>().position);

                chasing = false;
                audioSource.clip = walk;
                audioSource.Play();

            }


        }

    }

    private int counter = 0;
    private void SetNewDestination(){
        if (counter == positions.Length - 1)
            counter = 0;
        else
            counter++;

        distVector = positions[counter];

        navAgent.SetDestination(distVector);
    }


    private void CreateSquareOfPoints(float width, float height){
         Vector3 startingPos = GetComponent<Transform>().position; //bottom left
        positions[0] = startingPos;
        positions[1] = new Vector3(startingPos.x, startingPos.y + 1 , startingPos.z + height);
        positions[2] = new Vector3(startingPos.x + width , startingPos.y + 1, startingPos.z + height);
        positions[3] = new Vector3(startingPos.x + width, startingPos.y + 1, startingPos.z);

        if (debug){
            for (int i = 0; i < positions.Length; i++)
            {
                if (i != positions.Length - 1)
                    Debug.DrawLine(positions[i], positions[i + 1], Color.red, 100, false);
                else
                    Debug.DrawLine(positions[0], positions[i], Color.red, 100, false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawSphere(GetComponent<Transform>().position, fieldOfView);
            Gizmos.DrawWireSphere(GetComponent<Transform>().position, fieldOfView);

            if (positions != null)
            {
                for (int i = 0; i < positions.Length; i++)
                {
                    if (i != positions.Length - 1)
                        Gizmos.DrawLine(positions[i], positions[i + 1]);
                    else
                        Gizmos.DrawLine(positions[0], positions[i]);
                }
            }

        }
        
    }
}
