using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic : MonoBehaviour
{
    [SerializeField] private GameObject BobOmb;
    private float spawnInterval = 4f;
    private float time = 0.0f;
    private BobOmbControl bobOmbControl;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnBobOmb(spawnInterval, BobOmb));
        bobOmbControl = BobOmb.GetComponent<BobOmbControl>(); // Get reference to BobOmbControl
    }

    // Update is called once per frame
    void Update()
    {
        time += 1 * Time.deltaTime;

        if (time >= 25) {
            bobOmbControl.publicBobOmbVelocity = 4.0f;
        }

        if (time >= 60) {
            bobOmbControl.publicBobOmbVelocity = 4.5f;
        }
    }

    private IEnumerator spawnBobOmb(float Interval, GameObject BobOmb)
    {
        yield return new WaitForSeconds(Interval);

        if (time >= 40) {
            Interval = 3.5f;
        }

        if (time >= 90) {
            Interval = 3.2f;
        }

        if (gameObject.CompareTag("TopSpawner") && time >= 15.0f)
        {
            GameObject newBobOmb = Instantiate(BobOmb, new Vector3(0.0462f, 4.03f, 0), Quaternion.identity);
        }
        else if (gameObject.CompareTag("BottomSpawner"))
        {
            GameObject newBobOmb = Instantiate(BobOmb, new Vector3(0.0462f, -4.03f, 0), Quaternion.identity);
        }
        StartCoroutine(spawnBobOmb(Interval, BobOmb));
        Debug.Log(gameObject.tag);
    }
}
