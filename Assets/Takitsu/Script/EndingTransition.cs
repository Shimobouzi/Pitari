using System.Drawing.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingTransition : MonoBehaviour
{
	private void Transition()
	{
		SceneManager.LoadScene("Ending");
	}
}
