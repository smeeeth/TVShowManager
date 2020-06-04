using System;
using System.Collections.Generic;
using System.Text;

namespace APIAccessor.API
{
    public static class JSONInterpreter
    {
        public static string GetArrayJSON(String name, String JSON)
        {
            return GetObjectJSON(name, JSON, '[', ']');
        }

        public static string GetArrayJSON(String name, String JSON, out int startIndex, out int endIndex)
        {
            return GetObjectJSON(name, JSON, out startIndex, out endIndex, '[', ']');
        }

        public static string GetObjectJSON(String name, String JSON, char open = '{', char close = '}')
        {
            return GetObjectJSON(name, JSON, out int start, out int end, open, close);
        }

        //returns first object with specified name
        public static string GetObjectJSON(String name, String JSON, out int startIndex, out int endIndex, char open = '{', char close = '}')
        {
            startIndex = JSON.IndexOf(name);

            int index = JSON.IndexOf(open, startIndex);

            var thisObject = GetFirstObjectFrom(JSON, index, open, close);

            endIndex = JSON.IndexOf(thisObject) + thisObject.Length;

            return thisObject;
        }

        public static string GetFirstObjectFrom(String JSON, int startIndex, char open = '{', char close = '}')
        {
            string thisObject = "";

            List<char> brackets = new List<char>();

            Char[] JSONArray = JSON.ToCharArray();

            bool foundBracket = false;

            for (int i = startIndex; i < JSONArray.Length && (foundBracket == false || brackets.Count > 0); i++)
            {
                thisObject += JSONArray[i];
                if (JSONArray[i] == open)
                {
                    foundBracket = true;
                    brackets.Add(JSONArray[i]);
                }
                else if (JSONArray[i] == close)
                {
                    brackets.RemoveAt(0);
                }
            }
            return thisObject;
        }

        public static String GetValue(String name, String JSON)
        {
            return GetValue(name, JSON, out int start, out int end);
        }

        public static String GetValue(String name, String JSON, out int startIndex, out int endIndex)
        {
            if (!JSON.Contains(name)) //null check, in case value is not found
            {
                startIndex = 0;
                endIndex = 0;
                return null;
            }

            name = $"\"{name}\"";
            startIndex = JSON.IndexOf(name);

            int index = JSON.IndexOf(":", startIndex) + 1;

            string thisValue = "";

            Char[] JSONArray = JSON.ToCharArray();

            for (int i = index; i < JSONArray.Length && JSONArray[i] != ',' ;i++)
            {
                thisValue += JSONArray[i];
            }

            endIndex = JSON.IndexOf(thisValue) + thisValue.Length;

            return Clean(thisValue);

        }

        public static string Clean(string value)
        {
            string startChars = "\"{[ ";
            var valueArray = value.ToCharArray();
            var startCharsArray = startChars.ToCharArray();
            for (int i = 0; i < value.Length; i++)
            {
                if (startChars.Contains(value[i]))
                {
                    value = value.Remove(i--, 1);
                }
                else
                {
                    break;
                }
            }


            string endChars = "}] \"";
            for (int i = value.Length - 1; i > 0; i--)
            {
                if (endChars.Contains(value[i]))
                {
                    value = value.Remove(i, 1);
                }
                else
                {
                    break;
                }
            }

            return value;
        }

        public static string RemoveArrayBrackets(string JSONArray)
        {
            string startChars = "[";
            for (int i = 0; i < JSONArray.Length; i++)
            {
                if (startChars.Contains(JSONArray[i]))
                {
                    JSONArray = JSONArray.Remove(i, 1);
                }
                else
                {
                    break;
                }
            }


            string endChars = "]";
            for (int i = JSONArray.Length - 1; i > 0; i--)
            {
                if (endChars.Contains(JSONArray[i]))
                {
                    JSONArray = JSONArray.Remove(i, 1);
                }
                else
                {
                    break;
                }
            }

            return JSONArray;
        }


    }
}
