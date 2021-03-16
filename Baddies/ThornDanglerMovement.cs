using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornDanglerMovement : MonoBehaviour
{

    protected Rigidbody2D rb2d;
    protected bool stopBurn;
    public Vector3 BoostUp = new Vector3(0,15,0);
    public float BurnTime;
    public float RestTime;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!stopBurn)
        {
            StartCoroutine(BurnTimer());
        }
        else
        {
            StartCoroutine(RestTimer());
        }
    }

    protected IEnumerator BurnTimer()
    {
        rb2d.AddForce(BoostUp);
        yield return new WaitForSeconds(BurnTime);
        stopBurn = true;
    }
    protected IEnumerator RestTimer()
    {
        stopBurn = true;
        yield return new WaitForSeconds(RestTime);
        stopBurn = false;
    }


}
