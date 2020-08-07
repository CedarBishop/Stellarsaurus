using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricField : DeathBarrier
{
    [Range(1, 10)] public int size;

    public float onTime;
    public float offTime;
    private float timer;
    private bool on = true;

    private Animator[] electricFields;
    private Transform topCoil;

    private BoxCollider2D boxCollider2D;
    
    void Start()
    {
        InitialiseField();
        CalculateHitbox();
        InitialiseTimer();
    }

    private void Update()
    {
        ElectricFieldManager();
    }

    //public void InitialiseField()   // Organise elec field length and which objects should be enabled.
    //{
    //    electricFields = GetComponentsInChildren<Animator>(true);

    //    // Grab reference to top cap sprite
    //    topCoil = electricFields[electricFields.Length - 1].transform;
    //    for (int i = 1; i < electricFields.Length; i++)
    //    {
    //        if (i < size)   // Enable electric fields up till 'size'
    //        {
    //            electricFields[i].gameObject.SetActive(true);
    //            electricFields[i].transform.localPosition = Vector3.up * i;
    //            Debug.Log("enabling " + electricFields[i].name);
    //        }
    //        else    // Disable all unused electric fields
    //        {
    //            electricFields[i].gameObject.SetActive(false);
    //        }
    //    }
    //    // Place top cap
    //    electricFields[electricFields.Length - 1].gameObject.SetActive(true);
    //    electricFields[electricFields.Length - 1].transform.localPosition = Vector3.up * size;

    //    Debug.Log("placing " + electricFields[electricFields.Length - 1].name + " at " + electricFields[electricFields.Length - 1].transform.localPosition);
    //    topCoil.localPosition = electricFields[electricFields.Length - 1].transform.localPosition;
    //}

    public void InitialiseField()   // Organise elec field length and which objects should be enabled.
    {      
        electricFields = GetComponentsInChildren<Animator>(true);

        // Grab reference to top cap sprite
        topCoil = transform.GetChild(transform.childCount - 1).transform;
        for (int i = 1; i < electricFields.Length; i++)
        {
            if (i + 1 < size)   // Enable electric fields up till 'size'
            {
                electricFields[i].gameObject.SetActive(true);
                electricFields[i].transform.localPosition = Vector3.up * i;
            }
            else    // Disable all unused electric fields
            {
                electricFields[i].gameObject.SetActive(false);
            }
        }
        // Place top cap
        electricFields[electricFields.Length - 1].gameObject.SetActive(true);
        electricFields[electricFields.Length - 1].transform.localPosition = Vector3.up * (size - 1);
        
        topCoil.localPosition = electricFields[electricFields.Length - 1].transform.localPosition;

        if (size == 1)  // Final check to see if the size is one, in which case disable the animated cap (otherwise electricity will double up and look crowded)
        {
            electricFields[electricFields.Length - 1].gameObject.SetActive(false);
        }

        // Reset electric field array to toggle only active objects
        electricFields = transform.GetComponentsInChildren<Animator>(false);
    }

    public void CalculateHitbox()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.offset = new Vector2(0, (float)(size - 1) / 2);
        boxCollider2D.size = new Vector2(0.5f, size - 0.5f);
    }

    private void InitialiseTimer()
    {
        timer = 0;
    }

    private void ElectricFieldManager()
    {
        if (on)
        {
            if (Time.time >= timer)
            {
                on = false;
                timer += offTime;
                ToggleFields();
            }
        }
        else
        {
            if (Time.time >= timer)
            {
                on = true;
                timer += onTime;
                ToggleFields();
            }
        }
    }

    private void ToggleFields()
    {
        foreach (Animator i in electricFields)
        {
            i.gameObject.SetActive(!i.gameObject.activeSelf);
        }
        boxCollider2D.enabled = !boxCollider2D.enabled;

        if (boxCollider2D.enabled)
        {
            if (SoundManager.instance != null)
                SoundManager.instance.PlayExclusiveSFX("SFX_ElectricFieldAmbient");

            print("Called play exclusive");
        }
    }
}
