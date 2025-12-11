namespace chamwhy
{
    public interface ICommonMonsterState<T>: IState<T>
    {
        void OnCancel();
    }
}