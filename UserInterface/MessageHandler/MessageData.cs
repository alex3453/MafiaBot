using CommonInteraction;

namespace UserInterface
{
    public class MessageData
    {
        public readonly Service Service;
        public readonly string Content;
        public readonly Author Author;
        public readonly string[] MentionedUsers;
        public readonly bool IsCommonChannel;
        public readonly ulong CommonChannelId;

        public MessageData(
            string content, 
            Author author, 
            string[] mentionedUsers, 
            bool isCommonChannel, 
            ulong commonChannelId, 
            Service service)
        {
            Content = content;
            Author = author;
            MentionedUsers = mentionedUsers;
            IsCommonChannel = isCommonChannel;
            CommonChannelId = commonChannelId;
            Service = service;
        }
    }

    public class Author
    {
        public readonly bool IsBot;
        public readonly string Username;
        public readonly ulong Id;

        public Author(bool isBot, string username, ulong id)
        {
            IsBot = isBot;
            Username = username;
            Id = id;
        }
    }
}