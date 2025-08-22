using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMover : NetworkBehaviour
{
    private Vector3 move_input;

    void Update()
    {
        if(IsOwner)
        this.transform.position += this.move_input * 3f * Time.deltaTime;
    }

    void OnMove(InputValue value)
    {
        Vector2 move_value = value.Get<Vector2>();

        this.move_input = new Vector3(move_value.x, 0, move_value.y);
    }
}
