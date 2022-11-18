namespace Health
{
    public interface IDamageble
    {
        public void TakeDamage(Damage damage);
        public void AddStatusEffect(StatusEffect statusEffect);
    }
}
