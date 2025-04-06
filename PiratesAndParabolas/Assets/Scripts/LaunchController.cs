using UnityEngine;
using TMPro;

public class LaunchController : MonoBehaviour
{
    public TMP_InputField angleInputField; // Input for angle
    public TMP_InputField strengthInputField; // Input for strength
    public TMP_InputField lengthInputField; // Input for length
    public TMP_InputField heightInputField; // Input for height
    public GameObject cannonballPrefab; // Cannonball prefab
    public Transform launchPoint; // Where the cannonball is launched from
    public Transform CannonPivot; // The pivot point of the cannon  

    private float gravity = 9.81f;

    bool AngleWasLastChanged = false;

    // Call this method to process user inputs and launch the cannonball
    public void LaunchProjectile()
    {
        // Check if user is using length/height or angle/strength inputs
        if (!AngleWasLastChanged)
        {
            // Convert length/height to angle/strength
            float range = float.Parse(lengthInputField.text);
            float maxHeight = float.Parse(heightInputField.text);

            (float angle, float strength) = CalculateAngleAndStrengthFromLengthAndHeight(range, maxHeight);

            // Update UI to show length and height values
            angleInputField.text = angle.ToString();
            strengthInputField.text = strength.ToString();

            // Launch the cannonball
            LaunchCannonball(angle, strength);
        }
        else
        {
            // Convert angle/strength to length/height
            float angle = float.Parse(angleInputField.text);
            float strength = float.Parse(strengthInputField.text);

            (float range, float maxHeight) = CalculateLengthAndHeightFromAngleAndStrength(angle, strength);

            // Update UI to show length and height values
            lengthInputField.text = range.ToString();
            heightInputField.text = maxHeight.ToString();

            // Launch the cannonball
            LaunchCannonball(angle, strength);
        }
    }

    // Convert length and height to angle and strength
    (float, float) CalculateAngleAndStrengthFromLengthAndHeight(float range, float height)
    {
        // Calculate the strength (initial speed)
        float strength = Mathf.Sqrt((range * gravity) / Mathf.Sin(2 * Mathf.Deg2Rad * 45f)); // Assume 45 degrees for simplicity

        // Calculate the angle (in radians)
        float angle = Mathf.Atan((2 * height) / range); // Simplified approach, assumes 45 degrees initially

        // Set the Z rotation of the CannonPivot to the specified angle
        CannonPivot.rotation = Quaternion.Euler(0, 0, -90 + angle);

        return (angle * Mathf.Rad2Deg, strength);
    }

    // Convert length and height to angle and strength
    public void CalculateAngleAndStrengthFromLengthAndHeight() {

        AngleWasLastChanged = false;

        // Calculate the strength (initial speed)
        strengthInputField.text = "" + Mathf.Sqrt((float.Parse(lengthInputField.text) * gravity) / Mathf.Sin(2 * Mathf.Deg2Rad * 45f)); // Assume 45 degrees for simplicity

        // Calculate the angle (in radians)
        angleInputField.text = "" + Mathf.Rad2Deg*Mathf.Atan((2 * float.Parse(heightInputField.text)) / float.Parse(lengthInputField.text)); // Simplified approach, assumes 45 degrees initially

        // Set the Z rotation of the CannonPivot to the specified angle
        CannonPivot.rotation = Quaternion.Euler(0, 0, -90 + float.Parse(angleInputField.text));

    }

    // Convert angle and strength to length and height
    (float, float) CalculateLengthAndHeightFromAngleAndStrength(float angle, float strength)
    {
        // Convert angle to radians
        float angleRadians = Mathf.Deg2Rad * angle;

        // Calculate the range (horizontal distance)
        float range = (Mathf.Pow(strength, 2) * Mathf.Sin(2 * angleRadians)) / gravity;

        // Calculate the maximum height
        float maxHeight = Mathf.Pow(strength * Mathf.Sin(angleRadians), 2) / (2 * gravity);

        CannonPivot.rotation = Quaternion.Euler(0, 0, -90 + angle);

        return (range, maxHeight);
    }

    // Convert angle and strength to length and height
    public void CalculateLengthAndHeightFromAngleAndStrength()
    {

        AngleWasLastChanged = true;

        CannonPivot.rotation = Quaternion.Euler(0, 0, -90 + float.Parse(angleInputField.text));

        // Convert angle to radians
        float angleRadians = Mathf.Deg2Rad * float.Parse(angleInputField.text);

        // Calculate the range (horizontal distance)
        lengthInputField.text = ""+(Mathf.Pow(float.Parse(strengthInputField.text), 2) * Mathf.Sin(2 * angleRadians)) / gravity;

        // Calculate the maximum height
        heightInputField.text = ""+Mathf.Pow(float.Parse(strengthInputField.text) * Mathf.Sin(angleRadians), 2) / (2 * gravity);

    }

    // Launch the cannonball with the calculated angle and strength
    void LaunchCannonball(float angle, float strength)
    {

        // Set the Z rotation of the CannonPivot to the specified angle
        CannonPivot.rotation = Quaternion.Euler(0, 0, -90+angle);


        // Instantiate the cannonball at the launch point
        GameObject cannonball = Instantiate(cannonballPrefab, launchPoint.position, Quaternion.identity);

        // Calculate the initial velocity components
        float angleRadians = Mathf.Deg2Rad * angle;
        Vector3 initialVelocity = new Vector3(
            strength * Mathf.Cos(angleRadians),  // Horizontal component
            strength * Mathf.Sin(angleRadians),  // Vertical component
            0f
        );

        // Set the cannonball's initial velocity
        Cannonball cannonballScript = cannonball.GetComponent<Cannonball>();
        cannonballScript.SetInitialVelocity(initialVelocity);
    }
}
