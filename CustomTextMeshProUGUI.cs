using System;
using TMPro;

namespace OneProjest
{

    public class CustomTextMeshProUGUI : TextMeshProUGUI {
        private Action resetCallBack;

        public void SetCallBack(Action callBack) {
            resetCallBack = callBack;
        }

        protected override void Reset() {
            base.Reset();
            if (resetCallBack != null) {
                resetCallBack.Invoke();
            }
        }

        protected override void OnDestroy() {
            resetCallBack = null;
        }
    }
}

