namespace _Project._Scripts
{
    public interface ICustomUpdate
    {
        //is used by managers to update the object with a custom update method instead of the default update
        void CustomUpdate();
    }
}