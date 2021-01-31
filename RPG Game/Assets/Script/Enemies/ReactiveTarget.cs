using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReactToHit()
    {
        WanderingAI wanderingAI = GetComponent<WanderingAI>();
        if (wanderingAI != null)
        {
            wanderingAI.SetAlive(false);
        }
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        transform.Rotate(-75, 0, 0);

        yield return new WaitForSeconds(1.5f);
        //Debug.Log(this);
        Destroy(this.gameObject);
    }
}
