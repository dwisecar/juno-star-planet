using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{
    public class ObjectDisablerOnLoad : MonoBehaviour
    {
        [Header("Objects to disable if the loading checkpoint is not x")]
        public GameObject[] ObjectToDisable;
        public int CheckPointNumberLoadingInAt;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(CheckIfObjectShouldBeDisabled());
        }

        protected IEnumerator CheckIfObjectShouldBeDisabled()
        {
            yield return new WaitForSeconds(.5f);
            if (LevelManager.Instance.CurrentCheckPoint.CheckpointNumber != CheckPointNumberLoadingInAt)
            {
                for (int i=0; i < ObjectToDisable.Length; i++)
                {
                    if (ObjectToDisable[i] != null)
                    {
                        ObjectToDisable[i].SetActive(false);
                    }
                }
            }
        }
    }
}