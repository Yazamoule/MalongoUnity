using FeetEnum = Movement.FeetStateEnum;

using UnityEditor.Overlays;
using UnityEngine;

public class Feet : MonoBehaviour
{
    GameManager gm;
    LevelManager lm;
    Movement move;

    public Vector3 groundNormal = Vector3.one;
    public Vector3 currentStep = Vector3.zero;

    [SerializeField] float maxSlopeAngle = 45;
    [SerializeField] float springForce = 50;
    [SerializeField] float springDamp = 50;

    private float groundDist = 0f;
    private float lastGroundDist = 0f; 

    private float VerticalGroundSpeed = 0f;

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

    public void CheckGround()
    {
        //set the last Feet enum
        move.backFeetEnum = move.feetEnum;

        lastGroundDist = groundDist;

        Ray ray = new Ray(start.position, Vector3.down);
        RaycastHit hit = new RaycastHit();
        Debug.DrawRay(start.position, end.position, Color.red);

        if (Physics.Raycast(ray, out hit, length, rayLayermask, QueryTriggerInteraction.Ignore))
        {
            groundNormal = hit.normal;
            currentStep = hit.point;

            float angle = Vector3.Angle(groundNormal, Vector3.up);
            groundDist = hit.distance;

            if (angle > maxSlopeAngle)
            {
                move.feetEnum = FeetEnum.ToSteepGround;
            }
            else
            {
                move.feetEnum = FeetEnum.OnGround;
            }
        }
        else
        {
            groundNormal = Vector3.up;
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
            VerticalGroundSpeed = Time.deltaTime * (lastGroundDist - groundDist);
        }
        else
        {
            VerticalGroundSpeed = move.rb.linearVelocity.y;
        }

        float force = 1 - (groundDist / length);

        //force = (4.1f * Mathf.Pow(force - 0.5f, 3) + 0.5462f) * 0.9f;

        force = force * springForce * Time.deltaTime;
        //add vertical friction so you dont bounce
        force = force - move.rb.linearVelocity.y * springDamp * Time.deltaTime;
        //aply the feet force to stay above ground

        move.rb.AddForce(Vector3.up * force, ForceMode.VelocityChange);

        // Draw debug lines
        Debug.DrawLine(transform.position, transform.position + groundNormal * 25, Color.green, 2.0f);
    }

    public void Slide()
    {
    }
}