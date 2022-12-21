using UnityEngine;

public class Shotgun : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform _shotgunShootPoint;
    [SerializeField] private float _shootDistance = 100f;
    [SerializeField] private GameObject _shootEffect;
    [SerializeField] private GameObject _gunHitEffect;
    [SerializeField] private float hitForce = 20f;
    [SerializeField] Recoil _recoilScript;
    #endregion

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("SHOOTING");
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        Vector3 hitForceVector = new Vector3(2f, 2f, 2f);
        GameObject muzzleInstance = Instantiate(_shootEffect, _shotgunShootPoint.position, _shotgunShootPoint.rotation);
        muzzleInstance.transform.parent = _shotgunShootPoint;

        _recoilScript.RecoilFire();

        if (Physics.Raycast(_shotgunShootPoint.position, _shotgunShootPoint.forward, out hit,_shootDistance))
        {
            // Do hit effects here
            if (hit.rigidbody)
            {
                hit.rigidbody.AddForce(Vector3.Scale(_shotgunShootPoint.forward, (hitForceVector * hitForce)), ForceMode.Impulse);
            }
            GameObject effect = Instantiate(_gunHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(effect, 1f);
        }
    }
}
