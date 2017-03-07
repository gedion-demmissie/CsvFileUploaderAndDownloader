# CsvFileUploaderAndDownloader
A simple web application that will accept the upload of a CSV file. The CSV file will be required as  attachement that represents what should be a tree like structure. The application consumes this CSV file , validate it (reports errors), and then persist it to a strongly typed storage table (SQL Server preferred) so that it may be downloaded at a later time.

In order to run the application do the following steps:
1. Clone the repository to your local
2. Run it in your visual studio (VS2015 )
3. Make sure you have SQl Server instance  installed and running in your machine
     3.1 . Modify the Connection String in the Web.Copnfig/ App.Config to point to your machine
4. Run the application in visual studio.Upload the CSV file, download the CSV file from the application, enjoy it.
5.Create a CSV file with the following structure.

e.g.
Parent,Child,Quantity
FPA,PPA,1
FPA,SAE,2
FPA,SAF,3
ICA,PPI,1
ICB,SAC,1
ICC,SAB,1
ICD,PPC,1
SAA,PPG,1
SAA,PPH,1
SAB,PPE,1
SAB,PPF,1
SAB,SAA,1
SAC,ICA,1
SAC,PPJ,1
SAD,ICC,1
SAD,PPD,2
SAD,FPA,3
SAE,ICD,1
SAE,PPB,1
SAF,ICB,1
RPB,FPA,1
