using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //������Ʈ ĳ�� ó���� ���� ����
    private CharacterController controller;
    private new Transform transform;
    private Animator animator;
    private new Camera camera;
    //������ Plange�� ����ĳ�����ϱ� ���� ����
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



        //Plange ����ü�� ������ �������� ������ �ٴ��� �����Ѵ�.
        //�ٴ��� ũ��� ��ġ������ ���Ǳ� ������ ������ �����̴�.
        //���� Ray�� �浹�� �ٴ��� �������� Box Collider�� �����Ѵٸ� ���ΰ��� �̵��� ��
        //���� �̵���Ű�ų�, �ƴϸ� ���������� ���� ũ�� �����ؾ��Ѵ�.

        //������ �ٴ��� ���ΰ��� ��ġ�� �������� ����
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
        //���ΰ� ĳ���� �̵�ó��
        controller.SimpleMove(moveDir * moveSpeed);
        //���ΰ� ĳ������ �ִϸ��̼� ó��
        float forward = Vector3.Dot(moveDir, transform.forward);
        float strafe = Vector3.Dot(moveDir, transform.right);
        //��Ʈ������ : ������� �¿� ������ó�� �� ��� �̵��ϴ� ���
        animator.SetFloat("Forward", forward);
        animator.SetFloat("Strafe", strafe);
    }
    void Turn()
    {
        ray = camera.ScreenPointToRay(Input.mousePosition);

        float enter = 0.0f;

        plane.Raycast(ray, out enter);
        //ray.GetPoint(float distance)��
        //ray�� ���� distance ��ŭ ������ ������ ��ȯ�Ѵ�.
        hitPoint = ray.GetPoint(enter);
        //ȸ���ؾ� �� ������ ���͸� ���
        Vector3 lookDir = hitPoint - transform.position;
        lookDir.y = 0;
        //���ΰ� ĳ������ ȸ�� �� ����
        transform.localRotation = Quaternion.LookRotation(lookDir);//���콺 Ŭ�� ������ �ٶ����
    }
}
