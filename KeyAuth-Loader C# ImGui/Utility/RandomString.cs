namespace KeyAuth.Utility
{
    public static class Randomize
    {
        public static string TextString()
        {
            string str = null;

            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                str += Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))).ToString();
            }
            return str;
        }
    }
}
