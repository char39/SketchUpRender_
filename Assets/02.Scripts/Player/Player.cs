using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable] // public으로 선언된 클래스를 인스펙터 창에 노출
public class PlayerAnimation
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBackward;
    public AnimationClip runRight;
    public AnimationClip runLeft;
    public AnimationClip sprint;
}

public class Player : MonoBehaviour
{
    public PlayerAnimation playerAnimation;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float finalMoveSpeed;
    private Vector3 moveDir;
    private float rotSpeed = 400f;
    [SerializeField] private Rigidbody rb;
    private CapsuleCollider capcol;
    private Transform tr;
    private Animation ani;
    private float h, v, r;

    private AudioSource source;
    public bool isRun;
    public bool isJump;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capcol = GetComponent<CapsuleCollider>();
        tr = GetComponent<Transform>();
        ani = GetComponent<Animation>();
        ani.Play(playerAnimation.idle.name);

        moveSpeed = 5f;
        finalMoveSpeed = moveSpeed;

        source = GetComponent<AudioSource>();

        isRun = false;
        isJump = false;
    }

    void Update()
    {
        PlayerMove_All();
    }

    private void PlayerMove_All()   // 플레이어의 움직이는 모든 것
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        r = Input.GetAxisRaw("Mouse X");
        moveDir = (h * Vector3.right + v * Vector3.forward).normalized;
        MoveAni();
        MoveRun();
        MoveJump();
        tr.Translate(moveDir * finalMoveSpeed * Time.deltaTime, Space.Self);     // Space.Self 로컬좌표, Space.World 절대좌표
        tr.Rotate(Vector3.up * r * rotSpeed * Time.deltaTime, Space.Self);
        
    }
    private void MoveRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && (Mathf.Abs(h) > 0.1 || Mathf.Abs(v) > 0.1))
        {
            finalMoveSpeed = moveSpeed * 1.5f;
            isRun = true;
        }
        else
        {
            finalMoveSpeed = moveSpeed;
            isRun = false;
        }
    }
    private void MoveJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            rb.velocity = new Vector3(0, 5f, 0);
            isJump = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
        }
    }

    private void MoveAni()  // 애니메이션만 재생
    {
        if (h > 0.1f)
        {
            ani.CrossFade(playerAnimation.runRight.name, 0.3f); // 0.3초 동안 천천히 전환
            if (finalMoveSpeed == moveSpeed)
                ani[playerAnimation.runRight.name].speed = 1.0f;
            else if (finalMoveSpeed == moveSpeed * 1.5f)
                ani[playerAnimation.runRight.name].speed = 1.5f;
        }
        else if (h < -0.1f)
        {
            ani.CrossFade(playerAnimation.runLeft.name, 0.3f);
            if (finalMoveSpeed == moveSpeed)
                ani[playerAnimation.runLeft.name].speed = 1.0f;
            else if (finalMoveSpeed == moveSpeed * 1.5f)
                ani[playerAnimation.runLeft.name].speed = 1.5f;
        }
        else if (v > 0.1f)
        {
            ani.CrossFade(playerAnimation.runForward.name, 0.3f);
            if (finalMoveSpeed == moveSpeed)
                ani[playerAnimation.runForward.name].speed = 1.0f;
            else if (finalMoveSpeed == moveSpeed * 1.5f)
                ani[playerAnimation.runForward.name].speed = 1.5f;
        }
        else if (v < -0.1f)
        {
            ani.CrossFade(playerAnimation.runBackward.name, 0.3f);
            if (finalMoveSpeed == moveSpeed)
                ani[playerAnimation.runBackward.name].speed = 1.0f;
            else if (finalMoveSpeed == moveSpeed * 1.5f)
                ani[playerAnimation.runBackward.name].speed = 1.5f;
        }
        else
        {
            ani.CrossFade(playerAnimation.idle.name, 0.3f);
            ani[playerAnimation.idle.name].speed = 1.0f;
        }
    }

}
