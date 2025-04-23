using System.Runtime.InteropServices;

namespace NewStringApp
{
    public sealed unsafe class NewString
    {
        private const int MIN_SIZE = 1;
        private char* buffer;
        private uint size;

        public NewString()
        {
            size = MIN_SIZE;
            buffer = (char*)Marshal.AllocHGlobal((int)size * sizeof(char));
            for (int i = 0; i < size; i++)
            {
                buffer[i] = '\0';
            }
        }
        public NewString(string str)
        {
            size = (uint)str.Length + 1; // +1 for null terminator
            buffer = (char*)Marshal.AllocHGlobal((int)size * sizeof(char));
            for (int i = 0; i < str.Length; i++)
            {
                buffer[i] = str[i];
            }
            buffer[str.Length] = '\0'; // null terminator
        }
        public NewString(char c)
        {
            size = 2; // +1 for null terminator
            buffer = (char*)Marshal.AllocHGlobal((int)size * sizeof(char));
            buffer[0] = c;
            buffer[1] = '\0'; // null terminator
        }
        public NewString(NewString newString)
        {
            size = newString.size;
            buffer = (char*)Marshal.AllocHGlobal((int)size * sizeof(char));
            for (int i = 0; i < size; i++)
            {
                buffer[i] = newString.buffer[i];
            }
        }
        public NewString(char[] buffer, int size)
        {
            this.size = (uint)size + 1; // +1 for null terminator
            this.buffer = (char*)Marshal.AllocHGlobal((int)this.size * sizeof(char));
            for (int i = 0; i < size; i++)
            {
                this.buffer[i] = buffer[i];
            }
            this.buffer[size] = '\0'; // null terminator
        }
        public uint Length()
        {
            return this.size - 1;
        }
        public static NewString operator +(NewString a, NewString b)
        {
            // Obliczamy nowy rozmiar (bez podwójnych null terminatorów, dodajemy tylko jeden na końcu)
            uint newSize = a.Length() + b.Length() + 1;
            NewString result = new NewString();
            result.size = newSize;
            result.buffer = (char*)Marshal.AllocHGlobal((int)newSize * sizeof(char));

            // Kopiujemy zawartość a
            for (int i = 0; i < a.Length(); i++)
            {
                result.buffer[i] = a.buffer[i];
            }

            // Doklejamy zawartość b
            for (int i = 0; i < b.Length(); i++)
            {
                result.buffer[a.Length() + i] = b.buffer[i];
            }

            // Null terminator
            result.buffer[newSize - 1] = '\0';

            return result;
        }
        public static NewString operator +(NewString a, string b)
        {
            return AddString(a, b);
        }
        public static NewString operator +(string a, NewString b)
        {
            return AddString(b, a);
        }
        public static NewString operator +(NewString a, char b)
        {
            return AddChar(a, b);
        }
        public static NewString operator +(char a, NewString b)
        {
            return AddChar(b, a);
        }
        public static NewString operator *(NewString a, int b)
        {
            return Mul(a, b);
        }
        public static NewString operator *(int a, NewString b)
        {
            return Mul(b, a);
        }
        public static NewString operator -(NewString a, NewString b)
        {
            // Obliczamy nowy rozmiar (bez podwójnych null terminatorów, dodajemy tylko jeden na końcu)
            uint newSize = a.Length() - b.Length() + 1; // +1 for null terminator
            NewString result = new NewString();
            result.size = newSize;
            result.buffer = (char*)Marshal.AllocHGlobal((int)newSize * sizeof(char));
            // Kopiujemy zawartość a
            for (int i = 0; i < a.Length() - b.Length(); i++)
            {
                result.buffer[i] = a.buffer[i];
            }
            int start = (int)(a.Length() - b.Length());
            for (int i = start; i < a.Length(); i++)
            {
                if (a.buffer[i] != b[i - start])
                {
                    throw new InvalidDataException("Nie można usunąć podłańcucha, ponieważ nie znajduje się na końcu łańcucha głównego");
                }
            }
            // Null terminator
            result.buffer[newSize - 1] = '\0';
            return result;
        }
        public static NewString operator -(NewString a, string b)
        {
            // Obliczamy nowy rozmiar (bez podwójnych null terminatorów, dodajemy tylko jeden na końcu)
            uint newSize = a.Length() - (uint)b.Length + 1; // +1 for null terminator
            NewString result = new NewString();
            result.size = newSize;
            result.buffer = (char*)Marshal.AllocHGlobal((int)newSize * sizeof(char));
            // Kopiujemy zawartość a
            for (int i = 0; i < a.Length() - b.Length; i++)
            {
                result.buffer[i] = a.buffer[i];

            }
            int start = (int)(a.Length() - b.Length);
            for (int i = start; i < a.Length(); i++)
            {
                if(a.buffer[i] != b[i - start])
                {
                    throw new InvalidDataException("Nie można usunąć podłańcucha, ponieważ nie znajduje się na końcu łańcucha głównego");
                }
            }

            // Null terminator
            result.buffer[newSize - 1] = '\0';
            return result;
        }
        private static NewString Mul(NewString a, int b)
        {
            // Obliczamy nowy rozmiar (bez podwójnych null terminatorów, dodajemy tylko jeden na końcu)
            uint newSize = a.Length() * (uint)b + 1; // +1 for null terminator
            NewString result = new NewString();
            result.size = newSize;
            result.buffer = (char*)Marshal.AllocHGlobal((int)newSize * sizeof(char));
            // Kopiujemy zawartość a
            for (int i = 0; i < b; i++)
            {
                for (int j = 0; j < a.Length(); j++)
                {
                    result.buffer[j+i*a.Length()] = a.buffer[j];
                }
            }
            // Null terminator
            result.buffer[newSize - 1] = '\0';
            return result;
        }
        private static NewString AddString(NewString a, string b)
        {
            // Obliczamy nowy rozmiar (bez podwójnych null terminatorów, dodajemy tylko jeden na końcu)
            uint newSize = a.Length() + (uint)b.Length + 1; // +1 for null terminator
            NewString result = new NewString();
            result.size = newSize;
            result.buffer = (char*)Marshal.AllocHGlobal((int)newSize * sizeof(char));
            // Kopiujemy zawartość a
            for (int i = 0; i < a.Length(); i++)
            {
                result.buffer[i] = a.buffer[i];
            }
            // Doklejamy zawartość b
            for (int i = 0; i < b.Length; i++)
            {
                result.buffer[a.Length() + i] = b[i];
            }
            // Null terminator
            result.buffer[newSize - 1] = '\0';
            return result;
        }
        private static NewString AddChar(NewString a, char b)
        {
            // Obliczamy nowy rozmiar (bez podwójnych null terminatorów, dodajemy tylko jeden na końcu)
            uint newSize = a.Length() + 2; // +1 for char +1 for null terminator
            NewString result = new NewString();
            result.size = newSize;
            result.buffer = (char*)Marshal.AllocHGlobal((int)newSize * sizeof(char));
            // Kopiujemy zawartość a
            for (int i = 0; i < a.Length(); i++)
            {
                result.buffer[i] = a.buffer[i];
            }
            // Doklejamy zawartość b
            result.buffer[a.Length()] = b;
            // Null terminator
            result.buffer[newSize - 1] = '\0';
            return result;
        }
        public void Insert(string newString, int index)
        {
            if (index < 0 || index > this.Length())
            {
                throw new IndexOutOfRangeException("Index out of range");
            }
            // Obliczamy nowy rozmiar (bez podwójnych null terminatorów, dodajemy tylko jeden na końcu)
            uint newSize = this.Length() + (uint)newString.Length + 1; // +1 for null terminator
            char* newBuffer = (char*)Marshal.AllocHGlobal((int)newSize * sizeof(char));
            // Kopiujemy zawartość obecnego obiektu do nowego bufora
            for (int i = 0; i < index; i++)
            {
                newBuffer[i] = this.buffer[i];
            }
            // Doklejamy zawartość nowego obiektu
            for (int i = 0; i < newString.Length; i++)
            {
                newBuffer[index + i] = newString[i];
            }
            // Doklejamy pozostałą część obecnego obiektu
            for (int i = index; i < this.Length(); i++)
            {
                newBuffer[newString.Length + i] = this.buffer[i];
            }
            // Null terminator
            newBuffer[newSize - 1] = '\0';
            // Zwalniamy stary bufor
            Marshal.FreeHGlobal((IntPtr)this.buffer);
            // Przypisujemy nowy bufor
            this.buffer = newBuffer;
            this.size = newSize;
        }

        public void Append(NewString newString)
        {
            Append(newString.ToString());
        }
        public void Append(char c)
        { 
            Append(c.ToString());
        }
        public void Append(string str)
        {
            // Obliczamy nowy rozmiar (bez podwójnych null terminatorów, dodajemy tylko jeden na końcu)
            uint newSize = this.Length() + (uint)str.Length + 1; // +1 for null terminator
            char* newBuffer = (char*)Marshal.AllocHGlobal((int)newSize * sizeof(char));
            // Kopiujemy zawartość obecnego obiektu
            for (int i = 0; i < this.Length(); i++)
            {
                newBuffer[i] = this.buffer[i];
            }
            // Doklejamy zawartość nowego obiektu
            for (int i = 0; i < str.Length; i++)
            {
                newBuffer[this.Length() + i] = str[i];
            }
            // Null terminator
            newBuffer[newSize - 1] = '\0';
            // Zwalniamy stary bufor
            Marshal.FreeHGlobal((IntPtr)this.buffer);
            // Przypisujemy nowy bufor
            this.buffer = newBuffer;
            this.size = newSize;
        }
        public void Prepend(NewString newString)
        {
            Prepend(newString.ToString());
        }
        public void Prepend(char c)
        {
            Prepend(c.ToString());
        }
        public void Prepend(string str)
        {
            // Obliczamy nowy rozmiar (bez podwójnych null terminatorów, dodajemy tylko jeden na końcu)
            uint newSize = this.Length() + (uint)str.Length + 1; // +1 for null terminator
            char* newBuffer = (char*)Marshal.AllocHGlobal((int)newSize * sizeof(char));
            // Kopiujemy zawartość nowego obiektu
            for (int i = 0; i < str.Length; i++)
            {
                newBuffer[i] = str[i];
            }
            // Doklejamy zawartość obecnego obiektu
            for (int i = 0; i < this.Length(); i++)
            {
                newBuffer[str.Length + i] = this.buffer[i];
            }
            // Null terminator
            newBuffer[newSize - 1] = '\0';
            // Zwalniamy stary bufor
            Marshal.FreeHGlobal((IntPtr)this.buffer);
            // Przypisujemy nowy bufor
            this.buffer = newBuffer;
            this.size = newSize;
        }
        public void Erase()
        {
            // Zwalniamy pamięć
            Marshal.FreeHGlobal((IntPtr)this.buffer);
            size = MIN_SIZE;
            buffer = (char*)Marshal.AllocHGlobal((int)size * sizeof(char));
            for (int i = 0; i < size; i++)
            {
                buffer[i] = '\0';
            }
        }
        public int? ConvertToInt()
        {
            int result = 0;
            for (int i = 0; i < this.Length(); i++)
            {
                if (buffer[i] < '0' || buffer[i] > '9')
                {
                    return null;
                }
                try
                {
                    checked
                    {
                        result = result * 10 + (int)(buffer[i] - '0');
                    }
                }
                catch (OverflowException)
                {
                    return null;
                }
            }
            return result;
        }
        public char[] ToCharArray()
        {
            char[] result = new char[this.Length()];
            for (int i = 0; i < this.Length(); i++)
            {
                result[i] = buffer[i];
            }
            return result;
        }
        public byte[] ToByteArray()
        {
            byte[] result = new byte[this.Length()];
            for (int i = 0; i < this.Length(); i++)
            {
                result[i] = (byte)buffer[i];
            }
            return result;
        }
        public String ToStringClass()
        {
            String s = new String(buffer);
            return s;
        }
        public void Print()
        {
            for (int i = 0; i < this.Length(); i++)
            {
                Console.Write(buffer[i]);
            }
        }
        public void PrintLine()
        {
            for (int i = 0; i < this.Length(); i++)
            {
                Console.Write(buffer[i]);
            }
            Console.Write('\n');
        }

        public char this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Length())
                {
                    throw new IndexOutOfRangeException("Index out of range");
                }
                return buffer[index];
            }
            set
            {
                if (index < 0 || index >= this.Length())
                {
                    throw new IndexOutOfRangeException("Index out of range");
                }
                buffer[index] = value;
            }
        }
        public static bool operator ==(NewString a, NewString b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(NewString a, NewString b)
        {
            return !a.Equals(b);
        }
        public static bool operator ==(NewString a, string b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(NewString a, string b)
        {
            return !a.Equals(b);
        }
        public static bool operator >(NewString a, NewString b)
        {
            return a.Compare(b) > 0;
        }
        public static bool operator <(NewString a, NewString b)
        {
            return a.Compare(b) < 0;
        }
        public static bool operator >=(NewString a, NewString b)
        {
            return a.Compare(b) >= 0;
        }
        public static bool operator <=(NewString a, NewString b)
        {
            return a.Compare(b) <= 0;
        }
        public static implicit operator string(NewString newString)
        {
            return newString.ToString();
        }
        public bool FindSubstring(string substring, out int startIndex, out int endIndex)
        {
            startIndex = -1;
            endIndex = -1;
            for (int i = 0; i < this.Length(); i++)
            {
                if (this[i] == substring[0])
                {
                    bool found = true;
                    for (int j = 0; j < substring.Length; j++)
                    {
                        if (i + j >= this.Length() || this[i + j] != substring[j])
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        startIndex = i;
                        endIndex = i + substring.Length - 1;
                        return true;
                    }
                }
            }
            return false;
        }
        public NewString GetSubstring(int startIndex, int endIndex)
        {
            if (startIndex < 0 || endIndex >= this.Length() || startIndex > endIndex)
            {
                throw new IndexOutOfRangeException("Index out of range");
            }
            char[] substring = new char[endIndex - startIndex + 1];
            for (int i = startIndex; i <= endIndex; i++)
            {
                substring[i - startIndex] = this[i];
            }
            return new NewString(substring, endIndex - startIndex);
        }
        public NewString Reverse()
        {
            char[] reversed = new char[this.Length()];
            for (int i = 0; i < this.Length(); i++)
            {
                reversed[i] = this[(int)this.Length() - 1 - i];
            }
            return new NewString(reversed, (int)this.Length());
        }
        public bool IsNullOrEmpty()
        {
            return this.Length() == 0;
        }
        public override string ToString()
        {
            string value = new string(buffer);
            return value;
        }
        public NewString Reduce(Func<char, char> func)
        {
            char[] reduced = new char[this.Length()];
            for (int i = 0; i < this.Length(); i++)
            {
                reduced[i] = func(this[i]);
            }
            return new NewString(reduced, (int)this.Length());
        }
        public NewString Execute(Func<string, string> func)
        {
            string value = new string(buffer);
            return new NewString(func(value));
        }
        public void Execute(Action<string> action)
        {
            string value = new string(buffer);
            action(value);
        }
        public override bool Equals(object? obj)
        {
            if (obj is string str)
            {
                return this.ToString() == str;
            }
            if (obj is NewString other)
            {
                if(this.Length() != other.Length())
                {
                    return false;
                }
                for (int i = 0; i < this.Length(); i++)
                {
                    if (this.buffer[i] != other.buffer[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        public int Compare(string str)
        {
            return this.ToString().CompareTo(str);
        }
        public int Compare(NewString newString)
        {
            return Compare(newString.ToString());
        }
        ~NewString()
        {
            // Free the allocated memory
            Marshal.FreeHGlobal((IntPtr)buffer);
        }
    }
}
