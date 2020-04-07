using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKunai : MonoBehaviour
{

    bool isOnCooldown = false;
    public float fireTimer;
    public float fireCooldown = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleKunaiCooldown();

        KunaiInput();
    }

    private void HandleKunaiCooldown()
    {
        if (isOnCooldown)
        {
            fireTimer += Time.deltaTime;

            if (fireTimer >= fireCooldown)
            {
                isOnCooldown = false;
            }
        }
    }

    private void KunaiInput()
    {
        if (Input.GetButton("Fire1") && !isOnCooldown)
        {
            Singleton.Instance.KunaiInstantiate();
            isOnCooldown = true;
            fireTimer = 0f;
        }
    }
}
