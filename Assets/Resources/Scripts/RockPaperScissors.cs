using System.Collections;
using UnityEngine;

public class RockPaperScissors : MonoBehaviour
{
    // public static RockPaperScissors Instance { get; set; }
    public enum Choice { Rock, Paper, Scissors }

    public Animator playerAnimator;
    public Animator enemyAnimator;

    public void PlayerChooses(Choice playerChoice)
    {
        Choice enemyChoice = (Choice)Random.Range(0, 3);
        string outcome = DetermineWinner(playerChoice, enemyChoice);

        UIManager.Instance.HideRPSPanel();

        // Play animation
        playerAnimator.SetTrigger(playerChoice.ToString());
        enemyAnimator.SetTrigger(enemyChoice.ToString());

        // Wait for animation to finish, then spawn
        StartCoroutine(HandleAfterAnimations(playerChoice, enemyChoice, outcome));
    }

    IEnumerator HandleAfterAnimations(Choice playerChoice, Choice enemyChoice, string outcome)
    {
        // wait for animation to play
        yield return new WaitForSeconds(4f); // match animation duration

        playerAnimator.SetTrigger("Idle");
        enemyAnimator.SetTrigger("Idle");

        if (outcome == "Draw!")
        {
            UIManager.Instance.ShowDrawMessage();
            SoundManager.Instance.drawSound.Play();
            yield break;
        }

        RewardManager.Instance.HandlePlayerChoice(
            outcome == "You Win!",
            outcome == "You Lose!"
        );
    }



    string DetermineWinner(Choice p, Choice e)
    {
        if (p == e) return "Draw!";
        if ((p == Choice.Rock && e == Choice.Scissors) || (p == Choice.Paper && e == Choice.Rock) || (p == Choice.Scissors && e == Choice.Paper))
        {
            UIManager.Instance.ShowRPSRoundResult("win");
            return "You Win!";
        }

        UIManager.Instance.ShowRPSRoundResult("lose");
        return "You Lose!";
    }

    public void ChooseRock() => PlayerChooses(Choice.Rock);
    public void ChoosePaper() => PlayerChooses(Choice.Paper);
    public void ChooseScissors() => PlayerChooses(Choice.Scissors);

    
}
