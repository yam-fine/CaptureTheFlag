using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
public class GameMenuManager : Singleton<GameMenuManager>
{
   [SerializeField] private TextMeshProUGUI leftText;
   [SerializeField] private TextMeshProUGUI rightText;
   public enum Side {Right, Left}
   public void UpdateScore(int points, Side sideToUpdate )
   {
      switch (sideToUpdate)
      {
         case Side.Left:
            UpdateScoreText(points, leftText);
            break;
         case Side.Right:
            UpdateScoreText(points, rightText);
            break;

      }
   }

   private void UpdateScoreText(int points, TextMeshProUGUI text)
   {
      int newScore = int.Parse(text.text) + points ;
      text.text = newScore.ToString();
   }
   
  
}
