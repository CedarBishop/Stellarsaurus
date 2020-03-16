using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [HideInInspector] public int pairNumber;
    [HideInInspector] public bool isTeleporterOne;
    [HideInInspector] public bool isActive;

    private void Start()
    {
        isActive = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive == false)
        {
            return;
        }
        if (collision.CompareTag("Player"))
        {
            if (isTeleporterOne)
            {
                if (LevelManager.instance.teleporterPairs[pairNumber].teleporterTwo)
                {
                    LevelManager.instance.teleporterPairs[pairNumber].DeactivateTeleporters();
                    collision.gameObject.transform.position = LevelManager.instance.teleporterPairs[pairNumber].teleporterTwo.transform.position;
                    StartCoroutine("ReactivateTeleporters");
                }
            }
            else
            {
                if (LevelManager.instance.teleporterPairs[pairNumber].teleporterOne)
                {
                    LevelManager.instance.teleporterPairs[pairNumber].DeactivateTeleporters();
                    collision.gameObject.transform.position = LevelManager.instance.teleporterPairs[pairNumber].teleporterOne.transform.position;
                    StartCoroutine("ReactivateTeleporters");
                }
            }
        }
    }

    IEnumerator ReactivateTeleporters ()
    {
        yield return new WaitForSeconds(3);
        LevelManager.instance.teleporterPairs[pairNumber].ActivateTeleporters();
    }

}





[System.Serializable]
public class TeleporterPairs
{

    public Teleporter teleporterOne;
    public Teleporter teleporterTwo;


    public void InitTeleporters (int index)
    {
        teleporterOne.pairNumber = index;
        teleporterTwo.pairNumber = index;
    }

    public void DeactivateTeleporters ()
    {
        teleporterOne.isActive = false;
        teleporterTwo.isActive = false;
    }

    public void ActivateTeleporters ()
    {
        teleporterOne.isActive = true;
        teleporterTwo.isActive = true;
    }
}

