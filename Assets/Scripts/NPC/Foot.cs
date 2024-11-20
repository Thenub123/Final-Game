using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class FootPositioner : MonoBehaviour
{
    // reference to player character object
    public GameObject NPCObj;

    // reference to IK target
    public Transform target;   
 
    // reference to the other foot
    public FootPositioner otherFoot;
    public float offset;

    public bool isBalanced;

    public float lerp;
    private Vector3 startPos;
    private Vector3 endPos;
    public float overShootFactor = 0.1f;
    public float stepSpeed = 2f; 
    public float footDisplacementOnX = 0.25f;

    private Vector3 midPos;

    public LayerMask layer;

    private void Start()
    {
        startPos = midPos = endPos = target.position;
    }

    private void Update()
    {
        Debug.DrawRay(NPCObj.transform.position + new Vector3(footDisplacementOnX, 0, 0), Vector2.down, Color.green);
        UpdateBalance();

        if (!isBalanced && lerp > 1)
        {
            CalculateNewStep();
        }

        // using ease in/ease out value will make the animation look more natural
        float easedLerp = EaseInOutCubic(lerp);

        target.position = Vector3.Lerp(startPos, endPos, easedLerp);
        lerp += Time.deltaTime * stepSpeed;

            // using ease in/ease out value will make the animation look more natural

        // a lerping method that draws an arc using startPos, midPos, and endPos
        target.position = Vector3.Lerp(
            Vector3.Lerp(startPos, midPos, easedLerp),
            Vector3.Lerp(midPos, endPos, easedLerp),
            easedLerp
            );
        lerp += Time.deltaTime * stepSpeed;

    }

    private float EaseInOutCubic(float x)
    {
        return 1f / (1 + Mathf.Exp(-10 * (x - 0.5f)));
    }

    private void CalculateNewStep()
    {

        startPos = target.position;

        lerp = 0;

        RaycastHit2D raycast = Physics2D.Raycast(NPCObj.transform.position + new Vector3(footDisplacementOnX, 0, 0), Vector2.down, 10, layer);

        Vector3 posDiff = ((Vector3)raycast.point - target.position) * (1 + overShootFactor);

        endPos = target.position + posDiff;

        bool thisFootCanMove = otherFoot.lerp > 1 && lerp > otherFoot.lerp;

        if (!isBalanced && lerp > 1 && thisFootCanMove)
        {
            CalculateNewStep();
        }

        float stepSize = Vector3.Distance(startPos, endPos);
        midPos = startPos + posDiff / 2f + new Vector3(0, stepSize * 0.8f);
    }

    private void UpdateBalance()
    {
        // get center of mass in world position
        float centerOfMass = NPCObj.transform.position.x;

        // if center of mass is between two feet, the body is balanced
        isBalanced = IsFloatInRange(centerOfMass, target.position.x - footDisplacementOnX, otherFoot.target.position.x - otherFoot.footDisplacementOnX);
    }
    bool IsFloatInRange(float value, float bound1, float bound2)
    {
        float minValue = Mathf.Min(bound1, bound2);
        float maxValue = Mathf.Max(bound1, bound2);
        return value > minValue + offset  && value < maxValue - offset;
    }

}