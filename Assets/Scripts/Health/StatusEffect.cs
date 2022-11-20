namespace Health
{
    [System.Serializable]
    public struct StatusEffect
    {
        public string name;
        public float lifetime;
        public float tickRate;
        public Damage tickDamage;

        public override int GetHashCode()
        { 
            return name.GetHashCode();
        }
    }
}
