using System;
using UnityEngine;

public enum FacialEmotions
{
    None,
    Crying,
    Disgust,
    Surprise
}
public class FacialExpressions : MonoBehaviour
{
    [SerializeField] private GameObject face;
    [SerializeField] private Material cryMaterial;
    [SerializeField] private Material disgustMaterial;
    [SerializeField] private Material surprisedMaterial;
    [SerializeField] private Material faceMaterial;

    Emotions emotions;

    private void Start()
    {
        emotions = GameObject.FindGameObjectWithTag("Player").GetComponent<Emotions>();
        emotions.OnTriggerEmotion += SetFacialExpressions;
    }

    public void SetFacialExpressions(FacialEmotions facialEmotions)
    {
        switch (facialEmotions)
        {
            case FacialEmotions.None:
                face.GetComponent<MeshRenderer>().material = faceMaterial;
                break;
            case FacialEmotions.Crying:
                face.GetComponent<MeshRenderer>().material = cryMaterial;
                break;
            case FacialEmotions.Disgust:
                face.GetComponent<MeshRenderer>().material = disgustMaterial;
                break;
            case FacialEmotions.Surprise:
                face.GetComponent<MeshRenderer>().material = surprisedMaterial;
                break;
            default:
                break;
        }
        
    }
}
