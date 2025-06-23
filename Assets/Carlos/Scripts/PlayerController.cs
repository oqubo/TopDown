using System;
using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public event Action eventoQuitarVida, eventoAumentarPuntos;
    
 
    [SerializeField] private float vida;
    public float Vida { get => vida; set => vida = value; }
    
    [SerializeField] private int puntos;

    public int Puntos { get => puntos; set => puntos = value; }
    
    
    // movimiento
    private Vector3 ultimoInput, puntoDestino;
    private float inputH, inputV;
    [SerializeField] private float velocidadMovimiento;
    private bool moviendo;
    private Animator anim;


    // interaccion
    private Vector3 puntoInteraccion;
    [SerializeField] private float radioInteraccion;
    private Collider2D colliderDelante;
    private bool interactuando;
    public bool Interactuando { get => interactuando; set => interactuando = value; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Mover();

        if (Input.GetKeyDown(KeyCode.E)) LanzarInteraccion();

    }

    public void Mover()
    {

        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");

        // evitar diagonales: prioriza horizontal sobre vertical
        if (inputH != 0) inputV = 0;
        else if (inputV != 0) inputH = 0;

        if (!interactuando && !moviendo && (inputH != 0 || inputV != 0))
        {
            anim.SetBool("andando", true);
            anim.SetFloat("inputH", inputH);
            anim.SetFloat("inputV", inputV);

            ultimoInput = new Vector3(inputH, inputV, 0);
            puntoDestino = transform.position + ultimoInput;
            puntoInteraccion = puntoDestino;
            colliderDelante = LanzarCheck();
            if (!colliderDelante)
            {
                StartCoroutine(MoverCorrutina());
            }
        }
        else if (inputH == 0 && inputV == 0)
        {
            anim.SetBool("andando", false);
            anim.SetFloat("inputH", ultimoInput.x);
            anim.SetFloat("inputV", ultimoInput.y);
        }

    }
    IEnumerator MoverCorrutina()
    {
        moviendo = true;

        while (transform.position != puntoDestino)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoDestino, velocidadMovimiento * Time.deltaTime);
            yield return null;
        }

        puntoInteraccion = transform.position + ultimoInput;
        moviendo = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(puntoInteraccion, radioInteraccion);
    }

    private Collider2D LanzarCheck()
    {
        return Physics2D.OverlapCircle(puntoInteraccion, radioInteraccion);

    }

    public void LanzarInteraccion()
    {
        colliderDelante = LanzarCheck();
        if (colliderDelante)
        {
            // otra forma de hacerlo
            /*
            Interactuable interactuable = colliderDelante.GetComponent<Interactuable>();
            if (interactuable != null) interactuable.Interactuar();
            */

            if (colliderDelante.TryGetComponent<Interactuable>(out Interactuable interactuable)) interactuable.Interactuar();

        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Door"))
        {
            Interactuable interactuable = col.GetComponent<Interactuable>();
            if (interactuable != null) interactuable.Interactuar();
        }
        
        else if (col.CompareTag("Danger"))
        {
            Vida -= 10f;
            eventoQuitarVida.Invoke();

            if (Vida <= 0) Muerte();
        }
        
        else if (col.CompareTag("Coin"))
        {
            Debug.Log("detecta colision con moneda");
            Puntos++;
            Destroy(col.gameObject);
            eventoAumentarPuntos.Invoke();
        }
        
        else if (col.CompareTag("test"))
        {
            
            Destroy(col.gameObject);
            
        }

    }


    private void Muerte()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Collider2D>().enabled = false;
            
        GameObject npc = GameObject.FindGameObjectWithTag("NPC");
        npc.GetComponent<SpriteRenderer>().enabled = true;
        
        Interactuable interactuable = npc.GetComponent<Interactuable>();
        if (interactuable != null) interactuable.Interactuar();
        
        Invoke("IrAlPueblo",5f);
    }

    private void IrAlPueblo()
    {
        GameManager.Instance.CargarEscenaPorNombre("Pueblo");
    }
}
