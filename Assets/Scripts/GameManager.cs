using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CharacterController charController;
    public MobController mobController;


    private void Start()
    {
      charController = GetComponent<CharacterController>();
      charController.strength = Random.Range(1, 3);
      charController.agility = Random.Range(1, 3);
      charController.stamina = Random.Range(1, 3);
    }


    private void Update()
    {
        
    }
}
