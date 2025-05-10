using UnityEngine;

public class ChangeCameraBackToPlayer : StateMachineBehaviour
{
    public bool enableCutsceneCameraOnEnter = true;
    public bool enablePlayerCameraOnExit = true;

    //TODO: Make it less taśma
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!enableCutsceneCameraOnEnter) return;

        CutSceneCameraManager camManager = animator.GetComponent<CutSceneCameraManager>();

        if (!camManager) return;

            camManager.cutSceneCam.enabled = true;
            camManager.cutSceneCamNoShader.enabled = true;
            camManager.playerCam.enabled = false;
            camManager.playerCamNoShader.enabled = false;
            camManager.scanner.SetActive(false);
        if (camManager.cutSceneCam)
        {
        }

        if (camManager.cutSceneCamNoShader)
        {
        }

        if (camManager.playerCam)
        {
        }

        if (camManager.playerCamNoShader)
        {
        }

        if (camManager.scanner)
        {
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!enablePlayerCameraOnExit) return;

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

        if (camManager.cutSceneCamNoShader == null)
        {
            Debug.LogError("cutSceneCamNoPostProcessing nie jest przypisana w Inspektorze!");
        }

        if (camManager.playerCamNoShader == null)
        {
            Debug.LogError("playerCamNoPostProcessing nie jest przypisana w Inspektorze!");
        }

        if (camManager != null)
        {
            camManager.cutSceneCam.enabled = false;
            camManager.cutSceneCamNoShader.enabled = false;
            camManager.playerCam.enabled = true;
            camManager.playerCamNoShader.enabled = true;
            Debug.Log("Kamery zosta�y zmienione: CutSceneCam wy��czona, PlayerCam w��czona.");
        }
        camManager.scanner.SetActive(true);
    }
}
