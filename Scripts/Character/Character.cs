using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Character class. This is the base class for all 'characters', moveable creatures or people within the game.
/// It has currently drifted into the player class in terms of functionality, considering that there likely won't be any other characters, this may not be a bad thing.
/// </summary>

[RequireComponent(typeof(DeathManager))]
[RequireComponent(typeof(HidePlayer))]
[RequireComponent(typeof(Timer))]
//[RequireComponent(typeof(Cinemachine.CinemachineVirtualCamera))]

public class Character : MonoBehaviour
{
    //Should add some default values here and look at removing UI elements to their own scripts.

    [SerializeField]
    [Tooltip("First arm")]
    private ArmMovement armOne;

    [SerializeField]
    [Tooltip("Second arm")]
    private ArmMovement armTwo;
      
    [SerializeField]
    [Tooltip("Player's health. Currently should always be 1.")]
    private int health;

    [SerializeField]
    [Tooltip("Switch between movement systems. True = B, False = C")]
    private bool usingMovementB = true;

    [SerializeField]
    [Tooltip("Victory text for clam total")]
    private Text clamTotal;

    //Should these really be in character and not on a UI script?

    //The sprite for a collected clam
    public Sprite foundClamSprite;

    public List<Image> clamUiImages = new List<Image>();

    //The UI image components for the clam UI
    //public Image clamStatus1;
    //public Image clamStatus2;
    //public Image clamStatus3;

   // [SerializeField]
    //[Tooltip("The sound effect that plays when a clam is collected.")]
    //private AudioSource clamPickupSound;

    //Contains info about medal times so the character can check which medals are won.
    private SceneData sceneData;

    //Parent object
    private GameObject rootObject;

    //The cinemachine camera following the player
    private Cinemachine.CinemachineVirtualCamera followCamera;

    //Last activated checkpoint
    private GameObject currentCheckpoint;

    //Handles death models
    private DeathManager deathManager;

    //The type of death the octopus has currently experienced. Used to set the correct death sequence.
    private EnumDefinitions.DeathTypes currentDeathType;

    //Tracks if the character is dead or not.
    private bool isDead;

    //Script to hide the player's render components.
    private HidePlayer hidePlayer;

    //Checkpoint manager that handles respawning positions
    private CheckpointManager checkpointManager;

    //Calling Audiomanager
    private AudioManager audioManager;

    private bool audioPlayed;

    //Timer class to track time taken to complete each level. 
    //Currently also does other stuff to do with medals but working on refactoring.
    private Timer timer;

    //Number of clams collected. Used by timer to determine which medal to award. //Why is this static?
    private static int numberFoundClams = 0;

    //The armRadius renderer. Used to enable and disable the radius.
    private SpriteRenderer armRadius;

    private bool isCollidedWithPiston = false;
    private bool isCollidedWithSurface = false;

    private AudioSource checkpointSound;

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    public static int NumberFoundClams
    {
        get
        {
            return numberFoundClams;
        }

        set
        {
            numberFoundClams = value;
        }
    }

    IEnumerator waitForDeath;


    // Use this for initialization
    void Start()
    {

        //Get Audiomanager
        try
        {
            audioManager = FindObjectOfType<AudioManager>();
        }

        catch
        {
            Debug.LogWarning("Could not find audio Manager");
        }

        audioPlayed = false;

        //Init pickups
        NumberFoundClams = 0;

        clamUiImages.Add(GameObject.Find("ClamUI1").GetComponent<Image>());
        clamUiImages.Add(GameObject.Find("ClamUI2").GetComponent<Image>());
        clamUiImages.Add(GameObject.Find("ClamUI3").GetComponent<Image>());

        //Get parent object
        rootObject = gameObject.transform.parent.gameObject;

        //Get components -> should have null checks. RequireComponent could be good.
        deathManager = gameObject.GetComponent<DeathManager>();
        hidePlayer = gameObject.GetComponent<HidePlayer>();
        followCamera = rootObject.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();
        timer = gameObject.GetComponent<Timer>();

        armRadius = gameObject.GetComponentInChildren<SpriteRenderer>();

        //Find the SceneData object in the scene and store it's data script.
        //There may well be a nicer way of doing this.
        try
        {
            sceneData = GameObject.Find("SceneData").GetComponent<SceneData>();
        }

        catch (System.NullReferenceException)
        {
            Debug.LogError("No SceneData component in scene. Add a SceneData prefab! \nAlternatively you may have renamed it to something other than SceneData.");
        }        
        
        //Get the checkpoint manager from the parent transform. Could put the checkpoint manager just on the octo itself possibly.
        checkpointManager = gameObject.GetComponentInParent<CheckpointManager>();

    }

    // Update is called once per frame
    void Update()
    {
        //Escape key to return to main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main Menu");
        }

        //Number keys to skip to levels for testing purposes. Could add Debug.isDebugBuild to the check so it only works in editor/debug build?
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("ProtoLevel1");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Speed Level");
        }

        //R key for restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        //Swap between movement systems
        if (Input.GetKeyDown(KeyCode.Q))
        {
            usingMovementB = !usingMovementB;
            armOne.UseMovementB = usingMovementB;
            armTwo.UseMovementB = usingMovementB;
        }

        
        //If still alive and health is 0, die.
        if (health <= 0 && isDead != true)
        {
            Die();
        }

        if (Input.GetKeyDown("."))
        {
            EndLevel();
        }
    }
    
    //Change the character's health
    public void ChangeHealth(int healthChange)
    {
        health += healthChange;
    }

    /// <summary>
    /// Handles the death of the character. Starts the death sequence defined in DeathManager and respawns the player.
    /// </summary>
    void Die()
    {
        Debug.Log("Die!");
        isDead = true;       

        //Pause user input for arms while dead.
        armOne.Paused = true;
        armTwo.Paused = true;

        //Stop camera from following dead octo chunks
        followCamera.Follow = null;

        //Hide the player model
        hidePlayer.EnableRenderers(false);

        armRadius.enabled = false;

        //Start death sequence
        deathManager.Die(currentDeathType);

        //Start post-death coroutine
        waitForDeath = WaitForDeath(deathManager.RespawnTime);
        StartCoroutine(waitForDeath);

    }

    /// <summary>
    /// Waits for an amount of time to account for death sequence before respawning character.
    /// </summary>
    /// <param name="time">How long to wait before respawning.</param>
    /// <returns></returns>
    IEnumerator WaitForDeath(float time)
    {
        yield return new WaitForSeconds(time);

        //Respawn character.
        Respawn();

        //Unpause user input for arms.
        armOne.Paused = false;
        armTwo.Paused = false;

        //Stop hiding the character.
        hidePlayer.EnableRenderers(true);

        //Arm radius is unhidden.
        armRadius.enabled = true;

        //Reset health
        health = 1; //This should be a constant somehwere not just a magic number!! <----!!!
        isDead = false;
    }

    /// <summary>
    /// Resapwn the character. 
    /// Restarts the level if there are no available checkpoints or no checkpoint manager is found.
    /// </summary>
    void Respawn()
    {
        //Camera resumes following the player
        followCamera.Follow = this.transform;
        
        //If unable to resapwn at a checkpoint, restart the scene.
        if (checkpointManager == null)
        {
            Debug.LogError("CheckpointManager is null! Make sure a CheckpointManager is assigned to the character script.");
            NumberFoundClams = 0;
            Restart();
        }

        else if (currentCheckpoint == null)
        {
            Debug.LogWarning("CurrentCheckpoint is null, defaulting to level restart. If this is not desired, add a checkpoint to the level.");
            NumberFoundClams = 0;
            Restart();
        }

        //If able to, restart from checkpoint.
        else
        {
            checkpointManager.Restart(currentCheckpoint.transform.position);
            ArmReset();
        }
    }

    // Restart the level
    public void Restart()
    {
        //Restarts the level. 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Resets the arms back to the main body
    void ArmReset()
    {
        armOne.Fired = false;
        armOne.Grabbed = false;
        armOne.transform.position = armOne.ArmHolder.position;
        armTwo.Fired = false;
        armTwo.Grabbed = false;
        armTwo.transform.position = armTwo.ArmHolder.position;
    }

    void CheckTimeMedal()
    {
        //Check if player has earned a gold time medal.
        if (timer.TimeScore <= sceneData.GoldTime & (timer.IncreaseTime == false))
        {
            //Store the player's medal data.  Is this ever used?
            PlayerMedals.Time = true;

            //Update the victory screen and main menu with new medals.
            timer.VictoryMenu.SetImage(timer.VictoryMenu.TimeMedal, timer.VictoryMenu.GoldMedalSprite);

            //If levelData exists (created in the mainMenu) save the game
            if (LevelData.levelData != null)
            {
                //Set gold time medal to achieved.
                LevelData.levelData.myLevels[timer.Level].goldTime = true;
            }

            else
            {
                Debug.LogWarning("Unable access LevelData, no such file exists. This won't occur when loading levels through the mainMenu.");
            }
        }

        else if (timer.TimeScore <= sceneData.SilverTime & (timer.IncreaseTime == false))
        {
            //Store the player's medal data.  Is this ever used?
            PlayerMedals.Time = true;

            //Update the victory screen and main menu with new medals.
            timer.VictoryMenu.SetImage(timer.VictoryMenu.TimeMedal, timer.VictoryMenu.SilverMedalSprite);

            //If levelData exists (created in the mainMenu) save the game
            if (LevelData.levelData != null)
            {
                //Set gold time medal to achieved.
                LevelData.levelData.myLevels[timer.Level].silverTime = true;
            }

            else
            {
                Debug.LogWarning("Unable access LevelData, no such file exists. This won't occur when loading levels through the mainMenu.");
            }
        }

        else if (timer.TimeScore <= sceneData.BronzeTime & (timer.IncreaseTime == false))
        {
            //Store the player's medal data.  Is this ever used?
            PlayerMedals.Time = true;

            //Update the victory screen and main menu with new medals.
            timer.VictoryMenu.SetImage(timer.VictoryMenu.TimeMedal, timer.VictoryMenu.BronzeMedalSprite);

            //If levelData exists (created in the mainMenu) save the game
            if (LevelData.levelData != null)
            {
                //Set gold time medal to achieved.
                LevelData.levelData.myLevels[timer.Level].bronzeTime = true;
            }

            else
            {
                Debug.LogWarning("Unable access LevelData, no such file exists. This won't occur when loading levels through the mainMenu.");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HazardCheck(collision.collider);

        if (collision.collider.tag == "Breakable")

        {
            Destroy(GameObject.Find("BreakableWall1"));

        }

        if (collision.collider.tag == "Climbable")
        {
            isCollidedWithSurface = true;
        }
    }

    /// <summary>
    /// Reset surface collision flag
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Climbable")
        {
            isCollidedWithSurface = false;
        }
    }

    /// <summary>
    /// Checks if the octo is still on a surface, prevents bugs where the octo is registered as not touching the floor.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Climbable")
        {
            isCollidedWithSurface = true;
        }
    }

    /// <summary>
    /// Checks for entry into triggers, including clams, checkpoints, exit doors and pistons.
    /// </summary>
    /// <param name="trigger"></param>
    private void OnTriggerEnter(Collider trigger)
    {
        HazardCheck(trigger);

        //Check for picking up clam.
        if (trigger.gameObject.tag == "Pickup")
        {
            FoundClam(trigger.gameObject);
        }

        //If collider is a checkpoint set new checkpoint location.
        else if ((trigger.gameObject.tag == "Checkpoint") && (isDead == false))
        {
            Debug.Log("Hit checkpoint");
            currentCheckpoint = trigger.gameObject;

            if (audioManager != null && audioPlayed == false)
            {
                //Play checkpoint activation sound
                audioManager.Play("Checkpoint");
                audioPlayed = true;
            }


            //Get the checkpoint light script from the parent of trigger and activate light.
            trigger.gameObject.transform.parent.GetComponent<Checkpoint>().LightIsActivated(true);
        }

        //If the player reaches the end of the level
        else if ((trigger.gameObject.name == "Exit Door") && (isDead == false))
        {
            EndLevel();
        }

        //Check for collision with a piston.
        else if (trigger.gameObject.tag == "Piston")
        {
            isCollidedWithPiston = true;

            //If currently collided with both the piston and a surface, squishy time.
            if (isCollidedWithPiston == true && isCollidedWithSurface == true)
            {
                //Change character's health by the damage of the hazard, note the minus operator to remove the health.
                ChangeHealth(-10);
                currentDeathType = EnumDefinitions.DeathTypes.Explode;
            }
        }
    }

    /// <summary>
    /// Reset piston collision flag
    /// </summary>
    /// <param name="trigger"></param>
    private void OnTriggerExit(Collider trigger)
    {
        if (trigger.gameObject.tag == "Piston")
        {
            isCollidedWithPiston = false;
        }
    }

    /// <summary>
    /// Called when the script is first loaded and then called whenever the script is changed in the editor. 
    /// </summary>
    void OnValidate()
    {
        armOne.UseMovementB = usingMovementB;
        armTwo.UseMovementB = usingMovementB;
    }

    void EndLevel()
    {            
        //Stop tracking time score
        timer.IncreaseTime = false;

        //Show victory screen
        timer.VictoryCanvas.enabled = true;

        //Update clam medals
        timer.UpdateClamMedals();

        //Set the clam text on the vicotry screen
        timer.VictoryCanvas.GetComponent<VictoryMenu>().ClamCountText.text = "Clams: " + NumberFoundClams.ToString();

        //Disable HUD
        timer.UiCanvas.enabled = false;

        //Pause the game
        Time.timeScale = 0.0f;

        //Disable player's arms <- this should be done with the references to arms not .find!
        GameObject.Find("Arm 1").GetComponent<ArmMovement>().enabled = false;
        GameObject.Find("Arm 2").GetComponent<ArmMovement>().enabled = false;

        if (LevelData.levelData != null)
        {
            //Unlock the next level
            LevelData.levelData.myLevels[(timer.Level + 1)].unlocked = true;
        }

        else
        {
            Debug.LogWarning("Unable unlock levels, no LevelData file exists. This won't occur when loading levels through the mainMenu.");
        }

        //Check which time medals were won.
        CheckTimeMedal();
    }

    /// <summary>
    /// Check for collision with hazards and update death type as necessary.
    /// </summary>
    /// <param name="collider"></param>
    public void HazardCheck(Collider collider)
    {
        if (collider.tag == "Hazard")
        {
            // if the hazard is active
            if ((collider.GetComponent<Hazard>()) != null && (collider.GetComponent<Hazard>().IsActive))
            {
                //Change character's health by the damage of the hazard, note the minus operator to remove the health.
                ChangeHealth(-collider.GetComponent<Hazard>().Damage);
                currentDeathType = EnumDefinitions.DeathTypes.Default;
                if (audioManager != null)
                {
                    audioManager.Play("HazardDeath");
                }
            }
        }

        else if ((collider.GetComponent<Hazard>()) != null && (collider.tag == "DissolveHazard"))
        {
            // if the hazard is active
            if (collider.GetComponent<Hazard>().IsActive)
            {
                //Change character's health by the damage of the hazard, note the minus operator to remove the health.
                ChangeHealth(-collider.GetComponent<Hazard>().Damage);

                //Set death type.
                currentDeathType = EnumDefinitions.DeathTypes.Dissolve;

                //Pass the deathManager the hazard's collider for use in emitter trigger effects.
                deathManager.HazardCollider = collider;
            }
        }

        else if ((collider.GetComponent<Hazard>()) != null && (collider.tag == "BurnHazard"))
        {
            // if the hazard is active
            if (collider.GetComponent<Hazard>().IsActive)
            {
                //Change character's health by the damage of the hazard, note the minus operator to remove the health.
                ChangeHealth(-collider.GetComponent<Hazard>().Damage);

                //Set death type.
                currentDeathType = EnumDefinitions.DeathTypes.Dissolve;
            }
        }

        else if ((collider.GetComponent<Hazard>()) != null && (collider.tag == "ExplodeHazard"))
        {
            // if the hazard is active
            if (collider.GetComponent<Hazard>().IsActive)
            {
                //Change character's health by the damage of the hazard, note the minus operator to remove the health.
                ChangeHealth(-collider.GetComponent<Hazard>().Damage);
                currentDeathType = EnumDefinitions.DeathTypes.Explode;
            }
        }
    }

    /// <summary>
    /// Allows other scripts to change the currentDeathType
    /// </summary>
    /// <param name="newDeathType">Must be an existing EnumDefinitions.DeathTypes</param>
    public void ChangeDeathType(EnumDefinitions.DeathTypes newDeathType)
    {
        currentDeathType = newDeathType;
    }

    void FoundClam(GameObject clam)
    {
        //Destroy the clam.
        Destroy(clam);

        //Increment number of pickups found.
        NumberFoundClams++;

        //Find the first unfound clam image and update it.
        foreach (Image clamImage in clamUiImages)
        {
            if (clamImage.sprite != foundClamSprite)
            {
                clamImage.sprite = foundClamSprite;
                break;
            }
        }

        //Play sound effect.

        if (audioManager != null)
        {
            audioManager.Play("Clam");
        }

    }

}
