using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DM
{
    public class ControlManager : MonoBehaviour
    {
        [Header("Initialize")]
        public GameObject activeModel;   // defines the current active model.
        public string[] randomAttacks;  // array of normal attacks in string.
        

        [Header("Inputs")]
        public float vertical;         // stores vertical input.
        public float horizontal;      // stores horizontal input.
        public float moveAmount;     //shows the amount of movement from 0 to 1.
        public Vector3 moveDir;     //stores the moving vector value of main character.

        [Header("Player Stats")]
        public float Health = 1000f;
        public float moveSpeed = 3.5f;     //speed of running
        public float sprintSpeed = 5f;    //speed of sprinting(double time of running)
        public float rotateSpeed = 7;    //speed of character's turning around
        public float jumpForce = 600f;  //how high you can jump value.
        

        [Header("States")]
        public bool onGround;     //shows you are on ground or not.
        public bool sprint;      //shows you are sprinting or not.
        public bool onRamp;     //stores if you are on ramp or not.
        [HideInInspector]
        public bool jump;       //stores whether you jump or not
        [HideInInspector]
        public bool normalAttack;   //stores whether you do normal attack or not
        [HideInInspector]
        public bool comboAttack;       //stores whether you combo or not
        public bool canMove;          //shows you can move or not
        [HideInInspector]
        public bool roll;               //stores whether you roll or not
        public bool block;             //stores whether you block or not
        public bool isAttacking;      // stores whether you attacking or not
        public bool blockingState;   // other blocking variable checksum
        public bool rollingState;   // other rolling variable checksum


        float fixedDelta;        //stores Time.fixedDeltaTime
        float delta;
        Animator anim;      //for caching Animator component
        [HideInInspector]
        public Rigidbody rigid;      //for caching Rigidbody component
        CameraManager camManager;   //for caching CameraManager script
        
        void Start() // Initiallizing camera, animator, rigidboy
        {                  
            onRamp = false;
            camManager = CameraManager.singleton;
            camManager.Init(this.transform);
            SetupAnimator();
            rigid = GetComponent<Rigidbody>(); 
            canMove = true;           
        }
        
        void SetupAnimator()//Setting up Animator component in the hierarchy.
        {
            if (activeModel == null)
            {
                anim = GetComponentInChildren<Animator>();//Find animator component in the children hierarchy.
                if (anim == null)
                {
                    Debug.Log("No model");
                }
                else
                {
                    activeModel = anim.gameObject; //save this gameobject as active model.
                }
            }

            if (anim == null)
                anim = activeModel.GetComponent<Animator>();            
        }
        
        void FixedUpdate () //Since this is physics based controller, you have to use FixedUpdate.
        {
            fixedDelta = Time.fixedDeltaTime;    //storing the last frame updated time.             

            FixedTick(fixedDelta);   //update anything related to character moving.
            camManager.FixedTick(fixedDelta);     //update anything related to camera moving.       
        }
        
        void Update()
        {
            GetInput();     //getting control input from keyboard or joypad
            UpdateStates();   //Updating anything related to character's actions.         
        }

        

        void GetInput() //getting various inputs from keyboard or joypad.
        {
           vertical = Input.GetAxis("Vertical");    //for getting vertical input.
           horizontal = Input.GetAxis("Horizontal");    //for getting horizontal input.
           sprint = Input.GetButton("SprintInput");     //for getting sprint input.
           jump = Input.GetButtonDown("Jump");      //for getting jump input.
           normalAttack = Input.GetButtonDown("Fire1"); //for getting normal attack input.
           blockingState = Input.GetButtonDown("Fire2");    //for getting block input. - SECONDARY
           rollingState = Input.GetButtonDown("Fire3");     //for getting roll input. - SECONDARY
        }
        void UpdateStates() //updates character's various actions.
        {
            if (Input.GetMouseButtonDown(1)) {
                block = true;
            }
            else if (Input.GetMouseButtonUp(1)) {
                block = false;
            }
            if (block && !isAttacking && onGround) {
                anim.CrossFade("Defend_SwordShield", 0.1f);
                anim.SetBool("block", true);
                canMove = false;
            }
            else if (!isAttacking) {
                anim.SetBool("block", false);
                canMove = true;
            }
            if (onGround == false) {
                moveSpeed = 2.6f;
            } 
            else {
                moveSpeed = 3.6f;
            } 

            if (jump)   //I clicked jump, space key or B key in the joypad.
            {
                if (onGround) //do jump only when you are on ground and you can move.
                {
                    onGround = false;
                    anim.SetBool("onGround", false);
                    anim.CrossFade("falling", 0.1f); //play "falling" animation in 0.1 second as cross fade method.
                    rigid.AddForce(0, jumpForce, 0);  //Adding force to Y axis for jumping up.                  
                }            
            }   

            if(comboAttack)     //I clicked for combo attack. right mouse button or X key in the joypad.
            {
                if(onGround)    //only when you are on ground.
                {
                    anim.SetTrigger("combo");   //Set trigger named "combo" on
                    
                }
            }

            if(roll && onGround)    //I clicked for roll. middle mouse button or Y key in the joypad.
            {                
                anim.SetTrigger("roll");    //Set trigger named "roll" on
            }            
            
            float targetSpeed = moveSpeed;  //set run speed as target speed.
             
            if (sprint)
            {
                if (!onGround) {
                    targetSpeed = 5.0f;
                }
                else {
                    targetSpeed = sprintSpeed;    //set sprint speed as target speed.        
                }
                    
            }

            //mixing camera rotation value to the character moving value.
            Vector3 v = vertical * camManager.transform.forward;
            Vector3 h = horizontal * camManager.transform.right;            

            //multiplying target speed and move amount.
            moveDir = ((v + h).normalized) * (targetSpeed * moveAmount);            

            //This is for isolating y velocity from the character control. 
            moveDir.y = rigid.velocity.y;            
            
            //This is for limiting values from 0 to 1.
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            moveAmount = Mathf.Clamp01(m);
            
            if (normalAttack && canMove) // I clicked for normal attack when I can move around.
            {
                isAttacking = true;
                string targetAnim;

                //chosing random attack from array.
                anim.SetBool("sprint", false);
                canMove = false;
                int r = Random.Range(0, randomAttacks.Length);
                if (r == 0) {
                    StartCoroutine(ChangeValue(0.567f));
                }
                else {
                    StartCoroutine(ChangeValue(0.833f));
                }
                targetAnim = randomAttacks[r];

                anim.CrossFade(targetAnim, 0.1f); //play the targetted attack animation in 0.1 second.                 
                if (!onGround)
                {
                    anim.CrossFade("JumpAttack", 0.1f); // When you are air born, you do this jump attack.
                }

                normalAttack = false;
            } 
                       
        }
        IEnumerator ChangeValue(float t) {
            yield return new WaitForSeconds(t);
            canMove = true;
            isAttacking = false;
        }
        void FixedTick(float d)
        {
            float pDelta = d;            

                
                if(canMove)
                {
                    rigid.velocity = new Vector3(moveDir.x, rigid.velocity.y, moveDir.z);  //This controls the character movement.                  
                }      
                                     

            //This can control character's rotation.
            //if (canMove)
            //{
                Vector3 targetDir = moveDir;
                targetDir.y = 0;
                if (targetDir == Vector3.zero)
                    targetDir = transform.forward;

                Quaternion tr = Quaternion.LookRotation(targetDir);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, pDelta * moveAmount * rotateSpeed);
                transform.rotation = targetRotation;
            //}  

            HandleMovementAnimations(); //update character's animations.
        }

        void HandleMovementAnimations()
        {
            
            anim.SetBool("sprint", sprint);   //syncing sprint bool value to animator's "Sprint" value.           
            if(moveAmount == 0)
            {
                anim.SetBool("sprint", false);
            }            
            
            anim.SetFloat("vertical", moveAmount, 0.2f, fixedDelta); //syncing moveAmount value to animator's "vertical" value.
        }

        //   GROUND COLLISION
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Ramp")
            {
                onGround = true;                
                anim.SetBool("onGround", true);                
            }
            if (collision.gameObject.tag == "Ramp") {
                onRamp = true;
            }
        }
        private void OnCollisionExit(Collision c) {
            if (c.gameObject.tag == "Ground" && !onRamp) {
                onGround = false;
                anim.SetBool("onGround", false);    
            }
            if (c.gameObject.tag == "Ramp") {
                onRamp = false;
            }
        }
    }
}