using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Running : MonoBehaviour
{
    public RoadGenerator RoadGenerator;
    //[SerializeField] private Animation animation;
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (RoadGenerator.GameActive)
        {
            
            animator.SetBool("IsTrue", true);
        }
        else {
            animator.SetBool("IsTrue", false);
        }    
    }
}
