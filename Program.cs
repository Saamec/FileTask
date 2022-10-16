
Console.WriteLine("Введите путь до нужной дирректории, где нужно очистить файлы,");
Console.WriteLine("которые не использовались в течение 30 минут.");

int file = 0;
int dir = 0;
long size = 0;
long sizeZero = 0;
long osvobod = 0;

string rootDir = Console.ReadLine();


Walk(new DirectoryInfo(rootDir), ref file, ref dir, ref size);

Console.WriteLine("Файлов всего: " + file);
Console.WriteLine("Папок всего: " + dir);
Console.WriteLine("Исходный размер дирректории: " + size);

DirectoryInfo directoryInfo = new DirectoryInfo(rootDir);
if (directoryInfo.Exists && directoryInfo.GetFiles().Length == 0) Deleter(directoryInfo);
if (directoryInfo.Exists) sizeZero = directoryInfo.GetFiles().Length;
osvobod = size - sizeZero;
Console.WriteLine("Освобождено: " + osvobod);
Console.WriteLine("Текущий размер директории: " + sizeZero);




static void Walk(DirectoryInfo root, ref int file, ref int dir, ref long size)
{
    FileInfo[] files = null;
    DirectoryInfo[] subDirs = null;

    DateTime acsesFiles;
    try
    {
        files = root.GetFiles("*.*");
    }
    catch (UnauthorizedAccessException e)
    {
        Console.WriteLine("Нет прав доступа к файлу(ам)");
    }
    catch (DirectoryNotFoundException e)
    {
        Console.WriteLine("Директория не существует");
    }
    if (files != null)
    {
        foreach (FileInfo fi in files)
        {
            size += fi.Length;
            acsesFiles = fi.LastAccessTime;
            if (DateTime.Now - acsesFiles > TimeSpan.FromMinutes(1)) fi.Delete();
        }
        subDirs = root.GetDirectories();

        foreach (DirectoryInfo dirInfo in subDirs)
        {
            Walk(dirInfo, ref file, ref dir, ref size);
        }
    }

    if (files != null) file += files.Length;
    if (subDirs != null) dir += subDirs.Length;

}

static void Deleter(DirectoryInfo root)
{
    DirectoryInfo[] subDirs = null;
    subDirs = root.GetDirectories();
    foreach (DirectoryInfo dirInfo in subDirs)
    {
        Deleter(dirInfo);
        dirInfo.Delete();

    }
}

