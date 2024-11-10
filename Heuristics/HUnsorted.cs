using System;

namespace Game.Meta.Heuristics
{
    using Random = UnityEngine.Random;
    
    public static class HUnsorted
    {
        /// <summary>
        /// Method to get bool state using UnityEngine.Random
        /// </summary>
        /// <param name="chance">(0f, 1f]. Don't use with 0f</param>
        /// <returns>Random.value > chance</returns>
        public static bool GetRandomBoolean(float chance) { return (Random.value > chance); } 
        
        public static T NewDataOfType<T>() where T : new() { return new T(); }

        /// <summary>
        /// Convert '0'-'9' chars into numeric type
        /// </summary>
        public static byte ToByte(this char @char)
        {
            try { return (byte) (@char - '0');}
            catch { throw new Exception("Error while doing .ToByte(char ch)"); }

        }

        /// <summary>
        /// Get number in specified order of magnitude
        /// </summary>
        /// <remarks><c> int a = 12345.GetNthDigit(1); // a = 5 </c></remarks>
        /// <remarks><code> int b = 12345.GetNthDigit(5); // b = 1 </code></remarks>
        public static int GetNthDigit(this int @int, byte n)
        {
            try { return (int) (@int / (Math.Pow(10,n-1)) % 10); }
            catch { throw new Exception("Error while doing .GetNthDigit(this int @int, byte n)"); }
        }
        
        /// <summary>
        /// Get number in specified order of magnitude
        /// </summary>
        /// <remarks><c> byte a = 123.GetNthDigit(1); // a = 3 </c></remarks>
        /// <remarks><code> byte b = 123.GetNthDigit(3); // b = 1 </code></remarks>
        public static byte GetNthDigit(this byte @byte, byte n)
        {
            try { return (byte) (@byte / (Math.Pow(10,n-1)) % 10); }
            catch { throw new Exception("Error while doing .GetNthDigit(this byte @int, byte n)"); }
        }
        
        /// <summary>
        /// Get number in specified order of magnitude
        /// </summary>
        /// <remarks><c> short a = 12345.GetNthDigit(1); // a = 5 </c></remarks>
        /// <remarks><code> short b = 12345.GetNthDigit(5); // b = 1 </code></remarks>
        public static short GetNthDigit(this short @short, byte n)
        {
            try { return (short) (@short / (Math.Pow(10,n-1)) % 10); }
            catch { throw new Exception("Error while doing .GetNthDigit(this short @short, byte n)"); }
        }

        /// <summary>
        /// Calculates the angles of a circle divided into a specified number of equal parts.
        /// </summary>
        /// <param name="startAngle">
        /// The starting angle of the division. Defaults to 0 degrees.
        /// </param>
        /// <param name="anglesAmount">
        /// The number of equal divisions (angles) to make in the circle. Must be positive. Defaults to 1.
        /// </param>
        /// <returns>
        /// A float array where each element represents an angle of the divided circle starting from the 
        /// "startAngle" and evenly spaced by 360 degrees divided by "anglesAmount".
        /// </returns>
        public static float[] GetAnglesOfDividedCircle(float startAngle = 0f, uint anglesAmount = 1)
        {
            if (anglesAmount == 0) { return Array.Empty<float>() ;}
            if (anglesAmount == 1) { return new float[1] {startAngle};}
            
            float[] temp = new float[anglesAmount];
            temp[0] = startAngle;
            
            for (int i = 1; i < anglesAmount; i++)
            {
                temp[i] = (temp[i - 1] + 360f/anglesAmount) % 360;
            }

            return temp;
        }
        
        /// <summary>
        /// Calculates the angles of a circle divided into a specified number of equal parts.
        /// </summary>
        /// <param name="array">
        /// Array that will be modified. First elemets of array mus represent the starting angle
        /// </param>
        /// <returns>
        /// Returns array where each element represents an angle of the divided circle starting from the 
        /// "startAngle" and evenly spaced by 360 degrees divided by "anglesAmount".
        /// </returns>
        public static float[] GetAnglesOfDividedCircle(this float[] array)
        {
            if (array.Length == 0) { return Array.Empty<float>() ;}
            if (array.Length == 1) { return array;}

            for (int i = 1; i < array.Length; i++)
            {
                array[i] = (array[i - 1] + 360f/array.Length) % 360;
            }

            return array;
        }

        public static bool TryConvertToBoolean(this string input, out bool result)
        {
            if (Boolean.TryParse(input, out result)) { return true; }
            
            if (input == "1") { result = true; }
            else if (input == "0") { result = false; }
            else { return false; }
            
            return true;
        }
        
        
        /*
        public static T DigitToNumericType<T>(this char @char) where T : struct, IComparable, IFormattable, IConvertible
        {
            try { return (dynamic) (@char - '0'); }
            catch { throw new Exception("Error while doing .GetNthDigit<T>(this T number, byte n)"); }
        }
        */
        
        /*
        public static T GetNthDigit<T>(int g, byte n) where T : struct, IComparable, IFormattable, IConvertible
        {
            try
            {
                double hello = g / (Math.Pow(10, n - 1) % 10);
                return hello;
            }
            catch (Exception e) { throw new Exception("Error while doing .GetNthDigit<T>(this T number, byte n) \n",e); }
        }
        */
        
    }

    /// <summary>
    /// Samples of text, useful for tests
    /// </summary>
    /// <returns>"xPar" - x number of paragraphs</returns>
    public static class LoremIpsum
    {
        /// <summary>
        /// 518 symbols, 77 words, 1 paragraph
        /// </summary>
        public const string OnePar =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec nunc nunc, interdum in vestibulum non, laoreet quis justo. Aliquam fringilla turpis et venenatis porttitor. Ut nec mi congue nisi maximus vulputate a non odio. Vestibulum ut iaculis nisi. Nunc a orci ultricies, condimentum justo in, malesuada nunc. Sed facilisis, urna sit amet molestie lacinia, libero erat venenatis quam, ac eleifend diam velit vitae tortor. Praesent tempus mauris sed ipsum lacinia tincidunt. Morbi porttitor non magna ac pellentesque.";

        /// <summary>
        /// 1454 symbols, 217 words, 2 paragraph
        /// </summary>
        public const string TwoPar = OnePar + "\n" +
                                               "Quisque gravida maximus felis vitae euismod. Aenean pulvinar, ipsum a imperdiet commodo, enim elit pellentesque odio, non mattis mauris massa non metus. Nullam a enim at velit efficitur placerat. Vivamus varius tellus neque, sit amet semper est condimentum quis. Donec sit amet ullamcorper nunc, eu accumsan sapien. Quisque in dui eget leo venenatis sollicitudin. Phasellus massa est, feugiat at dui eu, ornare tincidunt quam. Donec eu semper eros, nec convallis orci. Integer massa ante, elementum cursus diam eu, hendrerit tincidunt lorem. Proin laoreet sit amet nisi nec lobortis. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer sed gravida urna. Nunc maximus, urna ac ullamcorper fermentum, tellus ex mollis mi, id ultrices dui sapien quis dolor. Nullam tristique mattis aliquet. Nam gravida lacinia lorem, non sodales felis tincidunt ac. Nullam et fringilla elit, eget faucibus urna.";

        /// <summary>
        /// 2174 symbols, 322 words, 3 paragraph
        /// </summary>
        public const string ThreePar = TwoPar + "\n" +
                                                 "Quisque et fringilla eros. Nunc bibendum venenatis scelerisque. Aliquam tincidunt dui non molestie pharetra. Sed et massa eu velit finibus ornare eu ac lorem. Cras eu justo condimentum, ultrices velit eu, tristique orci. Nulla a condimentum tellus. Curabitur nec enim est. Aliquam enim felis, auctor at sagittis a, euismod id metus. Pellentesque risus turpis, semper pellentesque sollicitudin ut, aliquam a ante. Suspendisse interdum ut ante at blandit. Nunc dapibus feugiat massa, nec tincidunt lectus molestie nec. Maecenas eu mi sodales, suscipit lorem facilisis, iaculis sem. Aliquam lobortis lectus quis leo varius, at ornare tellus ullamcorper. Vivamus nibh ante, fringilla in libero id, congue condimentum magna.";

        /// <summary>
        /// 2910 symbols, 435 words, 4 paragraph
        /// </summary>
        public const string FourPar = TwoPar + "\n" + TwoPar;
        

    }

}
