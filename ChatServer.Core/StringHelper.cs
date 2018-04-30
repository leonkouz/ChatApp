using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatServer.Shared
{
    public class StringHelper
    {
        /// <summary>
        /// Trims a response from the server and splits into an array so the actual data can be used/read.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] TrimAndSplitTcpResponse(DataPrefix prefix, string str)
        {
            // Trim the prefix and delimiter from the start of the response
            string response = str.Remove(0, prefix.GetDescription().Length).TrimStart(Constants.Delimiter.ToCharArray());

            // Trim the end of file tag
            response = response.TrimEnd("<EOF>".ToCharArray());

            // Split the data into an array
            string[] responseArray = response.Split(Convert.ToChar(Constants.Delimiter));

            return responseArray;
        }
    }
}
