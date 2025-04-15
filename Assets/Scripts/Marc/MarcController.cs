using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class MarcController : MonoBehaviour
{
    [SerializeField] private float talkDistance = 3f;
    [SerializeField] CinemachineVirtualCamera marcCamera;
    [SerializeField] private DialogDisplay dialogText;
    private Transform player;
    private PlayerController playerController;
    private void Start()
    {
        marcCamera.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        
        if (distance < talkDistance)
        {
            TalkToMarc();
        }
    }

    private void TalkToMarc()
    {
        player.LookAt(transform.position);
        marcCamera.enabled = true;
        playerController.IsCameraLocked = true;
        StartCoroutine(TalkToPlayer());

    }

    private void Leave()
    {
        marcCamera.enabled = false;
        playerController.IsCameraLocked = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, talkDistance);
    }

    private IEnumerator TalkToPlayer()
    {
        talkDistance = 0;
        dialogText.Display("Player: Hello There Marc");
        yield return new WaitForSeconds(4f);
        dialogText.Display("");
        dialogText.Display("Hello There. I have seem to lost my book");
        yield return new WaitForSeconds(5f);
        dialogText.Display("");
        dialogText.Display("Player: Aww That's to bad I will help you find it. Do you know where you lost it.");
        yield return new WaitForSeconds(5f);
        dialogText.Display("");
        dialogText.Display("I think at my house but I dont want to face the bullies again can you go get it");
        yield return new WaitForSeconds(5f);
        dialogText.Display("");
        dialogText.Display("Player: I am not afraid to the bullies with enough energy I can outrun them I will get it");
        yield return new WaitForSeconds(5f);
        dialogText.ResetDisplay();
        Leave();
    }
}
