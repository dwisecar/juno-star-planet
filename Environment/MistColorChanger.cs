using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MistColorChanger : MonoBehaviour
{

    Color lerpedColor = Color.red;
    Color originalColor = new Color(1, 1, 1, .5f);
    protected Color reddishColor = new Color(1f, .5f, .5f, .5f);
    SpriteRenderer sprite; 

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lerpedColor = Color.Lerp(reddishColor, originalColor, Mathf.PingPong(Time.time, .5835f)) ;
        sprite.color = lerpedColor;
    }
}
