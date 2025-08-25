using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerContorller : MonoBehaviour
{
    [SerializeField] private GameObject[] anim_objs;

    private Vector3 move_input;
    private Rigidbody2D rb;

    private float move_spd = 3f;
    private float jump_power = 10f;

    private bool isAttack = false;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isAttack)
            return;

        float dir_x = this.move_input.x;

        if (dir_x != 0)
        {

            this.transform.localScale = new Vector3(dir_x < 0 ? 1 : -1, 1, 1);
            SetAnimObject(0);


            transform.position += this.move_input * 3f * Time.deltaTime;
        }
        else
        {
            SetAnimObject(1);
        }
    }

    void OnMove(InputValue value)
    {
        Vector3 move_temp = value.Get<Vector2>();
        this.move_input = new Vector3(move_temp.x, move_temp.y, 0);
    }

    void OnJump()
    {
        rb.AddForceY(jump_power, ForceMode2D.Impulse);
    }

    void OnAttack()
    {
        if (!isAttack)
            StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        this.isAttack = true;
        SetAnimObject(2);

        yield return new WaitForSeconds(1f);

        SetAnimObject(0);
        this.isAttack = false;
    }

    private void SetAnimObject(int index)
    {
        foreach (GameObject element in anim_objs)
        {
            element.SetActive(false);
        }

        anim_objs[index].SetActive(true);
    }
}
