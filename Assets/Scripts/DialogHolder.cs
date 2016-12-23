using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHolder : Singleton<DialogHolder> {

	[SerializeField]
	private List<AudioClip> agreements;
	[SerializeField]
	private List<AudioClip> reasonQuestions;
	[SerializeField]
	private List<AudioClip> feedbackEliciations;
	[SerializeField]
	private List<AudioClip> confirmQuestions;
	[SerializeField]
	private List<AudioClip> disagreements_weak;
	[SerializeField]
	private List<AudioClip> disagreements_strong;
	[SerializeField]
	private List<AudioClip> disagreements_simple;

	public AudioClip GetRandomAgreemnt(){
		return agreements [(int)(Random.value * (float)agreements.Count)];
	}


	public AudioClip GetRandomFeedbackEliciation (){
		return feedbackEliciations [(int)(Random.value * (float)feedbackEliciations.Count)];
	}

	public AudioClip GetRandomConfirmQ(){
		return confirmQuestions[(int)(Random.value * (float)confirmQuestions.Count)];
	}
	public AudioClip GetRandomReasonQ(){
		return reasonQuestions[(int)(Random.value * (float)reasonQuestions.Count)];
	}

	public AudioClip GetRandomDisagreement_Simple(){
		return disagreements_simple[(int)(Random.value * (float)disagreements_simple.Count)];
	}
	public AudioClip GetRandomDisagreement_Weak(){
		return disagreements_weak[(int)(Random.value * (float) disagreements_weak.Count)];
	}
	public AudioClip GetRandomDisagreement_Strong(){
		return disagreements_strong [(int)(Random.value * (float)disagreements_strong.Count)];
	}
}
