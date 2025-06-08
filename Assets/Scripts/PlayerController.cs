using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float inputH;
    private float inputV;
    private bool moviendo;
    private Vector3 puntoDestino;
    [SerializeField] private float velocidadMovimiento;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");
        if(!moviendo && (inputH != 0 || inputV != 0))
        {
            StartCoroutine(Mover());
        }

        
    }

    IEnumerator Mover()
    {
        moviendo = true;

        puntoDestino = transform.position + new Vector3(inputH, inputV, 0);


        while (transform.position != puntoDestino)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoDestino, velocidadMovimiento * Time.deltaTime);
            yield return null;
        }

        moviendo = false;
    }

}
