namespace UpdServer
{
    internal class School
    {
        private string? name;
        public string Name { get { return name; } set { name = value; } }

        private string? surname;
        public string Surname { get { return surname; } set { surname = value; } }
        private int age;
        public int Age
        {
            get { return age; }
            set
            {
                if (value < 0)
                    age = 0;
                else
                    age = value;
            }
        }
        private bool isbad;
        public bool IsBad { get { return isbad; } set { isbad = value; } }

    }
}
