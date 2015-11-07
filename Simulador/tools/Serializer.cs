/*
 * Copyright (c) 2011 Xavier Borrell i Gómez
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Xb.Simulador.Tools
{
    public sealed class Serializer
    {
        #region Interface
        /// <summary>
        /// Serialize an object into an XML string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj)
        {
            return Serialize<T>(obj, Encoding.UTF8);
        }

        /// <summary>
        /// Serialize an object into an XML string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj, Encoding encoding)
        {
            XmlSerializerFactory factory = new XmlSerializerFactory();
            // Obtenemos el serializer        
            XmlSerializer serializer = factory.CreateSerializer(obj.GetType());

            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, encoding);
            serializer.Serialize(xmlTextWriter, obj);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;

            return ByteArrayToString(memoryStream.ToArray(), encoding);
        }

        /// <summary>
        /// Reconstruct an object from an XML string
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml)
        {
            return Deserialize<T>(xml, Encoding.UTF8);
        }

        /// <summary>
        /// Reconstruct an object from an XML string
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml, Encoding encoding)
        {
            XmlSerializerFactory factory = new XmlSerializerFactory();
            XmlSerializer serializer = factory.CreateSerializer(typeof(T));

            MemoryStream memoryStream = new MemoryStream(StringToByteArray(xml, encoding));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, encoding);
            return (T)serializer.Deserialize(memoryStream);
        }
        #endregion

        #region Implementation
        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        private static string ByteArrayToString(byte[] characters, Encoding encoding)
        {
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        private static Byte[] StringToByteArray(string pXmlString, Encoding encoding)
        {
            byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
        #endregion
    }
}
