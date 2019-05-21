using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [Header("Groundcheck")]
    public float m_SkinWidth = 0.05f;
    public float m_FallingMultiplier = 1.75f;
    public Vector3 m_Gravity = new Vector3(0.0f, -15f, 0.0f);

    public bool Grounded { get; private set; }
    public bool LastGrounded { get; private set; }
    public bool OnSlope { get { return m_slopeAngle > m_MaxSlopeLimit && Grounded; } }

    private float m_slopeAngle;
    private Vector3 m_groundNormal;
    private RaycastHit m_hit;
    private RaycastHit[] m_hits;

    [Header("Movement")]
    public float m_MaxSpeed = 5.0f;
    public float m_MaxVelocity = 50.0f;
    public AnimationCurve m_MovementCurve;

    public Vector3 Velocity
    {
        get { return m_velocity; }
        private set
        {
            m_velocity = value;

            if (m_velocity.sqrMagnitude > m_MaxVelocity * m_MaxVelocity)
            {
                m_velocity = m_velocity.normalized * m_MaxVelocity;
            }
        }
    }
    public Vector3 InputVelocity
    {
        get { return m_inputVelocity; }
        private set
        {
            m_inputVelocity = value;

            if (m_inputVelocity.sqrMagnitude > m_MaxSpeed * m_MaxSpeed)
            {
                m_inputVelocity = m_inputVelocity.normalized * m_MaxSpeed;
            }
        }
    }
    public Vector3 GravityVelocity
    {
        get { return m_gravityVelocity; }
        private set
        {
            m_gravityVelocity = value;

            if (m_gravityVelocity.sqrMagnitude > m_MaxVelocity * m_MaxVelocity)
            {
                m_gravityVelocity = m_gravityVelocity.normalized * m_MaxVelocity;
            }
        }
    }

    private float m_speed;
    private Vector2 m_moveVector;
    private Vector3 m_lastVel;
    private Vector3 m_lastPos;
    private Vector3 m_velocity;
    private Vector3 m_inputVelocity;
    private Vector3 m_gravityVelocity;

    [Header("Jump")]
    [Range(0, 1)] public float m_AirControl = 0.03f;

    private bool m_jumping;

    [Header("Slope")]
    public float m_MaxSlopeLimit = 45.5f;
    public float m_MinSlopeSpeed = 6.0f;
    public float m_SlopeRatio = 0.025f;
    public float m_SlopeInputRatio = 0.02f;

    [Header("Hook")]
    public float m_MaxHookLength = 25.0f;
    public float m_HookRadius = 0.25f;
    public float m_HookSpeed = 3.0f;
    public float m_HookInputRatio = 0.025f;
    [Range(0, 1)] public float m_HookRatio = 0.55f;
    public LayerMask m_HookLayer;

    private bool m_hooked;
    private Vector3 m_hookPoint;

    [Header("General")]
    public LayerMask m_GroundLayer;

    private Camera m_camera;
    private Rigidbody m_rigidbody;
    private CapsuleCollider m_collider;
    private CameraController m_camController;

    [Header("Debug")]
    public Transform m_LineTransform;
    public LineRenderer m_Line;

    #region --- MonoBehaviour ---
    private void Awake()
    {
        // set components
        m_camera = GetComponentInChildren<Camera>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponentInChildren<CapsuleCollider>();
        m_camController = GetComponent<CameraController>();

        // set collider material
        PhysicMaterial zeroFric = new PhysicMaterial("ZeroFriction");
        zeroFric.dynamicFriction = 0;
        zeroFric.staticFriction = 0;
        m_collider.material = zeroFric;

        // set variables
        m_speed = m_MaxSpeed;

        m_lastPos = transform.position;
    }

    private void FixedUpdate()
    {
        GroundCheck();

        if (!m_hooked && !OnSlope)
        {
            CalculateGravity();
        }

        MovementUpdate();

        CalculateVelocity();
    }
    #endregion

    #region --- public functions ---
    public void Move(Vector2 _velocity)
    {
        m_moveVector = _velocity;
    }

    public void Move(Vector3 _velocity)
    {
        Move(new Vector2(_velocity.x, _velocity.z));
    }

    public void SetSpeed(float _speed)
    {
        m_MaxSpeed = _speed;
    }

    public void Jump(float _force)
    {
        if (!Grounded || m_hooked || OnSlope)
        {
            return;
        }

        Vector3 jumpVel = InputVelocity;
        jumpVel.y = _force;

        if (jumpVel.magnitude > m_speed)
        {
            jumpVel = jumpVel.normalized * m_speed;
        }

        GravityVelocity += jumpVel;
        InputVelocity = Vector3.zero;

        m_jumping = true;
    }

    public void Hook()
    {
        RaycastHit hit;

        if (CanHook(out hit))
        {
            m_Line.SetPosition(0, m_LineTransform.position);
            m_hookPoint = hit.point;
            m_Line.SetPosition(1, m_hookPoint);

            GravityVelocity += InputVelocity;
            InputVelocity = Vector3.zero;

            m_hooked = true;
            m_jumping = false;
        }
    }

    public void StopHook()
    {
        m_Line.SetPosition(0, Vector3.zero);
        m_Line.SetPosition(1, Vector3.zero);

        m_hooked = false;
    }

    public bool CanHook(out RaycastHit _hit)
    {
        Ray hookRay = new Ray(m_camera.transform.position, m_camera.transform.forward);
       
        return Physics.SphereCast(hookRay, m_HookRadius, out _hit, m_MaxHookLength - m_HookRadius, m_HookLayer);
    }
    #endregion

    #region --- Movement ---
    private void MovementUpdate()
    {

        if (m_jumping)
        {
            return;
        }

        Vector3 dir = m_camController.GetMoveDirection(m_moveVector);

        if (m_hooked)
        {
            HookMove(dir);
        }
        else if (OnSlope)
        {
            SlopeMove(dir);
        }
        else if (Grounded)
        {
            BasicMove(dir);
        }
        else
        {
            AirMove(dir);
        }
    }

    private void BasicMove(Vector3 _dir)
    {
        Vector3 vel = _dir;

        if (m_groundNormal != Vector3.zero)
        {
            vel = Vector3.Cross(vel, m_groundNormal);
            vel = Vector3.Cross(-vel, m_groundNormal);
            vel.Normalize();
        }

        InputVelocity = vel * m_speed;
    }

    private void AirMove(Vector3 _dir)
    {
        Vector3 vel = _dir;

        vel *= m_speed;

        vel.y = 0;

        float value = 1 - GravityVelocity.magnitude / m_MaxVelocity;
        vel *= value;
        InputVelocity = Vector3.Lerp(InputVelocity, vel, m_AirControl);
    }

    private void HookMove(Vector3 _dir)
    {
        m_Line.SetPosition(0, m_LineTransform.position);

        Vector3 vel = _dir;
        InputVelocity = Vector3.Lerp(InputVelocity, _dir * m_HookSpeed, m_HookInputRatio);

        Vector3 calcVel = (m_hookPoint - m_camera.transform.position).normalized;
        calcVel += GravityVelocity;

        GravityVelocity = Vector3.Lerp(GravityVelocity, calcVel, m_HookRatio);

        if ((m_hookPoint - m_camera.transform.position).sqrMagnitude > m_MaxHookLength * m_MaxHookLength)
        {
            StopHook();
        }
    }

    private void SlopeMove(Vector3 _dir)
    {
        Vector3 inputVel = _dir * 0.2f * m_MaxVelocity;

        InputVelocity = Vector3.Lerp(InputVelocity, inputVel, m_SlopeInputRatio);

        Vector3 dir = Vector3.Cross(Vector3.Cross(Vector3.up, m_groundNormal), m_groundNormal);
        float perc = 1 - ((90 - m_slopeAngle) / (90 - m_MaxSlopeLimit));
        float desiredSpeed = Mathf.Lerp(m_MinSlopeSpeed, m_MaxVelocity, perc);
        Vector3 desiredVel = dir * desiredSpeed;
        GravityVelocity = Vector3.Lerp(GravityVelocity, desiredVel, m_SlopeRatio);
    }
    #endregion

    #region --- Ground Calculations ---
    private void CalculateGravity()
    {
        if (Grounded)
        {
            if (m_jumping)
            {
                return;
            }

            GravityVelocity = Vector3.zero;
            SnapToGround();
        }
        else
        {
            GravityVelocity += m_Gravity * Time.fixedDeltaTime * (m_rigidbody.velocity.y < 0 ? m_FallingMultiplier : 1);

            if (m_jumping)
            {
                m_jumping = false;
            }
        }
    }

    private void SnapToGround()
    {
        Vector3 center = transform.position + Vector3.up * m_collider.radius;
        Vector3 offset = -m_hit.normal * m_collider.radius;
        Vector3 posOffset = center + offset;
        Vector3 offsetPos = posOffset - transform.position;

        transform.position = m_hit.point - offsetPos;
    }

    private void GroundCheck()
    {
        LastGrounded = Grounded;

        // groundcheck
        Ray centerRay = new Ray(transform.position + Vector3.up * (m_collider.radius + 0.05f), Vector3.down);
        m_hits = CalculateGroundCheck(centerRay, m_SkinWidth + 0.05f);

        bool ground = m_hits.Length > 0;
        // if check hits ...
        if (ground)
        {
            // ... check if normal equals calculated normal
            CalculateNormalCheck(centerRay);
        }
        // if check doesn't hit and last frame was grounded ...
        else if (LastGrounded)
        {
            // ... check if slope changed
            float length = m_MaxSpeed * Time.fixedDeltaTime * ((2 * m_MaxSlopeLimit) / 180) + m_SkinWidth + 0.05f;
            m_hits = CalculateGroundCheck(centerRay, length);
            ground = m_hits.Length > 0;

            if (ground)
            {
                CalculateNormalCheck(centerRay);
            }

            Debug.DrawRay(centerRay.origin, Vector3.down * (length + m_collider.radius + 0.05f), Color.cyan, 10.0f);
        }

        // set grounded
        Grounded = ground;

        if (!Grounded)
        {
            m_hit = new RaycastHit();
            m_speed = m_MaxSpeed;
            m_slopeAngle = 0;
        }
        else
        {
            m_slopeAngle = Vector3.Angle(Vector3.up, m_groundNormal);
            m_speed = m_MaxSpeed * (InputVelocity.y < 0 ? 1 : m_MovementCurve.Evaluate(m_slopeAngle / m_MaxSlopeLimit));
        }

        if (LastGrounded && !Grounded)
        {
            GravityVelocity += InputVelocity;
            InputVelocity = Vector3.zero;
        }
    }

    private RaycastHit[] CalculateGroundCheck(Ray _ray, float _length)
    {
        List<RaycastHit> validHits = new List<RaycastHit>();
        RaycastHit[] hits = Physics.SphereCastAll(_ray, m_collider.radius, _length, m_GroundLayer);

        //kill invalid hits
        foreach (RaycastHit hit in hits)
        {
            if (hit.point != Vector3.zero)
            {
                validHits.Add(hit);
            }
        }
        return validHits.ToArray();
    }

    private void CalculateNormalCheck(Ray _ray)
    {
        m_hit = Helper.GetNearestHit(m_hits, transform.position + Vector3.up * m_collider.radius);
        m_groundNormal = m_hit.normal;

        Ray pointRay = new Ray(m_hit.point + Vector3.up * 0.01f, Vector3.down);
        RaycastHit pointHit;

        Physics.Raycast(pointRay, out pointHit, 0.02f, m_GroundLayer);

        // if normals differ ...
        if (m_groundNormal != pointHit.normal)
        {
            // ... calculate normal at transform position
            RaycastHit posHit;
            float length = m_collider.radius + m_SkinWidth + m_hit.point.y - transform.position.y;
            Debug.DrawRay(transform.position, Vector3.down * length, Color.red, 5.0f);

            // if posHit normal hits something ...
            if (Physics.Raycast(_ray, out posHit))
            {
                // ... take this normal
                m_groundNormal = posHit.normal;
            }
            // if transformPos normal hits nothing ...
            else
            {
                // ... take pointHit normal
                m_groundNormal = pointHit.normal;
            }
        }
    }
    #endregion

    private void CalculateVelocity()
    {
        Velocity = InputVelocity + GravityVelocity;

        Vector3 tmp = GravityVelocity;
        tmp.x = Mathf.Lerp(0, GravityVelocity.x, 0.9925f);
        tmp.z = Mathf.Lerp(0, GravityVelocity.z, 0.9925f);
        GravityVelocity = tmp;

        Vector3 dist = m_lastPos - transform.position;
        dist = Helper.AbsVector(dist);
        dist = Helper.RoundVector(dist, 3);
        Vector3 vel = m_lastVel * Time.fixedDeltaTime;
        vel = Helper.AbsVector(vel);
        vel = Helper.RoundVector(vel, 3);

        Vector3 calc = Helper.ClampVector01(Helper.DivVector(dist, vel));

        if (calc != Vector3.one)
        {
            InputVelocity = Helper.MultVector(InputVelocity, calc);
            GravityVelocity = Helper.MultVector(GravityVelocity, calc);
            Velocity = InputVelocity + GravityVelocity;

            if (m_jumping && calc.y != 1)
            {
                m_jumping = false;
            }
        }

        m_rigidbody.velocity = Velocity;

        m_lastPos = transform.position;
        m_lastVel = Velocity;
    }
}