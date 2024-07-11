using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumperController : MonoBehaviour
{
    [SerializeField] private GameObject Spawner;
    private LogicTrampolineTime LogicTT;
    private Rigidbody2D RB2D;
    [SerializeField] private LineDrawer lineDrawer;
    // Start is called before the first frame update
    void Start()
    {
        RB2D = GetComponent<Rigidbody2D>();

        LogicTT = Spawner.GetComponent<LogicTrampolineTime>();

        StartCoroutine(Launch(1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Launch(float LaunchTime) {
        yield return new WaitForSeconds(LaunchTime);

        RB2D.AddForce(new Vector2(Random.Range(1, 3), Random.Range(3, 5)) * 120);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("DeathZone")) {
            LogicTT.noJumpers --;
            LogicTT.livesLeft --;
            LogicTT.LivesLeftDisplay.text = LogicTT.livesLeft.ToString();
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("TrampolineTT")) {
            float width = collision.gameObject.GetComponent<Collider2D>().bounds.size.x;
            RB2D.AddForce(new Vector2(0.0f, 5.5f) * 50 * (width * 0.1f + 3.0f));

            lineDrawer.noLines -= 1;
            Destroy(collision.gameObject);
        }
    }
    
}
