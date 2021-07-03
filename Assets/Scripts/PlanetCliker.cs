using UnityEngine;

public class PlanetCliker : MonoBehaviour
{
    public float ZoomScale = 100.0f;

    //  이 함수는 Collider 가 있을 때만 사용할 수 있다!
    private void OnMouseUpAsButton()
    {
        //  FindObjectOfType? 컴포넌트 타입으로 객체를 찾는다.
        CameraManager manager = FindObjectOfType<CameraManager>();
        manager.ZoomPlanet(transform, ZoomScale);
    }

}
