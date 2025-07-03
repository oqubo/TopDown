using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Samuel : MonoBehaviour{
    private float inputH;
    private float inputV;
    private bool isMoving = false;
    private Vector3 destinationPoint;
    private Vector3 interactionPoint;
    private Vector3 lastInput;
    private Collider nearColider;
    private Animator animator;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float interactionRadius = 1f;
    [SerializeField] private LayerMask whatIsCollisionable;
    void Start(){
        foreach(Transform child in this.transform) {
            if (child.name == "PlayerSprite") {
                animator = child.GetComponent<Animator>();
                break;
            }
        }
    }

    // Update is called once per frame
    void Update(){
        handleInput();
        handleMovementAndAnims();        
    }

    void handleInput(){
        if (inputV == 0) { inputH = Input.GetAxisRaw("Horizontal"); }
        if (inputH == 0) { inputV = Input.GetAxisRaw("Vertical");   }
    }

    void handleMovementAndAnims(){
        if(!isMoving && (inputH != 0 || inputV != 0)) {
            animator.SetBool("Walking", true);
            animator.SetFloat("InputH", inputH);
            animator.SetFloat("InputV", inputV);

            lastInput = new Vector3(inputH, 0, inputV);
            destinationPoint = transform.position + lastInput;
            interactionPoint = destinationPoint;

            nearColider = Check();
            if (!nearColider) { StartCoroutine(Move()); }
        }
    }

    IEnumerator Move() {
        isMoving = true;
        while (transform.position != destinationPoint) {
            transform.position = Vector3.MoveTowards(transform.position, destinationPoint, movementSpeed * Time.deltaTime);
            yield return null;
        }
        interactionPoint = transform.position + lastInput;
        isMoving = false;
        animator.SetBool("Walking", inputH != 0 || inputV != 0);
    }

    private Collider Check(){
        Collider[] colliders = Physics.OverlapSphere(interactionPoint, interactionRadius, whatIsCollisionable);
        return colliders.Length > 0 ? colliders[0] : null;
    }
    private void OnDrawGizmos() {
        Gizmos.DrawSphere(interactionPoint, interactionRadius);
    }
}
