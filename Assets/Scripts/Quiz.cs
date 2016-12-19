using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz : MonoBehaviour {

	[SerializeField] 
	private Texture2D quizTexture;
	[SerializeField]
	private AudioClip selectedDialog;

	#region OnePerQuizDialog
	[SerializeField]
	private AudioClip mainQuestion;
	[SerializeField]
	private AudioClip correction;
	[SerializeField]
	private AudioClip agreement;
	#endregion
	#region somePatternDialog
	[SerializeField]
	private List<AudioClip> reasonQuestions;
	[SerializeField]
	private List<AudioClip> feedbackEliciations;
	[SerializeField]
	private List<AudioClip> hints;
	[SerializeField]
	private List<AudioClip> confirmQuestions;
	[SerializeField]
	private List<AudioClip> disagreements_weak;
	[SerializeField]
	private List<AudioClip> disagreements_strong;
	[SerializeField]
	private List<AudioClip> disagreements_simple;
	#endregion
	#region Flags
	private bool isSaidCorrection = false;
	private bool usedReasonQ = false;
	private bool usedConfirmQ = false;
	#endregion

	public AudioClip GetNextDialog(UserReactionType userReaction){
		float randomValue;

		switch(userReaction){
			#region MAINQ_ANSWER_CORRECT 
		case UserReactionType.MainQuestionAnswerCorrect:
			if (!isSaidCorrection && (!usedConfirmQ || !usedReasonQ)) {
				if (usedConfirmQ && !usedReasonQ) {
					selectedDialog = GetRandomReasonQ ();
				} else if (!usedConfirmQ && usedReasonQ) {
					selectedDialog = GetRandomConfirmQ ();
				} else {
					if (Random.value >= 0.5f) {
						selectedDialog = GetRandomConfirmQ ();
					} else {
						selectedDialog = GetRandomReasonQ ();
					}
				}
			} else {
				selectedDialog = agreement;
			}
			isSaidCorrection = true;
			break; 
				#endregion //ReasonQ confirmQ else agreement, if once corrected, isSaidCorrectionFlag is true, and next this pattern make agreement
			#region MAINQ_ANSWER_INCORRECT_WEAK
		case UserReactionType.MainQuestionAnswerIncorrectWeak:

			randomValue = Random.value;

			if (usedConfirmQ && !usedReasonQ) {//when Not used ResonQ

				if(randomValue <= 1.0f / 3.0f){
					selectedDialog = GetRandomReasonQ();
				} else if(randomValue <= 2.0f / 3.0f){
					selectedDialog = GetRandomDisagreement_Weak();
				} else {
					selectedDialog = GetRandomDisagreement_Simple();
				}

			} else if (!usedConfirmQ && usedReasonQ) {//not used confirmQ
					
				if(randomValue <= 1.0f / 3.0f){
					selectedDialog = GetRandomConfirmQ();
				} else if(randomValue <= 2.0f / 3.0f){
					selectedDialog = GetRandomDisagreement_Weak();
				} else {
					selectedDialog = GetRandomDisagreement_Simple();
				}

			} else if(!usedReasonQ && !usedConfirmQ){ //Not use ConfirmQ and ReasonQ

				if(randomValue <= 1.0f / 4.0f){
					selectedDialog = GetRandomConfirmQ();
				} else if(randomValue <= 2.0f / 4.0f){
					selectedDialog = GetRandomDisagreement_Weak();
				} else if(randomValue <= 3.0f / 4.0f){
					selectedDialog = GetRandomDisagreement_Simple();
				} else {
					selectedDialog = GetRandomReasonQ();
				}

			} else {
				
				if (randomValue >= 1.0f / 2.0f) {
					selectedDialog = GetRandomDisagreement_Simple();
				} else {
					selectedDialog = GetRandomDisagreement_Weak();;
				}
			}

			break;
			#endregion //reasonQ, disagreement weak, disagreement simple, confirmQ hint
			#region MAINQ_ANSWER_INCORRECT_STRONG
		case UserReactionType.MainQuestionAnswerIncorrectStrong:

			randomValue = Random.value;

			if (usedConfirmQ && !usedReasonQ) {//when Not used ResonQ

				if(randomValue <= 1.0f / 3.0f){
					selectedDialog = GetRandomReasonQ();
				} else if(randomValue <= 2.0f / 3.0f){
					selectedDialog = GetRandomDisagreement_Strong();
				} else {
					selectedDialog = GetRandomDisagreement_Simple();
				}

			} else if (!usedConfirmQ && usedReasonQ) {//not used confirmQ

				if(randomValue <= 1.0f / 3.0f){
					selectedDialog = GetRandomConfirmQ();
				} else if(randomValue <= 2.0f / 3.0f){
					selectedDialog = GetRandomDisagreement_Strong();
				} else {
					selectedDialog = GetRandomDisagreement_Simple();
				}

			} else if(!usedReasonQ && !usedConfirmQ){ //Not use ConfirmQ and ReasonQ

				if(randomValue <= 1.0f / 4.0f){
					selectedDialog = GetRandomConfirmQ();
				} else if(randomValue <= 2.0f / 4.0f){
					selectedDialog = GetRandomDisagreement_Strong();
				} else if(randomValue <= 3.0f / 4.0f){
					selectedDialog = GetRandomDisagreement_Simple();
				} else {
					selectedDialog = GetRandomReasonQ();
				}

			} else {

				if (randomValue >= 1.0f / 2.0f) {
					selectedDialog = GetRandomDisagreement_Strong();
				} else {
					selectedDialog = GetRandomDisagreement_Weak();;
				}
			}
			break; 
			#endregion //reasonQ, disagreement strong, disagreemnt simple, confirmQ hint
			#region REASON_CORRECT
		case UserReactionType.ReasonQuestionAnswerCorrect:
			selectedDialog = agreement;
			break;
			#endregion //Only agreement
			#region REASON_INCORRECT
		case UserReactionType.ReasonQuestionAnswerIncorrect:
			if(hints.Count > 0){
				if(Random.value >= 0.5f){
					selectedDialog = GetRandomHint();
				}else{
					selectedDialog = GetRandomDisagreement_Simple();	
				}
			} else {
				selectedDialog = GetRandomDisagreement_Simple();	
			}
			break;
			#endregion //hints or disagreesimple, if no hints, use only disagreesimle
			#region HAVE_NO_IDEA
		case UserReactionType.HaveNoIdea:
			//hint
			if (hints.Count > 0) {
				selectedDialog = GetRandomHint ();
			} else {
				selectedDialog = correction;
			}
			break;
			#endregion //if have no hints, select correction
			#region NO_REACTION
		case UserReactionType.NoReaction:
			//hint or feedback eliciation
			if(hints.Count > 0){
				if(Random.value >= 0.5f){
					selectedDialog = GetRandomHint();
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
	public bool finishedMainQ(){
		return mainQuestion == null;
	}
	public AudioClip GetMainQ(){
		selectedDialog = mainQuestion;
		mainQuestion = null;
		return selectedDialog;
	}
	public AudioClip GetRandomFeedbackEliciation (){
		return feedbackEliciations [(int)(Random.value * (float)feedbackEliciations.Count)];
	}
	public AudioClip GetCorrection(){
		return correction;	
	}
	private AudioClip GetRandomConfirmQ(){
		usedConfirmQ = true;
		return confirmQuestions[(int)(Random.value * (float)confirmQuestions.Count)];
	}
	private AudioClip GetRandomReasonQ(){
		usedReasonQ = true;
		return reasonQuestions[(int)(Random.value * (float)reasonQuestions.Count)];
	}
	private AudioClip GetRandomHint(){
		//delete selected hint
		int selectindex = (int)(Random.value * (float)hints.Count);
		AudioClip usedhint = hints [selectindex];
		hints.RemoveAt (selectindex);
		return usedhint;
	}
	private AudioClip GetRandomDisagreement_Simple(){
		return disagreements_simple[(int)(Random.value * (float)disagreements_simple.Count)];
	}
	private AudioClip GetRandomDisagreement_Weak(){
		return disagreements_weak[(int)(Random.value * (float) disagreements_weak.Count)];
	}
	private AudioClip GetRandomDisagreement_Strong(){
		return disagreements_strong [(int)(Random.value * (float)disagreements_strong.Count)];
	}
}
