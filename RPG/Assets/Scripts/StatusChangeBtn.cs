using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusChangeBtn : MonoBehaviour {
    [SerializeField]Animator animator;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void OnClick()
    {
        Debug.Log("test");
        animator.SetBool("Open", !animator.GetBool("Open"));

    }
}
