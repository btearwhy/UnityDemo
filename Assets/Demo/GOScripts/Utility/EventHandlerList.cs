using System;

public sealed class EventHandlerList<T> : IDisposable
{
    ListEntry head;

    public EventHandlerList()
    { }

    public Delegate this[T key]
    {
        get
        {
            ListEntry e = Find(key);
            return e == null ? null : e.m_handler;
        }
        set
        {
            ListEntry e = Find(key);
            if (e != null)
            {
                e.m_handler = value;
            }
            else
            {
                head = new ListEntry(key, value, head);
            }
        }
    }

    public void AddHandler(T key, Delegate value)
    {
        ListEntry e = Find(key);
        if (e != null)
        {
            e.m_handler = Delegate.Combine(e.m_handler, value);
        }
        else
        {
            head = new ListEntry(key, value, head);
        }
    }

    public void AddHandlers(EventHandlerList<T> listToAddFrom)
    {
        ListEntry currentListEntry = listToAddFrom.head;
        while (currentListEntry != null)
        {
            AddHandler(currentListEntry.m_key, currentListEntry.m_handler);
            currentListEntry = currentListEntry.m_next;
        }
    }

    public void Dispose()
    {
        head = null;
    }

    private ListEntry Find(T key)
    {
        ListEntry found = head;
        while (found != null)
        {
            if (found.m_key.Equals(key))
            {
                break;
            }
            found = found.m_next;
        }
        return found;
    }

    public void RemoveHandler(T key, Delegate value)
    {
        ListEntry e = Find(key);
        if (e != null)
        {
            e.m_handler = Delegate.Remove(e.m_handler, value);
        }
    }

    private sealed class ListEntry
    {
        internal ListEntry m_next;
        internal T m_key;
        internal Delegate m_handler;

        public ListEntry(T key, Delegate handler, ListEntry next)
        {
            m_next = next;
            m_key = key;
            m_handler = handler;
        }
    }
}
