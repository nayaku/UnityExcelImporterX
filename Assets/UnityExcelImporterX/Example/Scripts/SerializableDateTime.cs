using System;
using UnityEngine;

[Serializable]
public class SerializableDateTime
{
    public DateTime value;

    [HideInInspector]
    [SerializeField]
    private long ticks;


    public SerializableDateTime()
    {
        value = DateTime.Now;
    }

    public SerializableDateTime(DateTime dateTime)
    {
        value = dateTime;
    }

    public static implicit operator DateTime(SerializableDateTime sdt)
    {
        return sdt.value;
    }

    public static implicit operator SerializableDateTime(DateTime dateTime)
    {
        return new SerializableDateTime() { value = dateTime };
    }

    public void OnBeforeSerialize()
    {
        ticks = value.Ticks;
    }

    public void OnAfterDeserialize()
    {
        value = new DateTime(ticks);
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
