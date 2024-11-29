using UnityEngine.UI;

namespace VRKeyboard.Utils
{
    public class Shift : Key
    {
        private Text subscript;
        public string keytxt;
        public string subscripttxt;

        public override void Awake()
        {
            base.Awake();
            subscript = transform.Find("Subscript").GetComponent<Text>();
            // ShiftKeyT();
        }

        public override void ShiftKey()
        {
            var tmp = key.text;
            key.text = subscript.text;
            subscript.text = tmp;
        }

        public override void ShiftKeyT()
        {
            if (key != null)
            {
                var tmp = key.text;
                key.text = keytxt;
                if (subscript != null)
                    subscript.text = subscripttxt;
            }
        }
    }
}