using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : MonoBehaviour
{
    public GameObject car;
    private Rigidbody rb;
    public bool carCollision = false;
    public Transform cross;
    public float forceMultiplier = 1f;
    public float speedToAccel = 1.15f;

    private void Start()
    {
        carCollision = false;
        rb = car.GetComponent<Rigidbody>();

    }

    private void OnTriggerEnter(Collider other)
    {
        carCollision = true;
        StartCoroutine(Turbo());
    }

    IEnumerator Turbo()
    {
        Debug.Log("Turbo");
        float speedRb=rb.velocity.magnitude*speedToAccel;
        rb.AddForce(cross.transform.forward * forceMultiplier*speedRb*8000f);
        yield return new WaitForSeconds(0.1f);
        carCollision = false;

    }
}
