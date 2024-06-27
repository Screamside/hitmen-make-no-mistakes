using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class Cutscene
{
    public List<Cut> cuts = new();

    private bool playerFinishedTask = true;

    public IEnumerator PlayCutscene()
    {
        foreach (var cut in cuts)
        {
            Cut currentCut = cut;

            if (currentCut.type.Equals(CutType.Movement))
            {
                foreach (var cinemachineCamera in GameObject.FindObjectsByType<CinemachineCamera>(
                             FindObjectsSortMode.None))
                {
                    cinemachineCamera.gameObject.SetActive(false);
                }

                currentCut.cinemachineCamera.gameObject.SetActive(true);

                if (currentCut.waitForCameraAnimation)
                {
                    yield return Tween.Position(currentCut.cinemachineCamera.transform,
                        currentCut.movementSettings).ToYieldInstruction();
                    
                }
                else
                {
                    Tween.Position(currentCut.cinemachineCamera.transform,
                        currentCut.movementSettings).ToYieldInstruction();
                }
                
                Debug.Log("?");
            }

            if (currentCut.type.Equals(CutType.Dialogue))
            {

                yield return new WaitForSeconds(currentCut.dialogueDelay);
                
                UIController.ShowDialogue(currentCut.dialogue);
                playerFinishedTask = false;
                GameEvents.OnPlayerKeyPressAfterDialogue.AddListener(SetDialogueFinished);
                yield return new WaitUntil(() => playerFinishedTask);
                GameEvents.OnPlayerKeyPressAfterDialogue.RemoveListener(SetDialogueFinished);
                
                Debug.Log("??");
            }

            if (currentCut.type.Equals(CutType.Question))
            {
                UIController.ShowChoices(currentCut.choiceRight, currentCut.choiceLeft);
                playerFinishedTask = false;
            }
        }
    }

    private void SetDialogueFinished()
    {
        playerFinishedTask = true;
    }
}