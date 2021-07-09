namespace HomeWork_14_WPF
{
    public class TextMessage : IMessage
    {
        public TextMessage(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}
