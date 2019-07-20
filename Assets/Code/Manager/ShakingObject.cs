using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    private GameObject shakeObject;
    private bool shaking = false;
    private float shakePower;

    void Update()
    {
        if(shaking)
        {
            Vector3 newpos = Random.insideUnitSphere * (Time.deltaTime * shakePower);
            newpos.y = transform.position.y;
            newpos.z = shakeObject.transform.position.z;
            shakeObject.transform.position = newpos;
        }
    }

    public void Shake(float power, GameObject shake)
    {
        shakeObject = shake;
        shakePower = power;
        StartCoroutine(Shaking());
    }

    IEnumerator Shaking()
    {
        Vector3 origin = shakeObject.transform.position;

        if(!shaking)
        {
            shaking = true;
        }

        yield return new WaitForSeconds(0.5f);

        shaking = false;
        shakeObject.transform.position = origin;
    }

}
