using System.Collections.Generic;

public class MediatorArgs<T>
{
    public T Args;
    public MediatorArgs(T iArgs)
    {
        Args = iArgs;
    }
}

public class MediatorManager<T>
{
    private static MediatorManager<T> mInstance;
    public static MediatorManager<T> Instance => mInstance ??= new MediatorManager<T>();

    public delegate void MediatorHandler<Handler_T>(object iSender, MediatorArgs<Handler_T> iArgs);
    private Dictionary<string, MediatorHandler<T>> mAggregator = new Dictionary<string, MediatorHandler<T>>();

    public void Subscribe(string iKey, MediatorHandler<T> iHandler)
    {
        if (!mAggregator.ContainsKey(iKey))
        {
            mAggregator.Add(iKey, iHandler);
        }
        else
        {
            mAggregator[iKey] += iHandler;
        }
    }

    public void RemoveSubcribe(string iKey)
    {
        if (mAggregator.ContainsKey(iKey))
        {
            mAggregator.Remove(iKey);
        }
    }

    public void Publish(string iKey, object iSender, MediatorArgs<T> iArgs)
    {
        if (mAggregator.ContainsKey(iKey) && mAggregator[iKey] != null)
        {
            mAggregator[iKey](iSender, iArgs);
        }
    }

    public void ClearAllSubcribe()
    {
        mAggregator.Clear();
    }
}