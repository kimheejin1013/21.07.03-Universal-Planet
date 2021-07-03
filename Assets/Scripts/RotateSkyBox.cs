using System.Collections;
using UnityEngine;

public class RotateSkyBox : MonoBehaviour
{
    public Material Material;//회전시킬 재질
    public float RotateSpeed;//회전시킬 파워
    private float m_RotatdValue = 0;//현재 회전 값

    private void Update()
    {
        m_RotatdValue = m_RotatdValue + (RotateSpeed * Time.deltaTime);
            if (m_RotatdValue >= 360f) m_RotatdValue -= 360f;

        Material.SetFloat("_Rotation", m_RotatdValue);
    }
}