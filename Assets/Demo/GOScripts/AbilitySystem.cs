using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySystem : MonoBehaviour
{
    public List<Ability> actions;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAction(int actionNo)
    {
        if (actionNo < actions.Count){
            animator.SetBool(actions[actionNo].tiggerName, true);
            actions[actionNo].StartAction();
        }
    }
}
