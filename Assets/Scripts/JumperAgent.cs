using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;



public class JumperAgent : Agent
{
    public TextMeshPro scoreBoard;
    public Vector3 jump = new Vector3(0.0f, 4.0f, 0.0f);
    public float jumpForce = 10.0f;


    public float maxEnemiesHitBeforeRestart = 10f;
    public float maxPointsHitBeforeRestart = 10f;


    private bool isGrounded;

    private int oldScore = 0;
    private int oldPunishment = 0;
    private ScoreHelper scoreHelper;

    private int countEnemiesHit = 0;
    private int countPointsMissed = 0;

    Rigidbody rb; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        if (!scoreHelper)
        {
            scoreHelper = gameObject.GetComponentInParent<ScoreHelper>();
        }
    }


    private float nextActionTime = 0.0f;
    private float period = 1f;

    // Update is called once per frame
    void Update()
    {
        // f4 => 4 cijfers na de komma in float
        scoreBoard.text = GetCumulativeReward().ToString("f4");


        // Gives reward if enemy hits EndLine
        if (oldScore < scoreHelper.getScore())
        {
            AddReward(0.25f);
            oldScore++;
        }

        // Gives punishment if points hits EndLine
        if (oldPunishment < scoreHelper.getPunishment())
        {
            AddReward(-1f);
            countPointsMissed++; 
            oldPunishment++;
        }


        // when the agent missed x enemies or x points => restart episode 
        if (countEnemiesHit == maxEnemiesHitBeforeRestart || countPointsMissed == maxPointsHitBeforeRestart)
        {
            EndEpisode();
        }

        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            // every second AI gets a reward for being alive
            AddReward(0.01f);
        }
    }


    public override void OnEpisodeBegin()
    {
       
        oldScore = 0;
        oldPunishment = 0;

        scoreHelper.resetScore();
        scoreHelper.resetPunishment();

        countEnemiesHit = 0;
        countPointsMissed = 0;

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var vectorAction = actions.DiscreteActions;

        if(vectorAction[0] == 0)
        {
            //do nothing (no jumping)
        }
        if(vectorAction[0] == 1)
        {
            //jump
            if (isGrounded)
            {
                print("agent jumped"); 
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;

            }
        }
    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {
        print("heuristic entered");

        // if (Input.GetKeyDown(KeyCode.Space))
        int action = 0;
        if (Input.GetKey(KeyCode.Space))
        {
            print("pressed spacebar");
            action = 1;

        }
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = (int)action;

    }




    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.CompareTag("Enemy"))
        {
            //punishment for hitting enemies
            AddReward(-1f);
            countEnemiesHit++; 
            Destroy(collision.gameObject); 

        }

        if (collision.transform.CompareTag("Points"))
        {
            //reward for hitting objects
            AddReward(0.25f);
            Destroy(collision.gameObject);

        }

    }


    // Grounded = if on ground (necessairy for jumping mechanic)
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            print("ground");
            isGrounded = true;
        }
    }

}
