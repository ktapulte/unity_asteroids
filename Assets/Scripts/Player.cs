using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // prefab for regular instancing
    //public GameObject bulletPrefab;

    // prefab for pool instances (in the Resources folder)
    private const string BULLET_PREFAB_PATH = "Prefabs/Bullet";

    // player
    public float anglePerSeconds = 90f;
    public float unitsPerSeconds = 10f;

    // bullets
    public float bulletSpawnDistance = 0.5f;
    float unitsPerSecondsBooster = 1f;

    float speedMin = 0.1f;
    float speedMax = 3f;

    // acceleration
    float durationAccel = 2f;
    float startAccel = 0f;
    float speedCoef = 1f;
    float fwd_speed = 0f;
    float bwd_speed = 0f;

    // deceleration
    float durationDecel = 4f;
    //float startDecel = 0f;
    float fwd_startDecel = 0f;
    float bwd_startDecel = 0f;
    float fwd_momentum = 0f;
    float bwd_momentum = 0f;

    float timeBetweenShots = 0.2f;
    float timestamp = 0f;


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            startAccel = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            startAccel = Time.time;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            speedCoef = Mathf.Lerp(speedMin, speedMax, (Time.time - startAccel) / durationAccel);
            transform.Translate(Vector3.up * speedCoef * unitsPerSeconds * Time.deltaTime);
            fwd_speed = speedCoef;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            speedCoef = Mathf.Lerp(speedMin, speedMax, (Time.time - startAccel) / durationAccel);
            transform.Translate(Vector3.down * speedCoef * unitsPerSeconds * Time.deltaTime);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            //startDecel = Time.time;
            fwd_startDecel = Time.time;
            fwd_speed = speedCoef;
            fwd_momentum = 1;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            //startDecel = Time.time;
            bwd_startDecel = Time.time;
            bwd_speed = speedCoef;
            bwd_momentum = 1;
        }

        if (fwd_momentum > 0)
        {
            fwd_momentum = 1f - Mathf.Clamp((Time.time - fwd_startDecel) / durationDecel, 0f, 1f);
            transform.Translate(Vector3.up * fwd_momentum * fwd_speed * unitsPerSeconds * Time.deltaTime);
        }

        if (bwd_momentum > 0)
        {
            bwd_momentum = 1f - Mathf.Clamp((Time.time - bwd_startDecel) / durationDecel, 0f, 1f);
            transform.Translate(Vector3.down * bwd_momentum * bwd_speed * unitsPerSeconds * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, 0, anglePerSeconds * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, 0, -anglePerSeconds * Time.deltaTime);
        }

        // TO DO - player moves faster than bullets !!!
        unitsPerSecondsBooster = Mathf.Lerp(1f, 2f * speedMax, fwd_speed / speedMax);

        // mouse down shoots too many times !!!
        if (Input.GetMouseButton(0)
            && Time.time > timestamp) {
            FireBullet(transform.up * bulletSpawnDistance + transform.position, transform.rotation);
            timestamp = Time.time + timeBetweenShots;
        }

        if (Input.GetMouseButtonDown(0)
            || Input.GetKeyDown(KeyCode.LeftControl)
            || Input.GetKeyDown(KeyCode.Space))
        {
            FireBullet(transform.up * Mathf.Max(1, fwd_speed) * bulletSpawnDistance + transform.position, transform.rotation);
        }

    }

    private void FireBullet(Vector3 position, Quaternion rotation)
    {
        // Regular instancing of prefab
        // Instantiate(bulletPrefab, position, rotation);

        // Use instance from pool
        var bullet = ObjectPooler.GetPoolObject(BULLET_PREFAB_PATH);
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.SetActive(true);
        bullet.GetComponent<Bullet>().speedBoost = unitsPerSecondsBooster;
        bullet.GetComponent<Bullet>().birth = Time.time;

        Debug.Log("fwd_speed = " + fwd_speed + ", speed booster = " + unitsPerSecondsBooster);
    }
}

