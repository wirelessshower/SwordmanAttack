using System;
using UnityEngine;

public class TrainingDummy : MonoBehaviour
{

  private int hit = 0;

  private void OnTriggerEnter(Collider other)
  {
    if (!other.CompareTag("Sword")) return;
    hit++;  
    PlayerStats.Instance.GainExperience(1);
    Debug.Log("HIT" + hit);
  }
}
