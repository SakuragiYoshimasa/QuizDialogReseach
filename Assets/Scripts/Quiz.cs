﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz : MonoBehaviour {

	[SerializeField] 
	private Texture2D quizTexture;
	[SerializeField] 
	private Texture2D answerTexture;
	[SerializeField]
	private AudioClip selectedDialog;

	#region OnePerQuizDialog
	[SerializeField]
	private AudioClip mainQuestion;
	[SerializeField]
	private AudioClip correction;
	#endregion
	#region somePatternDialog
	[SerializeField]
	private List<AudioClip> hints;
	#endregion
	#region Flags
	private bool isSaidCorrection = false;
	private bool usedReasonQ = false;
	private bool usedConfirmQ = false;
	#endregion

	private bool saidCorrection = false;
	public bool SaidCorrection {
		get {
			return saidCorrection;
		}
	}

	public AudioClip GetNextDialog(UserReactionType userReaction){
		float randomValue;

		switch(userReaction){
			#region MAINQ_ANSWER_CORRECT 
		case UserReactionType.MainQuestionAnswerCorrect:
			if (!isSaidCorrection && (!usedConfirmQ || !usedReasonQ)) {
				if (usedConfirmQ && !usedReasonQ) {
					selectedDialog = DialogHolder.I.GetRandomReasonQ ();
					usedReasonQ = true;
				} else if (!usedConfirmQ && usedReasonQ) {
					selectedDialog = DialogHolder.I.GetRandomConfirmQ ();
					usedConfirmQ = true;
				} else {
					if (Random.value >= 0.5f) {
						selectedDialog = DialogHolder.I.GetRandomConfirmQ ();
						usedConfirmQ = true;
					} else {
						selectedDialog = DialogHolder.I.GetRandomReasonQ ();
						usedReasonQ = true;
					}
				}
			} else {
				selectedDialog = DialogHolder.I.GetRandomAgreemnt();
			}
			isSaidCorrection = true;
			break; 
				#endregion //ReasonQ confirmQ else agreement, if once corrected, isSaidCorrectionFlag is true, and next this pattern make agreement
			#region MAINQ_ANSWER_INCORRECT_WEAK
		case UserReactionType.MainQuestionAnswerIncorrectWeak:

			randomValue = Random.value;

			if (usedConfirmQ && !usedReasonQ) {//when Not used ResonQ

				if(randomValue <= 1.0f / 3.0f){
					selectedDialog = DialogHolder.I.GetRandomReasonQ();
					usedReasonQ = true;
				} else if(randomValue <= 2.0f / 3.0f){
					selectedDialog = DialogHolder.I.GetRandomDisagreement_Weak();
				} else {
					selectedDialog = DialogHolder.I.GetRandomDisagreement_Simple();
				}

			} else if (!usedConfirmQ && usedReasonQ) {//not used confirmQ
					
				if(randomValue <= 1.0f / 3.0f){
					selectedDialog = DialogHolder.I.GetRandomConfirmQ();
					usedConfirmQ = true;
				} else if(randomValue <= 2.0f / 3.0f){
					selectedDialog = DialogHolder.I.GetRandomDisagreement_Weak();
				} else {
					selectedDialog = DialogHolder.I.GetRandomDisagreement_Simple();
				}

			} else if(!usedReasonQ && !usedConfirmQ){ //Not use ConfirmQ and ReasonQ

				if(randomValue <= 1.0f / 4.0f){
					selectedDialog = DialogHolder.I.GetRandomConfirmQ();
					usedConfirmQ = true;
				} else if(randomValue <= 2.0f / 4.0f){
					selectedDialog = DialogHolder.I.GetRandomDisagreement_Weak();
				} else if(randomValue <= 3.0f / 4.0f){
					selectedDialog = DialogHolder.I.GetRandomDisagreement_Simple();
				} else {
					selectedDialog = DialogHolder.I.GetRandomReasonQ();
					usedReasonQ = true;
				}

			} else {
				
				if (randomValue >= 1.0f / 2.0f) {
					selectedDialog = DialogHolder.I.GetRandomDisagreement_Simple();
				} else {
					selectedDialog = DialogHolder.I.GetRandomDisagreement_Weak();;
				}
			}

			break;
			#endregion //reasonQ, disagreement weak, disagreement simple, confirmQ hint
			#region MAINQ_ANSWER_INCORRECT_STRONG
		case UserReactionType.MainQuestionAnswerIncorrectStrong:

			randomValue = Random.value;

			if (usedConfirmQ && !usedReasonQ) {//when Not used ResonQ

				if(randomValue <= 1.0f / 3.0f){
					selectedDialog = DialogHolder.I.GetRandomReasonQ();
					usedReasonQ = true;
				} else if(randomValue <= 2.0f / 3.0f){
					selectedDialog = DialogHolder.I.GetRandomDisagreement_Strong();
				} else {
					selectedDialog = DialogHolder.I.GetRandomDisagreement_Simple();
				}

			} else if (!usedConfirmQ && usedReasonQ) {//not used confirmQ

				if(randomValue <= 1.0f / 3.0f){
					selectedDialog = DialogHolder.I.GetRandomConfirmQ();
					usedConfirmQ = true;
				} else if(randomValue <= 2.0f / 3.0f){
					selectedDialog = DialogHolder.I.GetRandomDisagreement_Strong();
				} else {
					selectedDialog = DialogHolder.I.GetRandomDisagreement_Simple();
				}

			} else if(!usedReasonQ && !usedConfirmQ){ //Not use ConfirmQ and ReasonQ

				if(randomValue <= 1.0f / 4.0f){
					selectedDialog = DialogHolder.I.GetRandomConfirmQ();
					usedConfirmQ = true;
				} else if(randomValue <= 2.0f / 4.0f){
					selectedDialog = DialogHolder.I.GetRandomDisagreement_Strong();
				} else if(randomValue <= 3.0f / 4.0f){
					selectedDialog = DialogHolder.I.GetRandomDisagreement_Simple();
				} else {
					selectedDialog = DialogHolder.I.GetRandomReasonQ();
					usedReasonQ = true;
				}

			} else {

				if (randomValue >= 1.0f / 2.0f) {
					selectedDialog = DialogHolder.I.GetRandomDisagreement_Strong();
				} else {
					selectedDialog = DialogHolder.I.GetRandomDisagreement_Weak();;
				}
			}
			break; 
			#endregion //reasonQ, disagreement strong, disagreemnt simple, confirmQ hint
			#region REASON_CORRECT
		case UserReactionType.ReasonQuestionAnswerCorrect:
			selectedDialog = DialogHolder.I.GetRandomAgreemnt();
			break;
			#endregion //Only agreement
			#region REASON_INCORRECT
		case UserReactionType.ReasonQuestionAnswerIncorrect:
			if(hints.Count > 0){
				if(Random.value >= 0.5f){
					selectedDialog = GetHint();
				}else{
					selectedDialog = DialogHolder.I.GetRandomDisagreement_Simple();
				}
			} else {
				selectedDialog = DialogHolder.I.GetRandomDisagreement_Simple();	
			}
			break;
			#endregion //hints or disagreesimple, if no hints, use only disagreesimle
			#region HAVE_NO_IDEA
		case UserReactionType.HaveNoIdea:
			//hint
			if (hints.Count > 0) {
				selectedDialog = GetHint ();
			} else {
				saidCorrection = true;
				selectedDialog = correction;
			}
			break;
			#endregion //if have no hints, select correction
			#region NO_REACTION
		case UserReactionType.NoReaction:
			//hint or feedback eliciation
			if(hints.Count > 0){
				if(Random.value >= 0.5f){
					selectedDialog = GetHint();
				}else{
					selectedDialog = GetRandomFeedbackEliciation ();	
				}
			} else {
				selectedDialog = GetRandomFeedbackEliciation ();	
			}
			break;
			#endregion //eliciation, or make hint. if use all hint, we can use eliciation and recommend use corrention with key command
		default:
			break;
		}
		return selectedDialog;
	}
	public AudioClip RequestedRepeat(){
		return selectedDialog;
	}
	public Texture2D GetQuizTexture(){
		return quizTexture;
	}
	public Texture2D GetAnswerTexture(){
		return answerTexture;
	}
	public bool finishedMainQ(){
		return mainQuestion == null;
	}
	public AudioClip GetMainQ(){
		selectedDialog = mainQuestion;
		mainQuestion = null;
		return selectedDialog;
	}

	private AudioClip GetHint(){
		AudioClip usedhint = hints [0];
		hints.RemoveAt (0);
		return usedhint;
	}

	public AudioClip GetCorrection(){
		saidCorrection = true;
		return correction;	
	}

	public AudioClip GetRandomFeedbackEliciation (){
		return DialogHolder.I.GetRandomFeedbackEliciation();
	}
}