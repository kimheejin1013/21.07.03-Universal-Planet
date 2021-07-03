using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera m_Camera;
    private Vector3 m_ResetPosition;    //  확대되었다가 다시 돌아갈 위치
    private Quaternion m_ResetRotation; //  확대되었다가 다시 돌아갈 회전

    private Transform m_PlanetTransform = null; //  선택한 행성 Transform
    private float m_ZoomScale = 0;              //  선택한 행성의 Z Offset (거리)
    private bool m_IsUpdate = false;            //  확대되는 중 인가?

    private void Awake()
    {
        //  GetComponent ?
        //  gameObject 객체 안에 Camera 컴포넌트를 찾아서 받는다!
        m_Camera = GetComponent<Camera>();
     //   StartCoroutine(UpdateLog(new string[] { "A", "B", "C", "D" }));
    }

    private void Update()
    {
        if (m_PlanetTransform == null) return;

        transform.position = new Vector3(
            m_PlanetTransform.position.x,
            m_PlanetTransform.position.y,
            m_PlanetTransform.position.z - m_ZoomScale
            );

        if (Input.GetKeyDown(KeyCode.Mouse1)) ResetCamera();
    }

    //  함수 ? Function()
    //  소괄호 안에는 파라메터(인자) 를 넣을 수 있다
    //  파라메터가 있는 함수는 사용할 때 반드시 파라메터를 넣어주어야 한다
    public void ZoomPlanet(Transform target, float zoomScale)
    {
        if (m_PlanetTransform != null || m_IsUpdate) return;

        StartCoroutine(UpdateSmoothCamera(target, zoomScale));
    }

    public void ResetCamera()
    {
        if (m_PlanetTransform == null || m_IsUpdate) return;

        m_PlanetTransform = null;

        StartCoroutine(UpdateResetCamera());
    }

   /* //  IEnumerator 예제
    //  매 프레임마다 Debug.Log를 사용하며 logs의 개수만큼 출력하기!
    private IEnumerator UpdateLog(string[] logs)
    {
        //  ++i, i++, i += 1 -> 전위 연산자, 후위 연산자, 중위 연산자 찾아보기
        for (int i = 0; i < logs.Length; ++i)
        {
            Debug.Log(logs[i]);
            yield return new WaitForSeconds(1f);  //  한 프레임 쉬어라
        }

        yield break;    //  이 반복문이 종료되었음을 알리는 제어문
    }*/

    //  에러 나오면 코드 맨 위에다가 using System.Collections; 추가
    //  IEnumerator ? 반복자
    //  for 문이랑 비슷한데, 여러 기능이 있다!
    //  대표적으로 WaitForSecond(), yield return null 등등...
    //  제어할 수 있는 기능이 많다! (잠깐 쉰다거나, 중간에 종료한다거나 등등...)
    private IEnumerator UpdateSmoothCamera(Transform planetTransform, float zoomScale)
    {
        //  다른 마우스 입력을 방지하기 위해 bool업데이트를 켜 준다
        //  확대한 후 축소할 때 기존 카메라의 위치와 회전값으로 돌아가야 하므로
        //  m_Reset 변수에 위치와 회전값을 저장한다.
        m_IsUpdate = true;
        m_ResetPosition = transform.position;
        m_ResetRotation = transform.rotation;

        const float SmoothTimeSecond = 2f;

        float currentTime = Time.time;  //  시작한 시간
        float fixedTIme = Time.time + SmoothTimeSecond; //  최종적으로 변할 때까지의 시간
        while(Time.time <= fixedTIme)
        {
            //  시간에 대한 백분율을 구한다
            float timePercent = (Time.time - currentTime) / SmoothTimeSecond;


            //  Lerp ? 선형 보간 (선형 보간법)
            //  사용법 -> Lerp(from, to, percent)
            //  Lerp(0, 1, 0.5) -> 0.5, Lerp(0, 10, 0.5) -> 5
            Vector3 newPosition = new Vector3(
                Mathf.Lerp(m_ResetPosition.x, planetTransform.position.x, timePercent),
                Mathf.Lerp(m_ResetPosition.y, planetTransform.position.y, timePercent),
                Mathf.Lerp(m_ResetPosition.z, planetTransform.position.z - zoomScale, timePercent)
                );

            Vector3 direction = planetTransform.position - newPosition;
            Quaternion newRotation = Quaternion.LookRotation(direction);

            //transform.position = newPosition;
            // transform.rotation = Quaternion.Lerp(m_ResetRotation, newRotation, timePercent);
            transform.SetPositionAndRotation(
                newPosition,
                Quaternion.Lerp(m_ResetRotation, newRotation, timePercent));

            yield return null;
        }

        //  위에서 보간한 값이 내가 원하는 값으로 완전히 떨어지는 보장이 없다
        //  Ex) 2초 동안 Position이 (0, 0, 0)에서 (0, 0, 100)으로 가라!
        //  돌고 돌다가 (0, 0, 99.8f...) 이런 식일 수 있음
        //  그래서 보간 시간이 다 되면 마지막에 내가 원하는 값으로 고정시켜준다.
        transform.SetPositionAndRotation(
            new Vector3(
                    planetTransform.position.x,
                    planetTransform.position.y,
                    planetTransform.position.z - zoomScale),
            Quaternion.LookRotation(planetTransform.position - transform.position));

        m_PlanetTransform = planetTransform;
        m_ZoomScale = zoomScale;
        m_IsUpdate = false;

        yield break;
    }

    private IEnumerator UpdateResetCamera()
    {
        m_IsUpdate = true;

        const float ReleaseTimeSecond = 2f;
        float currentTime = Time.time;
        float fixedTime = Time.time + ReleaseTimeSecond;

        Vector3 prevPosition = transform.position;
        Quaternion prevRotation = transform.rotation;

        while(Time.time <= fixedTime)
        {
            float timePercent = (Time.time - currentTime) / ReleaseTimeSecond;

            Vector3 newPosition = Vector3.Lerp(prevPosition, m_ResetPosition, timePercent);
            Quaternion newRotation = Quaternion.Lerp(prevRotation, m_ResetRotation, timePercent);

            transform.SetPositionAndRotation(newPosition, newRotation);


            yield return null;
        }

        transform.SetPositionAndRotation(m_ResetPosition, m_ResetRotation);

        m_IsUpdate = false;
        yield break;
    }

}