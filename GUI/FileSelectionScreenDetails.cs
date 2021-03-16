using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine.EventSystems;
using Com.LuisPedroFonseca.ProCamera2D;

namespace MoreMountains.CorgiEngine
{
    public class FileSelectionScreenDetails : MonoBehaviour
    {
        public Text FileOneArea;
        public Text FileTwoArea;
        public Text FileThreeArea;
        public Text FileOnePercentage;
        public Text FileTwoPercentage;
        public Text FileThreePercentage;

        public string AreaOneFinalName = "COLONY BASE";
        public string AreaTwoFinalName = "IN A PLANT";
        public string AreaThreeFinalName = "CRYSTAL MINES";
        public string AreaFourFinalName = "CHURCHY PLACE";
        public string AreaFiveFinalName = "VOLCANO";


        // Start is called before the first frame update
        void Start()
        {
            CheckFileOneStatus();
            CheckFileTwoStatus();
            CheckFileThreeStatus();
        }

        protected void CheckFileOneStatus()
        {  
            switch (GameManager.Instance.FileOneArea)
            {
                case "Area One Final (1)":
                    FileOneArea.text = GameManager.Instance._file1CompletionPercentage != 0 ? AreaOneFinalName : "NEW GAME";
                    break;
                case "Area Two Final (1)":
                    FileOneArea.text = AreaTwoFinalName;
                    break;
                case "Area Three Final (1)":
                    FileOneArea.text = AreaThreeFinalName;
                    break;
                case "Area Four Final (1)":
                    FileOneArea.text = AreaFourFinalName;
                    break;
                case "Area Five Final (1)":
                    FileOneArea.text = AreaFiveFinalName;
                    break;
                default:
                    FileOneArea.text = "NEW GAME";
                    break;
            }

            FileOnePercentage.text = GameManager.Instance._file1CompletionPercentage.ToString() + "%";
        }

        protected void CheckFileTwoStatus()
        {
            switch (GameManager.Instance.FileTwoArea)
            {
                case "Area One Final (1)":
                    FileTwoArea.text = GameManager.Instance._file2CompletionPercentage != 0 ? AreaOneFinalName : "NEW GAME";
                    break;
                case "Area Two Final (1)":
                    FileTwoArea.text = AreaTwoFinalName;
                    break;
                case "Area Three Final (1)":
                    FileTwoArea.text = AreaThreeFinalName;
                    break;
                case "Area Four Final (1)":
                    FileTwoArea.text = AreaFourFinalName;
                    break;
                case "Area Five Final (1)":
                    FileTwoArea.text = AreaFiveFinalName;
                    break;
                default:
                    FileTwoArea.text = "NEW GAME";
                    break;
            }

            FileTwoPercentage.text = GameManager.Instance._file2CompletionPercentage.ToString() + "%";
        }

        protected void CheckFileThreeStatus()
        {
            switch (GameManager.Instance.FileThreeArea)
            {
                case "Area One Final (1)":
                    FileThreeArea.text = GameManager.Instance._file3CompletionPercentage != 0 ? AreaOneFinalName : "NEW GAME";
                    break;
                case "Area Two Final (1)":
                    FileThreeArea.text = AreaTwoFinalName;
                    break;
                case "Area Three Final (1)":
                    FileThreeArea.text = AreaThreeFinalName;
                    break;
                case "Area Four Final (1)":
                    FileThreeArea.text = AreaFourFinalName;
                    break;
                case "Area Five Final (1)":
                    FileThreeArea.text = AreaFiveFinalName;
                    break;
                default:
                    FileThreeArea.text = "NEW GAME";
                    break;
            }

            FileThreePercentage.text = GameManager.Instance._file3CompletionPercentage.ToString() + "%";
        }
    }
}