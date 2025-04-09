using UnityEngine;

public class ChangeCameraBackToPlayer : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CutSceneCameraManager camManager = animator.GetComponent<CutSceneCameraManager>();

        // Sprawdzenie, czy komponent CutSceneCameraManager zosta� znaleziony
        if (camManager == null)
        {
            Debug.LogError("Nie znaleziono komponentu CutSceneCameraManager na Animatorze!");
            return;
        }

        // Sprawdzenie, czy kamery s� przypisane
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
            Debug.Log("Kamery zosta�y zmienione: CutSceneCam wy��czona, PlayerCam w��czona.");
        }
        camManager.scanner.SetActive(true);
    }
}
