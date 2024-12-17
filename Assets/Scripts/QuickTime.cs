using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuickTime : MonoBehaviour
{
    public NPC qteNPC;
    public static bool isQuickTimeActive = false;
    [SerializeField] GameObject quickTimePanel;
    Slider quickTimeSlider;
    [SerializeField] float quickTimeSliderSpeed = 0.033f;
    [SerializeField] int quickTimeIncrement = 5;

    // Start is called before the first frame update
    void Start()
    {
        quickTimePanel.SetActive(false);
        Time.timeScale = 1;
        quickTimeSlider = quickTimePanel.transform.GetChild(0).GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(DestinationScript.instance.isGameOver)
        {
            quickTimePanel.SetActive(false);
            isQuickTimeActive = false;
        }

        if(quickTimePanel.activeSelf){
            if(Input.GetKeyDown(KeyCode.Space))
            {
                quickTimeSlider.value += quickTimeIncrement;
            }
            if(quickTimeSlider.value >= 100)
            {
                EndQuickTimeEvent();
            }
        }
    }

    public void StartQuickTimeEvent()
    {
        DestinationScript.instance.distractedCounter++;
        Time.timeScale = 0;
        isQuickTimeActive = true;
        quickTimeSlider.value = 10;
        quickTimePanel.SetActive(true);
        StartCoroutine(QuickTimeEvent());
    }

    IEnumerator QuickTimeEvent()
    {
        while(true){
            yield return new WaitForSecondsRealtime(quickTimeSliderSpeed);
            quickTimeSlider.value--;
            quickTimeSlider.value = Mathf.Clamp(quickTimeSlider.value, 0, 100);
            yield return null;
        }
    }

    void EndQuickTimeEvent()
    {
        //resume time
        Time.timeScale = 1;
        isQuickTimeActive = false;
        quickTimePanel.SetActive(false);
        if(qteNPC == null){
            Debug.Log("qteNPC is null, called from QuickTime");
        }
        qteNPC.canMove = true;
        GuardNPC guardScript = qteNPC.gameObject.GetComponent<GuardNPC>();
        ChasingNPC chasingScript = qteNPC.gameObject.GetComponent<ChasingNPC>();
        ChaseIfLookingNPC chaseIfLookingScript = qteNPC.gameObject.GetComponent<ChaseIfLookingNPC>();
        HaveToSneakNPC haveToSneakScript = qteNPC.gameObject.GetComponent<HaveToSneakNPC>();
        // Debug.Log("qteNPC.setDestination.Length = " + qteNPC.setDestination.Length);
        if(chasingScript != null){
            Debug.Log("ChasingNPC was the one that triggered the QuickTime event");
            // NPCSpawner.instance.totalChasingNPCs--;
            qteNPC.target = qteNPC.waypointsToGoTo[0];
        } else if (guardScript != null){
            Debug.Log("GuardNPC was the one that triggered the QuickTime event");
            qteNPC.target = guardScript.Anchor;
            StartCoroutine(TargetMiloCooldown(guardScript));
        } else if (chaseIfLookingScript != null){
            Debug.Log("ChaseIfLookingNPC was the one that triggered the QuickTime event");
            qteNPC.target = qteNPC.waypointsToGoTo[0];
            StartCoroutine(TargetMiloCooldown(chaseIfLookingScript));
        } else if (haveToSneakScript != null){
            Debug.Log("HaveToSneakNPC was the one that triggered the QuickTime event");
            qteNPC.target = haveToSneakScript.Anchor;
            StartCoroutine(TargetMiloCooldown(haveToSneakScript));
        }
    }

    public IEnumerator TargetMiloCooldown(GuardNPC guardScript)
    {
        guardScript.canMove = false;
        yield return new WaitForSeconds(5);
        guardScript.canMove = true;
    }

    public IEnumerator TargetMiloCooldown(ChaseIfLookingNPC chaseIfLookingScript)
    {
        chaseIfLookingScript.canMove = false;
        yield return new WaitForSeconds(5);
        chaseIfLookingScript.canMove = true;
    }
}
