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
    [SerializeField] private Transform topCoil;

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

    private void InitialiseField()
    {
        // Organise elec field length and which objects should be enabled.
        int end = 0;
        electricFields = GetComponentsInChildren<Animator>(true);
        for (int i = 1; i < electricFields.Length - 1; i++)
        {
            if (i < size)
            {
                electricFields[i].gameObject.SetActive(true);
                electricFields[i].transform.localPosition = Vector3.up * (i - 1);
            }
            else    // Disable all unused electric fields
            {
                if (end == 0)
                    end = i - 1;
                electricFields[i].gameObject.SetActive(false);
            }
        }
        // Place top cap
        if (end == 0)
            end = electricFields.Length - 2;

        electricFields[electricFields.Length - 1].gameObject.SetActive(true);
        electricFields[electricFields.Length - 1].transform.localPosition = Vector3.up * end;
        topCoil.localPosition = electricFields[electricFields.Length - 1].transform.localPosition;

        electricFields = GetComponentsInChildren<Animator>(false);
    }

    private void CalculateHitbox()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(0.5f, size);
        boxCollider2D.offset = new Vector2(0, (size - 1) / 2);
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
    }
}
