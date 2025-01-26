using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionTrigger : MonoBehaviour
{
    public UnityEvent onTriggerCollide;
    public UnityEvent onTriggerExitCollide;
    public UnityEvent onTriggerStayCollide;
    public string OtherObjectName;

    //public bool triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().gameObject.name == OtherObjectName)
        {
            onTriggerCollide.Invoke();
            //triggered = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Collider>().gameObject.name == OtherObjectName)
        {
            onTriggerExitCollide.Invoke();
            //triggered = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Collider>().gameObject.name == OtherObjectName)
        {
            onTriggerStayCollide.Invoke();
            //triggered = false;
        }
    }
}
