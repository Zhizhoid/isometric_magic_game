namespace Health
{
    public struct StatusEffect
    {
        public string name;
        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
    }
}
