using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 200f;
    private float _accumulationX = 0;

    private void Update()
    {
        if (GameManager.Instance.State != EGameState.Playing) return;
        if (GameManager.Instance.ViewMode == ECameraViewMode.Top)
            return;
        
        // 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X");
        // 마우스 입력을 누적한다
        _accumulationX += mouseX * RotationSpeed * Time.deltaTime;
      
        // 회전방향으로 카메라 회전하기
        transform.eulerAngles = new Vector3(0, _accumulationX, 0);
    }
}
