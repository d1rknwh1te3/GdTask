namespace GdTasks.Interfaces.Promises;

public interface IPromise : IResolvePromise, IRejectPromise, ICancelPromise
{ }

public interface IPromise<in T> : IResolvePromise<T>, IRejectPromise, ICancelPromise
{ }