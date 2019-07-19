using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private bool shaking = false;
    private float shakePower;

    void Update()
    {
        if(shaking)
        {
            Vector3 newpos = Random.insideUnitSphere * (Time.deltaTime * shakePower);
            //newpos.y = transform.position.y;
            newpos.z = transform.position.z;
        
            transform.position = newpos;
        }
    }

    public void Shake(float power)
    {
        shakePower = power;
        StartCoroutine(Shaking());
    }

    IEnumerator Shaking()
    {
        Vector3 origin = transform.position;

        if(!shaking)
        {
            shaking = true;
        }

        yield return new WaitForSeconds(0.5f);

        shaking = false;
        transform.position = origin;
    }

}
