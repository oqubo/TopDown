using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float inputHorizontal;
    private float inputVertical;
    private Vector3 targetPosition;
    private bool moving;
    [SerializeField] private float MovementSpeed;
    
    private Vector3 interactionPoint;
    private Vector3 lastInput = new Vector3();
    [SerializeField] private float interactionRadius;

    private Collider2D frontCollider;
    private bool interacting = false;
    public bool Interacting { get => interacting; set => interacting = value; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ReadInput();

        if (interacting)
            return;
        
        UpdateMovemente();
    }

    private void UpdateMovemente()
    {
        if (!(!moving && (inputHorizontal != 0 || inputVertical != 0))) 
            return;
        
        lastInput.x = inputHorizontal;
        lastInput.y = inputVertical;
        
        targetPosition = transform.position + lastInput;
        interactionPoint = targetPosition;

        frontCollider = SendCheck();
        if (!frontCollider)
        {
            StartCoroutine(Move());
        }
    }

    private void ReadInput()
    {
        if (inputVertical == 0)
            inputHorizontal = Input.GetAxis("Horizontal");

        if (inputHorizontal == 0)
            inputVertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
            SendInteraction();
    }

    private void SendInteraction()
    {
        frontCollider = SendCheck();
        if (!frontCollider)
            return;

        InteractWithNPC();
    }

    private void InteractWithNPC()
    {
        if (!frontCollider.gameObject.CompareTag("NPC"))
            return;
        
        NPC npcScript = frontCollider.gameObject.GetComponent<NPC>();
        npcScript?.Interact();
    }

    IEnumerator Move()
    {
        moving = true;
        
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, MovementSpeed * Time.deltaTime);
            yield return null;
        }
        interactionPoint = transform.position + lastInput;
        
        moving = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(interactionPoint, interactionRadius);
    }

    private Collider2D SendCheck()
    {
        return Physics2D.OverlapCircle(interactionPoint, interactionRadius);
    }
}
