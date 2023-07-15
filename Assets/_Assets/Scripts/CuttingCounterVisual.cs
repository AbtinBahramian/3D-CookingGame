using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    [SerializeField] CuttingCounter cuttingCounter;
    private Animator animator;

    private const string cut = "Cut"; 

    private void Awake() {
        animator = this.GetComponent<Animator>();
    }
    private void Start() {
        cuttingCounter.OnCutting += OnCuttig;
    }

    private void OnCuttig(object sender, EventArgs e)
    {
        animator.SetTrigger(cut);
    }
}
