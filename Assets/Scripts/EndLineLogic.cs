using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLineLogic : MonoBehaviour
{

    ScoreHelper scoreHelper;

    // Start is called before the first frame update
    void Start()
    {
        if (scoreHelper == null)
        {
            scoreHelper = gameObject.GetComponentInParent<ScoreHelper>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            scoreHelper.addSucces();
            Destroy(collision.gameObject);
        }
        if (collision.transform.CompareTag("Points"))
        {
            scoreHelper.addPunishment(); 
            Destroy(collision.gameObject);
        }
    }



}
