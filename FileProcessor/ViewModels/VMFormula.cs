namespace FileProcessor.ViewModels
{
    public class VMFormula
    {
        public const string F0 = "No Formula";
        public const string F1 = "A x B / C";
        public const string F2 = "A mod B x C";
        public const string F3 = "A^C - √B x C"; 
        public const string F4 = "Invalid Formula";

        public string GetDescription(int id)
        {
            string fDescription = "";

            if(id <= 0)
            {
                fDescription = F0.ToString();
            }
            else if (id == 1)
            {
                fDescription = F1.ToString();
            }
            else if(id == 2)
            {
                fDescription = F2.ToString();
            }
            else if(id == 3)
            {
                fDescription = F3.ToString();
            }
            else if(id >= 4)
            {
                fDescription = F4.ToString();
            }

            return fDescription;
        }
    }
    
}