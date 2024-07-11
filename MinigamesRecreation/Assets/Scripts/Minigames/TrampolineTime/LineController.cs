using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{   
    private float timeLeft = 6.0f;
    public LineDrawer lineDrawer;
    public bool isFinishedDrawing = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (isFinishedDrawing)  {
            timeLeft -= Time.deltaTime;
        }

        if (timeLeft < 0) {
            lineDrawer.noLines -= 1;
            Destroy(gameObject);
        }
    }
}
