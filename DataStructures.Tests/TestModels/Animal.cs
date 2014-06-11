namespace DataStructures.Tests.TestModels
{
    internal abstract class Animal
    {
        protected Animal(int id)
        {
            this.Id = id;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.GetType().Name, this.Id);
        }

        public int Id { get; private set; }
    }
}