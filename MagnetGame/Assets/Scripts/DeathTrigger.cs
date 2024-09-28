using System.Threading.Tasks;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private ChangeLevelManager _changeLevelManager;

    private void Start()
    {
        _changeLevelManager = ChangeLevelManager.Instance;
    }

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController playerController))
        {
            playerController.DIE();
            await Task.Delay(2000);
            _changeLevelManager.ChangeLevel(Utils.GetCurrentLevel().name);
        }
    }
}
