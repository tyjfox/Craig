using UnityEngine;
using System.Collections;

public class AvatarMovement : MonoBehaviour {
	
	const float speed = 10.0F;
	const float jumpSpeed = 16.0F;
	const float gravity = 25.0F;
	const float sensitivityX = 15F;
	
	public Vector3 velocity = Vector3.zero;
	public Vector3 rotation = Vector3.zero;

    private Animator anim;
    private float oldPosX;
    private float oldPosZ;
    private CharacterController controller;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        oldPosX = transform.position.x;
        oldPosZ = transform.position.z;
    }

	void Update() {
	    
        if (controller.isGrounded) {
			velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			velocity = transform.TransformDirection(velocity) * speed;
            
            if (Input.GetButton ("Jump")) {
                velocity.y = jumpSpeed;
                
			}
		}
		velocity.y -= gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
		
		rotation.y = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
		transform.localEulerAngles = rotation;
	    if (controller.isGrounded)
	    {
	        if (oldPosX != transform.position.x || oldPosZ != transform.position.z)
	        {
	            anim.Play("Walk");
	        }
	        else
	        {
	            anim.Play("Idle");
	        }
	    }
	    else
	    {
	        anim.Play("Jump");
	    }
	    oldPosX = transform.position.x;
        oldPosZ = transform.position.z;
    }
}