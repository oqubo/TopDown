using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pinchos : MonoBehaviour
{
    [SerializeField] private bool horizontal = true;
    [SerializeField] private float speed = 3f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        speed *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (horizontal) transform.position += speed * Time.deltaTime * Vector3.right;
        else transform.position += speed * Time.deltaTime * Vector3.up;
    }
}
