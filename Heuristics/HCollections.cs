using System.Collections;

namespace Game.Meta.Heuristics
{
    public static class HCollections
    {
        
        public static sbyte GetPreviousIndex(int lenghtOfArray, sbyte index)
        {
            index %= (sbyte) lenghtOfArray;
            index--;
            if (index < 0)
            {index = (sbyte) (lenghtOfArray - 1); }
            
            return index;
        }
        
        public static sbyte GetPreviousIndex(this ICollection array, sbyte index)
        {
            index %= (sbyte) array.Count;
            index--;
            if (index < 0)
            {index = (sbyte) (array.Count - 1); }
            
            return index;
        }
        
        public static sbyte GetNextIndex(int lenghtOfArray, sbyte index)
        {
            index %= (sbyte) lenghtOfArray;
            index++;
            if (index > (lenghtOfArray - 1))
            {index = 0;}
            
            return index;
        }
        
        public static sbyte GetNextIndex(this ICollection array, sbyte index)
        {
            index %= (sbyte) array.Count;
            index++;
            if (index > (array.Count - 1))
            {index = 0;}
            
            return index;
        }
        
        public static T GetNextItem<T>(this T[] array, sbyte index) where T : ICollection
        { return array[GetNextIndex(array.Length, index)]; }
        
        public static T GetPreviousItem<T>(this T[] array, sbyte index) where T : ICollection
        { return array[GetPreviousIndex(array.Length,index)]; }
        //todo: 1. Maybe get rid of all this useless shit
        //todo: 2. Maybe relocate here GetDropdownValues() from SettingsMenu.cs
    }
}