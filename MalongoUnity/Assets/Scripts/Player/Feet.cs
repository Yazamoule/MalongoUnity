using FeetEnum = Movement.FeetEnum;

using UnityEditor.Overlays;
using UnityEngine;

public class Feet : MonoBehaviour
{
    GameManager gm;
    LevelManager lm;
    Movement move;

    public Vector3 groundNormal = Vector3.one;
    public Quaternion groundRQuat = Quaternion.identity;
    public Vector3 currentStep = Vector3.zero;

    [SerializeField] float maxSlopeAngle = 45;
    [SerializeField] float springForce = 50;
    [SerializeField] float springDamp = 50;

    [SerializeField] private float groundDist = 0f;
    private float lastGroundDist = 0f; 

    public float verticalSpringSpeed = 0f;

    [SerializeField] Transform start = null;
    [SerializeField] Transform end = null;
    float length = 0f;
    [SerializeField] LayerMask rayLayermask;

        
    private void Awake()
    {
        gm = GameManager.Instance;
        lm = LevelManager.Instance;
    }

    private void Start()
    {
        move = LevelManager.Instance.player.move;

        length = start.position.y - end.position.y;
    }

    public void CustomUpdate()
    {
        CheckGround();
        AddGroundForce();
    }

    public void CheckGround()
    {
        //set the last Feet enum
        move.backFeetEnum = move.feetEnum;

        lastGroundDist = groundDist;

        Ray ray = new Ray(start.position, Vector3.down);
        RaycastHit hit = new RaycastHit();

      

        if (Physics.Raycast(ray, out hit, length, rayLayermask, QueryTriggerInteraction.Ignore))
        {
            groundNormal = hit.normal;
            groundRQuat = Quaternion.FromToRotation(Vector3.up, groundNormal);

            currentStep = hit.point;

            Color debugLineColor;  

            float angle = Vector3.Angle(groundNormal, Vector3.up);
            groundDist = hit.distance;

            if (angle > maxSlopeAngle)
            {
                move.feetEnum = FeetEnum.ToSteepGround;
                debugLineColor = Color.white;
            }
            else
            {
                move.feetEnum = FeetEnum.OnGround;
                debugLineColor = Color.black;
            }
            
            gm.DebugLine("Ground Normal", debugLineColor, groundNormal, hit.point, true, false);
        }
        else
        {
            groundNormal = Vector3.up;
            groundRQuat = Quaternion.identity;
            groundDist = 1000f;
            move.feetEnum = FeetEnum.OffGround;
        }


    }


    public void AddGroundForce()
    {
        if (move.feetEnum == FeetEnum.OffGround)
            return;
        


        if (move.backFeetEnum == FeetEnum.OffGround)
        {
            verticalSpringSpeed = move.rb.linearVelocity.y;
        }
        else
        {
            verticalSpringSpeed = (lastGroundDist - groundDist) / Time.fixedDeltaTime;
        }

        float force = 1 - (groundDist / length);

        //force = (4.1f * Mathf.Pow(force - 0.5f, 3) + 0.5462f) * 0.9f;

        force = force * springForce * Time.fixedDeltaTime;
        float damp = - verticalSpringSpeed * springDamp * Time.fixedDeltaTime;

        move.rb.linearVelocity += Vector3.up * (force + damp);

        // Draw debug lines
        //gm.DebugLine("Feet Damp", Color.red, Vector3.up * 50 * damp);
        //gm.DebugLine("Feet Force", Color.yellow, Vector3.up * 50 * force);
    }

    public void Slide()
    {
    }

    /// <param name="_input">vector you want to rotate.</param>
    /// <param name="_input">if up of the world is the normal of the slope you are standing on.</param>
    /// <param name="_input">wen canReturnZero is false, the function will use a Vector3.foward wen the _input is Zero.</param>
    public Vector3 RotateToLocalWorld(Vector3 _input, bool _acountForSlope, bool _canReturnZero = true)
    {
        Quaternion rotation;
        if (_acountForSlope)
        {
            rotation = groundRQuat;
        }
        else
        {
            rotation = Quaternion.identity;
        }

        if (!_canReturnZero && _input.sqrMagnitude == 0)
        {
            _input = Vector3.forward;
        }
        else
        {
            _input.Normalize();
        }

        return rotation * move.playerFoward.rotation * _input;
    }


    public Vector3 OverrideVerticalAxis(Vector3 _input, bool _acountForSlope, float _y = 0)
    {
        if (_acountForSlope)
        {
            gm.DebugLine("yesTEmp", Color.grey, Vector3.Scale(Vector3.forward, groundRQuat * new Vector3(1f, 0f, 1f)) + new Vector3(0f, _y, 0f));
            return Vector3.Scale(_input, groundRQuat * new Vector3(1f, 0f, 1f)) + new Vector3(0f, _y, 0f);
        }

        return Vector3.Scale(_input, new Vector3(1f, 0f, 1f)) + new Vector3(0f, _y, 0f);
    }
}