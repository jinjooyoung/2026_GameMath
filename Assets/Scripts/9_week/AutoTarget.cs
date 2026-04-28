using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutoTarget : MonoBehaviour
{
    public PredictionLineRender lr;
    public CameraSlerp cameraSlerp;
    public Transform cameraDefaultPos;

    public void OnRightClick(InputValue value)
    {
        if (!value.isPressed) return;
        Debug.Log("입력들어옴");

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // 타게팅
                lr.endPos = hit.transform;
                cameraSlerp.target = hit.transform;
            }
        }
        else
        {
            // 초기화
            lr.endPos = lr.transform;   // 자기자신 찍어도 되지만 라인렌더러의 positionCount를 0으로 해도 안 보임
            cameraSlerp.target = cameraDefaultPos;
        }
    }
}
