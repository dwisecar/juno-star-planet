using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanActivator : MonoBehaviour
{
    /// <summary>
    /// Fan causes framerate drop so IT'LL STAY UNACTIVE until player walks into space
    /// </summary>

    public GameObject FanToActivate;
    // Start is called before the first frame update
    void Start()
    {
        FanToActivate.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            FanToActivate.SetActive(true);

        }
    }
}
