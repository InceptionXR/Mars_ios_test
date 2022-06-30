using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class MenuButtonView : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI Label;

        public UnityEvent<MenuButtonView> OnClicked;

        public string Title
        {
            get
            {
                if (Label == null)
                {
                    return default;
                }

                return Label.text;
            }
            set
            {
                if (Label != null)
                {
                    Label.text = value;
                }
            }
        }

        public void OnButtonClick()
        {
            OnClicked?.Invoke(this);
        }
    }
}