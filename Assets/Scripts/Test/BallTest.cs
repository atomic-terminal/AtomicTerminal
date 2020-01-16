using MyTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTest : MonoBehaviour
{
    private Transform ballTransform;
    private Vector3 moveDir;
    public LayerMask targetLayer;
    public float moveSpeed;
    private void Start()
    {
        ballTransform = this.transform;
        int r = Random.Range(0,360);
        moveDir = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * transform.forward;

        InitFX();
    }
    public void DoUpdate()
    {
        transform.Translate(moveDir * Time.deltaTime * moveSpeed);
        RayTrigger();
    }
    public float fieldAngle = 30;
    public float lineNum = 10;
    public float fieldDistance = 5;
    private void RayTrigger()
    {
        sphereRay();
    }
    private void sphereRay()
    {
        RaycastHit hitInfo;
        Vector3 originPos = transform.position;
        Vector3 originDir = Quaternion.AngleAxis(fieldAngle, Vector3.up) * moveDir;
        for (int i = 0; i < lineNum; i++)
        {
            Vector3 dir = Quaternion.AngleAxis(fieldAngle / lineNum * 2 * i, Vector3.down) * originDir;
            if (Physics.Raycast(originPos, dir, out hitInfo, fieldDistance, targetLayer))
            {
                moveDir = Vector3.Reflect(moveDir, hitInfo.normal);
                FXImpact(hitInfo);
                break;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 originPos = transform.position ;
        Vector3 originDir = Quaternion.AngleAxis(fieldAngle, Vector3.up) * moveDir;

        Gizmos.color = Color.red;
        for (int i = 0; i < lineNum; i++)
        {
            Vector3 dir = Quaternion.AngleAxis(fieldAngle / lineNum * 2 * i, Vector3.down) * originDir;
            Gizmos.DrawLine(originPos, originPos + dir * fieldDistance);
        }
    }

    private Transform FXparent;
    public bool FXshow;
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;

    public string poolName;
    List<GameObject> goList = new List<GameObject>();
    private void InitFX()
    {
        if (!FXshow) return;
        FXparent = GameObject.Find("FXparent").transform;
            projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
            projectileParticle.transform.parent = transform;
            if (muzzleParticle)
            {
                muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
                muzzleParticle.transform.rotation = transform.rotation * Quaternion.Euler(180, 0, 0);
                Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
            muzzleParticle.transform.SetParent(FXparent);
            }
    }
    private void FXImpact(RaycastHit hit)
    {
        if (!FXshow) return;
        //impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
        GameObject go = ObjectPoolHandler.instance.GetObjectFromPool(poolName, Vector3.zero, Quaternion.identity);
        go.transform.SetPositionAndRotation(transform.position, Quaternion.FromToRotation(Vector3.up, hit.normal));
        go.transform.SetParent(FXparent);
        if (go)
        {
            goList.Add(go);
        }
    }
}
