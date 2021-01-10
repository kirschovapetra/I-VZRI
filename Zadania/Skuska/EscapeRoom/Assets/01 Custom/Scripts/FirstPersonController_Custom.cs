/*  Zjednoduseny a upraveny FirstPersonController z UnityStandardAssets
    src: https://assetstore.unity.com/packages/essentials/asset-packs/standard-assets-for-unity-2018-4-32351 */

using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
#pragma warning disable 618, 649

[RequireComponent(typeof (CharacterController))]
[RequireComponent(typeof (AudioSource))]

public class FirstPersonController_Custom : MonoBehaviour {

    [Header("Zoom")]
    public float zoomSpeed = 10;
    public float maxZoom = 120;
    public float minZoom = 5;

    [Header("Chôdza")]
    public float m_WalkSpeed;
    public float m_GravityMultiplier;
    public float m_StepInterval;
    public AudioClip[] m_FootstepSounds; 

    [Header("Rotácia kamery")]
    public MouseLook_Custom m_MouseLook;
    
    private Camera m_Camera;
    private float m_YRotation;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private float m_StepCycle;
    private float m_NextStep;
    private AudioSource m_AudioSource;
    private float currentZoom = 50;

    private void Start() {
        m_CharacterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle/2f;
        m_AudioSource = GetComponent<AudioSource>();
		m_MouseLook.Init(transform, m_Camera.transform);
    }

    private void Update() {
        m_MouseLook.LookRotation (transform, m_Camera.transform);
        
        //zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scroll * zoomSpeed;
        // od minZoom po maxZoom
        currentZoom = currentZoom > maxZoom ? maxZoom : currentZoom;
        currentZoom = currentZoom < minZoom ? minZoom : currentZoom;
        // field of view kamery
        m_Camera.fieldOfView = currentZoom;

    }

    private void FixedUpdate() {
        float speed;
        GetInput(out speed);
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                           m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        m_MoveDir.x = desiredMove.x*speed;
        m_MoveDir.z = desiredMove.z*speed;

        m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
    
        m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

        ProgressStepCycle(speed);
    }
    
    
    private void ProgressStepCycle(float speed) {
        if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            m_StepCycle += (m_CharacterController.velocity.magnitude + speed)* Time.fixedDeltaTime;
        
        if (!(m_StepCycle > m_NextStep)) 
            return;
        
        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }


    private void PlayFootStepAudio() {
        // pick & play a random footstep sound from the array, excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
    }


   
    private void GetInput(out float speed) {
        // Read input
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");
        
        speed = m_WalkSpeed;
        m_Input = new Vector3(horizontal, vertical);
        
        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1) 
            m_Input.Normalize();
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (m_CollisionFlags == CollisionFlags.Below) 
            return;
        
        if (body == null || body.isKinematic)
            return;
        
        body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
    }
}

