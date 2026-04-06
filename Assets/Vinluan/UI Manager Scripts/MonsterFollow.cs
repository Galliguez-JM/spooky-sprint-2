using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterFollow : MonoBehaviour
{
    public Transform playerTransform;
    public float moveSpeed = 5.0f;
    public float killDistance = 2.0f;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        // Automatically find Casper if he's not assigned
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // 1. Face the player
        Vector3 targetPos = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        transform.LookAt(targetPos);

        // 2. Move toward the player
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // 3. Set Animation (Make sure your Animator has a bool named 'isWalking')
        if (anim != null) anim.SetBool("isWalking", true);

        // 4. THE KILL: If Slender gets close enough, load the Game Over screen
        if (Vector3.Distance(transform.position, playerTransform.position) < killDistance)
        {
            Debug.Log("CAUGHT BY SLENDER!");
            SceneManager.LoadScene(8);
        }
    }
}
