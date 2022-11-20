using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollision : MonoBehaviour
{
    public TutorialManager ttM;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Dokkebi")
        {
            TutorialManager.collisionDetected++;
            Arrkkebi.Target = null;
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Dokkebi"))
        {
            StartCoroutine(TypingSet());
        }
    }
    IEnumerator TypingSet()
    {
        ttM.AllSetFalse();
        int number = Random.Range(-3, 0);
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[number], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        ttM.ReturnAll();
    }
}
