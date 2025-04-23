using System.Threading.Channels;

namespace NewStringApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NewString str = new NewString("hello world ");
            NewString str2 = new NewString("hello world");
            NewString newString = str + str2;
            newString = newString + " 0 " + '2' + ' ';
            newString.Append(new NewString("1234"));
            Console.WriteLine(newString);
            Console.WriteLine((3*newString));
            newString.Erase();
            Console.WriteLine(newString);
            newString.Append("99999");
            int? v = newString.ConvertToInt();
            Console.WriteLine(v);
            newString.Prepend("000");
            Console.WriteLine(newString);

            newString -= "999";
            Console.WriteLine(newString);

            newString.Insert("0000", 4);

            newString.PrintLine();

            NewString subString = newString.GetSubstring(0, 4);
            Console.WriteLine(subString);

            subString.Reverse().PrintLine();

            newString.Execute(x =>
            {
                Console.WriteLine(x+"JANEK");
                return x;
            });

            newString.Execute(x =>
            {
                Console.WriteLine(x + "JANEK");
            });


        }
    }
}
