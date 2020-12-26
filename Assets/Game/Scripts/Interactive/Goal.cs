using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Beat the Level!");
            ChangeScene();
        }
    }

    /// <summary>
    /// This method changes the scene to the next scene in the build order after calling DestroyLoop()
    /// in LoopReset to destroy persisting players and the persisting LoopReset. A different method should be used
    /// to reload the scene entirely in case of failure.
    /// </summary>
    private void ChangeScene()
    {
        LoopReset.Instance.DestroyLoop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
