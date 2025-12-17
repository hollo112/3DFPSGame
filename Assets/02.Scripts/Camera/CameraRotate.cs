using UnityEngine;

// 카메라 회전 기능
// 마우스를 조작하면 카메라를 그 방향으로 회전하고 싶다
public class CameraRotate : MonoBehaviour
{
   [SerializeField] private Transform Top;
   private bool _isTopRotationInitialized = false;
   
   // 게임이 시작되면 y축이 0도에서 -> -1도
   public float RotationSpeed = 200f;
   
   public float DeltaPitch { get; private set; }
   private float _previousPitch;
   
   // 유니티는 0~360각도 체계이므로 우리가 따로 저장할 -360 ~ 360 체계로 누적할 변수
   private float _accumulationX = 0;
   private float _accumulationY = 0;
   private void Update()
   {
      if (GameManager.Instance.State != EGameState.Playing) return;
      if (GameManager.Instance.ViewMode == ECameraViewMode.Top)
      {
         UpdateTopRotation();
      }
      else
      {
         _isTopRotationInitialized = false;
         UpdateFreeRotation();
      }
   }
   
   private void UpdateTopRotation()
   {
      if (_isTopRotationInitialized)
         return;
      
      transform.rotation = Top.rotation;

      Vector3 euler = Top.rotation.eulerAngles;
      _accumulationX = euler.y;
      _accumulationY = euler.x;

      DeltaPitch = 0f;
      _previousPitch = _accumulationY;
      _isTopRotationInitialized = true;
   }

   private void UpdateFreeRotation()
   {
      // 마우스 입력 받기
      float mouseX = Input.GetAxis("Mouse X");
      float mouseY = Input.GetAxis("Mouse Y");
      
      // 마우스 입력을 누적한다
      _accumulationX += mouseX * RotationSpeed * Time.deltaTime;
      _accumulationY += -mouseY * RotationSpeed * Time.deltaTime;
      
      // 사람처럼 -90 ~ 90도 사이로 제한한다
      _accumulationY = Mathf.Clamp(_accumulationY, -90f, 90f);
      
      
      // 회전방향으로 카메라 회전하기
      transform.rotation = Quaternion.Euler(_accumulationY, _accumulationX, 0);
      
      DeltaPitch = _accumulationY - _previousPitch;
      _previousPitch = _accumulationY;
      // 쿼터니언 : 사원수 : 쓰는 이유는 짐벌락 현상 방지
      // 공부 : 짐벌락, 쿼터니언
   }
}
