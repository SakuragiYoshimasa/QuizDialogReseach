using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// # Quiz controller.
/// ## Key Binds are these
/// ### force command
/// N:Move next quiz
/// C:Play Correction
/// F:Play Feedback Eliciation
/// X:Repeat
/// 
/// ### User Reactions
/// Q:Main Correct
/// W:Main Incorrect Weak
/// E:Main Incorrect Strong
/// R:Reason Correct
/// T:Reason Incorrect
/// Y:Have no Idea
/// U:No Reaction
/// 
/// </summary>
public class QuizController : Singleton<QuizController> {

	#region QuizFields
	[SerializeField]
	private List<Quiz> quizCollection;
	[SerializeField]
	private int quizIndex;
	#endregion
	#region AudioClips
	[SerializeField]
	private AudioClip greeting;
	[SerializeField]
	private List<AudioClip> moveNextQuizs;
	[SerializeField]
	private AudioClip acknowledge;
	#endregion
	#region Textures
	[SerializeField]
	private Texture2D startScreen;
	[SerializeField]
	private Texture2D acknowledgeScreen;
	#endregion

	enum ExperimentState {
		StartScreen,
		Greeting,
		InExperiment,
		acknowledge
	}

	[SerializeField]
	private ExperimentState expState;
	private Dictionary<KeyCode, UserReactionType> keyBinds;

	private const int WAIT_FRAME = 60;
	private int nowFrame = 0;


	void Start () {
		expState = ExperimentState.StartScreen;
		quizIndex = 0;
		GUIManager.I.SetScreenTexture (startScreen);
		keyBinds = new Dictionary<KeyCode, UserReactionType> (0);
		keyBinds.Add (KeyCode.Q, UserReactionType.MainQuestionAnswerCorrect);
		keyBinds.Add (KeyCode.W, UserReactionType.MainQuestionAnswerIncorrectWeak);
		keyBinds.Add (KeyCode.E, UserReactionType.MainQuestionAnswerIncorrectStrong);
		//keyBinds.Add (KeyCode.R, UserReactionType.ReasonQuestionAnswerCorrect);
		//keyBinds.Add (KeyCode.T, UserReactionType.ReasonQuestionAnswerIncorrect);
		keyBinds.Add (KeyCode.Y, UserReactionType.HaveNoIdea);
		keyBinds.Add (KeyCode.U, UserReactionType.NoReaction);
	}
	void StartGreeting(){
		expState = ExperimentState.Greeting;
		SoundController.I.playSound (greeting);
	}
	void Update () {
		switch(expState){
		case ExperimentState.StartScreen:
			UpdateInStartScreen ();
			break;
		case ExperimentState.Greeting:
			UpdateInGreeting ();
			break;
		case ExperimentState.InExperiment:
			UpdateInExperiment ();
			break;
		case ExperimentState.acknowledge:
			UpdateInAcknowledge ();
			break;
		default:
			break;
		}
	}
	void UpdateInStartScreen(){
		if(Input.GetKeyDown(KeyCode.Space)){
			StartGreeting ();
		}
	}
	void UpdateInGreeting(){
		if(!SoundController.I.isPlaying()){
			expState = ExperimentState.InExperiment;
		}
	}
	void UpdateInExperiment(){
		if(SoundController.I.isPlaying()){
			return;
		} //When is playing not disturb
		if(!quizCollection[quizIndex].finishedMainQ()){


			if (nowFrame < WAIT_FRAME) {
				nowFrame++;
				return;
			} else {
				nowFrame = 0;
			}

			SoundController.I.playSound (quizCollection[quizIndex].GetMainQ());
			GUIManager.I.SetScreenTexture (quizCollection[quizIndex].GetQuizTexture());
			return;
		} //Play MainQ
			
		foreach(KeyValuePair<KeyCode, UserReactionType> keybind in keyBinds){
			if(Input.GetKeyDown(keybind.Key)){
				SoundController.I.playSound(
					quizCollection [quizIndex].GetNextDialog (keybind.Value)
				);
				return;
			}
		} //Input User reactions

		if(Input.GetKeyDown(KeyCode.N)){ 
			MoveNextQuiz (); //when finish quiz, say correction or user say correction and agreemt
			return;
		}else if(Input.GetKeyDown(KeyCode.C)){
			CurrentQuizWillFinishAndSelectCorrection ();  //use correction when no hints and user like no idea
		}else if(Input.GetKeyDown(KeyCode.F)){
			SoundController.I.playSound (quizCollection[quizIndex].GetRandomFeedbackEliciation()); //for using when after hint or disagreement
		}else if(Input.GetKeyDown(KeyCode.X)){
			Repeat ();
		}

		if(quizCollection[quizIndex].SaidCorrection){
			GUIManager.I.SetScreenTexture (quizCollection[quizIndex].GetAnswerTexture());
		}
	}
	void UpdateInAcknowledge(){
		if(!SoundController.I.isPlaying()){
			GUIManager.I.SetScreenTexture (startScreen);
		}
	}
	void CurrentQuizWillFinishAndSelectCorrection(){
		SoundController.I.playSound (quizCollection[quizIndex].GetCorrection());
	}
	public void MoveNextQuiz(){
		if (!SoundController.I.isPlaying()) {

			quizIndex++;
			if (quizIndex >= quizCollection.Count) {
				FinishQuiz ();
			} else {
				SoundController.I.playSound (GetRandomMoveNextClip ());
				GUIManager.I.SetScreenTexture (quizCollection[quizIndex].GetQuizTexture());
			}

		}else{
			return;
		}
	}
	void FinishQuiz(){
		SoundController.I.StopSound ();
		expState = ExperimentState.acknowledge;
		SoundController.I.playSound (acknowledge);
		GUIManager.I.SetScreenTexture (acknowledgeScreen);
	}
	AudioClip GetRandomMoveNextClip(){
		return moveNextQuizs[(int)(Random.value * (float)moveNextQuizs.Count)];
	}

	void Repeat(){
		SoundController.I.playSound (quizCollection[quizIndex].RequestedRepeat());
	}
}
