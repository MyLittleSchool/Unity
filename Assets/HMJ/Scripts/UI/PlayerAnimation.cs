using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MJ.DecorationEnum;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace MJ
{
    public class PlayerAnimation : MonoBehaviour
    {
        private void Start()
        {
            InitPlayerAnimation();
        }

        private int[] animatorIndex = new int[(int)DecorationEnum.DECORATION_DATA.DECORATION_DATA_END];
        public Animator[] playerAnimator = new Animator[(int)DecorationEnum.DECORATION_DATA.DECORATION_DATA_END];

        public void SetDecorationAnimData(DecorationEnum.DECORATION_DATA decorationData, int idx)
        {
            playerAnimator[(int)decorationData].SetBool("Anim" + (idx + 1).ToString(), true);
            animatorIndex[(int)decorationData] = idx;

            ResetDecorationAnim();
        }

        public void ResetDecorationAnimData(DecorationEnum.DECORATION_DATA decorationData)
        {
            for (int i = 0; i < (int)DecorationEnum.DECORATION_DATA.DECORATION_DATA_END; i++)
            {
                playerAnimator[(int)decorationData].SetBool("Anim" + (i + 1).ToString(), false);
            }
        }

        public void ResetDecorationAnim()
        {
            for (int i = 0; i < (int)DecorationEnum.DECORATION_DATA.DECORATION_DATA_END; i++)
            {
                if(animatorIndex[i] >= 0)
                    playerAnimator[i].Play("Anim" + (animatorIndex[i] + 1).ToString(), 0, 0.0f);
            }
        }

        public void InitPlayerAnimation()
        {
            for (int i = 0; i < (int)DecorationEnum.DECORATION_DATA.DECORATION_DATA_END; i++)
                animatorIndex[i] = -1;
        }
    }

}
