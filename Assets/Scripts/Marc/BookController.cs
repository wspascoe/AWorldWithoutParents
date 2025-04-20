using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BookController : MonoBehaviour
{
    [SerializeField] Book book;
    [SerializeField] private DialogDisplay dialogText;
    private PlayerController player;

    private void Start()
    {
        book.OnPickup += PickupBook;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        
    }
        

    void PickupBook()
    {
        dialogText.Display("Player: We got the book. Marc will be so happy.");
        player.OnWin();
    }
}
