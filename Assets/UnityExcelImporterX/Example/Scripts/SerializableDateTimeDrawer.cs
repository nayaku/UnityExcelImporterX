using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SerializableDateTime))]
public class SerializableDateTimeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        _ = EditorGUI.BeginProperty(position, label, property);

        // 获取ticks属性
        SerializedProperty ticksProperty = property.FindPropertyRelative("ticks");
        long ticks = ticksProperty.longValue;

        // 转换为DateTime
        DateTime dateTime = new(ticks);

        // 计算位置
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        float totalWidth = position.width;

        // 日期部分 (年、月、日) 占60%宽度
        Rect dateSection = new(position.x, position.y, totalWidth * 0.6f, position.height);
        float datePartWidth = (dateSection.width / 3) - 4;

        // 时间部分 (时、分、秒) 占40%宽度
        Rect timeSection = new(position.x + (totalWidth * 0.6f) + 5, position.y, (totalWidth * 0.4f) - 5, position.height);
        float timePartWidth = (timeSection.width / 3) - 4;

        // 绘制年、月、日字段
        int year = EditorGUI.IntField(
            new Rect(dateSection.x, dateSection.y, datePartWidth, dateSection.height),
            dateTime.Year
        );

        int month = EditorGUI.IntField(
            new Rect(dateSection.x + datePartWidth + 4, dateSection.y, datePartWidth, dateSection.height),
            dateTime.Month
        );

        int day = EditorGUI.IntField(
            new Rect(dateSection.x + ((datePartWidth + 4) * 2), dateSection.y, datePartWidth, dateSection.height),
            dateTime.Day
        );

        // 绘制时、分、秒字段
        int hour = EditorGUI.IntField(
            new Rect(timeSection.x, timeSection.y, timePartWidth, timeSection.height),
            dateTime.Hour
        );

        int minute = EditorGUI.IntField(
            new Rect(timeSection.x + timePartWidth + 4, timeSection.y, timePartWidth, timeSection.height),
            dateTime.Minute
        );

        int second = EditorGUI.IntField(
            new Rect(timeSection.x + ((timePartWidth + 4) * 2), timeSection.y, timePartWidth, timeSection.height),
            dateTime.Second
        );

        // 验证日期时间值
        year = Mathf.Clamp(year, 1970, 2100);
        month = Mathf.Clamp(month, 1, 12);
        day = Mathf.Clamp(day, 1, DateTime.DaysInMonth(year, month)); // 根据年月获取当月最大天数
        hour = Mathf.Clamp(hour, 0, 23);
        minute = Mathf.Clamp(minute, 0, 59);
        second = Mathf.Clamp(second, 0, 59);

        // 尝试创建新的DateTime
        try
        {
            DateTime newDateTime = new(year, month, day, hour, minute, second);

            // 如果值发生变化，则更新
            if (newDateTime.Ticks != ticks)
            {
                ticksProperty.longValue = newDateTime.Ticks;
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            // 如果日期无效，不更新值
        }

        EditorGUI.EndProperty();
    }
}
#endif