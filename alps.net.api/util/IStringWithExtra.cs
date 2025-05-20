using alps.net.api.parsing;
using System;

namespace alps.net.api
{
    /// <summary>
    /// A language specific string is a string combined with a language specifier.
    /// </summary>
    public interface IStringWithExtra
    {
        /// <summary>
        /// Returns the content, the actual string
        /// </summary>
        /// <returns>the actual string</returns>
        public string getContent();

        /// <summary>
        /// Sets the content, the actual string
        /// </summary>
        /// <param name="content"></param>
        public void setContent(string content);

        /// <summary>
        /// Returns the extra that is specified in addition to the string.
        /// What this is depends on the concrete implementation.
        /// </summary>
        /// <returns>the extra information</returns>
        public string getExtra();

        /// <summary>
        /// Sets the extra that is specified in addition to the string.
        /// What this is depends on the concrete implementation.
        /// </summary>
        /// <param name="extra">the extra information</param>
        public void setExtra(string extra);


        /// <summary>
        /// Clones the current string with extra to get a new instance
        /// </summary>
        /// <returns>a clone of the current string</returns>
        IStringWithExtra clone();
    }
}