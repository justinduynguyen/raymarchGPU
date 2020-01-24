using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Vector3 preMousePos;
    public Vector3 diff;
    public Vector3 moveDirection = Vector3.zero;
    public float rotationSpeed;
 
    public Vector3 dir;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        preMousePos = Input.mousePosition;
    
    }

    private void Update()
    {
        diff = Input.mousePosition - preMousePos;
        diff.y = diff.x;
        diff.x = 0;
        preMousePos = Input.mousePosition;
        if (Input.GetMouseButton(0))
            transform.Rotate(diff * Time.deltaTime * rotationSpeed);
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes


        


           
            moveDirection = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;
        dir = transform.TransformDirection(moveDirection);
        // Move the controller

      
        characterController.Move(dir*Time.deltaTime);

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position,transform.forward*3);
    }
}
