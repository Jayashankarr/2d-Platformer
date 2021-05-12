using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private InputController input;

    public InputController Input
    {
        get {return input;}
    }
    public static GameManager Instance;
    
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}
