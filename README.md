# SQL INSERT INTO Cleaner (sqlicc)
An utility that removes INSERT INTO clausules from SQL dumps files. Useful for removing data from a database dump and preserving structure.

# Usage
Run the utility on command-line shell, indicating input and output files. If output file does't exist, the utility will try to create it.

Linux and macOS:
```bash
$ ./sqlicc input.sql output.sql
``` 

Windows:
```pwsh
PS > ./sqlicc.exe input.sql output.sql
```

# Remarks
This utility copies the file line by line and ignores lines beginning with "INSERT INTO". The copy is done line by line, so the entire input file is never loaded into memory. This allows a dump with millions of records to be reduced to just the structure.

We tested with a full 20 GB database dump, generating a 100 KB file with just the structure.
