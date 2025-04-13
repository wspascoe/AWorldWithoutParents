using System;
using UnityEngine;

public class Emotions : MonoBehaviour
{
   public Action<FacialEmotions> OnTriggerEmotion;
   
   public void TriggerEmotion(FacialEmotions facialEmotion)
   {
      OnTriggerEmotion?.Invoke(facialEmotion);
   }
}
