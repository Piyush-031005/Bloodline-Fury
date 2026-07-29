using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCapsule : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag the GameObject with PhoneController here, or leave empty if on same object.")]
    public PhoneController phoneController;
    
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    
    private Rigidbody rb;
    private Material mat;
    private Color originalColor;

    // Track previous state to detect "Button Down" frame events
    private bool prevX, prevY, prevA, prevB;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Auto-find phone controller if not assigned
        if (phoneController == null)
            phoneController = GetComponent<PhoneController>();
        if (phoneController == null)
            phoneController = FindObjectOfType<PhoneController>();

        // Get material to flash colors on attacks
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            mat = renderer.material;
            originalColor = mat.color;
        }
    }

    void FixedUpdate()
    {
        if (phoneController == null) return;

        // Apply Movement (Phone Joystick)
        Vector2 input = phoneController.JoystickInput;
        Vector3 move = new Vector3(input.x, 0, input.y) * moveSpeed;
        
        // Preserve vertical velocity for gravity
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
    }

    void Update()
    {
        if (phoneController == null) return;

        // Detect Actions (Just Pressed)
        if (phoneController.ButtonY && !prevY) 
            TriggerAttack("Heavy Punch (Y)", Color.red);
            
        if (phoneController.ButtonX && !prevX) 
            TriggerAttack("Light Punch (X)", Color.blue);
            
        if (phoneController.ButtonB && !prevB) 
            TriggerAttack("Special (B)", Color.yellow);
            
        if (phoneController.ButtonA && !prevA) 
            TriggerAttack("Kick (A)", Color.green);

        // Store state for next frame
        prevX = phoneController.ButtonX;
        prevY = phoneController.ButtonY;
        prevA = phoneController.ButtonA;
        prevB = phoneController.ButtonB;
    }

    private void TriggerAttack(string attackName, Color flashColor)
    {
        Debug.Log($"<color=white><b>{attackName} EXECUTE!</b></color>");
        
        if (mat != null)
        {
            mat.color = flashColor;
            CancelInvoke("ResetColor");
            Invoke("ResetColor", 0.15f);
        }
    }

    private void ResetColor()
    {
        if (mat != null) mat.color = originalColor;
    }
}
