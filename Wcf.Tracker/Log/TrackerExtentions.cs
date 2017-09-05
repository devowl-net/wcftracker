using System.IO;
using System.ServiceModel.Channels;
using System.Xml;

namespace Wcf.Tracker.Log
{
    /// <summary>
    /// Contains extension methods required for info gathering.
    /// </summary>
    internal static class TrackerExtentions
    {
        /// <summary>
        /// Get human readable size of <see cref="Message"/>.
        /// </summary>
        /// <param name="message"><see cref="Message"/> reference.</param>
        /// <returns></returns>
        public static string GetHumanSize(this Message message)
        {
            var bufferedCopy = message.CreateBufferedCopy(int.MaxValue);
            var messageBytesSize = CalculateSize(bufferedCopy.CreateMessage());

            if (messageBytesSize == 0)
            {
                return string.Empty;
            }

            // https://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net
            string[] sizes = {
                "b",
                "Kb",
                "Mb",
                "Gb",
                "Tb"
            };

            int order = 0;
            var size = messageBytesSize;

            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size = size / 1024;
            }

            return $"{size:0.##} {sizes[order]}";
        }

        private static long CalculateSize(Message message)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = XmlDictionaryWriter.CreateBinaryWriter(ms))
                {
                    message.WriteMessage(writer);
                    return ms.Position;
                }
            }
        }
    }
}