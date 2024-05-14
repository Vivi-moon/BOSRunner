using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class barrierControl : MonoBehaviour
{
    //public RoadGenerator _roadGenerator;
    // Start is called before the first frame update
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            transform.gameObject.SetActive(false);
            //other.gameObject.GetComponent<PlayerController>().ResetLevel();
        }
    }
}
