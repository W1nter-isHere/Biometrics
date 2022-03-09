namespace Core
{
    public interface IDamagable
    {
        public bool Damage(uint amount);

        public void Heal(uint amount);
    }
}