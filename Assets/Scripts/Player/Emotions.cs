using System;
using UnityEngine;

public class Emotions : MonoBehaviour
{
   public Action<FacialEmotions> OnTriggerEmotion;

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Alpha1))
      {
         OnTriggerEmotion?.Invoke(FacialEmotions.Surprise);
      }
      if (Input.GetKeyDown(KeyCode.Alpha2))
      {
         OnTriggerEmotion?.Invoke(FacialEmotions.Crying);
      }
      if (Input.GetKeyDown(KeyCode.Alpha3))
      {
         OnTriggerEmotion?.Invoke(FacialEmotions.Disgust);
      }
      if (Input.GetKeyDown(KeyCode.Alpha4))
      {
         OnTriggerEmotion?.Invoke(FacialEmotions.None);
      }
   }
}
