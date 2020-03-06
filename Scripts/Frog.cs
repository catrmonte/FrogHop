
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    public float myTimeScale = 1.0f;
    public GameObject[] target;
    public float launchForce = 30f;
    Rigidbody rb;
    int targetNum = 0;
    FiringSolution fs;
    bool justCollided = false;
    bool landed = false;
    public Material targetMaterial;
    public Material lilypadMaterial;
    Renderer targetRenderer;
    

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = myTimeScale; // allow for slowing time to see what's happening
        rb = GetComponent<Rigidbody>();

        targetRenderer = target[targetNum].GetComponent<Renderer>();
        targetRenderer.material = targetMaterial;

        fs = new FiringSolution();
        //Nullable<Vector3> aimVector = fs.Calculate(transform.position, target.transform.position, launchForce, Physics.gravity);
        //if (aimVector.HasValue)
        //{
        //  rb.AddForce(aimVector.Value.normalized * launchForce, ForceMode.VelocityChange);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (landed)
        {
            rb.transform.position = target[targetNum].transform.position;
        }

        if (Input.GetKeyDown(KeyCode.H)) {
            landed = false;
            Nullable<Vector3> aimVector = fs.Calculate(transform.position, target[targetNum].transform.position, launchForce, Physics.gravity);
            if (aimVector.HasValue)
            {
                rb.constraints = RigidbodyConstraints.None;
                justCollided = false;
                rb.AddForce(aimVector.Value.normalized * launchForce, ForceMode.VelocityChange);
                
            }
            else
            {
                Debug.Log("No value: target Num is " + targetNum);
            }
        }
    }

    public void switchTarget()
    {
        int newNum = UnityEngine.Random.Range(0, target.Length);

        // Set old target to normal material
        targetRenderer.material = lilypadMaterial;

        while (newNum == targetNum)
        {
            newNum = UnityEngine.Random.Range(0, target.Length);
        }

        targetNum = newNum;

        // Get new renderer
        targetRenderer = target[targetNum].GetComponent<Renderer>();
        // Set new target to target Material
        targetRenderer.material = targetMaterial;

        Debug.Log("New target is: " + targetNum);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == target[targetNum])
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
    }
}