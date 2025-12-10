using TMPro;
using UnityEngine;

public class PlayerGunFire : MonoBehaviour
{
    // 목표: 마우스의 왼쪽 버튼을 누르면 바라보는 방향으로 총을 발사하고 싶다. (총알을 날리고 싶다.)
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private ParticleSystem _hitEffect;
    
    public int MagazineMaxSize = 30;
    private int _magazineSize = 30;
    public int MagzineSize => _magazineSize;
    public int MagazineCount = 5; 
    private int _reserveAmmo;
    public int ReserveAmmo =>_reserveAmmo;
    
    public float FireCooldown = 0.1f;
    private float _timer = 0;

    private void Start()
    {
        _reserveAmmo = MagazineMaxSize * (MagazineCount - 1);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
       
        if (_magazineSize <= 0) return;
        
        Fire();

    }

    private void Reload()
    {
        int reloadCount = MagazineMaxSize - _magazineSize;

        if (reloadCount <= 0) return; 
        if (_reserveAmmo <= 0) return; 

        int take = Mathf.Min(reloadCount, _reserveAmmo);
        _magazineSize += take;
        _reserveAmmo -= take;
    }
    private bool CanFireCoolDown()
    {
        _timer += Time.deltaTime;
        if (_timer <= FireCooldown)
        {
            return false;
        }
        else
        {
            _timer = 0;
            return true;
        }
    }
    
    private void Fire()
    {
        if (!CanFireCoolDown()) return;
        
        // 1. 마우슨 왼쪽 버튼이 눌린다면..
        if (Input.GetMouseButton(0))
        {
            _magazineSize--;
            // 2. Ray를 생성하고 발사할 위치, 방향, 거리를 설정한다. (쏜다.)
            Ray ray = new Ray(_fireTransform.position, Camera.main.transform.forward);
            
            // 3. RayCastHit(충돌한 대상의 정보)를 저장할 변수를 생성한다.
            RaycastHit hitInfo = new RaycastHit();
            
            // 4. 발사하고,
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                // 5.  충돌했다면... 피격 이펙트 표시
                Debug.Log(hitInfo.transform.name);
                
                _hitEffect.transform.position = hitInfo.point;
                _hitEffect.transform.forward = hitInfo.normal;
                _hitEffect.Play();
            }
        }
    }
}