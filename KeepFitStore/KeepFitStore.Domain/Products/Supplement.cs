namespace KeepFitStore.Domain.Products
{
    public abstract class Supplement : Product
    {
        protected Supplement()
        {
            this.IsSuatableForVegans = false;
        }

        public bool IsSuatableForVegans { get; set; }

        public string Directions { get; set; }
    }
}