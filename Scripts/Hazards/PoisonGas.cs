using System.Collections;
using UnityEngine;

public class PoisonGas : MonoBehaviour {

    [Tooltip("How long the player is able to stay in the gas before dying. 1.0f = 1 second")]
    [SerializeField] private float timeTillDeath;

    private Character characterScript; // holds the character script that is attached to the player
    private IEnumerator gasCountdown; // holds the coroutine so it can be stopped and reset
    private EnumDefinitions.DeathTypes deathType = EnumDefinitions.DeathTypes.Dissolve; // the type of death that plays for the character when dying in poison gas

    // Update is called once per frame
    void Start () {
        // set the coroutine for gas countdown
        gasCountdown = GasCountdown();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // store the character script
            characterScript = other.GetComponent<Character>();
            // start the gas countdown
            StartCoroutine(gasCountdown);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // Stop the gas countdown
            StopCoroutine(gasCountdown);
            // Reset the gas countdown
            gasCountdown = GasCountdown();
        }
    }

    /// <summary>
    /// Waits for a given amount of time from the inspector then kills the player
    /// </summary>
    /// <returns></returns>
    IEnumerator GasCountdown()
    {
        // wait for the given time to kill the player
        yield return new WaitForSeconds(timeTillDeath);
        // set the characters current death type to the one needed for poison gas
        characterScript.ChangeDeathType(deathType);
        // Kill the player
        characterScript.Health = 0;
    }
}
