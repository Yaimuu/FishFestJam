using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public Text nameText; // name storage to show player
    public Text dialogueText; // dialogue storage to show player

    public Animator animator; // animator reference

    private Queue<string> sentences; // storage for dialogue sentences

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>(); // define sentences
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true); // start diag box animator open
        // Debug.Log("Starting conversation with " + dialogue.name); // debug test functionality

        nameText.text = dialogue.name;

        sentences.Clear(); // clear old sentences

        // Go through sentence array and queue them up
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    // Show player next sentence
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines(); 
        StartCoroutine(TypeSentence(sentence));
        // dialogueText.text = sentence;
        // Debug.Log(sentence);
    }

    // display sentences by loading a letter at a time
    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    // end convo
    void EndDialogue()
    {
        animator.SetBool("IsOpen", false); // end diag box animator close
        // Debug.Log("End of conversation.");
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
