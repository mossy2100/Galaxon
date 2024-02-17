for (int i = 0; i < 128; i++)
{
    char c = (char)i;
    if (!char.IsControl(c) && !char.IsLetterOrDigit(c))
        Console.Write(c);
}
