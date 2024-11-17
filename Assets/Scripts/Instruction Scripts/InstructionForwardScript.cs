using System;
using TMPro;
using UnityEngine;

public class InstructionForwardScript : MonoBehaviour
{
    public TextMeshProUGUI Forward;
    public TextMeshProUGUI Backwards;
    public TextMeshProUGUI Jump;
    public TextMeshProUGUI Hide;
    public GameObject backgroundPanel;

    private bool hasMovedForwards = false;
    private bool hasMovedBackwards = false;
    private bool hasJumped = false;
    private bool hasHiden = false;

    // Start is called before the first frame update
    void Start()
    {
        Forward.gameObject.SetActive(true);
        backgroundPanel.SetActive(true);
        Backwards.gameObject.SetActive(false);
        Jump.gameObject.SetActive(false);
        Hide.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player has moved forwards
        if (!hasMovedForwards && Input.GetKeyDown(KeyCode.D))
        {
            hasMovedForwards = true;
            HideInstructionsForwards();
        }

        // Check if the player has moved backwards
        if (hasMovedForwards && !hasMovedBackwards && Input.GetKeyDown(KeyCode.A))
        {
            hasMovedBackwards = true;
            HideInstructionsBackwards();
        }

        // Check if the player has jumped backwards
        if (hasMovedForwards && hasMovedBackwards && !hasJumped && Input.GetButtonDown("Jump"))
        {
            hasJumped = true;
            HideInstructionsJump();
        }

        // Check if the player has Hiden backwards
        if (hasMovedForwards && hasMovedBackwards && hasJumped && !hasHiden && Input.GetKeyDown(KeyCode.S))
        {
            hasHiden = true;
            HideInstructionsHiden();
        }
    }

    void HideInstructionsForwards()
    {
        Forward.gameObject.SetActive(false);
        Backwards.gameObject.SetActive(true);
    }

    void HideInstructionsBackwards()
    {
        Backwards.gameObject.SetActive(false);
        Jump.gameObject.SetActive(true);

    }

    void HideInstructionsJump()
    {
        Jump.gameObject.SetActive(false);
        Hide.gameObject.SetActive(true);
    }

    void HideInstructionsHiden()
    {
        Hide.gameObject.SetActive(false);
        backgroundPanel.gameObject.SetActive(false);

    }
}
