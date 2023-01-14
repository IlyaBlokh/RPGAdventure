using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core
{
  public sealed class SceneLoader
  {
    private const string GameScene = "WorldScene";

    public async void LoadGame()
    {
      await LoadScene(GameScene);
    }

    private async UniTask LoadScene(string sceneName)
    {
      await UniTask.SwitchToMainThread();
      await SceneManager.LoadSceneAsync(sceneName);
    }
  }
}