using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private float testInterval = 1f; // Interval in seconds
    private Coroutine testCoroutine;
    public GameObject soldier_go;
    public GameObject gun_go;

    void Awake()
    {

    }

    void Start()
    {
        testCoroutine = StartCoroutine(TestCoroutine());
    }

    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    IEnumerator TestCoroutine()
    {
        while (true)
        {
            // Your test logic here
            gun_go.GetComponent<Gun>().shoot(soldier_go.transform.position - gun_go.transform.position);

            // Wait for the specified interval before running again
            yield return new WaitForSeconds(testInterval);
        }
    }

    void OnDestroy()
    {
        // Stop the coroutine when the object is destroyed
        if (testCoroutine != null)
        {
            StopCoroutine(testCoroutine);
        }
    }
}
