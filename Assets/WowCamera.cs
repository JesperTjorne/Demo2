using UnityEngine;

public class WowCamera : MonoBehaviour
{
    [SerializeField]
    public Transform Target;

    [SerializeField]
    private float targetHeight = 1.7f;
    [SerializeField]
    private float distance = 5.0f;
    [SerializeField]
    private float offsetFromWall = 0.1f;

    [SerializeField]
    private float maxDistance = 20;
    [SerializeField]
    private float minDistance = .6f;
    [SerializeField]
    private float speedDistance = 5;

    [SerializeField]
    private float xSpeed = 200.0f;
    [SerializeField]
    private float ySpeed = 200.0f;

    [SerializeField]
    private float playerControlledXSpeed = 100.0f;
    [SerializeField]
    private float playerControlledYSpeed = 100.0f;

    [SerializeField]
    private int yMinLimit = -40;
    [SerializeField]
    private int yMaxLimit = 80;

    [SerializeField]
    private int zoomRate = 5;

    [SerializeField]
    private float rotationDampening = 3.0f;
    [SerializeField]
    private float zoomDampening = 5.0f;

    [SerializeField]
    private LayerMask collisionLayers = -1;

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private float correctedDistance;

    void Awake()
    {
        Vector3 angles = transform.eulerAngles;
        xDeg = angles.x;
        yDeg = angles.y;

        currentDistance = distance;
        desiredDistance = distance;
        correctedDistance = distance;

        // Make the rigid body not change rotation
        if (this.gameObject.GetComponent<Rigidbody>())
            this.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
    }

    /**
     * Camera logic on LateUpdate to only update after all character movement logic has been handled.
     */
    void LateUpdate()
    {
        Vector3 vTargetOffset;

        // Don't do anything if target is not defined
        if (!Target)
            return;

        // If either mouse buttons are down, let the mouse govern camera position
        if (GUIUtility.hotControl == 0)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            }
            else if (Input.GetMouseButton(2))
            {
                float targetRotationAngle = Target.eulerAngles.y;
                float currentRotationAngle = transform.eulerAngles.y;
                xDeg = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
            }
        }

        // calculate the desired distance
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance) * speedDistance;
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);

        yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);

        // set camera rotation
        Quaternion rotation = Quaternion.Euler(yDeg, xDeg, 0);
        correctedDistance = desiredDistance;

        // calculate desired camera position
        vTargetOffset = new Vector3(0, -targetHeight, 0);
        Vector3 position = Target.position - (rotation * Vector3.forward * desiredDistance + vTargetOffset);

        // check for collision using the true target's desired registration point as set by user using height
        RaycastHit collisionHit;
        Vector3 trueTargetPosition = new Vector3(Target.position.x, Target.position.y, Target.position.z) - vTargetOffset;

        // if there was a collision, correct the camera position and calculate the corrected distance
        bool isCorrected = false;
        if (Physics.Linecast(trueTargetPosition, position, out collisionHit, collisionLayers.value))
        {
            // calculate the distance from the original estimated position to the collision location,
            // subtracting out a safety "offset" distance from the object we hit.  The offset will help
            // keep the camera from being right on top of the surface we hit, which usually shows up as
            // the surface geometry getting partially clipped by the camera's front clipping plane.
            correctedDistance = Vector3.Distance(trueTargetPosition, collisionHit.point) - offsetFromWall;
            isCorrected = true;
        }

        // For smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance
        currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * zoomDampening) : correctedDistance;

        // keep within legal limits
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        // recalculate position based on the new currentDistance
        position = Target.position - (rotation * Vector3.forward * currentDistance + vTargetOffset);

        transform.rotation = rotation;
        transform.position = position;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}