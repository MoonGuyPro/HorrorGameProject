using UnityEngine;

public class ChangeCameraBackToPlayer : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CutSceneCameraManager camManager = animator.GetComponent<CutSceneCameraManager>();

        // Sprawdzenie, czy komponent CutSceneCameraManager zosta³ znaleziony
        if (camManager == null)
        {
            Debug.LogError("Nie znaleziono komponentu CutSceneCameraManager na Animatorze!");
            return;
        }

        // Sprawdzenie, czy kamery s¹ przypisane
        if (camManager.cutSceneCam == null)
        {
            Debug.LogError("cutSceneCam nie jest przypisana w Inspektorze!");
        }

        if (camManager.playerCam == null)
        {
            Debug.LogError("playerCam nie jest przypisana w Inspektorze!");
        }

        if (camManager != null)
        {
            camManager.cutSceneCam.enabled = false;
            camManager.playerCam.enabled = true;
            Debug.Log("Kamery zosta³y zmienione: CutSceneCam wy³¹czona, PlayerCam w³¹czona.");
        }
        camManager.scanner.SetActive(true);
    }
}
