using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    const float gravity = 9.82f;
    public float WalkSpeed = 1.5f;
    public float SprintSpeed = 3.0f;
    public float RotateSpeed = 55;
    public float JumpForce = 3.0f;
    public float DistToGround = 0.9f;

    private CharacterController m_characterController;
    private WowCamera m_wowCam;
    private Animator m_anim;

    private Rigidbody m_rigidbody;
    private CapsuleCollider m_capsuleCollider;

    private float yaw;

    private bool UseGravity = true;
    private bool m_sprinting;

    void Awake()
    {
        Initialize();
    }

    public void Initialize() 
    {
        m_characterController = GetComponent<CharacterController>();
        m_wowCam = Camera.main.GetComponent<WowCamera>();
        m_anim = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_capsuleCollider = GetComponent<CapsuleCollider>();

        DistToGround = m_capsuleCollider.bounds.extents.y;
    }

    public void TakeControl()
    {
        Initialize();

        transform.tag = "Player";

        m_wowCam.PlayerControlled = true;
        m_characterController.enabled = true;
        m_capsuleCollider.enabled = true;
    }

    public void ResumeNPC()
    {
        transform.tag = "Villager";

        m_wowCam.PlayerControlled = false;
        m_characterController.enabled = false;
        m_capsuleCollider.enabled = false;

        var v = Vector3.zero;
        m_rigidbody.velocity.Set(v.x, v.y, v.z);
        m_rigidbody.angularVelocity.Set(v.x, v.y, v.z);
    }

    void Update()
    {
        #region Axes movement

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        var cam = Camera.main.transform;

        Vector3 _forward = new Vector3(transform.forward.x, cam.transform.forward.y, transform.forward.z);
        Vector3 _right = new Vector3(transform.right.x, cam.right.y, transform.right.z);

        Vector3 forward;
        Vector3 right;
        
        // This is a terrible way to allow only forward sprinting. 
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            m_sprinting = false;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            m_sprinting = true;
        }
        else
        {
            m_sprinting = false;
        }

        var speed = m_sprinting ? SprintSpeed : WalkSpeed;
        forward = _forward * v * speed * Time.deltaTime;
        right = _right * h * speed * Time.deltaTime;

        Vector3 moveDir = forward + right;

        if (UseGravity)
            moveDir.y -= gravity * Time.deltaTime;

        m_characterController.Move(moveDir);

        #endregion

        #region Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                Debug.Log("Jump!");
                // m_anim.SetFloat("Jumping", 1.0f);
                //  m_rigidbody.AddForce(transform.up * JumpForce, ForceMode.Impulse);
            }
        }
        #endregion

        #region Turn (Rotation)

        if (Input.GetKey(KeyCode.Q))
        {
            Rotate(true);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Rotate(false);
        }
        else 
        {
            yaw += RotateSpeed * 0.02f * Input.GetAxis("Mouse X");
            transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
        }

        SetAnimationVariables();
    }

    private void Rotate(bool right)
    {
        Vector3 rotation;

        if (right)
        {
            rotation = new Vector3(0, -RotateSpeed * Time.deltaTime, 0);
        }
        else
        {
            rotation = new Vector3(0, RotateSpeed * Time.deltaTime, 0);
        }

        yaw = transform.eulerAngles.y;
        transform.Rotate(rotation);
    }
        #endregion

    private void SetAnimationVariables()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        var moving = Math.Abs(x) + Math.Abs(y) > 0.25 ? true : false;

        m_anim.SetFloat("VelocityX", x);
        m_anim.SetFloat("VelocityY", y);

        m_anim.SetBool("Moving", moving);
        m_anim.SetBool("Running", m_sprinting);
    }

    private bool IsGrounded() => Physics.Raycast(transform.position, -Vector3.up, DistToGround + 0.1f);
}
