using System.Text;

namespace NDTV.Utilities
{
    /// <summary>
    /// Helper class which can decode urls.
    /// This is same as HttpUtility in System.Web
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// Url(Link) decoder
        /// Decodes the url string
        /// </summary>
        /// <param name="message">string</param>
        /// <returns>string</returns>
        public static string LinkDecode(string message)
        {
            if (message == null)
            {
                return null;
            }
            return LinkDecode(message, Encoding.UTF8);
        }

        /// <summary>
        /// Decodes the Url string, using encoding
        /// </summary>
        /// <param name="message">string</param>
        /// <param name="encode">encoding</param>
        /// <returns>string</returns>
        public static string LinkDecode(string message, Encoding encode)
        {
            if (message == null)
            {
                return null;
            }
            return LinkDecodeStringFromStringInternal(message, encode);
        }

        /// <summary>
        /// Decodes Url(Link) string using Url decoder
        /// </summary>
        /// <param name="input">string</param>
        /// <param name="encode">encoding</param>
        /// <returns>string</returns>
        private static string LinkDecodeStringFromStringInternal(string input, Encoding encode)
        {
            int length = input.Length;
            LinkDecoder decoder = new LinkDecoder(length, encode);
            for (int i = 0; i < length; i++)
            {
                char ch = input[i];
                if (ch == '+')
                {
                    ch = ' ';
                }
                else if ((ch == '%') && (i < (length - 2)))
                {
                    if ((input[i + 1] == 'u') && (i < (length - 5)))
                    {
                        int num3 = HexToInt(input[i + 2]);
                        int num4 = HexToInt(input[i + 3]);
                        int num5 = HexToInt(input[i + 4]);
                        int num6 = HexToInt(input[i + 5]);
                        if (((num3 < 0) || (num4 < 0)) || ((num5 < 0) || (num6 < 0)))
                        {
                            goto Label_0106;
                        }
                        ch = (char)((((num3 << 12) | (num4 << 8)) | (num5 << 4)) | num6);
                        i += 5;
                        decoder.AddChar(ch);
                        continue;
                    }
                    int num7 = HexToInt(input[i + 1]);
                    int num8 = HexToInt(input[i + 2]);
                    if ((num7 >= 0) && (num8 >= 0))
                    {
                        byte b = (byte)((num7 << 4) | num8);
                        i += 2;
                        decoder.AddByte(b);
                        continue;
                    }
                }
            Label_0106:
                if ((ch & 0xff80) == 0)
                {
                    decoder.AddByte((byte)ch);
                }
                else
                {
                    decoder.AddChar(ch);
                }
            }
            return decoder.GetString();
        }

        /// <summary>
        /// Convert Hex to int
        /// </summary>
        /// <param name="input">char</param>
        /// <returns>int</returns>
        private static int HexToInt(char input)
        {
            if ((input >= '0') && (input <= '9'))
            {
                return (input - '0');
            }
            if ((input >= 'a') && (input <= 'f'))
            {
                return ((input - 'a') + 10);
            }
            if ((input >= 'A') && (input <= 'F'))
            {
                return ((input - 'A') + 10);
            }
            return -1;
        }

        /// <summary>
        /// Class Url(Link) Decoder
        /// </summary>
        private class LinkDecoder
        {
            private int bufferSize;
            private byte[] byteBuffer;
            private char[] charBuffer;
            private Encoding encoding;
            private int numBytes;
            private int numChars;

            /// <summary>
            /// Url(Link) decoder
            /// </summary>
            /// <param name="bufferSize">int</param>
            /// <param name="encoding">Encoding</param>
            internal LinkDecoder(int bufferSize, Encoding encoding)
            {
                this.bufferSize = bufferSize;
                this.encoding = encoding;
                this.charBuffer = new char[bufferSize];
            }

            /// <summary>
            /// Add byte
            /// </summary>
            /// <param name="input">byte</param>
            internal void AddByte(byte input)
            {
                if (this.byteBuffer == null)
                {
                    this.byteBuffer = new byte[this.bufferSize];
                }
                this.byteBuffer[this.numBytes++] = input;
            }

            /// <summary>
            /// Add character
            /// </summary>
            /// <param name="input">char</param>
            internal void AddChar(char input)
            {
                if (this.numBytes > 0)
                {
                    this.FlushBytes();
                }
                this.charBuffer[this.numChars++] = input;
            }

            /// <summary>
            /// Flush bytes
            /// </summary>
            private void FlushBytes()
            {
                if (this.numBytes > 0)
                {
                    this.numChars += this.encoding.GetChars(this.byteBuffer, 0, this.numBytes, this.charBuffer, this.numChars);
                    this.numBytes = 0;
                }
            }

            /// <summary>
            /// Get String
            /// </summary>
            /// <returns>string</returns>
            internal string GetString()
            {
                if (this.numBytes > 0)
                {
                    this.FlushBytes();
                }
                if (this.numChars > 0)
                {
                    return new string(this.charBuffer, 0, this.numChars);
                }
                return string.Empty;
            }
        }
    }
}
