using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Attributes

    // dependent on player speed 
    public float speedBoost = 1f;
    public float birth = 0f;

    float unitsPerSeconds = 5f;
    float speed = 0;
    float lifetime = 4f;

    #endregion 

    #region MonoBehaviour API

    void OnEnable()
    {
        // kill bullet at lifetime
        Invoke("Destroy", lifetime);
    }

    void Start()
    {
        // moved Invoke to OnEnable - some bullets were never dying
    }

    // Update is called once per frame
    void Update()
    {
        speed = Mathf.Lerp(speedBoost, 1f, (Time.time - birth) / lifetime);
        var forwardDir = transform.InverseTransformDirection(transform.up);
        transform.Translate(forwardDir * speed * unitsPerSeconds * Time.deltaTime);
    }

    #endregion
   
    private void Destroy()
    {
        // regular instancing - delete object
        //Destroy(gameObject);

        // spawn from pool - disable object
        gameObject.SetActive(false);
    }
}
