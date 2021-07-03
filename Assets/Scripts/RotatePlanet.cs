using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlanet : MonoBehaviour
{
    //  실행 순서!
    //  한 번만 실행된다
    //  void Awake() -> 1빠. 딱 한 번만 실행
    //  void OnEnable() -> 여러 번 실행될 수 있음
    //  void Start() -> OnEnable 실행되고 다음에 실행 됨

    //  계속 실행되는 함수
    //  void Update()

    public Vector3 RotateAxis = Vector3.up;
    public float RotateSpeed = 10.0f;

    private void Update()
    {
        //  Axis ? 축
        //  회전은 항상 '축 (Axis)' 이 있어야 실행할 수 있다 (회전할 수 있다)
        //  삼차원 (상용엔진 모두) 을 기준으로 하기 때문에 x, y, z 축을 사용할 수 있다
        //  2D 는 x, y 축 두개
        //  x -> Vector3.left, Vector3.right
        //  y -> Vector3.down, Vector3.up
        //  z -> Vector3.back, Vector3.forward

        transform.Rotate(RotateAxis, RotateSpeed * Time.deltaTime);

        //  유니티는 오일러 각(Euler) + 사원수(Quaternion) 두 가지를 사용한다
        //  1. Euler ? -> 짐벌락 이라는 단점이 있다
        //  - x, y, z 기준의 축을 가지고 회전시킬 수 있는 공식
        //  장점 ? 회전 값을 알기 쉽다 (유니티 Inspector 창에서 보이는 회전 값이 이 값이다!)
        //  단점 ? 느리다 (세 축을 한 번에 돌릴 수 없다. 따로따로 하나씩 곱해준다)
        //  2. Quaternion ?
        //  - x, y, z, w. 차원 + 1 개수 만큼 축을 한개 더 늘려서 사용
        //  장점 ? 연산이 빠르다 (한 번에 연산한다)
        //  단점 ? 보기가 어렵다 (0 ~ 1)
        //  그래서! 유니티는 볼 때는 Euler 를 이용하고, 연산은 Quaternion 을 사용한다

        // Quaternion rotation = transform.rotation;
        // Quaternion addRotate = Quaternion.Euler(
        //                             RotateAxis.x * RotateSpeed,
        //                             RotateAxis.y * RotateSpeed,
        //                             RotateAxis.z * RotateSpeed);
        // transform.rotation = rotation * addRotate;
    }

}
