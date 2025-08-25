using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkPlayerController : NetworkBehaviour
{
    /// <summary> 0 : Idle , 1 : Move , 2: Attack </summary> 
    private NetworkVariable<int> cur_anim_state = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] private GameObject[] anim_objs;

    private Vector3 move_input;
    private Rigidbody2D rb;

    private float move_spd = 3f;
    private float jump_power = 10f;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        rb = GetComponent<Rigidbody2D>();
        cur_anim_state.OnValueChanged += UpdateAnimation;

        if (!IsServer)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        if (!IsOwner)
        {
            this.GetComponent<PlayerInput>().enabled = false;
        }
        else
        {
            CameraFollow camera_follow = Camera.main.GetComponent<CameraFollow>();

            if (camera_follow != null)
            {
                camera_follow.target = this.transform;
            }
        }

    }

    void Update()
    {
        if (IsOwner)
        {
            MovementServerRpc(this.move_input);
        }
    }

    [ServerRpc]
    private void MovementServerRpc(Vector2 param_input)
    {
        if (cur_anim_state.Value == 2)
            return;

        float dir_x = param_input.x;

        if (dir_x != 0)
        {
            // rb.linearVelocity = new Vector2(param_input.x * move_spd, rb.linearVelocityY);
            this.transform.localScale = new Vector3(dir_x < 0 ? 1 : -1, 1, 1);

            rb.linearVelocity = new Vector2(dir_x * move_spd, rb.linearVelocity.y);
            cur_anim_state.Value = 1;
        }
        else
        {
            cur_anim_state.Value = 0;
        }
    }

    void OnMove(InputValue value)
    {
        this.move_input = value.Get<Vector2>();
    }

    void OnJump()
    {
        if (IsOwner)
            JumpServerRpc();
    }

    [ServerRpc]
    private void JumpServerRpc()
    {
        rb.AddForceY(jump_power, ForceMode2D.Impulse);
    }

    void OnAttack()
    {
        if (IsOwner && cur_anim_state.Value != 2)
            AttackServerRpc();
    }

    [ServerRpc]
    private void AttackServerRpc()
    {
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        cur_anim_state.Value = 2;
        yield return new WaitForSeconds(1f);
        cur_anim_state.Value = 0;
    }

    /// <summary> 현재 상태에 따른 애니메이션 활성화 . 현재 액션 타입 변환 후 호출  </summary>
    private void UpdateAnimation(int perv_index, int new_index)
    {
        foreach (GameObject element in anim_objs)
        {
            element.SetActive(false);
        }

        anim_objs[new_index].SetActive(true);
    }
}
