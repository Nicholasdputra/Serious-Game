using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class TutorialManager : MonoBehaviour
{
    public Milo miloScript;
    public James jamesScript;
    public DestinationScript destinationScript;

    public GameObject chasingNpc;
    public GameObject npc;
    public GameObject stationaryNpc;
    public GameObject lookingChasingNpc;
    public GameObject guardNpc;
    public GameObject sneakNpc;
    public GameObject[] npcs;
    public GameObject button;
    public bool hasCompletedThisStep;
    public int runOnce;
    public int tutorialStateIndex = 0;
    private Coroutine incrementTutorialIndexCoroutine;
    public TextMeshProUGUI tutorialText;
    public GameObject tutorialImage;
    public Sprite[] npcImages;

    [SerializeField] private string[] tutorialTexts = new string[]
    {
        "Welcome to the tutorial! In this game, you play as Milo, a samoyed service dog for your human, James.",
        "As Milo, there are a couple things you can do!",
        "To move around, you can use the arrow keys or the WASD keys.",
        "Aside from that, you can do things like bark, lick, and help guide james by dragging him along!",
        "To bark, you can press Q, which will scare other people away to help clear a path for James",
        "To guide james, you can press E and drag him to where you want him to go, you need to be close to him though",
        "James can get anxious when you get distracted or are too far away, so to calm him down and reassure him, you can lick him",
        "To lick, you can press X, which will help calm james down and reduce his anxiety",
        "Sometimes, James will accidentally drop his keys, you can help him by pressing F to pick them up",
        "Reminder that you can't finish your objective if you don't have the keys",
        "Speaking of which, if you ever need a reminder of which direction to go, you can press C to show the direction",
        "You can also move faster and run around by holding the left shift key",
        "or be sneaky to avoid certain types of people that you think might distract you by holding left ctrl",
        "That reminds me, there are different types of people in this game",
        "There are people who are determined to chase after you unless you tire them out",
        "There are people who are just walking around and won't bother you",
        "People who are busy standing around doing their own thing and won't bother you",
        "People who are walking around doing their own thing but will try to pet and distract you if they see you",
        "People who are standing by and will notice you if you get too close and try to pet and distract you",
        "As well as people who are a bit oblivious to you if you sneak around them but will otherwise try to pet and distract you",
        "Oh, when you get distracted, you'll need to press space to try to refocus yourself",
        "Finally, in order to complete the level, you just need to make sure to bring James to where he needs to go to",
        "Good luck and have fun! Reminder to be nice to service animals and let them focus on helping their humans!"
    };
    //0 = chasing npc
    //1 = npc
    //2 = stationary
    //3 = looking chasing
    //4 = guard
    //5 = sneak

    // Start is called before the first frame update
    void Start()
    {
        // Time.timeScale = 0;
        tutorialStateIndex = 0;
        miloScript = GameObject.FindWithTag("Milo").GetComponent<Milo>();
        jamesScript = GameObject.FindWithTag("James").GetComponent<James>();
        destinationScript = GameObject.FindWithTag("LevelTarget").GetComponent<DestinationScript>();
        DestinationScript.isGameOver = false;
        npcs = GameObject.FindGameObjectsWithTag("NPC");
        foreach(GameObject npc in npcs){
            npc.SetActive(false);
        }
        miloScript.canMove = false;
        tutorialImage.SetActive(false);
        destinationScript.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(tutorialStateIndex == 0){
            miloScript.canMove = false;
            jamesScript.hasKeys = false;
        }
        // else if (tutorialStateIndex == 1){
        //     miloScript.canMove = false;
        //     jamesScript.hasKeys = false;
        // } 
        else if (tutorialStateIndex == 2){
            miloScript.canMove = true;
            button.SetActive(false);
            if(Input.GetKeyDown(KeyCode.W) 
            || Input.GetKeyDown(KeyCode.A) 
            || Input.GetKeyDown(KeyCode.S) 
            || Input.GetKeyDown(KeyCode.D) 
            || Input.GetKeyDown(KeyCode.UpArrow) 
            || Input.GetKeyDown(KeyCode.LeftArrow) 
            || Input.GetKeyDown(KeyCode.DownArrow) 
            || Input.GetKeyDown(KeyCode.RightArrow)){
                if(incrementTutorialIndexCoroutine == null){
                    incrementTutorialIndexCoroutine = StartCoroutine(DelayedIncrementTutorialIndex(2f));
                }
            }
        } 
        else if (tutorialStateIndex == 3){
            miloScript.rb.velocity = new Vector2(0, 0);
            button.SetActive(true);
            miloScript.canMove = false;
        } 
        else if (tutorialStateIndex == 4){
            incrementTutorialIndexCoroutine = null;
            button.SetActive(false);
            npc.SetActive(true);
            if(Input.GetKeyDown(KeyCode.Q)){
                if(incrementTutorialIndexCoroutine == null){
                    incrementTutorialIndexCoroutine = StartCoroutine(DelayedIncrementTutorialIndex(2f));
                }
            }
            miloScript.speed = miloScript.defaultSpeed;
        } 
        else if (tutorialStateIndex == 5){
            npc.SetActive(false);
            miloScript.canMove = true;
            if(Input.GetKeyDown(KeyCode.E)){
                if(incrementTutorialIndexCoroutine == null){
                    incrementTutorialIndexCoroutine = StartCoroutine(DelayedIncrementTutorialIndex(2f));
                }
            }
        }
        else if (tutorialStateIndex == 6){
            miloScript.rb.velocity = new Vector2(0, 0);
            incrementTutorialIndexCoroutine = null;
            miloScript.canMove = false;
            button.SetActive(true);
        } 
        else if (tutorialStateIndex == 7){
            button.SetActive(false);    
            if(Input.GetKeyDown(KeyCode.X)){
                if(incrementTutorialIndexCoroutine == null){
                    incrementTutorialIndexCoroutine = StartCoroutine(DelayedIncrementTutorialIndex(2f));
                    jamesScript.hasKeys = true;
                }
            }
        } 
        else if (tutorialStateIndex == 8){
            if(runOnce == 0){
                runOnce++;
                incrementTutorialIndexCoroutine = null;
                miloScript.canMove = true;
            }
            if(jamesScript.hasKeys && !hasCompletedThisStep){
                jamesScript.hasKeys = false;
                jamesScript.DropKeys();
            }
            if(Input.GetKeyDown(KeyCode.F)){
                if(incrementTutorialIndexCoroutine == null){
                    jamesScript.hasKeys = false;
                    hasCompletedThisStep = true;
                    incrementTutorialIndexCoroutine = StartCoroutine(DelayedIncrementTutorialIndex(2f));
                    // jamesScript.hasKeys = false;
                }
            }
        } 
        else if (tutorialStateIndex == 9){
            button.SetActive(true);
            miloScript.rb.velocity = new Vector2(0, 0);
            miloScript.canMove = false;
            jamesScript.hasKeys = false;
        } 
        else if (tutorialStateIndex == 10){
            // miloScript.canMove = true;
            button.SetActive(false);
            if(Input.GetKeyDown(KeyCode.C)){
                if(incrementTutorialIndexCoroutine == null){
                    hasCompletedThisStep = true;
                    incrementTutorialIndexCoroutine = StartCoroutine(DelayedIncrementTutorialIndex(2f));
                }
            }
        } 
        else if (tutorialStateIndex == 11){
            miloScript.canMove = true;
            if(Input.GetKeyDown(KeyCode.LeftShift)){
                if(incrementTutorialIndexCoroutine == null){
                    hasCompletedThisStep = true;
                    incrementTutorialIndexCoroutine = StartCoroutine(DelayedIncrementTutorialIndex(2f));
                }
            }
        }
        else if (tutorialStateIndex == 12){
            if(Input.GetKeyDown(KeyCode.LeftControl)){
                if(incrementTutorialIndexCoroutine == null){
                    hasCompletedThisStep = true;
                    incrementTutorialIndexCoroutine = StartCoroutine(DelayedIncrementTutorialIndex(2f));
                }
            }
        }
        else if (tutorialStateIndex == 13){
            miloScript.canMove = false;
            jamesScript.hasKeys = false;
            button.SetActive(true);
        } 
        else if (tutorialStateIndex == 14){
            tutorialImage.SetActive(true);
            tutorialImage.GetComponent<Image>().sprite = npcImages[0];
        } 
        else if (tutorialStateIndex == 15){
            tutorialImage.GetComponent<Image>().sprite = npcImages[1];
        } 
        else if (tutorialStateIndex == 16){
            tutorialImage.GetComponent<Image>().sprite = npcImages[2];
        } 
        else if (tutorialStateIndex == 17){
            tutorialImage.GetComponent<Image>().sprite = npcImages[3];            
        } 
        else if (tutorialStateIndex == 18){
            tutorialImage.GetComponent<Image>().sprite = npcImages[4];
        } 
        else if (tutorialStateIndex == 19){
            tutorialImage.GetComponent<Image>().sprite = npcImages[5];
        } 
        else if (tutorialStateIndex == 20){
            tutorialImage.SetActive(false);
            button.SetActive(false);
            
            if(guardNpc.activeSelf == false){
                guardNpc.SetActive(true);
                guardNpc.GetComponent<GuardNPC>().canUpdate = true;
                guardNpc.GetComponent<GuardNPC>().canMove = true;
                guardNpc.GetComponent<GuardNPC>().target = miloScript.gameObject;
            }
            
            if(guardNpc.activeSelf && !guardNpc.GetComponent<GuardNPC>().canMove && !QuickTime.isQuickTimeActive == false){
                if(incrementTutorialIndexCoroutine == null){
                    incrementTutorialIndexCoroutine = StartCoroutine(DelayedIncrementTutorialIndex(2f));
                }
            }
            // miloScript.canMove = true;
        } 
        else if (tutorialStateIndex == 21){
            // miloScript.canMove = false;
            
        } 
        else if (tutorialStateIndex == 22){
            // miloScript.canMove = true;
        } 
        else{
            // miloScript.canMove = false;
        }
    }

    public IEnumerator DelayedIncrementTutorialIndex(float delay){
        yield return new WaitForSeconds(delay);
        IncrementTutorialIndex();
        Debug.Log("Current velocity: " + miloScript.rb.velocity);
        miloScript.rb.velocity = new Vector2(0, 0);
        Debug.Log("Velocity after setting to zero: " + miloScript.rb.velocity);
        miloScript.transform.position = new Vector3(-9,-4, 0);
        jamesScript.transform.position = new Vector3(-7,-4, 0);
        runOnce = 0;
        incrementTutorialIndexCoroutine = null;
    }

    public void IncrementTutorialIndex(){
        tutorialStateIndex++;
        tutorialText.text = string.Empty;
        tutorialText.text = "";
        tutorialText.text = tutorialTexts[tutorialStateIndex];
        runOnce = 0;
        hasCompletedThisStep = false;
        Debug.Log("Tutorial State Index: " + tutorialStateIndex);
        Debug.Log("Tutorial Text: " + tutorialTexts[tutorialStateIndex]);

    }
}
