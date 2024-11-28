namespace GameManagement
{
    /// <summary>
    /// Définition des actions du jeu
    /// </summary>
    public class Action
    {
        public string Holder;
        public string Name;
        public string Description;
        public int Value;

        public Action(string holder, string name, string description, int value)
        {
            Holder = holder;
            Name = name;
            Description = description;
            Value = value;
        }
    }
}