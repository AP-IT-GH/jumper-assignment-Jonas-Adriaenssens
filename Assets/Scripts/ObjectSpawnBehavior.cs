using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnBehavior : MonoBehaviour
{

    public GameObject enemy;
    public GameObject points; 
    public float objectSpeed = 20f;
    public float yOffset = 6f;
    public float maxSpeedOffset = 40f; 



    // Start is called before the first frame update
    void Start()
    {
        if (!enemy)
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy");
        }
        if (!points)
        {
            points = GameObject.FindGameObjectWithTag("Points");
        }
    }


    float timer = 0f;
    public float shootRate = 4f;



    // Update is called once per frame
    void Update()
    {

        //shoot's enmy when timer is ready
        if (timer >= shootRate)
        {
            print("Enemy shot"); 
            shootObject();
            timer = 0f;

        } else
        {
            timer += Time.deltaTime; 
        }

    }


    void shootObject()
    {

        float randomChoice = Random.value; 



        float yAxis = Random.Range(0f, yOffset) + this.transform.position.y;
        Vector3 position = new Vector3(this.transform.position.x, yAxis, this.transform.position.z);


        if (randomChoice > 0.5f)
        {
            //Spawn enemy
            GameObject newEnemy = Instantiate(enemy, position, this.transform.rotation);
            newEnemy.transform.SetParent(transform);
            newEnemy.transform.tag = "Enemy";
            newEnemy.GetComponent<Rigidbody>().AddForce((transform.forward * (objectSpeed + Random.Range(-5f, maxSpeedOffset)) * -1), ForceMode.VelocityChange);

        }
        else
        {
            //Spawn points

  
            GameObject newPoints = Instantiate(points, position, this.transform.rotation );
            //rotate to correct position
            newPoints.transform.Rotate(0, 0, 90f);
            newPoints.transform.SetParent(transform);
            newPoints.transform.tag = "Points";
            newPoints.GetComponent<Rigidbody>().AddForce((transform.forward * (objectSpeed + Random.Range(-5f, maxSpeedOffset)) * -1), ForceMode.VelocityChange);
        }

    }



}
