using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    // movimiento
    private PlayerInput playerInput;
    private Vector3 input, ultimoInput, puntoDestino;
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
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       Mover();

    }

    public void Mover()
    {
        // para simular el raw input con las player input actions
        // he usado Mathf.Sign para que lo cambie a 1 o -1
        // y para que ponga 0 si no hay input he usado un threshold
        // ya que si no lo pongo, cuando el input es muy pequeño
        // lo convertiría a 1 o -1

        input = playerInput.actions["Move"].ReadValue<Vector2>();
        float threshold = 0.2f;

        inputH = Mathf.Abs(input.x) < threshold ? 0 : (int)Mathf.Sign(input.x);
        inputV = Mathf.Abs(input.y) < threshold ? 0 : (int)Mathf.Sign(input.y);

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
            if (colliderDelante.CompareTag("NPC"))
            {
                colliderDelante.GetComponent<NPC>().Interactuar();
            }
        }
    }

}
