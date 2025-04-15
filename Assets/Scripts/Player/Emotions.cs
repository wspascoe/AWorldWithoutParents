using System;
using System.Collections;
using UnityEngine;

public class Emotions : MonoBehaviour
{
   public Action<FacialEmotions> OnTriggerEmotion;
   
   public IEnumerator TriggerEmotion(FacialEmotions facialEmotion)
   {
      OnTriggerEmotion?.Invoke(facialEmotion);
      yield return new WaitForSeconds(5f);
      OnTriggerEmotion?.Invoke(FacialEmotions.None);
   }
}
