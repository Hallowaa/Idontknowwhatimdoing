using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{

    public PlayerMovement player;

    public Eyes playerCamera;

    public GameObject shootingPoint;

    public GameObject throwPoint;

    public GameObject kunai;

    public GameObject rasenShuriken;

    public GameObject rasenExplosion;

    public static Singleton Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        player = FindObjectOfType<PlayerMovement>();
        playerCamera = player.GetComponentInChildren<Eyes>();
        shootingPoint = GameObject.Find("Shooting Point");
        throwPoint = GameObject.Find("Throw Point");
    }

    public void KunaiInstantiate()
    {
        GameObject Shuriken = Instantiate(kunai, shootingPoint.transform.position, playerCamera.transform.rotation) as GameObject;
    }

    public void RasenShurikenInstatiate()
    {
        GameObject RasenShuriken = Instantiate(rasenShuriken, throwPoint.transform.position, playerCamera.transform.rotation) as GameObject;
    }




}
