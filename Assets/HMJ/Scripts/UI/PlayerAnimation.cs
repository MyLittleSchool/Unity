using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MJ.DecorationEnum;

namespace MJ
{
    public class PlayerAnimation : MonoBehaviour
    {
        public Animator[] playerAnimator = new Animator[(int)DecorationEnum.DECORATION_DATA.DECORATION_DATA_END];


        public void SetDecorationAnimData(DecorationEnum.DECORATION_DATA decorationData, int idx)
        {
            playerAnimator[(int)decorationData].SetBool("Anim" + (idx + 1).ToString(), true);
        }

        public void ResetDecorationAnimData(DecorationEnum.DECORATION_DATA decorationData)
        {
            for(int i = 0; i < (int)DecorationEnum.DECORATION_DATA.DECORATION_DATA_END; i++)
            {
                playerAnimator[(int)decorationData].SetBool("Anim" + (i + 1).ToString(), false);
            }
        }
    }
}
