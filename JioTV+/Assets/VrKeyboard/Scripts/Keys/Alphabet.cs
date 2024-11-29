using System;

namespace VRKeyboard.Utils
{
    public class Alphabet : Key
    {
        public override void CapsLock(bool isUppercase)
        {
            try
            {
                if (isUppercase)
                {
                    key.text = key.text.ToUpper();
                }
                else
                {
                    key.text = key.text.ToLower();
                }
            }
            catch (Exception E)
            {
            }
        }
    }
}