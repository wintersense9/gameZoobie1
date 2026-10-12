using UnityEngine;

/// <summary>
/// Human player movement via WASD / arrow keys.
/// Disabled automatically when the ML agent is in control.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Disable human input component when the ML agent is present
        if (GetComponent<SurvivorAgent>() != null)
            enabled = false;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(h, 0f, v);
        controller.Move(move * speed * Time.deltaTime);
    }
}