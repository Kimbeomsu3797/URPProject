using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //컴포넌트 캐시 처리를 위한 변수
    private CharacterController controller;
    private new Transform transform;
    private Animator animator;
    private new Camera camera;
    //가상의 Plange에 레이캐스팅하기 위한 변수
    private Plane plane;
    private Ray ray;
    private Vector3 hitPoint;

    public float moveSpeed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        camera = Camera.main;



        //Plange 구조체는 지정한 지점에서 가상의 바닥을 생성한다.
        //바닥의 크기는 수치적으로 계산되기 때문에 무한한 영역이다.
        //만약 Ray에 충돌할 바닥을 물리적인 Box Collider로 구현한다면 주인공이 이동할 때
        //같이 이동시키거나, 아니면 물리적으로 아주 크게 생성해야한다.

        //가상의 바닥을 주인공의 위치를 기준으로 생성
        plane = new Plane(transform.up, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Turn();
    }

    float h => Input.GetAxis("Horizontal");
    float v => Input.GetAxis("Vertical");
    private void Move()
    {
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;
        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        Vector3 moveDir = (cameraForward * v) + (cameraRight * h);
        moveDir.Set(moveDir.x, 0.0f, moveDir.z);
        //주인공 캐릭터 이동처리
        controller.SimpleMove(moveDir * moveSpeed);
        //주인공 캐릭터의 애니메이션 처리
        float forward = Vector3.Dot(moveDir, transform.forward);
        float strafe = Vector3.Dot(moveDir, transform.right);
        //스트레이핑 : 서든어택 좌우 움직임처럼 좌 우로 이동하는 모습
        animator.SetFloat("Forward", forward);
        animator.SetFloat("Strafe", strafe);
    }
    void Turn()
    {
        ray = camera.ScreenPointToRay(Input.mousePosition);

        float enter = 0.0f;

        plane.Raycast(ray, out enter);
        //ray.GetPoint(float distance)는
        //ray를 따라 distance 만큼 떨어진 지점을 반환한다.
        hitPoint = ray.GetPoint(enter);
        //회전해야 할 방향의 벡터를 계산
        Vector3 lookDir = hitPoint - transform.position;
        lookDir.y = 0;
        //주인공 캐릭터의 회전 값 지정
        transform.localRotation = Quaternion.LookRotation(lookDir);//마우스 클릭 지점을 바라봐라
    }
}
