using UnityEngine.SceneManagement;

public class Developer : Singleton<Developer>
{
   public void NextScene()
   {
      SceneManager.LoadScene(1);
   }

   public void PreviousScene()
   {
      SceneManager.LoadScene(0);
   }
}